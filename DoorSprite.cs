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
    class DoorSprite : Sprite
    {
        public bool open = false;
        const float openSpeed = 500f;

        
        public DoorSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;

            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 150, 150));
            //drawCollision = true;


        }


        public void Update()
        {
            spritePos = new Vector2(-150, -150);

        }
        public void level1()
        {
            spritePos = new Vector2(1205, 300);
        }


        public void Move(Vector2 newPos)
        {
            spritePos = newPos;
        }
    }
}
