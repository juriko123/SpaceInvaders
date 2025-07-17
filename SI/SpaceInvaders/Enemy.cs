using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Enemy
    {
        private PictureBox enemy_Battleship;
        private const int SIZE = 30;
        private const int ROWS = 5;
        private const int COLUMNS = 5;
        private const int SPACE_BETWEEN = 20;
        private const int POLOVICA = 60;
        private int speed;
        private const int MOVE_DOWN = 25;
        private List<PictureBox> enemies = new List<PictureBox>();
        private int direction = 1;

        public int Speed
        {
            get { return speed; }
            set {
                if (value <= 0)
                {
                   throw new ArgumentException("Speed ne sme biti manjši od 0");
                }
                speed = value; }
        }
        

        public Enemy(Control.ControlCollection controls, int window_width, int speed)
        {
            this.speed = speed;

            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    enemy_Battleship = new PictureBox();
                    enemy_Battleship.Size = new Size(SIZE, SIZE);
                    enemy_Battleship.BackColor = Color.Blue;
                    int location_x = i * (SIZE + SPACE_BETWEEN) + (window_width - POLOVICA) / 2;
                    int location_y = j * (SIZE + SPACE_BETWEEN) + 51; //se 51, ker toliko je višina našega labela
                    enemy_Battleship.Location = new Point(location_x, location_y);
                    enemies.Add(enemy_Battleship);
                    controls.Add(enemy_Battleship);
                }
            }
        }

        public void Move_enemies(int window_width)
        {
            bool edge = false;

            foreach (PictureBox enemmy in enemies)
            {
                if ((direction == 1 && enemmy.Right + speed >= window_width) ||
                    (direction == -1 && enemmy.Left - speed <= 0))
                {
                    edge = true;
                    break; // ZDAJ je smiselno imeti break
                }
            }

            if (edge)
            {
                direction *= -1;
                foreach (PictureBox enemmy in enemies)
                {
                    enemmy.Top += MOVE_DOWN;
                }
            }
            else
            {
                foreach (PictureBox enemmy in enemies)
                {
                    enemmy.Left += direction * speed;
                }
            }
        }


        public List<PictureBox> Get_List()
        {
            return enemies;
        }
    }
}
