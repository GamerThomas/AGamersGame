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
    class BackSprite : Sprite
    {
        Vector2 startPos;
        public BackSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0f, 0f);
            isColliding = true;

            collisionInsetMin = new Vector2();
            collisionInsetMax = new Vector2();

            startPos = newLocation;

            frameTime = 1f;
            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 250, 115));
            animations[0].Add(new Rectangle(250, 0, 250, 115));
            animations[0].Add(new Rectangle(500, 0, 250, 115));
            animations[0].Add(new Rectangle(750, 0, 250, 115));

            animations.Add(new List<Rectangle>());
            animations[1].Add(new Rectangle(1000, 0, 70, 65));
            animations[1].Add(new Rectangle(1070, 0, 70, 65));
            animations[1].Add(new Rectangle(1140, 0, 70, 65));



        }

        public void move2(Vector2 newPos)
        {
            spritePos = newPos;
            setAnim(1);
        }

        public void move1()
        {
            spritePos = startPos;
            setAnim(0);
        }
    }
}
