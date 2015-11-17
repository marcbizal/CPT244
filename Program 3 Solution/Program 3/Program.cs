using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Soduku myPuzzle = new Soduku("sudoku-good.txt");
            if (myPuzzle.isLoaded())
            {
                myPuzzle.print();
                if (myPuzzle.isSolved())
                {
                    Console.WriteLine("Huzzah! This Soduku puzzle is solved.");
                }
                else
                {
                    Console.WriteLine("This puzzle is NOT solved, try again...");
                }
            }

            Console.ReadKey();
        }
    }
}
