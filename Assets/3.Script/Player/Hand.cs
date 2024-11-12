using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hand : MonoBehaviour
{
    public List<GameObject> hand_list;
    [SerializeField] private Tile_Manager tile_manager;
    [SerializeField] private Tile tile;
    private float spacing = 0.1f; // Ÿ�� ���� ���� (����Ʈ ��ǥ ����)
    private float yOffset = 0.12f; // ȭ�� �ϴ��� Y�� ������
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
            tile_manager.tile_Queue.Enqueue(tile); // �� Ÿ���� tile_Queue�� �߰�
            tile.SetActive(false);                 // Ÿ���� ��Ȱ��ȭ (SetActive(false))
            return true;                           // �� Ÿ���� hand_list���� �����ϵ��� ����
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
        Add_Hand(); // ���ο� Ÿ�� �߰�
    }
    // hand_list�� �ִ� Ÿ�ϵ��� ��ġ�� ȭ�� �ϴܿ� ��ġ�ϴ� �޼���
    private void SetHandTilesPosition()
    {
        Camera mainCamera = Camera.main;

        for (int i = 0; i < hand_list.Count; i++)
        {

            // ȭ�� �ϴܿ� ������ ��ġ�� ���� (����Ʈ ��ǥ�� ����)
            Vector3 viewPortPosition = new Vector3(0.5f + (-1+i) * spacing, yOffset, 10f); // �߾� �������� �¿� ��ġ
            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewPortPosition);

            // Ÿ���� ��ġ�� ȭ�� �ϴܿ� �°� ����
            hand_list[i].transform.position = new Vector3(worldPosition.x, worldPosition.y + 4, worldPosition.z + 2f);
            
        }
    }

}
