using System;
using System.Collections.Generic;

namespace Kernel.CommandProcessor.Commands
{
    public class CommandResult
    {
        public List<string> SuccessMsg = new List<string>();
        public List<string> ErrorMsg = new List<string>();
        public bool Success { get; set; }
        public Exception Exception;

        public CommandResult()
        {
            Success = true;
        }

        public void AddSuccessMessage(string v)
        {
            SuccessMsg.Add(v);
        }

        public void AddErrorMessage(string v)
        {
            ErrorMsg.Add(v);
            Success = false;
        }
    }
}
