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
        Cue attackSound; // Attack ljud
        Cue attackMiss; // Attack miss ljud
        Cue buttonKlick; //Knapptrycks ljud

        bool playmenumusic = true;
        bool playingamemusic = true;
        float musicends = 155000;
        float timermusic = 0;
        float musicends2 = 135000;
        float timermusic2 = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rnd = new Random();
        public enum GameState { NewGame, MainMenu, LoadGame, ChangeLevel, Game, GameOver, Victory, Pause, Information }

        public GameState currentGameState = GameState.MainMenu;    //S�tter start gamestatet

        PositionManager[, ,] positionManager = new PositionManager[34, 52, 3]; //[x koordinat, y koordinat, v�ning]

        FogOfWar fogOfWar1 = new FogOfWar();    //Ritar ut fog of war

        ButtonKlick buttonKlick1 = new ButtonKlick();   //Klass f�r knapptryckningar

       //Vector2 cursorPos; //muspekar pos
        protected MouseState prevMs1; //h�ller koll p� musens f�rra state

        Character player = new Character(3, 4, 15, 3, 2, 15,"Fighter");
        
        //test fiende ta bort n�r lista med fiender finns
        List <Enemy> enemies = new List<Enemy>();

        //Skapar en spar och ladningsklass
        LoadSave saveAndLoadGame = new LoadSave();
        

        int SpawnTimer = 300;

        GameObj bana = new GameObj();
        List<GameObj> floortiles = new List<GameObj>();
        List<GameObj> walls = new List<GameObj>();
        List<GameObj> objects = new List<GameObj>();
        Texture2D tileset;
        Texture2D goblinGFX, darkelfGFX, felorcGFX, gGoblinGFX;
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
            SpriteFont hpText2;
            public int playerdmg;
            public int enemydmg;

            Texture2D interface_ingame;


        protected KeyboardState prevKs;

        List<Attack> attack = new List<Attack>();
        Attack attack2 = new Attack();

        bool attackDone = true; //h�ller koll p� n�r spelaren f�r genomf�ra en attack
        bool attackDone2 = true;
        bool attackDoneMiss = true;

        //Bakgrundsgrafik f�r huvudmeny
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


        //grafik f�r new game
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

        //Victory grafik
        Button buttonVictoryMenu = new Button(616, 594, 170, 98);
        Button buttonVictoryExit = new Button(17, 592, 170, 98);

        //Fog of war texturer
        Texture2D visionTileGfx;

        //skatt textur
        Texture2D treasureGfx;

        //Kollar om man k�r load game eller new game
        bool loadgameTrueorFalse = false;


        //textur f�r animation 
        Texture2D enemyAttackGfx;

        //Levelup popupf�nster
        Texture2D popUpLevelUp;

        bool visaLevelUp = false;
        int visaLevelPopUp = 0;


        //Gameover
        Texture2D gameOverGfx;

        //victory
        bool VictoryShowButtons = false;

        Texture2D victoryGfx;

        Texture2D VictoryCreditsGfx;
        Vector2 VictoryCreditsPos;

        Texture2D VictoryCreditsGfx2;
        Vector2 VictoryCreditsPos2;

        Texture2D VictoryCreditsGfx3;
        Vector2 VictoryCreditsPos3;

        //pause game
        Texture2D inGameMenyBackGroundGfx;
        Texture2D inGameMenyMenuGfx;

        Vector2 inGameMenyBackGroundPos;

        //Knappar ingame-menu
        Texture2D buttonInGameMenuResumeGfx;
        Texture2D buttonInGameMenuSaveGfx;

        Button buttonInGameMenuResume = new Button(315, 170, 171, 104);
        Button buttonInGameMenuSave = new Button(315, 296, 171, 104);
        Button buttonInGameMenuExit = new Button(315, 422, 171, 104);


        int playerDamgeDelt = 0;
        Random spawnTimerRandom = new Random();

        //Informaion menu
        Texture2D informationMenuBackGroundGfx;
        Texture2D buttonInformationCreditsGfx;

        Button buttonInformationCredits = new Button(564, 561, 171, 104);
        Button buttonInformationBack = new Button(74, 561, 171, 104);

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

            //Informaton meny
            informationMenuBackGroundGfx = Content.Load<Texture2D>("informationmenu");
            buttonInformationCreditsGfx = Content.Load<Texture2D>("informationKnappcredits");

            //Ingame menu
            inGameMenyBackGroundGfx = Content.Load<Texture2D>("ingamemenyback");
            inGameMenyBackGroundPos = new Vector2(0, 0);

            buttonInGameMenuResumeGfx = Content.Load<Texture2D>("ingamemenyknappresume");
            buttonInGameMenuSaveGfx = Content.Load<Texture2D>("ingamemenyknappsave");

            inGameMenyMenuGfx = Content.Load<Texture2D>("ingamemenymenu");


            //Game over screen
            gameOverGfx = Content.Load<Texture2D>("gameover");

            //Victroy screen
            victoryGfx = Content.Load<Texture2D>("victory");

            VictoryCreditsGfx = Content.Load<Texture2D>("creditsFirst");
            VictoryCreditsPos = new Vector2(0, 0);

            VictoryCreditsGfx2 = Content.Load<Texture2D>("creditsMiddle");
            VictoryCreditsPos2 = new Vector2(0, 2000);

            VictoryCreditsGfx3 = Content.Load<Texture2D>("creditsLast");
            VictoryCreditsPos3 = new Vector2(0, 4000);



            //Laddar in grafik f�r levelup popupf�nster
            popUpLevelUp = Content.Load<Texture2D>("popupLevel");

            //Laddar in grafik f�r skatt
            treasureGfx = Content.Load<Texture2D>("skatt");

            //Laddar in grafik f�r visiontile
            visionTileGfx = Content.Load<Texture2D>("visiontile");

            //Laddar in grafik f�r new game
            newGameBackground = Content.Load<Texture2D>("newgame");

            buttonNewGameGfx = Content.Load<Texture2D>("newgamebutton");
            buttonNewGameStartGfx = Content.Load<Texture2D>("newGameStart");
            buttonNewGameBackGfx = Content.Load<Texture2D>("newGameBack");

            newGameText = Content.Load<SpriteFont>("SpriteFont1");
            

            //Laddar in grafik f�r huvudmenyns bakgrund
            mainMenuGfx = Content.Load<Texture2D>("mainmenu");

            //laddar in grafik f�r knappar i menyerna
            startKnappGfx = Content.Load<Texture2D>("startKnappNer");
            loadKnappGfx = Content.Load<Texture2D>("loadKnappNer");
            infoKnappGfx = Content.Load<Texture2D>("infoKnappNer");
            exitKnappGfx = Content.Load<Texture2D>("exitKnappNer");

            //Laddr in grafik f�r game over screen
            menuKnappGfx = Content.Load<Texture2D>("menuKnappNer"); 

            startKnappPos = new Vector2(200, 200);

            //Laddar in grafiken f�r spelaren
            player.Gfx = Content.Load<Texture2D>("dwarf_male");
            player.Gfx2 = Content.Load<Texture2D>("orc");
            player.Gfx3 = Content.Load<Texture2D>("elf");

            //Laddar in grafiken f�r fienderna
            goblinGFX = Content.Load<Texture2D>("goblin");
            darkelfGFX = Content.Load<Texture2D>("dark_elf");
            felorcGFX = Content.Load<Texture2D>("fel_orc");
            gGoblinGFX = Content.Load<Texture2D>("goblin_gross");

            //Laddar in grafiken f�r attackanimationen
            attack2.Gfx = Content.Load<Texture2D>("animation2");
           
            enemyAttackGfx = Content.Load<Texture2D>("animation2");
            

            //Laddar in grafiken f�r Hpbaren
            hpBarGfx = Content.Load<Texture2D>("hpbar");
            hpBarPos = new Rectangle(400, 400, 412, 11);

            //Laddar in grafiken f�r xpBaren
            xpBarGfx = Content.Load<Texture2D>("xpbar");
            xpBarPos = new Rectangle(400, 450, 0, 11);

            //Laddar in spritefont f�r, hpbar text, xpbar, level, str, dex
            hpText = Content.Load<SpriteFont>("hpfont");
            hpText2 = Content.Load<SpriteFont>("SpriteFont2");

            //Laddar in grafiken f�r ingame-interface
            interface_ingame = Content.Load<Texture2D>("gameinterface");

            //Laddar in tilesetet f�r banan
            tileset = Content.Load<Texture2D>("tiles_completed");

            //bana = new GameObj() { Gfx = Content.Load<Texture2D>("level"), Position = new Vector2(0, 0), Angle = 0 };

            audioEngine = new AudioEngine("Content\\sounds.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Sound Bank.xsb");

            IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
            MenyHT = soundBank.GetCue("MENY.FO-HighTension");
            attackSound = soundBank.GetCue("AttackSound");
            attackMiss = soundBank.GetCue("AttackMiss");
            buttonKlick = soundBank.GetCue("CLICK18B");
            

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

                    //G�r s� det ser ut som om New gameknappen r nedtryckt
                    if (buttonMainMenu.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonMainMenu.pressed = true; }
                        else { buttonMainMenu.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
                            currentGameState = GameState.Information;
                        }
                    }

                    if (ButtonMainMenuExit.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { ButtonMainMenuExit.pressed = true; }
                        else { ButtonMainMenuExit.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
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
                            soundBank.PlayCue("CLICK18B");
                            currentGameState = GameState.MainMenu;
                        }
                    }



                    prevMs1 = mousestate3;

                    break;
                case GameState.LoadGame:
                    levelManager.BuildGame(ref positionManager);


                    //Ifall man k�r load gam ist�llet f�r new game
                    if (loadgameTrueorFalse == true)
                    {
                        for (int y = 0; y < 34; y++)
                            for (int x = 0; x < 50; x++)
                                for (int z = 0; z < 3; z++)
                                {
                                    if (positionManager[y,x,z].type == "enemy")
                                        positionManager[y,x,z].type = "empty";
                                }
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
                    //positionManager[enemies.yCoord, enemies.xCoord, 0].type = "enemy"; //ta bort sedan n�r l�mlig ers�ttare lagts till
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
                        switch (enemy.ReturnExp())
                        {
                            case 60:
                                enemy.Gfx = goblinGFX;
                                break;
                            case 30:
                                enemy.Gfx = gGoblinGFX;
                                break;
                            case 80:
                                enemy.Gfx = darkelfGFX;
                                break;
                            case 200:
                                enemy.Gfx = felorcGFX;
                                break;
                        }
                        enemy.Position = new Vector2(enemy.xCoord * 64, enemy.yCoord * 64);
                    }
                    entry.Gfx = tileset;

                    currentGameState = GameState.Game;
                    break;
                case GameState.Game:

                    //B�rjar spela upp ingame musiken
                    if (playingamemusic == true)
                    {
                        IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
                        IngameTGU.Play();
                        playingamemusic = false;
                        MenyHT.Stop(AudioStopOptions.Immediate);

                    }
                    
                    timermusic += (float)gameTime.ElapsedGameTime.Milliseconds; //Timer som h�ller koll p� musiken
                    
                    //Spelar upp l�ten igen n�r den tagit slut
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
                     attack2.Update(gameTime, ref attackDone, ref attackDone2);

                

                    
                     if (SpawnTimer <= 0)
                     {
                         AddEnemy();

                         

                         SpawnTimer = spawnTimerRandom.Next(150, 301);
                     }
                     

                     SpawnTimer--;
                     foreach (Enemy enemy in enemies)
                     {
                         
                         enemy.PlayerPos = new Vector2(player.playerPosX, player.playerPosY); //Test ta bort sen
                         enemy.Update(gameTime, ref positionManager, floor, player.Totdex, ref skada, soundBank, attackSound, attackMiss, player.Level, ref player);
                     }
            //MessageBox.Show(enemy1.PlayerPos.X.ToString());

                     

                     //Spelarens hpbar utr�kning
                     float hpBarBredd = (float)412 / player.maximumHp;
                     //player.TotalHp -= skada;

                     int hploss = (int)(hpBarBredd * skada);
                     hpBarPos.Width -= hploss;

                     if (skada != 0)
                     {
                        
                         enemydmg = skada;
                     }
                     skada = 0;

                     if (player.TotalHp <= 0)
                     {
                         currentGameState = GameState.GameOver;
                     }
                    //

                     //Spelarens xpbar utr�kning
                     if (player.Xp >= player.XpToLevel)
                     {
                         xpBarPos.Width = 0;
                     }
                     float xpBarBredd = (float)412 / player.XpToLevel;
                     xpBarPos.Width = (int)(xpBarBredd * player.Xp);


                    


                  

            // Allows the game to exit
            KeyboardState ks = Keyboard.GetState();
            KeyboardState prks;

            //Hp och hpbar utr�kningar test
            Random tal = new Random();


            buttonKlick1.Update(gameTime, ref player, ref saveAndLoadGame, ref floor, ref enemies, ref positionManager, ref hpBarBredd, ref playingamemusic
                , ref playingamemusic, IngameTGU, ref levelManager, ref attackDone, ref attackDone2, ref attackDoneMiss, soundBank, attackSound, attackMiss
                , ref attack2, ref playerDamgeDelt, ref hpBarPos, objects, tileset);


            //�ppnar ingamenyn
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))  //Knapptryckning f�r att r�ra sig ner
            {
                currentGameState = GameState.Pause;
            }

                    //Tillf�llig sparknapp
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.R))  //Knapptryckning f�r att r�ra sig ner
            {

                saveAndLoadGame.SaveTheGame(player, floor, enemies, hpBarPos.Width, positionManager);
               
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6))
            {
                saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar p� load save f�r att reseta stats
                player.SetNewGameStats();   //Resetar karakt�rs stas som str, dex, hp

                IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                playmenumusic = true;   //G�r s� att menymusiken kan spelas
                playingamemusic = true; //G�r s� att ingame musiken kan spelas upp igen

                currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
            }

           
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Q))  //Knapptryckning f�r att r�ra sig ner
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
                    int hpPotsHeal = (int)player.maximumHp / 2; // Hur mycket en HpPot helar
                    if (player.maximumHp >= player.TotalHp + hpPotsHeal) // Om den helar till mindre en max hp
                    {
                        hpBarBredd = (float)412 / player.maximumHp; // S�tter hp till spelarens nuvarande hp + s� mycket som poten helar
                        player.TotalHp += hpPotsHeal;
                        int hpGain = (int)hpBarBredd * hpPotsHeal;
                        hpBarPos.Width += hpGain;
                        positionManager[player.playerPosY, player.playerPosX, floor].type = "emptychest"; // �ndrar rutans typ till en tom kista s� att man bara kan anv�nda samma kista 1 g�ng
                    }
                    else if (player.maximumHp < player.TotalHp + hpPotsHeal) // Om den helar till mer �n max hp
                    {
                        hpBarBredd = (float)412 / player.maximumHp; // S�tter hp till max
                        player.TotalHp = (int)player.maximumHp;
                        hpBarPos.Width = 412;
                        positionManager[player.playerPosY, player.playerPosX, floor].type = "emptychest";
                    }
                    else if (player.maximumHp == player.TotalHp) // Om spelaren har max hp s� h�nder inget (poten finns kvar)
                    {
                    }


                    levelManager.OpenChest(floor, positionManager, ref objects);
                    foreach (GameObj item in objects)
                    {
                        item.Gfx = tileset;
                    }

                }


            }

           
           prevKs = ks;



           //R�relse via musklick
           MouseState mousestate1 = Mouse.GetState();
           var mouseposition = new Point(mousestate1.X, mousestate1.Y);




           prevMs1 = mousestate1;
            
            // TODO: Add your update logic here

           if (player.Victory == true)
           {
               currentGameState = GameState.Victory;
           }


                    break;
                case GameState.GameOver:

                     MouseState mousestate4 = Mouse.GetState();
                    var mouseposition4 = new Point(mousestate4.X, mousestate4.Y);

                    // g�r till huvudmenyn och resetar variabler till sina ursprungliga v�rden f�r att man skall kunna spela igen
                    if (buttonGameOverMenu.buttonRect.Contains(mouseposition4))
                    {
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonGameOverMenu.pressed = true; }
                        else { buttonGameOverMenu.pressed = false; }
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar p� load save f�r att reseta stats
                            player.SetNewGameStats();   //Resetar karakt�rs stas som str, dex, hp

                            IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                            playmenumusic = true;   //G�r s� att menymusiken kan spelas
                            playingamemusic = true; //G�r s� att ingame musiken kan spelas upp igen

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
                            soundBank.PlayCue("CLICK18B");
                            Application.Exit();
                        }
                    }


                    prevMs1 = mousestate4;

                 


                    break;
                case GameState.Victory:

                    if (VictoryCreditsPos3.Y > -220)
                    {
                        VictoryCreditsPos.Y -= 1;
                        VictoryCreditsPos2.Y -= 1;
                        VictoryCreditsPos3.Y -= 1;
                    }

                    MouseState mousestate5 = Mouse.GetState();
                    var mouseposition5 = new Point(mousestate5.X, mousestate5.Y);

                    if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed)) //G�r s� att knapparna visas, s� att man kan st�nga av eller g� till menyn
                    { VictoryShowButtons = true; }

                    if (VictoryShowButtons == true)
                    {
                        // g�r till huvudmenyn och resetar variabler till sina ursprungliga v�rden f�r att man skall kunna spela igen
                        if (buttonVictoryMenu.buttonRect.Contains(mouseposition5))
                        {
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            { buttonVictoryMenu.pressed = true; }
                            else { buttonVictoryMenu.pressed = false; }
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            {
                                soundBank.PlayCue("CLICK18B");
                                saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar p� load save f�r att reseta stats
                                player.SetNewGameStats();   //Resetar karakt�rs stas som str, dex, hp

                                IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                                playmenumusic = true;   //G�r s� att menymusiken kan spelas
                                playingamemusic = true; //G�r s� att ingame musiken kan spelas upp igen

                                VictoryShowButtons = false;

                                //Nollst�ller positionerna p� credits
                                VictoryCreditsPos.Y =0;
                                VictoryCreditsPos2.Y =2000;
                                VictoryCreditsPos3.Y =4000;

                                currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                            }
                        }

                        if (buttonVictoryExit.buttonRect.Contains(mouseposition5))
                        {
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            { buttonVictoryExit.pressed = true; }
                            else { buttonVictoryExit.pressed = false; }
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            {
                                soundBank.PlayCue("CLICK18B");
                                Application.Exit();
                            }
                        }
                    }

                    prevMs1 = mousestate5;

                    break;
                case GameState.Pause:


                    if (inGameMenyBackGroundPos.Y <= -1300)
                    {
                        inGameMenyBackGroundPos.Y = 0;
                    }
                    inGameMenyBackGroundPos.Y -= 2;


                    MouseState mousestate6 = Mouse.GetState();
                    var mouseposition6 = new Point(mousestate6.X, mousestate6.Y);

                    // g�r till huvudmenyn och resetar variabler till sina ursprungliga v�rden f�r att man skall kunna spela igen
                    if (buttonInGameMenuExit.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuExit.pressed = true; }
                        else { buttonInGameMenuExit.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar p� load save f�r att reseta stats
                            player.SetNewGameStats();   //Resetar karakt�rs stas som str, dex, hp

                            IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                            playmenumusic = true;   //G�r s� att menymusiken kan spelas
                            playingamemusic = true; //G�r s� att ingame musiken kan spelas upp igen

                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    if (buttonInGameMenuResume.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuResume.pressed = true; }
                        else { buttonInGameMenuResume.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            currentGameState = GameState.Game;

                        }
                    }

                    if (buttonInGameMenuSave.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuSave.pressed = true; }
                        else { buttonInGameMenuSave.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            saveAndLoadGame.SaveTheGame(player, floor, enemies, hpBarPos.Width, positionManager);

                        }
                    }

                    prevMs1 = mousestate6;
                    KeyboardState ks2 = Keyboard.GetState();
                    

                    if (ks2.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))  //Knapptryckning f�r att r�ra sig ner
                    {
                        currentGameState = GameState.Game;
                    }

                    prevKs = ks2;


                    break;
                case GameState.Information:

                    MouseState mousestate7 = Mouse.GetState();
                    var mouseposition7 = new Point(mousestate7.X, mousestate7.Y);

                    // g�r till huvudmenyn och resetar variabler till sina ursprungliga v�rden f�r att man skall kunna spela igen
                    if (buttonInformationBack.buttonRect.Contains(mouseposition7))
                    {
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInformationBack.pressed = true; }
                        else { buttonInformationBack.pressed = false; }
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    if (buttonInformationCredits.buttonRect.Contains(mouseposition7))
                    {
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInformationCredits.pressed = true; }
                        else { buttonInformationCredits.pressed = false; }
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");
                            currentGameState = GameState.Victory;
                        }
                    }

                   

                    prevMs1 = mousestate7;

                    break;

            }
           

            base.Update(gameTime);
        }

        protected void AddEnemy()
        {
            int HPtemp = 0;
            int STRtemp = 0;
            int DEXtemp = 0;
            int enemySpeed = 0;
            int enemyEXP = 0;
            Texture2D tempGFX = goblinGFX;
            switch (rnd.Next(3))
            {
                case 0:
                    HPtemp = 4;
                    STRtemp = 3;
                    DEXtemp = 12;
                    enemySpeed = 8;
                    enemyEXP = 30;
                    tempGFX = gGoblinGFX;
                    break;
                case 1:
                    HPtemp = 20; 
                    STRtemp = 8; 
                    DEXtemp = 12; 
                    enemySpeed = 2;
                    enemyEXP = 60;
                    break;
                case 2:
                    HPtemp = 14;
                    STRtemp = 8; 
                    DEXtemp = 8; 
                    enemySpeed = 4;
                    enemyEXP = 80;
                    tempGFX = darkelfGFX;
                    break;
            }

            int enemyhp = HPtemp + (player.Level * 3);
            int enemystr = DEXtemp + (player.Level*2);
            int enemydex = STRtemp + (player.Level*3);
            
            int temp = rnd.Next(floortiles.Count - 1);
            int pSight = 8;
            if (!(((floortiles.ElementAt(temp).Position.X) / 64 > player.playerPosX - pSight) &&
            ((floortiles.ElementAt(temp).Position.X) / 64 < player.playerPosX + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 < player.playerPosY + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 > player.playerPosY - pSight)) &&
            (positionManager[(int)(floortiles.ElementAt(temp).Position.Y) / 64, (int)(floortiles.ElementAt(temp).Position.X) / 64, floor].type == "empty"))
            {
                enemies.Add(new Enemy(enemyhp, enemystr, enemydex, tempGFX, enemySpeed, enemyEXP)
                {
                    xCoord = ((int)floortiles.ElementAt(temp).Position.X) / 64,
                    yCoord = ((int)floortiles.ElementAt(temp).Position.Y) / 64,
                    Position = new Vector2(floortiles.ElementAt(temp).Position.X, floortiles.ElementAt(temp).Position.Y)
                });
                positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].type = "enemy";
                switch (enemyEXP)
                {
                    case 30:
                        positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].monster = "g_goblin";
                        break;
                    case 60:
                        positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].monster = "goblin";
                        break;
                    case 80:
                        positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].monster = "dark_elf";
                        break;
                    case 200:
                        positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].monster = "fel_orc";
                        break;
                }
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
                    if (buttonMainMenu.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
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

                    //ritar ut bakgrund f�r karakt�rsskapande
                    spriteBatch.Draw(newGameBackground, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.95f);


                    if (buttonNewGameDwarf.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameDwarf.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameDwarf.pressed = false; }

                    if (buttonNewGameOrc.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameOrc.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameOrc.pressed = false; }

                    if (buttonNewGameElf.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameElf.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameElf.pressed = false; }

                    if (buttonNewGameFighter.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameFighter.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameFighter.pressed = false; }


                    if (buttonNewGameRogue.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameRogue.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameRogue.pressed = false; }


                    if (buttonNewGameTank.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameTank.draw(spriteBatch, 1f, buttonNewGameGfx); }
                    else { buttonNewGameTank.pressed = false; }


                    if (buttonNewGameStart.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonNewGameStart.draw(spriteBatch, 1f, buttonNewGameStartGfx); }
                    else { buttonNewGameStart.pressed = false; }

                    if (buttonNewGameBack.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
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
                        if (positionManager[(int)obj.Position.Y/64,(int)obj.Position.X/64,floor].type == "door")
                            obj.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y + 78), ((0.9f / 34f) * ((obj.Position.Y / 64f)-0.0000001f)));
                        else
                            obj.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y ), (0.001f));
                    }

                    

                    
                    //Ritar ut levelup popupf�nstret
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

                    if (attackDone2 == false) //g�r s� att spelarens attackanimation bara visas nr man attackerar
                    {
                        attack2.Draw(spriteBatch, player.Position, 1f);
                    }

                    if (attackDone2 == false && playerDamgeDelt > 0)
                    {
                        spriteBatch.DrawString(hpText2, playerDamgeDelt.ToString(), new Vector2(((player.playerPosX) - attack2.randomnr), (player.playerPosY) - 64) + new Vector2(400, 350), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                    else if (playerDamgeDelt == 0 && attackDoneMiss == false)
                    {
                        spriteBatch.DrawString(hpText2, "Miss!", new Vector2(((player.playerPosX) - attack2.randomnr), (player.playerPosY) - 64) + new Vector2(400, 350), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }

                    //if (playerdmg > 0 && attackDone == false)
                    //{
                    //    spriteBatch.DrawString(hpText2, playerdmg.ToString(), new Vector2((player.playerPosX) * 64, (player.playerPosY - 1) * 64) - player.Position + new Vector2(400, 350), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    //}



                    //Ritar ut attacker
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].attackAnimationDone == true )
                        {
                          
                            enemies[i].Gfx2 = enemyAttackGfx;
                            enemies[i].AttackDraw(spriteBatch, player.Position,1f , new Vector2((player.playerPosX) * 64, (player.playerPosY) * 64));
                        }
                        if (enemies[i].attackDidDmg == false)
                        {
                            enemies[i].DmgDraw(spriteBatch, hpText2, enemydmg.ToString(), new Vector2((enemies[i].Position.X) - enemies[i].randomnr, (enemies[i].Position.Y)) - enemies[i].Position + new Vector2(400, 350), Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                        }
                        else if (enemies[i].attackMissed == false && skada == 0)
                        {
                            enemies[i].DmgDraw(spriteBatch, hpText2, "Dodge!", new Vector2((enemies[i].Position.X) - enemies[i].randomnr, (enemies[i].Position.Y)) - enemies[i].Position + new Vector2(400, 350), Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
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

                    //Ritar ut fog of war
                    fogOfWar1.Draw(spriteBatch, new Vector2(0, 0), 0.89f, visionTileGfx, player, positionManager, floor);

                 

                    break;
                case GameState.GameOver:

                    spriteBatch.Draw(gameOverGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                   
                    
                    if (buttonGameOverMenu.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonGameOverMenu.draw(spriteBatch, 0.95f, menuKnappGfx); }
                    else { buttonGameOverMenu.pressed = false; }

                    if (buttonGameOverExit.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonGameOverExit.draw(spriteBatch, 0.95f, exitKnappGfx); }
                    else { buttonGameOverExit.pressed = false; }


                    break;
                case GameState.Victory:

                     spriteBatch.Draw(VictoryCreditsGfx, VictoryCreditsPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits
                    spriteBatch.Draw(VictoryCreditsGfx2, VictoryCreditsPos2, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits
                    spriteBatch.Draw(VictoryCreditsGfx3, VictoryCreditsPos3, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits

                    if (VictoryShowButtons == true)
                    {

                        spriteBatch.Draw(victoryGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                        if (buttonVictoryMenu.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                        { buttonVictoryMenu.draw(spriteBatch, 0.95f, menuKnappGfx); }
                        else { buttonVictoryMenu.pressed = false; }

                        if (buttonVictoryExit.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                        { buttonVictoryExit.draw(spriteBatch, 0.95f, exitKnappGfx); }
                        else { buttonVictoryExit.pressed = false; }
                    }


                    break;
                case GameState.Pause:

                    spriteBatch.Draw(inGameMenyBackGroundGfx, inGameMenyBackGroundPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    spriteBatch.Draw(inGameMenyMenuGfx, new Vector2(240, 100), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);

                    //ritar ut knappar
                    if (buttonInGameMenuExit.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonInGameMenuExit.draw(spriteBatch, 1f, exitKnappGfx); }
                    else { buttonInGameMenuExit.pressed = false; }

                    if (buttonInGameMenuResume.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonInGameMenuResume.draw(spriteBatch, 1f, buttonInGameMenuResumeGfx); }
                    else { buttonInGameMenuResume.pressed = false; }

                    if (buttonInGameMenuSave.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonInGameMenuSave.draw(spriteBatch, 1f, buttonInGameMenuSaveGfx); }
                    else { buttonInGameMenuSave.pressed = false; }





                    break;
                case GameState.Information:

                    spriteBatch.Draw(informationMenuBackGroundGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    if (buttonInformationBack.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonInformationBack.draw(spriteBatch, 1f, buttonNewGameBackGfx); }
                    else { buttonInformationBack.pressed = false; }

                    if (buttonInformationCredits.pressed == true)//G�r s� det ser ut som man trycker p� knappen new game
                    { buttonInformationCredits.draw(spriteBatch, 1f, buttonInformationCreditsGfx); }
                    else { buttonInformationCredits.pressed = false; }


                    break;

            }
            
            

           

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
