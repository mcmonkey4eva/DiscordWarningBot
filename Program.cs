using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.Net;
using Discord;
using Discord.WebSocket;
using System.Diagnostics;
using FreneticUtilities.FreneticExtensions;
using FreneticUtilities.FreneticDataSyntax;

namespace ModBot
{
    /// <summary>
    /// General program entry and handler.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The current bot object (the instance will change if the bot is restarted).
        /// </summary>
        public static DiscordModBot CurrentBot = null;

        /// <summary>
        /// Software entry point - starts the bot.
        /// </summary>
        static void Main(string[] args)
        {
            CurrentBot = new DiscordModBot();
            LaunchBotThread(args);
            while (true)
            {
                string read = Console.ReadLine();
                string[] commandDataSplit = read.Split(new char[] { ' ' }, 2);
                string commandName = commandDataSplit[0].ToLowerInvariant();
                if (commandName == "quit" || commandName == "stop" || commandName == "exit")
                {
                    CurrentBot.Shutdown();
                    Environment.Exit(0);
                }
                // Could have more commands and/or a more advanced console command handler here.
            }
        }

        /// <summary>
        /// Launches a bot thread.
        /// </summary>
        public static void LaunchBotThread(string[] args)
        {
            Thread thr = new Thread(new ParameterizedThreadStart(BotThread));
            thr.Name = "discordmodbot" + new Random().Next(5000);
            thr.Start(args);
        }

        /// <summary>
        /// The bot thread rootmost method, takes a string array object as input.
        /// </summary>
        public static void BotThread(Object obj)
        {
            try
            {
                CurrentBot.InitAndRun(obj as string[]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Discord crash: " + ex.ToString());
                Thread.Sleep(10 * 1000);
                Thread.CurrentThread.Name = "discordbotthread_dead" + new Random().Next(5000);
                LaunchBotThread(new string[0]);
            }
        }
    }
}
