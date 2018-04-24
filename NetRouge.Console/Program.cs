using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetRogue.Core;

namespace NetRogue.CMD {
    class Program {
        static void Main(string[] args) {
            bool running = true;
            World world = new World();
            Screen screen = new Screen(Console.WindowWidth, Console.WindowHeight);
            InputMap input = new InputMap();
            Console.CursorVisible = false;

            Console.CancelKeyPress += (sender, arg) => {
                running = false;
            };
            screen.Draw(world);
            screen.Display();
            while (running) {
                if (world.IsPlayerTurn() || !world.Player.IsAlive) {
                    (int x, int y) = world.Player.Position;
                    Console.SetCursorPosition(x, y);
                    var key = Console.ReadKey(true);
                    //ActorAction action = input.Parse(key);
                    //world.SetPlayerAction(action);
                    switch (key.Key) {
                        // Movement
                        case ConsoleKey.K:
                        case ConsoleKey.UpArrow:
                            world.SetPlayerMove(Direction.Up);
                            break;
                        case ConsoleKey.J:
                        case ConsoleKey.DownArrow:
                            world.SetPlayerMove(Direction.Down);
                            break;
                        case ConsoleKey.H:
                        case ConsoleKey.LeftArrow:
                            world.SetPlayerMove(Direction.Left);
                            break;
                        case ConsoleKey.L:
                        case ConsoleKey.RightArrow:
                            world.SetPlayerMove(Direction.Right);
                            break;
                        case ConsoleKey.Spacebar:
                            world.SetPlayerAction(new WaitAction());
                            break;
                        case ConsoleKey.Escape:
                            running = false;
                            break;
                        default:
                            break;
                    }
                }
                world.Tick();
                screen.Resize(Console.WindowWidth, Console.WindowHeight);
                screen.Draw(world);
                screen.Display();
            }
        }
    }
}
