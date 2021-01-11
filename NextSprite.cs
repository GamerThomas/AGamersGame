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
    class NextSprite : Sprite
    {

        
        public NextSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            drawCollision = true;

            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 50, 50));

        }

        public void level1()
        {
            spritePos = new Vector2(1260, 300);
        }

        public void level2()
        {
            spritePos = new Vector2(10, 490);
        }

        public void level3()
        {
            spritePos = new Vector2(1250, 480);
        }
    }
}
