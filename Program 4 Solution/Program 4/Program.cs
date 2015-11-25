using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_4
{
    class BookStats
    {
        SortedDictionary<string, int> words;

        public BookStats(string path)
        {
            words = new SortedDictionary<string, int>();
            string text = File.ReadAllText(path);

            char[] delimiters = { ' ', '\n' };
            string[] allWords = text.Split(delimiters);

            foreach (string word in allWords)
            {
                if (words.ContainsKey(word))
                {
                    words[word]++;
                }
                else
                {
                    words.Add(word, 1);
                }
            }

            // POSTPROCESSING
            double avgFrequency = 0;
            int maxFrequency = 0;

            int freq;
            for (int i = 0; i < words.Count; i++)
            {
                freq = words.ElementAt(i).Value;
                avgFrequency += freq;
                if (freq > maxFrequency) maxFrequency = freq;
            }

            avgFrequency /= words.Count;

            Console.Write(
                "\n-----------------------------------\n" +
                "Filename: {0}\n" +
                "Unique Words: {1}\n" +
                "Average Frequency: {2}\n" +
                "Max Frequency: {3}\n" +
                "-----------------------------------\n\n",
                path,
                words.Count,
                avgFrequency,
                maxFrequency
            );

        }

        public int getFrequency(string word)
        {
            if (words.ContainsKey(word))
            {
                return words[word];
            }
            else
            {
                return 0;
            }
        }

        public ArrayList getWordsWithFrequency(int targetFrequency)
        {
            ArrayList matches = new ArrayList();

            for (int i = 0; i < words.Count; i++)
			{
			    if (words.ElementAt(i).Value == targetFrequency) 
                    matches.Add(words.ElementAt(i).Key);
			}

            // Because the word list is already sorted alphabetically, this list should be
            // alphabetized as well. No need to sort.
            // matches.Sort();
            return matches;
        }

        public List<KeyValuePair<string, int>> getWordsWithLength(int targetLength)
        {
            List<KeyValuePair<string, int>> matches = new List<KeyValuePair<string, int>>();

            for (int i = 0; i < words.Count; i++)
			{
                if (words.ElementAt(i).Key.Length == targetLength)
                    matches.Add(new KeyValuePair<string, int>(words.ElementAt(i).Key, words.ElementAt(i).Value));
			}

            // Sort first by frequency, and then alphabetize
            matches.Sort((x, y) => x.Value != y.Value ? x.Value.CompareTo(y.Value) : x.Key.CompareTo(y.Key));
            return matches;
        }
    }

    class Program
    {
        private const string MENU = "1. Find frequency for word\n" +
                                    "2. Find words for frequency\n" +
                                    "3. Find words with length\n" +
                                    "4. Quit\n";

        public static int ReadNumber()
        {
            int num;
            string input = Console.ReadLine();

            while (!Int32.TryParse(input, out num))
            {
                Console.Write("Invalid input! Please enter a number.\n");
                input = Console.ReadLine();
            }

            return num;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Which file would you like to analyze?");
            string filename = Console.ReadLine();

            BookStats book = null;
            if (File.Exists(filename))
            {
                book = new BookStats(filename);

                Console.Write("Please make a selection:\n" + MENU);

                int selection = ReadNumber();
                while (selection != 4)
                {
                    switch (selection)
                    {
                        case 1:
                        {
                            Console.WriteLine("What word would you like to find the frequency of?");
                            string word = Console.ReadLine();
                            int freq = book.getFrequency(word);

                            Console.WriteLine("\"{0}\" occurs {1} times in this book.", word, freq);
                            break;
                        }
                        case 2:
                        {
                            Console.WriteLine("What frequency would you like to match?");
                            int freq = ReadNumber();
                            ArrayList matches = book.getWordsWithFrequency(freq);

                            Console.WriteLine("Matched {0} words with a frequency of {1}:", matches.Count, freq);
                            foreach (string match in matches)
                            {
                                Console.WriteLine(match);
                            }

                            break;
                        }
                        case 3:
                        {
                            Console.WriteLine("What word length would you like to match?");
                            int length = ReadNumber();
                            List<KeyValuePair<string, int>> matches = book.getWordsWithLength(length);

                            Console.WriteLine("Matched {0} words with a length of {1}:", matches.Count, length);
                            for (int i = 0; i < matches.Count; i++)
                            {
                                Console.WriteLine("{0} ({1})", matches.ElementAt(i).Key, matches.ElementAt(i).Value);
                            }

                            break;
                        }
                        default:
                        {
                            Console.Write("Invalid selection! Please choose one of the following:\n" + MENU);
                            break;
                        }
                    }
                    Console.Write("Please make another selection:\n");
                    selection = ReadNumber();
                }
            }
            else
            {
                Console.WriteLine("{0} does not exist!", filename);
            }
        }
    }
}
