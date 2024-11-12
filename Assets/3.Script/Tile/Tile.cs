
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public enum TileShape { Row, Column, Re_Row, Re_Column, Null } // 타일 상태를 나타내는 enum
    public TileShape currentTileShape; // 현재 타일 상태

    [SerializeField] private Grid grid;
    private Tile_Manager tile_manager;

    public LayerMask tile_layer_mask;
    public LayerMask placed_tile_layermask;

    public Transform current_hovered_tile_placeholder;
    public GameObject current_tile;

    public Vector3 tileOffset; // 타일의 피벗 보정용 오프셋

    public List<GameObject> placed_Tile = new List<GameObject>();
    private Hand player;

    private List<Coordinate> placed_placeholder = new List<Coordinate>();

    [SerializeField] private GameObject ending_ui;

    private void Start()
    {
        tile_manager = this.GetComponent<Tile_Manager>();
        UpdateTileOffset();
        SetStartTile();

    }

    private void Update()
    {
       if (current_tile == null)
       {
            return;
       }else
       {
            DetectHoverTilePlaceholder();
            if (current_hovered_tile_placeholder != null)
            {
                ShowNextTileAtPlaceholder();
                PlaceTile();
            }
            HandleTileRotation();
       }

    }

    public void PlayerAdd(Hand hand)
    {
        player = hand;
    }

    private bool Node_Check()
    {
        return current_tile.GetComponent<Tile_Coordinate>().NodeCheck(this);
    }

    private void SetStartTile()
    {
        string target_name = "50,50";
        Transform target_placeholder = grid.transform.Find(target_name);
        Transform adjacent_placeholder = grid.transform.Find("49,50");
        GameObject start_tile;
        if (target_placeholder != null)
        {
            if (GameManager.instance.current_scene == "EasyGame")
            {
            start_tile = Instantiate(tile_manager.easy_Tile, target_placeholder.position, Quaternion.identity, target_placeholder);
            }
            else
            {
            start_tile = Instantiate(tile_manager.hard_Tile, target_placeholder.position, Quaternion.identity, target_placeholder);
            }
            start_tile.name = "Bandido";

            // Z축 기준으로 90도 회전
            start_tile.transform.Rotate(0, 90, 0);

            // 타일 오프셋 적용
            start_tile.transform.position = target_placeholder.position + new Vector3(-1, 0.01f, 0) + tileOffset;
            target_placeholder.tag = "TilePlaced";
            adjacent_placeholder.tag = "TilePlaced";
            AddPlaceHolder(50, 50);
            AddPlaceHolder(49, 50);
            start_tile.GetComponent<Tile_Coordinate>().SetStartTileNode();
        }
    }




    private void DetectHoverTilePlaceholder()
    {
        if (current_tile == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tile_layer_mask))
        {
            if (hit.transform.CompareTag("TilePlaceholder") || hit.transform.CompareTag("TilePlaceable"))
            {
                current_hovered_tile_placeholder = hit.transform;
            }
            else
            {
                if (current_tile.activeSelf) current_tile.SetActive(false);
                current_hovered_tile_placeholder = null;
            }
        }
        else
        {
            current_hovered_tile_placeholder = null;
        }
    }

    private void ShowNextTileAtPlaceholder()
    {
        if (current_tile == null)
        {
            return;
        }

        if (current_tile != null)
        {
            current_tile.SetActive(true);

            // 타일 피벗 보정을 통해 타일의 한쪽 끝이 플레이스홀더 중심에 위치하도록 설정
            current_tile.transform.position = current_hovered_tile_placeholder.position + new Vector3(0, 0.01f, 0) + tileOffset;
        }
    }

    private void HandleTileRotation()
    {
        if (current_tile != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                current_tile.transform.RotateAround(current_tile.transform.position - tileOffset, Vector3.up, -90);
                // 피벗 기준으로 회전
                UpdateTileShapeAndOffset();
                // 타일 모양과 오프셋 업데이트

                Debug.Log($"{currentTileShape}");
                GetAdjacentPlaceHolder();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                current_tile.transform.RotateAround(current_tile.transform.position - tileOffset, Vector3.up, 90);
                // 피벗 기준으로 회전
                UpdateTileShapeAndOffset();
                // 타일 모양과 오프셋 업데이트
            }
        }
    }
    public void InitRotate()
    {
        current_tile.transform.RotateAround(current_tile.transform.position - tileOffset, Vector3.up, -90);
        // 피벗 기준으로 회전
        UpdateTileShapeAndOffset();
        // 타일 모양과 오프셋 업데이트
    }

    // 타일의 현재 모양에 맞는 오프셋을 업데이트
    private void UpdateTileShapeAndOffset()
    {
        // 회전각도에 따라 타일이 Row(가로) 상태인지 Column(세로) 상태인지 판별
        if (current_tile.transform.rotation.eulerAngles.y == 90)
        {
            currentTileShape = TileShape.Row; // 가로 상태
        }
        else if (current_tile.transform.rotation.eulerAngles.y == 0)
        {
            currentTileShape = TileShape.Column; // 세로 상태
        }
        else if (current_tile.transform.rotation.eulerAngles.y == 180)
        {
            currentTileShape = TileShape.Re_Column; // 역세로 상태
        }
        else
        {
            currentTileShape = TileShape.Re_Row; //역가로 상태
        }

        UpdateTileOffset();
    }

    // 타일의 모양에 따라 오프셋 설정
    public void UpdateTileOffset()
    {

        if (currentTileShape == TileShape.Row)
        {
            tileOffset = new Vector3(0.5f, 0, 0); // 가로일 때 오프셋 (X축으로 이동)
        }
        else if (currentTileShape == TileShape.Column)
        {
            tileOffset = new Vector3(0, 0, 0.5f); // 세로일 때 오프셋 (Z축으로 이동)
        }
        else if (currentTileShape == TileShape.Re_Row)
        {
            tileOffset = new Vector3(-.5f, 0, 0);
        }
        else
        {
            tileOffset = new Vector3(0, 0, -0.5f);
        }
    }

    private void PlaceTile()
    {
        Coordinate adjacentTileCoordinate = GetAdjacentPlaceHolder();

        Transform adjacent_placeholder = grid.transform.Find($"{adjacentTileCoordinate.x},{adjacentTileCoordinate.y}");
        grid.transform.Find($"{adjacentTileCoordinate.x},{adjacentTileCoordinate.y}");
        if (Input.GetMouseButtonDown(0)
            &&
            (current_hovered_tile_placeholder.CompareTag("TilePlaceable") ||
            adjacent_placeholder.CompareTag("TilePlaceable"))
            &&
            !(current_hovered_tile_placeholder.CompareTag("TilePlaced") ||
            adjacent_placeholder.CompareTag("TilePlaced"))
            &&
            Node_Check()) // 마우스 왼쪽 버튼 클릭 시 타일 배치
        {
            if (current_hovered_tile_placeholder != null && current_tile != null
                && current_hovered_tile_placeholder.tag != "TilePlaced")
            {
                player.hand_list.Remove(current_tile);
                player.Add_Hand();
                // 타일의 끝과 placeholder의 끝이 맞도록 오프셋을 적용하여 배치
                current_tile.transform.position = (current_hovered_tile_placeholder.position + new Vector3(0, 0.01f, 0)) + tileOffset;
                current_tile.transform.SetParent(current_hovered_tile_placeholder); // 타일을 placeholder의 자식으로 설정
                current_tile.GetComponent<Tile_Coordinate>().SetTileNode(this);
                current_tile = null;
                current_hovered_tile_placeholder.gameObject.tag = "TilePlaced";
                adjacent_placeholder.tag = "TilePlaced";

                AddPlaceHolder(int.Parse(current_hovered_tile_placeholder.name.Split(',')[0]),
                    int.Parse(current_hovered_tile_placeholder.name.Split(',')[1]));
                AddPlaceHolder(adjacentTileCoordinate);
                

                
                if (WinCheck())
                {
                    GameManager.instance.ending = "Win";
                    ending_ui.SetActive(true);

                }else if (player.hand_list.Count == 0)
                {
                    GameManager.instance.ending = "Lose";
                    ending_ui.SetActive(true);
                }

                               
            }
        }
    }
    private void AddPlaceHolder(Coordinate coordinate)
    {
        placed_placeholder.Add(coordinate);
    }
    private void AddPlaceHolder(int x,int y)
    {
        Coordinate coordinate=new Coordinate(x,y);
        placed_placeholder.Add(coordinate);
    }
    private bool WinCheck()
    {
        foreach(var coordinate in placed_placeholder)
        {
           foreach(var node in grid.transform.
                Find($"{coordinate.x},{coordinate.y}").GetComponent<PlaceHolder_Node>().
                nodeList)
            {
                if (node.isOpen == true)
                {
                    Debug.Log($"{coordinate.x},{coordinate.y}");
                    return false;
                }
            }
        }
        Debug.Log("모든 오픈노드 닫힘");
        return true;
    }

    public Coordinate GetAdjacentPlaceHolder()
    {
        Coordinate coordinate = new Coordinate();
        if (currentTileShape == TileShape.Row)
        {
            int x = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[0]) + 1;
            int y = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[1]);
            coordinate.x = x;
            coordinate.y = y;
        }
        else if (currentTileShape == TileShape.Column)
        {
            int x = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[0]);
            int y = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[1]) + 1;
            coordinate.x = x;
            coordinate.y = y;
        }
        else if (currentTileShape == TileShape.Re_Row)
        {
            int x = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[0]) - 1;
            int y = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[1]);

            coordinate.x = x;
            coordinate.y = y;
        }
        else
        {
            int x = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[0]);
            int y = int.Parse(current_hovered_tile_placeholder.gameObject.name.Split(',')[1]) - 1;
            coordinate.x = x;
            coordinate.y = y;
        }

        return coordinate;
    }
}

public class Coordinate
{
    public int x;
    public int y;
    public Coordinate() { }
    public Coordinate(int x,int y)
    {
        this.x = x;
        this.y = y;
    }
}
