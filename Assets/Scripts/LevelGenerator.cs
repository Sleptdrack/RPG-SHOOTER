using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] wall;//0-1D-1U-1L-1B--2-UB-2RL-2RU-2LU-2LB-2RB-3R-3U-3L-3B-4
    public GameObject[] platform;
    public List<Vector3> createdTiles;
    static public float[] MapSize = { 60, 30 };
    public int tileSize;
    public float WaitTime;
    public float ExtremeX;
    public float ExtremeY;
    public float Wall_Porcentaje;//entre 0-1
    public float Ptf_Porcentaje;//entre 0-1
    public float[] X = new float[2];
    public int Fixed = 0;
    public int[,] TileType = new int[(int)MapSize[0], (int)MapSize[1]];
    public int[,] WallType = new int[(int)MapSize[0], (int)MapSize[1]];
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GeneratedLevel());
    }
    IEnumerator GeneratedLevel()
    {
        bool flag = true;
        X[0] = 0;
        X[1] = 0;
        int Vft = 0;
        CreateLimits();
        while ((X[0] != MapSize[0] && X[1] != MapSize[1]) || flag)
        {
            if (X[0] == MapSize[0] && X[1] == 0)
            {
                Finish();
                Fixed = 1;
                flag = false;
            }
            else
            {
                int p = Random.Range(0, 3);
                X[0] = transform.position.x / tileSize;
                X[1] = transform.position.y / tileSize;
                CreateTile();
                MoveGenPath(p, Vft);
            }
            //yield return new WaitForSeconds(WaitTime);
        }
        yield return 0;
    }
    // Update is called once per frame
    void MoveGenPath(int p, int vft)
    {
        if (transform.position.x / tileSize < MapSize[0])
        {
            switch (p)
            {
                case 0:
                    transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                    vft = 0;
                    break;
                case 1:
                    if (transform.position.y / tileSize < MapSize[1])
                    {
                        if (vft < 2)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y + tileSize, 0);
                            vft += 1;
                        }
                        else
                        {
                            transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                            vft = 0;
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                        vft = 0;
                    }
                    break;
                case 2:
                    if (transform.position.y / tileSize > 0)
                    {
                        if (vft < 2)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y - tileSize, 0);
                            vft += 1;
                        }
                        else
                        {
                            transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                            vft = 0;
                        }
                    }
                    else
                    {
                        transform.position = new Vector3(transform.position.x + tileSize, transform.position.y, 0);
                        vft = 0;
                    }
                    break;
            }
        }
        else
        {
            if (transform.position.y / tileSize > 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - tileSize, 0);
            }
        }
    }
    void CreateTile()
    {
        if (!(createdTiles.Contains(transform.position)))
        {
            createdTiles.Add(transform.position);
        }
    }
    void CreateLimits()
    {
        //Build Top
        for (int y = (int)MapSize[1]; y < (int)(MapSize[1] + ExtremeY / 2); y++)
        {
            for (int x = (int)(-ExtremeX / 2); x < (int)(ExtremeX / 2 + MapSize[0]); x++)
            {
                Instantiate(wall[15], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Bot
        for (int y = (int)-ExtremeY / 2; y < 0; y++)
        {
            for (int x = (int)(-ExtremeX / 2); x < (int)(ExtremeX / 2 + MapSize[0]); x++)
            {
                Instantiate(wall[15], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Left
        for (int x = (int)-ExtremeX / 2; x < 0; x++)
        {
            for (int y = 0; y < MapSize[1]; y++)
            {
                Instantiate(wall[15], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Rigth
        for (int x = (int)MapSize[0]; x < (int)(ExtremeX / 2 + MapSize[0]); x++)
        {
            for (int y = 0; y < MapSize[1]; y++)
            {
                Instantiate(wall[15], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
    }
    void Finish()
    {
        CreateWall();
    }
    void CreateWall()
    {
        //Set tile class
        for (int y = 0; y < MapSize[1]; y++)
        {
            for (int x = 0; x < MapSize[0]; x++)
            {
                if (!(createdTiles.Contains(new Vector3(x * tileSize, y * tileSize, 0))))
                {
                    int p = Random.Range(0, 100);
                    if (p < 100 - Wall_Porcentaje - Ptf_Porcentaje)
                    {
                        TileType[x, y] = 0;
                    }
                    else if (p < 100 - Wall_Porcentaje)
                    {
                        TileType[x, y] = 1;
                    }
                    else
                    {
                        TileType[x, y] = 2;                        
                    }
                    if (y > 0)
                    {
                        if (TileType[x, y] == 2 && TileType[x, y - 1] == 1)
                        {
                            TileType[x, y] = 0;
                        }
                    }
                }
                else
                {
                    TileType[x, y] = 0;
                }

            }
        }
        //Assign sprite to tile
        for(int y = 0; y < MapSize[1]; y++)
        {
            for(int x = 0; x < MapSize[0]; x++)
            {
                //0-1R-1U-1L-1B--2UB-2RL-2RU-2LU-2LB-2RB-3R-3B-3L-3U-4
                //0-1--2--3--4---5---6---7---8---9---10--11-12-13-14-15
                if (y < MapSize[1] - 1 && y > 0 && x > 0 && x < MapSize[0] - 1 && TileType[x,y]==2)
                {
                    //Todo Moho*
                    if (TileType[x - 1, y] != 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 15;
                    }
                    //todo moho menos arriba 3U*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 14;
                    }
                    //todo moho menos izq 3L*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 13;
                    }
                    //todo moho menos 3B*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 12;
                    }
                    //3R*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 11;
                    }
                    //2BR*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 10;
                    }
                    //2BL*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 9;
                    }
                    //2UL*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 8;
                    }
                    //2UR*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 7;
                    }
                    //2RL*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 6;
                    }
                    //2UB*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 5;
                    }
                    //1B*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] != 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 4;
                    }
                    //1L*
                    else if (TileType[x - 1, y] != 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 3;
                    }
                    //1U*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] != 2)
                    {
                        WallType[x, y] = 2;
                    }
                    //1R*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] != 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 1;
                    }
                    //0*
                    else if (TileType[x - 1, y] == 2 && TileType[x, y - 1] == 2 && TileType[x + 1, y] == 2 && TileType[x, y + 1] == 2)
                    {
                        WallType[x, y] = 0;
                    }
                }
            }
        }
        //Build tile

        for (int y = 0; y < MapSize[1]; y++)
        {
            for (int x = 0; x < MapSize[0]; x++)
            {
                if (!(createdTiles.Contains(new Vector3(x * tileSize, y * tileSize, 0))))
                {
                    switch (TileType[x, y])
                    {
                        case 1:
                            Instantiate(platform[Random.Range(0,platform.Length)], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
                            break;
                        case 2:
                            Instantiate(wall[WallType[x,y]], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
                            break;
                    }
                }
            }
        }
    }
    void Update()
    {

    }
}
