using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Coordinate : MonoBehaviour
{
    

    [SerializeField] private List<NodeState> nodeList = new List<NodeState>();
    private Grid grid;

    // NodeList가 변경될 때마다 호출되어 자동으로 enum 값을 추가하는 메서드
    private void OnValidate()
    {
        // enum의 모든 값을 순회
        foreach (Node node in Enum.GetValues(typeof(Node)))
        {
            // 만약 nodeList에 해당 Node가 없다면 추가
            if (!nodeList.Exists(n => n.node == node))
            {
                nodeList.Add(new NodeState { node = node, isOpen = false }); // 기본값으로 isOpen을 false로 설정
            }
        }
        // nodeList에 존재하지 않는 enum 값을 삭제하지 않기 위해 위의 추가 작업만 수행
    }

    private void OnEnable()
    {
        grid = FindObjectOfType<Grid>();
    }

    // OnDrawGizmos에서 Open인 노드만 시각화
    private void OnDrawGizmos()
    {
        // Gizmo의 색상을 설정 (Open인 노드는 녹색으로 표시)
        Gizmos.color = Color.green;

        // nodeList를 순회하면서 isOpen이 true인 노드만 시각화
        foreach (var nodeState in nodeList)
        {
            if (nodeState.isOpen)
            {
                Vector3 nodePosition = GetNodePosition(nodeState.node);
                Gizmos.DrawSphere(nodePosition, 0.1f); // 각 노드를 구체로 시각화
            }
        }
    }

    

    // 각 Node Enum 값에 따라 좌표를 설정하고 회전 적용
    private Vector3 GetNodePosition(Node node)
    {
        Vector3 localNodePosition;

        switch (node)
        {
            case Node.Left_Up:
                localNodePosition = new Vector3(-0.5f, 0, -0.5f);  // 로컬 좌표 (위쪽 좌측)
                break;
            case Node.Left_Down:
                localNodePosition = new Vector3(0.5f, 0, -0.5f); // 로컬 좌표 (아래쪽 좌측)
                break;
            case Node.Right_Up:
                localNodePosition = new Vector3(-0.5f, 0, 0.5f);   // 로컬 좌표 (위쪽 우측)
                break;
            case Node.Right_Down:
                localNodePosition = new Vector3(0.5f, 0, 0.5f);  // 로컬 좌표 (아래쪽 우측)
                break;
            case Node.Left:
                localNodePosition = new Vector3(0, 0, -1);  // 로컬 좌표 (왼쪽)
                break;
            case Node.Right:
                localNodePosition = new Vector3(0, 0, 1);   // 로컬 좌표 (오른쪽)
                break;
            default:
                localNodePosition = Vector3.zero;
                break;
        }

        // 로컬 좌표를 타일의 회전값에 맞게 변환하여 반환 (회전 적용)
        return transform.position + (transform.rotation * localNodePosition);
    }

    public void SetTileNode(Tile tile)
    {
        
        switch (tile.currentTileShape)
        {
            case Tile.TileShape.Row:
                foreach (var node in nodeList)
                {
                    int x = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]);
                    int y = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]);

                    switch (node.node)
                    {
                        case Node.Left_Up:
                            y += 1;
                            break;
                        case Node.Left_Down:
                            y -= 1;
                            break;
                        case Node.Right_Up:
                            x += 1;
                            y += 1;
                            break;
                        case Node.Right_Down:
                            x += 1;
                            y -= 1;
                            break;
                        case Node.Left:
                            x -= 1;
                            break;
                        case Node.Right:
                            x += 2;
                            break;
                    }
                    if (node.isOpen)
                    {
                        if (!grid.transform.Find($"{x},{y}").CompareTag("TilePlaced"))
                        {
                            grid.transform.Find($"{x},{y}").tag = "TilePlaceable";
                        }
                        else
                        {
                            switch (node.node)
                            {
                                case Node.Left_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                                case Node.Left_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                                case Node.Right_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0])+1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                                case Node.Right_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0])+1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                                case Node.Left:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                                case Node.Right:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0])+1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                            }

                            
                        }

                        

                    }
                    else// closenode의 경우
                    {
                        switch (node.node)
                        {
                            case Node.Left_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen=false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                            case Node.Left_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                            case Node.Right_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                            case Node.Right_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                            case Node.Left:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                            case Node.Right:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                        }
                    }

                    grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                    grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;


                }
                break;
            case Tile.TileShape.Column:
                foreach (var node in nodeList)
                {
                    int x = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]);
                    int y = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]);

                    switch (node.node)
                    {
                        case Node.Left_Up:
                            x -= 1;
                            break;
                        case Node.Left_Down:
                            x += 1;
                            break;
                        case Node.Right_Up:
                            x -= 1;
                            y += 1;
                            break;
                        case Node.Right_Down:
                            x += 1;
                            y += 1;
                            break;
                        case Node.Left:
                            y -= 1;
                            break;
                        case Node.Right:
                            y += 2;
                            break;
                    }
                    if (node.isOpen)
                    {
                        if (!grid.transform.Find($"{x},{y}").CompareTag("TilePlaced"))
                        {
                            grid.transform.Find($"{x},{y}").tag = "TilePlaceable";
                        }
                        else
                        {
                            switch (node.node)
                            {
                                case Node.Left_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                                case Node.Left_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                                case Node.Right_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1])+1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                                case Node.Right_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1])+1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                                case Node.Left:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                                case Node.Right:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1])+1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                            }


                        }
                    }
                    else
                    {
                        switch (node.node)
                        {
                            case Node.Left_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                            case Node.Left_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                            case Node.Right_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                            case Node.Right_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                            case Node.Left:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                            case Node.Right:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                        }
                    }

                    grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                    grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                }
                break;
            case Tile.TileShape.Re_Row:
                foreach (var node in nodeList)
                {
                    int x = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]);
                    int y = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]);

                    switch (node.node)
                    {
                        case Node.Left_Up:
                            y -= 1;
                            break;
                        case Node.Left_Down:
                            y += 1;
                            break;
                        case Node.Right_Up:
                            y -= 1;
                            x -= 1;
                            break;
                        case Node.Right_Down:
                            y += 1;
                            x -= 1;
                            break;
                        case Node.Left:
                            x += 1;
                            break;
                        case Node.Right:
                            x -= 2;
                            break;
                    }
                    if (node.isOpen)
                    {
                        if (!grid.transform.Find($"{x},{y}").CompareTag("TilePlaced"))
                        {
                            grid.transform.Find($"{x},{y}").tag = "TilePlaceable";
                        }
                        else
                        {
                            switch (node.node)
                            {
                                case Node.Left_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                                case Node.Left_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                                case Node.Right_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]) + -1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                                case Node.Right_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0])  -1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                                case Node.Left:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                                case Node.Right:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]) + -1}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                            }


                        }
                    }
                    else
                    {
                        switch (node.node)
                        {
                            case Node.Left_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                            case Node.Left_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                            case Node.Right_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                            case Node.Right_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                            case Node.Left:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                            case Node.Right:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                        }
                    }

                    grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                    grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                }
                break;
            case Tile.TileShape.Re_Column:
                foreach (var node in nodeList)
                {
                    int x = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]);
                    int y = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]);

                    switch (node.node)
                    {
                        case Node.Left_Up:
                            x += 1;
                            break;
                        case Node.Left_Down:
                            x -= 1;
                            break;
                        case Node.Right_Up:
                            x += 1;
                            y -= 1;
                            break;
                        case Node.Right_Down:
                            x -= 1;
                            y -= 1;
                            break;
                        case Node.Left:
                            y += 1;
                            break;
                        case Node.Right:
                            y -= 2;
                            break;
                    }
                    if (node.isOpen)
                    {
                        if (!grid.transform.Find($"{x},{y}").CompareTag("TilePlaced"))
                        {
                            grid.transform.Find($"{x},{y}").tag = "TilePlaceable";
                        }
                        else
                        {
                            switch (node.node)
                            {
                                case Node.Left_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                                case Node.Left_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                                case Node.Right_Up:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]) + -1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    break;
                                case Node.Right_Down:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]) + -1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                    break;
                                case Node.Left:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{tile.current_hovered_tile_placeholder.name.Split(',')[1]}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    break;
                                case Node.Right:
                                    grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                    grid.transform.Find
                                        ($"{tile.current_hovered_tile_placeholder.name.Split(',')[0]}," +
                                        $"{int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]) + -1}").
                                        GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                    break;
                            }


                        }
                    }
                    else
                    {
                        switch (node.node)
                        {
                            case Node.Left_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                            case Node.Left_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                            case Node.Right_Up:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                break;
                            case Node.Right_Down:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
                                break;
                            case Node.Left:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                break;
                            case Node.Right:
                                grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                                grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                                break;
                        }
                    }

                    grid.transform.Find($"{tile.current_hovered_tile_placeholder.name}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen = false;
                    grid.transform.Find($"{tile.GetAdjacentPlaceHolder().x},{tile.GetAdjacentPlaceHolder().y}").GetComponent<PlaceHolder_Node>().
                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen = false;
                }
                break;
        }

    }

    public void SetStartTileNode()
    {
        if (GameManager.instance.current_scene == "EasyGame")
        {
        grid.transform.Find("49,51").tag = "TilePlaceable";
        grid.transform.Find("50,51").tag = "TilePlaceable";
        grid.transform.Find("49,49").tag = "TilePlaceable";
        grid.transform.Find("50,49").tag = "TilePlaceable";
        grid.transform.Find("51,50").tag = "TilePlaceable";
        grid.transform.Find("48,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
        grid.transform.Find("49,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
        grid.transform.Find("49,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
        grid.transform.Find("50,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
        }
        else
        {
            grid.transform.Find("48,50").tag = "TilePlaceable";
            grid.transform.Find($"49,51").tag = "TilePlaceable";
            grid.transform.Find($"50,51").tag = "TilePlaceable";
            grid.transform.Find($"49,49").tag = "TilePlaceable";
            grid.transform.Find($"50,49").tag = "TilePlaceable";
            grid.transform.Find($"51,50").tag = "TilePlaceable";
            grid.transform.Find("49,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Right).isOpen = false;
            grid.transform.Find("50,50").GetComponent<PlaceHolder_Node>().GetNodeState(PlaceHolder_Node.Node.Left).isOpen = false;
        }

    }

    public bool NodeCheck(Tile tile)
    {
        foreach(var node in tile.current_tile.GetComponent<Tile_Coordinate>().nodeList)
        {
            int x = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[0]);
            int y = int.Parse(tile.current_hovered_tile_placeholder.name.Split(',')[1]);

            int right_x = tile.GetAdjacentPlaceHolder().x;
            int right_y = tile.GetAdjacentPlaceHolder().y;

                if (node.isOpen)
                {//opennode일 경우
                    switch (node.node)
                    {
                        case Node.Left_Up:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                            }
                            break;
                        case Node.Left_Down:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                            }
                            break;
                        case Node.Right_Up:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    Debug.Log($"{right_x},{right_y}");
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                            }
                            break;
                        case Node.Right_Down:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                       GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                            }
                            break;
                        case Node.Left:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                            }
                            break;
                        case Node.Right:
                            switch (tile.currentTileShape)
                            {
                                case Tile.TileShape.Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Right).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Row:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Left).isOpen) return false;
                                    break;
                                case Tile.TileShape.Column:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Top).isOpen) return false;
                                    break;
                                case Tile.TileShape.Re_Column:
                                    if (!grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                        GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen) return false;
                                    break;
                            }
                            break;
                    }
            }
            else // closenode 일 경우
            {
                switch (node.node)
                {
                    case Node.Left_Up:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                    &&
                                    grid.transform.Find($"{x-1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x+1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                    case Node.Left_Down:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x + 1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                    &&
                                    grid.transform.Find($"{x - 1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                    case Node.Right_Up:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x+1},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x-1},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                    &&
                                    grid.transform.Find($"{x-1},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x+1},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                    case Node.Right_Down:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x+1},{y - 1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x-1},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x + 1},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                   GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                   &&
                                    grid.transform.Find($"{x-1},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                    case Node.Left:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                    &&
                                    grid.transform.Find($"{x-1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x+1},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y-1}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{x},{y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y+1}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                    case Node.Right:
                        switch (tile.currentTileShape)
                        {
                            case Tile.TileShape.Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Right).isOpen
                                    &&
                                    grid.transform.Find($"{x+2},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Row:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Left).isOpen
                                    &&
                                    grid.transform.Find($"{x-2},{y}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Top).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y+2}").CompareTag("TilePlaced")) return false;
                                break;
                            case Tile.TileShape.Re_Column:
                                if (grid.transform.Find($"{right_x},{right_y}").GetComponent<PlaceHolder_Node>().
                                    GetNodeState(PlaceHolder_Node.Node.Bottom).isOpen
                                    &&
                                    grid.transform.Find($"{x},{y-2}").CompareTag("TilePlaced")) return false;
                                break;
                        }
                        break;
                }
            }
                
            

            

            
        }
        return true;
        
    }


    public enum Node
    {
        Left_Up,
        Left_Down,
        Right_Up,
        Right_Down,
        Left,
        Right
    }

    [System.Serializable]  // Inspector에서 노출되도록 [System.Serializable] 속성 추가
    public class NodeState
    {
        public Node node;   // Node Enum 값
        public bool isOpen; // Open 상태인지 여부
    }

    

}

