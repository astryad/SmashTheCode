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
    }


}
