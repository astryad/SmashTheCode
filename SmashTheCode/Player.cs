using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
namespace SmashTheCode
{
    public class Player
    {
        public static void Main(string[] args)
        {

            // game loop
            while (true)
            {
                var colorA = -1;
                var colorB = -1;

                for (int i = 0; i < 8; i++)
                {
                    string[] inputs = Console.ReadLine().Split(' ');

                    if (i > 0) continue;

                    colorA = int.Parse(inputs[0]); // color of the first block
                    colorB = int.Parse(inputs[1]); // color of the attached block
                }

                Console.Error.WriteLine("Current color: " + colorA);

                var columnHeights = new int[6];
                var topColors = new int[6];
                for (int i = 0; i < 6; i++) topColors[i] = -1;

                for (int i = 0; i < 12; i++)
                {
                    string row = Console.ReadLine();

                    for (int j = 0; j < 6; j++)
                    {
                        if (topColors[j] == -1 && row[j] != '.')
                            topColors[j] = int.Parse(row[j].ToString());
                        if (row[j] != '.')
                            columnHeights[j]++;
                    }
                }

                for (int i = 0; i < 12; i++)
                {
                    // One line of the map ('.' = empty, '0' = skull block, '1' to '5' = colored block)
                    string row = Console.ReadLine(); 
                }

                var min = 999;
                var minColumn = -1;

                var minSameColor = 999;
                var sameColorColumn = -1;

                Console.Error.WriteLine("Top colors: " + String.Join(" ", topColors));

                for (int i = 0; i < 6; i++)
                {
                    if (topColors[i] == colorA && columnHeights[i] < minSameColor)
                    {
                        minSameColor = columnHeights[i];
                        sameColorColumn = i;
                    }

                    if (columnHeights[i] < min)
                    {
                        min = columnHeights[i];
                        minColumn = i;
                    }
                }

                Console.Error.WriteLine("Column (s): " + sameColorColumn);
                Console.Error.WriteLine("Column: " + minColumn);

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                // "x": the column in which to drop your blocks
                Console.WriteLine(sameColorColumn != -1 ? sameColorColumn : minColumn); 
            }
        }
    }
}