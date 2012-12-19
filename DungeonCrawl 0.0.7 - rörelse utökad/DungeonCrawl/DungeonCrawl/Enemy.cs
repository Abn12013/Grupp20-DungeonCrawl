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
        public int hp;
        public int dex;
        public int str;
        Random random = new Random();
        Character character = new Character();
        int dmg;
        string gfx;

        int sight = 5;
        // if sats för att kolla om Character är inom 5 rutor av Enemy. Enemy blir då Active.
        int radX = 0;
        int radY = 0;

        int movespeed = 2; //måste vara delbart med 64
        
        public Enemy()
        {

        }
        public Enemy(int hp, int dmg)
        {
            this.hp = hp;
            this.dmg = dmg;
            this.gfx = gfx;
            //xCoord 
            //yCoord 

            ActiveMove = false;

            allowMove = true;

            str = 10;

            //PlayerPos = playerPos;  //may work, annars placera om PlayerPos
           
            // if sats för att kolla om Character är inom 5 rutor av Enemy. Enemy blir då Active.

            radX = xCoord - sight;
            radY = yCoord - sight;

            moveEnemyrRight = false;
            moveEnemyLeft = false;
            moveEnemyUp = false;
            moveEnemyDown = false;

            Frame = 0;

            dex = 5;


        }

        public int EnemyAttackCalc(int dex)
        {

            resetAttack = false;

            if (str + random.Next(0, 20) > dex)                         
            {
                dmg = str - random.Next(0, str / 2);

            }
            else
            {
                int i = random.Next(1, 4);//ger en fjärdedels chans för träff
                if (i == 1)
                {
                    dmg = str / (random.Next(2,5));

                }
                else
                {
                    dmg = 0;
                }
            }

           


            return dmg;
        

        }
        public int Frame
        { get; set; }

        public Vector2 PlayerPos
        { get; set; }

        public int xCoord
        { get; set; }

        public int yCoord
        { get; set; }

        public bool ActiveMove
        { get; set; }



        

        public void CheckActive()
        {
            //int sight = 5;
            //// if sats för att kolla om Character är inom 5 rutor av Enemy. Enemy blir då Active.
            //int radX = xCoord - sight;
            //int radY = yCoord - sight;

            if (PlayerPos.X > (xCoord - sight) &&
                PlayerPos.X < (xCoord + sight) &&
                PlayerPos.Y > (yCoord - sight) &&
                PlayerPos.Y < (yCoord + sight))
                ActiveMove = true;
            
        }

        //private bool Active()// Ska ha en Active metod som kollar om spelaren är tillräckligt nära fienden för att den ska aktiveras varje gång spelaren rör sig.
        //{
        //    int sight = 5;
        //    // if sats för att kolla om Character är inom 5 rutor av Enemy. Enemy blir då Active.
        //    int radX = xCoord - sight;
        //    int radY = yCoord - sight;
        //    Vector2 checkPosNow = new Vector2(radX, radY);

        //    bool isITTURE = false;

        //    for (int i = 0; i < 121; i++)
        //    {

        //        if (checkPosNow == PlayerPos)
        //        {
        //            isITTURE = true;
        //        }
        //        else
        //        {
        //            radX += 1;

        //            if (radX == xCoord + sight)
        //            {
        //                radY++;
        //                if (radY == yCoord + sight)
        //                {
        //                    break;
        //                }
        //            }
        //        }
               
        //    }

            

        //    return true;
            
        //    // kolla om spelaren är tillräckligt nära för att fienden ska vara Active, 4*4 ruta runt spelaren.
        //    // Detta gör den varje gång spelaren rör sig.   
        //}

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


        private bool allowMove;


        private int moved = 0;  //Håller koll på hur långt karaktären har rört sig, och stännar rörelsen när karaktären rört sig 64 pixlar.
        private int moved2 = 0;

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


        public void Update(GameTime gameTime,  ref PositionManager[,,]  positionManager, int floor, int playerdex, ref int skada)
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

                if (moved2 == 64)    // när man rört sig 64 pixlar så stannar gubben
                { moved2 = 0; Frame = 0; resetAttack = true; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse
            }


            CheckActive();

            if (allowMove)
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

                            }
                        }
                        // kod för att gå till höger.
                        else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                            positionManager[yCoord, xCoord + 1, floor].type != "chest")
                        {

                            moveEnemyrRight = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                            positionManager[yCoord, xCoord +1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
                            xCoord++;
                        }
                            //Alternativ när fienden går in i en vägg
                        else if (positionManager[yCoord, xCoord + 1, floor].type == "wall" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "enemy" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "upstairs" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "downstairs" ||
                           positionManager[yCoord, xCoord + 1, floor].type == "chest")
                        {
                            if (PlayerPos.Y < yCoord) // Kollar om spelaren är ovanför enemy.
                            {
                                if (yCoord - 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                    }
                                }
                                // kod för att gå up.
                                else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "chest")
                                {

                                    moveEnemyUp = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord - 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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
                                    }

                                }
                                //  kod för att gå ner.
                                else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "chest")
                                {
                                    moveEnemyDown = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord + 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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
                                
                            }
                        }
                        // Kod för att gå till vänster

                        else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                            positionManager[yCoord, xCoord - 1, floor].type != "chest")
                        {
                            moveEnemyLeft = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                            positionManager[yCoord, xCoord - 1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
                            xCoord--;
                        }

                        else if (positionManager[yCoord, xCoord - 1, floor].type == "wall" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "enemy" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "upstairs" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "downstairs" ||
                           positionManager[yCoord, xCoord - 1, floor].type == "chest")
                        {
                            if (PlayerPos.Y < yCoord) // Kollar om spelaren är ovanför enemy.
                            {
                                if (yCoord - 1 == PlayerPos.Y && xCoord == PlayerPos.X) //kollar om spelaren är på rutan ovnaför enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);
                                    }
                                }
                                // kod för att gå up.
                                else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord - 1, xCoord, floor].type != "chest")
                                {

                                    moveEnemyUp = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord - 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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
                                    }

                                }
                                //  kod för att gå ner.
                                else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                                    positionManager[yCoord + 1, xCoord, floor].type != "chest")
                                {
                                    moveEnemyDown = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord + 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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
                            }
                        }
                        // kod för att gå up.
                        else if (positionManager[yCoord - 1, xCoord, floor].type != "wall" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "enemy" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "upstairs" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "downstairs" &&
                            positionManager[yCoord - 1, xCoord, floor].type != "chest")
                        {

                            moveEnemyUp = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                            positionManager[yCoord - 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
                            yCoord--;
                        }
                        else if (positionManager[yCoord - 1, xCoord, floor].type == "wall" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "enemy" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "upstairs" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "downstairs" ||
                            positionManager[yCoord - 1, xCoord, floor].type == "chest")
                        {
                            if (PlayerPos.X > xCoord) // kollar om spelaren är till höger om enemy.
                            {


                                if (xCoord + 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till höger om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);

                                    }
                                }
                                // kod för att gå till höger.
                                else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "chest")
                                {

                                    moveEnemyrRight = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord, xCoord + 1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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

                                    }
                                }
                                // Kod för att gå till vänster

                                else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "chest")
                                {
                                    moveEnemyLeft = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord, xCoord - 1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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
                            }
                            
                        }
                        //  kod för att gå ner.
                        else if (positionManager[yCoord + 1, xCoord, floor].type != "wall" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "enemy" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "upstairs" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "downstairs" &&
                            positionManager[yCoord + 1, xCoord, floor].type != "chest")
                        {
                            moveEnemyDown = true;
                            allowMove = false;
                            positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                            positionManager[yCoord + 1, xCoord, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
                            yCoord++;
                        }
                        else if (positionManager[yCoord + 1, xCoord, floor].type == "wall" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "enemy" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "upstairs" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "downstairs" ||
                           positionManager[yCoord + 1, xCoord, floor].type == "chest")
                        {

                            if (PlayerPos.X > xCoord) // kollar om spelaren är till höger om enemy.
                            {


                                if (xCoord + 1 == PlayerPos.X && yCoord == PlayerPos.Y) //kollar om spelaren är på rutan till höger om enemy.
                                {
                                    if (resetAttack == true)
                                    {
                                        skada = EnemyAttackCalc(playerdex);

                                    }
                                }
                                // kod för att gå till höger.
                                else if (positionManager[yCoord, xCoord + 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord + 1, floor].type != "chest")
                                {

                                    moveEnemyrRight = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord, xCoord + 1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
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

                                    }
                                }
                                // Kod för att gå till vänster

                                else if (positionManager[yCoord, xCoord - 1, floor].type != "wall" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "enemy" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "upstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "downstairs" &&
                                    positionManager[yCoord, xCoord - 1, floor].type != "chest")
                                {
                                    moveEnemyLeft = true;
                                    allowMove = false;
                                    positionManager[yCoord, xCoord, floor].type = "empty";    //Sätter sin förra position i 2d-arrayen till "null"
                                    positionManager[yCoord, xCoord - 1, floor].type = "enemy"; //Sätter rutan man rörde sig mot till player
                                    xCoord--;
                                }
                            }

                        
                        }




                    }
                }
                
            }

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

        public override void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        {
            Rectangle tmp2 = new Rectangle((Frame % 3) * 56, (Frame / 3) * 56, 56, 56);
            spriteBatch.Draw(Gfx,
            Position - DrawOffset + new Vector2(400, 350),
            tmp2, Color.White, 0,
            new Vector2(28, 28), 1.0f, SpriteEffects.None, layer);
        }

        //public override void Draw2(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        //{
        //    Rectangle tmp2 = new Rectangle((Frame % 4) * 64, (Frame / 4) * 64, 64, 64);
        //    spriteBatch.Draw(Gfx, Position - DrawOffset + new Vector2(400, 350), tmp2, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, layer);
        //}

    }
}