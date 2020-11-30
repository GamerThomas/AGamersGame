using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Collections.Generic;

namespace AGamersGame
{
    class BadSprite1 : Sprite
    {
        bool falling;


        public BadSprite1(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;

            //drawCollision = true;

            collisionInsetMin = new Vector2(0.1f, 0.1f);
            collisionInsetMax = new Vector2(0.1f, 0.02f);


            frameTime = 0.5f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 50, 50));
            animations[0].Add(new Rectangle(50, 0, 50, 50));
            animations[0].Add(new Rectangle(100, 0, 50, 50));
            animations[0].Add(new Rectangle(150, 0, 50, 50));


            falling = true;
        }

        public void Update(GameTime gameTime, List<PlatformSprite> platforms)
        {
            if (falling)
            {
                spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                spritePos += spriteVelocity;
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

    }
}
