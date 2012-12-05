using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;


namespace DungeonCrawl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        PositionManager[, ,] positionManager = new PositionManager[50, 32, 3]; //[x koordinat, y koordinat, v�ning]
        List<GameObj> floortiles = new List<GameObj>();
        List<GameObj> walls = new List<GameObj>();
        List<GameObj> chest = new List<GameObj>();
        List<GameObj> upstairs = new List<GameObj>();
        List<GameObj> downstairs = new List<GameObj>();
        GameObj entry;

        Vector2 cursorPos; //muspekar pos
        protected MouseState prevMs1; //h�ller koll p� musens f�rra state

        Character hej = new Character(3, 4, 2, 3, 2, 4,"Fighter");
        GameObj bana = new GameObj();

        int playerposX = 5;
            int playerposY = 5;

            int rad = 0;

            float levelHeight;
            int tempY;

         

        protected KeyboardState prevKs;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 700;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            hej.Gfx = Content.Load<Texture2D>("char_test");
            hej.Position = new Vector2(400, 350);

          

            

            bana = new GameObj() { Gfx = Content.Load<Texture2D>("level"), Position = new Vector2(0, 0), Angle = 0 };

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            hej.Update(gameTime);
            // Allows the game to exit
            KeyboardState ks = Keyboard.GetState();
            KeyboardState prks;

            levelHeight = (float)tempY *64;

            //Tillf�lliga manuella tilldelningar i positionManger f�r v�ggar, f�r att kunna genomf�ra kollisionstester
            //V�ggarna �r dock osynliga, men om man r�r sig n�gra steg till h�ger tr�ffar man p� dem
            positionManager[3, 13, 1].type = "wall";
            positionManager[4, 13, 1].type = "wall";
            positionManager[5, 13, 1].type = "wall";
            positionManager[6, 13, 1].type = "wall";
            positionManager[7, 13, 1].type = "wall";

            positionManager[4, 5, 1].type = "wall";
            positionManager[7, 5, 1].type = "wall";

            positionManager[playerposY, playerposX, 1].type = "player"; //Tilldelar spelarens nuvarande positon i rutn�tet.
                                                                        //Varablerna tilldelas h�gs upp i Game1.cs, men borde l�ggas in som en variabel i Character klassen.
            //R�relse via tangentbordskontroller
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D)) //Knapptryckning f�r att r�ra sig till h�ger
           {
               if (hej.moveCharRight == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
               {
                   hej.Frame = 6;   //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                   if (positionManager[playerposY, playerposX + 1, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                   {
                       hej.moveCharRight = true;    //G�r s� att man r�r sig �t h�ger
                       positionManager[playerposY, playerposX, 1].type = "null";    //S�tter sin f�rra position i 2d-arrayen till "null"
                       positionManager[playerposY, playerposX += 1, 1].type = "player"; //S�tter rutan man r�rde sig mot till player
                       hej.allowButtonPress = false;    //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                   }
               }
           }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A)) //Knapptryckning f�r att r�ra sig till v�nster
            {
                if (hej.moveCharLeft == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                {
                    hej.Frame = 3;  //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                    if (positionManager[playerposY, playerposX - 1, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                    {
                        hej.moveCharLeft = true;    //G�r s� att man r�r sig �t v�nster
                        positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                        positionManager[playerposY, playerposX -= 1, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                        hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.W)) //Knapptryckning f�r att r�ra sig upp
            {
                if (hej.moveCharUp == false && hej.allowButtonPress == true)    //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                {
                    hej.Frame = 9;  //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                    if (positionManager[playerposY - 1, playerposX, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                    {
                        hej.moveCharUp = true;  //G�r s� att man r�r sig upp
                        positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                        positionManager[playerposY -= 1, playerposX, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                        hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))  //Knapptryckning f�r att r�ra sig ner
            {
                if (hej.moveCharDown == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                {
                    hej.Frame = 0; //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                    if (positionManager[playerposY + 1, playerposX, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                    {
                        hej.moveCharDown = true;    //G�r s� att man r�r sig ner
                        positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                        positionManager[playerposY += 1, playerposX, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                        hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                    }
                }
            }
           prevKs = ks;
            
            //R�relse via musklick
           MouseState mousestate1 = Mouse.GetState();
           var mouseposition = new Point(mousestate1.X, mousestate1.Y);
           Rectangle moveUpBox = new Rectangle(400-28, 294-28, 56, 56);
           Rectangle moveDownBox = new Rectangle(400 - 28, 406 - 28, 56, 56);
           Rectangle moveLeftBox = new Rectangle(344 - 28, 350 - 28, 56, 56);
           Rectangle moveRightBox = new Rectangle(456 - 28, 350 - 28, 56, 56);

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if(moveUpBox.Contains(mouseposition))
           {
               if (hej.moveCharUp == false && hej.allowButtonPress == true)    //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
               {
                   hej.Frame = 9;  //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                   if (positionManager[playerposY - 1, playerposX, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                   {
                       hej.moveCharUp = true;  //G�r s� att man r�r sig upp
                       positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                       positionManager[playerposY -= 1, playerposX, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                       hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                   } } } }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveDownBox.Contains(mouseposition))
               {
                   if (hej.moveCharDown == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                   {
                       hej.Frame = 0; //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                       if (positionManager[playerposY + 1, playerposX, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                       {
                           hej.moveCharDown = true;    //G�r s� att man r�r sig ner
                           positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                           positionManager[playerposY += 1, playerposX, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                           hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                       }
                   }
               }
           }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveLeftBox.Contains(mouseposition))
               {
                   if (hej.moveCharLeft == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                   {
                       hej.Frame = 3;  //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                       if (positionManager[playerposY, playerposX - 1, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                       {
                           hej.moveCharLeft = true;    //G�r s� att man r�r sig �t v�nster
                           positionManager[playerposY, playerposX, 1].type = "null";   //S�tter sin f�rra position i 2d-arrayen till "null"
                           positionManager[playerposY, playerposX -= 1, 1].type = "player";    //S�tter rutan man r�rde sig mot till player
                           hej.allowButtonPress = false;   //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                       }
                   }
               }
           }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveRightBox.Contains(mouseposition))
               {
                   if (hej.moveCharRight == false && hej.allowButtonPress == true)  //G�r s� att man enbart kan genomf�ra en ny r�relse om karakt�ren f�r tillf�llet inte r�r sig �t n�got h�ll
                   {
                       hej.Frame = 6;   //s�tter framen till det h�ll man f�rs�ker g� �t, ifall det �r en v�gg iv�gen k�rs inte animationen men karakt�ren v�nder sig mot v�ggen
                       if (positionManager[playerposY, playerposX + 1, 1].type != "wall")   //Kollar om det �r en v�gg framf�r karakt�ren, om detta �r fallet utf�rs ingen r�relse
                       {
                           hej.moveCharRight = true;    //G�r s� att man r�r sig �t h�ger
                           positionManager[playerposY, playerposX, 1].type = "null";    //S�tter sin f�rra position i 2d-arrayen till "null"
                           positionManager[playerposY, playerposX += 1, 1].type = "player"; //S�tter rutan man r�rde sig mot till player
                           hej.allowButtonPress = false;    //G�r s� att man inte kan trycka p� n�gon annan knapp medans en r�relse genomf�rs
                       }
                   }
               }
           }



           prevMs1 = mousestate1;
            
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            bana.Draw(spriteBatch, hej.Position, 0f);

            hej.Draw(spriteBatch, hej.Position , 1f);

          

           

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
