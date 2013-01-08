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

namespace DungeonCrawl
{
    class FogOfWar
    {
        //Ansvarig: Jonas Holmberg

        //Denna klass enda uppgift är att rita ut spelets "Fog of war" eller med andra ord begränsa spelarens synfält. Vad klassen gör enkelt är att den
        //kollar om spelaren befinner sig nära eller mot en vägg, den ritar sedan ut 64x64 svarta rutor där spelaren inte skall kunna se, ex över en vägg.

        public void Draw(SpriteBatch spriteBatch, Vector2 DrawOffset, float layer, Texture2D visionTileGfx, Character player, PositionManager[, ,] positionManager, int floor)
        {

            //Fog of war
            for (int i = -6; i < 7; i++)//Ritar ut det maximala synfältet
            {

                for (int j = -7; j < 8; j++)
                {
                    if (new Vector2(j, i) == new Vector2(-4, 0))//mitten
                    { j += 9; }
                    else if (new Vector2(j, i) == new Vector2(-3, -1))//mitten -1
                    { j += 7; }
                    else if (new Vector2(j, i) == new Vector2(-3, 1))//mitten +1
                    { j += 7; }
                    else if (new Vector2(j, i) == new Vector2(-2, 2))//mitten +2
                    { j += 5; }
                    else if (new Vector2(j, i) == new Vector2(-2, -2))//mitten -2
                    { j += 5; }
                    else if (new Vector2(j, i) == new Vector2(-1, -3))//mitten -3
                    { j += 3; }
                    else if (new Vector2(j, i) == new Vector2(-1, 3))//mitten +3
                    { j += 3; }
                    else if (new Vector2(j, i) == new Vector2(0, 4))//mitten +4
                    { j += 1; }
                    else if (new Vector2(j, i) == new Vector2(0, -4))//mitten -4
                    { j += 1; }


                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }

            }

            //Ritar ut fog of war om det är väggar över spelaren
            if (positionManager[player.playerPosY - 1, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 2, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 3, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //Slut på utritning av fog of war över spelaren

            //Ritar ut fog of war om det är väggar under spelaren
            if (positionManager[player.playerPosY + 1, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 2, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 3, player.playerPosX, floor].type == "wall")
            {
                for (int j = -7; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //Slut på utritning av fog of war under spelaren

            //Kollar fog of war till vänster om spelaren om det är en vägg där

            if (positionManager[player.playerPosY, player.playerPosX - 1, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY, player.playerPosX - 2, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY, player.playerPosX - 3, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX - 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //Slut på koll till vänster

            //Kollar fog of war till höger om spelaren om det är en vägg där

            if (positionManager[player.playerPosY, player.playerPosX + 1, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 1) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY, player.playerPosX + 2, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 2) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY, player.playerPosX + 3, floor].type == "wall")
            {
                for (int i = -6; i < 7; i++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 3) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 4) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + 5) * 64, (player.playerPosY + i) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //Slut på koll till höger

            //Kollar fog of war till vänster om spelaren och UPP en bit om det är en vägg där

            if (positionManager[player.playerPosY - 1, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 2, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 3, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }

            //Slut på koll till vänster

            //Kollar fog of war till höger om spelaren och UPP en bit om det är en vägg där

            if (positionManager[player.playerPosY - 1, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 2, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY - 3, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY - 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }

            //Slut på koll till 

            //kollar vänster ner
            if (positionManager[player.playerPosY + 1, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 2, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 3, player.playerPosX - 1, floor].type == "wall")
            {
                for (int j = -7; j < -1; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //slut koll

            //kollar höger ner
            if (positionManager[player.playerPosY + 1, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 1) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 2, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 2) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            else if (positionManager[player.playerPosY + 3, player.playerPosX + 1, floor].type == "wall")
            {
                for (int j = 1; j < 8; j++)
                {
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 3) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 4) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                    spriteBatch.Draw(visionTileGfx, new Vector2((player.playerPosX + j) * 64, (player.playerPosY + 5) * 64) - player.Position + new Vector2(400, 350), null, Color.White, 0, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0.89f);
                }
            }
            //slut koll

        }

    }
}
