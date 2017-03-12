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
        public List<string> Calculate(int[,] grid)
        {
            var result = new List<string>();
            var links = new List<Link>();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    var cell = grid[x, y];
                    if (cell == 0)
                    {
                        continue;
                    }

                    var maxNode = links.Where(it => it.X == x || it.Y == y)
                        .OrderByDescending(it => it.Count)
                        .FirstOrDefault();
                    if (maxNode == null)
                    {
                        links.Add(new Link(x, y, cell));
                        continue;
                    }
                    else
                    {
                        if (maxNode.Count <= cell)
                        {
                            result.Add($"{maxNode.ToString()} {x} {y} {maxNode.Count}");
                            links.Remove(maxNode);
                            if (maxNode.Count < cell)
                            {
                                links.Add(new Link(x, y, cell));
                            }
                        }
                        else
                        {
                            result.Add($"{maxNode.ToString()} {x} {y} {cell}");
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
                        result.Add($"{current.ToString()} {end.ToString()} 1");
                    }
                }
            }
            return result;
        }

        private class Link
        {
            public Link(int j, int i, int count)
            {
                X = j;
                Y = i;
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
    }
}
