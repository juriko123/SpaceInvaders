using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Boss2
    {
        private PictureBox boss2;
        private PictureBox laserBeam;
        private const int LENGTH_SIZE = 120;
        private const int HEIGHT_SIZE = 50;
        private const int HEIGHT_POS = 50;
        private const int SPEED = 5;
        private bool edge;
        private int DIRECTION = 1; //desno, -1 levo
        private string path = "Resources/LargeAlien.png";
        private bool laserActive;
        private int laserCooldown = 0;
        private int laserDuration = 50;
        private int laserTimer = 0;

        public Boss2(Control.ControlCollection controls, int posX, int height)
        {
            boss2 = new PictureBox();
            boss2.Size = new Size(LENGTH_SIZE, HEIGHT_SIZE);
            boss2.Location = new Point(posX, HEIGHT_POS);
            boss2.Image = Image.FromFile(path);
            boss2.SizeMode = PictureBoxSizeMode.StretchImage;
            controls.Add(boss2);

            //laser
            // Laser
            laserBeam = new PictureBox();
            laserBeam.Size = new Size(20, height); // visina laserja
            laserBeam.BackColor = Color.Blue;
            laserBeam.Visible = false;
            controls.Add(laserBeam);
        }

        public void Boss2Movement(int windowidth)
        {
            if (boss2.Right >= windowidth || boss2.Left <= 0)
            {
                DIRECTION *= -1;
            }

            boss2.Left += SPEED * DIRECTION;

            if (laserActive)
            {
                laserBeam.Left = boss2.Left + boss2.Width / 2 - laserBeam.Width / 2;
                laserBeam.Top = boss2.Bottom;
            }

        }

        public void UstreliLaser()
        {
            if (!laserActive)
            {
                if (laserCooldown <= 0)
                {
                    laserActive = true;
                    laserTimer = laserDuration;
                    laserBeam.Left = boss2.Left + boss2.Width / 2 - laserBeam.Width / 2;
                    laserBeam.Top = boss2.Bottom;
                    laserBeam.Visible = true;
                }
                else
                {
                    laserCooldown--;
                }
            }
            else
            {
                laserTimer--;
                if (laserTimer <= 0)
                {
                    laserActive = false;
                    laserBeam.Visible = false;
                    laserCooldown = 100;
                }
            }
        }

        public bool LaserHits(Rectangle playerBounds)
        {
            return laserActive && laserBeam.Bounds.IntersectsWith(playerBounds);
        }

        public Rectangle Bounds()
        {
            return boss2.Bounds;
        }
        public void Destroy_second_Boss(Control.ControlCollection controls)
        {
            // Odstrani boss in laser iz kontrole
            controls.Remove(boss2);
            controls.Remove(laserBeam);
            boss2.Dispose();
            laserBeam.Dispose();
        }

    }
}
