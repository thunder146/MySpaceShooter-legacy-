using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace thunder146.MySpaceShooter
{
    interface IAsteroidsDrawer
    {
        List<Vector2> Asteroids { get; }

        void LoadContent(ContentManager Content);

        void Update(GameTime gt);

        void Draw(GameTime gt, SpriteBatch sprite);
    }
}
