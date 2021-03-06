﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawl
{
    class LevelManager
    {
        //Ansvarig Antti Lindén
        
        //Denna klass ansvarar för att bygga upp banan och tilldela olika objekt deras positioner i rutntet. 

        private string[] map = new string[3];
        //E = Entry/Exit, # = Vägg, C = Chest, U = Upstairs, D = Downstairs, . = golv


        public LevelManager()
        {
            //Sräng för kartan, vad olika saker skall befinna sig
            map[0] =
        "                                                    " + "\n" +
        "   #################                   ###          " + "\n" +
        " ###....t..........###                ##.##         " + "\n" +
        " #....###########....#      ######### #.C.#         " + "\n" +
        " #....# ######  #....#      ##..#..## ##.## ####### " + "\n" +
        " #.g..###....#  #....#      #.......###...###.....# " + "\n" +
        " #........C..#  #....###### #...C...##.....##..D..# " + "\n" +
        " #....###....####....##...# ##.....##.......#.....# " + "\n" +
        " #....# #...............D.# #.......#.......#.....# " + "\n" +
        " #....# #########....##...# #.......#.......##....# " + "\n" +
        " #....#         #....###### ##.....###.....###....# " + "\n" +
        " #....###########....#      #.......###...###....## " + "\n" +
        " #...................#      #.......# ##.## #....## " + "\n" +
        " ###...............###      ##..#######...####....# " + "\n" +
        "   #...............#  ########..#...##.....###....# " + "\n" +
        "   #...............#  #.....##..#.#.#.......#....## " + "\n" +
        "   #################  #..D..##..#...........#....## " + "\n" +
        "          #############.....##..#...........##....# " + "\n" +
        "          #......######.....##..#.D.#.......##....# " + "\n" +
        "          #.....................#...##.....##....## " + "\n" +
        "    #######......######.....###########...###....## " + "\n" +
        "    #D...........#    #.....#####.....###.####....# " + "\n" +
        "    ###################.....####..###.#.....##....# " + "\n" +
        "           #.....................## #.#.....#....## " + "\n" +
        "           #.##########.....######  #.#.....#....## " + "\n" +
        "           #.#        #.....#      ##.#######.....# " + "\n" +
        "           #.##########.....########..#....##.....# " + "\n" +
        "           #.#.....................#.##.#.........# " + "\n" +
        "           #.##########.....######...##.#....##...# " + "\n" +
        "           #..........#################.########### " + "\n" +
        "           ###........########...........#          " + "\n" +
        "             #...........................#          " + "\n" +
        "             #############################          " + "\n" +
        "                                                    ";

            map[1] =
            "                                                    " + "\n" +
            "         ########################################## " + "\n" +
            "   ###   #.....#.....##....#......................# " + "\n" +
            "   #.#####.#.#.#.#.........#........#############.# " + "\n" +
            "   #.......#.#.#.#.........#........############### " + "\n" +
            "   #######.#.#.#.#...##....#........#........#...#  " + "\n" +
            "  ########.#.#.#.#...#######........#..........U.#  " + "\n" +
            "  #........#.#...#...##...##........#........#...#  " + "\n" +
            "  #.########.#.###.g....U.######..####.############ " + "\n" +
            "  #.# #......#.#.#...##...#.......................# " + "\n" +
            "  #.# #.######.#.##########.......................# " + "\n" +
            "  #.# #.#    #...#..........########.....######.### " + "\n" +
            "  #.# #.##########...........#.#...#.d.d.#...#....# " + "\n" +
            "  #####.#C...###############.#.#.C.#.....#...####.# " + "\n" +
            "    #...#....##............#.#.#.............#..#.# " + "\n" +
            "    #.#####.###............#.#.#...#.d.d.#...#..#.# " + "\n" +
            "    #.# #................U.#.#.#...#.....#...#....# " + "\n" +
            "    #.###.#####............#.#.#####.....#####..#.# " + "\n" +
            "    #.....#   #............#.#....U#.....#......### " + "\n" +
            "    ########################.#.######...#########.# " + "\n" +
            "    ##############........#..#..#..#.....#..#.....# " + "\n" +
            "   ##U..G....G..##.######.#####.#..........C###.### " + "\n" +
            "   #.............#.#....#.......#..#.....#..#.....# " + "\n" +
            "   #...#######...#.#....#############...#####.#.#.# " + "\n" +
            "   #..##     ##..#...##.#.......#..#.....#..#.....# " + "\n" +
            "   #.G#       #G.#...##.#.#.#.#.#...........#.#.#.# " + "\n" +
            "   #..#       #..#...##.......###..#.....#..#.....# " + "\n" +
            "   #..##     ##..#.#.##.......#######...#####.#.#.# " + "\n" +
            "   #.G.#######G..#.#....#.#.#.#D#..#.....#..#.....# " + "\n" +
            "   #.....G.......#.#....#.......##............#.#.# " + "\n" +
            "   ##.........G.##.############################.### " + "\n" +
            "    ##############..............................#   " + "\n" +
            "                 ################################   " + "\n" +
            "                                                    ";


            map[2] =
            "                                                    " + "\n" +
            "                          ###########               " + "\n" +
            "                          #.........#               " + "\n" +
            "                          #.........#               " + "\n" +
            "                          #.........#               " + "\n" +
            "                          #.........#               " + "\n" +
            "                          #.........#               " + "\n" +
            "                     ######.........######          " + "\n" +
            "                    ##...######.######...##         " + "\n" +
            "                    #.....#...#f#...#.....#         " + "\n" +
            "                    #..d......#.#......d..#         " + "\n" +
            "                    #.....#...#.#...#.....#         " + "\n" +
            "                    ##...###.##.##.###...##         " + "\n" +
            "                     ##.##...........##.##          " + "\n" +
            "                     #...#...........#...#          " + "\n" +
            "                     #...................#          " + "\n" +
            "                     #...#...........#...#          " + "\n" +
            "                     #####...........#####          " + "\n" +
            "                     #...#...........#...#          " + "\n" +
            "                     #...................#          " + "\n" +
            "                     #...#...........#...#          " + "\n" +
            "                     ##.##...........##.##          " + "\n" +
            "                    ##...###.##.##.###...##         " + "\n" +
            "                    #.....#...#.#...#.....#         " + "\n" +
            "                    #..d......#.#......d..#         " + "\n" +
            "                    #.....#...#.#...#.....#         " + "\n" +
            "                    ##...######.######...##         " + "\n" +
            "                     #####   #...#   #####          " + "\n" +
            "                             #.U.#                  " + "\n" +
            "                             #...#                  " + "\n" +
            "                             #####                  " + "\n" +
            "                                                    " + "\n" +
            "                                                    " + "\n" +
            "                                                    ";
        }

        //Läser in båda strängar till positionmanager
        public void BuildGame(ref PositionManager[, ,] positionManager) //Metod som tilldelar rätt typ av objekt till poitionmanger
        {
            string level;

            for (int floor = 0; floor < 3; floor++)
            {
                level = map[floor];
                Vector2 temp = new Vector2(0, 0); //position du läser nuvarande tecken till
                for (int i = 0; i < level.Length; i++)
                {//for loopen kommer fortsätta tills i är lika stor som längden på strängen level, och kommer köra varje tecken i ordning genom en switch
                    switch (level[i])
                    {
                        case ' ':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager//Sätter dessa värden i nuvarande koordinater i positionmanager
                            {
                                type = "empty",
                                floor = false
                            };
                            temp.X++; //flyttar fram ett steg på x koordinaten till nästa tecken
                            break;
                        case 't':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "door",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case '#':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "wall",
                                floor = false
                            };
                            temp.X++;
                            break;
                        case 'g':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "enemy",
                                monster = "goblin",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'G':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "enemy",
                                monster = "g_goblin",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'd':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "enemy",
                                monster = "dark_elf",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'f':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "enemy",
                                monster = "fel_orc",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case '.':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "empty",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'C':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "chest",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'E':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "wall",
                                entry = true
                            };
                            temp.X++;
                            break;
                        case 'D':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "downstairs",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'U':
                            positionManager[(int)temp.Y, (int)temp.X, floor] = new PositionManager
                            {
                                type = "upstairs",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case '\n'://när radbyte läses så hoppar man ett steg ner i Y axeln och flyttar X till 0 för att läsa nästa rad till rätt positioner
                            temp.Y++;
                            temp.X = 0;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void OpenChest(int floor, PositionManager[, ,] positionManager, ref List<GameObj> objects)   //Metod som körs när man öppnar en kista, den uppdaterar objects för att bytta grafik för kistor
        {
            objects.Clear();
            string currentObject;
            Vector2 currentPosition;

            for (int y = 0; y < 34; y++)
            {
                for (int x = 0; x < 52; x++)
                {
                    currentObject = positionManager[y, x, floor].type;
                    currentPosition = new Vector2(x, y);

                    switch (currentObject)
                    {
                        case "door":
                            objects.Add(new GameObj()
                            {
                                Frame = 23,
                                Position = new Vector2(currentPosition.X * 64, (currentPosition.Y * 64))
                            });
                            break;
                        case "chest":
                            objects.Add(new GameObj()
                            {
                                Frame = 18,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            positionManager[y, x, floor].iteration = objects.Count();
                            positionManager[y, x, floor].hp = 1;
                            break;
                        case "emptychest":
                            objects.Add(new GameObj()
                            {
                                Frame = 19,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            positionManager[y, x, floor].iteration = objects.Count();
                            positionManager[y, x, floor].hp = 1;
                            break;
                        case "upstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 21,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            break;
                        case "downstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 22,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            break;
                    }

                }
            }

        }

        // Medotd som körs när man startar ett spel eller om man byter våning.
        // Vad den gör är att den lägger till rätt objekt på rätt ställe, den kollar även vilken textur som ska användas
        public void ChangeFloor(int floor, PositionManager[, ,] positionManager, ref List<GameObj> floortiles,
            ref List<GameObj> walls, ref List<GameObj> objects, ref List<Enemy> enemies)
        {
            floortiles.Clear();
            walls.Clear();
            objects.Clear();
            enemies.Clear();
            

            string currentMONSTER;
            string currentObject;
            Vector2 currentPosition;

            for (int y = 0; y < 34; y++)
            {
                for (int x = 0; x < 52; x++)
                {
                    currentObject = positionManager[y, x, floor].type;
                    currentMONSTER = positionManager[y, x, floor].monster;
                    currentPosition = new Vector2(x, y);

                    if (positionManager[y, x, floor].floor == true)
                    {
                        floortiles.Add(new GameObj()
                        {
                            Frame = 17,
                            Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                        });
                    }

                   

                    switch (currentObject)
                    {
                        case "empty":
                            break;
                        case "wall":
                            

                          if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 0,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                           

                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 1,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 2,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 3,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 4,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 5,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 6,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 7,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type != "wall" &&
                                positionManager[y - 1, x - 1, floor].type == "wall" && positionManager[y - 1, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 8,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 9,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 10,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type != "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 11,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 12,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type == "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 13,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type != "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 14,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type == "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 15,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                            else if (positionManager[y + 1, x, floor].type == "wall" && positionManager[y - 1, x, floor].type != "wall" &&
                                positionManager[y, x + 1, floor].type != "wall" && positionManager[y, x - 1, floor].type == "wall")
                                walls.Add(new GameObj()
                                {
                                    Frame = 16,
                                    Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                                });
                               
                            break;
                        case "door":
                            objects.Add(new GameObj()
                            {
                                Frame = 23,
                                Position = new Vector2(currentPosition.X * 64, (currentPosition.Y * 64) )
                            });
                            break;
                        case "chest":
                            objects.Add(new GameObj()
                            {
                                Frame = 18,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            positionManager[y, x, floor].iteration = objects.Count();
                            positionManager[y, x, floor].hp = 1;
                            break;
                        case "emptychest":
                            objects.Add(new GameObj()
                            {
                                Frame = 19,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            positionManager[y, x, floor].iteration = objects.Count();
                            positionManager[y, x, floor].hp = 1;
                            break;
                        case "upstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 21,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            break;
                        case "downstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 22,
                                Position = new Vector2(currentPosition.X * 64, currentPosition.Y * 64)
                            });
                            break;
                        case "enemy":
                            switch (currentMONSTER)
                            {
                                case "goblin":
                                    enemies.Add(new Enemy(20, 8, 12, 2, 60)
                                    {
                                        xCoord = (int)currentPosition.X,
                                        yCoord = (int)currentPosition.Y
                                    });
                                    break;
                                case "dark_elf":
                                    enemies.Add(new Enemy(14, 8, 18, 2, 80)
                                    {
                                        xCoord = (int)currentPosition.X,
                                        yCoord = (int)currentPosition.Y
                                    });
                                    break;
                                case "g_goblin":
                                    enemies.Add(new Enemy(4, 3, 12, 8, 30)
                                    {
                                        xCoord = (int)currentPosition.X,
                                        yCoord = (int)currentPosition.Y
                                    });
                                    break;
                                case "fel_orc":
                                    enemies.Add(new Enemy(200, 20, 4, 1, 200)
                                    {
                                        xCoord = (int)currentPosition.X,
                                        yCoord = (int)currentPosition.Y
                                    });
                                    break;
                                default:
                                    enemies.Add(new Enemy(20, 8, 12, 2, 60)
                                    {
                                        xCoord = (int)currentPosition.X,
                                        yCoord = (int)currentPosition.Y
                                    });
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}

