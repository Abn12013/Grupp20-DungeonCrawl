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
using System.IO;

namespace DungeonCrawl
{
    class LoadSave
    {
        //This class is responsible for the save and load functions of the program. It contains two methods, one that handle the save and another
        //that handle the load. The save game method gets calld from the Game1.cs and it takes a few arguments: The player class, a list class of the enemis
        //and what floor is he current one. Then it writes all the necessary information to a text file. Each information is written to its own row.

        //The load method reads the text file line by line and adds the values to its corresponding ref objects. This method is loaded from the gamestate
        //load game in the game1.cs. 
      
       
        public LoadSave()
        {
            
        }

        //Ordningen som allt måste laddas in i
        //1: Player.playerPosX
        //2: Player.playerPosY
        //3: Floor
        //4: Player str
        //5: Player Dex
        //6: Player Hp
        //7: Player Max Hp
        //8: Player level
        //9: Player Xp
        //10: Player XpToLevel
        //11: Player Victorycondition
        //12: Player Race
        //13: Player Class
        
        //Fiende värden
        //14: Antalet Fiender
        //15: Fienders xCord
        //16: Fienders yCord
        //17: Fienders Hp

        //Övrigasaker
        //18: hpbar bredd
        //19: Antal öppnade kistor
        //20: Öppnadekistor Y position
        //21: Öppnadekistor X position
        //22: Öppnadekistor Våning

        public void resetGameStats(ref Character Player, ref int floor, ref List<Enemy> enemis, ref int bredd)
        {

            Player.playerPosX = 8;
            Player.playerPosY = 2;
            floor = 0;

            

            Player.Level = 1;
            Player.Xp = 0;
            Player.XpToLevel = 100;

            Player.victoryConition = false;
            Player.PlayerRace = "Orc";
            Player.playerClass = "Fighter";

            bredd = 412;

            enemis.Clear();

            //Fiende värden
            //14: Antalet Fiender
            //15: Fienders xCord
            //16: Fienders yCord
            //17: Fienders Hp
        
        }

        public void SaveTheGame(Character Player, int floor, List<Enemy> enemis, float bredd, PositionManager[,,]positionManager)
        {
            StreamWriter saveGame = new StreamWriter("save");
            //Spelar värden
            saveGame.WriteLine(Player.playerPosX);
            saveGame.WriteLine(Player.playerPosY);
            saveGame.WriteLine(floor);
            saveGame.WriteLine(Player.Totstr);
            saveGame.WriteLine(Player.Totdex);
            saveGame.WriteLine(Player.TotalHp);
            saveGame.WriteLine(Player.maximumHp);
            saveGame.WriteLine(Player.Level);
            saveGame.WriteLine(Player.Xp);
            saveGame.WriteLine(Player.XpToLevel);
            saveGame.WriteLine(Player.victoryConition);
            saveGame.WriteLine(Player.PlayerRace);
            saveGame.WriteLine(Player.playerClass);

            //Fiende värden
            saveGame.WriteLine(enemis.Count );
            for (int i = 0; i < enemis.Count; i++) //sparar data för varje fiende
            {
                saveGame.WriteLine(enemis[i].hp);
                saveGame.WriteLine(enemis[i].ReturnExp());
                saveGame.WriteLine(enemis[i].ReturnSpeed());
                saveGame.WriteLine(enemis[i].xCoord);
                saveGame.WriteLine(enemis[i].yCoord);
            }

            //överiga saker
            saveGame.WriteLine(bredd);

            int antalÖppnadeKistor = 0;

            for (int våning = 0; våning < 3; våning++)
            {
                for (int y = 0; y < 34; y++)
                {
                    for (int x = 0; x < 52; x++)
                    {
                        if (positionManager[y, x, våning].type == "emptychest")
                        {
                            antalÖppnadeKistor += 1;
                        }
                    }
                }
            }
            saveGame.WriteLine(antalÖppnadeKistor);

            for (int våning = 0; våning < 3; våning++)
            {
                for (int y = 0; y < 34; y++)
                {
                    for (int x = 0; x < 52; x++)
                    {
                        if (positionManager[y, x, våning].type == "emptychest")
                        {
                            saveGame.WriteLine(y);
                            saveGame.WriteLine(x);
                            saveGame.WriteLine(våning);
                        }
                    }
                }
            }


            saveGame.Close();
        }

        public void LoadTheGame(ref Character PlayerLoad, ref int floor, ref List<Enemy> enemis, ref PositionManager[,,] positionmanger, ref int bredd)
        {
            StreamReader loadGame = new StreamReader("save");
            //Spelar värden
            PlayerLoad.playerPosX = int.Parse(loadGame.ReadLine());
            PlayerLoad.playerPosY = int.Parse(loadGame.ReadLine());
            floor = int.Parse(loadGame.ReadLine());
            PlayerLoad.Totstr = int.Parse(loadGame.ReadLine());
            PlayerLoad.Totdex = int.Parse(loadGame.ReadLine());
            PlayerLoad.TotalHp = int.Parse(loadGame.ReadLine());
            PlayerLoad.maximumHp = int.Parse(loadGame.ReadLine());
            PlayerLoad.Level = int.Parse(loadGame.ReadLine());
            PlayerLoad.Xp = int.Parse(loadGame.ReadLine());
            PlayerLoad.XpToLevel = int.Parse(loadGame.ReadLine());
            PlayerLoad.victoryConition = bool.Parse(loadGame.ReadLine());
            PlayerLoad.PlayerRace = (loadGame.ReadLine());
            PlayerLoad.playerClass = (loadGame.ReadLine());

            //Finde värden
            int numberOfEnemis = int.Parse(loadGame.ReadLine());
            for (int i = 0; i < numberOfEnemis; i++)
            {
                int enemyhp = int.Parse(loadGame.ReadLine());
                int enemyXP = int.Parse(loadGame.ReadLine());
                int enemySpeed = int.Parse(loadGame.ReadLine());

                int enemystr = 0;
                int enemydex = 0;

                switch (enemyXP)
                {
                    case 30:
                        enemystr = 3;
                        enemydex = 12;
                        break;
                    case 60:
                        enemystr = 8;
                        enemydex = 12;
                        break;
                    case 80:
                        enemystr = 8;
                        enemydex = 18;
                        break;
                    case 200:
                        enemystr = 14;
                        enemydex = 4;
                        break;
                }

                enemis.Add(new Enemy(enemyhp, enemystr, enemydex, enemySpeed, enemyXP));
                
                enemis[i].xCoord = int.Parse(loadGame.ReadLine());
                enemis[i].yCoord = int.Parse(loadGame.ReadLine());
                
                positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].type = "enemy";
                switch (enemyXP)
                {
                    case 30:
                        positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].monster = "g_goblin";
                        break;
                    case 60:
                        positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].monster = "goblin";
                        break;
                    case 80:
                        positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].monster = "dark_elf";
                        break;
                    case 200:
                        positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].monster = "fel_orc";
                        break;
                }

            }

            //överiga värden
            bredd = int.Parse(loadGame.ReadLine());

            int numberOfOpenChest = int.Parse(loadGame.ReadLine());

            if (numberOfOpenChest != 0)
            {
                for (int i = 0; i < numberOfOpenChest; i++)
                {

                    int ypos = int.Parse(loadGame.ReadLine());
                    int xpos = int.Parse(loadGame.ReadLine());
                    int våning = int.Parse(loadGame.ReadLine());

                    positionmanger[ypos, xpos, våning].type = "emptychest";

                }
            }
            
            loadGame.Close();

        }
    }
}
