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

namespace thunder146.MySpaceShooter
{
    public enum LaserState
    {
        One,
        Two,
        Three
    }

    internal class Player
    {
        private Vector2 _position;
        private Texture2D _laserImage;
        private float _laserTime;
        private int _laserShootIntervall = 250; // you can shoot every x milliseconds
        private Texture2D _playerImage;
        private int _moveSpeed = 35;
        private SoundEffect _laserSound;

        public Player()
        {
            // set initial position
            _position = new Vector2(285, 700);
        }

        public Rectangle PlayerRect { get; set; }

        public List<Vector2> Lasers = new List<Vector2>();

        public LaserState LaserState = LaserState.One;

        public void LoadContent(ContentManager Content)
        {
            _playerImage = Content.Load<Texture2D>("Images\\PlayerShip");
            _laserImage = Content.Load<Texture2D>("Images\\laser");
            _laserSound = Content.Load<SoundEffect>("Sounds\\shot");
        }

        public void Update(GameTime gt)
        {
            _laserTime += gt.ElapsedGameTime.Milliseconds; // time until next shoot
            ProcessInputKeys(gt);
        }

        private void ProcessInputKeys(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Left) && (int)_position.X > 0)
            {
                _position.X -= _moveSpeed * gameTime.ElapsedGameTime.Milliseconds / 50;
            }
            if (ks.IsKeyDown(Keys.Right) && (int)_position.X < (600 - _playerImage.Width))
            {
                _position.X += _moveSpeed * gameTime.ElapsedGameTime.Milliseconds / 50;
            }
            if (ks.IsKeyDown(Keys.Up) && (int)_position.Y > 0)
            {
                _position.Y -= _moveSpeed / 2 * gameTime.ElapsedGameTime.Milliseconds / 50;
            }
            if (ks.IsKeyDown(Keys.Down) && (int)_position.Y < (800 - _playerImage.Height))
            {
                _position.Y += _moveSpeed / 2 * gameTime.ElapsedGameTime.Milliseconds / 50;
            }

            if (ks.IsKeyDown(Keys.Space))
            {
                ShootLaser(gameTime);
            }
        }

        private void ShootLaser(GameTime gameTime)
        {
            if (_laserTime > _laserShootIntervall)
            {
                switch (LaserState)
                {
                    case MySpaceShooter.LaserState.One:
                        ShootOne();
                        break;

                    case MySpaceShooter.LaserState.Two:
                        ShootTwo();
                        break;

                    case MySpaceShooter.LaserState.Three:
                        ShootThree();
                        break;
                }

                _laserTime = 0;

                _laserSound.Play(0.3f, 0, 0);
            }
        }

        private void ShootThree()
        {
            Vector2 nV = new Vector2((int)_position.X - 5, (int)_position.Y - 10);
            Lasers.Add(nV);

            Vector2 nV2 = new Vector2((int)_position.X + 10, (int)_position.Y - 10);
            Lasers.Add(nV2);

            Vector2 nV3 = new Vector2((int)_position.X + 25, (int)_position.Y - 10);
            Lasers.Add(nV3);
        }

        private void ShootTwo()
        {
            Vector2 nV = new Vector2((int)_position.X, (int)_position.Y - 10);
            Lasers.Add(nV);

            Vector2 nV2 = new Vector2((int)_position.X + (_playerImage.Width - 10), (int)_position.Y - 10);
            Lasers.Add(nV2);
        }

        private void ShootOne()
        {
            Vector2 nV = new Vector2((int)_position.X + 11, (int)_position.Y - 10);
            Lasers.Add(nV);
        }

        public void Draw(GameTime gt, SpriteBatch sprite)
        {
            // draw player
            sprite.Draw(_playerImage, PlayerRect = new Rectangle((int)_position.X, (int)_position.Y, _playerImage.Width, _playerImage.Height), Color.White);

            // draw layers
            foreach (Vector2 item in Lasers)
                sprite.Draw(_laserImage, item, Color.White);
        }
    }
}
