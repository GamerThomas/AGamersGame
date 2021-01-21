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

    //Add More Levels
    //Add Harder Enemies


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Random rng = new Random();

        SpriteFont uiFont,bigFont;

        Texture2D playerTxr, backgroundTxr1, backgroundTxr2,backgroundTxr3,backgroundTxr4, whiteBox, platformTxr, backTxr, backTxr2,badG1Txr, keyTxr, doorTxr, blankTxr, healthTxr, fallTxr,wallTxr,arrowTxr,badG2Txr;

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
        List<List<BadSprite1>> badSprite1 = new List<List<BadSprite1>>();
        List<List<KeySprite>> keySprite = new List<List<KeySprite>>();
        List<List<SpikeSprite>> spikeSprites = new List<List<SpikeSprite>>();
        List<List<WallSprite>> wallSprites = new List<List<WallSprite>>();
        List<List<ArrowSprite>> arrowSprites = new List<List<ArrowSprite>>();
        List<List<BadSprite2>> badSprite2 = new List<List<BadSprite2>>();
        List<List<navSprite>> navSprites = new List<List<navSprite>>();

        public bool openDoor = false;

        float playTime = 0;


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
            backgroundTxr3 = Content.Load<Texture2D>("Background3");
            backgroundTxr4 = Content.Load<Texture2D>("Background4");
            platformTxr = Content.Load<Texture2D>("platSheet1");
            backTxr = Content.Load<Texture2D>("BackAnim");
            backTxr2 = Content.Load<Texture2D>("BackAnim2");
            badG1Txr = Content.Load<Texture2D>("BadGuySheet1");
            badG2Txr = Content.Load<Texture2D>("BadGuySheet2");
            keyTxr = Content.Load<Texture2D>("KeySheet");
            doorTxr = Content.Load<Texture2D>("Door");
            blankTxr = Content.Load<Texture2D>("Blank");
            healthTxr = Content.Load<Texture2D>("LivesSheet");
            fallTxr = Content.Load<Texture2D>("FallTxr");
            wallTxr = Content.Load<Texture2D>("WallSheet");
            arrowTxr = Content.Load<Texture2D>("Arrow");
            uiFont = Content.Load<SpriteFont>("UiFont");
            bigFont = Content.Load<SpriteFont>("BigFont");


            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });




            playerSprite = new PlayerSprite(playerTxr, whiteBox, new Vector2(50, 50));
            doorSprite = new DoorSprite(doorTxr, whiteBox, new Vector2(1205, 300));
            nextSprite = new NextSprite(blankTxr, whiteBox, new Vector2(1260, 300));
            backSprite = new BackSprite(backTxr, whiteBox, new Vector2(780, 135));
            
      

            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1190, 0)));
            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1220, 0)));
            healthSprites.Add(new HealthSprite(healthTxr, whiteBox, new Vector2(1250, 0)));

            BuildLevels();
            AddKeys();
            AddEnemy();
            AddSpike();
            AddWalls();
            AddArrows();
            AddNav();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            { 
                levelNumber = 1;
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                levelNumber = 2;
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Home))
            {
                levelNumber = 3;
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }


            playerSprite.Update(gameTime, levels[levelNumber], badSprite1[levelNumber], doorSprite, keySprite[levelNumber],nextSprite,spikeSprites[levelNumber],wallSprites[levelNumber],arrowSprites[levelNumber],badSprite2[levelNumber]) ;

            if(levelNumber == 1)
            {
                nextSprite.level2();
                doorSprite.Move(new Vector2(50, 530));
            }
            if(levelNumber ==0)
            {
                nextSprite.level1();
                doorSprite.level1();
            }
            if (levelNumber ==2)
            {
                doorSprite.Move(new Vector2(1200, 475));
                nextSprite.level3();
            }
            if(levelNumber == 3)
            {
                doorSprite.Move(new Vector2(-200, -200));
            }




            //When its level 2 change to other platform sprite
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

            foreach (BadSprite2 badSprite in badSprite2[levelNumber])
            {
                badSprite.update(gameTime, levels[levelNumber], navSprites[levelNumber]);
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

            foreach(ArrowSprite arrow in arrowSprites[levelNumber])
            {
                arrow.update(gameTime);
                if (arrow.spritePos.X < screenSize.X - 1300) arrow.Reset();
                if (arrow.leftArrow && arrow.spritePos.X > screenSize.X +20 ) arrow.Reset();

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

            
            
            if (playerSprite.nextLevel == true && levelNumber <3)
            {
                playerSprite.ResetPlayer(new Vector2(60, 60));
                levelNumber++;
            }


            if (openDoor)doorSprite.Update();


            //If the player falls out the map remove a life
            if (playerSprite.spritePos.Y > screenSize.Y + 50)
            {
                playerSprite.ResetPlayer(new Vector2(50, 50));
                lives--;
            }
            //If the player is dead reset the player and remove a life
            if (playerSprite.playerDead)
            {
                playerSprite.ResetPlayer(new Vector2(50, 50));
                lives--;
            }

            playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;


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
            else if(levelNumber ==2)
            {
                _spriteBatch.Draw(backgroundTxr3, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);
            }
            else if(levelNumber ==3)
            {
                _spriteBatch.Draw(backgroundTxr4, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);
                playerSprite.Stop();
                foreach (BadSprite1 badguy in badSprite1[levelNumber]) badguy.Win();
            }
            //Setting backgroun animation for lev1 and 2
            if (levelNumber == 0)
            {
                backSprite.Draw(_spriteBatch, gameTime);
                backSprite.move1();
            }
            if (levelNumber == 2)
            {
                backSprite.Draw(_spriteBatch, gameTime);
                backSprite.move2(new Vector2(1201, 419));
            }

            foreach (BadSprite1 badSprite in badSprite1[levelNumber]) badSprite.Draw(_spriteBatch, gameTime);

            foreach (BadSprite2 badSprite in badSprite2[levelNumber]) badSprite.Draw(_spriteBatch, gameTime);

            nextSprite.Draw(_spriteBatch, gameTime);
            doorSprite.Draw(_spriteBatch, gameTime);
        
            foreach (KeySprite key in keySprite[levelNumber]) key.Draw(_spriteBatch, gameTime);
            
            foreach (SpikeSprite spike in spikeSprites[levelNumber]) spike.Draw(_spriteBatch, gameTime);

            foreach (navSprite nav in navSprites[levelNumber] )nav.Draw(_spriteBatch,gameTime);

            

            playerSprite.Draw(_spriteBatch, gameTime);


            if (levelNumber < 3)
            {
                foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);
            }
            foreach (WallSprite walls in wallSprites[levelNumber]) walls.Draw(_spriteBatch, gameTime);

            foreach (ArrowSprite arrow in arrowSprites[levelNumber]) arrow.Draw(_spriteBatch, gameTime);

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
                foreach (KeySprite keys in keySprite[0]) keys.open = false;
                foreach (KeySprite keys in keySprite[1]) keys.open = false;
                foreach (KeySprite keys in keySprite[2]) keys.open = false;
                foreach (BadSprite1 badguy in badSprite1[0]) badguy.enDead = false;
                foreach (BadSprite1 badguy in badSprite1[1]) badguy.enDead = false;
                foreach (BadSprite1 badguy in badSprite1[2]) badguy.enDead = false;
                foreach (BadSprite2 badguy in badSprite2[0]) badguy.Alive();
                foreach (BadSprite2 badguy in badSprite2[1]) badguy.Alive();
                foreach (BadSprite2 badguy in badSprite2[2]) badguy.Alive();

            }

            if (levelNumber < 3)
            {
                _spriteBatch.DrawString(uiFont, "Level: " + (levelNumber + 1), new Vector2(12, 12), Color.Black);

                _spriteBatch.DrawString(uiFont, "Level: " + (levelNumber + 1), new Vector2(10, 10), Color.White);
            }
            else if(levelNumber ==3)
            {
                _spriteBatch.DrawString(uiFont, "Level: END" , new Vector2(12, 12), Color.Black);

                _spriteBatch.DrawString(uiFont, "Level: END", new Vector2(10, 10), Color.White);

                _spriteBatch.DrawString(bigFont, "YOU WIN", new Vector2(377, 147), Color.Cyan);

                _spriteBatch.DrawString(bigFont, "YOU WIN", new Vector2(378, 148), Color.HotPink);

                _spriteBatch.DrawString(bigFont, "YOU WIN", new Vector2(380, 150), Color.Yellow);

                _spriteBatch.DrawString(uiFont, "Press ESC To Quit", new Vector2(737, 297), Color.Cyan);

                _spriteBatch.DrawString(uiFont, "Press ESC To Quit", new Vector2(738, 298), Color.HotPink);

                _spriteBatch.DrawString(uiFont, "Press ESC To Quit", new Vector2(740, 300), Color.Yellow);

            }
            _spriteBatch.DrawString(uiFont, "Time: " + Math.Round(playTime), new Vector2(10, 32), Color.Black);

            _spriteBatch.DrawString(uiFont, "Time: " + Math.Round(playTime), new Vector2(10, 30), Color.White);

            _spriteBatch.End();


            base.Draw(gameTime);
        }

        void BuildLevels()
        {
            //Platforms level 1
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
            //Platforms level 2
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

            //Platforms level 3
            levels.Add(new List<PlatformSprite>());
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(50, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(150, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(450, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(550, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(650, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(750, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(850, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(950, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1050, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1150, 60)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1250, 60)));

            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(50, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(150, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(250, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(350, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(450, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(550, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(650, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(750, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(850, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(950, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1050, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1150, -40)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1250, -40)));

            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(200, 210)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(300, 210)));

            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(350, 360)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(450, 360)));

            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(550, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(650, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(750, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(850, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(950, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1050, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1150, 475)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1250, 475)));

            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(525, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(625, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(725, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(750, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(825, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(925, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1025, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1125, 260)));
            levels[2].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1225, 260)));

            levels.Add(new List<PlatformSprite>());
            levels[3].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(50, 250)));
            levels[3].Add(new PlatformSprite(platformTxr, whiteBox, new Vector2(1200, 250)));

        }

        void AddWalls()
        {
            wallSprites.Add(new List<WallSprite>());
            //walls on the left of the screen lev 1
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 0)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 50)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 100)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 150)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 200)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 250)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 300)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 350)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 400)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 450)));
            //wall on the right of the screen lev1
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 0)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 50)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 100)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 150)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 200)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 250)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 300)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 350)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 400)));
            wallSprites[0].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 450)));

            wallSprites.Add(new List<WallSprite>());
            //walls on the left of the screen lev 2
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 0)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 50)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 100)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 150)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 200)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 250)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 300)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 350)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 400)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 450)));
            //walls on the right of the screen lev 2
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 0)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 50)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 100)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 150)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 200)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 250)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 300)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 350)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 400)));
            wallSprites[1].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 450)));

            wallSprites.Add(new List<WallSprite>());
            //walls on the left of the screen lev 3
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 0)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 50)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 100)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 150)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 200)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 250)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 300)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 350)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 400)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-20, 450)));
            //walls on the left of the screen lev 3
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 0)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 50)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 100)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 150)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 200)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 250)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 300)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 350)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 400)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(1300, 450)));

            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(175, 110)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(450, 110)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(450, 210)));
            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(325, 260)));

            wallSprites[2].Add(new WallSprite(wallTxr, whiteBox, new Vector2(475, 410)));

            wallSprites.Add(new List<WallSprite>());
            wallSprites[3].Add(new WallSprite(wallTxr, whiteBox, new Vector2(-200, -100)));
        }
        

        void AddEnemy()
        {
            badSprite1.Add(new List<BadSprite1>());
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1050, 250)));
            badSprite1[0].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(250, 250)));

            badSprite1.Add(new List<BadSprite1>());
            badSprite1[1].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1000, 230)));
            badSprite1[1].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(600, 360)));

            badSprite1.Add(new List<BadSprite1>());
            badSprite1[2].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1050, 250)));

            badSprite1.Add(new List<BadSprite1>());
            badSprite1[3].Add(new BadSprite1(badG1Txr, whiteBox, new Vector2(1200, 50)));


            badSprite2.Add(new List<BadSprite2>());
            badSprite2[0].Add(new BadSprite2(badG2Txr, whiteBox, new Vector2(2000, -100)));

            badSprite2.Add(new List<BadSprite2>());
            badSprite2[1].Add(new BadSprite2(badG2Txr, whiteBox, new Vector2(600, 450)));

            badSprite2.Add(new List<BadSprite2>());
            badSprite2[2].Add(new BadSprite2(badG2Txr, whiteBox, new Vector2(750, 400)));

            badSprite2.Add(new List<BadSprite2>());
            badSprite2[3].Add(new BadSprite2(badG2Txr, whiteBox, new Vector2(2000, 400)));
        }

        void AddKeys()
        {
            keySprite.Add(new List<KeySprite>());
            keySprite[0].Add(new KeySprite(keyTxr,whiteBox,new Vector2(30, 298)));

            keySprite.Add(new List<KeySprite>());
            keySprite[1].Add(new KeySprite(keyTxr, whiteBox, new Vector2(100, 350)));

            keySprite.Add(new List<KeySprite>());
            keySprite[2].Add(new KeySprite(keyTxr, whiteBox, new Vector2(1000, 50)));

            keySprite.Add(new List<KeySprite>());
            keySprite[3].Add(new KeySprite(keyTxr, whiteBox, new Vector2(-100, -100)));
        }

        void AddSpike()
        {
            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[0].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2 (- 100, -100),10f));
            
            
            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(1100, 0),130f));
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(1150, 0),170f));

            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(50, 100),190f));
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(100, 100),210f));

            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(500, 0),200f));
            spikeSprites[1].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(800, 0),150f));

            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[2].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(-100, -100), 10f));
            
            spikeSprites.Add(new List<SpikeSprite>());
            spikeSprites[3].Add(new SpikeSprite(fallTxr, whiteBox, new Vector2(-100, -100), 10f));
        }

        void AddArrows()
        {
            int speed = rng.Next(150, 400);
            arrowSprites.Add(new List<ArrowSprite>());
            arrowSprites[0].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(-100, -100), 500f,false));

            arrowSprites.Add(new List<ArrowSprite>());
            arrowSprites[1].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(-100, -100), 500f, false));

            arrowSprites.Add(new List<ArrowSprite>());
            arrowSprites[2].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(1280, 50), (float)speed, false));
            arrowSprites[2].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(0, 200), (float)speed, true));
            arrowSprites[2].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(1280, 350), (float)speed, false));
            arrowSprites[2].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(0, 475), (float)speed, true));

            arrowSprites.Add(new List<ArrowSprite>());
            arrowSprites[3].Add(new ArrowSprite(arrowTxr, whiteBox, new Vector2(-100, -100), (float)speed, false));

        }

        void AddNav()
        {
            navSprites.Add(new List<navSprite>());
            navSprites[0].Add(new navSprite(blankTxr, wallTxr, new Vector2()));

            navSprites.Add(new List<navSprite>());
            navSprites[1].Add(new navSprite(blankTxr, wallTxr, new Vector2(1100, 475)));
            navSprites[1].Add(new navSprite(blankTxr, wallTxr, new Vector2(150, 475)));

            navSprites.Add(new List<navSprite>());
            navSprites[2].Add(new navSprite(blankTxr, wallTxr, new Vector2(1100, 475)));
            navSprites[2].Add(new navSprite(blankTxr, wallTxr, new Vector2(550, 475)));

            navSprites.Add(new List<navSprite>());
            navSprites[3].Add(new navSprite(blankTxr, wallTxr, new Vector2(-200, -200)));


        }

    }
}
