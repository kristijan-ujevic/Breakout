using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class Brick : Sprite
    {
        public bool alive;
        int HP;
        public bool damaged = false;
        
        public Brick(Texture2D newTexture) : base(newTexture)
        {
            alive = true;
            HP = Game.level;
            color = new Color(120, rand.Next(140, 200), rand.Next(60, 255));
            rectangle.Inflate(1, 4);
        }

        public void calculateBallDamage()
        {
            if (damaged)
            {
                damage(Game.player.ballDamage);
                damaged = false;
            }
        }

        public void damage(int amount)
        {
            HP -= amount;
            checkAlive();
        }

        void checkAlive()
        {
            if (HP <= 0)
            {
                alive = false;
                dropPowerUp();
                explode();
                Game.player.increaseScore(25 * Game.level);
            }
        }

        void dropPowerUp()
        {
            //higher level = greater drop chance
            if (rand.Next(100) < Math.Floor(15 + Game.level * 0.2f))
            {
                Game.powerUps.Add(new PowerUp(Game.powerUpTexture, this));
            }
        }

        void explode()
        {
            Vector2 position = new Vector2(rectangle.X, rectangle.Y);
            Game.explosions.Add(new Explosion(Game.explodeTexture, position, 80, 46, 8, rand.Next(25, 45)));
            Game.explodeSound.Play(0.3f, 0f, 0);
        }

         public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
