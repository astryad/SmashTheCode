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
        private const int BoardHeight = 12;
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
            ReadNextTurns();

            _console.Debug("Next turn color: " + NextTurns[0].First);

            ReadPlayerBoard();
            ReadOpponentBoard();

            var min = 999;
            var minColumn = -1;

            var minSameColor = 999;
            var sameColorColumn = -1;

            var columnHeights = new int[6];
            var topColors = new int[6];

            for (var i = 0; i < 6; i++)
                topColors[i] = -1;

            for (var i = 0; i < BoardHeight; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    if (topColors[j] == -1 && PlayerBoard[i][j] != '.')
                        topColors[j] = int.Parse(PlayerBoard[i][j].ToString());
                    if (PlayerBoard[i][j] != '.')
                        columnHeights[j]++;
                }
            }

            _console.Debug("Top colors: " + String.Join(" ", topColors));

            for (var i = 0; i < 6; i++)
            {
                if (topColors[i] == NextTurns[0].First && columnHeights[i] < minSameColor)
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

            // "x": the column in which to drop your blocks
            return sameColorColumn != -1 ? sameColorColumn : minColumn;
        }

        private void ReadPlayerBoard()
        {
            PlayerBoard = new string[BoardHeight];

            for (var i = 0; i < BoardHeight; i++)
            {
                PlayerBoard[i] = _console.ReadLine();
            }
        }

        private void ReadOpponentBoard()
        {
            OpponentBoard = new string[BoardHeight];

            for (var i = 0; i < BoardHeight; i++)
            {
                // One line of the map ('.' = empty, '0' = skull block, '1' to '5' = colored block)
                OpponentBoard[i] = _console.ReadLine(); 
            }
        }

        private void ReadNextTurns()
        {
            NextTurns = new TurnBlocks[8];

            for (int i = 0; i < 8; i++)
            {
                string[] inputs = _console.ReadLine().Split(' ');

                var color = new TurnBlocks
                {
                    First = int.Parse(inputs[0]),
                    Second = int.Parse(inputs[1])
                };

                NextTurns[i] = color;
            }
        }

        public TurnBlocks[] NextTurns { get; private set; }
        public string[] PlayerBoard { get; set; }
        public string[] OpponentBoard { get; set; }
    }

    public struct TurnBlocks
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