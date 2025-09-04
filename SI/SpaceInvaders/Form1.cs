using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;//za zvok
using System.Threading;
using System.Windows.Markup;

namespace SpaceInvaders
{
    public partial class Form1 : Form
    {
        Random rand = new Random(); 

        //our player
        private Player player;
        private bool left, right, shoot;
        private const int MAX_HP = 100;
        private int hp_player = MAX_HP;
        private int points = 0;
        
        //bullets 
        private Bullet bullet;
        private Bullet bullet_enemy;
        private List<Bullet> bullets_enemy = new List<Bullet>(); // shranjevali bullete enemijve
        private List<Bullet> bullets_player = new List<Bullet>(); //shranjevali bullete igralca
        private List<Bullet> bullets_first_boss = new List<Bullet>(); //shranjevali bullete first_bossa
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

        //Colors
        private Color COL_RED = Color.Red;
        private Color COL_BLUE = Color.Blue;
        private Color COL_GREEN = Color.Green;

        //Boss1
        Boss1 first_boss;
        private int first_boss_hp = 100;
        private const int BOSS1DAMAGE = 10;

        //Boss2
        private Boss2 second_boss;
        private int second_boss_hp = 100;
        private const int BOSS2DAMAGE = 1;

        //COunter
        private int counter = 0;

        //start game
        private bool start;

        //za zvok
        SoundPlayer mainMusic; //glavna glasba
        SoundPlayer laserSound; // ko ustrelimo
        string path_to_mainmusic = "C:\\Users\\jurij\\Desktop\\SpaceI\\SI\\sounds\\Music.wav";
        string path_to_laser = "C:\\Users\\jurij\\Desktop\\SpaceI\\SI\\sounds\\Laser.wav";

        private Panel menuPanel;
        private Button startButton;
        private Button keyboardButton;
        private Label gameName;
        private Panel keybindingsPanel;
        private Button backKeyboard;
        private Button values;
        private Panel valuesPanel;
        private Button backValues;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;

            //glavni menu
            InitializeMenu();

            //GAMElabel
            GameLabel();

            //our player
            player = new Player(Controls, ClientSize.Width, ClientSize.Height);
            enemy_ships = new Enemy(Controls, ClientSize.Width, SPEED_ENEMY * level);

            //game start 
            start = false;

            //zvok
            mainMusic = new SoundPlayer(path_to_mainmusic);
            mainMusic.PlayLooping();

            // zvezno ozadje
            for (int i = 0; i < 100; i++)
            {
                Label star = new Label();
                star.Size = new Size(2, 2);
                star.BackColor = Color.White;
                star.Location = new Point(rand.Next(ClientSize.Width), rand.Next(ClientSize.Height));
                Controls.Add(star);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            start = true;
            menuPanel.Visible = false; // skrije menu
        }

        private void keyboardButton_Click(object sender, EventArgs e)
        {
            menuPanel.Visible=false;

            // Panel
            keybindingsPanel = new Panel();
            keybindingsPanel.Size = new Size(1150, 1000);
            keybindingsPanel.Location = new Point((ClientSize.Width - menuPanel.Width) / 2, (ClientSize.Height - menuPanel.Height) / 2);
            keybindingsPanel.BackColor = Color.Black;
            keybindingsPanel.Visible = true; 
            Controls.Add(keybindingsPanel);
            keybindingsPanel.BringToFront();

            //zvezdno ozadje
            for (int i = 0; i < 100; i++)
            {
                Label star = new Label();
                star.Size = new Size(2, 2);
                star.BackColor = Color.White;
                star.Location = new Point(rand.Next(keybindingsPanel.Width), rand.Next(keybindingsPanel.Height));
                keybindingsPanel.Controls.Add(star);
            }

            Label moving = new Label();
            moving.Text = "Moving: <= A   D => ";
            moving.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            moving.Size = new Size(700, 100);
            moving.Location = new Point((menuPanel.Width - gameName.Width) / 2, 300);
            moving.ForeColor = Color.Lime;
            moving.BackColor = Color.Transparent;
            moving.TextAlign = ContentAlignment.MiddleCenter;
            keybindingsPanel.Controls.Add(moving);

            Label shoot = new Label();
            shoot.Text = "Shooting: L";
            shoot.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            shoot.Size = new Size(700, 100);
            shoot.Location = new Point((menuPanel.Width - gameName.Width) / 2, 400);
            shoot.ForeColor = Color.Lime;
            shoot.BackColor = Color.Transparent;
            shoot.TextAlign = ContentAlignment.MiddleCenter;
            keybindingsPanel.Controls.Add(shoot);

            Label pause = new Label();
            pause.Text = "Pause: P";
            pause.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            pause.Size = new Size(700, 100);
            pause.Location = new Point((menuPanel.Width - gameName.Width) / 2, 500);
            pause.ForeColor = Color.Lime;
            pause.BackColor = Color.Transparent;
            pause.TextAlign = ContentAlignment.MiddleCenter;
            keybindingsPanel.Controls.Add(pause);

            Label proceed = new Label();
            proceed.Text = "Continue: N";
            proceed.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            proceed.Size = new Size(700, 100);
            proceed.Location = new Point((menuPanel.Width - gameName.Width) / 2, 600);
            proceed.ForeColor = Color.Lime;
            proceed.BackColor = Color.Transparent;
            proceed.TextAlign = ContentAlignment.MiddleCenter;
            keybindingsPanel.Controls.Add(proceed);

            backKeyboard = new Button();
            backKeyboard.Text = "BACK";
            backKeyboard.Font = new Font("Press Start 2P", 12, FontStyle.Bold);
            backKeyboard.Size = new Size(300, 60);
            backKeyboard.Location = new Point(800, 100);
            backKeyboard.ForeColor = Color.Lime;
            backKeyboard.BackColor = Color.Black;
            backKeyboard.FlatStyle = FlatStyle.Flat;
            backKeyboard.FlatAppearance.BorderColor = Color.Lime;
            backKeyboard.FlatAppearance.BorderSize = 2;
            backKeyboard.Click += backKeyboard_Click;
            keybindingsPanel.Controls.Add(backKeyboard);
        }

        private void backKeyboard_Click(object sender, EventArgs e)
        {
            keybindingsPanel.Visible = false;
            menuPanel.Visible = true;
        }

        private void backValues_Click(object sender, EventArgs e)
        {
            valuesPanel.Visible = false;
            menuPanel.Visible = true;
        }

        private void values_Click(object sender, EventArgs e)
        {
            menuPanel.Visible = false;

            // Panel
            valuesPanel = new Panel();
            valuesPanel.Size = new Size(1150, 1000);
            valuesPanel.Location = new Point((ClientSize.Width - menuPanel.Width) / 2, (ClientSize.Height - menuPanel.Height) / 2);
            valuesPanel.BackColor = Color.Black;
            valuesPanel.Visible = true;
            Controls.Add(valuesPanel);
            valuesPanel.BringToFront();

            //zvezdatno ozadje
            for (int i = 0; i < 100; i++)
            {
                Label star = new Label();
                star.Size = new Size(2, 2);
                star.BackColor = Color.White;
                star.Location = new Point(rand.Next(valuesPanel.Width), rand.Next(valuesPanel.Height));
                valuesPanel.Controls.Add(star);
            }

            Label enemyvalues = new Label();
            enemyvalues.Text = "All regular enemies have value of 10 points. Boss has value of 50 points";
            enemyvalues.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            enemyvalues.Size = new Size(700, 100);
            enemyvalues.Location = new Point((menuPanel.Width - gameName.Width) / 2, 600);
            enemyvalues.ForeColor = Color.Lime;
            enemyvalues.BackColor = Color.Transparent;
            enemyvalues.TextAlign = ContentAlignment.MiddleCenter;
            valuesPanel.Controls.Add(enemyvalues);

            Label regularEnemies = new Label();
            regularEnemies.Text = "REGULAR ENEMIES:";
            regularEnemies.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            regularEnemies.Size = new Size(700, 100);
            regularEnemies.Location = new Point((menuPanel.Width - gameName.Width) / 2, 200);
            regularEnemies.ForeColor = Color.Lime;
            regularEnemies.BackColor = Color.Transparent;
            regularEnemies.TextAlign = ContentAlignment.MiddleCenter;
            valuesPanel.Controls.Add(regularEnemies);

            // zanka da mi nrdi sliko za usak sprite posebi
            for (int i = 0; i < 4; i++)
            {
                string pathRegular = $"C:\\Users\\jurij\\Desktop\\SpaceI\\SI\\images\\{i}.png";

                PictureBox regEnemyPic = new PictureBox();
                regEnemyPic.Image = Image.FromFile(pathRegular);
                regEnemyPic.SizeMode = PictureBoxSizeMode.StretchImage;
                regEnemyPic.Size = new Size(70, 70); 
                regEnemyPic.Location = new Point(350 + i * 120, 300);
                valuesPanel.Controls.Add(regEnemyPic);
            }

            Label boss = new Label();
            boss.Text = "BOSS:";
            boss.Font = new Font("Press Start 2P", 25, FontStyle.Bold);
            boss.Size = new Size(700, 100);
            boss.Location = new Point((menuPanel.Width - gameName.Width) / 2, 400);
            boss.ForeColor = Color.Lime;
            boss.BackColor = Color.Transparent;
            boss.TextAlign = ContentAlignment.MiddleCenter;
            valuesPanel.Controls.Add(boss);

            string pathBoss = $"C:\\Users\\jurij\\Desktop\\SpaceI\\SI\\images\\LargeAlien.png";
            PictureBox Bosspic = new PictureBox();
            Bosspic.Image = Image.FromFile(pathBoss);
            Bosspic.SizeMode = PictureBoxSizeMode.StretchImage;
            Bosspic.Size = new Size(70, 70);
            Bosspic.Location = new Point(550, 500);
            valuesPanel.Controls.Add(Bosspic);

            backValues = new Button();
            backValues.Text = "BACK";
            backValues.Font = new Font("Press Start 2P", 12, FontStyle.Bold);
            backValues.Size = new Size(300, 60);
            backValues.Location = new Point(800, 100);
            backValues.ForeColor = Color.Lime;
            backValues.BackColor = Color.Black;
            backValues.FlatStyle = FlatStyle.Flat;
            backValues.FlatAppearance.BorderColor = Color.Lime;
            backValues.FlatAppearance.BorderSize = 2;
            backValues.Click += backValues_Click;
            valuesPanel.Controls.Add(backValues);
        }

        private void InitializeMenu()
        {
            // Panel
            menuPanel = new Panel();
            menuPanel.Size = new Size(1150, 1000);
            menuPanel.Location = new Point((ClientSize.Width - menuPanel.Width) / 2, (ClientSize.Height - menuPanel.Height) / 2);
            menuPanel.BackColor = Color.Black; 
            Controls.Add(menuPanel);

            // zvezdno ozadje
            for (int i = 0; i < 100; i++)
            {
                Label star = new Label();
                star.Size = new Size(2, 2);
                star.BackColor = Color.White;
                star.Location = new Point(rand.Next(menuPanel.Width), rand.Next(menuPanel.Height));
                menuPanel.Controls.Add(star);
            }

            // Label – game title
            gameName = new Label();
            gameName.Text = "SPACE INVADERS";
            gameName.Font = new Font("Press Start 2P", 36, FontStyle.Bold); 
            gameName.Size = new Size(700, 100);
            gameName.Location = new Point((menuPanel.Width - gameName.Width) / 2, 100);
            gameName.ForeColor = Color.Lime;
            gameName.BackColor = Color.Transparent;
            gameName.TextAlign = ContentAlignment.MiddleCenter;
            menuPanel.Controls.Add(gameName);

            // Start game gumb
            startButton = new Button();
            startButton.Text = "START GAME";
            startButton.Font = new Font("Press Start 2P", 12, FontStyle.Bold);
            startButton.Size = new Size(300, 60);
            startButton.Location = new Point((menuPanel.Width - startButton.Width) / 2, 400);
            startButton.ForeColor = Color.Lime;
            startButton.BackColor = Color.Black;
            startButton.FlatStyle = FlatStyle.Flat;
            startButton.FlatAppearance.BorderColor = Color.Lime;
            startButton.FlatAppearance.BorderSize = 2;
            startButton.Click += startButton_Click;
            menuPanel.Controls.Add(startButton);

            // Keyboard gumb
            keyboardButton = new Button();
            keyboardButton.Text = "KEYBOARD";
            keyboardButton.Font = new Font("Press Start 2P", 12, FontStyle.Bold);
            keyboardButton.Size = new Size(300, 60);
            keyboardButton.Location = new Point((menuPanel.Width - keyboardButton.Width) / 2, 500);
            keyboardButton.ForeColor = Color.Lime;
            keyboardButton.BackColor = Color.Black;
            keyboardButton.FlatStyle = FlatStyle.Flat;
            keyboardButton.FlatAppearance.BorderColor = Color.Lime;
            keyboardButton.FlatAppearance.BorderSize = 2;
            keyboardButton.Click += keyboardButton_Click;
            menuPanel.Controls.Add(keyboardButton);

            values = new Button();
            values.Text = "VALUES";
            values.Font = new Font("Press Start 2P", 12, FontStyle.Bold);
            values.Size = new Size(300, 60);
            values.Location = new Point((menuPanel.Width - keyboardButton.Width) / 2, 600);
            values.ForeColor = Color.Lime;
            values.BackColor = Color.Black;
            values.FlatStyle = FlatStyle.Flat;
            values.FlatAppearance.BorderColor = Color.Lime;
            values.FlatAppearance.BorderSize = 2;
            values.Click += values_Click;
            menuPanel.Controls.Add(values);
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

            //pause
            if (e.KeyCode == Keys.P)
            {
                start = false;
            }

            //nadaljevanje
            if (e.KeyCode == Keys.N)
            {
                start = true;
            }

            // ko pritisnemo L, moramo dobiti pozicijo igralca(njegovo sredino in vrh)
            // ustvariti now objekt bullet in ga dodati v seznam bullet
            if (e.KeyCode == Keys.L)
            {
                int playerX = player.Left() + (player.Width() / 2 - 5);
                int playerY = player.Top();
                if (bullets_player.Count < MAX_BULLETS)
                {
                    bullet = new Bullet(playerX, playerY, COL_RED, Controls);
                    bullets_player.Add(bullet);
                    laserSound = new SoundPlayer(path_to_laser);
                    laserSound.Play();
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
            if (!start) return;
            
            else
            {
                if (level % 10 == 0)
                {
                    if (left) player.Move_left();
                    if (right) player.Move_Right(ClientSize.Width);

                    Izbrisi_enemy();
                    IzbrisiMeteorje();
                    IzbrisiEnemyBullete();

                    if (second_boss == null)
                    {
                        second_boss = new Boss2(Controls, ClientSize.Width / 2, ClientSize.Height);
                    }

                    second_boss.Boss2Movement(ClientSize.Width);
                    second_boss.UstreliLaser();

                    if (second_boss.LaserHits(player.GetBounds()))
                    {
                        hp_player -= BOSS2DAMAGE;

                        HpLabel.Text = $"HP: {hp_player}";

                        //progress bar
                        HpBar.Value = Math.Max(0, Math.Min(hp_player, MAX_HP));
                        HpBar.Refresh();
                    }

                    PlayerBullets();

                    Preveri_HP_Igralca();
                }

                else if (level % 5 == 0)
                {
                    //player movement
                    if (left) player.Move_left();

                    if (right) player.Move_Right(ClientSize.Width);

                    //delete enemy ladje, ker je boss level
                    Izbrisi_enemy();

                    //izbrise vse meteorje ki so še na sliki
                    IzbrisiMeteorje();

                    //Izbrise enemy bullete
                    IzbrisiEnemyBullete();

                    // samo enkrat ustvarimo first_boss
                    if (first_boss == null)
                    {
                        first_boss = new Boss1(Controls, ClientSize.Width / 2);
                    }

                    //premikanje boss ladje
                    first_boss.Boss1Movement(ClientSize.Width);

                    //streljanje
                    First_boss_ustreli();


                    PlayerBullets();

                    //Preverjanje življenja
                    Preveri_HP_Igralca();
                    

                }
                else
                {
                    if (left) player.Move_left();

                    if (right) player.Move_Right(ClientSize.Width);


                    // ustrelimo bullet, če gre nad screen ga uničimo, preveri trk in uniči če zadane enemy_ship
                    Trk_bulletov_enemy();

                    //premiki enemyjev, če zadane rob, da gre dol...
                    enemy_ships.Move_enemies(ClientSize.Width);

                    //ustvarimo meteroje z verjetnostjo 1% in jih spuščamo
                    Ustvari_meteor();

                    //Preveri trke med igralcem in window height
                    Trk_Meteorja();

                    //povecanje levela, če ni nasprotnikov, ustvarjanje nasprotnikov spet
                    Povecanje_levela();

                    //Preverjanje življenja
                    Preveri_HP_Igralca();

                    // enemy ustreli metek
                    Enemy_ustreli();

                    TrkEnemy_Player();

                    //izbrise bullete prvega bossa
                    IzbrisiFirstBossBullete();
                }
            }

            Invalidate();
        }


        /// <summary>
        /// ustvari bullet in preveri trk med enemy in bulletom
        /// odstrani bullet ce gre iz screena
        /// </summary>
        public void Trk_bulletov_enemy()
        {
            for (int i = bullets_player.Count - 1; i >= 0; i--)
            {
                bullets_player[i].Move();

                if (bullets_player[i].Off_screen())
                {
                    bullets_player[i].Destroy_bullet(Controls);
                    bullets_player.RemoveAt(i);
                    continue; // gremo na naslednji bullet, da se izognemo napaki
                }

                //Preverimo če bullet in enemy trčita
                List<PictureBox> enemy_list = enemy_ships.Get_List();

                for (int j = enemy_list.Count - 1; j >= 0; j--)
                {
                    if (bullets_player[i].GetBounds().IntersectsWith(enemy_list[j].Bounds))
                    {
                        points += 10;
                        PointsLabel.Text = $"POINTS: {points}";
                        Controls.Remove(enemy_list[j]);
                        enemy_list.RemoveAt(j);

                        bullets_player[i].Destroy_bullet(Controls);
                        bullets_player.RemoveAt(i);
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
                    HpBar.Value = Math.Max(0, Math.Min(hp_player, MAX_HP));
                    HpBar.Refresh();
                }
            }
        }

        public void Povecanje_levela()
        {
            if (enemy_ships.Get_List().Count == 0)
            {
                NaslednjiLevel();
            }
        }

        /// <summary>
        /// preverja če je hp manjši ali enak 0
        /// </summary>
        public void Preveri_HP_Igralca()
        {
            if (hp_player <= 0)
            {
                start = false;
                level = 1;
                menuPanel.Visible = true;
                MessageBox.Show($"Igre je konec, dosegli ste {points} točk");
                hp_player = 100;
            }
        }

        public void Enemy_ustreli()
        {
            if (rand.Next(0, 50) == 1)
            {
                List<PictureBox> enemy_list = enemy_ships.Get_List();
                if (enemy_list.Count > 0)
                {
                    PictureBox kdo_bo_ustrelil = enemy_list[rand.Next(0, enemy_list.Count)];
                    int posX = kdo_bo_ustrelil.Left + (kdo_bo_ustrelil.Width / 2 - 5);
                    int posY = kdo_bo_ustrelil.Bottom;
                    bullet_enemy = new Bullet(posX, posY, COL_BLUE, Controls);
                    bullets_enemy.Add(bullet_enemy);
                }
            }

            for (int i = bullets_enemy.Count - 1; i >= 0; i--)
            {
                bullets_enemy[i].Move_enemy();

                if (bullets_enemy[i].Off_screen_bottom(ClientSize.Height))
                {
                    bullets_enemy[i].Destroy_bullet(Controls);
                    bullets_enemy.RemoveAt(i);
                    continue; // gremo na naslednji bullet, da se izognemo napaki
                }

                if (bullets_enemy[i].GetBounds().IntersectsWith(player.GetBounds()))
                { 
                    bullets_enemy[i].Destroy_bullet(Controls);
                    bullets_enemy.RemoveAt(i);

                    hp_player -= 10;

                    //label
                    HpLabel.Text = $"HP: {hp_player}";

                    //progress bar
                    HpBar.Value = Math.Max(0, Math.Min(hp_player, MAX_HP));
                    HpBar.Refresh();

                }
            }
        }

        public void Izbrisi_enemy()
        {
            var enemies = enemy_ships.Get_List();
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Controls.Remove(enemies[i]);
                enemies.RemoveAt(i);
            }
        }

        public void First_boss_ustreli()
        {

            if (counter % 10 == 0)
            {
                int x_pos_boss_left = first_boss.GetLeft();
                int y_pos_boss = first_boss.GetBottom();
                int x_pos_boss_middle = first_boss.GetLeft() + first_boss.GetWidth() / 2 - 5;
                int x_pos_boss_right = first_boss.GetRight() - 10; //10 je size bulleta
                Bullet bullet_first_boss_left = new Bullet(x_pos_boss_left, y_pos_boss, COL_GREEN, Controls);
                Bullet bullet_first_boss_middle = new Bullet(x_pos_boss_middle, y_pos_boss, COL_GREEN, Controls);
                Bullet bullet_first_boss_right = new Bullet(x_pos_boss_right, y_pos_boss, COL_GREEN, Controls);
                bullets_first_boss.Add(bullet_first_boss_left);
                bullets_first_boss.Add(bullet_first_boss_middle);
                bullets_first_boss.Add(bullet_first_boss_right);

            }
            counter++;

            // premikanje metkov
            for (int i = bullets_first_boss.Count - 1; i >= 0; i--)
            {
                bullets_first_boss[i].Move_enemy();

                if (bullets_first_boss[i].Off_screen_bottom(ClientSize.Height))
                {
                    bullets_first_boss[i].Destroy_bullet(Controls);
                    bullets_first_boss.RemoveAt(i);
                    continue; // gremo na naslednji bullet, da se izognemo napaki
                }

                if (bullets_first_boss[i].GetBounds().IntersectsWith(player.GetBounds()))
                {
                    bullets_first_boss[i].Destroy_bullet(Controls);
                    bullets_first_boss.RemoveAt(i);

                    hp_player -= BOSS1DAMAGE;

                    //label
                    HpLabel.Text = $"HP: {hp_player}";

                    //progress bar
                    HpBar.Value = Math.Max(0, Math.Min(hp_player, MAX_HP));
                    HpBar.Refresh();
                }
            }
        }

        public void IzbrisiMeteorje()
        {
            for (int i = meteorji.Count - 1; i >= 0; i--)
            {
                meteorji[i].Destroy_meteor(Controls);
                meteorji.RemoveAt(i);
            }
        }

        public void IzbrisiEnemyBullete()
        {
            for (int i = bullets_enemy.Count - 1; i >= 0; i--)
            {
                bullets_enemy[i].Destroy_bullet(Controls);
                bullets_enemy.RemoveAt(i);
            }
        }

        public void IzbrisiFirstBossBullete()
        {
            for (int i = bullets_first_boss.Count - 1; i >= 0; i--)
            {
                bullets_first_boss[i].Destroy_bullet(Controls);
                bullets_first_boss.RemoveAt(i);
            }
        }
        /// <summary>
        /// v boss levelih se premikajo igralcevi bulleti in pregledajo trke z bossi
        /// </summary>
        public void PlayerBullets()
        {
            for (int i = bullets_player.Count - 1; i >= 0; i--)
            {
                bullets_player[i].Move();

                if (bullets_player[i].Off_screen())
                {
                    bullets_player[i].Destroy_bullet(Controls);
                    bullets_player.RemoveAt(i);
                    continue;
                }

                if (first_boss != null && bullets_player[i].GetBounds().IntersectsWith(first_boss.Bounds()))
                {
                    bullets_player[i].Destroy_bullet(Controls);
                    bullets_player.RemoveAt(i);

                    // boss hp --
                    first_boss_hp -= 10;

                    if (first_boss_hp <= 0)
                    {
                        first_boss.Destroy_first_Boss(Controls);
                        first_boss = null;
                        first_boss_hp = 100;

                        points += 50;
                        PointsLabel.Text = $"SCORE: {points}";

                        NaslednjiLevel();
                    }
                }

                else if (second_boss != null && bullets_player[i].GetBounds().IntersectsWith(second_boss.Bounds()))
                {
                    bullets_player[i].Destroy_bullet(Controls);
                    bullets_player.RemoveAt(i);

                    // boss hp --
                    second_boss_hp -= 10;

                    if (second_boss_hp <= 0)
                    {
                        second_boss.Destroy_second_Boss(Controls);
                        second_boss = null;
                        second_boss_hp = 100;

                        points += 50;
                        PointsLabel.Text = $"SCORE: {points}";

                        NaslednjiLevel();
                    }
                }
            }
        }

        private void NaslednjiLevel()
        {
            level++;
            LevelLabel.Text = $"LEVEL: {level}";

            // če je boss level, počakamo da ga ustvari GameUpdate()
            if (level % 5 != 0)
            {
                // ustvari nove enemy ladje
                enemy_ships = new Enemy(Controls, ClientSize.Width, level * SPEED_ENEMY);
            }
        }

        /// <summary>
        /// pregleda ali trčita enemy ship in player ship in če grejo enemy_ladje iz zaslona
        /// če ja, zgubi player hp
        /// </summary>
        public void TrkEnemy_Player()
        {
            List<PictureBox> enemy_ladje = enemy_ships.Get_List();
            for (int i = enemy_ladje.Count - 1; i >= 0; i--)
            {
                // Check for collision with the player or if the enemy is offscreen
                if (enemy_ladje[i].Bounds.IntersectsWith(player.GetBounds()) || enemy_ladje[i].Top > ClientSize.Height)
                {
                    // Remove the enemy from the controls and list
                    Controls.Remove(enemy_ladje[i]);
                    enemy_ladje.RemoveAt(i);

                    // Update the player's health
                    hp_player -= 10;

                    // Update label and progress bar
                    HpLabel.Text = $"HP: {hp_player}";
                    HpBar.Value = Math.Max(0, Math.Min(hp_player, MAX_HP));
                    HpBar.Refresh();
                }
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
            LevelLabel.Text = $"LEVEL: {level}";
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
            HpBar.Maximum = MAX_HP;
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
