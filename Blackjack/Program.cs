using System;
using System.Threading.Channels;

namespace MyApp
{
    enum PlayerChoice
    {
        None,
        Hit,
        Call
    }

    internal class Program
    {
        static bool startFinished = false;
        static int handTotal = 0;
        static int cardsTotal = 0;
        static int[] dealer = new int[21];
        static int[] hand = new int[5];
        static Random rng = new Random();
        static bool gameOver = false;
        static int milliseconds = 1500;
        static int dealerTotal = 0;

        static void Main(string[] args)
        {
            bool playAgain = true;
            while (playAgain)
            {
                ResetGame();
                Start();
                Spaces();
                Console.WriteLine("Do you want to play again? (yes/no)");
                string response = Console.ReadLine().ToLower();
                playAgain = response == "yes";
            }
            Console.WriteLine("Press any key to close the window.");
            Console.ReadKey();
        }
        static void ResetGame()
        {
            startFinished = false;
            handTotal = 0;
            cardsTotal = 0;
            dealer = new int[21];
            hand = new int[5];
            gameOver = false;
            dealerTotal = 0;
        }

        static void Hit()
        {
            hand[cardsTotal] = rng.Next(1, 14);
            if (hand[cardsTotal] > 10)
            {
                hand[cardsTotal] = 10;
            }
            handTotal += hand[cardsTotal];
            Console.WriteLine(hand[cardsTotal]);
            cardsTotal++;
        }

        static void Call()
        {
            Console.WriteLine("Your total is " + handTotal + "\nThe dealer will now draw cards.\n");
            Thread.Sleep(milliseconds);
            for (int f = 0; dealerTotal <= handTotal; f++)
            {
                dealer[f] = rng.Next(1, 14);
                if (dealer[f] > 10)
                {
                    dealer[f] = 10;
                }
                dealerTotal += dealer[f];
                Thread.Sleep(milliseconds);
                Console.WriteLine(dealer[f]);
            }
        }

        static PlayerChoice GetPlayerChoice()
        {
            PlayerChoice playerChoice = PlayerChoice.None;
            while (playerChoice == PlayerChoice.None)
            {
                Console.Write("\nHIT or CALL?\n ");
                string choice = Console.ReadLine().ToLower();
                if (choice == "hit")
                {
                    playerChoice = PlayerChoice.Hit;
                }
                else if (choice == "call")
                {
                    playerChoice = PlayerChoice.Call;
                }
                else
                {
                    Console.WriteLine("Try Again");
                }
            }

            return playerChoice;
        }

        static void CheckHitResult()
        {
            if (cardsTotal == 5 && handTotal <= 21)
            {
                Spaces();
                Console.WriteLine("YOU WIN!\nYou managed to draw five cards without busting.\n");
                gameOver = true;
            }
            else if (handTotal > 21)
            {
                Spaces();
                Console.WriteLine("\nGAME OVER!\nYou busted!\n");
                gameOver = true;
            }
            else if (handTotal == 21)
            {
                Spaces();
                Console.WriteLine("\nBLACKJACK! YOU WIN!\n");
                gameOver = true;
            }
        }

        static void CheckCallResult()
        {
            Console.WriteLine("Dealer total is " + dealerTotal + ".");
            if (dealerTotal > 21)
            {
                Thread.Sleep(milliseconds);
                Spaces();
                Console.WriteLine("YOU WIN!\nDealer busts!");
            }
            else if (handTotal > dealerTotal)
            {
                Thread.Sleep(milliseconds);
                Spaces();
                Console.WriteLine("YOU WIN!\nYou are closer!");
            }
            else
            {
                Thread.Sleep(milliseconds);
                Spaces();
                Console.WriteLine("GAME OVER.\nDealer is closer.");
            }
            gameOver = true;
        }

        static void Start()
        {
            Console.WriteLine("Welcome to Blackjack!\n-------------------------\nPress any key to continue.");
            Console.ReadKey();
            Console.WriteLine("You will now be dealt two cards");
            Spaces();
            Hit();
            Hit();

            while (!gameOver)
            {
                Console.WriteLine($"Your total is now {handTotal}");
                var choice = GetPlayerChoice();
                switch (choice)
                {
                    case PlayerChoice.Hit:
                        Hit();
                        CheckHitResult();
                        break;
                    case PlayerChoice.Call:
                        Call();
                        CheckCallResult();
                        break;
                }
            }
        }



        static void Spaces()
        {
            Console.WriteLine("-------------------------");
        }
    }
}