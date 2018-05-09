using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public partial class Form1 : Form
    {
        enum Mode
        {
            Draw,
            Move,
            Delete
        }

        public Caretaker Caretaker { get; set; }
        public event Action StateChanged;
        private Memento _lastMemento;

        FigurePrototype _protoFigure;
        List<Panel> _figures = new List<Panel>();

        Mode _mode = Mode.Move;

        public Form1()
        {
            InitializeComponent();
            this.AllowDrop = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _protoFigure = FigurePrototype.Square;
            _mode = Mode.Draw;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _protoFigure = FigurePrototype.Rectangle;
            _mode = Mode.Draw;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _protoFigure = FigurePrototype.Circle;
            _mode = Mode.Draw;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if(_mode == Mode.Draw)
            {
                FigurePrototype f = _protoFigure.Clone();
                f.Figure.MouseDown += (o, ev) =>
                {
                    if (_mode == Mode.Move)
                    {
                        f.Figure.DoDragDrop(f, DragDropEffects.Move);                        
                    }
                    else if (_mode == Mode.Delete)
                    {
                        f.Figure.Parent.Controls.Remove(f.Figure);
                        _lastMemento = new Memento(ActionType.Delete)
                        {
                            Panel = f.Figure
                        };
                        StateChanged?.Invoke();
                       
                    }
                };
                f.Figure.Location = e.Location;
                this.Controls.Add(f.Figure);

                _lastMemento = new Memento(ActionType.Create)
                {
                    Panel = f.Figure
                };
                StateChanged?.Invoke();
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            FigurePrototype f = e.Data.GetData(typeof(FigurePrototype)) as FigurePrototype;
            Panel o = f.Figure;
            Point oldLocation = o.Location;

            if(o != null)
            {
                o.Location = this.PointToClient(new Point(e.X, e.Y));
            }
            _lastMemento = new Memento(ActionType.Move)
            {
                OldLocation = oldLocation,
                NewLocation = o.Location,
                Panel = o
            };
            StateChanged?.Invoke();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            _mode = Mode.Move;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            _mode = Mode.Delete;
        }

        public void RestoreMemento(Memento m, bool Redo=false)
        {
            switch(m.ActionType)
            {
                case ActionType.Create:
                    if(Redo)
                    {
                        this.Controls.Add(m.Panel);
                    }
                    else
                    {
                        this.Controls.Remove(m.Panel);
                    }
                    break;
                case ActionType.Delete:
                    if(Redo)
                    {
                        this.Controls.Remove(m.Panel);
                    }
                    else
                    {
                        this.Controls.Add(m.Panel);
                    }
                    break;
                case ActionType.Move:
                    if(Redo)
                    {
                        m.Panel.Location = m.NewLocation;
                    }
                    else
                    {
                        m.Panel.Location = m.OldLocation;
                    }
                    break;
            }
        }

        public Memento CreateMemento()
        {
            return _lastMemento;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if(Caretaker != null)
            {
                Caretaker.Undo();
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (Caretaker != null)
            {
                Caretaker.Redo();
            }
        }
    }
}
