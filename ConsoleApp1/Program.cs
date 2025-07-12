using System.IO.Pipes;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();

            //Listi AI ile yazmisham
            List<Player> players = new List<Player>
            {
                new Player
                {
                    Name = "Elvin",
                    GameRecords = new List<GameRecord>
                    {
                        new GameRecord { Hardness = "Easy", Score = 120, Attempts = 5, Won = true },
                        new GameRecord { Hardness = "Normal", Score = 80, Attempts = 6, Won = false },
                        new GameRecord { Hardness = "Difficult", Score = 150, Attempts = 7, Won = true },
                    }
                },
                new Player
                {
                    Name = "Nigar",
                    GameRecords = new List<GameRecord>
                    {
                        new GameRecord { Hardness = "Easy", Score = 100, Attempts = 4, Won = true },
                        new GameRecord { Hardness = "Normal", Score = 90, Attempts = 5, Won = true },
                    }
                },
                new Player
                {
                    Name = "Rəşad",
                    GameRecords = new List<GameRecord>
                    {
                        new GameRecord { Hardness = "Normal", Score = 70, Attempts = 6, Won = false },
                        new GameRecord { Hardness = "Difficult", Score = 180, Attempts = 8, Won = true },
                        new GameRecord { Hardness = "Difficult", Score = 0, Attempts = 6, Won = false },
                        new GameRecord { Hardness = "Easy", Score = 60, Attempts = 3, Won = true },
                    }
                },
                new Player
                {
                    Name = "Günay",
                    GameRecords = new List<GameRecord>
                    {
                        new GameRecord { Hardness = "Easy", Score = 90, Attempts = 4, Won = true },
                        new GameRecord { Hardness = "Normal", Score = 100, Attempts = 5, Won = true },
                        new GameRecord { Hardness = "Normal", Score = 60, Attempts = 6, Won = false },
                    }
                },
                new Player
                {
                    Name = "Tural",
                    GameRecords = new List<GameRecord>
                    {
                        new GameRecord { Hardness = "Easy", Score = 110, Attempts = 3, Won = true },
                        new GameRecord { Hardness = "Difficult", Score = 0, Attempts = 6, Won = false },
                    }
                }
            };

            Console.WriteLine("🔹🔹🔹 WELCOME TO THE WORLD OF GUESSING OF NUMBERS 🔹🔹🔹\n");


            Console.Write("Enter your name to start the game: ");
            Player loggedPlayer;

            string name;
            while (true)
            {
                name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Name cannot be empty. Please enter a valid name.");
                }
            }

            bool userExist = players.Any(x => x.Name == name);

            if (userExist)
            {
                loggedPlayer = players.FirstOrDefault(x => x.Name == name);
            }
            else
            {
                loggedPlayer = new Player() { Name = name };
                players.Add(loggedPlayer);
            }

            // 🟢 BURDAN ETİBARƏN DAXİL OLDUQDAN SONRA ARTIQ YENİDƏN AD SORUŞULMUR
            while (true)
            {
                int highScore = 0;

                try
                {
                    highScore = loggedPlayer.GameRecords.Max(x => x.Score);
                }
                catch
                {
                    highScore = 0;
                }

                Console.Clear();
                Console.WriteLine("🔹🔹🔹 WELCOME TO THE WORLD OF GUESSING OF NUMBERS 🔹🔹🔹\n");
                Console.WriteLine($"\n Welcome {loggedPlayer.Name}.");
                Console.WriteLine($"Your best score is {highScore}. You played {loggedPlayer.TotalGameCount} games and won {loggedPlayer.WonGames} games!");

                Console.WriteLine("\nPlease choose an option:");
                Console.WriteLine("[1] Start Game");
                Console.WriteLine("[2] Statistics");
                Console.Write("Enter your choice: ");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        Console.Clear();
                        

                        foreach (string opt in Enum.GetNames(typeof(Hardness)))
                        {
                            Console.WriteLine($"- {opt}");
                        }

                        Console.Write("\n Choose the hardness of the game: \n");

                        while (true)
                        {
                            string input = Console.ReadLine();

                            if (Enum.TryParse<Hardness>(input, true, out Hardness hardness))
                            {
                                var results = game.StartGame(loggedPlayer, hardness);

                                GameRecord gameRecord = new GameRecord
                                {
                                    Attempts = results.attempts,
                                    Score = results.score,
                                    Won = results.won,
                                    Hardness = hardness.ToString()
                                };

                                loggedPlayer.GameRecords.Add(gameRecord);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("❌ Invalid input. Please type one of the following options:");
                                foreach (string hrd in Enum.GetNames(typeof(Hardness)))
                                {
                                    Console.WriteLine($"- {hrd}");
                                }
                                Console.Write("Try again: ");
                            }
                        }
                        break;


                    case "2":
                        Console.Clear();
                        Console.WriteLine($"Statistics for {loggedPlayer.Name}:\n");

                        if (loggedPlayer.GameRecords.Count == 0)
                        {
                            Console.WriteLine("No game records found.");
                        }
                        else
                        {
                            Console.WriteLine($"{"Date",-20} | {"Hardness",-12} | {"Score",-5} | {"Attempts",-9} | {"Won",-5}");
                            Console.WriteLine("-----------------------------------------------------------------");
                            foreach (var rec in loggedPlayer.GameRecords)
                            {
                                Console.WriteLine($"{rec.DateTime,-20} | {rec.Hardness,-12} | {rec.Score,-5} | {rec.Attempts,-9} | {(rec.Won ? "Yes" : "No"),-5}");
                            }
                        }

                        Console.WriteLine("\nPress ENTER to go back to menu.");
                        Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("❌ Invalid option. Please try again.");
                        break;
                }
            }
        }
    }

    public class Player
    {
        //Oyunchunun adi
        public string Name { get; set; }

        //Oyunlarinin siyahisi
        public List<GameRecord> GameRecords { get; set; } = [];

        //Butun xallarinin cemi
        private int _totalScore = 0;
        public int TotalScore => GameRecords.Select(x => x.Score).Sum();

        //Butun oyunlarinin cemi
        private int _totalGameCount = 0;
        public int TotalGameCount => GameRecords.Count;

        //Uddugu oyunlarinin cemi
        private int _wonGames = 0;
        public int WonGames => GameRecords.Where(x => x.Won).Count();

        //Uduzdugu oyunlarinin cemi
        private int _lostGames = 0;
        public int LostGames => GameRecords.Where(x => !x.Won).Count();
    }

    public class GameRecord
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Hardness { get; set; }
        public int Attempts { get; set; }
        public int Score { get; set; }
        public bool Won { get; set; }
    }

    public enum Hardness
    {
        Easy,
        Normal,
        Difficult
    }

    public class Game
    {
        public (bool won, int score, int attempts) StartGame(Player player, Hardness hardness)
        {
            //Random classini cagiririq
            var rand = new Random();

            //xali yaradiriq
            int score = 0;

            //total cehd sayini yaradiriq
            int totalAttempts = 0;

            //Xal uchun emsali teyin edirik
            int emsal = 0;
            if (hardness == Hardness.Easy) emsal = 1;
            else if (hardness == Hardness.Normal) emsal = 2;
            else if (hardness == Hardness.Difficult) emsal = 3;

            //Neche can oldugunu teyin edirik
            int lives = 0;
            if (hardness == Hardness.Easy) lives = 5;
            else if (hardness == Hardness.Normal) lives = 4;
            else if (hardness == Hardness.Difficult) lives = 3;

            //Level 1 ucun diapazonu teyin edirik
            int l1RandLimit = 0;
            if (hardness == Hardness.Easy) l1RandLimit = 8;
            else if (hardness == Hardness.Normal) l1RandLimit = 10;
            else if (hardness == Hardness.Difficult) l1RandLimit = 12;

            //Level 2 ucun diapazonu teyin edirik
            int l2RandLimit = 0;
            if (hardness == Hardness.Easy) l2RandLimit = 16;
            else if (hardness == Hardness.Normal) l2RandLimit = 20;
            else if (hardness == Hardness.Difficult) l2RandLimit = 24;

            //Level 3 ucun diapazonu teyin edirik
            int l3RandLimit = 0;
            if (hardness == Hardness.Easy) l3RandLimit = 40;
            else if (hardness == Hardness.Normal) l3RandLimit = 50;
            else if (hardness == Hardness.Difficult) l3RandLimit = 60;

            //Cehd saylarini teyin edirik
            int l1Attempts = 4;
            int l2Attempts = 5;
            int l3Attempts = 6;


            //LEVEL 1 bashlayir
            Console.Clear();
            Console.WriteLine($"LEVEL 1 STARTED - {hardness.ToString()}");
            Console.WriteLine($"Lives: {lives} | Score: {score}");
            Console.WriteLine();

            int level1Number = rand.Next(1, l1RandLimit);
            int tries = 0;

            while (true)
            {
                if (lives == 0)
                {
                    Console.WriteLine($"YOU LOST THE GAME. Score earned: {score}");

                    return (false, score, totalAttempts);
                }

                if (tries == 4)
                {
                    lives--;
                    Console.WriteLine();
                    Console.WriteLine("You LOST 1 heart.");
                    Console.WriteLine($"The number was: {level1Number}");
                    Console.WriteLine($"Lives: {lives} | Score: {score}");
                    Console.WriteLine();
                    tries = 0;
                    l1Attempts = 4;
                    level1Number = rand.Next(1, l1RandLimit);

                    continue;
                }

                Console.WriteLine();
                Console.Write($"Attempt {tries + 1}/{l1Attempts + tries}: ");
                //int input = int.Parse(Console.ReadLine());

                int input;
                while (true)
                {
                    string userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out input))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("❌ Only integer numbers are allowed. Please try again.");
                    }
                }


                if (input == level1Number)
                {
                    int earnedScore = l1Attempts * emsal * 10;
                    score += earnedScore;
                    Console.WriteLine();
                    Console.WriteLine("You guessed the number!");
                    Console.WriteLine("LEVEL 1 Completed!");
                    Console.WriteLine($"You earned {earnedScore} score.");
                    Console.WriteLine($"Total score is {score}.");

                    totalAttempts++;
                    tries = 0;
                    break;
                }
                else if (input > level1Number)
                {
                    //Console.WriteLine();
                    if (tries < 3) Console.WriteLine("Try less number");
                    l1Attempts--;
                    tries++;
                    totalAttempts++;
                }
                else if (input < level1Number)
                {
                    //Console.WriteLine();
                    if (tries < 3) Console.WriteLine("Try bigger number");
                    l1Attempts--;
                    tries++;
                    totalAttempts++;
                }

                if (Math.Abs(input - level1Number) <= 3)
                {
                    if (tries < 4) Console.WriteLine("❤️ Too close to the number!");
                }

            }

            Console.WriteLine();
            Console.Write("Do you want to continue? (y/n): ");
            string cntCmnd = Console.ReadLine().ToLower();

            if (cntCmnd == "n") return (true, score, totalAttempts);

            //LEVEL 2 bashlayir
            Console.Clear();
            Console.WriteLine($"LEVEL 2 STARTED - {hardness.ToString()}");
            Console.WriteLine($"Lives: {lives} | Score: {score}");
            Console.WriteLine();

            int level2Number = rand.Next(1, l2RandLimit);
            tries = 0;

            while (true)
            {
                if (lives == 0)
                {
                    Console.WriteLine($"YOU LOST THE GAME. Score earned: {score}");

                    return (false, score, totalAttempts);
                }

                if (tries == 5)
                {
                    lives--;
                    Console.WriteLine();
                    Console.WriteLine("You LOST 1 heart.");
                    Console.WriteLine($"The number was: {level2Number}");
                    Console.WriteLine($"Lives: {lives} | Score: {score}");
                    Console.WriteLine();
                    tries = 0;
                    l2Attempts = 5;
                    level2Number = rand.Next(1, l2RandLimit);

                    continue;
                }

                Console.WriteLine();
                Console.Write($"Attempt {tries + 1}/{l2Attempts + tries}: ");

                //int input = int.Parse(Console.ReadLine());

                int input;
                while (true)
                {
                    string userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out input))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("❌ Only integer numbers are allowed. Please try again.");
                    }
                }


                if (input == level2Number)
                {
                    int earnedScore = l2Attempts * emsal * 10;
                    score += earnedScore;
                    Console.WriteLine();
                    Console.WriteLine("You guessed the number!");
                    Console.WriteLine("LEVEL 2 Completed!");
                    Console.WriteLine($"You earned {earnedScore} score.");
                    Console.WriteLine($"Total score is {score}.");

                    totalAttempts++;
                    tries = 0;
                    break;
                }
                else if (input > level2Number)
                {
                    //Console.WriteLine();
                    if (tries < 4) Console.WriteLine("Try less number");
                    l2Attempts--;
                    tries++;
                    totalAttempts++;
                }
                else if (input < level2Number)
                {
                    //Console.WriteLine();
                    if (tries < 4) Console.WriteLine("Try bigger number");
                    l2Attempts--;
                    tries++;
                    totalAttempts++;
                }

                if (Math.Abs(input - level2Number) <= 3)
                {
                    if (tries < 5) Console.WriteLine("❤️ Too close to the number!");
                }

            }

            Console.WriteLine();
            Console.Write("Do you want to continue? (y/n): ");
            cntCmnd = Console.ReadLine().ToLower();

            if (cntCmnd == "n") return (true, score, totalAttempts);

            //LEVEL 3 bashlayir
            Console.Clear();
            Console.WriteLine($"LEVEL 3 STARTED - {hardness.ToString()}");
            Console.WriteLine($"Lives: {lives} | Score: {score}");
            Console.WriteLine();

            int level3Number = rand.Next(1, l3RandLimit);
            tries = 0;

            while (true)
            {
                if (lives == 0)
                {
                    Console.WriteLine($"YOU LOST THE GAME. Score earned: {score}");

                    return (false, score, totalAttempts);
                }

                if (tries == 6)
                {
                    lives--;
                    Console.WriteLine();
                    Console.WriteLine("You LOST 1 heart.");
                    Console.WriteLine($"The number was: {level3Number}");
                    Console.WriteLine($"Lives: {lives} | Score: {score}");
                    Console.WriteLine();
                    tries = 0;
                    l3Attempts = 6;
                    level3Number = rand.Next(1, l3RandLimit);

                    continue;
                }

                Console.WriteLine();
                Console.Write($"Attempt {tries + 1}/{l3Attempts + tries}: ");

                //int input = int.Parse(Console.ReadLine());

                int input;
                while (true)
                {
                    Console.Write("Enter a number: ");
                    string userInput = Console.ReadLine();

                    if (int.TryParse(userInput, out input))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("❌ Only integer numbers are allowed. Please try again.");
                    }
                }


                if (input == level3Number)
                {
                    totalAttempts++;
                    int earnedScore = l3Attempts * emsal * 10;
                    score += earnedScore;
                    Console.Clear();
                    Console.WriteLine("You guessed the number!");
                    Console.WriteLine("LEVEL 3 Completed!");
                    Console.WriteLine($"You earned {earnedScore} score.");
                    Console.WriteLine();
                    Console.WriteLine($"Game is finished in {totalAttempts} attempts.");
                    Console.WriteLine($"Total score is {score}.");

                    Console.WriteLine();
                    Console.WriteLine("Tap ENTER to continue");
                    string command = Console.ReadLine();
                    if (command != "uywoiehfjksdkjfhsdjhkgfjshf") command = "defe";
                    tries = 0;
                    return (true, score, totalAttempts);
                }
                else if (input > level3Number)
                {
                    //Console.WriteLine();
                    if (tries < 5) Console.WriteLine("Try less number");
                    l3Attempts--;
                    tries++;
                    totalAttempts++;
                }
                else if (input < level3Number)
                {
                    //Console.WriteLine();
                    if (tries < 5) Console.WriteLine("Try bigger number");
                    l3Attempts--;
                    tries++;
                    totalAttempts++;
                }

                if (Math.Abs(input - level3Number) <= 3)
                {
                    if (tries < 6) Console.WriteLine("❤️ Too close to the number!");
                }

            }

            return (false, score, totalAttempts);
        }
    }


}
