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
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue MenyHT;//High tension
        Cue IngameTGU;//The great unknown
        bool playmenumusic = true;
        bool playingamemusic = true;
        float musicends = 155000;
        float timermusic = 0;
        float musicends2 = 135000;
        float timermusic2 = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rnd = new Random();
        enum GameState {NewGame, MainMenu, LoadGame, ChangeLevel, Game,GameOver }

        GameState currentGameState = GameState.MainMenu;    //Sätter start gamestatet

        PositionManager[, ,] positionManager = new PositionManager[34, 52, 3]; //[x koordinat, y koordinat, våning]

       //Vector2 cursorPos; //muspekar pos
        protected MouseState prevMs1; //håller koll på musens förra state

        Character player = new Character(3, 4, 15, 3, 2, 15,"Fighter");
        
        //test fiende ta bort när lista med fiender finns
        List <Enemy> enemies = new List<Enemy>();

        //Skapar en spar och ladningsklass
        LoadSave saveAndLoadGame = new LoadSave();
        

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

           
            //Hp bar
            Texture2D hpBarGfx;
            Rectangle hpBarPos;

            //Xp bar
            Texture2D xpBarGfx;
            Rectangle xpBarPos;


            SpriteFont hpText;

            Texture2D interface_ingame;


        protected KeyboardState prevKs;

        List<Attack> attack = new List<Attack>();
        Attack attack2 = new Attack();

        bool attackDone = true; //håller koll på när spelaren får genomföra en attack


        //Bakgrundsgrafik för huvudmeny
        Texture2D mainMenuGfx;

        //Knappar Menyn
        Texture2D startKnappGfx;
        Texture2D loadKnappGfx;
        Texture2D infoKnappGfx;
        Texture2D exitKnappGfx;

        Vector2 startKnappPos;

        Button buttonMainMenu = new Button(47, 67, 171, 104);
        Button buttonMainMenuLoadGame = new Button(47, 214, 171, 104);
        Button ButtonMainMenuInfo = new Button(47, 390, 171, 104);
        Button ButtonMainMenuExit = new Button(47, 535, 171, 104);


        //grafik för new game
        Texture2D newGameBackground;

        Texture2D buttonNewGameGfx;
        Texture2D buttonNewGameStartGfx;
        Texture2D buttonNewGameBackGfx;


        Button buttonNewGameDwarf = new Button(83, 340, 125, 56);
        Button buttonNewGameOrc = new Button(83, 399, 125, 56);
        Button buttonNewGameElf = new Button(83, 460, 125, 56);

        Button buttonNewGameFighter = new Button(353, 340, 125, 56);
        Button buttonNewGameRogue = new Button(353, 399, 125, 56);
        Button buttonNewGameTank = new Button(353, 460, 125, 56);

        SpriteFont newGameText;

        Button buttonNewGameStart = new Button(613, 586, 170, 98);
        Button buttonNewGameBack = new Button(16, 586, 170, 98);

        //Game over grfik
        Texture2D menuKnappGfx;

        Button buttonGameOverMenu = new Button(616, 594, 170, 98);
        Button buttonGameOverExit = new Button(17, 592, 170, 98);

        //Fog of war texturer
        Texture2D visionTileGfx;

        //skatt textur
        Texture2D treasureGfx;

        //Kollar om man kör load game eller new game
        bool loadgameTrueorFalse = false;


        //textur för animation 
        Texture2D enemyAttackGfx;

        //Levelup popupfönster
        Texture2D popUpLevelUp;

        bool visaLevelUp = false;
        int visaLevelPopUp = 0;


        //Gameover
        Texture2D gameOverGfx;

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


            //Game over screen
            gameOverGfx = Content.Load<Texture2D>("gameover");

            //Laddar in grafik för levelup popupfönster
            popUpLevelUp = Content.Load<Texture2D>("popupLevel");

            //Laddar in grafik för skatt
            treasureGfx = Content.Load<Texture2D>("skatt");

            //Laddar in grafik för visiontile
            visionTileGfx = Content.Load<Texture2D>("visiontile");

            //Laddar in grafik för new game
            newGameBackground = Content.Load<Texture2D>("newgame");

            buttonNewGameGfx = Content.Load<Texture2D>("newgamebutton");
            buttonNewGameStartGfx = Content.Load<Texture2D>("newGameStart");
            buttonNewGameBackGfx = Content.Load<Texture2D>("newGameBack");

            newGameText = Content.Load<SpriteFont>("SpriteFont1");
            

            //Laddar in grafik för huvudmenyns bakgrund
            mainMenuGfx = Content.Load<Texture2D>("mainmenu");

            //laddar in grafik för knappar i menyerna
            startKnappGfx = Content.Load<Texture2D>("startKnappNer");
            loadKnappGfx = Content.Load<Texture2D>("loadKnappNer");
            infoKnappGfx = Content.Load<Texture2D>("infoKnappNer");
            exitKnappGfx = Content.Load<Texture2D>("exitKnappNer");

            //Laddr in grafik för game over screen
            menuKnappGfx = Content.Load<Texture2D>("menuKnappNer"); 

            startKnappPos = new Vector2(200, 200);

            //Laddar in grafiken för spelaren
            player.Gfx = Content.Load<Texture2D>("char_test");

            //Laddar in grafiken för fienden
            enemyGFX = Content.Load<Texture2D>("char_test");

            //Laddar in grafiken för attackanimationen
            attack2.Gfx = Content.Load<Texture2D>("animation2");
           
            enemyAttackGfx = Content.Load<Texture2D>("animation2");
            

            //Laddar in grafiken för Hpbaren
            hpBarGfx = Content.Load<Texture2D>("hpbar");
            hpBarPos = new Rectangle(400, 400, 412, 11);

            //Laddar in grafiken för xpBaren
            xpBarGfx = Content.Load<Texture2D>("xpbar");
            xpBarPos = new Rectangle(400, 450, 0, 11);

            //Laddar in spritefont för, hpbar text, xpbar, level, str, dex
            hpText = Content.Load<SpriteFont>("hpfont");

            //Laddar in grafiken för ingame-interface
            interface_ingame = Content.Load<Texture2D>("gameinterface");

            //Laddar in tilesetet för banan
            tileset = Content.Load<Texture2D>("tiles_completed");

            //bana = new GameObj() { Gfx = Content.Load<Texture2D>("level"), Position = new Vector2(0, 0), Angle = 0 };

            audioEngine = new AudioEngine("Content\\sounds.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");

            IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
            MenyHT = soundBank.GetCue("MENY.FO-HighTension");

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
                case GameState.MainMenu:

                    
                    if (playmenumusic == true)
                    {
                        MenyHT = soundBank.GetCue("MENY.FO-HighTension");
                        MenyHT.Play();
                        playmenumusic = false;                        

                    }
                    
                   
                    
                    timermusic2 += (float)gameTime.ElapsedGameTime.Milliseconds;
                    
                    if (timermusic2 > musicends2)
                    {

                        MenyHT.Stop(AudioStopOptions.AsAuthored);
                        MenyHT = soundBank.GetCue("MENY.FO-HighTension");
                        MenyHT.Play();
                           
                            timermusic2 = 0;

                        
                        
                        
                    }
                    MouseState mousestate2 = Mouse.GetState();
                    var mouseposition2 = new Point(mousestate2.X, mousestate2.Y);

                    //Gör så det ser ut som om New gameknappen r nedtryckt
                    if (buttonMainMenu.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonMainMenu.pressed = true; }
                        else { buttonMainMenu.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            currentGameState = GameState.NewGame;
                        }
                    }

                    if (buttonMainMenuLoadGame.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonMainMenuLoadGame.pressed = true; }
                        else { buttonMainMenuLoadGame.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            loadgameTrueorFalse = true;
                            //saveAndLoadGame.LoadTheGame(ref player, ref floor, ref enemies, ref positionManager);
                            currentGameState = GameState.LoadGame;

                        }
                    }

                    if (ButtonMainMenuInfo.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { ButtonMainMenuInfo.pressed = true; }
                        else { ButtonMainMenuInfo.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {

                        }
                    }

                    if (ButtonMainMenuExit.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { ButtonMainMenuExit.pressed = true; }
                        else { ButtonMainMenuExit.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            Application.Exit();
                        }
                    }

                   

                    

                    prevMs1 = mousestate2;

                    break;
                case GameState.NewGame:

                     MouseState mousestate3 = Mouse.GetState();
                    var mouseposition3 = new Point(mousestate3.X, mousestate3.Y);



                    if (buttonNewGameDwarf.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameDwarf.pressed = true; }
                        else { buttonNewGameDwarf.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.PlayerRace = "Dwarf";
                            player.SetNewGameStats();
                        }
                    }
                    if (buttonNewGameOrc.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameOrc.pressed = true; }
                        else { buttonNewGameOrc.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.PlayerRace = "Orc";
                            player.SetNewGameStats();


                        }
                    }
                    if (buttonNewGameElf.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameElf.pressed = true; }
                        else { buttonNewGameElf.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.PlayerRace = "Elf";
                            player.SetNewGameStats();
                        }
                    }


                    if (buttonNewGameFighter.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameFighter.pressed = true; }
                        else { buttonNewGameFighter.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.playerClass = "Fighter";
                            player.SetNewGameStats();
                        }
                    }
                    if (buttonNewGameRogue.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameRogue.pressed = true; }
                        else { buttonNewGameRogue.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.playerClass = "Rogue";
                            player.SetNewGameStats();
                        }
                    }
                    if (buttonNewGameTank.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameTank.pressed = true; }
                        else { buttonNewGameTank.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            player.playerClass = "Tank";
                            player.SetNewGameStats();
                           
                        }
                    }
                    
                    //startknapp
                    if (buttonNewGameStart.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameStart.pressed = true; }
                        else { buttonNewGameStart.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            loadgameTrueorFalse = false;
                            currentGameState = GameState.LoadGame;
                        }
                    }

                    if (buttonNewGameBack.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameBack.pressed = true; }
                        else { buttonNewGameBack.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            currentGameState = GameState.MainMenu;
                        }
                    }



                    prevMs1 = mousestate3;

                    break;
                case GameState.LoadGame:
                    levelManager.BuildGame(ref positionManager);


                    //Ifall man kör load gam istället för new game
                    if (loadgameTrueorFalse == true)
                    {
                        saveAndLoadGame.LoadTheGame(ref player, ref floor, ref enemies, ref positionManager, ref hpBarPos.Width);
                    }

                    
                    Vector2 temporary = entry.Position;
                    temporary.Y += 1;
                    //player.playerPosX = 8;//(int)temporary.X;
                    //player.playerPosY = 2;//(int)temporary.Y;


                    //player.playerPosX = 8;//(int)temporary.X;
                    //player.playerPosY = 2;//(int)temporary.Y;
                    player.Position = new Vector2(player.playerPosX * 64, player.playerPosY * 64);

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

                    //Börjar spela upp ingame musiken
                    if (playingamemusic == true)
                    {
                        IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
                        IngameTGU.Play();
                        playingamemusic = false;
                        MenyHT.Stop(AudioStopOptions.Immediate);

                    }
                    
                    timermusic += (float)gameTime.ElapsedGameTime.Milliseconds; //Timer som håller koll på musiken
                    
                    //Spelar upp låten igen när den tagit slut
                    if (timermusic > musicends)
                    {
                        
                            IngameTGU.Stop(AudioStopOptions.AsAuthored);
                            IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
                            IngameTGU.Play();
                            MenyHT.Stop(AudioStopOptions.AsAuthored);
                            timermusic = 0;

                        
                        
                        
                    }
                    //playerhp = player.TotalHp;  //Hp innan skada
                    // int playerhp2 = player.TotalHp;  //Hp innan skada
                     player.Update(gameTime, floor);
                    attack2.Update(gameTime, ref attackDone);

                    bool updateEnemys = false;
                   
                     if (SpawnTimer <= 0)
                     {
                         AddEnemy();
                        
                         
                         SpawnTimer = 300;
                     }
                     

                     SpawnTimer--;
                     foreach (Enemy enemy in enemies)
                     {
                         
                         enemy.PlayerPos = new Vector2(player.playerPosX, player.playerPosY); //Test ta bort sen
                         enemy.Update(gameTime, ref positionManager, floor,player.Totdex, ref skada);
                     }
            //MessageBox.Show(enemy1.PlayerPos.X.ToString());



                     //Spelarens hpbar uträkning
                     float hpBarBredd = (float)412 / player.maximumHp;
                     player.TotalHp -= skada;

                     int hploss = (int)hpBarBredd * skada;
                     hpBarPos.Width -= hploss;

                     skada = 0;

                     if (player.TotalHp <= 0)
                     {
                         MessageBox.Show("game over son");
                         currentGameState = GameState.GameOver;
                     }
                    //

                     //Spelarens xpbar uträkning
                     if (player.Xp >= 412)
                     {
                         xpBarPos.Width = 0;
                     }
                     float xpBarBredd = (float)412 / player.XpToLevel;
                     xpBarPos.Width = (int)xpBarBredd * player.Xp;


                    


                  

            // Allows the game to exit
            KeyboardState ks = Keyboard.GetState();
            KeyboardState prks;

            //Hp och hpbar uträkningar test
            Random tal = new Random();

                    //Tillfällig sparknapp
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.R))  //Knapptryckning för att röra sig ner
            {

                saveAndLoadGame.SaveTheGame(player, floor, enemies, hpBarPos.Width);
               
            }

           
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
                else if (positionManager[player.playerPosY, player.playerPosX, floor].type == "chest")
                { 
                    int hpPotsHeal = 10; // Hur mycket en HpPot helar
                    if (player.maximumHp >= player.TotalHp + hpPotsHeal) // Om den helar till mindre en max hp
                    {
                        hpBarBredd = (float)412 / player.maximumHp; // Sätter hp till spelarens nuvarande hp + så mycket som poten helar
                        player.TotalHp += hpPotsHeal;
                        int hpGain = (int)hpBarBredd * hpPotsHeal;
                        hpBarPos.Width += hpGain;
                        positionManager[player.playerPosY, player.playerPosX, floor].type = "emptychest"; // Ändrar rutans typ till en tom kista så att man bara kan använda samma kista 1 gång
                    }
                    else if (player.maximumHp < player.TotalHp + hpPotsHeal) // Om den helar till mer än max hp
                    {
                        hpBarBredd = (float)412 / player.maximumHp; // Sätter hp till max
                        player.TotalHp = (int)player.maximumHp;
                        hpBarPos.Width = 412;
                        positionManager[player.playerPosY, player.playerPosX, floor].type = "emptychest";
                    }
                    else if (player.maximumHp == player.TotalHp) // Om spelaren har max hp så händer inget (poten finns kvar)
                    {
                    }
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
                                           player.LevelUp(ref hpBarPos.Width);
                                           
                                           
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
                                            player.LevelUp(ref hpBarPos.Width);
                                            
                                          
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
                                            player.LevelUp(ref hpBarPos.Width);
                                            
                                           
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
                                            player.LevelUp(ref hpBarPos.Width);
                                            
                                            
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
                case GameState.GameOver:

                     MouseState mousestate4 = Mouse.GetState();
                    var mouseposition4 = new Point(mousestate4.X, mousestate4.Y);

                    // går till huvudmenyn och resetar variabler till sina ursprungliga värden för att man skall kunna spela igen
                    if (buttonGameOverMenu.buttonRect.Contains(mouseposition4))
                    {
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonGameOverMenu.pressed = true; }
                        else { buttonGameOverMenu.pressed = false; }
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            
                            saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar på load save för att reseta stats
                            player.SetNewGameStats();   //Resetar karaktärs stas som str, dex, hp

                            IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                            playmenumusic = true;   //Gör så att menymusiken kan spelas
                            playingamemusic = true; //Gör så att ingame musiken kan spelas upp igen

                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    if (buttonGameOverExit.buttonRect.Contains(mouseposition4))
                    {
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonGameOverExit.pressed = true; }
                        else { buttonGameOverExit.pressed = false; }
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            Application.Exit();
                        }
                    }


                    prevMs1 = mousestate4;

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
                case GameState.MainMenu:

                    spriteBatch.Draw(mainMenuGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.96f);

                    //ritaar ut knapptest
                    if (buttonMainMenu.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonMainMenu.draw(spriteBatch, 1f, startKnappGfx); }
                    else { buttonMainMenu.pressed = false; }

                    if (buttonMainMenuLoadGame.pressed == true)
                    { buttonMainMenuLoadGame.draw(spriteBatch, 1f, loadKnappGfx); }
                    else { buttonMainMenuLoadGame.pressed = false; }

                    if (ButtonMainMenuInfo.pressed == true)
                    { ButtonMainMenuInfo.draw(spriteBatch, 1f, infoKnappGfx); }
                    else { ButtonMainMenuInfo.pressed = false; }

                    if (ButtonMainMenuExit.pressed == true)
                    { ButtonMainMenuExit.draw(spriteBatch, 1f, exitKnappGfx); }
                    else { ButtonMainMenuExit.pressed = false; }


                    
                    

                    break;
                case GameState.NewGame:

                    //ritar ut bakgrund för karaktärsskapande
                    spriteBatch.Draw(newGameBackground, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);


                    if (buttonNewGameDwarf.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameDwarf.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameDwarf.pressed = false; }

                    if (buttonNewGameOrc.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameOrc.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameOrc.pressed = false; }

                    if (buttonNewGameElf.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameElf.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameElf.pressed = false; }

                    if (buttonNewGameFighter.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameFighter.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameFighter.pressed = false; }


                    if (buttonNewGameRogue.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameRogue.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameRogue.pressed = false; }


                    if (buttonNewGameTank.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameTank.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameTank.pressed = false; }


                    if (buttonNewGameStart.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameStart.draw(spriteBatch, 1f, buttonNewGameStartGfx); }
                    else { buttonNewGameStart.pressed = false; }

                    if (buttonNewGameBack.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonNewGameBack.draw(spriteBatch, 1f, buttonNewGameBackGfx); }
                    else { buttonNewGameBack.pressed = false; }

                   

                    //ritar ut infotext
                    string showClassRace = player.PlayerRace + " " + player.playerClass;
                    spriteBatch.DrawString(newGameText, showClassRace, new Vector2(500, 350), Color.AntiqueWhite , 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    string showRaceStats = player.PlayerRace + " str: " + player.RaceStr + ", dex: " + player.RaceDex + ", hp: " + player.RaceHp;
                    spriteBatch.DrawString(newGameText, showRaceStats, new Vector2(500, 390), Color.AntiqueWhite, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                     string showClassStats = player.playerClass + " str: " + player.ClassStr + ", dex: " + player.ClassDex + ", hp: " + player.ClassHp;
                     spriteBatch.DrawString(newGameText, showClassStats, new Vector2(500, 410), Color.AntiqueWhite, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                     string showTotalStats = "Total str: "+player.Totstr+ "\n" +"Total dex: " + player.Totdex +" \n" + "Total hp: " + player.TotalHp;
                     spriteBatch.DrawString(newGameText, showTotalStats, new Vector2(500, 450), Color.AntiqueWhite, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                     

                    break;
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

                    
                    //Ritar ut levelup popupfönstret
                    if (player.VisaLevelUp == false)
                    { visaLevelPopUp = 100; }
                    if (player.VisaLevelUp == true)
                    {
                        visaLevelPopUp--;
                        if(visaLevelPopUp <=0)
                        { player.VisaLevelUp = false; }
                        spriteBatch.Draw(popUpLevelUp, new Vector2(20, 20), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                   



                    //bana.Draw(spriteBatch, hej.Position, 0f);

                    player.Draw(spriteBatch, player.Position, (0.9f / 34) * ((float)player.playerPosY));
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(spriteBatch, player.Position, (0.9f / 34) * ((float)enemy.yCoord));

                    }

                    if (attackDone == false) //gör så att spelarens attackanimation bara visas nr man attackerar
                    {
                        attack2.Draw(spriteBatch, player.Position, 1f);
                    }


                    //Ritar ut attacker
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].attackAnimationDone == true)
                        {
                            enemies[i].Gfx2 = enemyAttackGfx;
                            enemies[i].AttackDraw(spriteBatch, player.Position,1f , new Vector2((player.playerPosX) * 64, (player.playerPosY) * 64));
                        }
                    }


                    //Ritar ut hpbar
                    spriteBatch.Draw(hpBarGfx, new Vector2(195, 624), hpBarPos, Color.LawnGreen, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);

                    //Ritar ut xpbar
                    spriteBatch.Draw(xpBarGfx, new Vector2(195, 671), xpBarPos, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);

                    //ritarut hpbar text
                    string show = player.TotalHp + "/" + player.maximumHp;
                    spriteBatch.DrawString(hpText, show, new Vector2(380, 620), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    //ritar ut xpbar text
                    string showXp = player.Xp + "/" + player.XpToLevel;
                    spriteBatch.DrawString(hpText, showXp, new Vector2(380, 667), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                    //ritar ut stats text: Str, Dex, Level
                    spriteBatch.DrawString(hpText, player.Totstr.ToString(), new Vector2(115, 620), Color.OrangeRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(hpText, player.Totdex.ToString(), new Vector2(115, 647), Color.OrangeRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    spriteBatch.DrawString(hpText, player.Level.ToString(), new Vector2(115, 672), Color.OrangeRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);


                    //ritar ut interface
                    spriteBatch.Draw(interface_ingame, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.94f);



                    //Ritar ut skatten
                    if (player.victoryConition == false && floor == 2)
                    {
                        spriteBatch.Draw(treasureGfx, new Vector2(31 * 64, 4 * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.88f);
                    }


                    //Fog of war
                    for (int i = -6; i < 7; i++)
                    {

                        for (int j = -7; j < 8; j++)
                        {
                            if (new Vector2(j,i) == new Vector2(-4, 0))//mitten
                            { j += 9; }
                            else if (new Vector2(j, i) == new Vector2(-3, -1))//mitten -1
                            { j += 7; }
                            else if (new Vector2(j, i) == new Vector2(-3, 1))//mitten +1
                            { j += 7; }
                            else if (new Vector2(j, i) == new Vector2(-2, 2))//mitten +2
                            { j += 5; }
                            else if (new Vector2(j, i) == new Vector2(-2, -2))//mitten -2
                            { j += 5; }
                            else if (new Vector2(j, i) == new Vector2(-1, -3))//mitten -3
                            { j += 3; }
                            else if (new Vector2(j, i) == new Vector2(-1, 3))//mitten +3
                            { j += 3; }
                            else if (new Vector2(j, i) == new Vector2(0, 4))//mitten +4
                            { j += 1; }
                            else if (new Vector2(j, i) == new Vector2(0, -4))//mitten -4
                            { j += 1; }


                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }

                    }

                    //Ritar ut fog of war om det är väggar över spelaren
                    if (positionManager[player.playerPosY-1, player.playerPosX, floor].type == "wall")
                    {
                            for (int j = -7; j < 8; j++)
                            {
                                spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                                spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                                spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                                spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            }
                    }
                    else if (positionManager[player.playerPosY - 2, player.playerPosX, floor].type == "wall")
                    {
                        for (int j = -7; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY - 3, player.playerPosX, floor].type == "wall")
                    {
                        for (int j = -7; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    //Slut på utritning av fog of war över spelaren

                    //Ritar ut fog of war om det är väggar under spelaren
                    if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type == "wall")
                    {
                        for (int j = -7; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 2, player.playerPosX, floor].type == "wall")
                    {
                        for (int j = -7; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 3, player.playerPosX, floor].type == "wall")
                    {
                        for (int j = -7; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }

                    //Kollar fog of war till vänster om spelaren om det är en vägg där

                    if (positionManager[player.playerPosY , player.playerPosX -1, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX -2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY, player.playerPosX - 2, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY, player.playerPosX - 3, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                  //Slut på koll till vänster

                    //Kollar fog of war till höger om spelaren om det är en vägg där

                    if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 1) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY, player.playerPosX + 2, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY, player.playerPosX + 3, floor].type == "wall")
                    {
                        for (int i = -6; i < 7; i++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    //Slut på koll till höger

                    //Kollar fog of war till vänster om spelaren och UPP en bit om det är en vägg där

                    if (positionManager[player.playerPosY - 1, player.playerPosX-1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY - 2, player.playerPosX-1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY - 3, player.playerPosX-1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                   
                    //Slut på koll till vänster

                    //Kollar fog of war till höger om spelaren och UPP en bit om det är en vägg där

                    if (positionManager[player.playerPosY - 1, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY - 2, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY - 3, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }

                    //Slut på koll till 

                    //kollar vänster ner
                    if (positionManager[player.playerPosY + 1, player.playerPosX - 1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 2, player.playerPosX - 1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 3, player.playerPosX - 1, floor].type == "wall")
                    {
                        for (int j = -7; j < -1; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    //slut koll

                    //kollar höger ner
                    if (positionManager[player.playerPosY + 1, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 2, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    else if (positionManager[player.playerPosY + 3, player.playerPosX + 1, floor].type == "wall")
                    {
                        for (int j = 1; j < 8; j++)
                        {
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                            spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                        }
                    }
                    //slut koll





                    break;
                case GameState.GameOver:

                    spriteBatch.Draw(gameOverGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                   
                    
                    if (buttonGameOverMenu.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonGameOverMenu.draw(spriteBatch, 0.95f, menuKnappGfx); }
                    else { buttonGameOverMenu.pressed = false; }

                    if (buttonGameOverExit.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonGameOverExit.draw(spriteBatch, 0.95f, exitKnappGfx); }
                    else { buttonGameOverExit.pressed = false; }


                    break;
            }
            
            

           

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
