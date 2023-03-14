using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    public static Tile_Manager Instance { get; private set; }

    private Dictionary<Vector2, GameObject> tiles = new Dictionary<Vector2, GameObject>();


    void Awake()
    {
        Instance = this;

        // get all tiles and put them into a dictionary
        Tile[] all_tiles = FindObjectsOfType<Tile>();
        foreach (var tile in all_tiles)
        {
            Vector2 correctedPos = tile.transform.position;

            correctedPos.x = Mathf.FloorToInt(correctedPos.x) + 0.5f;
            correctedPos.y = Mathf.FloorToInt(correctedPos.y) + 0.5f;

            tiles.Add(correctedPos, tile.gameObject);
        }
    }
    
    */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
