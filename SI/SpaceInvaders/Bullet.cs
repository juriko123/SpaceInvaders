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
        private const int SIZE = 10;
        private const int SPEED = 10;

        /// <summary>
        /// posx -> bomo dobili od playerja
        /// posy -> bomo dobili od playerja
        /// da vemo, kam postaviti bullet
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        public Bullet(int posx, int posy, Control.ControlCollection controls)
        {
            this.bullet = new PictureBox();
            bullet.Size = new Size(SIZE, SIZE);
            bullet.BackColor = Color.Red;
            bullet.Location = new Point(posx,posy);
            controls.Add(bullet);
        }

        public void Move()
        {
            bullet.Top -= SPEED;
        }

        public bool Off_screen()
        {
            return bullet.Top < 0;
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
