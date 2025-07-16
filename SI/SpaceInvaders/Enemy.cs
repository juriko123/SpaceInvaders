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
        private const int ROWS = 3;
        private const int COLUMNS = 3;
        private const int SPACE_BETWEEN = 20;
        private const int POLOVICA = 60;
        private const int SPEED = 5;
        private const int MOVE_DOWN = 25;
        private List<PictureBox> enemies = new List<PictureBox>();
        private int direction = 1;
        

        public Enemy(Control.ControlCollection controls, int window_width)
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    enemy_Battleship = new PictureBox();
                    enemy_Battleship.Size = new Size(SIZE, SIZE);
                    enemy_Battleship.BackColor = Color.Blue;
                    int location_x = i * (SIZE + SPACE_BETWEEN) + (window_width - POLOVICA) / 2;
                    int location_y = j * (SIZE + SPACE_BETWEEN);
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
                if ((direction == 1 && enemmy.Right >= (window_width - 100)) // idk drugače gre čez???
                    || (direction == -1 && enemmy.Left - SPEED <= 0))
                {
                    edge = true;
                }
                break;
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
                    enemmy.Left += direction * SPEED;
                }
            }
        }

        public List<PictureBox> Get_List()
        {
            return enemies;
        }
    }
}
