using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout.Classes
{
    class Ball : Sprite
    {
        Vector2 velocity;
        const float tangentVelocity = 7.5f;
        public bool started;

        public Ball(Texture2D newTexture) : base (newTexture)
        {
            initialize();
        }

        public void initialize()
        {
            started = false;
            rotation = toRadians(rand.Next(-15, 15));
            setVelocity(rotation);
            velocity.Y = -velocity.Y;
        }

        public void update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W)
                || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                started = true;
            }
            if (started)
            {
                checkBounds();
                rectangle.X += (int)(velocity.X * 60 * gameTime.ElapsedGameTime.TotalSeconds);
                rectangle.Y += (int)(velocity.Y * 60 * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                rectangle.X = Game.player.rectangle.Center.X - rectangle.Width / 2;
                rectangle.Y = Game.player.rectangle.Top - rectangle.Height;
            }
        }

        void checkBounds()
        {
            if ((rectangle.Intersects(Game.borderLeft.rectangle) && velocity.X < 0)
                || (rectangle.Intersects(Game.borderRight.rectangle) && velocity.X > 0))
            {
                velocity.X *= (-1);
                Game.bounceSound.Play(0.5f, 0, 0);
            }
            if (rectangle.Y <= rectangle.Height / 2 && velocity.Y < 0)
            {
                velocity.Y *= (-1);
                Game.bounceSound.Play(0.5f, 0, 0);
            }

            if (Game.shieldTimer.currentTime > 0 && (rectangle.Y > Game.screen.Height - rectangle.Height * 2) && velocity.Y > 0)
            {
                velocity.Y *= (-1);
            }

            if (rectangle.Y > Game.screen.Height + rectangle.Height)
            {
                initialize();
                Game.player.lives--;
            }
        }

        public void checkBrickCollisions()
        {
            int collidedX = 0;
            int collidedY = 0;
            foreach (Brick element in Game.bricks)
            {
                if (element.alive == true && rectangle.Intersects(element.rectangle))
                {
                    if (rectangle.Center.Y > element.rectangle.Top 
                        && rectangle.Center.Y < element.rectangle.Bottom)
                    {
                        collidedX++;
                    }
                    if(rectangle.Center.X >= element.rectangle.Left + rectangle.Width / 2 
                        && rectangle.Left <= element.rectangle.Right - rectangle.Width / 2)
                    {
                        collidedY++;
                    }
                   
                    if (collidedY > 0 || collidedX > 0)
                    {
                        element.damaged = true; //reminder: queue the damage for after updating the ball to avoid multiple collisions
                        Game.bounceSound.Play(0.4f, 0, 0);
                    }
                }
            }
            if (collidedY > 0)
            {
                velocity.Y *= (-1);
            }
            if (collidedX > 0)
            {
                velocity.X *= (-1);
            }
        }

        public void checkPlayerCollision(Player player)
        {
            if (player.rectangle.Intersects(rectangle) && player.rectangle.Top >= rectangle.Center.Y && velocity.Y > 0)
            {
                rotation = toRadians((float)((player.rectangle.Center.X - rectangle.Center.X) / 1.1f * (-1)));
                setVelocity(rotation);
                velocity.Y *= (-1);
                Game.bounceSound.Play(0.5f, 0, 0);
            }
        }

        public void setVelocity(float rotation)
        {
            velocity.X = (float)Math.Sin(rotation) * tangentVelocity;
            velocity.Y = (float)Math.Cos(rotation) * tangentVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
