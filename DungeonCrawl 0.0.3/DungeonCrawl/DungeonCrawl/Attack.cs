using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl
{
    class Attack
    {
        Character character=new Character();
        int str;
        int dex;
        int damage;
        Random random = new Random();
        public Attack()
        {

        }

        public Attack(int str, int dex)
        {
            this.str = str;
            this.dex = dex;

        }
        public int CharAttack(int x,int y,int str, int dex, float angle) //Metod för karaktärens attack
        {
            
            damage=str - random.Next(0,str/2);

            if (str + random.Next(0,20) > enemy.dex)
            {
                
            }

                return damage;
        }
    }
}
