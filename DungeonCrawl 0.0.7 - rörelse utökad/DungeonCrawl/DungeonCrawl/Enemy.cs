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
    class Enemy : MovingGameObj
    {
        //Ansvarig Albin Billman

        //Denna klass har som uppgift att skapa nya fiender. Denna innehåller alla des attribut, och det viktigaste dess ai och rörelse/attack.

        public int hp;  //Fiendens hp
        public int dex; //fiendens dex
        public int str; //Fiendens str

        Random random = new Random();
        
        int sight = 5;  //Så nära man måste vara, i koordinater för att fiendens skall bli aktiv

        public int exp = 0; //Xp som fienden ger när den dör
        int movespeed = 2; //måste vara delbart med 64, fiendens rörelsehastighet

        Random rndnr = new Random();
        public int randomnr;

        public Enemy()
        {

        }
        public Enemy(int hp, int str, int dex, int speed, int exp)
        {
            this.hp = hp;   //Tilldelar hp
            this.str = str; //Tilldelar str
            this.dex = dex; //Tilldelar dex
            movespeed = speed;  //Tilldelar rörelsefart
            this.exp = exp; //Tilldelar xp

            ActiveMove = false; //Den kan inte röra sig från början

            allowMove = true;
            attackMissed = true;
            attackDidDmg = true;
       
            moveEnemyrRight = false;
            moveEnemyLeft = false;
            moveEnemyUp = false;
            moveEnemyDown = false;

            Frame = 0;

        }

        //Konstruktor som kallar på den andra konstruktorn
        public Enemy(int hp, int str, int dex, Texture2D Gfx, int speed, int exp):this(hp, str, dex, speed, exp)
        {
            this.Gfx = Gfx;
        }

        public int EnemyAttackCalc(int dex) //Fiendens attack uträkning
        {
            //Denna fungerar på samma sätt som characterklassens attackkalk
            int damage = 0; //Skadan som görs
            resetAttack = false;    //Gör att man inte kan utföra en ny attack tills denna är klar

            if (random.NextDouble() > 1 / (1 + ((double)str / (double)dex) * 3))    //Samma som i characterklassen                       
            {
                randomnr = rndnr.Next(1, 64);
                damage = str - random.Next(0, str / 2);
                attackAnimationDone = true; //kör attackanimation
                attackDidDmg = false;
            }

                else
                {
                    randomnr = rndnr.Next(1, 64);
                    attackMissed = false;
                    damage = 0;
                }
            
            return damage;
       
        }
        public int Frame    
        { get; set; }

        public Vector2 PlayerPos
        { get; set; }

        public int xCoord   //Xpos i koordinater
        { get; set; }

        public int yCoord   //Ypos i koordinater
        { get; set; }

        public bool ActiveMove  //Bool för att kolla när fienden för röra sig
        { get; set; }

        
        public void CheckActive()   //Metod som kollar när fienden skall bli aktiv
        { 

            if (PlayerPos.X > (xCoord - sight) &&
                PlayerPos.X < (xCoord + sight) &&
                PlayerPos.Y > (yCoord - sight) &&
                PlayerPos.Y < (yCoord + sight))
                ActiveMove = true;
            
        }
       
        public bool moveEnemyrRight   //Om man tryckt på "D" för att röra sig åt höger.
        { get; set; }
        public bool moveEnemyLeft    //Om man tryckt på "A" för att röra sig åt vänster.  
        { get; set; }
        public bool moveEnemyUp  //Om man tryckt på "W" för att röra sig upp.
        { get; set; }
        public bool moveEnemyDown    //Om man tryckt på "S" för att röra sig ner.
        { get; set; }


        public bool resetAttack
        { get; set; }

        public bool attackMissed
        { get; set; }

        public bool attackDidDmg //ritar ut text om attack träffar
        { get; set; }

        private bool allowMove;


        private int moved = 0;  //Håller koll på hur långt karaktären har rört sig, och stännar rörelsen när karaktären rört sig 64 pixlar.
        private int moved2 = 0;

        //Attack animation variabler
        private int movedattack = 0;
        public bool attackAnimationDone = false;
        public int attackFrame = 0;

        public void MoveRight2() //Medtod om man rör sig till höger
        {
            float Xpos = Position.X;    //Karaktärens nuvarande position på x-leden
            moved += movespeed; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 7; }
            else if (moved > 34 && moved < 64)
            { Frame = 8; }

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveEnemyrRight = false; moved = 0; allowMove = true; Frame = 6; } //olika variabler ändras så att man nu kan genomföra en ny rörelse
            Position = new Vector2(Xpos += movespeed, Position.Y); //positionen för karaktären ökar med 4 för varje tick av gametime
        }

        public void MoveLeft2()  //Medtod om man rör sig till left
        {
            float Xpos = Position.X;    //Karaktärens nuvarande position på x-leden
            moved += movespeed; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 4; }
            else if (moved > 34 && moved < 64)
            { Frame = 5; }

            if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveEnemyLeft = false; moved = 0; allowMove = true; Frame = 3; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse
            Position = new Vector2(Xpos -= movespeed, Position.Y);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }

        public void MoveUp2()    //Medtod om man rör sig upp
        {
            float Ypos = Position.Y;    //Karaktärens nuvarande position på y-leden
            moved += movespeed; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 10; }
            else if (moved > 34 && moved < 64)
            { Frame = 11; }

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveEnemyUp = false;  moved = 0; allowMove = true; Frame = 9; } //olika variabler ändras så att man nu kan genomföra en ny rörelse

            Position = new Vector2(Position.X, Ypos -= movespeed);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }

        public void MoveDown2()  //Medtod om man rör sig ner
        {
            float ypos = Position.Y;    //Karaktärens nuvarande position på y-leden
            moved += movespeed; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 1; }
            else if (moved > 34 && moved < 64)
            { Frame = 2; }

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveEnemyDown = false; moved = 0; allowMove = true; Frame = 0; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse

            Position = new Vector2(Position.X, ypos += movespeed);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }


        public void Update(GameTime gameTime,  ref PositionManager[,,]  positionManager, int floor, int playerdex, ref int skada, SoundBank soundBank, Cue attackHit, Cue attackMiss, int level, ref Character player1)
        {
            
            if (resetAttack == false)
            {
                moved2 += 1;
                if (moved2 > 1 && moved2 < 16)
                { Frame = 0; }
                else if (moved2 > 16 && moved2 < 32)
                { Frame = 1; }
                else if (moved2 > 32 && moved2 < 48)
                { Frame = 2; }
                else if (moved2 > 48 && moved2 < 64)
                { Frame = 3; }

                if (moved2 == 64) 
                { moved2 = 0; Frame = 0; resetAttack = true; attackMissed = true; attackDidDmg = true; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse
            }

            //attack animation
            if (attackAnimationDone == true)
            {
                movedattack += 4;
                if (movedattack > 1 && movedattack < 16)
                { attackFrame = 0; }
                else if (movedattack > 16 && movedattack < 32)
                { attackFrame = 1; }
                else if (movedattack > 32 && movedattack < 48)
                { attackFrame = 2; }
                else if (movedattack > 48 && movedattack < 64)
                { attackFrame = 3; }

                if (movedattack == 64)    // när man rört sig 64 pixlar så stannar gubben
                { movedattack = 0; attackFrame = 0; attackAnimationDone = false; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse
            }



            CheckActive();  //Kör metoden för att kolla när fienden skall aktiveras

            //Detta ai fungerar så att fienden kollar var spelaren befinner sig i rutnätet. Först kollar den vart den är på X-leden, sedan rör den sig mot fienden tills den är 
            //under den på y-leden. Efter det rör sig fienden mot spelaren på y. Om fienden befinner sig bredvid spelaren kommer den att försöka utföra en attack.
            //Skulle en vägg komma ivägen om fienden exempelvis gick in i en vägg på x-leden kommer den i detta läge undersöka om fienden befinner sig längre upp eller ner
            //på y och sedan röra sig mot spelaren på de viset. Samma sak om den går in i en vägg på y-leden, men då kommer den gå åt något håll på x-leden.

            if (allowMove)  //Om den frå röras startar ai
            {
                if (ActiveMove) // Fienden är aktiv och går mot spelaren samt attackerar om den kan.
                {   // Enemy går mot spelaren om den är tillräckligt när för att attackera så gör den det istället.
                    //if ()//ska kolla om det är kortast till spelaren från Enemy på x kordinaten eller om avståndet är lika.        
                    //{
                    if (PlayerPos.X > xCoord) // kollar om spelaren är till höger om enemy.
                    {


                        if (xCoord + 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till höger om enemy.
                        {
                            if (resetAttack == true)
                            {
                                skada = EnemyAttackCalc(playerdex);
                                if (skada != 0)
                                { soundBank.PlayCue("AttackSound"); }
                                else if (skada == 0)
                                { soundBank.PlayCue("AttackMiss");  }

                                player1.TotalHp = player1.TotalHp - skada;

                            }
                        }
                        // kod för att gå till höger.
                        else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "chest" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "door")
                        {

                            moveEnemyrRight = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord + 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                            positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                            xCoord++;
                        }
                        //Alternativ när fienden går in i en vägg
                        else if (positionManager[yCoord, xCoord + 1, floor].type == "wall" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "enemy" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "upstairs" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "downstairs" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "chest" ||
                           positionManager[yCoord, xCoord + 1, floor].type != "door")
                        {
                            if (PlayerPos.Y < yCoord) // Kollar om spelaren är ovanför enemy.
                            {
                                if (yCoord - 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;

                                    }
                                }
                                // kod för att gå up.
                                else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "chest" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "door")
                                {

                                    moveEnemyUp = true;
                                    allowMove = false;
                                    positionManager[yCoord - 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    yCoord--;
                                }
                            }
                            else if (PlayerPos.Y > yCoord) // annars gå uppåt.
                            {
                                if (yCoord + 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;

                                    }

                                }
                                //  kod för att gå ner.
                                else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "chest" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "door")
                                {
                                    moveEnemyDown = true;
                                    allowMove = false;
                                    positionManager[yCoord + 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    yCoord++;
                                }
                            }



                        }


                    }
                    else if (PlayerPos.X < xCoord)  // Annars gå till vänster.
                    {
                        if (xCoord - 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till vänster om enemy.
                        {
                            if (resetAttack == true)
                            {
                                skada = EnemyAttackCalc(playerdex);
                                if (skada != 0)
                                { soundBank.PlayCue("AttackSound"); }
                                else if (skada == 0)
                                { soundBank.PlayCue("AttackMiss"); }
                                player1.TotalHp = player1.TotalHp - skada;
                              
                                
                            }
                        }
                        // Kod för att gå till vänster

                        else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "chest" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "door")
                        {
                            moveEnemyLeft = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord - 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                            positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                            xCoord--;
                        }

                        else if (positionManager[yCoord, xCoord - 1, floor].type == "wall" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "enemy" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "upstairs" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "downstairs" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "chest" ||
                           positionManager[yCoord, xCoord - 1, floor].type != "door")
                        {
                            if (PlayerPos.Y < yCoord) // Kollar om spelaren är ovanför enemy.
                            {
                                if (yCoord - 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                       
                                    }
                                }
                                // kod för att gå up.
                                else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "chest" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "door")
                                {

                                    moveEnemyUp = true;
                                    allowMove = false;
                                    positionManager[yCoord - 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    yCoord--;
                                }

                            }
                            else if (PlayerPos.Y > yCoord) // annars gå uppåt.
                            {
                                if (yCoord + 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                   
                                    }

                                }
                                //  kod för att gå ner.
                                else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "chest" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "door")
                                {
                                    moveEnemyDown = true;
                                    allowMove = false;
                                    positionManager[yCoord + 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    yCoord++;
                                }
                            }
                            ///////////////////////
                        ///////////////////////
                        }

                    }
                    //}


                    else if (PlayerPos.Y < yCoord) // Kollar om spelaren är ovanför enemy.
                    {
                        if (yCoord - 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                        {
                            if (resetAttack == true)
                            {
                                skada = EnemyAttackCalc(playerdex);
                                if (skada != 0)
                                { soundBank.PlayCue("AttackSound"); }
                                else if (skada == 0)
                                { soundBank.PlayCue("AttackMiss"); }
                                player1.TotalHp = player1.TotalHp - skada;
                              
                            }
                        }
                        // kod för att gå up.
                        else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "chest" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "door")
                        {

                            moveEnemyUp = true;
                            allowMove = false;
                            positionManager[yCoord - 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                            positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                            yCoord--;
                        }
                        else if (positionManager[yCoord - 1, xCoord, floor].type == "wall" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "enemy" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "upstairs" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "downstairs" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "chest" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "door")
                        {
                            if (PlayerPos.X > xCoord) // kollar om spelaren är till höger om enemy.
                            {


                                if (xCoord + 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till höger om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                     

                                    }
                                }
                                // kod för att gå till höger.
                                else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "chest" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "door")
                                {

                                    moveEnemyrRight = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord + 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true};    //Sätter sin förra position i 2d-arrayen till "null"
                                    xCoord++;
                                }

                            }
                            else if (PlayerPos.X < xCoord)  // Annars gå till vänster.
                            {
                                if (xCoord - 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till vänster om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                        

                                    }
                                }
                                // Kod för att gå till vänster

                                else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "chest" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "door")
                                {
                                    moveEnemyLeft = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord - 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    xCoord--;
                                }
                            }
                            //////////////////////////////////
                        //////////////////////////////////
                        }




                    }
                    else if (PlayerPos.Y > yCoord) // annars gå uppåt.
                    {
                        if (yCoord + 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                        {
                            if (resetAttack == true)
                            {
                                skada = EnemyAttackCalc(playerdex);
                                if (skada != 0)
                                { soundBank.PlayCue("AttackSound"); }
                                else if (skada == 0)
                                { soundBank.PlayCue("AttackMiss"); }
                                player1.TotalHp = player1.TotalHp - skada;
                               
                            }
                            
                        }
                        //  kod för att gå ner.
                        else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "chest" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "door")
                        {
                            moveEnemyDown = true;
                            allowMove = false;
                            positionManager[yCoord + 1, xCoord, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                            positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                            yCoord++;
                        }
                        else if (positionManager[yCoord + 1, xCoord, floor].type == "wall" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "enemy" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "upstairs" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "downstairs" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "chest" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "door")
                        {

                            if (PlayerPos.X > xCoord) // kollar om spelaren är till höger om enemy.
                            {


                                if (xCoord + 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till höger om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                       

                                    }
                                }
                                // kod för att gå till höger.
                                else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "chest" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "door")
                                {

                                    moveEnemyrRight = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord + 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    xCoord++;
                                }

                            }
                            else if (PlayerPos.X < xCoord)  // Annars gå till vänster.
                            {
                                if (xCoord - 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till vänster om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                        if (skada != 0)
                                        { soundBank.PlayCue("AttackSound"); }
                                        else if (skada == 0)
                                        { soundBank.PlayCue("AttackMiss"); }
                                        player1.TotalHp = player1.TotalHp - skada;
                                        //attackAnimationDone = true; //Gör så att fiendens attackanimation

                                    }
                                }
                                // Kod för att gå till vänster

                                else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "chest" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "door")
                                {
                                    moveEnemyLeft = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord - 1, floor] = positionManager[yCoord, xCoord, floor]; //Sätter rutan man rörde sig mot till player
                                    positionManager[yCoord, xCoord, floor] = new PositionManager { type = "empty", floor = true };    //Sätter sin förra position i 2d-arrayen till "null"
                                    xCoord--;
                                }
                            }

                        
                        }




                    }
                }
                
            }

            //Kallar på metod för att röra sig, beroende på vilket håll den valt i ai-uträkningarna
            if (moveEnemyrRight == true)    
            {
                MoveRight2();
            }
            if (moveEnemyLeft == true)
            {
                MoveLeft2();
            }
            if (moveEnemyUp == true)
            {
                MoveUp2();
            }
            if (moveEnemyDown == true)
            {
                MoveDown2();
            }
            


            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer) //Ritar ut fiendens grafik
        {
            Rectangle tmp2 = new Rectangle((Frame % 3) * 56, (Frame / 3) * 56, 56, 56);
            spriteBatch.Draw(Gfx,
            Position - DrawOffset + new Vector2(400, 350),
            tmp2, Color.White, 0,
            new Vector2(28, 28), 1.0f, SpriteEffects.None, layer);
        }

        public Texture2D Gfx2
        { get; set; }

        public void AttackDraw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer, Vector2 attackpos) //ritr ut attackanimationen
        {
 
                Rectangle tmp2 = new Rectangle((attackFrame % 4) * 64, (attackFrame / 4) * 64, 64, 64);
                spriteBatch.Draw(Gfx2, attackpos - DrawOffset + new Vector2(400, 350), tmp2, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, layer);

        }

        //Ritar ut text för hur mycket skada som gjorts
        public void DmgDraw(SpriteBatch spritebatch, SpriteFont font, string text, Vector2 pos, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            spritebatch.DrawString(font, text, pos, color, rotation, origin, scale, effects, depth);
        }

        public int ReturnExp()
        {
            return exp;
        }

        public int ReturnSpeed()
        {
            return movespeed;
        }
       

    }
}