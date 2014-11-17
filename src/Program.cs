using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace OCStatScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Instructions for use:\n\n- Spell everything correctly or the program will not work\n- Case does not matter\n- If you see something in parentheses EG: (PA) it means that you can type that \ninstead, excluding the parentheses.\n- You must fill everything in!\n\nPress enter to continue!");
            Console.ReadLine();
            //Handle usernames
            Console.Clear();
            Console.Write("Please enter your username: ");
            String user = Console.ReadLine();
            user = user.ToLower().Trim();

            //Check name against basic conditions to find out if it's a real name
            if (user == "" || user.Contains(" ") || user.Length > 16)
            {
                Console.WriteLine("--------\nPlease enter a valid username.");
                Console.Read();
                Environment.Exit(1);
            }


            //Get type of stats
            Console.Clear();
            Console.Write("Please select from the following types of stats:\n\n- Kills\n- Deaths\n- Playtime\n- Cores\n- Wools\n- Monuments\n- KD\n- KK\n\nEnter your choice: ");
            String type = Console.ReadLine();

            //Check stat input against basic conditions
            if(type.Trim() == "" || type.Trim().Contains(" "))
            {
                Console.WriteLine("--------\nPlease enter a valid option.");
                Console.Read();
                Environment.Exit(1);
            }

            //Get timeframe of stats
            Console.Clear();
            Console.Write("Please select the time frame you wish to scan in, your options are as follows.\n\n- Day\n- Week\n- Ever\n\nPlease enter your choice: ");
            String timeFrame = Console.ReadLine();
            timeFrame = timeFrame.ToLower().Trim();

            //Check against basic conditions
            if(timeFrame.Trim() == "" || timeFrame.Trim().Contains(" "))
            {
                Console.WriteLine("--------\nPlease enter a valid option.");
                Console.Read();
                Environment.Exit(1);
            }

            //Get gamemode of stats
            Console.Clear();
            Console.Write("Please select the gamemode you wish to scan in, your options are as follows.\n\n- Blitz\n- Project Ares(PA)\n- Ghost Squadron(GS)\n- All\n\nEnter your choice: ");
            String gm = Console.ReadLine();
            gm = gm.ToLower().Trim();

            //Copy to clipboard?
            Console.Clear();
            Console.Write("Would you like to copy the url of the page we locate you on to your clipboard?\nyes/no: ");
            String toCopy = Console.ReadLine();
            toCopy = toCopy.ToLower().Trim();

            //Start the switch statements

            String statType = null;
            //Find out what the leaderboard input is

            switch (type)
            {
                case "kills":
                    statType = "kills";
                    break;

                case "deaths":
                    statType = "deaths";
                    break;

                case "wools":
                    statType = "wool_placed";
                    break;

                case "monuments":
                    statType = "destroyables_destroyed";
                    break;

                case "cores":
                    statType = "cores_leaked";
                    break;

                case"kd":
                    statType = "kd";
                    break;

                case"kk":
                    statType = "kk";
                    break;

                case"playtime":
                    statType = "playing_time";
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("You provided and invalid stat type (1)");
                    Console.ReadLine();
                    Environment.Exit(1);
                    break;
            }

            String timeFrameInput = null;
            //Find out what the timeframe input is
            switch (timeFrame)
            { 
                case "day":
                    timeFrameInput = "day";
                    break;

                case "week":
                    timeFrameInput = "week";
                    break;

                case "ever":
                    timeFrameInput = "eternity";
                    break;
                
                default:
                    Console.Clear();
                    Console.WriteLine("You provided an invalid timeframe (2)");
                    Console.ReadLine();
                    Environment.Exit(1);
                    break;
            }

            //Find out what the gamemode input is
            String gamemode = null;
            switch (gm)
            { 
                case "blitz":
                    gamemode = "blitz";
                    break;

                case "project ares":
                    gamemode = "projectares";
                    break;

                case "pa":
                    gamemode = "projectares";
                    break;

                case "ghost squadron":
                    gamemode = "ghostsquadron";
                    break;

                case "gs":
                    gamemode = "ghostsquadron";
                    break;

                case "all":
                    gamemode = "all";
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("You provided and invalid gamemode (3)");
                    Console.ReadLine();
                    Environment.Exit(1);
                    break;
            }

            //Copy to clipboard?
            bool yesNo = false;

            switch (toCopy)
            {
                case "yes":
                    yesNo = true;
                    break;

                case "no":
                    yesNo = false;
                    break;

                default:
                    Console.Clear();
                    Console.Write("You provided and invalid arguement (4)");
                    Console.ReadLine();
                    Environment.Exit(1);
                    break;

            }

            Console.WriteLine("\n");
            String scanningUrl = "https://oc.tc/stats?game=" + gamemode + "&page=";
            String secondScanningUrl = "&sort="+ statType +"&time=" + timeFrameInput;
            Console.WriteLine(scanningUrl+1+secondScanningUrl+" will be scanned for your name");
            Console.ReadLine();
            Console.Clear();

            scrape(user,scanningUrl, secondScanningUrl, yesNo);
        }
        

        //Scrapping code
        public static void scrape(String searchName, String firstUrl, String secondUrl, bool copyFinalUrl) 
        {
            int page = 1;
            int userNum = 1;
            bool isDone = false;
            while (isDone == false)
            {
                String url = firstUrl + page + secondUrl;
                HtmlWeb stats = new HtmlWeb();
                HtmlDocument doc = stats.Load(url);
                System.Threading.Thread.Sleep(1000);
                foreach (HtmlNode name in doc.DocumentNode.SelectNodes("//img[@title]"))
                {
                    HtmlAttribute att = name.Attributes["title"];
                    String curName = att.Value.ToString().ToLower().Trim(); 
                    Console.WriteLine("#"+userNum+" "+att.Value.ToString());
                    if (curName == searchName)
                    {
                        Console.WriteLine("You are currently number " + userNum + "\nin the field you entered!\nPlease visit\n" + url + "\nto learn more!");
                        Console.ReadLine();
                        Console.WriteLine("Warning if you press enter again the program will be closed!");
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                    userNum++;
                }
                page++;
            }
         }
    }
}
