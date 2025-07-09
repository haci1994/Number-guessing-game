using System.IO.Pipes;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            game.StartGame(null, Hardness.Normal);


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


                    GameRecord game = new GameRecord();
                    game.Attempts = totalAttempts;
                    game.Score = score;
                    game.Hardness = hardness.ToString();
                    game.Won = true;

                    player.GameRecords.Add(game);

                    break;
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
                Console.Write($"Attempt {tries + 1}: ");
                int input = int.Parse(Console.ReadLine());

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

            if(cntCmnd == "n") return (true, score, totalAttempts);




            return (true, 0, 0);           

        }

        
    }
}
