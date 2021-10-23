using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchPointsFarmer.Models;
using TwitchPointsFarmer.Utils.Exceptions;

namespace TwitchPointsFarmer.Utils
{
    public class CommandParsing
    {
        public List<Command> Commands { get; set; }
        public CommandParsing(IEnumerable<Command> commands)
        {
            if (commands == null)
            {
                commands = new List<Command>();
            }
            Commands = commands.ToList();
        }
        public void Parse(string input)
        {
            string[] args = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            bool isfound = false;
            Command cmd = null;
            Commands.ForEach(c => {
                if (c.Label == args[0] || c.Aliases.Contains(args[0]))
                {
                    isfound = true;
                    cmd = c;
                }
            });
            if (!isfound) throw new CommandNotFoundException("Command not found!");
            //execute command
            if (cmd.SubCommands == null || !cmd.SubCommands.Any())
            {
                //has args
                if (cmd.HasUserArgs)
                {
                    if (cmd.NumberOfParameters != -1)
                    {
                        if (args.Length-1 > cmd.NumberOfParameters) throw new TooManyArgsException();
                        if (args.Length - 1 < cmd.NumberOfParameters) throw new TooFewArgsException();
                    }
                    //has the correct number of args
                    var temp = args.ToList();
                    temp.RemoveAt(0);

                    //executes the method
                    cmd.Action(temp.ToArray());
                }else{
                    //cmd doesnt have args
                    cmd.Action(Array.Empty<object>());
                }
            }
            else
            {
                //has subcommands
                Parse(args);
            }
        }
        public void Parse(string[] args)
        {
            Parse(string.Join(' ', args));
        }

    }
}
