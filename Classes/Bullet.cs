using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class Bullet : Sprite
    {
        public bool alive;
        int bulletSpeed = 17;

        public Bullet(Texture2D newTexture, Player player, float newRotation) : base(newTexture)
        {
            alive = true;
            rectangle.X = player.rectangle.Center.X - rectangle.Width / 2;
            rectangle.Y = player.rectangle.Center.Y;
            rotation = toRadians(newRotation);
        }

        public void update()
        {
            checkCollisions();
            if (rotation > 0)
            {
                rectangle.X += 2; 
            }
            if (rotation < 0)
            {
                rectangle.X -= 2;
            }
            rectangle.Y -= bulletSpeed;
        }

        public void checkCollisions()
        {
            foreach (Brick element in Game.bricks)
            {
                if (element.rectangle.Intersects(rectangle) && element.alive == true && this.alive == true)
                {
                    this.alive = false;
                    element.damage(Game.player.bulletDamage);
                }
            }
            if (rectangle.Y <= -rectangle.Height)
            {
                this.alive = false;
            }
        }
    }
}
