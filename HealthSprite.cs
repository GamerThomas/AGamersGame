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
    class HealthSprite : Sprite
    {
        public HealthSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0f, 0f);
            //drawCollision = true;


            frameTime = 0.7f;
            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 30, 40));
            animations[0].Add(new Rectangle(70, 0, 30, 40));


        }



        public void Update()
        {
            setAnim(0);
        }
    }

}
