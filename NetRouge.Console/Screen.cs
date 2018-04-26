using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using NetRogue.Core;

namespace NetRogue.CMD {
    internal class Screen {
        Glyph[] glyphs;

        int width, height;

        Glyph[] previous;
        Glyph[] screen;

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
            glyphs[(int)Tile.Coin] = new Glyph {
                character = '$',
                fore = ConsoleColor.Yellow,
                back = ConsoleColor.Black,
            };

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

            Rect mapArea = new Rect(6, 0, world.Level.Map.Width+2, world.Level.Map.Height+2);
            Point offset = new Point(0, 0);

            Draw(world.Level, offset.x, offset.y, mapArea);

            foreach (var act in world.Actors) {
                var screenpos = act.Position - offset + mapArea.TopLeft;
                if (mapArea.Contains(screenpos)) {
                    screen[ToIndex(screenpos)] = glyphs[(int)act.Glyph];
                }
            }

            Draw(world.Player.Name, 0, 0);
            Draw($"HP:{world.Player.Health}", 0, 1);
            Draw($"STR:{world.Player.Strength}", 0, 2);
            Draw($"DEF:{world.Player.Defence}", 0, 3);

            var msg = world.Log.GetLast(height - mapArea.Bottom).Reverse();
            var y = height-1;
            foreach (var item in msg) {
                Draw(item, 0, y--);
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
            right = Math.Min(width, right);
            left = Math.Max(0, left);
            bottom = Math.Min(height, bottom);
            top = Math.Max(0, top);
            for (int y = top; y < bottom; y++) {
                for (int x = left; x < right; x++) {
                    screen[ToIndex(x, y)] = glyphs[(int)level.Map.GetTile(offx + x - left, offy + y - top)];
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
            }
        }
    }
}