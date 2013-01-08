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
    class Character : MovingGameObj
    {


        //int totDex = 0;
        //int totStr = 0;
        //int totHp = 0;
        //int xp = 0;
        //int level = 1;

        //int classStr = 0;
        //int classDex = 0;
        //int classHp = 0;

        //int raceStr = 0;
        //int raceDex = 0;
        //int raceHp = 0;
        public Character()
        {
        }

        public Character(int CStr, int CDex, int CHp, int RStr, int RDex, int RHp, string Pclass)
        {
            hej = 19;

            //ClassStr = CStr;
            //ClassDex = CDex;
            //ClassHp = CHp;

            //RaceDex = RDex;
            //RaceStr = RStr;
            //RaceHp = RHp;

            //Totstr = RaceStr + ClassStr;
            //Totdex = RaceDex + ClassDex;
            //TotalHp = RaceHp + ClassHp;

            maximumHp = TotalHp;

            Position = new Vector2(300, 300);

            Level = 1;
            Xp = 0;
            XpToLevel = 100;

            //Klass vid start av spel
            playerClass = "Fighter";
            PlayerRace = "Orc";

            //ClassStr = 10;
            //ClassDex = 3;
            //ClassHp = 13;

            //RaceDex = RDex;
            //RaceStr = RStr;
            //RaceHp = RHp;

            //Totstr = RaceStr + ClassStr;
            //Totdex = RaceDex + ClassDex;
            //TotalHp = RaceHp + ClassHp;

            victoryConition = false;
            Victory = false;

            Frame = 0;

            moveCharRight = false;
            moveCharLeft = false;
            moveCharUp = false;
            moveCharDown = false;

            allowButtonPress = true;

            SetNewGameStats();

            //sätter startpositionen
            playerPosX = 8;
            playerPosY = 2;
            //Position = new Vector2(playerPosX * 64, playerPosY * 64);

        }

        public bool victoryConition
        { get; set; }

        public bool Victory
        { get; set; }

        public int playerPosY   //Håller koll på spelarens Y pos i rutnätet
        { get; set; }

        public int playerPosX    //Håller koll på spelarens X pos i rutnätet
        { get; set; }

        public float maximumHp
        { get; set; }

        public int hej
        { get; set; }

        //Vilken fram som skall visas
        public int Frame
        { get; set; }

        //Sätter det unika klass str-värdet
        public int ClassStr
        { get; set; }

        //Sätter det unika klass dex-värdet
        public int ClassDex
        { get; set; }

        //Sätter det unika klass hp-värdet
        public int ClassHp
        { get; set; }



        //Sätter det unika race str-värdet
        public int RaceStr
        { get; set; }

        //Sätter det unika race dex-värdet
        public int RaceDex
        { get; set; }

        //Sätter det unika race hp-värdet
        public int RaceHp
        { get; set; }



        //sätter eller hämtar karaktärens level
        public int Level
        { get; set; }

        public int Xp
        { get; set; }

        public int XpToLevel
        { get; set; }


        //sätter eller hämtar karaktärens hp
        public int TotalHp
        { get; set; }


        //hämtar det totala dex-värdet
        public int Totdex
        { get; set; }

        //hämtar det totala str-värdet
        public int Totstr
        { get; set; }

        //spelaren klass
        public string playerClass
        { get; set; }
        public string PlayerRace
        { get; set; }

        //spelarens gender
        public string playerGender
        { get; set; }


        public void SetNewGameStats()
        {

            switch (playerClass)
            { 
                case "Fighter":
                    ClassStr = 10;
                    ClassDex = 3;
                    ClassHp = 13;
                    break;
                case "Rogue":
                     ClassStr = 6;
                     ClassDex = 8;
                     ClassHp = 10;
                    break;
                case "Tank":
                     ClassStr = 6;
                     ClassDex = 3;
                     ClassHp = 16;
                    break;
            }

            switch (PlayerRace)
            { 
                case "Dwarf":
                      RaceDex = 3;
                      RaceStr = 5;
                      RaceHp = 13;
                    break;
                case "Orc":
                    RaceDex = 3;
                    RaceStr = 7;
                    RaceHp = 12;
                    break;
                case "Elf":
                    RaceDex = 6;
                    RaceStr = 3;
                    RaceHp = 10;
                    break;
            }

            TotalHp = RaceHp + ClassHp;
            Totdex = ClassDex + RaceDex;
            Totstr = ClassStr + RaceStr;

            maximumHp = TotalHp;

        }


        public bool VisaLevelUp = false;

        public void LevelUp(ref int hpbarbredd)
        {
            Level++;
            switch (playerClass)
            {
                case "Fighter":
                    Totstr += 5;
                    Totdex += 3;
                    TotalHp += 4;
                    break;
                case "Rogue":
                    Totstr += 3;
                    Totdex += 6;
                    TotalHp += 3;
                    break;
                case "Tank":
                    Totstr += 4;
                    Totdex += 3;
                    TotalHp += 5;
                    break;
            }
            TotalHp = (int)maximumHp + 5 * Level;
            maximumHp = TotalHp;
            Xp = Xp - XpToLevel;
            XpToLevel = XpToLevel + 50 * Level;
            hpbarbredd = 412;
            VisaLevelUp = true;
            
        }

        //Hålle koll på vilken knapp som har tryckts på, och åt vilket håll karaktären ska röra sig
        public bool moveCharRight   //Om man tryckt på "D" för att röra sig åt höger.
        { get; set; }
        public bool moveCharLeft    //Om man tryckt på "A" för att röra sig åt vänster.  
        { get; set; }
        public bool moveCharUp  //Om man tryckt på "W" för att röra sig upp.
        { get; set; }
        public bool moveCharDown    //Om man tryckt på "S" för att röra sig ner.
        { get; set; }

        public bool allowButtonPress    //Håller koll om någon av knapparnda "W, A, S, D" har tryckts ner och gör så att man inte kan 
        { get; set; }                   //Trycka på flera knappar samtidigt.

        public int moved = 0;  //Håller koll på hur långt karaktären har rört sig, och stännar rörelsen när karaktären rört sig 64 pixlar.

        public void MoveRight() //Medtod om man rör sig till höger
        {
            float Xpos = Position.X;    //Karaktärens nuvarande position på x-leden
            moved += 4; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 21)
            { Frame = 7; }
            else if (moved > 21 && moved < 42)
            { Frame = 6; }
            else if (moved > 42 && moved < 64)
            { Frame = 8; }

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveCharRight = false; moved = 0; allowButtonPress = true; Frame = 6; } //olika variabler ändras så att man nu kan genomföra en ny rörelse
            Position = new Vector2(Xpos += 4, Position.Y); //positionen för karaktären ökar med 4 för varje tick av gametime
        }
        
        public void MoveLeft()  //Medtod om man rör sig till left
        {
            float Xpos = Position.X;    //Karaktärens nuvarande position på x-leden
            moved += 4; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 21)
            { Frame = 4; }
            else if (moved > 21 && moved < 42)
            { Frame = 3; }
            else if (moved > 42 && moved < 64)
            { Frame = 5; }

            if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveCharLeft = false; moved = 0; allowButtonPress = true; Frame = 3; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse
            Position = new Vector2(Xpos -= 4, Position.Y);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }

        public void MoveUp()    //Medtod om man rör sig upp
        {
            float Ypos = Position.Y;    //Karaktärens nuvarande position på y-leden
            moved += 4; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 10; }
            else if (moved > 34 && moved < 64)
            { Frame = 11; }

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveCharUp = false; moved = 0; allowButtonPress = true; Frame = 9; } //olika variabler ändras så att man nu kan genomföra en ny rörelse

            Position = new Vector2(Position.X, Ypos -= 4);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }
        
        public void MoveDown()  //Medtod om man rör sig ner
        {
            float ypos = Position.Y;    //Karaktärens nuvarande position på y-leden
            moved += 4; //plussas på med 4 för vara tick av gametime.

            //animerar karaktären
            if (moved > 1 && moved < 34)
            { Frame = 1; }
            else if (moved > 34 && moved < 64)
            { Frame = 2;}

            else if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moveCharDown = false; moved = 0; allowButtonPress = true; Frame = 0; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse

            Position = new Vector2(Position.X, ypos += 4);  //positionen för karaktären ökar med 4 för varje tick av gametime
        }

        public void Update(GameTime gameTime, int floor1) 
        {
            if (moveCharRight == true)  //Om knappen för att röra sig till höger har tryckts ner, blir boolen true och metoden för att röra sig körs för varje tick av gametime
            { MoveRight(); }

            if (moveCharLeft == true)   //Om knappen för att röra sig till vänster har tryckts ner, blir boolen true och metoden för att röra sig körs för varje tick av gametime
            { MoveLeft(); }

            if (moveCharUp == true) //Om knappen för att röra sig upp har tryckts ner, blir boolen true och metoden för att röra sig körs för varje tick av gametime
            { MoveUp(); }

            if (moveCharDown == true)   //Om knappen för att röra sig ner har tryckts ner, blir boolen true och metoden för att röra sig körs för varje tick av gametime
            { MoveDown(); }


            //Victory conditions
            if (playerPosX == 8 && playerPosY == 2 && floor1 == 0)
            {
                if (victoryConition == true)
                {
                    MessageBox.Show("Add victory screen adam");
                    Victory = true;

                }
                
            }
            if (playerPosX == 31 && playerPosY == 4 && floor1 == 2)
            {
                if (victoryConition == false)
                {
                    //MessageBox.Show("skatten e din, add graphic adamplz, one job");
                }
                victoryConition = true;
            }


            base.Update(gameTime);
        }

        public Texture2D Gfx2 //Grafik för orc
        { get; set; }

        public Texture2D Gfx3 //Grafik för elf
        { get; set; }

        public override void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        {
            Rectangle tmp2 = new Rectangle((Frame % 3) * 56, (Frame / 3) * 56, 56, 56);
            switch (PlayerRace) //Beroende på vilken ras man väljer kommer olika sprite sheets att väljas
            { 
                case "Dwarf":

                  
                  spriteBatch.Draw(Gfx,
                  Position - DrawOffset + new Vector2(400, 350),
                  tmp2, Color.White, 0,
                  new Vector2(28, 28), 1.0f, SpriteEffects.None, layer);

                    break;
                case "Orc":

                  spriteBatch.Draw(Gfx2,
                  Position - DrawOffset + new Vector2(400, 350),
                  tmp2, Color.White, 0,
                  new Vector2(28, 28), 1.0f, SpriteEffects.None, layer);

                    break;
                case "Elf":

                  spriteBatch.Draw(Gfx3,
                  Position - DrawOffset + new Vector2(400, 350),
                  tmp2, Color.White, 0,
                  new Vector2(28, 28), 1.0f, SpriteEffects.None, layer);

                    break;
            }
           
        }


    }
}
