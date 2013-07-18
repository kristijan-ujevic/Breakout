using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Breakout.Classes;

namespace Breakout
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        enum gameStates
        {
            mainMenu,
            playing,
            paused,
            highScores,
            gameOver
        }

        gameStates gameState;

        SpriteFont font;
        SpriteFont scoreFont;
        public static SpriteFont fadingTextFont;

        public static int level;

        public static Viewport screen = new Viewport();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Random rand = new Random();

        KeyboardState keyNewState;
        KeyboardState keyOldState;

        MouseState mouseNewState;
        MouseState mouseOldState;

        public static TimerBar tripleShotTimer;
        public static TimerBar shieldTimer;

        //menu
        Sprite startButton;
        Sprite highscoresButton;
        Sprite exitButton;

        List<HighScore> highscores = new List<HighScore>();
        string name = "player";  

        Sprite shield;
        Ball ball;
        Sprite background;
        public static Player player;
        public static Sprite borderLeft, borderRight;
        public static Sprite tripleShotIndicator, shieldIndicator;

        public static Brick[] bricks = new Brick[80];
        public static List<Bullet> bullets = new List<Bullet>();
        public static List<PowerUp> powerUps = new List<PowerUp>();
        public static List<Explosion> explosions = new List<Explosion>();
        public static List<FadingText> fadingText = new List<FadingText>();

        List<String> powerUpText = new List<String>();

        Texture2D startTexture;
        Texture2D highscoreTexture;
        Texture2D exitTexture;
        Texture2D playerTexture;
        Texture2D ballTexture;
        Texture2D brickTexture;
        Texture2D shieldTexture;
        Texture2D backgroundTexture;
        Texture2D borderTexture;
        Texture2D defaultTexture;

        public static Texture2D bulletTexture;     
        public static Texture2D powerUpTexture;
        public static Texture2D explodeTexture;

        public static SoundEffect explodeSound;
        public static SoundEffect bulletSound;
        public static SoundEffect tripleShotSound;
        public static SoundEffect pickupSound;
        public static SoundEffect bounceSound;
        public static SoundEffect powerDownSound;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            Window.AllowUserResizing = false;

            Content.RootDirectory = "Content";
            
            this.graphics.PreferredBackBufferWidth = 1000;
            this.graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
            gameState = gameStates.mainMenu;
        }

        protected override void Initialize()
        {
            base.Initialize();
            screen = GraphicsDevice.Viewport;

            //menu, offset = 75
            startButton = new Sprite(startTexture);
            startButton.rectangle.X = screen.Width / 2 - startButton.rectangle.Width / 2;
            startButton.rectangle.Y = screen.Height / 2 - startButton.rectangle.Height / 2 + 50;
            highscoresButton = new Sprite(highscoreTexture);
            highscoresButton.rectangle.X = screen.Width / 2 - startButton.rectangle.Width / 2;
            highscoresButton.rectangle.Y = screen.Height / 2 - startButton.rectangle.Height / 2 + 125;
            exitButton = new Sprite(exitTexture);
            exitButton.rectangle.X = screen.Width / 2 - startButton.rectangle.Width / 2;
            exitButton.rectangle.Y = screen.Height / 2 - startButton.rectangle.Height / 2 + 200;


            //game
            player = new Player(playerTexture);
            ball = new Ball(ballTexture);
            background = new Sprite(backgroundTexture);

            borderLeft = new Sprite(borderTexture);
            borderLeft.rectangle.X = 150 - borderLeft.rectangle.Width;
            borderRight = new Sprite(borderTexture);
            borderRight.rectangle.X = screen.Width - 150;

            shield = new Sprite(shieldTexture);
            shield.rectangle.X = borderLeft.rectangle.Right;
            shield.rectangle.Y = screen.Height - shield.rectangle.Height;
            
            tripleShotTimer = new TimerBar(defaultTexture);
            tripleShotTimer.rectangle.Y = screen.Height - tripleShotTimer.rectangle.Height;
            tripleShotTimer.rectangle.X = 50 - tripleShotTimer.rectangle.Width;

            tripleShotIndicator = new Sprite(powerUpTexture);
            tripleShotIndicator.rectangle = new Rectangle (42 * 7, 0, 42, 42);
            
            shieldTimer = new TimerBar(defaultTexture);
            shieldTimer.rectangle.Y = screen.Height - shieldTimer.rectangle.Height;
            shieldTimer.rectangle.X = 100 - shieldTimer.rectangle.Width;

            shieldIndicator = new Sprite(powerUpTexture);
            shieldIndicator.rectangle = new Rectangle(42 * 5, 0, 42, 42);

            placeBricks();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //menu
            startTexture = Content.Load<Texture2D>("startButton");
            highscoreTexture = Content.Load<Texture2D>("highscoresButton");
            exitTexture = Content.Load<Texture2D>("exitButton");

            //highscores
            loadHighScores();
            sortHighScores();

            //game
            playerTexture = Content.Load<Texture2D>("paddle");
            ballTexture = Content.Load<Texture2D>("ball");
            brickTexture = Content.Load<Texture2D>("brick");
            bulletTexture = Content.Load<Texture2D>("bullet");
            shieldTexture = Content.Load<Texture2D>("shield");
            powerUpTexture = Content.Load<Texture2D>("powerup");
            explodeTexture = Content.Load<Texture2D>("explode");
            backgroundTexture = Content.Load<Texture2D>("background");
            borderTexture = Content.Load<Texture2D>("border");

            font = Content.Load<SpriteFont>("NormalFont");
            fadingTextFont = Content.Load<SpriteFont>("FadingTextFont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");

            explodeSound = Content.Load<SoundEffect>("blast");
            bulletSound = Content.Load<SoundEffect>("shoot");
            tripleShotSound = Content.Load<SoundEffect>("tripleshot");
            pickupSound = Content.Load<SoundEffect>("pickup");
            bounceSound = Content.Load<SoundEffect>("bounce");
            powerDownSound = Content.Load<SoundEffect>("powerdown");

            defaultTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            defaultTexture.SetData<Color>(new Color[] { Color.White });
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            saveHighScores();
        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                case (gameStates.mainMenu):
                {
                    IsMouseVisible = true;
                    mouseOldState = mouseNewState;
                    mouseNewState = Mouse.GetState();

                    if (mouseNewState.LeftButton == ButtonState.Released && mouseOldState.LeftButton == ButtonState.Pressed)
                    {
                        if (startButton.rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            gameState = gameStates.playing;
                            reset();
                        }

                        else if (exitButton.rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            this.Exit();
                        }

                        else if (highscoresButton.rectangle.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                        {
                            gameState = gameStates.highScores;
                        }
                    }
                    break;
                }
                
                case (gameStates.playing):
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = gameStates.mainMenu;
                    }

                    if (!this.IsActive)
                    {
                        gameState = gameStates.paused;
                    }

                    IsMouseVisible = false;

                    if (player.lives < 0)
                    {
                        gameState = gameStates.gameOver;
                    }

                    keyOldState = keyNewState;
                    keyNewState = Keyboard.GetState();
                    if (ball.started)
                    {
                        ball.checkBrickCollisions();
                        ball.checkPlayerCollision(player);
                    }

                    ball.update(gameTime);

                    foreach (Brick element in bricks) //damaging the bricks after updating the ball to avoid multiple collisions
                    {
                        element.calculateBallDamage();
                    }

                    powerUps.RemoveAll(e => e.alive == false);
                    powerUps.ForEach(e => e.update());

                    shieldTimer.update(gameTime);
                    tripleShotTimer.update(gameTime);

                    player.update(gameTime);
                    if (keyNewState.IsKeyDown(Keys.Space) && keyOldState.IsKeyUp(Keys.Space) && ball.started == true)
                    {
                        player.shoot();
                    }

                    bullets.RemoveAll(e => e.alive == false);
                    bullets.ForEach(e => e.update());

                    explosions.RemoveAll(e => e.index >= 7);
                    explosions.ForEach(e => e.Update(gameTime));

                    fadingText.RemoveAll(e => e.color.A <= 40);
                    fadingText.ForEach(e => e.update());

                    if (levelClear())
                    {
                        nextLevel();
                    }
                    break;
                }

                case (gameStates.highScores):
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = gameStates.mainMenu;
                    }
                    break;
                }

                case (gameStates.paused):
                {
                    if(this.IsActive)
                    {
                        gameState = gameStates.playing;
                    }
                    break;
                }

                case (gameStates.gameOver):
                {
                    keyOldState = keyNewState;
                    keyNewState = Keyboard.GetState();

                    Keys[] pressedKeys;
                    pressedKeys = keyNewState.GetPressedKeys();

                    foreach (Keys key in pressedKeys)
                    {
                        if (keyOldState.IsKeyUp(key))
                        {
                            if (key == Keys.Back && name.Length > 0)
                            {
                                name = name.Remove(name.Length - 1, 1);
                            }
                            else if (key == Keys.Space)
                            {
                                name = name.Insert(name.Length, " ");
                            }
                            else if (key == Keys.Enter)
                            {
                                highscores.Add(new HighScore(name, player.score));
                                sortHighScores();
                                gameState = gameStates.mainMenu;
                            }
                            else if(name.Length < 10 && key.ToString().Length == 1)
                            {
                                name += key.ToString();
                            }
                        }   
                    } 
                    break;

                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            background.Draw(spriteBatch);

            switch (gameState)
            {
                case (gameStates.mainMenu):
                {
                    startButton.Draw(spriteBatch);
                    highscoresButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    break;
                }

                case (gameStates.playing):
                {
                    borderLeft.Draw(spriteBatch);
                    borderRight.Draw(spriteBatch);

                    tripleShotTimer.Draw(spriteBatch, Color.DarkViolet);
                    tripleShotIndicator.Draw(spriteBatch, new Vector2(12, screen.Height - shieldIndicator.rectangle.Height - 10), 1.3f);
                    shieldTimer.Draw(spriteBatch, Color.Blue);
                    shieldIndicator.Draw(spriteBatch, new Vector2(62, screen.Height - shieldIndicator.rectangle.Height - 10), 1.3f);

                    spriteBatch.DrawString(font, "Score:\n", new Vector2(10), Color.White);
                    spriteBatch.DrawString(font, player.score.ToString(), new Vector2(10, 40), Color.Yellow);
                    spriteBatch.DrawString(font, "Level:\n", new Vector2(10, 90), Color.White);
                    spriteBatch.DrawString(font, level.ToString(), new Vector2(10, 120), Color.Yellow);
                    spriteBatch.DrawString(font, "Balls:\n", new Vector2(880, 10), Color.White);
                    spriteBatch.DrawString(font, player.lives.ToString(), new Vector2(880, 40), Color.Yellow);
                    spriteBatch.DrawString(font, "Bullets:\n", new Vector2(880, 70), Color.AliceBlue);
                    spriteBatch.DrawString(font, player.numberOfBullets.ToString(), new Vector2(880, 100), Color.Yellow);

                    if(shieldTimer.currentTime > 0)
                    {
                        shield.Draw(spriteBatch);
                    }
                    foreach (Brick element in bricks)
                    {
                        if (element.alive == true)
                        {
                            element.Draw(spriteBatch);
                        }
                    }
                    bullets.ForEach(e => e.Draw(spriteBatch));   
                    powerUps.ForEach(e => e.Draw(spriteBatch));
                    fadingText.ForEach(e => e.Draw(spriteBatch));
                    explosions.ForEach(e => e.Draw(spriteBatch));

                    player.Draw(spriteBatch);
                    ball.Draw(spriteBatch);

                    break;
                }

                case (gameStates.highScores):
                {
                    Vector2 vector = new Vector2(screen.Width / 6, screen.Height / 9);
                    int count = 1;
                    spriteBatch.Draw(defaultTexture , new Rectangle(screen.Width / 2 - 450, screen.Height / 2 - 300, 900, 600), Color.DeepSkyBlue * .5f);
                    foreach(HighScore element in highscores)
                    {
                        spriteBatch.DrawString(scoreFont, count.ToString(), vector, Color.Yellow);
                        vector.X += 100;
                        spriteBatch.DrawString(scoreFont, element.name, vector, Color.Yellow);
                        vector.X += 470;
                        vector.X -= element.score.ToString().Length * 13;
                        spriteBatch.DrawString(scoreFont, element.score.ToString(), vector, Color.Yellow);

                        count++;
                        vector.Y += 50;
                        vector.X = screen.Width / 6;
                    }
                    break;
                }

                case (gameStates.paused):
                {
                    spriteBatch.DrawString(font, "Paused", new Vector2(screen.Width / 2 - 50, screen.Height / 2 - 10), Color.Yellow);
                    break;
                }

                case (gameStates.gameOver):
                {
                    spriteBatch.Draw(defaultTexture, new Rectangle(screen.Width / 2 - 450, screen.Height / 2 - 300, 900, 600), Color.DeepSkyBlue * .3f);
                    spriteBatch.DrawString(scoreFont, "Game Over", new Vector2(screen.Width / 2 - 120, screen.Height / 6 - 10), Color.Yellow);
                    spriteBatch.DrawString(scoreFont, "You Scored:", new Vector2(screen.Width / 2 - 140, screen.Height / 6 + 70), Color.Yellow);
                    spriteBatch.DrawString(scoreFont, player.score.ToString(), new Vector2(screen.Width / 2 - player.score.ToString().Length * 13, screen.Height / 6 + 160), Color.Yellow);
                    spriteBatch.DrawString(scoreFont, "Input your name:", new Vector2(screen.Width / 2 - 192, screen.Height / 2 + 80), Color.Yellow);
                    spriteBatch.DrawString(scoreFont, name, new Vector2(screen.Width / 2 - name.Length * 13, screen.Height / 2 + 140), Color.Yellow);
                    if (highscores.Count == 0 || player.score > highscores[0].score)
                    {
                        spriteBatch.DrawString(scoreFont, "NEW HIGHSCORE!", new Vector2(screen.Width / 2 - 160, screen.Height / 6 + 230), Color.Yellow);
                    }
                    break;
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        void placeBricks()
        {
            int tempX = borderLeft.rectangle.Right + 5;
            int tempY = 0;
            for (int i = 0; i < bricks.Length; i++)
            {
                bricks[i] = new Brick(brickTexture);
            }
            foreach (Brick element in bricks)
            {
                element.rectangle.X = tempX;
                element.rectangle.Y = tempY;
                if (tempX >= screen.Width + brickTexture.Width - 350)
                {
                    tempX = borderLeft.rectangle.Right + 5;
                    tempY += element.rectangle.Height;
                }
                else
                {
                    tempX += element.rectangle.Width * rand.Next(1, 3);
                }
            }
        }

        void loadHighScores()
        {
            System.IO.Stream stream = TitleContainer.OpenStream("Content\\highscores.xml");
            XDocument doc = XDocument.Load(stream);

            highscores = (from item in doc.Descendants("item")
                          select new HighScore(null, 0)
                          {
                              name = item.Element("name").Value,
                              score = Convert.ToInt32(item.Element("score").Value),
                          }).ToList();
            stream.Close();
        }

        void saveHighScores()
        {
            XmlTextWriter writer = new XmlTextWriter("Content\\highscores.xml", null);
            writer.Formatting = Formatting.Indented;

            writer.WriteRaw("<root>");
            foreach (HighScore element in highscores)
            {
                writer.WriteStartElement("item");
                writer.WriteElementString("name", element.name);
                writer.WriteElementString("score", element.score.ToString());
                writer.WriteFullEndElement();
            }
            writer.WriteRaw("\n</root>");
            writer.Close();
        }

        public void sortHighScores()
        {
            highscores.Sort((x, y) => y.score.CompareTo(x.score));  
            if (highscores.Count > 10)
            {
                highscores.RemoveRange(10, highscores.Count - 10);
            }
        }

        bool levelClear()
        {
            foreach (Brick element in bricks)
            {
                if (element.alive)
                {
                    return false;
                }
            }
            return true;
        }

        void nextLevel()
        {
            ball.initialize();
            bullets.Clear();
            powerUps.Clear();
            explosions.Clear();
            fadingText.Clear();
            placeBricks();
            Game.level++;

            fadingText.Add(new FadingText("LEVEL " + level + "!", new Vector2(screen.Width / 2 - 50, screen.Height / 2 + 150)));
        }

        void reset()
        {
            Game.level = 1;
            ball.initialize();
            bullets.Clear();
            powerUps.Clear();
            explosions.Clear();
            fadingText.Clear();
            player = new Player(playerTexture);
            placeBricks();
        }
    }
}
