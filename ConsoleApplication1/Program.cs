using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    
    class Program
    {
        // Private variables
        private static Encoding jp = Encoding.GetEncoding(932); // changes encoding to JIF to allow Japanese output
        private static int[] multChoice = new int[6]; // array for the possible answers
        private static int currIndex;  // random flashcard array index for the randomly generated question
        private static ConsoleKeyInfo keyInfo;  // the multiple-choice menu response typed in by the user
        private static int cardTotal = 0;
        private static Random random = new Random();
        private static int points;
        private static int combo;
        //
        static void Main(string[] args)
        {
            string[] kanjiQuestions = { };
            string[] answers = { };
            CreateArray(out kanjiQuestions, out answers);
            cardTotal = kanjiQuestions.Length;
            List<int> listNoRepeat = new List<int>();

            // Displays welcome message
            Console.WriteLine("\nこんにちは！漢字アプリへようこそ！");
            // Main loop for flash card game
            do
            {
                // Sets the current index that will be the flash card question
                currIndex = random.Next(0, cardTotal);
                // while loop that checks for repeats
                while (listNoRepeat.Contains(currIndex))
                {
                    currIndex = random.Next(0, cardTotal);
                }
                
                for (int i = 0; i < multChoice.Length; i++)
                {
                    if (i == multChoice.Length - 1)
                    {
                        multChoice[i] = currIndex;
                    } else
                    {
                        multChoice[i] = random.Next(0, cardTotal);
                    }
                    
                }
                
                // Using LINQ the array is shuffled 6 times
                for (int i = 0; i < 5; i++)
                {
                    multChoice = multChoice.OrderBy(x => random.Next()).ToArray();
                }

                Console.WriteLine("\n************************************************************************************************************************");
                Console.WriteLine("質問:    {0}", kanjiQuestions[currIndex]);

                Console.WriteLine("\n一.  {0}", answers[multChoice[0]]);
                Console.WriteLine("\n二.  {0}", answers[multChoice[1]]);
                Console.WriteLine("\n三.  {0}", answers[multChoice[2]]);
                Console.WriteLine("\n四.  {0}", answers[multChoice[3]]);
                Console.WriteLine("\n五.  {0}", answers[multChoice[4]]);
                Console.WriteLine("\n六.  {0}\n", answers[multChoice[5]]);
 
                keyInfo = Console.ReadKey();

                // convert char to int (add try and maybe invalid loop here later)
                int intReadKey = (int)char.GetNumericValue(keyInfo.KeyChar);

                if (keyInfo.KeyChar == 'q')
                {
                    Console.WriteLine("\nYou scored  " + points + " points!");
                    break;
                }

                if (keyInfo.KeyChar >= '1' | keyInfo.KeyChar <= '4')
                {
                    // sometimes errors with a negative one
                    if (multChoice[intReadKey-1] == currIndex)
                    {
                        Console.WriteLine("\n\nおめでとうございます不明ユーザー様！");
                        listNoRepeat.Add(currIndex);
                        points += 100;
                        combo += 1;
                    } else
                    {
                        Console.WriteLine("\n\nとても下手サンですねぇ");
                        points -= 100;
                        combo = 0;
                    }

                    if (combo == 5)
                    {
                        Console.WriteLine("\nYou got five right in a row have some bonus points!");
                        points += 300;
                        combo = 0;
                    }
                }
                Console.WriteLine("\nScore: " + points);
            }
            while (keyInfo.KeyChar != 'q');

            Console.ReadLine();
        }


        // method used to populate the arrays
        public static void CreateArray(out string[] kanji, out string[] kanjiAnswer)
        {
            // Creates variables and arrays
            string line;
            kanji = new string[0];
            kanjiAnswer = new string[0];
            // Creates steamreader for reading in flashcard values
            StreamReader sra = new StreamReader("flashcard.txt", jp, true);
            // populatesa the arrays
            line = sra.ReadLine();
            //Console.WriteLine(line);
            kanji = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (kanji.Length < 2)
            {
                Console.WriteLine("Not enough flashcards!");
                return;
            }
            line = sra.ReadLine();
            kanjiAnswer = line.Split(new[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
            if (kanjiAnswer.Length < 2)
            {
                Console.WriteLine("Not enough flashcards!");
                return;
            }
            // closes the reader
            sra.Close();
            }
    }
}

 