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

        //Klass för att enkelt skapa nya knappar. 


        public Rectangle buttonRect = new Rectangle();
        public Texture2D buttonIsPressed;

        public int Xpos; //Pos på xled
        public int Ypos; //Pos på yled

        public bool pressed = false; //om knappen är nedtrycket eller ej

        public Button(int xPos, int yPos, int width, int height)
        {
            Xpos = xPos;
            Ypos = yPos;

            buttonRect = new Rectangle(Xpos , Ypos , width, height);    //Skapar en ny rektangel för var knappen skall ritas ut
        }

        public void draw(SpriteBatch spritebatch ,  float layer, Texture2D buttonPressed)
        {
            
                buttonIsPressed = buttonPressed;

                spritebatch.Draw(buttonIsPressed, new Vector2(Xpos, Ypos), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, layer);
            
        }

    }
}
