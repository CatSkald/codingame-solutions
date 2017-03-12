using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/**
 * The machines are gaining ground. Time to show them what we're really made of...
 **/
class Player
{
    static void Main(string[] args)
    {
        int width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        int height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis
        var grid = new int[width, height];
        for (int i = 0; i < height; i++)
        {
            string line = Console.ReadLine(); // width characters, each either a number or a '.'
            for (int j = 0; j < width; j++)
            {
                var c = line[j];
                grid[j, i] = c == '.' ? 0 : int.Parse(c.ToString());
            }
        }

        foreach (var link in new LinksCalculator().Calculate(grid))
        {
            Console.WriteLine(link);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");


        // Two coordinates and one integer: a node, one of its neighbors, the number of links connecting them.
        //Console.WriteLine("0 0 2 0 1");
    }

    public class LinksCalculator
    {
        public IEnumerable<string> Calculate(int[,] grid)
        {
            var result = new List<Link>();
            var links = new List<Node>();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    var cell = grid[x, y];
                    if (cell == 0)
                        continue;

                    var node = links.Where(it => it.X == x || it.Y == y)
                        .OrderBy(it => it.Count == 1 ? int.MaxValue : it.Count)
                        .FirstOrDefault();
                    if (node == null)
                    {
                        links.Add(new Node(x, y, cell));
                        continue;
                    }
                    else
                    {
                        var min = MinOr2(node.Count, cell);
                        result.Add(new Link(node.X, node.Y, x, y, min));
                        node.Count -= min;
                        cell -= min;
                        if (node.Count <= 0)
                        {
                            links.Remove(node);
                        }
                        if (cell > 0)
                        {
                            links.Add(new Node(x, y, cell));
                        }
                    }
                }
            }
            while (links.Any())
            {
                var current = links.First();
                var end = links.Skip(1).FirstOrDefault(it => it.X == current.X || it.Y == current.Y);
                //TODO prevent from adding more than 2 links in this while loop
                //var existingCount = result.Where(r => r.X1 == current.X && r.Y1 == current.Y
                //                && r.X2 == end.X && r.Y2 == end.Y).Sum(r => r.Count);
                if (end != null) //&& existingCount < 2)
                {
                    var min = MinOr2(current.Count, end.Count); // Math.Min(end.Count, existingCount));
                    result.Add(new Link(current.X, current.Y, end.X, end.Y, min));
                    current.Count -= min;
                    end.Count -= min;
                    if (current.Count <= 0)
                    {
                        links.Remove(current);
                    }
                    if (end.Count <= 0)
                    {
                        links.Remove(end);
                    }
                }
                else
                {
                    links.Remove(current);
                }
            }

            //TODO optimize
            return result.GroupBy(l => $"{l.X1}{l.Y1}{l.X2}{l.Y2}")
                .Select(it => Link.CreateWithSum(it.First(), it.Sum(l => l.Count)))
                .Select(l => l.ToString())
                .ToList();
        }

        private int MinOr2(int val1, int val2)
        {
            return Math.Min(2, Math.Min(val1, val2));
        }

        [DebuggerDisplay("[{X} {Y}] {Count}")]
        private class Node
        {
            public Node(int x, int y, int count)
            {
                X = x;
                Y = y;
                Count = count;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Count { get; set; }

            public override string ToString()
            {
                return $"{X} {Y}";
            }
        }

        [DebuggerDisplay("[{X1} {Y1}] [{X2} {Y2}] {Count}")]
        private struct Link
        {
            public Link(int x1, int y1, int x2, int y2, int count)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
                Count = count;
            }

            public static Link CreateWithSum(Link link, int count)
            {
                return new Link
                {
                    X1 = link.X1,
                    Y1 = link.Y1,
                    X2 = link.X2,
                    Y2 = link.Y2,
                    Count = count
                };
            }

            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
            public int Count { get; set; }

            public override string ToString()
            {
                var count = Count > 2 ? 2 : Count;
                return $"{X1} {Y1} {X2} {Y2} {count}";
            }
        }
    }
}
