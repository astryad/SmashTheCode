using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using SmashTheCode;
using FakeItEasy;

namespace SmashTheCodeTests
{
    public class GameTests
    {
        [Test]
        public void Should_read_next_colors_when_resolving_a_turn()
        {
            var inputReader = A.Fake<IConsole>();
            A.CallTo(() => inputReader.ReadLine()).ReturnsNextFromSequence(
                "0 0",
                "0 0",
                "0 0",
                "0 0",
                "0 0",
                "0 0",
                "0 0",
                "0 0",
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
                "......",
                "......");

            var game = new Game(inputReader);

            game.ResolveTurn();

            Check.That(game.NextColors).ContainsExactly(
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0}, 
                new GameColor { First = 0, Second = 0});
        }
    }


}
