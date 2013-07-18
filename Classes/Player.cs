using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout.Classes
{
    public class Player : Sprite
    {
        public int numberOfBullets = 100;
        public int bulletDamage = 1;
        public int ballDamage = 1;
        public int lives = 3;
        public int score = 0;

        float momentum = 1;

        public Player(Texture2D newTexture) : base (newTexture)
        {
            rectangle.Y = Game.screen.Height - rectangle.Height;
            rectangle.X = Game.screen.Width / 2 - rectangle.Width / 2;
        }

        public void update(GameTime gameTime)
        {
            momentum = MathHelper.Clamp(momentum, 0, 10);
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                rectangle.X -= (int)momentum;
                momentum += (float)gameTime.ElapsedGameTime.TotalSeconds * 40;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                rectangle.X += (int)momentum;
                momentum += (float)gameTime.ElapsedGameTime.TotalSeconds * 40;
            }
            else
            {
                momentum = 1;
            }

            rectangle.X = (int)(MathHelper.Clamp(rectangle.X, 150, Game.screen.Width - rectangle.Width - 150));
            
            checkPowerUpCollision(Game.powerUps);
        }

        public void shoot()
        {
            if (numberOfBullets > 0)
            {
                Game.bulletSound.Play();
                Game.bullets.Add(new Bullet(Game.bulletTexture, this, 0));
                if (Game.tripleShotTimer.currentTime > 0 && numberOfBullets > 2)
                {
                    Game.bullets.Add(new Bullet(Game.bulletTexture, this, -5));
                    Game.bullets.Add(new Bullet(Game.bulletTexture, this, 5));
                    Game.tripleShotSound.Play(0.4f, 0, 0);
                    numberOfBullets--;
                }
                numberOfBullets--;                   
            }
        }

        public void increaseScore(int amount)
        {
            score += amount;
        }

        void checkPowerUpCollision(List<PowerUp> powerUps)
        {
            foreach (PowerUp element in powerUps)
            {
                if(rectangle.Intersects(element.boundingRectangle) && element.alive == true)
                {
                    element.alive = false;
                    givePowerUp(element);
                }
            }
        }

        void givePowerUp(PowerUp powerUp)
        {
            switch (powerUp.type)
            {
                case PowerUp.powerUpType.ballDamage:
                    ballDamage += 1;
                    Game.fadingText.Add(new FadingText("Ball Damage!", powerUp.position));
                    break;
                case PowerUp.powerUpType.extraBullet:
                    //higher level = more bullets given
                    numberOfBullets += (Game.level + 15);
                    Game.fadingText.Add(new FadingText("Ammo!", powerUp.position));
                    break;
                case PowerUp.powerUpType.extraScore:
                    increaseScore(100 * Game.level);
                    Game.fadingText.Add(new FadingText(("+" + (100 * Game.level).ToString() + "!"), powerUp.position));
                    break;
                case PowerUp.powerUpType.extraLife:
                    if (lives < 5)
                    {
                        lives++;
                        Game.fadingText.Add(new FadingText("Extra Life!", powerUp.position));
                    }
                    else
                    {
                        Game.player.increaseScore(100 * Game.level);
                        Game.fadingText.Add(new FadingText(("+" + (100 * Game.level).ToString() + "!"), powerUp.position));
                    }
                    break;
                case PowerUp.powerUpType.bulletDamage:
                    bulletDamage += 1;
                    Game.fadingText.Add(new FadingText("Bullet Damage!", powerUp.position));
                    break;
                case PowerUp.powerUpType.shield:
                    Game.shieldTimer.currentTime += 5;
                    Game.fadingText.Add(new FadingText("Shield!", powerUp.position));
                    break;
                case PowerUp.powerUpType.tripleShot:
                    Game.tripleShotTimer.currentTime += 15;
                    Game.fadingText.Add(new FadingText("Triple Shot!", powerUp.position));
                    break;
                case PowerUp.powerUpType.penalty:
                    giveRandomPenalty(powerUp.position);
                    return;
            } 
            Game.pickupSound.Play();
        }

        void giveRandomPenalty(Vector2 position)
        {
            int value = rand.Next(40);
            if(value < 10)
            {
                increaseScore(-100 * Game.level);
            }
            else if (value < 20)
            {
                numberOfBullets = numberOfBullets / 2;
            }
            else if (value < 30)
            {
                Game.tripleShotTimer.currentTime = 0;
                Game.shieldTimer.currentTime -= 5;
            }
            else if (value < 40)
            {
                if (Game.player.lives > 2)
                {
                    Game.player.lives--;
                }
                else
                {
                    numberOfBullets = numberOfBullets / 2;
                }
            }
            Game.player.numberOfBullets = (int)MathHelper.Clamp(Game.player.numberOfBullets, 0, int.MaxValue);
            Game.fadingText.Add(new FadingText("Sucker!", position));
            Game.powerDownSound.Play();
        }
    }
}
