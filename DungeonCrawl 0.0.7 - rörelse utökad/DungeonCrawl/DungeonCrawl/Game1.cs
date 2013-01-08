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
        Cue MenyHT; //High tension, menymusik
        Cue IngameTGU;  //The great unknown, spelmusik
        Cue attackSound; // Attack ljud
        Cue attackMiss; // Attack miss ljud
        Cue buttonKlick; //Knapptrycks ljud
        Cue Creditsmusic; //Musik i eftertext

        bool playmenumusic = true;  //kollar om menymusik ska spelas
        bool playingamemusic = true;//kollar om spelmusiken ska spelas
        float musicends = 155000;   //Timer för menymusik
        float timermusic = 0;   //ElapsedGameTime timer
        float musicends2 = 135000;  //Timer för spelmusik
        float timermusic2 = 0;  //ElapsedGameTime timer

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random rnd = new Random();  //Random som används för många saker

        enum GameState { NewGame, MainMenu, LoadGame, ChangeLevel, Game, GameOver, Victory, Pause, Information }    //Spelets olika gamestates

        GameState currentGameState = GameState.MainMenu;    //Sätter start gamestatet


        PositionManager[, ,] positionManager = new PositionManager[34, 52, 3]; //[y koordinat, x koordinat, våning]

        FogOfWar fogOfWar1 = new FogOfWar();    //skapar en ny fog of war

        ButtonKlick buttonKlick1 = new ButtonKlick();   //Klass för knapptryckningar

        Character player = new Character(3, 4, 15, 3, 2, 15, "Fighter"); //skapar en ny spelare

        Attack attack2 = new Attack();  //Skapar en ny attack
        
        List <Enemy> enemies = new List<Enemy>();   //Skapar fiender i en listaa

        LoadSave saveAndLoadGame = new LoadSave(); //skapar en spar och laddnings funktion
        

        int SpawnTimer = 300; //håller koll på hur ofta fiender spawnar i millisekunder

        GameObj bana = new GameObj();   //Skapar en ny gameObj
        List<GameObj> floortiles = new List<GameObj>(); //skapar nya golvobjekt
        List<GameObj> walls = new List<GameObj>();  //Skapar nya väggar
        List<GameObj> objects = new List<GameObj>();    //Skappar nya objekt, ex: kistor, trappor
        
        Texture2D tileset; //Tileset för alla bangrafik
        Texture2D goblinGFX, darkelfGFX, felorcGFX, gGoblinGFX; //Textrurer för fiender
        int floor = 0; //Nuvarande våning
        int skada;  //Håller koll på vilken skada som görs på spelaren
       
        LevelManager levelManager = new LevelManager(); //Skapar en ny levelmanger
        //int player.playerPosX = 3, player.playerPosY = 2;

           
            //Hp bar
            Texture2D hpBarGfx;
            Rectangle hpBarPos;

            //Xp bar
            Texture2D xpBarGfx;
            Rectangle xpBarPos;

            
            SpriteFont hpText;  //in-game interface text

            //Text och värden för skada
            SpriteFont hpText2; 
            public int playerdmg;  
            public int enemydmg;

            Texture2D interface_ingame; //Grafik för ingame interface

        bool attackDone = true; //håller koll på när spelaren får genomföra en attack
        bool attackDone2 = true;    //håller koll på när spelaren attackanimation ritas ut
        bool attackDoneMiss = true; //håller koll på om man missar eller ej, för att rita ut text som säger att man missar

        //grafik för huvudmeny
        Texture2D mainMenuGfx;
        Texture2D mainmenuBackGround;
        Vector2 mainmenuBackGroundPos;

        //Grafik för huvudmenyknappar
        Texture2D startKnappGfx;
        Texture2D loadKnappGfx;
        Texture2D infoKnappGfx;
        Texture2D exitKnappGfx;

        Vector2 startKnappPos;

        //Huvudmenyknappar
        Button buttonMainMenu = new Button(47, 67, 171, 104);
        Button buttonMainMenuLoadGame = new Button(47, 214, 171, 104);
        Button ButtonMainMenuInfo = new Button(47, 390, 171, 104);
        Button ButtonMainMenuExit = new Button(47, 535, 171, 104);


        //grafik för new game
        Texture2D newGameBackground;

        //Grafik för newgameknappr
        Texture2D buttonNewGameGfx;
        Texture2D buttonNewGameStartGfx;
        Texture2D buttonNewGameBackGfx;

        //Knappr för newgame
        Button buttonNewGameDwarf = new Button(83, 340, 125, 56);
        Button buttonNewGameOrc = new Button(83, 399, 125, 56);
        Button buttonNewGameElf = new Button(83, 460, 125, 56);

        Button buttonNewGameFighter = new Button(353, 340, 125, 56);
        Button buttonNewGameRogue = new Button(353, 399, 125, 56);
        Button buttonNewGameTank = new Button(353, 460, 125, 56);

        Button buttonNewGameStart = new Button(613, 586, 170, 98);
        Button buttonNewGameBack = new Button(16, 586, 170, 98);

        SpriteFont newGameText; //Text som visas i newgame

        //Game over grfik
        Texture2D menuKnappGfx;

        //Game over knappar
        Button buttonGameOverMenu = new Button(616, 594, 170, 98);
        Button buttonGameOverExit = new Button(17, 592, 170, 98);

        //Victory knappar
        Button buttonVictoryMenu = new Button(616, 594, 170, 98);
        Button buttonVictoryExit = new Button(17, 592, 170, 98);

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

        //Gameover grafik
        Texture2D gameOverGfx;

        //victory 
        bool VictoryShowButtons = false;    //Bestämmer om vctory skärmens knappar ska visas

        //Victory grafik
        Texture2D victoryGfx;

        Texture2D VictoryCreditsGfx;
        Vector2 VictoryCreditsPos;

        Texture2D VictoryCreditsGfx2;
        Vector2 VictoryCreditsPos2;

        Texture2D VictoryCreditsGfx3;
        Vector2 VictoryCreditsPos3;

        //pause gamegrafik
        Texture2D inGameMenyBackGroundGfx;
        Texture2D inGameMenyMenuGfx;

        Vector2 inGameMenyBackGroundPos;

        //Knappar ingame-menu
        Texture2D buttonInGameMenuResumeGfx;
        Texture2D buttonInGameMenuSaveGfx;

        Button buttonInGameMenuResume = new Button(315, 170, 171, 104);
        Button buttonInGameMenuSave = new Button(315, 296, 171, 104);
        Button buttonInGameMenuExit = new Button(315, 422, 171, 104);

        //Informaionsmenu grafik
        Texture2D informationMenuBackGroundGfx;
        Texture2D buttonInformationCreditsGfx;

        //Informaionsmenu knappar
        Button buttonInformationCredits = new Button(564, 561, 171, 104);
        Button buttonInformationBack = new Button(74, 561, 171, 104);

        int visaLevelPopUp = 0; //timer för hur länga levelupfönstret skall visas

        int playerDamgeDelt = 0;

        Random spawnTimerRandom = new Random(); //Timer som avgör när finder skall spawna

        protected KeyboardState prevKs;

        protected MouseState prevMs1; //håller koll på musens förra state

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
            mainmenuBackGround = Content.Load<Texture2D>("mainmenuBackground");
            mainmenuBackGroundPos = new Vector2(0, 0);

            //laddar in grafik för knappar i menyerna
            startKnappGfx = Content.Load<Texture2D>("startKnappNer");
            loadKnappGfx = Content.Load<Texture2D>("loadKnappNer");
            infoKnappGfx = Content.Load<Texture2D>("infoKnappNer");
            exitKnappGfx = Content.Load<Texture2D>("exitKnappNer");

            //Laddr in grafik för game over screen
            menuKnappGfx = Content.Load<Texture2D>("menuKnappNer"); 

            startKnappPos = new Vector2(200, 200);

            //Laddar in grafiken för spelaren
            player.Gfx = Content.Load<Texture2D>("dwarf_male");
            player.Gfx2 = Content.Load<Texture2D>("orc");
            player.Gfx3 = Content.Load<Texture2D>("elf");

            //Laddar in grafiken för fienderna
            goblinGFX = Content.Load<Texture2D>("goblin");
            darkelfGFX = Content.Load<Texture2D>("dark_elf");
            felorcGFX = Content.Load<Texture2D>("fel_orc");
            gGoblinGFX = Content.Load<Texture2D>("goblin_gross");

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
            hpText2 = Content.Load<SpriteFont>("SpriteFont2");

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
            attackSound = soundBank.GetCue("AttackSound");
            attackMiss = soundBank.GetCue("AttackMiss");
            buttonKlick = soundBank.GetCue("CLICK18B");
            Creditsmusic = soundBank.GetCue("FO-EnemyShips");
            
        }

      
        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {

            switch (currentGameState)
            {
                case GameState.MainMenu:    //Spelets Huvudmeny

                    //Gör så att en del av bakgrunden i huvudmenyn rör på sig
                    mainmenuBackGroundPos.X -= 1;   
                    if (mainmenuBackGroundPos.X == -1200)
                    {
                        mainmenuBackGroundPos.X = 0;
                    }

                    //Stänger av tidigare ljud, och startar ett nytt
                    if (playmenumusic == true)
                    {
                        MenyHT.Stop(AudioStopOptions.AsAuthored);
                        MenyHT = soundBank.GetCue("MENY.FO-HighTension");
                        MenyHT.Play();
                        playmenumusic = false;                        

                    }

                    timermusic2 += (float)gameTime.ElapsedGameTime.Milliseconds;    //Timer för när musiken skall börja om

                    //Loopar huvudmenymusiken
                    if (timermusic2 > musicends2)
                    {

                        MenyHT.Stop(AudioStopOptions.AsAuthored);
                        MenyHT = soundBank.GetCue("MENY.FO-HighTension");
                        MenyHT.Play();
                           
                        timermusic2 = 0; 

                    }

                    MouseState mousestate2 = Mouse.GetState();
                    var mouseposition2 = new Point(mousestate2.X, mousestate2.Y);

                    //Knapp för att starta ett nytt spel
                    if (buttonMainMenu.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonMainMenu.pressed = true; }
                        else { buttonMainMenu.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");  //Spelar upp knappljud
                            currentGameState = GameState.NewGame;
                        }
                    }

                    //Knapp för att ladda spel
                    if (buttonMainMenuLoadGame.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonMainMenuLoadGame.pressed = true; }
                        else { buttonMainMenuLoadGame.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud

                            loadgameTrueorFalse = true; //Bool som talar om att man laddar ett spel istället för att starta ett nytt

                            currentGameState = GameState.LoadGame;

                        }
                    }

                    //Knapp för att öppna informationsmenyn
                    if (ButtonMainMenuInfo.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { ButtonMainMenuInfo.pressed = true; }
                        else { ButtonMainMenuInfo.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            currentGameState = GameState.Information;
                        }
                    }

                    //Knapp för att stänga ner spelet
                    if (ButtonMainMenuExit.buttonRect.Contains(mouseposition2))
                    {
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { ButtonMainMenuExit.pressed = true; }
                        else { ButtonMainMenuExit.pressed = false; }
                        if (mousestate2.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            Application.Exit();
                        }
                    }

                    prevMs1 = mousestate2;

                    break;
                case GameState.NewGame:     //Meny för att skapa ny spelomgång

                     MouseState mousestate3 = Mouse.GetState();
                    var mouseposition3 = new Point(mousestate3.X, mousestate3.Y);

                    //Knapp för att välja ras dwarf
                    if (buttonNewGameDwarf.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameDwarf.pressed = true; }
                        else { buttonNewGameDwarf.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.PlayerRace = "Dwarf";
                            player.SetNewGameStats(); //Kallar på en metod som updaterar spelarstats
                        }
                    }

                    //Knapp för att välja ras orc
                    if (buttonNewGameOrc.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameOrc.pressed = true; }
                        else { buttonNewGameOrc.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.PlayerRace = "Orc";
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats


                        }
                    }

                    //Knapp för att välja ras elf
                    if (buttonNewGameElf.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameElf.pressed = true; }
                        else { buttonNewGameElf.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.PlayerRace = "Elf";
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats
                        }
                    }

                    //Knapp för att välja klass fighter
                    if (buttonNewGameFighter.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameFighter.pressed = true; }
                        else { buttonNewGameFighter.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.playerClass = "Fighter";
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats
                        }
                    }

                    //Knapp för att välja klass rogue
                    if (buttonNewGameRogue.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameRogue.pressed = true; }
                        else { buttonNewGameRogue.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.playerClass = "Rogue";
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats
                        }
                    }

                    //Knapp för att välja klass Tank
                    if (buttonNewGameTank.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameTank.pressed = true; }
                        else { buttonNewGameTank.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            player.playerClass = "Tank";
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats
                           
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
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            loadgameTrueorFalse = false;
                            currentGameState = GameState.LoadGame;
                        }
                    }

                    //Knapp för att backa tillbaka till huvudmenyn
                    if (buttonNewGameBack.buttonRect.Contains(mouseposition3))
                    {
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonNewGameBack.pressed = true; }
                        else { buttonNewGameBack.pressed = false; }
                        if (mousestate3.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            currentGameState = GameState.MainMenu;
                        }
                    }

                    prevMs1 = mousestate3;

                    break;
                case GameState.LoadGame:    //Ett gamestate som laddar in spelets bana
                    levelManager.BuildGame(ref positionManager);

                    //Ifall man kör load gam istället för new game
                    if (loadgameTrueorFalse == true)
                    {
                        for (int z = 0; z < 3; z++)
                            for (int y = 0; y < 34; y++)
                                for (int x = 0; x < 52; x++)
                                {
                                    if (positionManager[y, x, z].type == "enemy")
                                        positionManager[y, x, z].type = "empty"; //sätter alla fiendepositioner till empty
                                }
                        saveAndLoadGame.LoadTheGame(ref player, ref floor, ref enemies, ref positionManager, ref hpBarPos.Width);   //Laddar in den sparade datan som behövs för att skapa spelomgången
                    }

                    player.Position = new Vector2(player.playerPosX * 64, player.playerPosY * 64);  //sätter spelarens startposition i pixelkoordinater

                    currentGameState = GameState.ChangeLevel;   //Byter gamestate till ´changelevel
                    break;
                case GameState.ChangeLevel:

                    //Detta gamestate körs både när man startar nytt spel, laddar ett spel, eller byter en våning
                    //Vad den gör är att ladda om och ladda in alla objekt, på den nya våningen eller spelet

                    levelManager.ChangeFloor(floor, positionManager, ref floortiles, ref walls, ref objects, ref enemies);  //Kallar på en metod som updaterar alla objekt på spelplanen
                    foreach (GameObj tile in floortiles)    //Tilldelar alla golv textur
                    {
                        tile.Gfx = tileset;
                    }
                    foreach (GameObj wall in walls)     //Tilldelar alla väggar textur
                    {
                        wall.Gfx = tileset;
                    }
                    foreach (GameObj item in objects)   //Tilldelar alla objekt sina unika texturer
                    {
                        item.Gfx = tileset;
                    }
                    foreach (Enemy enemy in enemies)    ////Tilldelar alla fiender sina unika texturer
                    {
                        switch (enemy.ReturnExp())  //Vilken textur en fiende skall få avgörs av hur mycket xp dom ger, dvs hur svåra dem att döda
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
                        enemy.Position = new Vector2(enemy.xCoord * 64, enemy.yCoord * 64); //Tilldelar fienderna sina positioner i pixlar
                    }
                    
                    currentGameState = GameState.Game;  //Byter gamestate till game
                    break;
                case GameState.Game:
                    //Detta gamestate innhehåller all kod för att spela spelet


                    //Börjar spela upp ingame musiken
                    if (playingamemusic == true)
                    {
                        IngameTGU = soundBank.GetCue("IG.FO-TheGreatUnknown");
                        IngameTGU.Play();
                        playingamemusic = false;
                        MenyHT.Stop(AudioStopOptions.Immediate);    //Stoppar tidigare musik

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

                     player.Update(gameTime, floor);    //Updaterar spelarklassen
                     attack2.Update(gameTime, ref attackDone, ref attackDone2);     //Uppdaterar attackklassen

                     if (SpawnTimer <= 0)   //kallar på en metod som skapar en ny fiende
                     {
                         AddEnemy();

                         SpawnTimer = spawnTimerRandom.Next(150, 301);  //Gör så att spawntiden för fiender blir ett ranomvärde mellan ca 10 och 5 sek
                     }
                     
                     SpawnTimer--;  //Gör så att timern räknar ner

                     foreach (Enemy enemy in enemies)   //Körs för varje fiende
                     {
                         
                         enemy.PlayerPos = new Vector2(player.playerPosX, player.playerPosY); //Tilldelar fienden spelarens position för att ai skall fungera
                         enemy.Update(gameTime, ref positionManager, floor, player.Totdex, ref skada, soundBank, attackSound, attackMiss, player.Level, ref player);    //Updaterar alla fiender
                     }

                     //Spelarens hpbar uträkning
                     float hpBarBredd = (float)412 / player.maximumHp;  //Delar upp baren i så många delar som spelaren har hp

                     int hploss = (int)(hpBarBredd * skada);   //Så många bitar som skall tas bort från hpbar
                     hpBarPos.Width -= hploss;  //tar bort en del av baren

                     if (skada != 0)    //Om skada inte = 0, så får enemydmg värdet som skall ritas ut av skadan som gjordes av fienden
                     {
                         enemydmg = skada; 
                     }
                     skada = 0; //skada säts till 0

                     if (player.TotalHp <= 0)   //Ifall spelaraens hp blir mindre eller likamed 0, förlorar man
                     {
                         currentGameState = GameState.GameOver; //Kör gamestatet för förlust
                     }
                 
                     //Spelarens xpbar uträkning
                     if (player.Xp >= player.XpToLevel)
                     {
                         xpBarPos.Width = 0;
                     }
                     float xpBarBredd = (float)412 / player.XpToLevel;  //Samma som tidigare, delar upp baren i flera delar av det totala xp som behövs för att gå upp i level
                     xpBarPos.Width = (int)(xpBarBredd * player.Xp); //Breden är likamed så mycket xp man gånger så stor en bar är


            KeyboardState ks = Keyboard.GetState();
           
                    //uppdaterar klassen buttonklick, som tar hand om spelarens knapptryck för röresle och attack
            buttonKlick1.Update(gameTime, ref player, ref saveAndLoadGame, ref floor, ref enemies, ref positionManager, ref hpBarBredd, ref playingamemusic
                , ref playingamemusic, IngameTGU, ref levelManager, ref attackDone, ref attackDone2, ref attackDoneMiss, soundBank, attackSound, attackMiss
                , ref attack2, ref playerDamgeDelt, ref hpBarPos, objects, tileset);


            //Öppnar ingamenyn
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))  //Knapptryckning för att röra sig ner
            {
                currentGameState = GameState.Pause;
            }

            //sparknapp
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.R) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.R))  //Knapptryckning för att röra sig ner
            {

                saveAndLoadGame.SaveTheGame(player, floor, enemies, hpBarPos.Width, positionManager);
               
            }

            //Gå till huvudmenyknapp
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F6))
            {
                saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar på load save för att reseta stats
                player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats   //Resetar karaktärs stas som str, dex, hp

                IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                playmenumusic = true;   //Gör så att menymusiken kan spelas
                playingamemusic = true; //Gör så att ingame musiken kan spelas upp igen

                currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
            }

           //Spelets "actionknapp" utför olika saker, ex: gå ned eller upp för trappor, öppna kistor osv
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
                    int hpPotsHeal = (int)player.maximumHp / 2; // Hur mycket en HpPot helar
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


                    levelManager.OpenChest(floor, positionManager, ref objects);
                    foreach (GameObj item in objects)
                    {
                        item.Gfx = tileset;
                    }
                }
            }
           
           prevKs = ks;

           //Musklick
           MouseState mousestate1 = Mouse.GetState();
           var mouseposition = new Point(mousestate1.X, mousestate1.Y);


           prevMs1 = mousestate1;
           
           //Ifall detta vilkor uppfylls vinner spelaren och visar credits
           if (player.Victory == true)
           {
               IngameTGU.Stop(AudioStopOptions.AsAuthored);
               Creditsmusic.Play();
               currentGameState = GameState.Victory;
           }
                    break;
                case GameState.GameOver:    //Körs om man förlorar

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
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar på load save för att reseta stats
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats   //Resetar karaktärs stas som str, dex, hp

                            IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                            playmenumusic = true;   //Gör så att menymusiken kan spelas
                            playingamemusic = true; //Gör så att ingame musiken kan spelas upp igen

                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    //Knapp för att stänga ner spelet
                    if (buttonGameOverExit.buttonRect.Contains(mouseposition4))
                    {
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonGameOverExit.pressed = true; }
                        else { buttonGameOverExit.pressed = false; }
                        if (mousestate4.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            Application.Exit();
                        }
                    }

                    prevMs1 = mousestate4;

                    break;
                case GameState.Victory:

                    //Gör så att eftertexterna rullar ner
                    if (VictoryCreditsPos3.Y > -220)
                    {
                        VictoryCreditsPos.Y -= 1;
                        VictoryCreditsPos2.Y -= 1;
                        VictoryCreditsPos3.Y -= 1;
                    }

                    MouseState mousestate5 = Mouse.GetState();
                    var mouseposition5 = new Point(mousestate5.X, mousestate5.Y);

                    
                    if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed)) //Gör så att knapparna visas, så att man kan stänga av eller gå till menyn
                    { VictoryShowButtons = true; }

                    if (VictoryShowButtons == true)
                    {
                        // går till huvudmenyn och resetar variabler till sina ursprungliga värden för att man skall kunna spela igen
                        if (buttonVictoryMenu.buttonRect.Contains(mouseposition5))
                        {
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            { buttonVictoryMenu.pressed = true; }
                            else { buttonVictoryMenu.pressed = false; }
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            {
                                soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                                saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar på load save för att reseta stats
                                player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats   //Resetar karaktärs stas som str, dex, hp

                                IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                                playmenumusic = true;   //Gör så att menymusiken kan spelas
                                playingamemusic = true; //Gör så att ingame musiken kan spelas upp igen

                                VictoryShowButtons = false;

                                //Nollställer positionerna på credits
                                VictoryCreditsPos.Y =0;
                                VictoryCreditsPos2.Y =2000;
                                VictoryCreditsPos3.Y =4000;

                                currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                            }
                        }

                        //stänger av spelet
                        if (buttonVictoryExit.buttonRect.Contains(mouseposition5))
                        {
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            { buttonVictoryExit.pressed = true; }
                            else { buttonVictoryExit.pressed = false; }
                            if (mousestate5.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                            {
                                soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                                Application.Exit();
                            }
                        }
                    }
                    prevMs1 = mousestate5;

                    break;
                case GameState.Pause:   //Körs om man pausar

                    //Gör så att bakgrunden rör sig
                    if (inGameMenyBackGroundPos.Y <= -1300)
                    {
                        inGameMenyBackGroundPos.Y = 0;
                    }
                    inGameMenyBackGroundPos.Y -= 2;


                    MouseState mousestate6 = Mouse.GetState();
                    var mouseposition6 = new Point(mousestate6.X, mousestate6.Y);

                    // går till huvudmenyn och resetar variabler till sina ursprungliga värden för att man skall kunna spela igen
                    if (buttonInGameMenuExit.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuExit.pressed = true; }
                        else { buttonInGameMenuExit.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            saveAndLoadGame.resetGameStats(ref player, ref floor, ref enemies, ref hpBarPos.Width); //Kallar på load save för att reseta stats
                            player.SetNewGameStats(); //kallar på en metod som updaterar spelarstats   //Resetar karaktärs stas som str, dex, hp

                            IngameTGU.Stop(AudioStopOptions.Immediate); //Slutar spela upp ingameljud
                            playmenumusic = true;   //Gör så att menymusiken kan spelas
                            playingamemusic = true; //Gör så att ingame musiken kan spelas upp igen

                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    //Återupptar spelet
                    if (buttonInGameMenuResume.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuResume.pressed = true; }
                        else { buttonInGameMenuResume.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            currentGameState = GameState.Game;

                        }
                    }

                    //Sparar spelet
                    if (buttonInGameMenuSave.buttonRect.Contains(mouseposition6))
                    {
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInGameMenuSave.pressed = true; }
                        else { buttonInGameMenuSave.pressed = false; }
                        if (mousestate6.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            saveAndLoadGame.SaveTheGame(player, floor, enemies, hpBarPos.Width, positionManager);

                        }
                    }

                    prevMs1 = mousestate6;
                    KeyboardState ks2 = Keyboard.GetState();
                    
                    //Startar spelet igen om man trycker escape
                    if (ks2.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && prevKs.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))  //Knapptryckning för att röra sig ner
                    {
                        currentGameState = GameState.Game;
                    }

                    prevKs = ks2;


                    break;
                case GameState.Information:

                    MouseState mousestate7 = Mouse.GetState();
                    var mouseposition7 = new Point(mousestate7.X, mousestate7.Y);

                    // går till huvudmenyn och resetar variabler till sina ursprungliga värden för att man skall kunna spela igen
                    if (buttonInformationBack.buttonRect.Contains(mouseposition7))
                    {
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInformationBack.pressed = true; }
                        else { buttonInformationBack.pressed = false; }
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            currentGameState = GameState.MainMenu;  //Byter gamestate till huvudmenyn
                        }
                    }

                    //Kör credits
                    if (buttonInformationCredits.buttonRect.Contains(mouseposition7))
                    {
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        { buttonInformationCredits.pressed = true; }
                        else { buttonInformationCredits.pressed = false; }
                        if (mousestate7.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.LeftButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
                        {
                            soundBank.PlayCue("CLICK18B");   //Spelar upp knappljud
                            currentGameState = GameState.Victory;
                        }
                    }

                    prevMs1 = mousestate7;

                    break;
            }
            base.Update(gameTime);
        }

        //Skapar nya fiender
        protected void AddEnemy()
        {
            //Variabler för hp, str, dex, fart, xp
            int HPtemp = 0;
            int STRtemp = 0;
            int DEXtemp = 0;
            int enemySpeed = 0;
            int enemyEXP = 0;
            Texture2D tempGFX = goblinGFX;
            switch (rnd.Next(3))    //Switch för vilken sorts fiende som skall spawna
            {
                case 0: //fiendetyp: snabb goblin
                    HPtemp = 4;
                    STRtemp = 3;
                    DEXtemp = 12;
                    enemySpeed = 8;
                    enemyEXP = 30;
                    tempGFX = gGoblinGFX;
                    break;
                case 1: //fiendetyp: Vanlig goblin
                    HPtemp = 20; 
                    STRtemp = 8; 
                    DEXtemp = 12; 
                    enemySpeed = 2;
                    enemyEXP = 60;
                    break;
                case 2: //fiendetyp: Darkelf
                    HPtemp = 14;
                    STRtemp = 8; 
                    DEXtemp = 8; 
                    enemySpeed = 2;
                    enemyEXP = 80;
                    tempGFX = darkelfGFX;
                    break;
            }

            //Gör att de nya fienderna som spawnar blir starkare beroende på spelarens level
            int enemyhp = HPtemp + (player.Level * 3);
            int enemystr = DEXtemp + (player.Level*2);
            int enemydex = STRtemp + (player.Level*3);
            
            //Gör att fienden spawnar på en random position, dock ej nära spelaren
            int temp = rnd.Next(floortiles.Count - 1);
            int pSight = 8; //antalet rutor  rutnätet nära spelaren som fienden inte kan spawna vid
            if (!(((floortiles.ElementAt(temp).Position.X) / 64 > player.playerPosX - pSight) && //Ser till så att fienden inte spawnar nära spelaren
            ((floortiles.ElementAt(temp).Position.X) / 64 < player.playerPosX + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 < player.playerPosY + pSight) &&
            ((floortiles.ElementAt(temp).Position.Y) / 64 > player.playerPosY - pSight)) &&
            (positionManager[(int)(floortiles.ElementAt(temp).Position.Y) / 64, (int)(floortiles.ElementAt(temp).Position.X) / 64, floor].type == "empty"))
            {
                enemies.Add(new Enemy(enemyhp, enemystr, enemydex, tempGFX, enemySpeed, enemyEXP)   //Lägger till en ny fiende
                {
                    xCoord = ((int)floortiles.ElementAt(temp).Position.X) / 64, //Ger fienden sin rutnätsposition
                    yCoord = ((int)floortiles.ElementAt(temp).Position.Y) / 64, //Ger fienden sin rutnätsposition
                    Position = new Vector2(floortiles.ElementAt(temp).Position.X, floortiles.ElementAt(temp).Position.Y)    //Ger fienden sin pixelposition
                });
                positionManager[((int)floortiles.ElementAt(temp).Position.Y) / 64, ((int)floortiles.ElementAt(temp).Position.X) / 64, floor].type = "enemy";    //Tilldelar rätt grafik på fienden
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

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            switch (currentGameState)   //Ritar ut rätt grafik för rätt gamestate
            {
                case GameState.MainMenu:

                    //Ritar ut menybakgrund
                    spriteBatch.Draw(mainMenuGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.96f);
                    spriteBatch.Draw(mainmenuBackGround, mainmenuBackGroundPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.94f);

                    //ritar grafik så det ser ut som man trycker på knapparna
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
                    foreach (GameObj tile in floortiles)    //Ritar ut allt golv
                    {
                        tile.Draw(spriteBatch, player.Position, 0);
                    }
                    foreach (GameObj wall in walls)     //Ritar ut alla väggar
                    {
                        wall.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y + 46), (0.9f / 34f) *(wall.Position.Y/64f));
                    }
                    foreach (GameObj obj in objects)       //ritar ut alla objekt
                    {
                        if (positionManager[(int)obj.Position.Y/64,(int)obj.Position.X/64,floor].type == "door")
                            obj.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y + 78), ((0.9f / 34f) * ((obj.Position.Y / 64f)-0.0000001f)));
                        else
                            obj.Draw(spriteBatch, new Vector2(player.Position.X, player.Position.Y ), (0.001f));
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
                   
                    //Ritar ut spelaren
                    player.Draw(spriteBatch, player.Position, (0.9f / 34) * ((float)player.playerPosY));

                    //Ritar ut alla fiender
                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(spriteBatch, player.Position, (0.9f / 34) * ((float)enemy.yCoord));

                    }

                    if (attackDone2 == false) //gör så att spelarens attackanimation bara visas nr man attackerar
                    {
                        attack2.Draw(spriteBatch, player.Position, 1f);
                    }

                    if (attackDone2 == false && playerDamgeDelt > 0)    //Ritar ut text som säger hur mycket skada man gör
                    {
                        spriteBatch.DrawString(hpText2, playerDamgeDelt.ToString(), new Vector2(((player.playerPosX) - attack2.randomnr), (player.playerPosY) - 64) + new Vector2(400, 350), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                    else if (playerDamgeDelt == 0 && attackDoneMiss == false)   //Ifall man missar ritas text ut som informerar om det
                    {
                        spriteBatch.DrawString(hpText2, "Miss!", new Vector2(((player.playerPosX) - attack2.randomnr), (player.playerPosY) - 64) + new Vector2(400, 350), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }

                    //Ritar ut attacker
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i].attackAnimationDone == true )
                        {
                            enemies[i].Gfx2 = enemyAttackGfx;
                            enemies[i].AttackDraw(spriteBatch, player.Position,1f , new Vector2((player.playerPosX) * 64, (player.playerPosY) * 64));
                        }
                        if (enemies[i].attackDidDmg == false)   //Ritar ut text för fiendens skada
                        {
                            enemies[i].DmgDraw(spriteBatch, hpText2, enemydmg.ToString(), new Vector2((enemies[i].Position.X) - enemies[i].randomnr, (enemies[i].Position.Y)) - enemies[i].Position + new Vector2(400, 350), Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                        }
                        else if (enemies[i].attackMissed == false && skada == 0)    //Ritar ut text som säger att fienden missade
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

                    //Ritar ut bakgrunden
                    spriteBatch.Draw(gameOverGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    if (buttonGameOverMenu.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonGameOverMenu.draw(spriteBatch, 0.95f, menuKnappGfx); }
                    else { buttonGameOverMenu.pressed = false; }

                    if (buttonGameOverExit.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonGameOverExit.draw(spriteBatch, 0.95f, exitKnappGfx); }
                    else { buttonGameOverExit.pressed = false; }

                    break;
                case GameState.Victory:

                    //Riar ut alla texturer som bygger upp eftertexten
                    spriteBatch.Draw(VictoryCreditsGfx, VictoryCreditsPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits
                    spriteBatch.Draw(VictoryCreditsGfx2, VictoryCreditsPos2, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits
                    spriteBatch.Draw(VictoryCreditsGfx3, VictoryCreditsPos3, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.09f); //Credits

                    if (VictoryShowButtons == true) //OM sant ritar ut knaparna för menyn
                    {

                        spriteBatch.Draw(victoryGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                        if (buttonVictoryMenu.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                        { buttonVictoryMenu.draw(spriteBatch, 0.95f, menuKnappGfx); }
                        else { buttonVictoryMenu.pressed = false; }

                        if (buttonVictoryExit.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                        { buttonVictoryExit.draw(spriteBatch, 0.95f, exitKnappGfx); }
                        else { buttonVictoryExit.pressed = false; }
                    }


                    break;
                case GameState.Pause:

                    //Ritar ut bakgrunden
                    spriteBatch.Draw(inGameMenyBackGroundGfx, inGameMenyBackGroundPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    //Ritar ut menyrutan
                    spriteBatch.Draw(inGameMenyMenuGfx, new Vector2(240, 100), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);

                    //ritar ut knappar
                    if (buttonInGameMenuExit.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonInGameMenuExit.draw(spriteBatch, 1f, exitKnappGfx); }
                    else { buttonInGameMenuExit.pressed = false; }

                    if (buttonInGameMenuResume.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonInGameMenuResume.draw(spriteBatch, 1f, buttonInGameMenuResumeGfx); }
                    else { buttonInGameMenuResume.pressed = false; }

                    if (buttonInGameMenuSave.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonInGameMenuSave.draw(spriteBatch, 1f, buttonInGameMenuSaveGfx); }
                    else { buttonInGameMenuSave.pressed = false; }

                    break;
                case GameState.Information:

                    //Ritar ut bakgrundsgrafik
                    spriteBatch.Draw(informationMenuBackGroundGfx, new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);

                    if (buttonInformationBack.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonInformationBack.draw(spriteBatch, 1f, buttonNewGameBackGfx); }
                    else { buttonInformationBack.pressed = false; }

                    if (buttonInformationCredits.pressed == true)//Gör så det ser ut som man trycker på knappen new game
                    { buttonInformationCredits.draw(spriteBatch, 1f, buttonInformationCreditsGfx); }
                    else { buttonInformationCredits.pressed = false; }

                    break;

            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
