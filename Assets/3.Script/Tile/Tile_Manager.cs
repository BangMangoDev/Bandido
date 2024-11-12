using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] tile_Prefabs;
    [SerializeField] private GameObject[] tile_L_Prefabs;
    [SerializeField] public GameObject easy_Tile;
    [SerializeField] public GameObject hard_Tile;
    [SerializeField] private GameObject tile_Pool;
    public Queue<GameObject> tile_Queue;

    private void Awake()
    {
        InitializeTiles();
    }

    private void InitializeTiles()
    {
        List<GameObject> tile_List = new List<GameObject>();
        tile_Queue = new Queue<GameObject>();

        foreach(var prefab in tile_Prefabs)
        {
            int count = 2;
            for(int i = 0; i < count; i++)
            {
                tile_List.Add(prefab);
            }
        }
        foreach(var prefab in tile_L_Prefabs)
        {
            int count = 3;
            for(int i = 0; i < count; i++)
            {
                tile_List.Add(prefab);
            }
        }
        ShuffleTiles(tile_List);

        for(int i = 0; i < tile_List.Count; i++)
        {
            GameObject tile = Instantiate(tile_List[i]);
            tile.SetActive(false);
            tile.GetComponent<Tile_SO>().index = i;
            tile.transform.SetParent(tile_Pool.transform);
            tile_Queue.Enqueue(tile);
        }
    }

    public void ShuffleTiles(List<GameObject> tile_List)
    {
        for(int i = 0; i < tile_List.Count; i++)
        {
            GameObject temp = tile_List[i];
            int randomindex = Random.Range(i, tile_List.Count);
            tile_List[i] = tile_List[randomindex];
            tile_List[randomindex] = temp;
        }
    }

    public GameObject GetNextTile()
    {
        if (tile_Queue.Count > 0)
        {
            return tile_Queue.Peek();
        }
        return null;
    }
    public void DequeueTile()
    {
        if (tile_Queue.Count > 0)
        {
            tile_Queue.Dequeue();
        }
    }
}
