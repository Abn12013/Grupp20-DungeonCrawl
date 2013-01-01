using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

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

        public void SaveTheGame(Character Player, int floor, List<Enemy> enemis, float bredd)
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
                saveGame.WriteLine(enemis[i].xCoord);
                saveGame.WriteLine(enemis[i].yCoord);
                saveGame.WriteLine(enemis[i].hp);
            }

            //överiga saker
            saveGame.WriteLine(bredd);

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
                enemis.Add(new Enemy(18,5));
                
                enemis[i].xCoord = int.Parse(loadGame.ReadLine());
                enemis[i].yCoord = int.Parse(loadGame.ReadLine());
                
                positionmanger[enemis[i].yCoord, enemis[i].xCoord, floor].type = "enemy";
                enemis[i].hp = int.Parse(loadGame.ReadLine());

            }

            //överiga värden
            bredd = int.Parse(loadGame.ReadLine());
            
            loadGame.Close();

        }
    }
}
