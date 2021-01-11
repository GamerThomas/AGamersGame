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
    class ArrowSprite : Sprite
    {


        float movSpeed;

        public bool leftArrow;

        Random rng = new Random();
     
        Vector2 resetPos = new Vector2();
        public ArrowSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation, float Speed,bool left) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            //drawCollision = true;
            isColliding = true;

            collisionInsetMin = new Vector2(0f, 0f);
            collisionInsetMax = new Vector2(0f, 0f);

            resetPos = newLocation;


            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 50, 20));
            animations[0].Add(new Rectangle(0, 20, 50, 20));

            movSpeed = (float)Speed;
            leftArrow = left;
            setAnim(0);

        }

        public void update(GameTime gameTime)
        {
            if (leftArrow)
            {
                flipped = true;
                spriteVelocity.X = +movSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds ;
                spritePos += spriteVelocity;
            }
            else
            {
                flipped = false;
                spriteVelocity.X = +movSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                spritePos -= spriteVelocity;
            }

        }


        public void Reset()
        {
            spritePos = resetPos;
            movSpeed = rng.Next(150, 500);
        }
    }
}