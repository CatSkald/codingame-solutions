using System;
using System.Collections.Generic;

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
            string start = null;
            int startCount = 0;
            var result = new List<string>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var cell = grid[j, i];
                    if (cell == 0)
                    {
                        continue;
                    }
                    if (start == null)
                    {
                        start = $"{j} {i}";
                        startCount = cell;
                    }
                    else
                    {
                        if (startCount <= cell)
                        {
                            result.Add($"{start} {j} {i} {startCount}");
                            startCount = cell - startCount;
                            start = $"{j} {i}";
                        }
                        else
                        {
                            result.Add($"{start} {j} {i} {cell}");
                            startCount -= cell;
                        }
                        if (startCount == 0)
                        {
                            start = null;
                        }
                    }
                }
            }
            return result;
        }
    }
}
