using System;
using System.Collections.Generic;
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

                    var maxNode = links.Where(it => it.X == x || it.Y == y)
                        .OrderBy(it => it.Count == 1 ? int.MaxValue : it.Count)
                        .FirstOrDefault();
                    if (maxNode == null)
                    {
                        links.Add(new Node(x, y, cell));
                        continue;
                    }
                    else
                    {
                        if (maxNode.Count <= cell)
                        {
                            result.Add(new Link(maxNode.X, maxNode.Y, x, y, maxNode.Count));
                            links.Remove(maxNode);
                            if (maxNode.Count < cell)
                            {
                                links.Add(new Node(x, y, cell - maxNode.Count));
                            }
                        }
                        else
                        {
                            result.Add(new Link(maxNode.X, maxNode.Y, x, y, cell));
                            maxNode.Count -= cell;
                        }
                    }
                }
            }
            if (links.Count > 1)
            {
                for (int i = 0; i < links.Count; i++)
                {
                    var current = links[i];
                    var end = links.Skip(i + 1)
                        .FirstOrDefault(it => it.X == current.X || it.Y == current.Y);
                    if (end != null)
                    {
                        int count;
                        if (current.Count <= end.Count)
                        {
                            count = current.Count;
                            links.Remove(current);
                            if (current.Count == end.Count)
                            {
                                links.Remove(end);
                            }
                            else
                            {
                                end.Count -= current.Count;
                            }
                        }
                        else
                        {
                            count = end.Count;
                            links.Remove(end);
                            current.Count -= end.Count;
                        }
                        result.Add(new Link(current.X, current.Y, end.X, end.Y, count));
                    }
                }
            }
            return result.GroupBy(l => $"{l.X1}{l.Y1}{l.X2}{l.Y2}")
                .Select(it => Link.CreateWithSum(it.First(), it.Sum(l => l.Count)))
                .Select(l => l.ToString())
                .ToList();
        }

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
