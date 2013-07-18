using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Classes
{
    public class FadingText
    {
        String text;
        Vector2 position;
        public Color color = new Color();

        public FadingText(String newText, Vector2 newPosition)
        {
            text = newText;
            position = newPosition;
            color = Color.Yellow;
        }

        public void update()
        {
            color *= 0.99f;
            position.Y -= 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Game.fadingTextFont, text, position, color);
        }
    }
}
