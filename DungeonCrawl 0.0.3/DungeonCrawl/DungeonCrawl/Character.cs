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
    class Character :MovingGameObj
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

            ClassStr = CStr;
            ClassDex = CDex;
            ClassHp = CHp;

            RaceDex = RDex;
            RaceStr = RStr;
            RaceHp = RHp;

            Totstr = RaceStr;
            Totdex = RaceDex;
            TotalHp = RaceHp;

            Position = new Vector2(300, 300);

            Level = 1;
            Xp = 0;
            XpToLevel = 100;
            playerClass = Pclass;



            Angle = -(float)(Math.PI / 2);
                
        }

        public int hej
        {
            get;
            set;
        }

        //Sätter det unika klass str-värdet
        public int ClassStr
        {
            get;
            set;
        }

        //Sätter det unika klass dex-värdet
        public int ClassDex
        {
            get;
            set;
        }

        //Sätter det unika klass hp-värdet
        public int ClassHp
        {
            get;
            set;
        }



        //Sätter det unika race str-värdet
        public int RaceStr
        {
            get;
            set;
        }

        //Sätter det unika race dex-värdet
        public int RaceDex
        {
            get;
            set;
        }

        //Sätter det unika race hp-värdet
        public int RaceHp
        {
            get;
            set;
        }
        

        
        //sätter eller hämtar karaktärens level
        public int Level
        {
            get;
            set;
        }

        public int Xp
        {
            get;
            set;
        }
        public int XpToLevel
        {
            get;
            set;
        }
        

        //sätter eller hämtar karaktärens hp
        public int TotalHp
        {
            get;
            set;
        }
        

        //hämtar det totala dex-värdet
        public int Totdex
        {
            get;
            set;
        }

        //hämtar det totala str-värdet
        public int Totstr
        {
            get;
            set;
        }

        //spelaren klass
        public string playerClass
        { get; set; }

        //spelarens gender
        public string playerGender
        { get; set; }



        public void LevelUp()
        {
            Level++;
            switch (playerClass)
            {
                case "Fighter":
                    Totstr = ClassStr + 50;
                    Totdex = ClassDex + 30;
                    TotalHp = TotalHp + 40;
                    break;
                case "Rogue":
                    Totstr = ClassStr + 30;
                    Totdex = ClassDex + 40;
                    TotalHp = TotalHp + 30;
                    break;
                case "Tank":
                    Totstr = ClassStr + 50;
                    Totdex = ClassDex + 30;
                    TotalHp = TotalHp + 40;
                    break;
            }
            TotalHp = TotalHp;
            Xp = Xp - XpToLevel;
            XpToLevel = XpToLevel + 10 * Level;
        }



        public void MoveRight()
        {
            float Xpos = Position.X;

            Position = new Vector2(Xpos += 64, Position.Y);
        }

        public void MoveLeft()
        {
            float Xpos = Position.X;

            Position = new Vector2(Xpos -= 64, Position.Y);
        }

        public void MoveUp()
        {
            float Ypos = Position.Y;

            Position = new Vector2(Position.X , Ypos -= 64);
        }

        public void MoveDown()
        {
            float ypos = Position.Y;

            Position = new Vector2(Position.X, ypos += 64);
        }



        public override void Update(GameTime gameTime)
        {
            KeyboardState duug = Keyboard.GetState();



            if (duug.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {

                
            }

            Direction = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));

            base.Update(gameTime);
        }
        

    }
}
