using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Dungeon_Crawler.Managers;

namespace Dungeon_Crawler.Managers
{
    public class MapManager
    {
        Color[] colours;
        Color[,] colours2D;
        Rectangle[,] tileRects;
        Texture2D[,] tileTex;

        List<Rectangle> trackRects;
        public List<Rectangle> towerRects;
        public List<Rectangle> builtTowerRects;

        List<Vector2> openNodes;
        public List<Vector2> trackList;
        Vector2 startNode;
        Vector2 endNode;

        Texture2D map;
        Texture2D terrain;
        Texture2D path;
        
        Vector2 mapPos = new Vector2(270, 10);
        Vector2 TileDimentions = new Vector2(50, 50);
        Rectangle tileRect = new Rectangle(0, 0, 50, 50);

        public Rectangle tempTower;

        GameManager gameManager;

        public MapManager(GameManager _gameManager, List<Texture2D> _mapTextures)
        {
            map = _mapTextures[0];
            terrain = _mapTextures[1];
            path = _mapTextures[2];
            
            trackRects = new List<Rectangle>();
            towerRects = new List<Rectangle>();
            builtTowerRects = new List<Rectangle>();
            tileRects = new Rectangle[map.Width, map.Height];
            tileTex = new Texture2D[map.Width, map.Height];
            colours2D = new Color[map.Width, map.Height];

            openNodes = new List<Vector2>();
            trackList = new List<Vector2>();
            gameManager = _gameManager;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                {
                    spriteBatch.Draw(tileTex[x, y], tileRects[x, y], Color.White);
                }
            if (gameManager.towerManager.buildMode)
            {
                foreach (Rectangle rect in towerRects)
                {
                    spriteBatch.Draw(terrain, rect, Color.LightSkyBlue);
                }
            }
        }

        public Vector2 getStartNode()
        {
            return trackList[0];
        }
        public bool checkTowers(Point point)
        {
            foreach (Rectangle rect in towerRects)
            {
                if(rect.Contains(point))
                {
                    tempTower = rect;
                    return true;
                }
            }
            return false;
        }

        public void addTower()
        {
            builtTowerRects.Add(tempTower);
            towerRects.Remove(tempTower);
        }

        void CreateTrack()
        {
            colours = new Color[map.Width * map.Height];
            map.GetData<Color>(colours);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    colours2D[x, y] = colours[x + y * map.Width];
                }
            }
        }

        void CreateArrays()
        {
            Vector2 mapTemp = new Vector2();
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    mapTemp.X = mapPos.X + (50 * x);
                    mapTemp.Y = mapPos.Y + (50 * y);

                    tileRects[x, y] = new Rectangle((int)mapTemp.X, (int)mapTemp.Y, 50, 50);
                    towerRects.Add(new Rectangle((int)mapTemp.X, (int)mapTemp.Y, 50, 50));

                    if (colours2D[x, y].R == 0)
                        tileTex[x, y] = terrain;
                    else if (colours2D[x, y].R == 255)
                    {
                        tileTex[x, y] = path;
                        towerRects.RemoveAt(towerRects.Count - 1);
                        Vector2 tempNode = new Vector2(mapTemp.X + 25, mapTemp.Y + 25);
                        openNodes.Add(tempNode);
                        if (colours2D[x, y].G == 255)
                        {
                            startNode = tempNode;
                        }
                        else if (colours2D[x, y].B == 255)
                        {
                            endNode = tempNode;
                        }
                    }
                }
            }
        }

        void FindPath()
        {
            Vector2 sNode = Vector2.Zero;
            int sNodeRef = 0;

            //find start and end nodes
            for (int i = 0; i < openNodes.Count; i++)
            {
                if (openNodes[i] == startNode)
                {
                    sNode = openNodes[i];
                    sNodeRef = i;
                }
            }

            Vector2 cNode = Vector2.Zero;
            int cNodeRef = 0;
            Vector2 nNode = Vector2.Zero;
            int nNodeRef = 0;
            float cDistance = 50;

            cNode = sNode;
            cNodeRef = sNodeRef;
            while (openNodes.Count != 0)
            {

                openNodes.RemoveAt(cNodeRef);
                trackList.Add(cNode);
                for (int i = 0; i < openNodes.Count; i++)
                {
                    Vector2 v = openNodes[i];
                    float distance = (float)Math.Sqrt((cNode.X - v.X) * (cNode.X - v.X) + (cNode.Y - v.Y) * (cNode.Y - v.Y));
                    if (distance == cDistance)
                    {
                        nNode = v;
                        nNodeRef = i;
                        break;
                    }
                }
                cNode = nNode;
                cNodeRef = nNodeRef;
            }
        }

        public void Run()
        {
            CreateTrack();
            CreateArrays();
            FindPath();
        }
    }
}
