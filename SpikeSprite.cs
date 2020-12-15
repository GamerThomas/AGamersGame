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
    class SpikeSprite : Sprite
    {


        float speed;

        Vector2 resetPos = new Vector2();
        public SpikeSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation,float spikeSpeed) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            //drawCollision = true;
            isColliding = true;

            collisionInsetMin = new Vector2(0f, 0f);
            collisionInsetMax = new Vector2(0f, 0f);

            resetPos = newLocation;


            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 31, 30));

            speed = spikeSpeed;
        }



        public void Update(GameTime gameTime)
        {
                spriteVelocity.Y = +speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                spritePos += spriteVelocity;   
        }


        public void Reset()
        {
            spritePos = resetPos;
        }

    }
}
