using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Cornfield.SudokuSolver.Library
{
    public static class ActionRecorder
    {
        public static List<string> Actions { get; set; }
        public static void Init()
        {
            Actions = new List<string>();
        }

        public static void Record(string action)
        {
            Actions.Add(action);
        }
    }
}
