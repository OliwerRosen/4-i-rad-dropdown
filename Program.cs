using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace X_i_rad
{
    class Program
    {
        static string[,] board = new string[50, 50];
        public static int columnNumber;
        static bool exitGame;
        static int userTurn = 1;
        static string playerSymbol = "X";
        public static int maximumRows = 6;
        public static int maximumColumns = 7;
        public static int numberInARowToWin = 4;
        public static string topRowNumbers;
        public static bool drawGame;
        public static int availableRow;

        static void Main(string[] args)
        {
            SetupBoard();
            while (!exitGame)
            {
                drawGame = false;
                ShowCurrentBoard();
                UserTurn();
                CheckWonGame();
                nextPlayerTurn();
            }
        }
        private static void ShowCurrentBoard()
        {
            Console.Clear();
            Console.WriteLine(topRowNumbers + "\n");
            for (int i = 0; i < maximumRows; i++)
            {
                string currentRow = "";
                for (int j = 0; j < maximumColumns; j++)
                {
                    currentRow += board[i, j];
                }
                    Console.WriteLine($"{currentRow}\n");
            }
        }
        private static void UserTurn()
        {
            bool validColumnNumber = false;

            Console.WriteLine($" Spelare {userTurn} med {playerSymbol} får spela. \n");

            while (validColumnNumber != true)
            {
                Console.WriteLine($"Skriv in kolumnnummer mellan 1 och {maximumColumns} att släppa ner brickan i.");
                validColumnNumber = int.TryParse(Console.ReadLine(), out columnNumber);
                columnNumber--;
                if (!validColumnNumber | columnNumber < 0 | columnNumber >= maximumColumns)
                {
                    Console.WriteLine("Felaktigt kolumnnummer. \n");
                    validColumnNumber = false;
                    continue;
                }
                else
                {
                    bool availableSpace = CheckAvailableColumn();
                    if (availableSpace == false)
                    {
                        Console.WriteLine("Kolumnen är ifylld redan. Välj en annan.");
                        validColumnNumber = false;
                        continue;
                    }
                    else
                    {
                        if (userTurn == 1)
                        {
                            board[availableRow, columnNumber] = "[X] ";
                        }
                        else
                        {
                            board[availableRow, columnNumber] = "[O] ";
                        }
                        validColumnNumber = true;
                        continue;
                    }
                }
            }
        }
        private static bool CheckAvailableColumn()
        {
            for (int i = maximumRows-1; i >=0 ; i--)
            {
                bool availableSpace = board[i, columnNumber].Contains("[ ]");
                if (availableSpace == true)
                {
                    availableRow = i;
                    return true;
                }
            }
            return false;
        }
        private static void CheckWonGame()
        {
            checkFourHorizontal();
            checkFourVertical();
            checkFourDiagonal();
            drawGame = checkIfDraw();
            if (drawGame == true)
            {
                endGameDisplay();
            }
        }
        private static void checkFourHorizontal()
        {
            for (int i = 0; i < maximumRows; i++)
            {
                int numberOfScoresInARow = 0;
                for (int j = 0; j < maximumRows; j++)
                {

                    if (board[i, j].Contains(playerSymbol) == true)
                    {
                        numberOfScoresInARow++;
                        if (numberOfScoresInARow >= numberInARowToWin)
                        {
                            endGameDisplay();
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        numberOfScoresInARow = 0;
                        continue;
                    }
                }
            }
        }
        private static void checkFourVertical()
        {
            for (int j = 0; j < maximumColumns; j++)
            {
                int numberOfScoresInARow = 0;
                for (int i = 0; i < maximumRows; i++)
                {
                    if (board[i, j].Contains(playerSymbol) == true)
                    {
                        numberOfScoresInARow++;
                        if (numberOfScoresInARow >= numberInARowToWin)
                        {
                            endGameDisplay();
                            break;
                        }
                        continue;
                    }
                    else
                    {
                        numberOfScoresInARow = 0;
                        continue;
                    }
                }
            }
        }
        private static void checkFourDiagonal()
        {
            for (int i = 0; i < maximumRows; i++)
            {
                for (int j = 0; j < maximumColumns; j++)
                {
                    int numberOfScoresInARow = 0;
                    if (board[i, j].Contains(playerSymbol) == true)
                    {
                        int k = i;
                        int l = j;
                        numberOfScoresInARow = 0;
                        bool exceptionHandled = false;
                        while (board[k, l].Contains(playerSymbol) == true)
                        {
                            numberOfScoresInARow++;
                            if (numberOfScoresInARow >= numberInARowToWin)
                            {
                                endGameDisplay();
                                numberOfScoresInARow = 0;
                                break;
                            }
                            k++;
                            l++;
                            if (k <= -1 | k >= maximumRows | l <= -1 | l >= maximumColumns)
                            {
                                numberOfScoresInARow = 0;
                                exceptionHandled = true;
                                break;
                            }
                        }
                        if (exceptionHandled == false)
                        {
                            if (board[k, l].Contains(playerSymbol) == false)
                            {
                                numberOfScoresInARow = 0;
                            }
                        }
                        k = i;
                        l = j;
                        while (board[k, l].Contains(playerSymbol) == true)
                        {
                            numberOfScoresInARow++;
                            if (numberOfScoresInARow >= numberInARowToWin)
                            {
                                endGameDisplay();
                                break;
                            }
                            k++;
                            l--;
                            if (k <= -1 | k >= maximumRows | l <= -1 | l >= maximumColumns)
                            {
                                break;
                            }
                        }
                        continue;
                    }
                    else
                    {
                        numberOfScoresInARow = 0;
                        continue;
                    }
                }
            }
        }
        private static bool checkIfDraw()
        {
            bool noEmptySpotFound = true;
            for (int i = 0; i < maximumRows; i++)
            {
                for (int j = 0; j < maximumColumns; j++)
                {
                    if (board[i, j].Contains("[ ]") == true)
                    {
                        noEmptySpotFound = false;
                    }
                }
            }
            return noEmptySpotFound;
        }
        private static void nextPlayerTurn()
        {
            if (userTurn == 1)
            {
                userTurn = 2;
                playerSymbol = "O";
            }
            else
            {
                userTurn = 1;
                playerSymbol = "X";
            }
        }
        private static bool endGameDisplay()
        {
            ShowCurrentBoard();
            if (drawGame == true)
            {
                Console.WriteLine($"Det vart oavgjort! \n");
            }
            else
            {
                Console.WriteLine($"Spelare {userTurn} vann spelet! \n");
            }
            bool validRematchAnswer = false;
            while (validRematchAnswer == false)
            {
                Console.WriteLine("Spela igen? (Ja/nej)");
                string rematch = Console.ReadLine().ToLower().Trim();
                if (rematch == "ja")
                {
                    validRematchAnswer = true;
                    exitGame = false;
                    SetupBoard();
                    return exitGame;
                }
                else if (rematch == "nej")
                {
                    validRematchAnswer = true;
                    exitGame = true;
                    return exitGame;
                }
                else
                {
                    Console.WriteLine("Fel svar.");
                    continue;
                }
            }
            return false;
        }
        private static void SetupBoard()
        {
            Console.Clear();

            Console.WriteLine("Detta är ett 4 i rad spel där man släpper ner varje bricka lodrätt.");
           
            for (int i = 0; i < maximumRows; i++)
            {
                for (int j = 0; j < maximumColumns; j++)
                {
                    board[i, j] = "[ ] ";
                }
            }

            topRowNumbers = "";
            for (int i = 0; i < maximumColumns; i++)
            {
                if (i >= 1)
                {
                    topRowNumbers = topRowNumbers + "   " + (i + 1);
                }
                else
                {
                    topRowNumbers = topRowNumbers + " " + (i + 1);
                }
            }
        }
    }
}
