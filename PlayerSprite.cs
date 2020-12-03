using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Collections.Generic;
using System;


namespace AGamersGame
{
    class PlayerSprite : Sprite
    {
        bool jumping, walking, falling, jumpIsPressed, sprint,attack;

        public bool playerDead = false;


        const float jumpSpeed = 5f;
        const float walkSpeed = 100f;
        const float sprintSpeed = 150f;

        Vector2 NormalCollisonMin = new Vector2(0.25f, 0.1f);
        Vector2 NormalCollisonMax = new Vector2(0.25f, 0.1f);

        Vector2 attackHitMin = new Vector2(0.01f,0.1f);
        Vector2 attackHitMax = new Vector2(0.01f,0.1f);
        
        public PlayerSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            //drawCollision = true;

            collisionInsetMin = NormalCollisonMin;
            collisionInsetMax = NormalCollisonMax;

            frameTime = 0.2f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());//Idle Anim
            animations[0].Add(new Rectangle(0, 0, 50, 50));
            animations[0].Add(new Rectangle(0, 0, 50, 50));
            animations[0].Add(new Rectangle(50, 0, 50, 50));


            animations.Add(new List<Rectangle>());//Falling Anim
            animations[1].Add(new Rectangle(100, 0, 50, 50));
            animations[1].Add(new Rectangle(150, 0, 50, 50));

            animations.Add(new List<Rectangle>());//Jumping Anim
            animations[2].Add(new Rectangle(100, 0, 50, 50));

            animations.Add(new List<Rectangle>());//Walking Anim
            animations[3].Add(new Rectangle(0, 50, 50, 50));
            animations[3].Add(new Rectangle(50, 50, 50, 50));

            animations.Add(new List<Rectangle>());//Sprint Anim
            animations[4].Add(new Rectangle(100, 50, 50, 50));
            animations[4].Add(new Rectangle(150, 50, 50, 50));

            animations.Add(new List<Rectangle>());//Attack Anim
            animations[5].Add(new Rectangle(200, 0, 50, 50));
            animations[5].Add(new Rectangle(251, 0, 50, 50));


            attack = false;
            sprint = false;
            jumping = false;
            walking = false;
            falling = true;
            jumpIsPressed = false;

        }


        public void Update(GameTime gameTime, List<PlatformSprite> platforms,List<BadSprite1> badSprite1)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!jumpIsPressed && !jumping && !falling && (keyboardState.IsKeyDown(Keys.Space)))
            {
                attack = false;
                jumpIsPressed = true;
                jumping = true;
                walking = false;
                falling = false;
                spriteVelocity.Y -= jumpSpeed;
            }
            else if (jumpIsPressed && !jumping && !falling && !(keyboardState.IsKeyDown(Keys.Space)))
            {
                jumpIsPressed = false;
            }



            if (keyboardState.IsKeyDown(Keys.A))
            {
                attack = false;
                walking = true;
                spriteVelocity.X = -walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                flipped = true;
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    walking = false;
                    sprint = true;
                    spriteVelocity.X = -sprintSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else if (keyboardState.IsKeyDown(Keys.D))
            {
                attack = false;
                walking = true;
                spriteVelocity.X = +walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                flipped = false;
                if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    walking = false;
                    sprint = true;
                    spriteVelocity.X = +sprintSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else
            {
                attack = false;
                walking = false;
                sprint = false;
                spriteVelocity.X = 0;
            }

            if (keyboardState.IsKeyDown(Keys.F))
            {
                attack = true;
                walking = false;
                sprint = false;
                spriteVelocity.X = 0;
            }

            if (attack)
            {
                collisionInsetMin = attackHitMin;
                collisionInsetMax = attackHitMax;
            }
            else
            {
                collisionInsetMin = NormalCollisonMin;
                collisionInsetMax = NormalCollisonMax;
            }

            if ((falling || jumping) && spriteVelocity.Y < 500f) spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spritePos += spriteVelocity;

            bool hasCollided = false;

            foreach (PlatformSprite platform in platforms)
            {
                if (checkCollisionBelow(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y--;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = false;
                }
                else if (checkCollisionAbove(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y++;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = true;
                }
                if (checkCollisionLeft(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X--;
                    spriteVelocity.X = 0;
                }
                else if (checkCollisionRight(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X++;
                    spriteVelocity.X = 0;
                }


                if (attack)
                {
                    foreach (BadSprite1 badGuy in badSprite1)
                    {
                        if (checkCollisionBelow(badGuy))
                        {
                            badGuy.enDead = true;
                        }
                        else if (checkCollisionAbove(badGuy))
                        {
                            badGuy.enDead = true;
                        }
                        if (checkCollisionLeft(badGuy))
                        {
                            badGuy.enDead = true;
                        }
                        else if (checkCollisionRight(badGuy))
                        {
                           badGuy.enDead = true;
                        }
                    }
                }
                else
                {
                    foreach (BadSprite1 badGuy in badSprite1)
                    {
                        if (checkCollisionBelow(badGuy))
                        {
                            playerDead = true;
                        }
                        else if (checkCollisionAbove(badGuy))
                        {
                            playerDead = true;
                        }
                        if (checkCollisionLeft(badGuy))
                        {
                            playerDead = true;
                        }
                        else if (checkCollisionRight(badGuy))
                        {
                            playerDead = true;
                        }
                    }
                }

                if (!hasCollided && (walking || sprint)) falling = true;

                if (walking) setAnim(3);
                else if (sprint) setAnim(4);
                else if (falling) setAnim(1);
                else if (jumping) setAnim(2);
                else if (attack) setAnim(5);
                else setAnim(0);
            }
        }


        public void ResetPlayer(Vector2 newPos)
        {
            spritePos = newPos;
            spriteVelocity = new Vector2();
            jumping = false;
            walking = false;
            sprint = false;
            falling = true;
            playerDead = false;
            attack = false;
        }

    }


}

