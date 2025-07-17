using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        Random rand = new Random(); 

        //our player
        private Player player;
        private bool left, right, shoot;
        private int hp_player = 100;
        private int points = 0;

        //bullets 
        private Bullet bullet;
        private List<Bullet> bullets = new List<Bullet>(); //shranjevali bullet
        private const int MAX_BULLETS = 5;

        //enemy
        private Enemy enemy_ships;
        private const int SPEED_ENEMY = 2;

        //meteor
        private Meteor meteor; 
        private List<Meteor> meteorji = new List<Meteor>();
        private const int METEOR_DAMAGE = 10;

        // level
        private int level = 1;

        //label
        private Label LevelLabel;
        private Label HpLabel;
        private Label PointsLabel;

        //progress bar
        private ProgressBar HpBar;



        public Form1()
        {
            InitializeComponent();

            //GAMElabel
            GameLabel();

            //our player
            player = new Player(Controls, ClientSize.Width, ClientSize.Height);
            enemy_ships = new Enemy(Controls, ClientSize.Width, SPEED_ENEMY * level);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                left = true;
            }

            if (e.KeyCode == Keys.D)
            {
                right = true;
            }

            // ko pritisnemo L, moramo dobiti pozicijo igralca(njegovo sredino in vrh)
            // ustvariti now objekt bullet in ga dodati v seznam bullet
            if (e.KeyCode == Keys.L)
            {
                int playerX = player.Left() + (player.Width() / 2 - 5);
                int playerY = player.Top();
                if (bullets.Count < MAX_BULLETS)
                {
                    bullet = new Bullet(playerX, playerY, Controls);
                    bullets.Add(bullet);
                }
                shoot = true;
            }

        }

        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                left = false;
            }

            if (e.KeyCode == Keys.D)
            {
                right = false;
            }

            if (e.KeyCode == Keys.L)
            {
                shoot = false;
            }
        }

        private void GameUpdate(object sender, EventArgs e)
        {
            if (level % 5 != 0)
            {
                if (left) player.Move_left();

                if (right) player.Move_Right(ClientSize.Width);


                // ustrelimo bullet, če gre nad screen ga uničimo, preveri trk in uniči če zadane enemy
                Trk_bulletov();

                //premiki enemyjev, če zadane rob, da gre dol...
                enemy_ships.Move_enemies(ClientSize.Width);

                //ustvarimo meteroje z verjetnostjo 1% in jih spuščamo
                Ustvari_meteor();

                //Preveri trke med igralcem in window height
                Trk_Meteorja();

                //povecanje levela, če ni nasprotnikov, ustvarjanje nasprotnikov spet
                Povecanje_levela();

                //Preverjanje življenja
                Preveri_HP();
            }

                Invalidate();
        }


        /// <summary>
        /// ustvari bullet in preveri trk med enemy in bulletom
        /// odstrani bullet ce gre iz screena
        /// </summary>
        public void Trk_bulletov()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Move();

                if (bullets[i].Off_screen())
                {
                    bullets[i].Destroy_bullet(Controls);
                    bullets.RemoveAt(i);
                    continue; // gremo na naslednji bullet, da se izognemo napaki
                }

                //Preverimo če bullet in enemy trčita
                List<PictureBox> enemy_list = enemy_ships.Get_List();

                for (int j = enemy_list.Count - 1; j >= 0; j--)
                {
                    if (bullets[i].GetBounds().IntersectsWith(enemy_list[j].Bounds))
                    {
                        points += 10;
                        PointsLabel.Text = $"POINTS: {points}";
                        Controls.Remove(enemy_list[j]);
                        enemy_list.RemoveAt(j);

                        bullets[i].Destroy_bullet(Controls);
                        bullets.RemoveAt(i);
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// metoda ustvari meteorje z verjetnostjo 1%.
        /// </summary>
        public void Ustvari_meteor()
        {
            if (rand.Next(0, 100) == 1)
            {
                meteor = new Meteor(Controls, ClientSize.Width);
                meteorji.Add(meteor);
            }
        }

        /// <summary>
        /// preveri trk meteorja z igralcem in odstrani meteor z screena
        /// </summary>
        public void Trk_Meteorja()
        {
            for (int i = meteorji.Count - 1; i >= 0; i--)
            {
                meteorji[i].Move();

                if (meteorji[i].Off_screen(ClientSize.Height))
                {
                    meteorji[i].Destroy_meteor(Controls);
                    meteorji.RemoveAt(i);
                    continue;
                }

                if (meteorji[i].GetBounds().IntersectsWith(player.GetBounds()))
                {
                    meteorji[i].Destroy_meteor(Controls);
                    meteorji.RemoveAt(i);

                    hp_player -= METEOR_DAMAGE;

                    //label
                    HpLabel.Text = $"HP: {hp_player}";

                    //progress bar
                    HpBar.Value = Math.Max(0, hp_player);
                }
            }
        }

        public void Povecanje_levela()
        {
            if (enemy_ships.Get_List().Count == 0)
            {
                foreach (PictureBox e in enemy_ships.Get_List())
                {
                    Controls.Remove(e);
                }

                level++;
                enemy_ships = new Enemy(Controls, ClientSize.Width, level * SPEED_ENEMY);
                LevelLabel.Text = $"LEVEL: {level}";

            }
        }

        /// <summary>
        /// preverja če je hp manjši ali enak 0
        /// </summary>
        public void Preveri_HP()
        {
            if (hp_player <= 0)
            {
                Application.Exit();
            }
        }
        
        public void GameLabel()
        {
            Label BackgroundLabel = new Label();
            BackgroundLabel.BackColor = Color.Brown;
            BackgroundLabel.Size = new Size(ClientSize.Width, 50);
            BackgroundLabel.Location = new Point(0, 0);  
            Controls.Add(BackgroundLabel);

            LevelLabel = new Label();
            LevelLabel.Text = $"LEVEL: 1";
            LevelLabel.Size = new Size(200, 50);
            LevelLabel.ForeColor = Color.Black;
            LevelLabel.TextAlign = ContentAlignment.MiddleLeft;
            LevelLabel.Font = new Font("Arial", 14, FontStyle.Bold);

            PointsLabel = new Label();
            PointsLabel.Text = $"SCORE: 0";
            PointsLabel.Size = new Size(200, 50);
            PointsLabel.Location = new Point(BackgroundLabel.Width - 210, 0); // desno zgoraj
            PointsLabel.ForeColor = Color.Black;
            PointsLabel.TextAlign = ContentAlignment.MiddleRight;
            PointsLabel.Font = new Font("Arial", 14, FontStyle.Bold);

            HpLabel = new Label();
            HpLabel.Text = $"HP: {hp_player}";
            HpLabel.Size = new Size(200, 50);
            HpLabel.Location = new Point((BackgroundLabel.Width / 2) - 200, 0);
            HpLabel.ForeColor = Color.Black;
            HpLabel.TextAlign = ContentAlignment.MiddleRight;
            HpLabel.Font = new Font("Arial", 14, FontStyle.Bold);

            //progress bar
            HpBar = new ProgressBar();
            HpBar.Location = new Point((BackgroundLabel.Width / 2) - 50, 13);
            HpBar.Size = new Size(200, 25);
            HpBar.Maximum = hp_player;
            HpBar.Value = hp_player;
            HpBar.ForeColor = Color.Green;
            HpBar.Style = ProgressBarStyle.Continuous;


            BackgroundLabel.Controls.Add(LevelLabel);
            BackgroundLabel.Controls.Add(PointsLabel);
            BackgroundLabel.Controls.Add(HpLabel);
            BackgroundLabel.Controls.Add(HpBar);


        }
    }
}
