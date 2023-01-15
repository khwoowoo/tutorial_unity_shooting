using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;

    [Range(0, 1)]
    public float outlinePercent;

    List<Coord> allTileCoords;
    Queue<Coord> suffledTileCoords;

    public int seed = 10;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        suffledTileCoords = new Queue<Coord>(Utiliy.ShuffleArray(allTileCoords.ToArray(), seed));

        string holderName = "Generated map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTitle = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTitle.localScale = Vector3.one * (1 - outlinePercent);
                newTitle.parent = mapHolder;
            }
        }

        int obstacleCount = 10;
        for(int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoords();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity);
            newObstacle.parent = mapHolder;
        }
    }

    public Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }

    public Coord GetRandomCoords()
    {
        Coord temp = suffledTileCoords.Dequeue();
        suffledTileCoords.Enqueue(temp);

        return temp;
    }

    public struct Coord
    {
        public int x;
        public int y;
        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }
}
