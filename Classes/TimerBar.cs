using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class TimerBar : Sprite
    {
        public int currentTime;
        float seconds;

        public TimerBar(Texture2D newTexture) : base(newTexture)
        {
            currentTime = 0;
            seconds = 0f;
            rectangle = new Rectangle(0, 0, 20, 150);
        }

        public void update(GameTime gameTime)
        {
            currentTime = (int)MathHelper.Clamp(currentTime, 0, 15);
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= 1)
            {
                currentTime -= 1;
                seconds = 0;
            }
            rectangle.Height = currentTime * 12;
            rectangle.Y = Game.screen.Height - currentTime * 12 - 42;
        }
    }
}
