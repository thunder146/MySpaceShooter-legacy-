using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace thunder146.MySpaceShooter
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private Drawer _drawer;
        private Texture2D _img_laser;
        private Texture2D _topAsteroidImage;
        private Texture2D _diagonalAsteroidImage;
        private Texture2D _finalBossImage;
        private GameState _gameState;
        //private SoundEffect _bgMusic;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private int _bgMoveSpeed = 10;
        private TopAsteroidsDrawer _topAsteroidsDrawer;
        private DiagonalAsteroidsDrawer _diagonalAsteroidsDrawer;
        private CollisionDetection _collisionDetection;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            _player = new Player();
            _topAsteroidsDrawer = new TopAsteroidsDrawer();
            _diagonalAsteroidsDrawer = new DiagonalAsteroidsDrawer();
            _gameState = new GameState();
            _collisionDetection = new CollisionDetection(_gameState, _player, _topAsteroidsDrawer, _diagonalAsteroidsDrawer);

            _drawer = new Drawer(_graphics, _gameState, _player, _topAsteroidsDrawer, _diagonalAsteroidsDrawer);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            try
            {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(Content);
            _topAsteroidsDrawer.LoadContent(Content);
            _diagonalAsteroidsDrawer.LoadContent(Content);

            _img_laser = Content.Load<Texture2D>("Images\\laser");
            _topAsteroidImage = Content.Load<Texture2D>("Images\\Asteroid1");
            _diagonalAsteroidImage = Content.Load<Texture2D>("Images\\Asteroid2");
            _finalBossImage = Content.Load<Texture2D>("Images\\FinalBoss");

            //_bgMusic = Content.Load<SoundEffect>("Sounds\\bg_music");

            _collisionDetection.LoadContent(Content, _img_laser, _topAsteroidImage, _diagonalAsteroidImage, _finalBossImage);
            _drawer.LoadContent(Content, _img_laser, _topAsteroidImage, _diagonalAsteroidImage, _finalBossImage);
            
            //// TODO play background music in loop
            //if(!_bgMusic.Play())
            //    _bgMusic.Play(1, 0, 0);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameState.IsFinalBossDeath)
                return;

            KeyboardState ks = Keyboard.GetState();

            if (!_gameState.start)
            {
                if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                {
                    _gameState.start = true;
                    _gameState.lost = false;
                }
                return;
            }
            else if (_gameState.start && ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R))
            {
                RestartGame();
            }

            if (_gameState.lost) 
                return;

            MoveBackground(gameTime);

            _gameState.ElapsedGameTime += (float)gameTime.ElapsedGameTime.Milliseconds;

            SwitchLaser();
           
            _player.Update(gameTime);

            ChangeLevel();

            UpdateAsteroids(gameTime);

            _collisionDetection.CollisionWithTopAsteroids(gameTime);
            _collisionDetection.CollisionWithDiagonalAsteroids(gameTime);

            if (!_gameState.IsFinalBossDeath)
                _collisionDetection.CollisionFinalBoss(gameTime);
            
            base.Update(gameTime);
        }

        private void SwitchLaser()
        {
            if (_gameState.CurrentLevel == LevelSelection.Level10)
            {
                _player.LaserState = LaserState.Three;
            }
            else if (_gameState.CurrentLevel == LevelSelection.Level4 ||
                _gameState.CurrentLevel == LevelSelection.Level5 ||
                _gameState.CurrentLevel == LevelSelection.Level6 ||
                _gameState.CurrentLevel == LevelSelection.Level7 ||
                _gameState.CurrentLevel == LevelSelection.Level8 ||
                _gameState.CurrentLevel == LevelSelection.Level9)
            {
                _player.LaserState = LaserState.Two;
            }
        }

        private void RestartGame()
        {
            _gameState.Reset();
            _gameState.LaserMoveSpeed = -400;

            if (_player.Lasers.Count > 0)
                _player.Lasers.Clear();
            if (_topAsteroidsDrawer.Asteroids != null && _topAsteroidsDrawer.Asteroids.Count > 0)
                _topAsteroidsDrawer.Asteroids.Clear();
            if (_diagonalAsteroidsDrawer.Asteroids != null && _diagonalAsteroidsDrawer.Asteroids.Count > 0)
                _diagonalAsteroidsDrawer.Asteroids.Clear();

            _gameState.start = false;  // back to start screen
        }

        private void UpdateAsteroids(GameTime gameTime)
        {
            switch (_gameState.CurrentLevel)
            {
                case LevelSelection.Level1:
                    _topAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level2:
                    _topAsteroidsDrawer.Asteroids.Clear();
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level3:
                    _gameState.LaserMoveSpeed = -250;
                    _topAsteroidsDrawer.Update(gameTime);
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level4:
                    _diagonalAsteroidsDrawer.Asteroids.Clear();
                    _topAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level5:
                    _topAsteroidsDrawer.Asteroids.Clear();
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level6:
                    _topAsteroidsDrawer.Update(gameTime);
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level7:
                    _diagonalAsteroidsDrawer.Asteroids.Clear();
                    _topAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level8:
                    _topAsteroidsDrawer.Asteroids.Clear();
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level9:
                    _topAsteroidsDrawer.Update(gameTime);
                    _diagonalAsteroidsDrawer.Update(gameTime);
                    break;
                case LevelSelection.Level10:
                    _topAsteroidsDrawer.Asteroids.Clear();
                    _diagonalAsteroidsDrawer.Asteroids.Clear();
                    break;
            }
        }

        private void ChangeLevel()
        {
            // Level 2 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 15)
            {
                _gameState.CurrentLevel = LevelSelection.Level2;
                _gameState.LvlName = "2";
            }
            // Level 3 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 30)
            {
                _gameState.LaserMoveSpeed = -200;  // Halbieren, da doppelte Geschwindigkeit, weil der laser in level 1 und 2 bewegt wird

                _gameState.CurrentLevel = LevelSelection.Level3;
                _gameState.LvlName = "3";
            }
            // Level 4 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 45)
            {
                _gameState.LaserMoveSpeed = -400;

                _gameState.Meteor_move_speed = 275;

                _gameState.CurrentLevel = LevelSelection.Level4;
                _gameState.LvlName = "4";
            }
            // Level 5 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 60)
            {
                _gameState.LaserMoveSpeed = -400;

                _gameState.Meteorit2_speed_X = 275;
                _gameState.Meteorit2_speed_Y = 275;

                _gameState.CurrentLevel = LevelSelection.Level5;
                _gameState.LvlName = "5";
            }
            // Level 6 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 75)
            {
                _gameState.LaserMoveSpeed = -200; // Halbieren, da doppelte Geschwindigkeit, weil der laser in level 1 und 2 bewegt wird

                _gameState.CurrentLevel = LevelSelection.Level6;
                _gameState.LvlName = "6";
            }
            // Level 7 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 90)
            {
                _gameState.LaserMoveSpeed = -400;

                _gameState.Meteor_move_speed = 370;

                _gameState.CurrentLevel = LevelSelection.Level7;
                _gameState.LvlName = "7";
            }
            // Level 8 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 105)
            {
                _gameState.LaserMoveSpeed = -400;

                _gameState.Meteorit2_speed_X = 385;
                _gameState.Meteorit2_speed_Y = 385;

                _gameState.CurrentLevel = LevelSelection.Level8;
                _gameState.LvlName = "8";
            }
            // Level 9 aktivieren
            if ((int)(_gameState.ElapsedGameTime / 1000) == 130)
            {
                _gameState.LaserMoveSpeed = -200; // Halbieren, da doppelte Geschwindigkeit, weil der laser in level 1 und 2 bewegt wird

                _gameState.CurrentLevel = LevelSelection.Level9;
                _gameState.LvlName = "9";
            }
            // Final Boss
            if ((int)(_gameState.ElapsedGameTime / 1000) == 145)
            {
                _gameState.LaserMoveSpeed = -400;

                _gameState.CurrentLevel = LevelSelection.Level10;
                _gameState.LvlName = "10";
            }
        }

        private void MoveBackground(GameTime gameTime)
        {
            _gameState.Bg1_posY += _bgMoveSpeed * gameTime.ElapsedGameTime.Milliseconds / 100;
            _gameState.Bg2_posY += _bgMoveSpeed * gameTime.ElapsedGameTime.Milliseconds / 100;
            _gameState.Bg3_posY += _bgMoveSpeed * gameTime.ElapsedGameTime.Milliseconds / 100;

            if (_gameState.Bg1_posY >= 800)
                _gameState.Bg1_posY = -800;
            if (_gameState.Bg2_posY >= 800)
                _gameState.Bg2_posY = -800;
            if (_gameState.Bg3_posY >= 800)
                _gameState.Bg3_posY = -800;
        }

        protected override void Draw(GameTime gameTime)
        {
            _drawer.Draw(gameTime, _spriteBatch );
            base.Draw(gameTime);
        }
    }
}
