using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    internal class Boss1
    {
        private PictureBox boss1;
        private const int LENGTH_SIZE = 120;
        private const int HEIGHT_SIZE = 50;
        private const int HEIGHT_POS = 50;
        private const int SPEED = 20;
        private bool edge;
        private int DIRECTION = 1; //desno, -1 levo

        public Boss1(Control.ControlCollection controls, int posX)
        {
            boss1 = new PictureBox();
            boss1.Size = new Size(LENGTH_SIZE, HEIGHT_SIZE);
            boss1.Location = new Point(posX, HEIGHT_POS);
            boss1.BackColor = Color.Purple;

            controls.Add(boss1);
        }

        public void Boss1Movement(int windowidth)
        {
            if (boss1.Right >= windowidth || boss1.Left <= 0)
            {
                DIRECTION *= -1;
            }

            boss1.Left += SPEED * DIRECTION;
            
        }

        public int GetLeft()
        {
            return boss1.Left;
        }

        public int GetBottom()
        {
            return boss1.Bottom;
        }

        public int GetWidth()
        {
            return boss1.Width;
        }

        public int GetRight()
        {
            return boss1.Right;
        }
    }
}
