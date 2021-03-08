using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject wall;
    public GameObject platform;
    public List<Vector3> createdTiles;
    static public float[] MapSize = { 30, 15 };
    public int tileSize;
    public float WaitTime;
    public float ExtremeX;
    public float ExtremeY;
    public float[] X = new float[2];
    public int Fixed = 0;
    public int[,] TileType = new int[(int)MapSize[0], (int)MapSize[1]];
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
                int tile = Random.Range(0, tiles.Length);
                int p = Random.Range(0, 3);
                X[0] = transform.position.x / tileSize;
                X[1] = transform.position.y / tileSize;
                CreateTile(tile);
                MoveGenPath(p, Vft);
            }
            yield return new WaitForSeconds(WaitTime);
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
    void CreateTile(int tileindex)
    {
        if (!(createdTiles.Contains(transform.position)))
        {
            GameObject tileObject;
            tileObject = Instantiate(tiles[tileindex], transform.position, transform.rotation) as GameObject;
            createdTiles.Add(tileObject.transform.position);
        }
    }
    void Finish()
    {
        CreateWall();
    }
    void CreateWall()
    {
        //Build Top
        for (int y = (int)MapSize[1]; y < (int)(MapSize[1] + ExtremeY / 2); y++)
        {
            for (int x = (int)(-ExtremeX / 2); x < (int)(ExtremeX / 2 + MapSize[0]); x++)
            {
                Instantiate(wall, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Bot
        for (int y = (int)-ExtremeY / 2; y < 0; y++)
        {
            for (int x = (int)(-ExtremeX / 2); x < (int)(ExtremeX / 2 + MapSize[0]); x++)
            {
                Instantiate(wall, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Left
        for (int x = (int)-ExtremeX / 2; x < 0; x++)
        {
            for (int y = 0; y < MapSize[1]; y++)
            {
                Instantiate(wall, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Build Rigth
        for (int x = (int)MapSize[0]; x < (int)(ExtremeX / 2 + MapSize[0]); x++)
        {
            for (int y = 0; y < MapSize[1]; y++)
            {
                Instantiate(wall, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
            }
        }
        //Set tile class
        for (int y = 0; y < MapSize[1]; y++)
        {
            for (int x = 0; x < MapSize[0]; x++)
            {
                if (!(createdTiles.Contains(new Vector3(x * tileSize, y * tileSize, 0))))
                {
                    TileType[x, y] = Random.Range(0, 3);
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
        //Build tile
        for (int y = 0; y < MapSize[1]; y++)
        {
            for (int x = 0; x < MapSize[0]; x++)
            {
                if (!(createdTiles.Contains(new Vector3(x * tileSize, y * tileSize, 0))))
                {
                    switch (TileType[x, y])
                    {
                        case 0:
                            int t = Random.Range(0, tiles.Length);
                            Instantiate(tiles[t], new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
                            break;
                        case 1:
                            Instantiate(platform, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
                            break;
                        case 2:
                            Instantiate(wall, new Vector3(x * tileSize, y * tileSize, 0), transform.rotation);
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
