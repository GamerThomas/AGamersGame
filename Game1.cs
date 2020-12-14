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


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerTxr, backgroundTxr1,backgroundTxr2, whiteBox, platformTxr, backTxr, badG1Txr,keyTxr,doorTxr,blankTxr,healthTxr,fallTxr;

        Point screenSize = new Point(1280, 500);
        public int levelNumber = 0;
        public int spikeNum = 0;

        int lives = 3;

        PlayerSprite playerSprite;
        BackSprite backSprite;
        DoorSprite doorSprite;
        NextSprite nextSprite;

        List<HealthSprite> healthSprites = new List<HealthSprite>();

        List<List<PlatformSprite>> levels = new List<List<PlatformSprite>>();
        List<List<BadSprite1>>badSprite1 = new List<List<BadSprite1>>();
        List<List<KeySprite>> keySprite = new List<List<KeySprite>>();
        List<List<SpikeSprite>> spikeSprites = new List<List<SpikeSprite>>();

        public bool openDoor = false;

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
            backgroundTxr2 = Content.Load<Texture2D>("Background2");
            platformTxr = Content.Load<Texture2D>("platSheet1");
            backTxr = Content.Load<Texture2D>("BackAnim");
            badG1Txr = Content.Load<Texture2D>("BadGuySheet1");
            keyTxr = Content.Load<Texture2D>("KeySheet");
            doorTxr = Content.Load<Texture2D>("Door");
            blankTxr = Content.Load<Texture2D>("Blank");
            healthTxr = Content.Load<Texture2D>("LivesSheet");
            fallTxr = Content.Load<Texture2D>("FallTxr");


            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });




            playerSprite = new PlayerSprite(playerTxr, whiteBox, new Vector2(50, 50));
            backSprite = new BackSprite(backTxr, whiteBox, new Vector2(780, 135));
            doorSprite = new DoorSprite(doorTxr, whiteBox, new Vector2(1205, 300));
            nextSprite = new NextSprite(blankTxr,whiteBox,new Vector2(1260,300));




            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1190, 0)));
            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1220, 0)));
            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1250, 0)));

            BuildLevels();
            AddKeys();
            AddEnemy();
            AddSpike();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp)) levelNumber = 1;

            
            
            playerSprite.Update(gameTime, levels[levelNumber], badSprite1[levelNumber], doorSprite, keySprite[levelNumber],nextSprite,spikeSprites[levelNumber]);


                



            
            if (levelNumber == 1)
            {
                foreach (PlatformSprite platform in levels[levelNumber])
                { 
                    platform.setAnim(1);
                }

            }
            else
            {
                foreach (PlatformSprite platform in levels[levelNumber])
                {
                    platform.setAnim(0);
                }
            }

            
            
            
            foreach (BadSprite1 badSprite in badSprite1[levelNumber])
            {
                badSprite.Update(gameTime, levels[levelNumber]);
            }
            
            
            foreach (HealthSprite health in healthSprites)
            {
                health.Update();
            }

            foreach (SpikeSprite spike in spikeSprites[levelNumber])
            {
                spike.Update(gameTime);
                if (spike.spritePos.Y > screenSize.Y + 10) spike.Reset();
            }


            foreach (KeySprite keys in keySprite[levelNumber])
            {
                if(keys.hideKey)
                {
                    keys.setAnim(1);
                    keys.isColliding = false;
                }
                if(keys.open)
                {
                    openDoor = true;
                }
                else
                {
                    keys.setAnim(0);
                    keys.isColliding = true;
                    openDoor = false;
                }
            }

            
            
            if (playerSprite.nextLevel == true)
            {
                playerSprite.ResetPlayer(new Vector2(60, 60));
                levelNumber++;
            }



            if(openDoor == false && levelNumber ==1)
            {
                doorSprite.Move(new Vector2(50,530));
            }



            if (openDoor)doorSprite.Update();



            if (playerSprite.spritePos.Y > screenSize.Y + 50)
            {
                playerSprite.ResetPlayer(new Vector2(50, 50));
                lives--;
            }
            if (playerSprite.playerDead)
            {
                playerSprite.ResetPlayer(new Vector2(50, 50));
                lives--;
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (levelNumber == 0)
            {
                _spriteBatch.Draw(backgroundTxr1, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);
            }
            else if(levelNumber == 1)
            {
                _spriteBatch.Draw(backgroundTxr2, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);
            }
            else
            {
                _spriteBatch.Draw(backgroundTxr1, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);
            }


           if(levelNumber == 0) backSprite.Draw(_spriteBatch, gameTime);

            foreach (BadSprite1 badSprite in badSprite1[levelNumber]) badSprite.Draw(_spriteBatch, gameTime);


            nextSprite.Draw(_spriteBatch, gameTime);
            doorSprite.Draw(_spriteBatch, gameTime);
        
            foreach (KeySprite key in keySprite[levelNumber]) key.Draw(_spriteBatch, gameTime);
            
            foreach (SpikeSprite spike in spikeSprites[levelNumber]) spike.Draw(_spriteBatch, gameTime);
            
            playerSprite.Draw(_spriteBatch, gameTime);


            foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);

            if (lives == 3)
            {
                foreach (HealthSprite health in healthSprites) health.Draw(_spriteBatch, gameTime);
            }
            else if (lives == 2)
            {
                healthSprites[0].Draw(_spriteBatch, gameTime);
                healthSprites[1].Draw(_spriteBatch, gameTime);
            }
            else if (lives == 1)
            {
                healthSprites[0].Draw(_spriteBatch, gameTime);
            }
            else
            {
                playerSprite.ResetPlayer(new Vector2(50, 50));
                lives = 3;
                levelNumber = 0;
                foreach (BadSprite1 badguy in badSprite1[levelNumber]) badguy.enDead = false;

            }

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

            levels.Add(new List<PlatformSprite>());
            levels[1].Add(new PlatformSprite(platformTxr,whiteBox,new Vector2(0,50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(100, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(200, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(300, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(400, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(500, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(600, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(700, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(800, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(900, 50)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1000, 50)));
            
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1300, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1200, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1100, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1000, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(900, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(800, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(700, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(600, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(500, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(400, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(300, 200)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(200, 200)));

            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(0, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(100, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(200, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(300, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(400, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(500, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(600, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(700, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(800, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(900, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1000, 350)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1100, 350)));

            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(0, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(100, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(200, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(300, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(400, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(500, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(600, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(700, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(800, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(900, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1000, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1100, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1200, 470)));
            levels[1].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1300, 470)));




        }

        void AddEnemy()
        {
            badSprite1.Add(new List<BadSprite1>());
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1050, 250)));
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(250, 250)));

            badSprite1.Add(new List<BadSprite1>());
            badSprite1[1].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1050, 250)));
        }

        void AddKeys()
        {
            keySprite.Add(new List<KeySprite>());
            keySprite[0].Add(new KeySprite(keyTxr,whiteBox,new Vector2(30, 298)));

            keySprite.Add(new List<KeySprite>());
            keySprite[1].Add(new KeySprite(keyTxr, whiteBox, new Vector2(30, 298)));
        }

        void AddSpike()
        {
            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[0].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2 (- 100, -100)));
            
            
            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(1100, 0)));
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(1150, 0)));

            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(50, 100)));
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(100, 100)));
        }

    }
}
