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
using System.Windows.Forms;


namespace DungeonCrawl
{
    class Button
    {
        public Rectangle buttonRect = new Rectangle();
        public Texture2D buttonIsPressed;
        public Texture2D buttonIsNotPressed;

        public int Xpos;
        public int Ypos;

        public bool pressed = false;

        public Button(int xPos, int yPos, int width, int height)
        {
            Xpos = xPos;
            Ypos = yPos;

            buttonRect = new Rectangle(Xpos , Ypos , width, height);
        }

        public void draw(SpriteBatch spritebatch ,  float layer, Texture2D buttonPressed)
        {
            

            buttonIsPressed = buttonPressed;

            
           
                spritebatch.Draw(buttonIsPressed, new Vector2(Xpos, Ypos), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, layer);
            

        }

    }
}
