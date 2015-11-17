using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_3
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            int count = end - start + 1;
            T[] target = null;
            if (count > 0)
            {
                target = new T[count];
                for (int i = start; i <= end; i++)
                    target[i - start] = source[i];
            }
            return target;
        }
    }

    public class Soduku
    {
        private short[][] grid;

        public Soduku(string filename)
        {
            grid = null;
            loadFromFile(filename);
        }

        public void loadFromFile(string filename)
        {
            try {
                using (StreamReader file = new StreamReader(filename))
                {
                    short row = 0;
                    string line;

                    grid = new short[9][];
                    while ((line = file.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == "") {

                            if (row == 9)
                            {
                                Console.WriteLine("Loaded Soduku solution from {0}!", filename);
                            }
                            else
                            {
                                Console.WriteLine("Invalid solution file! Expected {0} more rows. Breaking...", 9 - row);
                                grid = null;
                            }
                            return;
                        }

                        string[] columns = line.Split(',');
                        if (columns.Length == 9)
                        {
                            grid[row] = new short[9];
                            for (short i = 0; i < columns.Length; i++)
                            {
                                if (!Int16.TryParse(columns[i], out grid[row][i]))
                                {
                                    Console.WriteLine("Invalid solution file! Expected number 1-9 in column {0}, got \"{1}\". Breaking...", i, columns[i]);
                                    grid = null;
                                    return;
                                }
                            }
                            row++;
                        }
                        else
                        {
                            Console.WriteLine("Invalid solution file! Expected {0} more columns in row {1}. Breaking...", 9 - columns.Length, row);
                            grid = null;
                            return;
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
                grid = null;
            }
        }

        public bool isLoaded()
        {
            return grid != null;
        }

        public bool unique(short[] array)
        {
            List<short> numbers = new List<short>();
            foreach (short number in array)
            {
                if (numbers.Contains(number)) return false;
                numbers.Add(number);
            }
            return true;
        }

        public short[] getBlock(short blockNum)
        {
            short[] block = null;
            if (blockNum >= 0 && blockNum < 9)
            {
                short startRow = (short)(Math.Floor(blockNum / 3.0) * 3);
                short startColumn = (short)((blockNum % 3) * 3);

                block = new short[9];
                for (short row = startRow; row <= startRow + 2; row++)
                {
                    grid[row].Slice<short>(startColumn, startColumn + 2).CopyTo(block, (row - startRow) * 3);
                }
            }
            else
            {
                Console.WriteLine("Block #{0} does not exist!");
            }

            return block;
        }

        public short[] getColumn(short columnNum)
        {
            short[] column = null;
            if (columnNum >= 0 && columnNum < 9)
            {
                column = new short[9];
                for (short row = 0; row < 9; row++)
                {
                    column[row] = grid[row][columnNum];
                }
            }
            return column;
        }

        public short[] getRow(short rowNum)
        {
            return grid[rowNum];
        }

        private bool checkSet(Func<short, short[]> get, string type)
        {
            bool allUnique = true;
            short[] set = null;
            for (short i = 0; i < 9; i++)
            {
                set = get(i);
                if (!unique(set))
                {
                    Console.WriteLine("{0}: {1}", type, i);
                    allUnique = false;
                }
            }
            return allUnique;
        }

        public bool isSolved()
        {
            bool rows = checkSet(getRow, "ROW");
            bool columns = checkSet(getColumn, "COL");
            bool blocks = checkSet(getBlock, "BLK");

            return rows && columns && blocks;
        }

        public void print()
        {
            for (short row = 0; row < 9; row++)
            {
                if (row == 3 || row == 6) Console.WriteLine("---+---+---");

                string strRow = string.Join("", getRow(row));
                strRow = strRow.Insert(6, "|");
                strRow = strRow.Insert(3, "|");

                Console.WriteLine(strRow);
            }
        }
    }
}
