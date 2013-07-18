using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class PowerUp : Sprite
    {
        public enum powerUpType
        {
            ballDamage,
            extraBullet,
            extraScore,
            extraLife,
            bulletDamage,
            shield,
            penalty,
            tripleShot
        }

        public powerUpType type;
        public bool alive;
        int index;
        int frameHeight, frameWidth;
        public Vector2 position;
        int speed;
        public Rectangle boundingRectangle;

        public PowerUp(Texture2D newTexture, Brick brick) : base(newTexture)
        {
            alive = true;
            speed = 2;
            position.X = brick.rectangle.Center.X;
            position.Y = brick.rectangle.Center.Y;
            frameHeight = texture.Height;
            frameWidth = texture.Width / 8;
            setType();
            rectangle = new Rectangle(frameWidth * index, 0, frameWidth, frameHeight);
            boundingRectangle = new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
        }

        public void update()
        {
            position.Y += speed;
            boundingRectangle.Y = (int)position.Y;
            checkBounds();
        }

        public void checkBounds()
        {
            if (rectangle.Y > 20)
            {
                this.alive = false;
            }
        }

        void setType()
        {
            int value = rand.Next(50);
            if(value < 1)
            {
                index = 0;
                type = powerUpType.ballDamage;
            }
            else if (value < 15)
            {
                index = 1;
                type = powerUpType.extraBullet;
            }
            else if (value < 30)
            {
                index = 2;
                type = powerUpType.extraScore;
            }
            else if (value < 31)
            {
                index = 3;
                type = powerUpType.extraLife;
            }
            else if (value < 32)
            {
                index = 4;
                type = powerUpType.bulletDamage;
            }
            else if (value < 38)
            {
                index = 5;
                type = powerUpType.shield;
            }
            else if (value < 48)
            {
                index = 6;
                type = powerUpType.penalty;
            }
            else if (value < 50)
            {
                index = 7;
                type = powerUpType.tripleShot;
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White);
        }
    }
}
