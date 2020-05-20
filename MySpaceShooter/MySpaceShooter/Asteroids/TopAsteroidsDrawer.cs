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
    public class TopAsteroidsDrawer : IAsteroidsDrawer
    {
        private Texture2D _asteroidImage;
        private Random _rnd = new Random();

        public TopAsteroidsDrawer()
        {
            Asteroids = new List<Vector2>();
        }

        public List<Vector2> Asteroids { get; private set; }
        
        public void LoadContent(ContentManager Content)
        {
            _asteroidImage = Content.Load<Texture2D>("Images\\Asteroid1");
        }

        public void Update(GameTime gt)
        {
            AddAsteroid(gt);
        }

        private void AddAsteroid(GameTime gameTime)
        {
            if (_rnd.Next(0, 100) == 5 || _rnd.Next(0, 100) == 50)
            {
                Vector2 nV = new Vector2(_rnd.Next(0, 600 - _asteroidImage.Width), -_asteroidImage.Height - 10);
                Asteroids.Add(nV);
            }
        }

        public void Draw(GameTime gt, SpriteBatch sprite)
        {
            foreach (Vector2 item in Asteroids)
                sprite.Draw(_asteroidImage, item, Color.White);
        }
    }
}
