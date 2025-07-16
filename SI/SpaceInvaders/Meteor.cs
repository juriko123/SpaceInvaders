using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    internal class Meteor
    {
        Random rand = new Random();
        private PictureBox meteor;
        private const int SIZE = 30;
        private const int METEOR_SPEED = 10;

        public Meteor(Control.ControlCollection controls,int window_width)
        {
            meteor = new PictureBox();
            meteor.Size = new Size(SIZE, SIZE);
            meteor.BackColor = Color.Green;
            meteor.Location = new Point(rand.Next(0, (window_width - SIZE)), -SIZE);
            controls.Add(meteor);
        }

        public void Move()
        {
            meteor.Top += METEOR_SPEED;
        }

        public bool Off_screen(int window_height)
        {
            return meteor.Top > window_height;
        }

        public void Destroy_meteor(Control.ControlCollection controls)
        {
            controls.Remove(meteor);
            meteor.Dispose();
        }

        public Rectangle GetBounds()
        {
            return meteor.Bounds;
        }
    }
}
