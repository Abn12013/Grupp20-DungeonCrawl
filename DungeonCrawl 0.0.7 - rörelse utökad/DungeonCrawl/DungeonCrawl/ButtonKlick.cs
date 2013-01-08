using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DungeonCrawl
{
    class ButtonKlick: MovingGameObj
    {
        protected KeyboardState prevKs;
        protected MouseState prevMs1;
         enum GameState { NewGame, MainMenu, LoadGame, ChangeLevel, Game, GameOver, Victory, Pause, Information }
        
        public ButtonKlick()
        {
           
        }

        public void Update(GameTime gameTime, ref Character player, ref LoadSave saveAndLoadGame, ref int floor, ref  List<Enemy> enemies, ref PositionManager[, ,] positionManager, ref float hpBarBredd
            , ref bool playmenumusic, ref bool playingamemusic, Cue IngameTGU, ref LevelManager levelManager, ref bool attackDone, ref bool attackDone2
            , ref bool attackDoneMiss, SoundBank soundBank, Cue attackHit, Cue attackMiss, ref Attack attack2, ref int playerDamgeDelt
            , ref Rectangle hpBarPos, List<GameObj> objects, Texture2D tileset )
        {
            
            KeyboardState ks = Keyboard.GetState();
            KeyboardState prks;

            //Hp och hpbar uträkningar test
            Random tal = new Random();
            
            

            if (attackDone == true)
            {
                attackDone2 = true;
                attackDoneMiss = true;
            }

            // TODO: Add your update logic here

            //MessageBox.Show(positionManager[6,5,0].type);

            //positionManager[player.playerPosY, player.playerPosX, 0].type = "player"; //Tilldelar spelarens nuvarande positon i rutnätet.
            //Varablerna tilldelas högs upp i Game1.cs, men borde läggas in som en variabel i Character klassen.
            //Rörelse via tangentbordskontroller
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) //Knapptryckning för att röra sig till höger
            {
                if (player.moveCharRight == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 6;   //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                    if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "enemy")
                    {
                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                {
                                    int tempEnemyHp = enemies[i].hp;
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                    attack2.attackPos = new Vector2(player.playerPosX + 1, player.playerPosY);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                    //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpBefore = enemies[i].hp;
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpAfter = enemies[i].hp;

                                    playerDamgeDelt = hpBefore - hpAfter;
                                    //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                    player.allowButtonPress = true;
                                    if (hpBefore == hpAfter)
                                    {
                                        attackDoneMiss = false;
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");
                                    }
                                    else if (hpBefore != hpAfter)
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;
                                        soundBank.PlayCue("AttackSound");
                                    }
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";

                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp(ref hpBarPos.Width);


                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }
                    }

                    else if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharRight = true;    //Gör så att man rör sig åt höger
                        player.allowButtonPress = false;    //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosX += 1;

                    }
                    //if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "empty")
                    //MessageBox.Show(positionManager[player.playerPosY, player.playerPosX + 1, floor].type);

                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)) //Knapptryckning för att röra sig till vänster
            {
                if (player.moveCharLeft == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 3;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen                    
                    if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type == "enemy")
                    {
                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX - 1 && enemies[i].yCoord == player.playerPosY)
                                {
                                    int tempEnemyHp = enemies[i].hp;
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX - 1, player.playerPosY);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                    //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpBefore = enemies[i].hp;
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpAfter = enemies[i].hp;

                                    playerDamgeDelt = hpBefore - hpAfter;
                                    //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                    player.allowButtonPress = true;
                                    if (hpBefore == hpAfter)
                                    {
                                        attackDoneMiss = false;
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");
                                    }
                                    else if (hpBefore != hpAfter)
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;
                                        soundBank.PlayCue("AttackSound");
                                    }


                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY, player.playerPosX - 1, floor].type = "empty";
                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp(ref hpBarPos.Width);
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }
                    }

                    else if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharLeft = true;    //Gör så att man rör sig åt vänster
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY, player.playerPosX - 1, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosX -= 1;
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W)) //Knapptryckning för att röra sig upp
            {
                if (player.moveCharUp == false && player.allowButtonPress == true)    //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 9;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                    if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type == "enemy")
                    {
                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY - 1)
                                {
                                    int tempEnemyHp = enemies[i].hp;
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY - 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                    //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpBefore = enemies[i].hp;
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpAfter = enemies[i].hp;

                                    playerDamgeDelt = hpBefore - hpAfter;
                                    //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                    player.allowButtonPress = true;
                                    if (hpBefore == hpAfter)
                                    {
                                        attackDoneMiss = false;
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");
                                    }
                                    else if (hpBefore != hpAfter)
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;
                                        soundBank.PlayCue("AttackSound");
                                    }
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        positionManager[player.playerPosY - 1, player.playerPosX, floor].type = "empty";
                                        player.Xp += enemies[i].ReturnExp();
                                        enemies.RemoveAt(i);
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp(ref hpBarPos.Width);
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }
                    }

                    else if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharUp = true;  //Gör så att man rör sig upp
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY - 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosY -= 1;
                    }
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))  //Knapptryckning för att röra sig ner
            {
                if (player.moveCharDown == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 0; //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                    if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type == "enemy")
                    {
                        if (attackDone == true)
                        {
                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY + 1)
                                {
                                    int tempEnemyHp = enemies[i].hp;
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY + 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                    //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpBefore = enemies[i].hp;
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                    int hpAfter = enemies[i].hp;

                                    playerDamgeDelt = hpBefore - hpAfter;
                                    //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                    player.allowButtonPress = true;
                                    if (hpBefore == hpAfter)
                                    {
                                        attackDoneMiss = false;
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");
                                    }
                                    else if (hpBefore != hpAfter)
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;
                                        soundBank.PlayCue("AttackSound");
                                    }
                                    //MessageBox.Show(enemies[i].hp.ToString());
                                    if (enemies[i].hp <= 0)
                                    {
                                        enemies.RemoveAt(i);
                                        positionManager[player.playerPosY + 1, player.playerPosX, floor].type = "empty";
                                        player.Xp += 60;
                                        if (player.Xp >= player.XpToLevel)
                                        {
                                            player.LevelUp(ref hpBarPos.Width);
                                        }
                                    }
                                }
                                else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                {

                                }
                            }
                        }
                    }

                    else if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                    {
                        player.moveCharDown = true;    //Gör så att man rör sig ner
                        //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                        //positionManager[player.playerPosY + 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                        player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        player.playerPosY += 1;
                    }

                }
            }
            prevKs = ks;



            //Rörelse via musklick
            MouseState mousestate1 = Mouse.GetState();
            var mouseposition = new Point(mousestate1.X, mousestate1.Y);
            Rectangle moveUpBox = new Rectangle(400 - 28, 294 - 28, 56, 56);
            Rectangle moveDownBox = new Rectangle(400 - 28, 406 - 28, 56, 56);
            Rectangle moveLeftBox = new Rectangle(344 - 28, 350 - 28, 56, 56);
            Rectangle moveRightBox = new Rectangle(456 - 28, 350 - 28, 56, 56);

            if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (moveUpBox.Contains(mouseposition))
                {
                    if (player.moveCharUp == false && player.allowButtonPress == true)    //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                    {
                        player.Frame = 9;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                        if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type == "enemy")
                        {
                            if (attackDone == true)
                            {
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY - 1)
                                    {
                                        int tempEnemyHp = enemies[i].hp;
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                        attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY - 1);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                        //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpBefore = enemies[i].hp;
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpAfter = enemies[i].hp;

                                        playerDamgeDelt = hpBefore - hpAfter;
                                        //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                        player.allowButtonPress = true;
                                        if (hpBefore == hpAfter)
                                        {
                                            attackDoneMiss = false;
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");
                                        }
                                        else if (hpBefore != hpAfter)
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;
                                            soundBank.PlayCue("AttackSound");
                                        }
                                        //MessageBox.Show(enemies[i].hp.ToString());
                                        if (enemies[i].hp <= 0)
                                        {
                                            enemies.RemoveAt(i);
                                            positionManager[player.playerPosY - 1, player.playerPosX, floor].type = "empty";
                                            player.Xp += 60;
                                            if (player.Xp >= player.XpToLevel)
                                            {
                                                player.LevelUp(ref hpBarPos.Width);
                                            }
                                        }
                                    }
                                    else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                    {

                                    }
                                }
                            }
                        }

                        else if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                        {
                            player.moveCharUp = true;  //Gör så att man rör sig upp
                            //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                            //positionManager[player.playerPosY - 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                            player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                            player.playerPosY -= 1;
                        }
                    }
                }
            }

            if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (moveDownBox.Contains(mouseposition))
                {
                    if (player.moveCharDown == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                    {
                        player.Frame = 0; //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                        if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type == "enemy")
                        {
                            if (attackDone == true)
                            {
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY + 1)
                                    {
                                        int tempEnemyHp = enemies[i].hp;
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                        attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY + 1);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                        //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpBefore = enemies[i].hp;
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpAfter = enemies[i].hp;

                                        playerDamgeDelt = hpBefore - hpAfter;
                                        //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                        player.allowButtonPress = true;
                                        if (hpBefore == hpAfter)
                                        {
                                            attackDoneMiss = false;
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");
                                        }
                                        else if (hpBefore != hpAfter)
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;
                                            soundBank.PlayCue("AttackSound");
                                        }
                                        //MessageBox.Show(enemies[i].hp.ToString());
                                        if (enemies[i].hp <= 0)
                                        {
                                            enemies.RemoveAt(i);
                                            positionManager[player.playerPosY + 1, player.playerPosX, floor].type = "empty";
                                            player.Xp += 60;
                                            if (player.Xp >= player.XpToLevel)
                                            {
                                                player.LevelUp(ref hpBarPos.Width);
                                            }
                                        }
                                    }
                                    else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                    {

                                    }
                                }
                            }
                        }

                        else if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                        {
                            player.moveCharDown = true;    //Gör så att man rör sig ner
                            //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                            //positionManager[player.playerPosY + 1, player.playerPosX, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                            player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                            player.playerPosY += 1;
                        }

                    }
                }
            }

            if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (moveLeftBox.Contains(mouseposition))
                {
                    if (player.moveCharLeft == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                    {
                        player.Frame = 3;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen
                        if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "enemy")
                        {
                            if (attackDone == true)
                            {
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                    {
                                        int tempEnemyHp = enemies[i].hp;
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                        //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpBefore = enemies[i].hp;
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpAfter = enemies[i].hp;

                                        playerDamgeDelt = hpBefore - hpAfter;
                                        //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                        player.allowButtonPress = true;
                                        if (hpBefore == hpAfter)
                                        {
                                            attackDoneMiss = false;
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");
                                        }
                                        else if (hpBefore != hpAfter)
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;
                                            soundBank.PlayCue("AttackSound");
                                        }
                                        if (enemies[i].hp <= 0)
                                        {
                                            enemies.RemoveAt(i);
                                            positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";

                                            player.Xp += 60;
                                            if (player.Xp >= player.XpToLevel)
                                            {
                                                player.LevelUp(ref hpBarPos.Width);
                                            }
                                        }
                                    }
                                    else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                    {

                                    }
                                }
                            }
                        }
                        else if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                        {
                            player.moveCharLeft = true;    //Gör så att man rör sig åt vänster
                            //positionManager[player.playerPosY, player.playerPosX, 0].type = "empty";   //Sätter sin förra position i 2d-arrayen till "null"
                            //positionManager[player.playerPosY, player.playerPosX -= 1, 0].type = "player";    //Sätter rutan man rörde sig mot till player
                            player.playerPosX -= 1;
                            player.allowButtonPress = false;   //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                        }
                    }
                }
            }

            if (mousestate1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Released) && prevMs1.RightButton == (Microsoft.Xna.Framework.Input.ButtonState.Pressed))
            {
                if (moveRightBox.Contains(mouseposition))
                {
                    if (player.moveCharRight == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                    {
                        player.Frame = 6;   //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                        if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "enemy")
                        {
                            if (attackDone == true)
                            {
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                    {
                                        int tempEnemyHp = enemies[i].hp;
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                        attack2.attackPos = new Vector2(player.playerPosX + 1, player.playerPosY);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);
                                        //playerdmg = attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpBefore = enemies[i].hp;
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);
                                        int hpAfter = enemies[i].hp;

                                        playerDamgeDelt = hpBefore - hpAfter;
                                        //MessageBox.Show(hpBefore.ToString() +" " + hpAfter.ToString()+" " + playerDamgeDelt.ToString());
                                        player.allowButtonPress = true;
                                        if (hpBefore == hpAfter)
                                        {
                                            attackDoneMiss = false;
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");
                                        }
                                        else if (hpBefore != hpAfter)
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;
                                            soundBank.PlayCue("AttackSound");
                                        }
                                        //MessageBox.Show(enemies[i].hp.ToString());
                                        if (enemies[i].hp <= 0)
                                        {
                                            enemies.RemoveAt(i);
                                            positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";

                                            player.Xp += 60;
                                            if (player.Xp >= player.XpToLevel)
                                            {
                                                player.LevelUp(ref hpBarPos.Width);


                                            }
                                        }
                                    }
                                    else if (enemies[i].xCoord != player.playerPosX + 1 || enemies[i].yCoord != player.playerPosY)
                                    {

                                    }
                                }
                            }
                        }

                        else if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "wall")   //Kollar om det är en vägg framför karaktären, om detta är fallet utförs ingen rörelse
                        {
                            player.moveCharRight = true;    //Gör så att man rör sig åt höger
                            player.allowButtonPress = false;    //Gör så att man inte kan trycka på någon annan knapp medans en rörelse genomförs
                            player.playerPosX += 1;

                        }
                        //if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type != "empty")
                        //MessageBox.Show(positionManager[player.playerPosY, player.playerPosX + 1, floor].type);

                    }
                }
            }





            prevMs1 = mousestate1;



            base.Update(gameTime);
            
        }



    }
}
