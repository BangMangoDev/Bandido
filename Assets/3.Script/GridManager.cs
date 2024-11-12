using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePlaceholderPrefab;
    public Grid grid;
    public int gridWidth = 100;
    public int gridHeight = 100;
    private bool[,] grid_arr; // �׸��� ���¸� �����ϴ� 2D �迭

    // Start �޼��忡�� �׸��� �ʱ�ȭ
    private void Awake()
    {
        grid = GetComponent<Grid>();
        InitializeGrid();
    }

    // �׸��� �ʱ�ȭ �޼���
    public void InitializeGrid()
    {
        grid_arr = new bool[gridWidth, gridHeight];

        for(int x = 0; x < gridWidth; x++)
        {
            for(int z = 0; z < gridHeight; z++)
            {
                Vector3Int cellPosition = new Vector3Int(x, 0, z);
                Vector3 cellCenterPosition = grid.GetCellCenterWorld(cellPosition);

                GameObject placeholder = Instantiate(tilePlaceholderPrefab, cellCenterPosition, Quaternion.identity, transform);

                // �� TilePlaceholder ������Ʈ�� �̸��� ��ǥ������ ����
                placeholder.name = $"{x + 1},{z + 1}";  // ��ǥ �̸� ����
                placeholder.tag = "TilePlaceholder"; // �±� ����
                placeholder.layer = LayerMask.NameToLayer("TileLayer"); // ���̾� ����

                placeholder.transform.localScale = new Vector3(grid.cellSize.x, grid.cellSize.y, grid.cellSize.z);  // ũ�� ����
            }
        }

        // ��� �׸��� ���� ��� �ִ� ���·� �ʱ�ȭ (false)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid_arr[x, y] = false;
            }
        }

        Debug.Log("100x100 �׸��尡 �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // Ư�� ��ǥ�� �׸��� ���� ��� �ִ��� Ȯ���ϴ� �޼���
    public bool IsGridCellEmpty(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return !grid_arr[x, y]; // false�� �������
        }
        return false; // �׸��� ������ ��� ��� false ��ȯ
    }

    // Ÿ���� ��ġ�ϴ� �޼��� (Ÿ���� ���� ��ġ�� true�� ����)
    public void PlaceTile(int x, int y)
    {
        if (IsGridCellEmpty(x, y))
        {
            grid_arr[x, y] = true;
            Debug.Log($"Ÿ���� ��ǥ ({x}, {y})�� ��ġ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("�ش� ��ǥ�� Ÿ���� ��ġ�� �� �����ϴ�.");
        }
    }
}
