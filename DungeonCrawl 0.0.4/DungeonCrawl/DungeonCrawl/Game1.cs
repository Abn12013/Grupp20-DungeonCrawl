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

        PositionManager[, ,] positionManager = new PositionManager[50, 32, 3]; //[x koordinat, y koordinat, våning]
        


        Character hej = new Character(3, 4, 2, 3, 2, 4,"Fighter");
        GameObj bana = new GameObj();

        int playerposX = 5;
            int playerposY = 5;

            int rad = 0;

         

        protected KeyboardState prevKs;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);

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

            hej.Gfx = Content.Load<Texture2D>("Namnlös");
            hej.Position = new Vector2(400, 400);

           

            

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

            //Tillfälliga manuella tilldelningar i positionManger för väggar, för att kunna genomföra kollisionstester
            //Väggarna är dock osynliga, men om man rör sig några steg till höger träffar man på dem
            positionManager[3, 13, 1].type = "wall";
            positionManager[4, 13, 1].type = "wall";
            positionManager[5, 13, 1].type = "wall";
            positionManager[6, 13, 1].type = "wall";
            positionManager[7, 13, 1].type = "wall";

            positionManager[4, 5, 1].type = "wall";
            positionManager[7, 5, 1].type = "wall";

            positionManager[playerposY, playerposX, 1].type = "player"; //Tilldelar spelarens nuvarande positon i rutnätet.
                                                                        //Varablerna tilldelas högs upp i Game1.cs, men borde läggas in som en variabel i Character klassen.

            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D)) //Knapptryckning för att röra sig till höger
           {
               if (hej.moveCharRight == false && hej.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
               {
                   
                   if (positionManager[playerposY, playerposX + 1, 1].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                   {
                       hej.moveCharRight = true;    //Gör så att man rör sig åt höger
                       positionManager[playerposY, playerposX, 1].type = "null";    //Sätter sin förra position i 2d-arrayen till "null"
                       positionManager[playerposY, playerposX += 1, 1].type = "player"; //Sätter rutan man rörde sig mot till player
                       hej.allowButtonPress = false;    //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                   }
               }
           }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A)) //Knapptryckning för att röra sig till vänster
            {
                if (hej.moveCharLeft == false && hej.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {

                    if (positionManager[playerposY, playerposX - 1, 1].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        hej.moveCharLeft = true;    //Gör så att man rör sig åt vänster
                        positionManager[playerposY, playerposX, 1].type = "null";   //Sätter sin förra position i 2d-arrayen till "null"
                        positionManager[playerposY, playerposX -= 1, 1].type = "player";    //Sätter rutan man rörde sig mot till player
                        hej.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.W)) //Knapptryckning för att röra sig upp
            {
                if (hej.moveCharUp == false && hej.allowButtonPress == true)    //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {

                    if (positionManager[playerposY - 1, playerposX, 1].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        hej.moveCharUp = true;  //Gör så att man rör sig upp
                        positionManager[playerposY, playerposX, 1].type = "null";   //Sätter sin förra position i 2d-arrayen till "null"
                        positionManager[playerposY -= 1, playerposX, 1].type = "player";    //Sätter rutan man rörde sig mot till player
                        hej.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.S))  //Knapptryckning för att röra sig ner
            {
                if (hej.moveCharDown == false && hej.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {

                    if (positionManager[playerposY + 1, playerposX, 1].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        hej.moveCharDown = true;    //Gör så att man rör sig ner
                        positionManager[playerposY, playerposX, 1].type = "null";   //Sätter sin förra position i 2d-arrayen till "null"
                        positionManager[playerposY += 1, playerposX, 1].type = "player";    //Sätter rutan man rörde sig mot till player
                        hej.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                    }
                }
            }
           prevKs = ks;
            
            
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

            hej.Draw(spriteBatch, hej.Position ,1f);

           

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
