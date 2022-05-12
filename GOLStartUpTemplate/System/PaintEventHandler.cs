using System.Windows.Forms;

namespace System
{
    internal class PaintEventHandler
    {
        private Action<object, PaintEventArgs> neighborCountToolStripMenuItem_Click;

        public PaintEventHandler(Action<object, PaintEventArgs> neighborCountToolStripMenuItem_Click)
        {
            this.neighborCountToolStripMenuItem_Click = neighborCountToolStripMenuItem_Click;
        }
    }
}