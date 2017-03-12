using NUnit.Framework;
using System.Linq;
using static Player;

namespace ThereIsNoSpoon2
{
    public class Tests
    {
        [Test]
        public void ReturnsEmptyListForEmptyGrid()
        {
            var calc = new LinksCalculator();

            var result = calc.Calculate(new int[2, 2]);

            Assert.That(result, Is.Empty);
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(0, 0, 1, 0)]
        [TestCase(1, 2, 4, 2)]
        public void ReturnsSingleLinkForTwoNodes(int i1, int j1, int i2, int j2)
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[i1, j1] = 1;
            input[i2, j2] = 1;

            var result = calc.Calculate(input);

            Assert.That(result.Single(), Is.EqualTo($"{i1} {j1} {i2} {j2} 1"));
        }

        [Test]
        public void ReturnsSingleLinkForMoreNodes()
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[0, 0] = 1;
            input[2, 0] = 1;
            input[3, 1] = 1;
            input[3, 2] = 1;
            input[4, 4] = 1;
            input[3, 4] = 1;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[] 
            {
                "0 0 2 0 1",
                "3 1 3 2 1",
                "3 4 4 4 1"
            }));
        }

        [TestCase(0, 0, 0, 1)]
        [TestCase(1, 2, 4, 2)]
        public void ReturnsMultipleLinksForTwoNodes(
            int i1, int j1, int i2, int j2)
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[i1, j1] = 2;
            input[i2, j2] = 2;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                $"{i1} {j1} {i2} {j2} 2"
            }));
        }

        /* 1 . 1
         * . . .
         * . . 1
         */
        [Test]
        public void ReturnsMultipleLinksForThreeNodes()
        {
            var calc = new LinksCalculator();
            var input = new int[3, 3];
            input[0, 0] = 1;
            input[2, 0] = 2;
            input[2, 2] = 1;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                "0 0 2 0 1",
                "2 0 2 2 1"
            }));
        }

        [TestCase(0, 0, 1, 1)]
        [TestCase(0, 0, 1, 2)]
        public void DoesNotReturnLinksForDiagonalNodes(int i1, int j1, int i2, int j2)
        {
            var calc = new LinksCalculator();
            var input = new int[3, 3];
            input[i1, j1] = 1;
            input[i2, j2] = 1;

            var result = calc.Calculate(input);

            Assert.That(result, Is.Empty);
        }

        /* . 1 .
         * 1 4 1
         * . 1 .
         */
        [Test]
        public void ReturnsMultipleLinksForCross()
        {
            var calc = new LinksCalculator();
            var input = new int[3, 3];
            input[1, 0] = 1;
            input[0, 1] = 1;
            input[1, 1] = 4;
            input[2, 1] = 1;
            input[1, 2] = 1;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                "1 0 1 1 1",
                "0 1 1 1 1",
                "1 1 2 1 1",
                "1 1 1 2 1",
            }));
        }

        /* 1 . 3
         * . . .
         * 1 2 3
         */
        [Test]
        public void CodingameTestCase3()
        {
            var calc = new LinksCalculator();
            var input = new int[3, 3];
            input[0, 0] = 1;
            input[2, 0] = 3;
            input[0, 2] = 1;
            input[1, 2] = 2;
            input[2, 2] = 3;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                "0 0 2 0 1",
                "0 2 1 2 1",
                "1 2 2 2 1",
                "2 0 2 2 2",
            }));
        }

        [TestCase(0, 0, 0, 1, 3)]
        [TestCase(1, 2, 4, 2, 6)]
        public void ReturnMax2LinksPerNode(
            int i1, int j1, int i2, int j2, int count)
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[i1, j1] = count;
            input[i2, j2] = count;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                $"{i1} {j1} {i2} {j2} 2"
            }));
        }

        /* 1 4 . 3
         * . . . .
         * . . . 4
         */
        [Test]
        public void CodingameTestCase4()
        {
            var calc = new LinksCalculator();
            var input = new int[4, 3];
            input[0, 0] = 1;
            input[1, 0] = 4;
            input[3, 0] = 3;
            input[1, 2] = 4;
            input[3, 2] = 4;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                "0 0 1 0 1",
                "1 0 3 0 2",
                "1 2 3 2 2",
                "1 0 1 2 2"
            }));
        }
    }
}
