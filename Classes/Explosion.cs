using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Breakout.Classes
{
    public class Explosion : Sprite
    {
        public int index;
        int framesPerRow, frameHeight, frameWidth, numberOfFrames;
        int fps;
        float timer;
        Vector2 position;
        int explosionType;

        public Explosion(Texture2D newTexture, Vector2 newPosition, int newFrameWidth, int newFrameHeight, int newNumberOfFrames, int framesPerSecond)
            : base(newTexture)
        {
            explosionType = rand.Next(1, 3);
            texture = newTexture;
            frameWidth = newFrameWidth;
            frameHeight = newFrameHeight;
            framesPerRow = texture.Width / frameWidth;
            fps = framesPerSecond;
            position = newPosition;
            numberOfFrames = newNumberOfFrames;
        }

        public void Update(GameTime gameTime)
        {
            if (timer > (1.0f / (float)fps))
            {
                if (index != (numberOfFrames - 1))
                {
                    index++;
                }
                timer = 0;
            }
            else
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle rectangle = new Rectangle(frameWidth * index, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 5, rectangle.Height / 5);
            switch (explosionType)
            {
                case 1:
                    spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1, SpriteEffects.None, 1);
                    break;
                case 2:
                    spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1, SpriteEffects.FlipVertically, 1);
                    break;
                case 3:
                    spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1, SpriteEffects.FlipHorizontally, 1);
                    break;
            }
        }

    }
}