using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace SmashTheCode
{
    public class Player
    {
        public static void Main(string[] args)
        {
            var game = new Game();

            while (true)
            {
                Console.WriteLine(game.ResolveTurn());
            }
        }
    }

    public class Game
    {
        private readonly IConsole _console;

        public Game() : this(new SystemConsole())
        {
            
        }

        public Game(IConsole console)
        {
            _console = console;
        }

        public int ResolveTurn()
        {
            NextColors = new GameColor[8];

            for (int i = 0; i < 8; i++)
            {
                string[] inputs = _console.ReadLine().Split(' ');

                var color = new GameColor
                {
                    First = int.Parse(inputs[0]),
                    Second = int.Parse(inputs[1])
                };

                NextColors[i] = color;
            }

            _console.Debug("Current color: " + NextColors[0].First);

            var columnHeights = new int[6];
            var topColors = new int[6];
            for (int i = 0; i < 6; i++) topColors[i] = -1;

            for (int i = 0; i < 12; i++)
            {
                string row = _console.ReadLine();

                for (int j = 0; j < 6; j++)
                {
                    if (topColors[j] == -1 && row[j] != '.')
                        topColors[j] = int.Parse(row[j].ToString());
                    if (row[j] != '.')
                        columnHeights[j]++;
                }
            }

            for (int i = 0; i < 12; i++)
            {
                // One line of the map ('.' = empty, '0' = skull block, '1' to '5' = colored block)
                string row = _console.ReadLine();
            }

            var min = 999;
            var minColumn = -1;

            var minSameColor = 999;
            var sameColorColumn = -1;

            _console.Debug("Top colors: " + String.Join(" ", topColors));

            for (int i = 0; i < 6; i++)
            {
                if (topColors[i] == NextColors[0].First && columnHeights[i] < minSameColor)
                {
                    minSameColor = columnHeights[i];
                    sameColorColumn = i;
                }

                if (columnHeights[i] < min)
                {
                    min = columnHeights[i];
                    minColumn = i;
                }
            }

            _console.Debug("Column (s): " + sameColorColumn);
            _console.Debug("Column: " + minColumn);

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            // "x": the column in which to drop your blocks
            return sameColorColumn != -1 ? sameColorColumn : minColumn;
        }

        public GameColor[] NextColors { get; private set; }
    }

    public struct GameColor
    {
        public int First { get; set; }
        public int Second { get; set; }
    }

    public interface IConsole
    {
        string ReadLine();

        void Debug(string message);
    }

    public class SystemConsole : IConsole
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Debug(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}