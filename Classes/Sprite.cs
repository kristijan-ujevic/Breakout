using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class Sprite
    {
        public Rectangle rectangle;
        public Texture2D texture;
        public float rotation;
        public Vector2 origin;
        public static Random rand = new Random();
        public Color color;

        public Sprite(Texture2D newTexture)
        {
            texture = newTexture;
            rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public float toRadians(float degree)
        {
            return (float)Math.PI / 180 * degree;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.None, 0);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}