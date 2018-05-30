using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public class Caretaker
    {
        Stack<Memento> undoStack = new Stack<Memento>();
        Stack<Memento> redoStack = new Stack<Memento>();
        private Form1 originator;

        public Caretaker(Form1 o)
        {
            this.originator = o;
            this.originator.StateChanged += OriginatorStateChanged;
        }

        public void Undo()
        {
            if (this.undoStack.Count > 0)
            {
                // bieżący stan na redo
                Memento m = undoStack.Pop();
                redoStack.Push(m);

                //Memento ps = undoStack.Peek();
                this.originator.RestoreMemento(m);
            }
        }

        public void Redo()
        {
            if (this.redoStack.Count > 0)
            {
                Memento m = redoStack.Pop();
                undoStack.Push(m);
                this.originator.RestoreMemento(m, Redo:true);
            }
        }

        public void OriginatorStateChanged()
        {
            redoStack.Clear();

            Memento m = this.originator.CreateMemento();
            undoStack.Push(m);
        }
    }
}
