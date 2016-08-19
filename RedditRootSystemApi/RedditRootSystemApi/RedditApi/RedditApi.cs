using RedditSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;

namespace RedditRootSystemApi.RedditApi
{
    public class RedditApi
    {
        public string UserReddit { get; set; }
        public string PassReddit { get; set; }

        public void OpenReddit()
        {
            Reddit reddit = null;
            var authenticated = false;
            while (!authenticated)
            {
                //Console.Write("OAuth? (y/n) [n]: ");
                var oaChoice = "";//Falta AuthConsole.ReadLine();
                if (!string.IsNullOrEmpty(oaChoice) && oaChoice.ToLower()[0] == 'y')
                {
                    //Console.Write("OAuth token: ");
                    var token = "";//Falta el Token Console.ReadLine();
                    reddit = new Reddit(token);
                    reddit.InitOrUpdateUser();
                    authenticated = reddit.User != null;
                    if (!authenticated)
                    {
                        //Console.WriteLine("Invalid token");

                    }
                }
                else
                {
                    //Console.Write("Username: ");
                    var username = UserReddit;
                    //Console.Write("Password: ");
                    var password = PassReddit;
                    try
                    {
                        //Console.WriteLine("Logging in...");
                        reddit = new Reddit(username, password);
                        authenticated = reddit.User != null;
                    }
                    catch (AuthenticationException)
                    {
                        Console.WriteLine("Incorrect login.");
                        authenticated = false;
                    }
                }
            }
            
        }


        public static string ReadPassword()
        {
            var passbits = new Stack<string>();
            //keep reading
            for (ConsoleKeyInfo cki = Console.ReadKey(true); cki.Key != ConsoleKey.Enter; cki = Console.ReadKey(true))
            {
                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (passbits.Count() > 0)
                    {
                        //rollback the cursor and write a space so it looks backspaced to the user
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        passbits.Pop();
                    }
                }
                else
                {
                    Console.Write("*");
                    passbits.Push(cki.KeyChar.ToString());
                }
            }
            string[] pass = passbits.ToArray();
            Array.Reverse(pass);
            Console.Write(Environment.NewLine);
            return string.Join(string.Empty, pass);
        }
    }
}