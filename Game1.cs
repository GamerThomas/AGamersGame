using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace AGamersGame
{

    //Add Attacking
    //Add More Levels
    //Add Harder Enemies
    //Add Puzzle(Get Key Open Door)
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerTxr, backgroundTxr1, whiteBox, platformTxr, backTxr, badG1Txr;

        Point screenSize = new Point(1280, 500);
        int levelNumber = 0;

        PlayerSprite playerSprite;
        BackSprite backSprite;


        List<List<PlatformSprite>> levels = new List<List<PlatformSprite>>();
        List<List<BadSprite1>>badSprite1 = new List<List<BadSprite1>>();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTxr = Content.Load<Texture2D>("SpriteSheetT");
            backgroundTxr1 = Content.Load<Texture2D>("Background1");
            platformTxr = Content.Load<Texture2D>("platSheet1");
            backTxr = Content.Load<Texture2D>("BackAnim");
            badG1Txr = Content.Load<Texture2D>("BadGuySheet1");

            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });


            playerSprite = new PlayerSprite(playerTxr, whiteBox, new Vector2(50, 50));
            backSprite = new BackSprite(backTxr, whiteBox, new Vector2(780, 135));

            
            BuildLevels();
            AddEnemy();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            playerSprite.Update(gameTime, levels[levelNumber],badSprite1[levelNumber]);

            foreach (BadSprite1 badSprite in badSprite1[levelNumber])
            {
                badSprite.Update(gameTime, levels[levelNumber]);
            }


            if (playerSprite.spritePos.Y > screenSize.Y + 50) playerSprite.ResetPlayer(new Vector2(50, 50));
            if (playerSprite.playerDead) playerSprite.ResetPlayer(new Vector2(50, 50));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTxr1, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);

            backSprite.Draw(_spriteBatch, gameTime);

            foreach (BadSprite1 badSprite in badSprite1[levelNumber]) badSprite.Draw(_spriteBatch, gameTime);

            playerSprite.Draw(_spriteBatch, gameTime);


            foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);


            _spriteBatch.End();


            base.Draw(gameTime);
        }

        void BuildLevels()
        {
            levels.Add(new List<PlatformSprite>());
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(50, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(150, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(250, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(350, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(450, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(650, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(750, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(850, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(950, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1050, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1150, 300)));
            levels[0].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1250, 300)));

        }

        void AddEnemy()
        {
            badSprite1.Add(new List<BadSprite1>());
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1050, 250)));
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(250, 250)));
        }
    }
}
