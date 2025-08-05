using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Bullet
    {
        PictureBox bullet;
        private const int HEIGHT = 20;
        private const int LENGTH = 5;
        private const int SPEED = 10;
        private Color col = Color.Red;

        /// <summary>
        /// posx -> bomo dobili od playerja
        /// posy -> bomo dobili od playerja
        /// da vemo, kam postaviti bullet
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        public Bullet(int posx, int posy, Color col, Control.ControlCollection controls)
        {
            this.col = col;
            this.bullet = new PictureBox();
            bullet.Size = new Size(LENGTH, HEIGHT);
            bullet.BackColor = col;
            bullet.Location = new Point(posx,posy);
            controls.Add(bullet);
        }

        public void Move()
        {
            bullet.Top -= SPEED;
        }

        public void Move_enemy()
        {
            bullet.Top += SPEED;
        }

        //to je samo za zgoraj
        public bool Off_screen()
        {
            return bullet.Top < 0;
        }

        public bool Off_screen_bottom(int window_height)
        {
            return bullet.Top > window_height;
        }

        public void Destroy_bullet(Control.ControlCollection controls)
        {
            controls.Remove(bullet);
            bullet.Dispose();
        }

        public Rectangle GetBounds()
        {
            return bullet.Bounds;
        }
    }
}
