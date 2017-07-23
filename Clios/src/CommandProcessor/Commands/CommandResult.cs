using System;
using System.Collections.Generic;

namespace Clios.CommandProcessor.Commands
{
    public class CommandResult
    {
        public List<string> SuccessMsg = new List<string>();
        public List<string> ErrorMsg = new List<string>();
        public bool Success { get; set; }
        public Exception Exception;
        public bool ClearScreen = false;

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
