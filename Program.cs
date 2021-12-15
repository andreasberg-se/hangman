//
// [Main Program]
// Hangman
//

using System;
using System.Text;
using static System.Console;

namespace Hangman
{

    class Program
    {

        // Game data.
        static bool gameIsRunning;
        static string word;
        static char[] correctLetters;
        static StringBuilder incorrectLetters;
        static int attempts;    

        // Get a random word.
        static string GetRandomWord()
        {
            string[] words = {"APPLE", "PEAR", "BANANA", "PINEAPPLE", "CHERRY",
                "TOMATO", "CUCUMBER", "CORN", "ONION", "BROCCOLI", "CARROT",
                "AMETHYST", "EMERALD", "DIAMOND", "AQUAMARINE", "RUBY", "SAPPHIRE", "TOPAZ"};
            Random random = new Random();
            return words[random.Next(words.Length)];
        }

        // Create word to display (reveal correct guessed letters).
        static string GetDisplayWord(string word, char[] letters)
        {
            string displayWord = "";
            for (int i = 0; i < word.Length; i++)
            {
                char displayLetter = '-';
                for (int j = 0; j < letters.Length; j++)
                {
                    if (word[i] == letters[j])
                    {
                        displayLetter = word[i];
                        break;
                    }
                }
                displayWord += displayLetter;
            }
            return displayWord;
        }

        // Add correct letter.
        static void AddCorrectLetter(char correctLetter, ref char[] letters)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == ' ')
                {
                    letters[i] = correctLetter;
                    break;
                } 
            }
        }

        // Get a list of guessed letters.
        static string GetGuessedLetters(char[] letters)
        {
            string guessedLetters = "";
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == ' ') break;
                if (i > 0) guessedLetters += ' ';
                guessedLetters += letters[i];
            }
            return guessedLetters;
        }

        static string GetGuessedLetters(StringBuilder letters)
        {
            string guessedLetters = "";
            for (int i = 0; i < letters.Length; i++)
            {
                if (i > 0) guessedLetters += ' ';
                guessedLetters += letters[i];
            }
            return guessedLetters;
        }

        // Already guessed a letter?
        static bool GuessedLetter(char letter, char[] letters)
        {
            bool foundLetter = false;
            for (int i = 0; i < letters.Length; i++)
            {
                if (letter == letters[i])
                {
                    foundLetter = true;
                    break;
                }
            }
            return foundLetter;
        }

        static bool GuessedLetter(char letter, StringBuilder letters)
        {
            bool foundLetter = false;
            for (int i = 0; i < letters.Length; i++)
            {
                if (letter == letters[i])
                {
                    foundLetter = true;
                    break;
                }
            }
            return foundLetter;
        }



        // Main method.
        static void Main()
        {
            // Main loop.
            bool isRunning = true;
            while (isRunning)
            {
                // Game Menu.
                Clear();
                WriteLine("Hangman\n" +
                    "--------\n" + 
                    "[ 1 ] Play game\n" +
                    "[ 0 ] Exit\n");

                // Get player input.
                Write("Select: ");
                string menuChoice = ReadLine().Trim();
                WriteLine("");

                // Handle player input.
                switch (menuChoice)
                {
                    case "0":
                        isRunning = false;
                        break;

                    case "1":
                        PlayGame();
                        break;

                    default:
                        if (menuChoice != "")
                        {
                            Write($"Error: Invalid choice '{menuChoice}'!");
                            ReadKey();
                        }
                        break;

                }
            }
        }



        // Win game.
        static void WinGame()
        {
            Write($"Congratulations! The right word is: {word}");
            ReadKey();
            gameIsRunning = false;
        }

        // Lose game.
        static void LoseGame()
        {
            Write("Game Over! You have made 10 failed attempts.");
            ReadKey();
            gameIsRunning = false;
        }

        // Guess a letter.
        static void GuessLetter()
        {
            string letter = "";
            while (letter.Length != 1)
            {
                Write("Guess a letter: ");
                letter = ReadLine().Trim().ToUpper();
            }
            WriteLine("");
            
            if (!Char.IsLetter(letter[0]))
            {
                Write($"Error: Is not a valid letter '{letter}'!");
                ReadKey();
                return;
            }

            if ((GuessedLetter(letter[0], correctLetters)) || (GuessedLetter(letter[0], incorrectLetters)))
            {
                Write($"Error: The letter has already been guessed '{letter}'!");
                ReadKey();
                return;
            }

            if (word.Contains(letter[0]))
            {
                AddCorrectLetter(letter[0], ref correctLetters);
                if (word == GetDisplayWord(word, correctLetters))
                {
                    WinGame();
                }
            }
            else
            {
                incorrectLetters.Append(letter[0]);
                attempts++;
                if (attempts == 10)
                {
                    LoseGame();
                }
            }
        }

        // Guess a word.
        static void GuessWord()
        {
            string guessWord = "";
             while (guessWord == "")
            {
                Write("Guess a word: ");
                guessWord = ReadLine().Trim().ToUpper();
            }
            WriteLine("");
            if (guessWord == word)
            {
                WinGame();
            }
            else
            {
                attempts++;
                if (attempts == 10)
                {
                    LoseGame();
                }
            }
        }

        // Play game.
        static void PlayGame()
        {
            // Set up game.
            gameIsRunning = true;
            word = GetRandomWord();
            correctLetters = new char[30];
            for (int i = 0; i < 30; i++) correctLetters[i] = ' ';
            incorrectLetters = new StringBuilder("");
            attempts = 0;          

            // Game loop.
            while (gameIsRunning)
            {
                // Game info and game menu.
                Clear();
                WriteLine(GetDisplayWord(word, correctLetters) +
                    "\n\nCorrect letters   : " +
                    GetGuessedLetters(correctLetters) +
                    "\nIncorrect letters : " +
                    GetGuessedLetters(incorrectLetters) +
                    $"\nFailed attempts   : {attempts}" +
                    "\n\nMenu\n----\n" +
                    "[ L ] Guess letter\n" +
                    "[ W ] Guess word\n" +
                    "[ 0 ] Exit\n");
                
                // Get player input.
                Write("Select: ");
                string menuChoice = ReadLine().Trim().ToUpper();
                WriteLine("");

                // Handle player input.
                switch (menuChoice)
                {
                    case "0":
                        gameIsRunning = false;
                        break;

                    case "L":
                        GuessLetter();
                        break;

                    case "W":
                        GuessWord();
                        break;

                    default:
                        if (menuChoice != "")
                        {
                            Write($"Error: Invalid choice '{menuChoice}'!");
                            ReadKey();
                        }
                        break;
                }
            }
        }

    }

}