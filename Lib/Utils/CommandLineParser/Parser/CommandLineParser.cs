﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Utils.CommandLineParser.Parser
{
    public class CommandLineParser
    {
        /// <summary>
        /// Help aliases
        /// </summary>
        public static readonly List<string> HelpWords = new List<string>() { "-?", "-h", "-help", "--help" };

        /// <summary>
        /// Parser arguments by defined commands
        /// </summary>
        /// <param name="args">Arguments</param>
        /// <param name="commands">Commands</param>
        /// <returns>Command</returns>
        public static CommandLineCommand Parse(string[] args, List<CommandLineCommand> commands)
        {
            CommandLineCommand command;
            string[] commandArgs = args;

            // first argument
            string firstArg = args?.Length > 0 ? args[0].Trim() : null;
            if (firstArg != null)
            {
                // help
                if (HelpWords.Contains(firstArg))
                {
                    ShowHelp(commands);
                    return null;
                }

                // get the command by the first argument
                command = commands?.FirstOrDefault(c => c.Words?.Contains(firstArg) ?? false);

                // remove first argument
                commandArgs = new string[args.Length - 1];
                Array.Copy(sourceArray: args, sourceIndex: 1, destinationArray: commandArgs, destinationIndex: 0, length: commandArgs.Length);
            }
            else
            {
                // command without parameters
                command = commands?.FirstOrDefault(c => c.Words == null || c.Words.Contains(""));
            }

            // Parse arguments or show help
            if (command == null)
                ShowHelp(commands: commands);
            else if (commandArgs?.Length > 0)
                command = command.ParseArguments(commandArgs);

            return command;
        }

        /// <summary>
        /// Show help of command
        /// </summary>
        /// <param name="commands">Registered commands</param>
        private static void ShowHelp(List<CommandLineCommand> commands)
        {
            commands.ForEach(command =>
            {
                command.ShowHelp();
                Console.WriteLine();
            });
        }
    }
}