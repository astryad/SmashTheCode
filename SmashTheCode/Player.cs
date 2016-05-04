using System;
using System.Collections.Generic;
using System.Linq;

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

            _console.Debug("Next turn color: " + NextTurns[0].Top);

            ReadPlayerBoard();
            ReadOpponentBoard();

            var scoreCalculator = new ScoreCalculator();
            var columnScore = Enumerable.Range(0, 6).Select(
                i => new
                {
                    Column = i,
                    Score = scoreCalculator.EvaluateScore(PlayerBoard, i, NextTurns)
                });

            return columnScore.OrderByDescending(c => c.Score).First().Column;
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
                    Top = inputs[0][0],
                    Bottom = inputs[1][0]
                };

                NextTurns[i] = color;
            }
        }

        public TurnBlocks[] NextTurns { get; private set; }
        public string[] PlayerBoard { get; set; }
        public string[] OpponentBoard { get; set; }
    }

    public class ScoreCalculator
    {
        private const char EmptyBlock = '.';

        public int EvaluateScore(string[] board, int column, params TurnBlocks[] nextTurns)
        {
            var score = 0;

            var columnTop = CalculateColumnTop(board, column);

            if (columnTop <= 0)
                return 0;

            if (columnTop < 11 && board[columnTop + 1][column] == nextTurns[0].Bottom)
                score += 10;
            if (columnTop < 10 && board[columnTop + 2][column] == nextTurns[0].Bottom)
                score += 10;

            score += CalculateEmptyScore(board, column - 1, columnTop, nextTurns[0]);
            score += CalculateEmptyScore2(board, column - 1, columnTop, nextTurns[0]);
            score += CalculateEmptyScore(board, column - 2, columnTop, nextTurns[0]);
            score += CalculateEmptyScore(board, column + 1, columnTop, nextTurns[0]);
            score += CalculateEmptyScore2(board, column + 1, columnTop, nextTurns[0]);
            score += CalculateEmptyScore(board, column + 2, columnTop, nextTurns[0]);

            if (column < 4 && nextTurns.Length > 1 && nextTurns[0].Top == nextTurns[1].Top)
            {
                if (board[columnTop][column + 2] == nextTurns[0].Bottom && board[columnTop][column + 1] == EmptyBlock)
                {
                    score += 20;
                }
                if (board[columnTop - 1][column + 2] == nextTurns[0].Top && board[columnTop - 1][column + 1] == EmptyBlock)
                {
                    score += 20;
                }
            }
            if (column > 1 && nextTurns.Length > 1 && nextTurns[0].Top == nextTurns[1].Top)
            {
                if (board[columnTop][column - 2] == nextTurns[0].Bottom && board[columnTop][column - 1] == EmptyBlock)
                {
                    score += 20;
                }
                if (board[columnTop - 1][column - 2] == nextTurns[0].Top && board[columnTop - 1][column - 1] == EmptyBlock)
                {
                    score += 20;
                }
            }

            return score;
        }

        private static int CalculateEmptyScore(string[] board, int column, int columnTop, TurnBlocks turn)
        {
            if (IsOutOfBoard(column))
                return 0;

            if (HasEmptyBlockUnder(board, column, columnTop))
                return 0;

            var score = 0;

            score += GetScore(board, column, columnTop - 1, turn.Top);
            score += GetScore(board, column, columnTop, turn.Bottom);

            return score;
        }

        private static int GetScore(string[] board, int column, int columnTop, char turnBlock)
        {
            if (board[columnTop][column] == EmptyBlock)
                return 1;

            return 0;
        }

        private static int CalculateEmptyScore2(string[] board, int column, int columnTop, TurnBlocks turn)
        {
            if (IsOutOfBoard(column))
                return 0;

            if (HasEmptyBlockUnder(board, column, columnTop))
                return 0;

            var score = 0;

            score += GetScore2(board, column, columnTop - 1, turn.Top);
            score += GetScore2(board, column, columnTop, turn.Bottom);

            return score;
        }

        private static int GetScore2(string[] board, int column, int columnTop, char turnBlock)
        {
            if (board[columnTop][column] == turnBlock)
                return 10;

            return 0;
        }

        private static bool HasEmptyBlockUnder(string[] board, int column, int columnTop)
        {
            return columnTop < 11 && board[columnTop + 1][column] == EmptyBlock;
        }

        private static bool IsOutOfBoard(int column)
        {
            return column < 0 || column > 5;
        }

        private static int CalculateColumnTop(string[] board, int column)
        {
            var columnTop = -1;

            for (var i = 0; i < 12; i++)
            {
                if (board[i][column] == EmptyBlock)
                    columnTop++;
            }
            return columnTop;
        }
    }

    public struct TurnBlocks
    {
        public char Top { get; set; }
        public char Bottom { get; set; }
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