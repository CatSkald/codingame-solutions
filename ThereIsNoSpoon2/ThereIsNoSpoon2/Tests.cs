﻿using NUnit.Framework;
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

        [TestCase(0, 0, 0, 1, 2)]
        [TestCase(1, 2, 4, 2, 4)]
        public void ReturnsMultipleLinksForTwoNodes(
            int i1, int j1, int i2, int j2, int count)
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[i1, j1] = count;
            input[i2, j2] = count;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                $"{i1} {j1} {i2} {j2} {count}"
            }));
        }

        [Test]
        public void ReturnsMultipleLinksForThreeNodes()
        {
            var calc = new LinksCalculator();
            var input = new int[5, 5];
            input[0, 0] = 1;
            input[2, 0] = 2;
            input[0, 2] = 1;

            var result = calc.Calculate(input);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                "0 0 2 0 1",
                "0 2 2 0 1"
            }));
        }
    }
}