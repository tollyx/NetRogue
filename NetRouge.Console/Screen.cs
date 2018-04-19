﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using NetRogue.Core;

namespace NetRogue.CMD {
    internal class Screen {
        Glyph[] glyphs;

        int width, height;

        Glyph[] previous;
        Glyph[] screen;
        
        bool dirty = true;

        public Screen(int windowWidth, int windowHeight) {
            glyphs = new Glyph[Enum.GetValues(typeof(Tile)).Length];
            glyphs[(int)Tile.None] = new Glyph {
                character = ' ',
                fore = ConsoleColor.White,
                back = ConsoleColor.Black,
            };
            glyphs[(int)Tile.Wall] = new Glyph {
                character = '#',
                fore = ConsoleColor.Black,
                back = ConsoleColor.Gray,
            };
            glyphs[(int)Tile.Floor] = new Glyph {
                character = '.',
                fore = ConsoleColor.DarkGray,
                back = ConsoleColor.Black,
            };
            glyphs[(int)Tile.Player] = new Glyph {
                character = '@',
                fore = ConsoleColor.DarkCyan,
                back = ConsoleColor.Black,
            };
            glyphs[(int)Tile.Goblin] = new Glyph {
                character = 'g',
                fore = ConsoleColor.Green,
                back = ConsoleColor.Black,
            };
            if (glyphs.Length < Enum.GetValues(typeof(Tile)).Length) {
                // Safety measure to make sure I don't forget to add glyphs for new tiles.
                // I would've loved to make this a compile-time check.
                throw new ApplicationException("Too few glyphs are defined! Tell tollyx to go punch himself!");
            }
            screen = new Glyph[windowWidth * windowHeight];
            previous = new Glyph[windowWidth * windowHeight];
            width = windowWidth;
            height = windowHeight;

            Console.SetBufferSize(width + Console.WindowLeft, height + Console.WindowTop);
        }

        int ToIndex(Point p) {
            return ToIndex(p.x, p.y);
        }

        int ToIndex(int x, int y) {
            return x % width + y * width;
        }

        bool IsInsideBounds(Point pos) {
            return IsInsideBounds(pos.x, pos.y);
        }

        bool IsInsideBounds(int x, int y) {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        internal void Draw(World world) {
            Clear();

            Rect mapArea = new Rect(8, 0, width-8, height-4);
            Point offset = new Point(0, 0);

            Draw(world.Level, offset.x, offset.y, mapArea);

            foreach (var act in world.Actors) {
                var screenpos = act.Position - offset + mapArea.TopLeft;
                if (mapArea.Contains(screenpos)) {
                    screen[ToIndex(screenpos)] = glyphs[(int)act.Glyph];
                }
            }

            Draw($"HP:{world.Player.Health}", 1, 1);
            Draw($"STR:{world.Player.Strength}", 1, 2);
            Draw($"DEF:{world.Player.Defence}", 1, 3);

            CalcDirty();
        }

        void CalcDirty() {
            dirty = false;
            for (int i = 0; !dirty && i < screen.Length; i++) {
                dirty = screen[i] != previous[i];
            }
        }

        void Draw(string text, int x, int y) {
            for (int i = 0; i < text.Length; i++) {
                if (IsInsideBounds(x + i, y)) {
                    screen[ToIndex(x + i, y)] = new Glyph(text[i]);
                }
            }
        }

        void Draw(Level level, int offx, int offy, Rect screenspace) {
            Draw(level, offx, offy, screenspace.Left, screenspace.Top, screenspace.Right, screenspace.Bottom);
        }

        void Draw(Level level) {
            Draw(level, 0, 0, 0, 0, width, height);
        }

        void Draw(Level level, int offx, int offy) {
            Draw(level, offx, offy, 0, 0, width, height);
        }

        void Draw(Level level, int offx, int offy, int left, int top, int right, int bottom) {
            for (int y = top; y < bottom; y++) {
                for (int x = left; x < right; x++) {
                    screen[ToIndex(x, y)] = glyphs[(int)level.GetTile(offx + x - left, offy + y - top)];
                }
            }
        }

        void Clear() {
            screen = new Glyph[screen.Length];
        }

        internal void Display() {
            var currentTile = glyphs[(int)Tile.None];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (x >= width - 1 && y >= height - 1) {
                        break; // Skip the final character so we won't scroll down a line
                    }
                    var tile = screen[ToIndex(x,y)];
                    var prev = previous[ToIndex(x, y)];
                    if (prev != tile) {
                        Console.SetCursorPosition(x, y);
                        if (tile != currentTile) {
                            Console.ForegroundColor = tile.fore;
                            Console.BackgroundColor = tile.back;
                            currentTile = tile;
                        }
                        Console.Write(tile.character);
                    }
                }
            }
            screen.CopyTo(previous, 0);
            dirty = false;
        }

        internal void Resize(int windowWidth, int windowHeight) {
            if (windowWidth != width || windowHeight != height) {
                screen = new Glyph[windowWidth * windowHeight];
                previous = new Glyph[windowWidth * windowHeight];
                width = windowWidth;
                height = windowHeight;
                Console.ResetColor();
                Console.Clear();
                Console.SetBufferSize(width + Console.WindowLeft, height + Console.WindowTop);
                Console.CursorVisible = false;
                dirty = true;
            }
        }
    }
}