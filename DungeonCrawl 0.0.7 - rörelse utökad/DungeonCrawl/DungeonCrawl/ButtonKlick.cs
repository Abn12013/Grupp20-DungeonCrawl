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
        //klass för knapptryckningar och musklick som hanterar rörese och attack. Knapparna som används i denna klass är " WASD".

        protected KeyboardState prevKs;
        protected MouseState prevMs1;
        
        public ButtonKlick()
        {
           
        }

        public void Update(GameTime gameTime, ref Character player, ref LoadSave saveAndLoadGame, ref int floor, ref  List<Enemy> enemies, ref PositionManager[, ,] positionManager, ref float hpBarBredd
            , ref bool playmenumusic, ref bool playingamemusic, Cue IngameTGU, ref LevelManager levelManager, ref bool attackDone, ref bool attackDone2
            , ref bool attackDoneMiss, SoundBank soundBank, Cue attackHit, Cue attackMiss, ref Attack attack2, ref int playerDamgeDelt
            , ref Rectangle hpBarPos, List<GameObj> objects, Texture2D tileset )
        {
            
            KeyboardState ks = Keyboard.GetState();

            //Hp och hpbar uträkningar test
            Random tal = new Random();
            
            //När en attack är klar sätts dessa till true
            if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
            {
                attackDone2 = true;
                attackDoneMiss = true;
            }

            //Rörelse via tangentbordskontroller
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D)) //Knapptryckning för att röra sig till höger
            {
                if (player.moveCharRight == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 6;   //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen

                    if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "enemy")
                    {
                        if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                        {
                            for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                            {
                                if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                {
                                    
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                    attack2.attackPos = new Vector2(player.playerPosX + 1, player.playerPosY);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                    
                                    int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                    int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                    playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                    
                                    player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                    if (hpBefore == hpAfter)   //Då har attacken missat
                                    {
                                        attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                    }
                                    else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                        soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                    }
                                    
                                    if (enemies[i].hp <= 0)   //Ifall fienden dör
                                    {
                                        
                                        positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";   //Sätter positionen den var på till empty

                                        player.Xp += enemies[i].ReturnExp();
                                        enemies.RemoveAt(i);  //Tar bort fienden

                                        if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                        {
                                            player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar


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
                    
                }
            }
            if (ks.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A)) //Knapptryckning för att röra sig till vänster
            {
                if (player.moveCharLeft == false && player.allowButtonPress == true)  //Gör så att man enbart kan genomföra en ny rörelse om karaktären för tillfället inte rör sig åt något håll
                {
                    player.Frame = 3;  //sätter framen till det håll man försöker gå åt, ifall det är en vägg ivägen körs inte animationen men karaktären vänder sig mot väggen                    
                    if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type == "enemy")
                    {
                        if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                        {
                            for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                            {
                                if (enemies[i].xCoord == player.playerPosX - 1 && enemies[i].yCoord == player.playerPosY)
                                {
                                    
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX - 1, player.playerPosY);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                    
                                    int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                    int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                    playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                    
                                    player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                    if (hpBefore == hpAfter)   //Då har attacken missat
                                    {
                                        attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                    }
                                    else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                        soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                    }


                                    
                                    if (enemies[i].hp <= 0)   //Ifall fienden dör
                                    {
                                        
                                        positionManager[player.playerPosY, player.playerPosX - 1, floor].type = "empty";
                                        player.Xp += enemies[i].ReturnExp();
                                        enemies.RemoveAt(i);  //Tar bort fienden
                                        if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                        {
                                            player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                        if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                        {
                            for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                            {
                                if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY - 1)
                                {
                                    
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY - 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                    
                                    int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                    int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                    playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                    
                                    player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                    if (hpBefore == hpAfter)   //Då har attacken missat
                                    {
                                        attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                    }
                                    else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                        soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                    }
                                    
                                    if (enemies[i].hp <= 0)   //Ifall fienden dör
                                    {
                                        positionManager[player.playerPosY - 1, player.playerPosX, floor].type = "empty";
                                        player.Xp += enemies[i].ReturnExp();
                                        enemies.RemoveAt(i);  //Tar bort fienden
                                        if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                        {
                                            player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                        if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                        {
                            for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                            {
                                if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY + 1)
                                {
                                    
                                    attackDone = false;
                                    player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                    attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY + 1);
                                    attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                    
                                    int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                    enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                    int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                    playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                    
                                    player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                    if (hpBefore == hpAfter)   //Då har attacken missat
                                    {
                                        attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                        attackDone2 = true;
                                        soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                    }
                                    else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                    {
                                        attackDoneMiss = true;
                                        attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                        soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                    }
                                    
                                    if (enemies[i].hp <= 0)   //Ifall fienden dör
                                    {
                                      
                                        positionManager[player.playerPosY + 1, player.playerPosX, floor].type = "empty";
                                        player.Xp += enemies[i].ReturnExp();
                                        enemies.RemoveAt(i);  //Tar bort fienden
                                        if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                        {
                                            player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                            if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                            {
                                for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                                {
                                    if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY - 1)
                                    {
                                        
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                        attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY - 1);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                        
                                        int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                        int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                        playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                        
                                        player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                        if (hpBefore == hpAfter)   //Då har attacken missat
                                        {
                                            attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                        }
                                        else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                            soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                        }
                                        
                                        if (enemies[i].hp <= 0)   //Ifall fienden dör
                                        {
                                            
                                            positionManager[player.playerPosY - 1, player.playerPosX, floor].type = "empty";
                                            player.Xp += enemies[i].ReturnExp();
                                            enemies.RemoveAt(i);  //Tar bort fienden
                                            if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                            {
                                                player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                            if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                            {
                                for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                                {
                                    if (enemies[i].xCoord == player.playerPosX && enemies[i].yCoord == player.playerPosY + 1)
                                    {
                                        
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack
                                        attack2.attackPos = new Vector2(player.playerPosX, player.playerPosY + 1);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                        
                                        int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                        int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                        playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                        
                                        player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                        if (hpBefore == hpAfter)   //Då har attacken missat
                                        {
                                            attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                        }
                                        else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                            soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                        }
                                        
                                        if (enemies[i].hp <= 0)   //Ifall fienden dör
                                        {
                                            
                                            positionManager[player.playerPosY + 1, player.playerPosX, floor].type = "empty";
                                            player.Xp += enemies[i].ReturnExp();
                                            enemies.RemoveAt(i);  //Tar bort fienden
                                            if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                            {
                                                player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                            if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                            {
                                for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                                {
                                    if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                    {
                                        
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                        
                                        int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                        int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                        playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                        
                                        player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                        if (hpBefore == hpAfter)   //Då har attacken missat
                                        {
                                            attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                        }
                                        else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                            soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                        }
                                        if (enemies[i].hp <= 0)   //Ifall fienden dör
                                        {
                                            
                                            positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";   //Sätter positionen den var på till empty

                                            player.Xp += enemies[i].ReturnExp();
                                            enemies.RemoveAt(i);  //Tar bort fienden
                                            if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                            {
                                                player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar
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
                            if (attackDone == true)    //Gör så att man enbart kan genomföra en atack åt gången
                            {
                                for (int i = 0; i < enemies.Count; i++)   //Kollar igenom alla fiender
                                {
                                    if (enemies[i].xCoord == player.playerPosX + 1 && enemies[i].yCoord == player.playerPosY)
                                    {
                                        
                                        attackDone = false;
                                        player.allowButtonPress = false; //Gör så man ej kan göra någon annan rörelse eller attack medans man genomför nuvarande attack

                                        attack2.attackPos = new Vector2(player.playerPosX + 1, player.playerPosY);
                                        attack2.Position = new Vector2(attack2.attackPos.X * 64, attack2.attackPos.Y * 64);   //Ritar ut attackanimationen i rätt ruta
                                        
                                        int hpBefore = enemies[i].hp;   //Fiendens hp innan utförd attack
                                        enemies[i].hp -= attack2.CharAttackCalc(player.Totstr, enemies[i].dex);   //Kör spelarens attackuträkning på fienden
                                        int hpAfter = enemies[i].hp;   //Fiendens hp efter attack

                                        playerDamgeDelt = hpBefore - hpAfter;   //Skadan som fienden tog
                                        
                                        player.allowButtonPress = true;   //Gör att man får trycka på en knapp igen
                                        if (hpBefore == hpAfter)   //Då har attacken missat
                                        {
                                            attackDoneMiss = false;   //Gör att texten för miss ritas ut
                                            attackDone2 = true;
                                            soundBank.PlayCue("AttackMiss");   //Spelar upp ljud för miss
                                        }
                                        else if (hpBefore != hpAfter)   //Ifall attacken träffar
                                        {
                                            attackDoneMiss = true;
                                            attackDone2 = false;   //gör så att attackanimationen får ritas ut
                                            soundBank.PlayCue("AttackSound");   //Spelar upp ljud för träff
                                        }
                                        
                                        if (enemies[i].hp <= 0)   //Ifall fienden dör
                                        {
                                            
                                            positionManager[player.playerPosY, player.playerPosX + 1, floor].type = "empty";   //Sätter positionen den var på till empty

                                            player.Xp += enemies[i].ReturnExp();
                                            enemies.RemoveAt(i);  //Tar bort fienden
                                            if (player.Xp >= player.XpToLevel)   //Gör så att man går upp i level
                                            {
                                                player.LevelUp(ref hpBarPos.Width);   //Återställer spelarens hpbar


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
                      
                    }
                }
            }

            prevMs1 = mousestate1;



            base.Update(gameTime);
            
        }



    }
}
