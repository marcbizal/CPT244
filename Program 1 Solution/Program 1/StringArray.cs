using System;

namespace ArraySorting_OOP
{
    class StringArray
    {
        private const int MAX_SIZE = 32;
        string[] sArray;
        int size = 0;

        public StringArray()
        {
            sArray = new string[MAX_SIZE];
        }

        public StringArray(string[] inArray, int length)
        {
            sArray = inArray;
            size = length;
        }

        public bool empty()
        {
            return size <= 0;
        }

        public void add(string newEntry)
        {
            sArray[size] = newEntry;
            size++;
        }

        private void shiftUp(int fromIndex = 0, int numEntries = 1)
        {
            for (int i = size - 1; i >= fromIndex; i--)
            {
                if (i + numEntries < MAX_SIZE)
                {
                    sArray[i + numEntries] = sArray[i];
                    sArray[i] = null;
                }
            }
            size += numEntries;
        }

        private void shiftDown(int fromIndex = 0, int numEntries = 1)
        {
            for (int i = fromIndex; i <= size - 1; i++)
            {
                if (i - numEntries >= 0)
                {
                    sArray[i - numEntries] = sArray[i];
                    sArray[i] = null;
                }
            }
            size -= numEntries;
        }

        public void insert(string newEntry, int atIndex = -1)
        {
            if (!empty())
            {
                if (atIndex == -1)
                    atIndex = Math.Abs(binarySearch(newEntry));

                // This fixes an edge case where there aren't enough elements to Binary Search properly.
                if (atIndex < size)
                {
                    shiftUp(atIndex);
                    sArray[atIndex] = newEntry;
                }
                else
                {
                    add(newEntry);
                }
            }
            else
            {
                add(newEntry);
            }
        }

        public void delete(int atIndex)
        {
            shiftDown(atIndex + 1);
        }

        public void BubbleSort()
        {
            int lastSwap = size - 1;
            while (lastSwap != 0)
            {
                int comparisonsToMake = lastSwap;
                lastSwap = 0;
                for (int j = 0; j < comparisonsToMake; j++)
                {
                    if (sArray[j].CompareTo(sArray[j + 1]) > 0)
                    {
                        string tmp = sArray[j];
                        sArray[j] = sArray[j + 1];
                        sArray[j + 1] = tmp;
                        lastSwap = j;
                    }
                }
            }
        }

        public int binarySearch(string target)
        {
            return binarySearch(target, 0, size);
        }

        public int binarySearch(string target, int min, int max)
        {
            if (min <= max)
            {
                int mid = min + ((max - min) / 2);

                int relationship = target.CompareTo(sArray[mid]);

                if (relationship < 0)
                {
                    return binarySearch(target, min, mid - 1);
                }
                else if (relationship > 0)
                {
                    return binarySearch(target, mid + 1, max);
                }
                else
                {
                    return mid;
                }
            }
            else
            {
                return -min;
            }
        }


        public void PrintArray()
        {
            for (int ndx = 0; ndx < size; ndx++)
            {
                Console.Write(sArray[ndx] + " ");
            }
            Console.WriteLine();
        }

        public static void demoStringSort()
        {
            string cat;
            int ndx;
            StringArray myArray = new StringArray();
            myArray.add("Tegan");
            myArray.add("Alystra");
            myArray.add("Brandon");
            myArray.add("Oliver");
            myArray.add("Casey");
            myArray.add("Midnight");
            myArray.add("Jeremiah");
            myArray.add("Maya");

            myArray.BubbleSort();
            myArray.PrintArray();
            cat = "Midnight";
            ndx = myArray.binarySearch(cat);

            string name = "Branford";
            int insertionPoint = myArray.binarySearch(name);
            Console.WriteLine("Should insert {0} at {1}", name, insertionPoint);
            Console.WriteLine("{0} found at index: {1}", cat, ndx);
        }

        public static void demoSortingInput()
        {
            StringArray myArray = new StringArray();
            string name;
            Console.Write("Enter a name ('quit' to stop': ");
            name = Console.ReadLine();
            while (name != "quit")
            {
                myArray.add(name);
                Console.Write("Enter a name ('quit' to stop': ");
                name = Console.ReadLine();
            }

            myArray.BubbleSort();
            myArray.PrintArray();
        }

        public static short getMenuSelection()
        {
            short selection;
            string input = Console.ReadLine();

            while (!Int16.TryParse(input, out selection))
            {
                Console.Write("Invalid selection! Please enter a number.\n");
                input = Console.ReadLine();
            }

            return selection;
        }

        public static void demoInsertDelete()
        {
            StringArray myArray = new StringArray();
            Console.Write(  
                "Please make a selection:\n" +
                "1. Insert\n" +
                "2. Delete\n" +
                "3. Display All\n" +
                "4. Quit\n"
            );

            short selection = getMenuSelection();
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                    {
                        Console.Write("What name would you like to insert?\n");
                        string name = Console.ReadLine();
                        myArray.insert(name);
                        break;
                    }
                    case 2:
                    {
                        Console.Write("What name would you like to delete?\n");
                        string name = Console.ReadLine();
                        int index = myArray.binarySearch(name);
                        if (index > 0)
                        {
                            myArray.delete(Math.Abs(index));
                        }
                        else
                        {
                            // Not found...
                            Console.Write("Could not find {0} in array!\n", name);
                        }
                        break;
                    }
                    case 3:
                    {
                        myArray.PrintArray();
                        break;
                    }
                    default:
                    {
                        Console.Write(
                            "Invalid selection! Please choose one of the following:\n" +
                            "1. Insert\n" +
                            "2. Delete\n" +
                            "3. Display All\n" +
                            "4. Quit\n"
                        );

                        break;
                    }
                }
                Console.Write("Please make another selection:\n");
                selection = getMenuSelection();
            }
        }

        static void Main(string[] args)
        {
            StringArray.demoInsertDelete();
            StringArray.demoStringSort();
            StringArray.demoSortingInput();
        }
    }
}
