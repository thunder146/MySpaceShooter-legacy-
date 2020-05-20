using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace thunder146.MySpaceShooter
{
    internal class CollisionDetection
    {
        private GameState _gameState;
        private TopAsteroidsDrawer _topAsteroidsDrawer;
        private DiagonalAsteroidsDrawer _diagonalAsteroidsDrawer;
        private Player _player;
        private Texture2D _img_laser;
        private Texture2D _topAsteroidImage;
        private Texture2D _diagonalAsteroidImage;
        private Texture2D _finalBossImage;
        private SoundEffect _sound_schiff_exp, _sound_schiff_meteor, _sound_meteor_exp;

        // Level 10 / Final Boss
        private List<Vector2> _lasers2 = new List<Vector2>();
        private float _laser2_Zeit; // laser recharge time
        private int _finalBossHealth = 10;
        private int _finalBossHitCounter = 0;

        public CollisionDetection(GameState gameState, 
            Player player, 
            TopAsteroidsDrawer topAsteroidsDrawer, 
            DiagonalAsteroidsDrawer diagonalAsteroidsDrawer)
        {
            _gameState = gameState;
            _topAsteroidsDrawer = topAsteroidsDrawer;
            _diagonalAsteroidsDrawer = diagonalAsteroidsDrawer;
            _player = player;
        }

        internal void LoadContent(ContentManager Content,
            Texture2D img_laser,
            Texture2D topAsteroidImage,
            Texture2D diagonalAsteroidImage,
            Texture2D finalBossImage)
        {
            _sound_schiff_exp = Content.Load<SoundEffect>("Sounds\\explosion");
            _sound_schiff_meteor = Content.Load<SoundEffect>("Sounds\\hit");
            _sound_meteor_exp = Content.Load<SoundEffect>("Sounds\\hit");

            _img_laser = img_laser;
            _topAsteroidImage = topAsteroidImage;
            _diagonalAsteroidImage = diagonalAsteroidImage;
            _finalBossImage = finalBossImage;
        }

        internal void CollisionWithTopAsteroids(GameTime gameTime)
        {
            if (_gameState.CurrentLevel == LevelSelection.Level4 || _gameState.CurrentLevel == LevelSelection.Level7)
            {

            }
            else if (_gameState.CurrentLevel != LevelSelection.Level1 &&
                _gameState.CurrentLevel != LevelSelection.Level3 &&
                _gameState.CurrentLevel != LevelSelection.Level6 &&
                _gameState.CurrentLevel != LevelSelection.Level9)
            {
                return;
            }

            bool collision = false;

            // Meteorit bewegen  +  Collision mit Schiff
            for (int i = 0; i < _topAsteroidsDrawer.Asteroids.Count; i++)
            {
                _topAsteroidsDrawer.Asteroids[i] += new Vector2(0, _gameState.Meteor_move_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle rect_meteor = new Rectangle((int)_topAsteroidsDrawer.Asteroids[i].X, (int)_topAsteroidsDrawer.Asteroids[i].Y, _topAsteroidImage.Width, _topAsteroidImage.Height);

                if (_player.PlayerRect.Intersects(rect_meteor))
                {
                    collision = true;

                    if (_gameState.PlayerLives > 0)
                        _sound_schiff_meteor.Play(0.3f, 0, 0);
                }

                if (_topAsteroidsDrawer.Asteroids[i].Y > +900) // remove asteroid when out of screen
                {
                    _topAsteroidsDrawer.Asteroids.RemoveAt(i);
                    --i;

                    // decrease score when missed an asteroid
                    if (_gameState.Score >= 100)
                        _gameState.Score -= 100;
                    else if (_gameState.Score >= 0)
                        _gameState.Score = 0;
                }


                if (collision) // Leben abziehen und Meteor entfernen !!!
                {
                    _gameState.PlayerLives--; // 1 Leben weniger

                    _gameState.DrawCollisionAnimation = true; // draw animation

                    if (_gameState.PlayerLives < 0) // Wenn keine Leben mehr, dann verloren
                    {
                        _gameState.lost = true;

                        _sound_schiff_exp.Play(0.6f, 0, 0);

                        return;
                    }

                    _topAsteroidsDrawer.Asteroids.RemoveAt(i);
                    --i;

                    collision = false;
                }
            }

            // Laser bewegen && Collision mit Meteor
            for (int i = 0; i < _player.Lasers.Count; i++)
            {
                bool laser_entfernen = false;

                _player.Lasers[i] += new Vector2(0, _gameState.LaserMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle rect_laser = new Rectangle((int)_player.Lasers[i].X, (int)_player.Lasers[i].Y, _img_laser.Width, _img_laser.Height);

                for (int j = 0; j < _topAsteroidsDrawer.Asteroids.Count; j++)
                {
                    if (rect_laser.Intersects(new Rectangle((int)_topAsteroidsDrawer.Asteroids[j].X, (int)_topAsteroidsDrawer.Asteroids[j].Y, _topAsteroidImage.Width, _topAsteroidImage.Height)))
                    {
                        _topAsteroidsDrawer.Asteroids.RemoveAt(j);
                        laser_entfernen = true;

                        _gameState.Score += 100;

                        _sound_meteor_exp.Play(0.2f, 0, 0);
                    }

                    else if (_player.PlayerRect.Location.Y - _player.Lasers[i].Y > 600) // wenn Laser weiter vom Schiff entfernt, => entfernen
                    {
                        laser_entfernen = true;
                    }
                }
                if (laser_entfernen)
                {
                    _player.Lasers.RemoveAt(i);
                    --i;
                    laser_entfernen = false;
                }
            }
        }

        internal void CollisionWithDiagonalAsteroids(GameTime gameTime)
        {
            if (_gameState.CurrentLevel == LevelSelection.Level5 || _gameState.CurrentLevel == LevelSelection.Level8)
            {

            }
            else if (_gameState.CurrentLevel != LevelSelection.Level2 &&
                _gameState.CurrentLevel != LevelSelection.Level3 &&
                _gameState.CurrentLevel != LevelSelection.Level6 &&
                _gameState.CurrentLevel != LevelSelection.Level9)
            {
                return;
            }

            bool collision = false;

            // Meteorit bewegen  +  Collision mit Schiff
            for (int i = 0; i < _diagonalAsteroidsDrawer.Asteroids.Count; i++)
            {
                _diagonalAsteroidsDrawer.Asteroids[i] += new Vector2(_gameState.Meteorit2_speed_X * (float)gameTime.ElapsedGameTime.TotalSeconds, _gameState.Meteorit2_speed_Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle rect_meteor = new Rectangle((int)_diagonalAsteroidsDrawer.Asteroids[i].X, (int)_diagonalAsteroidsDrawer.Asteroids[i].Y, _diagonalAsteroidImage.Width, _diagonalAsteroidImage.Height);

                if (_player.PlayerRect.Intersects(rect_meteor))
                {
                    collision = true;

                    // diesen Sound nur ausgeben, wenn nicht letztes Leben
                    if (_gameState.PlayerLives > 0)
                        _sound_schiff_meteor.Play(0.3f, 0, 0);
                }

                if (_diagonalAsteroidsDrawer.Asteroids[i].Y > +900 || _diagonalAsteroidsDrawer.Asteroids[i].X > 700) // meteoriten2 entfernen, wenn außerhalb des Bildschirms
                {
                    _diagonalAsteroidsDrawer.Asteroids.RemoveAt(i);
                    --i;

                    // für jeden meteoriten2 den man verpasst, verliert man 150 Punkte
                    if (_gameState.Score >= 150)
                        _gameState.Score -= 150;
                    else if (_gameState.Score >= 0)
                        _gameState.Score = 0;
                }


                if (collision) // Leben abziehen und Meteor entfernen !!!
                {
                    _gameState.PlayerLives--; // 1 Leben weniger

                    _gameState.DrawCollisionAnimation = true; // Animation fürs getroffen werden

                    if (_gameState.PlayerLives < 0) // Wenn keine Leben mehr, dann verloren
                    {
                        _gameState.lost = true;

                        _sound_schiff_exp.Play(0.6f, 0, 0);

                        return;
                    }

                    _diagonalAsteroidsDrawer.Asteroids.RemoveAt(i);
                    --i;

                    collision = false;
                }
            }

            // Laser bewegen && Collision mit Meteor
            for (int i = 0; i < _player.Lasers.Count; i++)
            {
                bool laser_entfernen = false;

                _player.Lasers[i] += new Vector2(0, _gameState.LaserMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle rect_laser = new Rectangle((int)_player.Lasers[i].X, (int)_player.Lasers[i].Y, _img_laser.Width, _img_laser.Height);

                for (int j = 0; j < _diagonalAsteroidsDrawer.Asteroids.Count; j++)
                {
                    if (rect_laser.Intersects(new Rectangle((int)_diagonalAsteroidsDrawer.Asteroids[j].X, (int)_diagonalAsteroidsDrawer.Asteroids[j].Y, _topAsteroidImage.Width, _topAsteroidImage.Height)))
                    {
                        _diagonalAsteroidsDrawer.Asteroids.RemoveAt(j);
                        laser_entfernen = true;

                        // Score +250
                        _gameState.Score += 250;

                        // Sound ausgeben
                        _sound_meteor_exp.Play(0.2f, 0, 0);
                    }

                    else if (_player.PlayerRect.Location.Y - _player.Lasers[i].Y > 600) // wenn Laser weiter vom Schiff entfernt, => entfernen
                    {
                        laser_entfernen = true;
                    }
                }
                if (laser_entfernen)
                {
                    _player.Lasers.RemoveAt(i);
                    --i;
                    laser_entfernen = false;
                }
            }
        }

        internal void CollisionFinalBoss(GameTime gameTime)
        {
            // todo ....... 

            if (_gameState.CurrentLevel != LevelSelection.Level10)
                return;




            // Endgegner bewegen .....





            // todo ...........................



            // Laser abschießen && Collision mit Spieler
            _laser2_Zeit += gameTime.ElapsedGameTime.Milliseconds;
            if (_laser2_Zeit > 600)
            {
                Vector2 nV = new Vector2((int)_gameState.FinalBoss_PosX, (int)_gameState.FinalBoss_PosY);
                _lasers2.Add(nV);

                _laser2_Zeit = 0; // Zeit bis zum nächsten Schuss beginnt wieder von vorne

                // Sounds ausgeben
                //sound_laser.Play(0.3f, 0, 0);
            }




            // todo ...........................




            // Laser bewegen && Collision mit Endgegner
            for (int i = 0; i < _player.Lasers.Count; i++)
            {
                bool laser_entfernen = false;

                _player.Lasers[i] += new Vector2(0, _gameState.LaserMoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle rect_laser = new Rectangle((int)_player.Lasers[i].X, (int)_player.Lasers[i].Y, _img_laser.Width, _img_laser.Height);


                if (rect_laser.Intersects(new Rectangle((int)_gameState.FinalBoss_PosX, (int)_gameState.FinalBoss_PosY, _finalBossImage.Width, _finalBossImage.Height)))
                {

                    laser_entfernen = true;

                    // Endgegner getroffen Zähler erhöhen
                    _finalBossHitCounter++;

                    if (_finalBossHitCounter == _finalBossHealth)
                        _gameState.IsFinalBossDeath = true;

                    // Sound ausgeben
                    _sound_meteor_exp.Play(0.2f, 0, 0);
                }

                else if (_player.PlayerRect.Location.Y - _player.Lasers[i].Y > 600) // wenn Laser weiter vom Schiff entfernt, => entfernen
                {
                    laser_entfernen = true;
                }

                if (laser_entfernen)
                {
                    _player.Lasers.RemoveAt(i);
                    --i;
                    laser_entfernen = false;
                }
            }
        }
    }
}
