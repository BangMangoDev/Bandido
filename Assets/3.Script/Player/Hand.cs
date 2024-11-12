using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hand : MonoBehaviour
{
    public List<GameObject> hand_list;
    [SerializeField] private Tile_Manager tile_manager;
    [SerializeField] private Tile tile;
    private float spacing = 0.1f; // 타일 간의 간격 (뷰포트 좌표 기준)
    private float yOffset = 0.12f; // 화면 하단의 Y축 오프셋
    private void Start()
    {
        Init_Hand();
        tile.PlayerAdd(this);
    }

    private void Update()
    {
        SetHandTilesPosition();
    }

    private void Init_Hand()
    {
        for (int i = 0; i < 3; i++)
        {
            hand_list.Add(tile_manager.GetNextTile());
            hand_list[i].SetActive(true);
            tile_manager.DequeueTile();
        }
    }

    public void Change_Hand()
    {
        if (tile.current_tile != null)
        {
            return;
        }

        hand_list.RemoveAll(tile =>
        {
            tile_manager.tile_Queue.Enqueue(tile); // 각 타일을 tile_Queue에 추가
            tile.SetActive(false);                 // 타일을 비활성화 (SetActive(false))
            return true;                           // 이 타일을 hand_list에서 제거하도록 지정
        });
        for(int i = 0; i < 3; i++)
        {
        Add_Hand();
        }
        
        
    }

    public void Add_Hand()
    {
        if (tile_manager.tile_Queue.Count == 0) return;

        hand_list.Add(tile_manager.GetNextTile());
        tile_manager.GetNextTile().SetActive(true);
        tile_manager.DequeueTile();
    }

    public void Use_Hand(int index)
    {
        hand_list.RemoveAt(index);
        Add_Hand(); // 새로운 타일 추가
    }
    // hand_list에 있는 타일들의 위치를 화면 하단에 배치하는 메서드
    private void SetHandTilesPosition()
    {
        Camera mainCamera = Camera.main;

        for (int i = 0; i < hand_list.Count; i++)
        {

            // 화면 하단에 고정된 위치를 설정 (뷰포트 좌표로 설정)
            Vector3 viewPortPosition = new Vector3(0.5f + (-1+i) * spacing, yOffset, 10f); // 중앙 기준으로 좌우 배치
            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewPortPosition);

            // 타일의 위치를 화면 하단에 맞게 설정
            hand_list[i].transform.position = new Vector3(worldPosition.x, worldPosition.y + 4, worldPosition.z + 2f);
            
        }
    }

}
