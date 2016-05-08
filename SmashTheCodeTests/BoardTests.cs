using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using SmashTheCode;

namespace SmashTheCodeTests
{
    class BoardTests
    {
        [Test]
        public void Should_return_0_when_board_contains_no_combos()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......"
            };

            var board = new Board(boardData);

            var score = board.ResolveCombos();

            Check.That(score).IsEqualTo(0);
        }

        [Test]
        public void Should_return_4_when_board_contains_a_simple_combo()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.ResolveCombos();

            Check.That(score).IsEqualTo(4);
        }

        [Test]
        public void Should_return_0_when_checked_block_has_no_combo()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.CalculateCombo(11, 1);

            Check.That(score).IsEqualTo(0);
        }

        [Test]
        public void Should_return_4_when_checked_block_has_a_simple_combo()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.CalculateCombo(11, 2);

            Check.That(score).IsEqualTo(4);
        }

        [Test]
        public void Should_return_0_when_checked_block_has_already_been_included_in_a_combo()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            board.CalculateCombo(11, 2);
            var score = board.CalculateCombo(11, 3);

            Check.That(score).IsEqualTo(0);
        }

        [Test]
        public void Should_change_block_type_to_empty_when_block_is_part_of_combo()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.CalculateCombo(11, 2);

            Check.That(board.GetBlockType(11, 2)).IsEqualTo('.');
        }

        [Test]
        public void Should_return_0_when_combo_uses_less_than_4_blocks()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..34..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.CalculateCombo(11, 2);

            Check.That(score).IsEqualTo(0);
        }

        [Test]
        public void Should_not_change_block_type_when_combo_uses_less_than_4_blocks()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..34..",
                "..33.."
            };

            var board = new Board(boardData);

            var score = board.CalculateCombo(11, 2);

            Check.That(board.GetBlockType(11, 2)).IsEqualTo('3');
        }

        [Test]
        public void Should_not_change_board_when_updating_a_board_with_no_holes()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            board.UpdateBoard();

            Check.That(board.BoardData).ContainsExactly(boardData);
        }

        [Test]
        public void Should_return_updated_board_when_updating_a_board_with_one_hole()
        {
            var boardData = new[]
               {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "...2..",
                "..33..",
                "..3..."
            };

            var board = new Board(boardData);

            board.UpdateBoard();

            Check.That(board.BoardData).ContainsExactly(new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..32..",
                "..33.."
            });
        }

        [Test]
        public void Should_return_updated_board_when_board_contains_multiple_complex_holes()
        {
            var boardData = new[]
                  {
                "......",
                "......",
                "......",
                "......",
                "......",
                "...3..",
                "...3..",
                "..5...",
                "1.5.5.",
                "1..25.",
                "42334.",
                "423.4."
            };

            var board = new Board(boardData);

            board.UpdateBoard();

            Check.That(board.BoardData).ContainsExactly(new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "1.535.",
                "1.535.",
                "42324.",
                "42334."
            });
        }

        [Test]
        public void Should_mark_all_blocks_as_not_visited_when_updating_board()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "..33..",
                "..33.."
            };

            var board = new Board(boardData);

            board.CalculateCombo(11, 2);
            board.UpdateBoard();

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Check.That(board.IsVisited(i,j)).IsFalse();
                }
            }
        }

        [Test]
        public void Should_count_chain_combos_in_score_when_a_board_contains_chain_combos()
        {
            var boardData = new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "...2..",
                "...2..",
                "...44.",
                "...44.",
                "...231",
                "...201"
            };

            var board = new Board(boardData);

            var score = board.ResolveCombos();

            Check.That(score).IsEqualTo(8);
        }

        [Test]
        public void Should_return_board_with_current_turn_when_turn_is_played_in_column()
        {
            var boardData = new[]
                  {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "121314"
            };

            var board = new Board(boardData);

            IBoard nextBoard = board.Play(new TurnBlocks {Top = '3', Bottom = '3'}, 3);

            Check.That(nextBoard.BoardData).ContainsExactly(new[]
            {
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "......",
                "...3..",
                "...3..",
                "121314"
            });
        }

    }


}
