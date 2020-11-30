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
        public BackSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0f, 0f);
            isColliding = true;

            collisionInsetMin = new Vector2();
            collisionInsetMax = new Vector2();

            frameTime = 1f;
            animations = new List<List<Rectangle>>();

            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 250, 115));
            animations[0].Add(new Rectangle(250, 0, 250, 115));
            animations[0].Add(new Rectangle(500, 0, 250, 115));
            animations[0].Add(new Rectangle(750, 0, 250, 115));

        }
    }
}
