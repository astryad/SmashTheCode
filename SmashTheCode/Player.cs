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

            var board = new Board(PlayerBoard);

            var columnScore = Enumerable.Range(0, 6).Select(
                i =>
                {
                    var nextBoard = board.Play(NextTurns[0], i);

                    var score = nextBoard.ResolveCombos();

                    var secondTurnScores = Enumerable.Range(0, 6).Select(
                        j => new
                        {
                            Column = j,
                            Score = nextBoard.Play(NextTurns[1], j).ResolveCombos()
                        });

                    score += secondTurnScores.OrderByDescending(c => c.Score).First().Column;

                    return new
                    {
                        Column = i,
                        Score = score
                    };
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

    public interface IBoard
    {
        string[] BoardData { get; }
        int ResolveCombos();
        int CalculateCombo(int row, int column);
        void UpdateBoard();
        bool IsVisited(int row, int column);
        char GetBlockType(int row, int column);
        IBoard Play(TurnBlocks turnBlocks, int column);
    }

    public class Board : IBoard
    {
        private readonly Block[][] _blocks;

        public string[] BoardData
        {
            get { return _blocks.Select(row => string.Join("", row.Select(block => block.BlockType))).ToArray(); }
        }

        public Board(string[] boardData)
        {
            _blocks = new Block[12][];
            for (var i = 0; i < 12; i++)
            {
                _blocks[i] = new Block[6];
                for (var j = 0; j < 6; j++)
                {
                    _blocks[i][j] = new Block(boardData[i][j], i, j);
                }
            }
        }

        private class Block
        {
            public char BlockType { get; set; }
            public int Row { get; private set; }
            public int Column { get; private set; }

            public Block(char blockType, int row, int column)
            {
                Visited = false;
                BlockType = blockType;
                Row = row;
                Column = column;
            }

            public bool Visited { get; set; }
        }

        public int ResolveCombos()
        {
            var score = 0;

            int turnScore = 0;
            do
            {
                turnScore = 0;
                for (int i = 11; i >= 0; i--)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        turnScore += CalculateCombo(i, j);
                    }
                }

                UpdateBoard();
                score += turnScore;
            } while (turnScore != 0);

            return score;
        }

        public int CalculateCombo(int row, int column)
        {
            var start = _blocks[row][column];

            if (start.BlockType == '.')
                return 0;

            var startType = start.BlockType;

            var blocksToCheck = new Stack<Block>();
            blocksToCheck.Push(start);

            var comboBlocks = new List<Block>();

            while (blocksToCheck.Count > 0)
            {
                var block = blocksToCheck.Pop();

                if (!block.Visited && block.BlockType == startType)
                {
                    comboBlocks.Add(block);
                    if (block.Column > 0) blocksToCheck.Push(_blocks[block.Row][block.Column - 1]);
                    if (block.Column < 5) blocksToCheck.Push(_blocks[block.Row][block.Column + 1]);
                    if (block.Row > 0) blocksToCheck.Push(_blocks[block.Row - 1][block.Column]);
                    block.Visited = true;
                }
            }

            if (comboBlocks.Count > 3)
            {
                comboBlocks.ForEach(block => block.BlockType = '.');
                return comboBlocks.Count;
            }

            return 0;
        }

        public void UpdateBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 11; j >= 0; j--)
                {
                    _blocks[j][i].Visited = false;

                    if (_blocks[j][i].BlockType == '.')
                    {
                        int shift = j;
                        while (shift >= 0 && _blocks[shift][i].BlockType == '.')
                            shift--;

                        int row = j;
                        while (shift >= 0)
                        {
                            _blocks[row][i].BlockType = _blocks[shift][i].BlockType;
                            row--;
                            shift--;
                        }

                        while (row >= 0)
                        {
                            _blocks[row][i].BlockType = '.';
                            row--;
                        }
                    }
                }
            }
        }

        public bool IsVisited(int row, int column)
        {
            return _blocks[row][column].Visited;
        }

        public char GetBlockType(int row, int column)
        {
            return _blocks[row][column].BlockType;
        }

        public IBoard Play(TurnBlocks turnBlocks, int column)
        {
            var newBoard = new Board(BoardData);

            var row = 11;
            while (newBoard._blocks[row][column].BlockType != '.' && row > 0)
                row--;

            if (row == 0)
                return new InvalidBoard();

            newBoard._blocks[row][column].BlockType = turnBlocks.Bottom;
            newBoard._blocks[row - 1][column].BlockType = turnBlocks.Top;

            return newBoard;
        }
    }

    public class InvalidBoard : IBoard
    {
        public string[] BoardData { get; private set; }
        public int ResolveCombos()
        {
            return -100;
        }

        public int CalculateCombo(int row, int column)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoard()
        {
            throw new NotImplementedException();
        }

        public bool IsVisited(int row, int column)
        {
            throw new NotImplementedException();
        }

        public char GetBlockType(int row, int column)
        {
            throw new NotImplementedException();
        }

        public IBoard Play(TurnBlocks turnBlocks, int column)
        {
            return this;
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