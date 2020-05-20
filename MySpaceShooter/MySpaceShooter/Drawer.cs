using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace thunder146.MySpaceShooter
{
    internal class Drawer
    {
        private SpriteFont _fontBig;
        private SpriteFont _fontSmall;
        private SpriteFont font_hud1;
        private Texture2D _hudImage;
        private Texture2D img_leben;
        private Texture2D img_exp_mini, img_exp_groß;
        private Texture2D _bg0, _bg1, _bg2, _bg3;
        private Texture2D _img_laser;
        private Texture2D _topAsteroidImage;
        private Texture2D _diagonalAsteroidImage;
        private Texture2D _finalBossImg;
        
        private Player _player;
        private Rectangle _finalBossRect;
        private GameState _gameState;
        private TopAsteroidsDrawer _topAsteroidsDrawer;
        private DiagonalAsteroidsDrawer _diagonalAsteroidsDrawer;
        private GraphicsDeviceManager _graphics;

        // FPS
        private float _fpsTimer;
        private int _fpsCounter;
        private int _previousFps;

        public Drawer(GraphicsDeviceManager graphics, GameState gameState, Player player, TopAsteroidsDrawer topAsteroidsDrawer, DiagonalAsteroidsDrawer diagonalAsteroidsDrawer)
        {
            _graphics = graphics;
            _gameState = gameState;
            _player = player;
            _topAsteroidsDrawer = topAsteroidsDrawer;
            _diagonalAsteroidsDrawer = diagonalAsteroidsDrawer;
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            DrawBackground(gameTime, spriteBatch);

            if (!_gameState.start)
            {
                spriteBatch.DrawString(_fontBig, "<<< Press Enter >>>", new Vector2(50, _graphics.PreferredBackBufferHeight / 2), Color.Red);
                spriteBatch.End();
                return;
            }

            _topAsteroidsDrawer.Draw(gameTime, spriteBatch);
            _diagonalAsteroidsDrawer.Draw(gameTime, spriteBatch);
            _player.Draw(gameTime, spriteBatch);

            if (_gameState.CurrentLevel == LevelSelection.Level10 && !_gameState.IsFinalBossDeath)
                DrawFinalBoss(gameTime, spriteBatch);

            DrawCollisionAnimation(gameTime, spriteBatch);

            if (_gameState.lost)
            {
                // draw big collision image
                spriteBatch.Draw(img_exp_groß, new Rectangle(_player.PlayerRect.Location.X - _player.PlayerRect.Width - 10, _player.PlayerRect.Location.Y - _player.PlayerRect.Height, img_exp_groß.Width / 2, img_exp_groß.Height / 2), Color.White);
                // draw HUD in red color
                spriteBatch.Draw(_hudImage, new Rectangle(0, 0, _hudImage.Width, _hudImage.Height), Color.Red);

                spriteBatch.DrawString(_fontBig, "<<< YOU FAIL >>>", new Vector2(20, 300), Color.DarkRed);
                spriteBatch.DrawString(font_hud1, "<<< Press 'R' to try again >>>", new Vector2(35, 360), Color.DarkRed);
            }
            else
            {
                // draw HUD in default green color
                spriteBatch.Draw(_hudImage, new Rectangle(0, 0, _hudImage.Width, _hudImage.Height), Color.WhiteSmoke);
            }

            DrawHUD(gameTime, spriteBatch);
            DrawPlayerHealth(gameTime, spriteBatch);
            DrawFPS(gameTime, spriteBatch);

            if (_gameState.IsFinalBossDeath) // WON!!! Draw score
            {
                spriteBatch.DrawString(_fontBig, "Score", new Vector2(100, 350), Color.Red);
                spriteBatch.DrawString(_fontBig, _gameState.Score.ToString(), new Vector2(100, 400), Color.Red);
            }

            spriteBatch.End();
        }



        private void DrawFinalBoss(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_finalBossImg, _finalBossRect = new Rectangle(_gameState.FinalBoss_PosX, _gameState.FinalBoss_PosY, _finalBossImg.Width, _finalBossImg.Height), Color.White);
        }

        private void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bg0, new Rectangle(0, 0, _bg1.Width, _bg1.Height), Color.White);
            spriteBatch.Draw(_bg1, new Rectangle(0, _gameState.Bg1_posY, _bg1.Width, _bg1.Height), Color.White);
            spriteBatch.Draw(_bg2, new Rectangle(0, _gameState.Bg2_posY, _bg2.Width, _bg2.Height), Color.White);
            spriteBatch.Draw(_bg3, new Rectangle(0, _gameState.Bg3_posY, _bg3.Width, _bg3.Height), Color.White);
        }

        private void DrawCollisionAnimation(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_gameState.DrawCollisionAnimation)
            {
                // draw HUD in red for a brief moment
                spriteBatch.Draw(_hudImage, new Rectangle(0, 0, _hudImage.Width, _hudImage.Height), Color.Red);
                // draw small collision image
                spriteBatch.Draw(img_exp_mini, new Rectangle(_player.PlayerRect.Location.X - 2, _player.PlayerRect.Location.Y - 2, img_exp_mini.Width + 5, img_exp_mini.Height + 5), Color.White);

                _gameState.CollisionTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (_gameState.CollisionTimer > 300)
                {
                    _gameState.CollisionTimer = 0;
                    _gameState.DrawCollisionAnimation = false;
                }
            }
        }

        private void DrawHUD(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font_hud1, "Time : " + ((int)_gameState.ElapsedGameTime / 1000).ToString(), new Vector2(290, 5), Color.WhiteSmoke);
            spriteBatch.DrawString(font_hud1, "Score : " + _gameState.Score.ToString(), new Vector2(130, 765), Color.WhiteSmoke);
            spriteBatch.DrawString(_fontBig, _gameState.LvlName, new Vector2(540, 5), Color.WhiteSmoke);
        }

        private void DrawPlayerHealth(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (_gameState.PlayerLives)
            {
                case 3: spriteBatch.Draw(img_leben, new Rectangle(10, 770, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 1 Leben
                    spriteBatch.Draw(img_leben, new Rectangle(10, 740, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 2 Leben
                    spriteBatch.Draw(img_leben, new Rectangle(40, 770, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 3 Leben
                    break;
                case 2: spriteBatch.Draw(img_leben, new Rectangle(10, 770, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 1 Leben
                    spriteBatch.Draw(img_leben, new Rectangle(10, 740, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 2 Leben
                    break;

                case 1: spriteBatch.Draw(img_leben, new Rectangle(10, 770, img_leben.Width / 2, img_leben.Height / 2), Color.WhiteSmoke); // 1 Leben
                    break;
                case 0:
                default: break;
            }
        }

        private void DrawFPS(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_fontSmall, "FPS : " + _previousFps.ToString(), new Vector2(10, 10), Color.White);
            _fpsCounter++;
            _fpsTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (_fpsTimer >= 1000)
            {
                _previousFps = _fpsCounter;
                _fpsTimer = 0;
                _fpsCounter = 0;
            }
        }

        internal void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, 
            Texture2D img_laser, 
            Texture2D topAsteroidImage, 
            Texture2D diagonalAsteroidImage, 
            Texture2D finalBossImg)
        {
            _img_laser = img_laser;
            _topAsteroidImage = topAsteroidImage;
            _diagonalAsteroidImage = diagonalAsteroidImage;
            _finalBossImg = finalBossImg;

            _hudImage = Content.Load<Texture2D>("Images\\hud");
            img_leben = Content.Load<Texture2D>("Images\\PlayerShip");
            img_exp_mini = Content.Load<Texture2D>("Images\\hit");
            img_exp_groß = Content.Load<Texture2D>("Images\\explosion");
            _fontBig = Content.Load<SpriteFont>("Fonts\\BigFont");
            font_hud1 = Content.Load<SpriteFont>("Fonts\\HudFont");
            _fontSmall = Content.Load<SpriteFont>("Fonts\\SmallFont");
            _bg0 = Content.Load<Texture2D>("Images\\bg0");
            _bg1 = Content.Load<Texture2D>("Images\\bg1");
            _bg2 = Content.Load<Texture2D>("Images\\bg2");
            _bg3 = Content.Load<Texture2D>("Images\\bg3");
        }
    }
}
