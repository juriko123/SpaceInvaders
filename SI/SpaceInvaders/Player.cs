using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Player
    {
        private PictureBox battleship;
        private const int SIZE = 40;
        private const int SPEED = 10;
        private string path = "C:\\Users\\jurij\\Desktop\\SpaceI\\SI\\images\\player.png";

        public Player(Control.ControlCollection controls ,int lengthWindow, int heightWindow)
        {
            this.battleship = new PictureBox();
            battleship.Size = new Size(SIZE, SIZE);
            battleship.Image = Image.FromFile(path);
            battleship.SizeMode = PictureBoxSizeMode.StretchImage;
            //battleship.BackColor = Color.Transparent;
            battleship.Location = new Point((lengthWindow - SIZE) / 2, heightWindow - SIZE);
            controls.Add(battleship);
        }

        public void Move_Right(int lengthWindow)
        {
            if (battleship.Right < lengthWindow)
            {
                battleship.Left += SPEED;
            }
        }

        public void Move_left()
        {
            if (battleship.Left > 0)
            {
                battleship.Left -= SPEED;
            }
        }

        public int Left()
        {
            return battleship.Left;
        }

        public int Width()
        {
            return battleship.Width;
        }

        public int Top()
        {
            return battleship.Top;
        }

        public Rectangle GetBounds()
        {
            return battleship.Bounds;
        }
    }
}
