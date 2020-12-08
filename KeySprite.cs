using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AGamersGame
{
    class KeySprite : Sprite
    {
        public bool hideKey = false;
        public bool open = false;
        public KeySprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation) : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            drawCollision = true;

            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(0, 0, 12, 30));
            animations[0].Add(new Rectangle(13, 0, 12, 30));
            animations[0].Add(new Rectangle(26, 0, 12, 30));
            animations[0].Add(new Rectangle(39, 0, 12, 30));

            animations.Add(new List<Rectangle>());
            animations[1].Add(new Rectangle(0, 30, 1, 1));
        }
    }
}
