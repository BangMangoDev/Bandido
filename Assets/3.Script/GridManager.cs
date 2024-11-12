using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePlaceholderPrefab;
    public Grid grid;
    public int gridWidth = 100;
    public int gridHeight = 100;
    private bool[,] grid_arr; // 그리드 상태를 저장하는 2D 배열

    // Start 메서드에서 그리드 초기화
    private void Awake()
    {
        grid = GetComponent<Grid>();
        InitializeGrid();
    }

    // 그리드 초기화 메서드
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

                // 각 TilePlaceholder 오브젝트의 이름을 좌표값으로 설정
                placeholder.name = $"{x + 1},{z + 1}";  // 좌표 이름 설정
                placeholder.tag = "TilePlaceholder"; // 태그 설정
                placeholder.layer = LayerMask.NameToLayer("TileLayer"); // 레이어 설정

                placeholder.transform.localScale = new Vector3(grid.cellSize.x, grid.cellSize.y, grid.cellSize.z);  // 크기 조정
            }
        }

        // 모든 그리드 셀을 비어 있는 상태로 초기화 (false)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid_arr[x, y] = false;
            }
        }

        Debug.Log("100x100 그리드가 초기화되었습니다.");
    }

    // 특정 좌표의 그리드 셀이 비어 있는지 확인하는 메서드
    public bool IsGridCellEmpty(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return !grid_arr[x, y]; // false면 비어있음
        }
        return false; // 그리드 범위를 벗어날 경우 false 반환
    }

    // 타일을 배치하는 메서드 (타일이 놓일 위치를 true로 설정)
    public void PlaceTile(int x, int y)
    {
        if (IsGridCellEmpty(x, y))
        {
            grid_arr[x, y] = true;
            Debug.Log($"타일이 좌표 ({x}, {y})에 배치되었습니다.");
        }
        else
        {
            Debug.Log("해당 좌표에 타일을 배치할 수 없습니다.");
        }
    }
}
