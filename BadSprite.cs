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
    class BadSprite1 : Sprite
    {
        bool falling;
        public bool enDead = false;
        bool win = false;
        public BadSprite1(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;

            //drawCollision = true;

            collisionInsetMin = new Vector2(0.01f, 0.1f);
            collisionInsetMax = new Vector2(0.01f, 0.02f);


            frameTime = 0.5f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());//Idle anim
            animations[0].Add(new Rectangle(0, 0, 50, 50));
            animations[0].Add(new Rectangle(50, 0, 50, 50));
            animations[0].Add(new Rectangle(100, 0, 50, 50));
            animations[0].Add(new Rectangle(150, 0, 50, 50));

            animations.Add(new List<Rectangle>());//Dead Anim
            animations[1].Add(new Rectangle(101, 51, 50, 50));
            animations[1].Add(new Rectangle(151, 51, 50, 50));

            animations.Add(new List<Rectangle>());//Win Anim
            animations[2].Add(new Rectangle(204, 0, 52, 52));
            animations[2].Add(new Rectangle(257, 50, 52, 52));
            animations[2].Add(new Rectangle(306, 0, 52, 52));
            animations[2].Add(new Rectangle(359, 0, 52, 52));



            falling = true;
        }

        public void Update(GameTime gameTime, List<PlatformSprite> platforms)
        {
            if (falling||enDead)
            {
                spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                spritePos += spriteVelocity;
            }

            if (enDead)
            {
                setAnim(1);
                collisionInsetMin = new Vector2(1f, 0f);
                collisionInsetMax = new Vector2(1f, 0f);
            }
            else if(win)
            {
                setAnim(2);
            }
            else
            {
                setAnim(0);
                collisionInsetMin = new Vector2(0.01f, 0.1f);
                collisionInsetMax = new Vector2(0.01f, 0.02f);
            }

            bool hasCollided = false;


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
        }


        public void Win()
        {
            win = true;
        }
    }
}
