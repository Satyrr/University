using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public enum ActionType
    {
        Create,
        Move,
        Delete
    }
    public class Memento
    {
        public ActionType ActionType;
        public Panel Panel { get; set; }
        public Point OldLocation { get; set; }
        public Point NewLocation { get; set; }

        public Memento(ActionType action)
        {
            this.ActionType = action;
        }
    }
}
