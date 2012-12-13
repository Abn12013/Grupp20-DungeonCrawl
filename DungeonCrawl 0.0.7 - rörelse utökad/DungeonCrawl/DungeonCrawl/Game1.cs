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
        Random rnd = new Random();
        enum GameState { LoadGame, ChangeLevel, Game }
        GameState currentGameState = GameState.LoadGame;
        PositionManager[, ,] positionManager = new PositionManager[34, 52, 3]; //[x koordinat, y koordinat, våning]

       //Vector2 cursorPos; //muspekar pos
        protected MouseState prevMs1; //håller koll på musens förra state

        Character player = new Character(3, 4, 15, 3, 2, 15,"Fighter");
        
        //test fiende ta bort när lista med fiender finns
        List <Enemy> enemies = new List<Enemy>();
        

        int SpawnTimer = 300;

        GameObj bana = new GameObj();
        List<GameObj> floortiles = new List<GameObj>();
        List<GameObj> walls = new List<GameObj>();
        List<GameObj> objects = new List<GameObj>();
        Texture2D tileset;
        Texture2D enemyGFX;
        int floor = 0;
        int skada;
        GameObj entry = new GameObj()
        {
            Position = new Vector2(0, 0),
            Frame = 23
        };
        LevelManager levelManager = new LevelManager();
        //int player.playerPosX = 3, player.playerPosY = 2;

           

            Texture2D hpBarGfx;
            Rectangle hpBarPos;

            SpriteFont hpText;


        protected KeyboardState prevKs;

        List<Attack> attack = new List<Attack>();
        Attack attack2 = new Attack();

        bool attackDone = true; //håller koll på när spelaren får genomföra en attack

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

            player.Gfx = Content.Load<Texture2D>("char_test");

            
            enemyGFX = Content.Load<Texture2D>("char_test");

            attack2.Gfx = Content.Load<Texture2D>("test_a_animation");

            hpBarGfx = Content.Load<Texture2D>("hpbar");
            hpBarPos = new Rectangle(400, 400, 300, 20);


            hpText = Content.Load<SpriteFont>("hpfont");


            tileset = Content.Load<Texture2D>("tiles_completed");

            //bana = new GameObj() { Gfx = Content.Load<Texture2D>("level"), Position = new Vector2(0, 0), Angle = 0 };

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
            
            /////////////////////////
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();
            switch (currentGameState)
            {
                case GameState.LoadGame:
                    levelManager.BuildGame(ref positionManager);
                    
                    Vector2 temporary = entry.Position;
                    temporary.Y += 1;
                    //player.playerPosX = 8;//(int)temporary.X;
                    //player.playerPosY = 2;//(int)temporary.Y;
                    player.playerPosX = 8;//(int)temporary.X;
                    player.playerPosY = 2;//(int)temporary.Y;
                    player.Position = new Vector2(player.playerPosX*64,player.playerPosY*64);

                    //test Ta bort sen!!
                    //enemies.yCoord = 8;
                    //enemies.xCoord = 4;
                    //positionManager[enemies.yCoord, enemies.xCoord, 0].type = "enemy"; //ta bort sedan när lämlig ersättare lagts till
                    //enemies.Position = new Vector2(enemies.xCoord * 64, enemies.yCoord * 64);


                    currentGameState = GameState.ChangeLevel;
                    break;
                case GameState.ChangeLevel:
                    levelManager.ChangeFloor(floor, positionManager, ref floortiles, ref walls, ref objects, ref entry, ref enemies);
                    foreach (GameObj tile in floortiles)
                    {
                        tile.Gfx = tileset;
                    }
                    foreach (GameObj wall in walls)
                    {
                        wall.Gfx = tileset;
                    }
                    foreach (GameObj item in objects)
                    {
                        item.Gfx = tileset;
                    }
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Gfx = enemyGFX;
                        enemy.Position = new Vector2(enemy.xCoord * 64, enemy.yCoord * 64);
                    }
                    entry.Gfx = tileset;

                    currentGameState = GameState.Game;
                    break;
                case GameState.Game:
                    //playerhp = player.TotalHp;  //Hp innan skada
                    // int playerhp2 = player.TotalHp;  //Hp innan skada
                     player.Update(gameTime);
                    attack2.Update(gameTime, ref attackDone);

                    bool updateEnemys = false;
                   
                     if (SpawnTimer <= 0)
                     {
                         AddEnemy();
                         //updateEnemys = true;
                         //enemies.Clear();
                         
                         //string currentObject2;
                         //Vector2 currentPosition2;

                         //for (int y = 0; y < 34; y++)
                         //{
                         //    for (int x = 0; x < 52; x++)
                         //    {
                         //        currentObject2 = positionManager[y, x, floor].type;
                         //        currentPosition2 = new Vector2(x, y);
                         //        switch (currentObject2)
                         //        {
                         //            case "enemy":
                         //                enemies.Add(new Enemy(18, 20)
                         //                {
                         //                    xCoord = (int)currentPosition2.X,
                         //                    yCoord = (int)currentPosition2.Y,
                         //                    Gfx = enemyGFX,
                         //                    Position = new Vector2((int)currentPosition2.X * 64, (int)currentPosition2.Y * 64)
                         //                });
                         //                break;
                         //        }
                         //    }
                         //}

                         
                         SpawnTimer = 3;
                     }
                     //if (updateEnemys == true)
                     //{
                     //    string currentObject2;
                     //    Vector2 currentPosition2;

                     //    for (int y = 0; y < 34; y++)
                     //    {
                     //        for (int x = 0; x < 52; x++)
                     //        {

                     //            currentObject2 = positionManager[y, x, floor].type;
                     //            currentPosition2 = new Vector2(x, y);

                     //            switch (currentObject2)
                     //            { 
                                 
                     //                case "enemy":
                     //                    enemies.Add(new Enemy(18, 20)
                     //                    {
                     //                        xCoord = (int)currentPosition2.X,
                     //                        yCoord = (int)currentPosition2.Y,
                     //                        Gfx = enemyGFX,
                     //                        Position = new Vector2((int)currentPosition2.X * 64, (int)currentPosition2.Y * 64)
                     //                    });
                     //                    break;

                     //            }

                     //        }

                     //    }

                     //    updateEnemys = false;
                     //}
                     SpawnTimer--;
                     foreach (Enemy enemy in enemies)
                     {
                         
                         enemy.PlayerPos = new Vector2(player.playerPosX, player.playerPosY); //Test ta bort sen
                         enemy.Update(gameTime, ref positionManager, floor,player.Totdex, ref skada);
                     }
            //MessageBox.Show(enemy1.PlayerPos.X.ToString());

          
            //player.TotalHp = playerhp;
                    float hpBarBredd = (float)300 / player.maximumHp;
                     player.TotalHp -= skada;

                     int hploss = (int)hpBarBredd * skada;
                     hpBarPos.Width -= hploss;

                     skada = 0;

                     if (player.TotalHp <= 0)
                     {
                         MessageBox.Show("game over son");
                         Application.Exit();
                     }
                    

                   
                    


            // Allows the game to exit
            KeyboardState ks = Keyboard.GetState();
            KeyboardState prks;

            //Hp och hpbar uträkningar test
            Random tal = new Random();
           
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Q))  //Knapptryckning för att röra sig ner
            {
                

                if (positionManager[player.playerPosY, player.playerPosX, floor].type == "downstairs")
                {
                    floor++;
                    currentGameState = GameState.ChangeLevel;
                }
                else if (positionManager[player.playerPosY, player.playerPosX, floor].type == "upstairs")
                {
                    floor--;
                    currentGameState = GameState.ChangeLevel;
                }


            }

            // TODO: Add your update logic here

            //MessageBox.Show(positionManager[6,5,0].type);

            //positionManager[player.playerPosY, player.playerPosX, 0].type = "player"; //Tilldelar spelarens nuvarande positon i rutnätet.
                                                                        //Varablerna tilldelas högs upp i Game1.cs, men borde läggas in som en variabel i Character klassen.
            //Rörelse via tangentbordskontroller
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) //Knapptryckning för att röra sig till höger
           {
               if (player.moveCharRight == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
               {
                   player.Frame = 6;   //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                  
                   if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "enemy")
                   {
                       if (attackDone == true)
                       {
                           for (int i = 0; i < enemies.Count; i++)
                           {
                               if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                               {
                                   attackDone = false;
                                   player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                   attack2.attackPos = new Vector2(player.playerPosX + 1, player.playerPosY);
                                   attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);

                                   enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                   player.allowButtonPress = true;
                                   //MessageBox.Show(enemies[i].hp.ToString());
                                   if (enemies[i].hp <= 0)
                                   {
                                       enemies.RemoveAt(i);
                                       positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";

                                       player.Xp += 60;
                                       if (player.Xp >= player.XpToLevel)
                                       {
                                           player.LevelUp();
                                           MessageBox.Show("Level up");
                                           hpBarPos.Width = 300;
                                       }
                                   }
                               }
                               else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                               {

                               }
                           }   
                       }
                   }

                   else if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                   {
                       player.moveCharRight = true;    //Gör så att man rör sig åt höger
                       player.allowButtonPress = false;    //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                       player.playerPosX += 1;
                       
                   }
                   //if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "empty")
                   //MessageBox.Show(positionManager[player.playerPosY, player.playerPosX + 1, floor].type);
                   
               }
           }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) ) //Knapptryckning för att röra sig till vänster
            {
                if (player.moveCharLeft == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 3;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                    
                    if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type == "enemy")
                    {


                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX - 1 && enemies[i].yCoord == player.playerPosY)
                                {
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                    attack2.attackPos = new Vector2(player.playerPosX - 1, player.playerPosY);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);

                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    player.allowButtonPress = true;
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY, player.playerPosX - 1, floor].type = "empty";

                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp();
                                            MessageBox.Show("Level up");
                                            hpBarPos.Width = 300;
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }


                    }

                    else if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharLeft = true;    //Gör så att man rör sig åt vänster
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY, player.playerPosX - 1, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosX -= 1;
                    }
                    
                }

            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W)) //Knapptryckning för att röra sig upp
            {
                if (player.moveCharUp == false && player.allowButtonPress == true)    //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                   
                    player.Frame = 9;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                   
                    if (positionManager[player.playerPosY -1 , player.playerPosX , floor].type == "enemy")
                    {


                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX  && enemies[i].yCoord == player.playerPosY - 1)
                                {
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY - 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);

                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    player.allowButtonPress = true;
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY - 1, player.playerPosX, floor].type = "empty";

                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp();
                                            MessageBox.Show("Level up");
                                            hpBarPos.Width = 300;
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }


                        //attack
                    }
                    
                    else if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharUp = true;  //Gör så att man rör sig upp
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY - 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosY -= 1;

                    }
                    
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))  //Knapptryckning för att röra sig ner
            {
                if (player.moveCharDown == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 0; //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                    if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type == "enemy")
                    {


                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY + 1)
                                {
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY + 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);

                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    player.allowButtonPress = true;
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY + 1, player.playerPosX, floor].type = "empty";

                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp();
                                            MessageBox.Show("Level up");
                                            hpBarPos.Width = 300;
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }


                        //attack
                    }
                    
                    else if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharDown = true;    //Gör så att man rör sig ner
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY + 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosY += 1;
                    }
                   
                }
            }
           prevKs = ks;
            
            

            //Rörelse via musklick
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
               if (player.moveCharUp == false && player.allowButtonPress == true)    //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
               {
                   player.Frame = 9;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                   if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                   {
                       player.moveCharUp = true;  //Gör så att man rör sig upp
                       //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                       //positionManager[player.playerPosY -= 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                       player.playerPosY -= 1;
                       player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                   } } } }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveDownBox.Contains(mouseposition))
               {
                   if (player.moveCharDown == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                   {
                       player.Frame = 0; //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                       if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                       {
                           player.moveCharDown = true;    //Gör så att man rör sig ner
                           //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                           //positionManager[player.playerPosY += 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player'
                           player.playerPosY += 1;
                           player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                       }
                   }
               }
           }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveLeftBox.Contains(mouseposition))
               {
                   if (player.moveCharLeft == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                   {
                       player.Frame = 3;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                       if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                       {
                           player.moveCharLeft = true;    //Gör så att man rör sig åt vänster
                           //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                           //positionManager[player.playerPosY, player.playerPosX -= 1, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                           player.playerPosX -= 1;
                           player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                       }
                   }
               }
           }

           if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
           {
               if (moveRightBox.Contains(mouseposition))
               {
                   if (player.moveCharRight == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                   {
                       player.Frame = 6;   //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                       if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                       {
                           player.moveCharRight = true;    //Gör så att man rör sig åt höger
                           //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                           //positionManager[player.playerPosY, player.playerPosX += 1, 0].type = "player"; //Sätter rutan man rörde sig mot till player
                           player.playerPosX += 1;
                           player.allowButtonPress = false;    //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                       }
                   }
               }
           }


            


           prevMs1 = mousestate1;
            
            // TODO: Add your update logic here

                    break;
            }
           

            base.Update(gameTime);
        }

        protected void AddEnemy()
        {


            int temp = rnd.Next(floortiles.Count - 1);
            int pSight = 8;
            if (!(((floortiles.ElementAt(temp).Position.X) / 64 > player.playerPosX - pSight) &&
            ((floortiles.ElementAt(temp).Position.X) / 64 < player.playerPosX + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 < player.playerPosY + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 > player.playerPosY - pSight)) &&
            (positionManager[(int)(floortiles.ElementAt(temp).Position.Y) / 64, (int)(floortiles.ElementAt(temp).Position.X) / 64, floor].type == "empty"))
            {
                enemies.Add(new Enemy(18, 5)
                {
                    xCoord = ((int)floortiles.ElementAt(temp).Position.X) / 64,
                    yCoord = ((int)floortiles.ElementAt(temp).Position.Y) / 64,
                    Gfx = enemyGFX,
                    Position = new Vector2(floortiles.ElementAt(temp).Position.X, floortiles.ElementAt(temp).Position.Y)
                });
                positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].type = "enemy";
            }


        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            switch (currentGameState)
            {
                case GameState.LoadGame:
                    break;
                case GameState.Game:
                    foreach (GameObj tile in floortiles)
                    {
                        tile.Draw(spriteBatch, player.Position, 0);
                    }
                    foreach (GameObj wall in walls)
                    {
                        wall.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y + 46), (0.9f / 34f) *(wall.Position.Y/64f));
                    }
                    foreach (GameObj obj in objects)
                    {
                        obj.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y + 46), (0.9f / 34f) * (obj.Position.Y / 64f));
                    }
                    
                    break;
            }
            
            //bana.Draw(spriteBatch, hej.Position, 0f);

            player.Draw(spriteBatch, player.Position, (0.9f/34)*((float)player.playerPosY));
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch, player.Position, (0.9f / 34) * ((float)enemy.yCoord));
               
            }

            if (attackDone == false) //gör så att spelarens attackanimation bara visas nr man attackerar
            {
                attack2.Draw(spriteBatch, player.Position, 1f);
            }

            //Ritar ut hpbar
            spriteBatch.Draw(hpBarGfx, new Vector2(250, 600), hpBarPos, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);

            //ritarut hpbar text
            string show = player.TotalHp + "/" + player.maximumHp;
            spriteBatch.DrawString(hpText, show, new Vector2(380, 597), Color.Black,0,Vector2.Zero,1f,SpriteEffects.None,1f);
           

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
