using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    internal class Laser
    {
        private PictureBox laserBeam;

        public Laser(Control.ControlCollection controls, int height)
        {
            laserBeam = new PictureBox();
            laserBeam.Size = new Size(20, height); 
            laserBeam.BackColor = Color.LightBlue;
            laserBeam.Visible = false;
            controls.Add(laserBeam);
        }

        public int left()
        {
            return laserBeam.Left;
        }
    }
}
