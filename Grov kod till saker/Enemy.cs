using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawl
{
    class Enemy : GameObj
    {
        int hp;
        int dmg;
        string gfx;
        int xcoord;
        int ycoord;
        
        public Enemy()
        { 
        
        }
        public Enemy(int hp, int dmg, string gfx, int xcoord, int ycoord)
        {
            this.hp = hp;
            this.dmg = dmg;
            this.gfx = gfx;
            this.xcoord = xcoord;
            this.ycoord = ycoord;
        
        }
        private bool Active()// Ska ha en Active metod som kollar om spelaren är tillräckligt nära fienden för att den ska aktiveras varje gång spelaren rör sig.
        {
            int sight = 5;
            // if sats för att kolla om Character är inom 5 rutor av Enemy. Enemy blir då Active.
            if (Character.Position.X < Position.X - sight && Character.Position.X > Position.X + sight && Character.Position.Y < Position.Y + sight && Character.Position.Y > Position.Y - sight)
            { 
                return true;     
            }
            // kolla om spelaren är tillräckligt nära för att fienden ska vara Active, 4*4 ruta runt spelaren.
            // Detta gör den varje gång spelaren rör sig.   
        }
        public void Update()
        {
            while (Active) // Fienden är aktiv och går mot spelaren samt attackerar om den kan.
            {   // Enemy går mot spelaren om den är tillräckligt när för att attackera så gör den det istället.
                if ()//ska kolla om det är kortast till spelaren från Enemy på x kordinaten eller om avståndet är lika.        
                {
                    if (Character.Position.X - Position.X < 0) // kollar om Enemy är till höger om spelaren.
                    {
                        if (Character.Position.X +1 == Position.X) //kollar om Enemy är på rutan till höger om spelaren.
                        {
                                // Attackera.
                        }
                        // kod för att gå till vänster.
                
                    }
                    else // Annars gå till höger.
                    { 
                        if (Character.Position.X -1 == Position.X) //kollar om Enemy är på rutan till vänster om spelaren.
                        {
                            // Attackera.
                        }
                        // Kod för att gå till höger
               
                    }
                }
                else // Om Y avståndet till spelaren var det kortaste.
                {
                    if (Character.Position.Y - Position.Y < 0) // Kollar om Enemy är ovanför spelaren.
                    {
                        if (Character.Position.Y -1 == Position.Y) //kollar om Enemy är på rutan ovnaför spelaren.
                        {
                            // Attackera.
                        }
                        // kod för att gå neråt.
                    }
                    else // annars gå uppåt.
                    {
                        if (Character.Position.Y -1 == Position.Y) //kollar om Enemy är på rutan ovnaför spelaren.
                        {
                            // Attackera.
                        }
                        //  kod för att gå uppåt.
                    }
                }
            }
        }
    }
}