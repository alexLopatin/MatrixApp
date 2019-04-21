using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachSAS.Command
{
    //пример паттерна singleton - одиночки
    public class CommandInvoker
    {
        private Stack<ICommand> commandHistory;
        private static CommandInvoker instance;
        ICommand command;
        private CommandInvoker()
        {
            commandHistory = new Stack<ICommand>();
        }
        public static CommandInvoker getInstance()
        {
            if (instance == null)
                instance = new CommandInvoker();
            return instance;
        }
        public void SetCommand(ICommand c)
        {
            commandHistory.Push(c);

            command = c;
            if(commandHistory.Count==2)
                commandHistory.Pop();
        }
        public void Run()
        {
            command.Execute();
        }
        public void Cancel()
        {
            if (commandHistory.Count == 0)
                return;
                var comm = commandHistory.Pop();
                if (comm != null)
                    comm.Undo();
            
        }
    }
}
