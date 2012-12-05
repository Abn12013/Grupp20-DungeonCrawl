using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DungeonCrawl
{
    class GameObj
    {

        public Vector2 Position //Objektets position
        {
            get;
            set;
        }
        public Texture2D Gfx //Objektets grafik
        {
            get;
            set;
        }
        public float Angle //Objektets vinkel
        {
            get;
            set;
        }

        public int Frame //Nuvarande bild från grafik
        {
            get;
            set;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        {
            spriteBatch.Draw(Gfx,
                Position - DrawOffset + new Vector2(400, 300), null,
                Color.White, 0,
                new Vector2(Gfx.Width / 2, Gfx.Height / 2), 1.0f,
                SpriteEffects.None, layer);
        }
    }
}
