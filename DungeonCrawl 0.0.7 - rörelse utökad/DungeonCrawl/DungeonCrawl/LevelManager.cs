using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawl
{
    class LevelManager
    {
        //E = Entry/Exit, # = Vägg, C = Chest, U = Upstairs, D = Downstairs, . = golv
        public string one =
        "                                                    " + "\n" +
        "   #####E###########                   ###          " + "\n" +
        " ###...............###                ##.##         " + "\n" +
        " #....###########....#      ######### #.C.#         " + "\n" +
        " #....# ######  #....#      ##..#..## ##.## ####### " + "\n" +
        " #....###....#  #....#      #.......###...###.....# " + "\n" +
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

        public string two =
        "                                                    " + "\n" +
        "         ########################################## " + "\n" +
        "   ###   #.....#.....##....#......................# " + "\n" +
        "   #.#####.#.#.#.#.........#........#############.# " + "\n" +
        "   #.......#.#.#.#.........#........############### " + "\n" +
        "   #######.#.#.#.#...##....#........#........#...#  " + "\n" +
        "  ########.#.#.#.#...#######........#..........C.#  " + "\n" +
        "  #........#.#...#...##...##........#........#...#  " + "\n" +
        "  #.########.#.###......U.######..####.############ " + "\n" +
        "  #.# #......#.#.#...##...#.......................# " + "\n" +
        "  #.# #.######.#.##########.......................# " + "\n" +
        "  #.# #.#    #...#..........########.....######.### " + "\n" +
        "  #.# #.##########...........#.#...#.....#...#....# " + "\n" +
        "  #####.#C...###############.#.#.C.#.....#...####.# " + "\n" +
        "    #...#....##............#.#.#.............#..#.# " + "\n" +
        "    #.#####.###............#.#.#...#.....#...#..#.# " + "\n" +
        "    #.# #................U.#.#.#...#.....#...#....# " + "\n" +
        "    #.###.#####............#.#.#####.....#####..#.# " + "\n" +
        "    #.....#   #............#.#....U#.....#......### " + "\n" +
        "    ########################.#.######...#########.# " + "\n" +
        "    ##############........#..#..#..#.....#..#.....# " + "\n" +
        "   ##U..........##.######.#####.#..........C###.### " + "\n" +
        "   #.............#.#....#.......#..#.....#..#.....# " + "\n" +
        "   #...#######...#.#....#############...#####.#.#.# " + "\n" +
        "   #..##     ##..#...##.#.......#..#.....#..#.....# " + "\n" +
        "   #..#       #..#...##.#.#.#.#.#...........#.#.#.# " + "\n" +
        "   #..#       #..#...##.......###..#.....#..#.....# " + "\n" +
        "   #..##     ##..#.#.##.......#######...#####.#.#.# " + "\n" +
        "   #...#######...#.#....#.#.#.#D#..#.....#..#.....# " + "\n" +
        "   #.............#.#....#.......##............#.#.# " + "\n" +
        "   ##...........##.############################.### " + "\n" +
        "    ##############..............................#   " + "\n" +
        "                 ################################   " + "\n" +
        "                                                    ";


        //Läser in båda strängar till positionmanager
        public void BuildGame(ref PositionManager[, ,] positionManager)
        {
            string level = one;
            int floor = 0;

            for (int e = 0; e < 2; e++)//loopar koden för att läsa in en våning två gånger. Kan utökas enkelt.
            {
                Vector2 temp = new Vector2(0, 0); //position du läser nuvarande tecken till
                for (int i = 0; i < level.Length; i++)
                {//for loopen kommer fortsätta tills i är lika stor som längden på strängen level, och kommer köra varje tecken i ordning genom en switch
                    switch (level[i])
                    {
                        case ' ':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager//Sätter dessa värden i nuvarande koordinater i positionmanager
                            {
                                type = "empty",
                                floor = false
                            };
                            temp.X++; //flyttar fram ett steg på x koordinaten till nästa tecken
                            break;
                        case '#':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
                            {
                                type = "wall",
                                floor = false
                            };
                            temp.X++;
                            break;
                        case '.':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
                            {
                                type = "empty",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'C':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
                            {
                                type = "chest",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'E':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
                            {
                                type = "wall",
                                entry = true
                            };
                            temp.X++;
                            break;
                        case 'D':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
                            {
                                type = "downstairs",
                                floor = true
                            };
                            temp.X++;
                            break;
                        case 'U':
                            positionManager[(int)temp.X, (int)temp.Y, floor] = new PositionManager
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

                floor++;
                level = two; //ifall fler kartor läggs till så passar det nog att lägga något i stil med en if sats här
            }
        }

        public void ChangeFloor(int floor, ref PositionManager[, ,] positionManager, ref List<GameObj> floortiles,
            ref List<GameObj> walls, ref List<GameObj> objects, ref GameObj entry)
        {
            floortiles.Clear();
            walls.Clear();
            objects.Clear();
            entry.Position = new Vector2(-50, -50);

            string currentObject;
            Vector2 currentPosition;

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    currentObject = positionManager[x, y, floor].type;
                    currentPosition = new Vector2(x, y);

                    if (positionManager[x, y, floor].floor == true)
                    {
                        floortiles.Add(new GameObj()
                            {
                                Frame = 17,
                                Position = currentPosition
                            });
                    }

                    if (positionManager[x, y, floor].entry == true)
                    {
                        entry.Position = currentPosition;
                    }

                    switch (currentObject)
                    {
                        case "empty":
                            break;
                        case "wall":
                            if (positionManager[x - 1, y, floor].type == "wall")
                                    if (positionManager[x + 1, y, floor].type == "wall")
                                            if (positionManager[x, y - 1, floor].type == "wall")
                                                    if (positionManager[x, y + 1, floor].type == "wall")
                                                            walls.Add(new GameObj()
                                                            {
                                                                Frame = 7,
                                                                Position = currentPosition
                                                            });
                                                    else
                                                        walls.Add(new GameObj()
                                                        {
                                                            Frame = 10,
                                                            Position = currentPosition
                                                        });
                                            else if (positionManager[x, y + 1, floor].type == "wall")
                                                    walls.Add(new GameObj()
                                                    {
                                                        Frame = 15,
                                                        Position = currentPosition
                                                    });
                                            
                                            else
                                                walls.Add(new GameObj()
                                                {
                                                    Frame = 2,
                                                    Position = currentPosition
                                                });
                                    else if (positionManager[x, y + 1, floor].type == "wall")
                                            if (positionManager[x, y - 1, floor].type == "wall")
                                                    walls.Add(new GameObj()
                                                    {
                                                        Frame = 12,
                                                        Position = currentPosition
                                                    });
                                            else
                                                walls.Add(new GameObj()
                                                {
                                                    Frame = 16,
                                                    Position = new Vector2(x, y)
                                                });
                                    else if (positionManager[x, y - 1, floor].type == "wall")
                                            walls.Add(new GameObj()
                                            {
                                                Frame = 11,
                                                Position = currentPosition
                                            });
                                    else
                                        walls.Add(new GameObj()
                                        {
                                            Frame = 3,
                                            Position = currentPosition
                                        });
                            else if (positionManager[x + 1, y, floor].type == "wall")
                                    if (positionManager[x, y + 1, floor].type == "wall")
                                            if (positionManager[x, y - 1, floor].type == "wall")
                                                    walls.Add(new GameObj()
                                                    {
                                                        Frame = 13,
                                                        Position = currentPosition
                                                    });
                                            else
                                                walls.Add(new GameObj()
                                                {
                                                    Frame = 15,
                                                    Position = currentPosition
                                                });
                                    else if (positionManager[x, y - 1, floor].type == "wall")
                                            walls.Add(new GameObj()
                                            {
                                                Frame = 9,
                                                Position = currentPosition
                                            });
                                    else
                                        walls.Add(new GameObj()
                                        {
                                            Frame = 1,
                                            Position = currentPosition
                                        });
                            else if (positionManager[x, y - 1, floor].type == "wall")
                                    if (positionManager[x - 1, y - 1, floor].type == "wall")
                                        if (positionManager[x + 1, y - 1, floor].type == "wall")
                                                    walls.Add(new GameObj()
                                                    {
                                                        Frame = 8,
                                                        Position = currentPosition
                                                    });
                                    else if (positionManager[x, y + 1, floor].type == "wall")
                                            walls.Add(new GameObj()
                                            {
                                                Frame = 5,
                                                Position = currentPosition
                                            });
                                    else
                                        walls.Add(new GameObj()
                                        {
                                            Frame = 6,
                                            Position = currentPosition
                                        });
                            else if (positionManager[x, y + 1, floor].type == "wall")
                                    walls.Add(new GameObj()
                                    {
                                        Frame = 4,
                                        Position = currentPosition
                                    });
                            else
                                walls.Add(new GameObj()
                                {
                                    Frame = 0,
                                    Position = currentPosition
                                });
                            break;
                        case "chest":
                            objects.Add(new GameObj()
                            {
                                Frame = 18,
                                Position = currentPosition
                            });
                            positionManager[x,y,floor].iteration = objects.Count();
                            break;
                        case "upstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 20,
                                Position = currentPosition
                            });
                            break;
                        case "downstairs":
                            objects.Add(new GameObj()
                            {
                                Frame = 21,
                                Position = currentPosition
                            });
                            break;
                        default:
                            break;
                    }
                }
            }
        }

    }
}

