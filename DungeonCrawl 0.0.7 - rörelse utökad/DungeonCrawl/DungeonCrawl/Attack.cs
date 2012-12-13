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

        Enemy enemy = new Enemy();
        Character character = new Character();
        public int str;//hämtas från character
        public int dex;//hämtas från character
        int damage;
        float x;
        float y;
        int moved = 0;
        Random random = new Random();
        SpriteBatch spriteBatch;
        //hämta int array för fiender

        public Attack()
        {

        }

        public Attack(int str, int dex)
        {
            this.str = character.Totstr;
            this.dex = character.Totdex;
            Frame = 0;

        }

        public Vector2 attackPos
        { set; get; }

        private bool resetAttack = false;

        public int CharAttackCalc(int str, int dex)                                     //Metod för karaktärens attack.  
        {
            resetAttack = true;

                                                                                       //Måste man hämta arrayen för att veta vilken fiende som träffas, hämtas från Enemyklassen. 
            if (str + random.Next(0, 20) > enemy.dex) //träff                         //Måste hämta dex och str från Characterklassen.
            {
                damage = str - random.Next(0, str / 2);

            }
            else//miss
            {
                damage = 0;
            }
            //float Xpos = Position.X;    //Karaktärens nuvarande position på x-leden
            //moved += movespeed; //plussas på med 4 för vara tick av gametime.

           
            ////animerar karaktären
            //if (moved > 1 && moved < 16)
            //{ Frame = 0; }
            //else if (moved > 16 && moved < 32)
            //{ Frame = 1; }
            //else if (moved > 32 && moved < 48)
            //{ Frame = 2; }
            //else if (moved > 48 && moved < 64)
            //{ Frame = 3; }

            //if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            //{ moved = 0; Frame = 0; canAttack = true; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse

            //if (resetAttack == false)
            //{
            //    canAttack = true;
            //}
            return damage;



        }



        //public void AttackAnim(int AttackPos)  //Medtod om man rör sig till left
        //{

        //}
        public void Update(GameTime gameTime, ref bool canAttack)
        {
            

            moved += 4;
            if (moved > 1 && moved < 16)
            { Frame = 0; }
            else if (moved > 16 && moved < 32)
            { Frame = 1; }
            else if (moved > 32 && moved < 48)
            { Frame = 2; }
            else if (moved > 48 && moved < 64)
            { Frame = 3; }

            if (moved == 64)    // när man rört sig 64 pixlar så stannar gubben
            { moved = 0; Frame = 0; resetAttack = false; }   //olika variabler ändras så att man nu kan genomföra en ny rörelse

            if (resetAttack == false)
            {
                canAttack = true;
            }

            base.Update(gameTime);
        }


        public int Frame
        { set; get; }


        public override void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer)
        {
            Rectangle tmp2 = new Rectangle((Frame % 4) * 64, (Frame / 4) * 64, 64, 64);
            spriteBatch.Draw(Gfx, Position - DrawOffset + new Vector2(400, 350), tmp2, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, layer);
        }
    }
}
