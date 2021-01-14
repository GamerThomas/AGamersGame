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
    class BadSprite2 : Sprite
    {
        bool left, right,falling;
        public bool alive;
        const float walkSpeed = 80f;
        public BadSprite2(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;

            //drawCollision = true;

            collisionInsetMin = new Vector2(0.01f, 0.1f);
            collisionInsetMax = new Vector2(0.01f, 0.11f);

            frameTime = 0.3f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 50, 50));
            animations[0].Add(new Rectangle(50, 0, 50, 50));
            animations[0].Add(new Rectangle(100, 0, 50, 50));

            animations.Add(new List<Rectangle>());
            animations[1].Add(new Rectangle(150, 0, 50, 50));
            animations[1].Add(new Rectangle(200, 0, 50, 50));
            animations[1].Add(new Rectangle(150, 0, 50, 50));
            animations[1].Add(new Rectangle(250, 0, 50, 50));

            alive = true;
            left = false;
            right = true;
            falling = true;
        }


        public void update(GameTime gameTime, List<PlatformSprite> platforms,List<navSprite>nav)
        {
            if (!alive)
            {
               spriteVelocity.X = 0;
                left = false;
                right = false;
                setAnim(1);
            }

            bool hasCollided = false;

            if (falling)
            {
                spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                spritePos += spriteVelocity;
            }

            foreach (PlatformSprite platform in platforms)
            {
                if (checkCollisionBelow(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y--;
                    spriteVelocity.Y = 0;
                    falling = false;
                }
                if (!hasCollided) falling = true;
                
            }


                foreach (navSprite navL in nav)
                {
                    if (checkCollisionLeft(navL))
                    {
                        right = false;
                        left = true;
                    }
                    else if (checkCollisionRight(navL))
                    {
                        right = true;
                        left = false;
                    }

                }
            

            if (right) MoveRight(gameTime);
            if (left) MoveLeft(gameTime);



        }


        public void MoveLeft(GameTime gameTime)
        {
            spriteVelocity.X = -walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
  
            flipped = true;
        }

        public void MoveRight(GameTime gameTime)
        {
            spriteVelocity.X = +walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            flipped = false;
        }

        public void Alive()
        {
            alive = true;
        }

    }
}
