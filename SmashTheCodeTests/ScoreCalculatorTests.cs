using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SmashTheCode;

namespace SmashTheCodeTests
{
    class ScoreCalculatorTests
    {
        public string[] EmptyBoard => Enumerable.Repeat("......", 12).ToArray();

        [TestCase(0)]
        [TestCase(5)]
        public void Should_return_score_of_4_when_column_has_4_free_blocks_on_sides(int column)
        {
            var scoreCalculator = new ScoreCalculator();
            var actual = scoreCalculator.EvaluateScore(EmptyBoard, column, new TurnBlocks {Top = '0', Bottom = '0'});
            Check.That(actual).IsEqualTo(4);
        }

        [TestCase(1)]
        [TestCase(4)]
        public void Should_return_score_of_6_when_column_has_6_free_blocks_on_sides(int column)
        {
            var scoreCalculator = new ScoreCalculator();
            var actual = scoreCalculator.EvaluateScore(EmptyBoard, column, new TurnBlocks { Top = '0', Bottom = '0' });
            Check.That(actual).IsEqualTo(6);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void Should_return_score_of_8_when_column_has_8_free_blocks_on_sides(int column)
        {
            var scoreCalculator = new ScoreCalculator();
            var actual = scoreCalculator.EvaluateScore(EmptyBoard, column, new TurnBlocks { Top = '0', Bottom = '0' });
            Check.That(actual).IsEqualTo(8);
        }
    }
}
