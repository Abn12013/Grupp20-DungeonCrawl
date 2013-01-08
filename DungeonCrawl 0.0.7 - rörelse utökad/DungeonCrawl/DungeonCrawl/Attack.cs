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

namespace DungeonCrawl
{
    class Attack : MovingGameObj
    {
        //Ansvarig: Adam Eriksson

        //Denna klass hanterar uträkning och utritning av spelarens attacker


        public int str;//hämtas från character
        public int dex;//hämtas från character
        int damage;
        int moved = 0;
        Random random = new Random();

        public int randomnr;

        public Attack()
        {

        }

        public Attack(int str, int dex)
        {
            Frame = 0;
        }

        public Vector2 attackPos
        { set; get; }

        private bool resetAttack = false;

        public int CharAttackCalc(int str, int dex) //Metod för karaktärens attack.  
        {
            resetAttack = true;

            //Måste man hämta arrayen för att veta vilken fiende som träffas, hämtas från Enemyklassen.
            //Ju mer str desto större chans att träffa
            if (random.NextDouble() > 1 / (1 + ((double)str / (double)dex) * 3)) //träff  //Måste hämta dex och str från Characterklassen.
            {
                randomnr = random.Next(1, 64);  //Randomnummer för var texten för hur mycket skada man gör ritas ut
                damage = str - random.Next(0, str / 2); //Skadan är spelarens str - ett random tal mellan str/2 och 0
            }
                else // miss
                {
                    randomnr = random.Next(1, 64);
                    damage = 0;
                }
           
            return damage;  //Retunerar skadans 
        }

        public void Update(GameTime gameTime, ref bool canAttack, ref bool attackDone2)
        {
            //Bestämmer vlken bild som skall visas i attackanimationen
            moved += 2;
            if (moved > 1 && moved < 16)
            { Frame = 0; }
            else if (moved > 16 && moved < 32)
            { Frame = 1; }
            else if (moved > 32 && moved < 48)
            { Frame = 2; }
            else if (moved > 48 && moved < 64)
            { Frame = 3; }

            if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moved = 0; Frame = 0; resetAttack = false;  }   //olika variabler ändras så att man nu kan genomföra en ny rörelse

            if (resetAttack == false)  //gör att man kan utföra en ny attack
            {
                canAttack = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        {
            Rectangle tmp2 = new Rectangle((Frame % 4) * 64, (Frame / 4) * 64, 64, 64);
            spriteBatch.Draw(Gfx, Position - DrawOffset + new Vector2(400, 350), tmp2, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, layer);
        }
    }
}
