/* 
Author: Gurman Brar
BME 24'
University of Waterloo
*/

using System;
using static System.Console;


namespace Bme121
{
    static class Program
    {

        static bool useBoxDrawingChars = true;
        static string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l" };
        // must be in the range 1..1

        static double cellMarkProb = 0.2;
        static Random rGen = new Random();


        // Variable used in the C# method
        static int xcor;
        static int ycor;

        // Method we created to convert the character input into a number value that cordinates into a cell on the array.
        public static int CharToInt(char character)
        {
            switch (character)
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;

                default:
                    return -1;
            }


        }
        static int boardSize;
        static void Main()
        {
            WriteLine("Enter your desired board size between 2 and 12: ");
            boardSize = int.Parse(ReadLine());


            // Declaring all hidden and shown arrays
            int[] rowSumCPU = new int[boardSize];
            int[] rowSumUser = new int[boardSize];
            int[] colSumCPU = new int[boardSize];
            int[] colSumUser = new int[boardSize];
            int[,] userBoard = new int[boardSize, boardSize];
            int[,] CPUBoard = new int[boardSize, boardSize];
            bool[,] TrueFalse = new bool[boardSize, boardSize];

            // This is filling the true or false array with all false values so no x is displayed
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    if (rGen.NextDouble() <= cellMarkProb)
                        CPUBoard[i, j] = 1;
                    else
                        CPUBoard[i, j] = 0;

            // Filling a true or false array with all false values
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                    TrueFalse[i, j] = false;


            // Producing arrays with the addition of the hidden sums within the hidden array
            for (int i = 0; i < boardSize; i++)
                for (int j = 0; j < boardSize; j++)
                {
                    colSumCPU[i] = colSumCPU[i] + (CPUBoard[i, j] * (j + 1));
                    rowSumCPU[j] = rowSumCPU[j] + (CPUBoard[i, j] * (i + 1));

                }


            bool gameNotQuit = true;
            while (gameNotQuit)
            {

                Console.Clear();
                WriteLine();
                WriteLine("    Play Kakurasu!");
                WriteLine();


                // Display the game board.
                // TO DO: Update code to correctly display the game state.

                if (useBoxDrawingChars)
                {
                    for (int row = 0; row < boardSize; row++)
                    {
                        if (row == 0)
                        {
                            Write("        ");
                            for (int col = 0; col < boardSize; col++)
                                Write("  {0} ", letters[col]);
                            WriteLine();

                            Write("        ");
                            for (int col = 0; col < boardSize; col++)
                                Write(" {0,2} ", col + 1);
                            WriteLine();

                            Write("        \u250c");
                            for (int col = 0; col < boardSize - 1; col++)
                                Write("\u2500\u2500\u2500\u252c");
                            WriteLine("\u2500\u2500\u2500\u2510");
                        }

                        Write("   {0} {1,2} \u2502", letters[row], row + 1);

                        for (int col = 0; col < boardSize; col++)
                        {
                            if (TrueFalse[row, col]) Write("  x\u2502");     // Displays the x's as well as removes them based on changes in the true or false array.
                            else Write("   \u2502");
                        }
                        WriteLine($"{colSumUser[row]:D2}"+ " " +$"{rowSumCPU[row]:D2}");    //Displays the users row input sum on the real board. Also displays the hidden row sum on board.

                        if (row < boardSize - 1)
                        {
                            Write("        \u251c");
                            for (int col = 0; col < boardSize - 1; col++)
                                Write("\u2500\u2500\u2500\u253c");
                            WriteLine("\u2500\u2500\u2500\u2524");
                        }
                        else
                        {
                            Write("        \u2514");
                            for (int col = 0; col < boardSize - 1; col++)
                                Write("\u2500\u2500\u2500\u2534");
                            WriteLine("\u2500\u2500\u2500\u2518");

                            Write("         ");
                            for (int col = 0; col < boardSize; col++)
                                Write($"  { rowSumUser[col]:D2}");               // Displays the users column input sum on the real board. 
                            WriteLine();

                            Write("         ");
                            for (int col = 0; col < boardSize; col++)
                                Write($"  {colSumCPU[col]:d2}");          //Dispalys the column sum of the hidden board on real board.
                            WriteLine();
                        }
                    }
                }

                WriteLine();
                WriteLine("   Toggle cells to match the row and column sums.");
                Write("   Enter a row-column letter pair or 'quit': ");
                WriteLine();

                string response = ReadLine();

                if (response == "quit") gameNotQuit = false;
                else if (response.Length == 2)
                {

                    char[] singular = response.ToCharArray();              // A built in C# method that changes strings to individual characters
                    xcor = CharToInt(singular[0]);                         // Assigning an int variable to the value of each char based on the method we created to change from character to integer
                    ycor = CharToInt(singular[1]);
                    if (TrueFalse[xcor, ycor])
                    {
                        TrueFalse[xcor, ycor] = false;
                        colSumUser[xcor] = colSumUser[xcor] - (ycor + 1);  // If the cell is already marked (true), subtract from the sum and make the cell equal to false to remove the x
                        rowSumUser[ycor] = rowSumUser[ycor] - (xcor + 1);
                    }
                    else
                    {
                        TrueFalse[xcor, ycor] = true;
                        colSumUser[xcor] = colSumUser[xcor] + (ycor + 1);
                        rowSumUser[ycor] = rowSumUser[ycor] + (xcor + 1);
                    }

                }
                else WriteLine("Please enter an appropriate cell coordinate");
            }
        }
    }
}
