using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Coordinate : MonoBehaviour
{
    

    [SerializeField] private List<NodeState> nodeList = new List<NodeState>();
    private Grid grid;

    // NodeList�� ����� ������ ȣ��Ǿ� �ڵ����� enum ���� �߰��ϴ� �޼���
    private void OnValidate()
    {
        // enum�� ��� ���� ��ȸ
        foreach (Node node in Enum.GetValues(typeof(Node)))
        {
            // ���� nodeList�� �ش� Node�� ���ٸ� �߰�
            if (!nodeList.Exists(n => n.node == node))
            {
                nodeList.Add(new NodeState { node = node, isOpen = false }); // �⺻������ isOpen�� false�� ����
            }
        }
        // nodeList�� �������� �ʴ� enum ���� �������� �ʱ� ���� ���� �߰� �۾��� ����
    }

    private void OnEnable()
    {
        grid = FindObjectOfType<Grid>();
    }

    // OnDrawGizmos���� Open�� ��常 �ð�ȭ
    private void OnDrawGizmos()
    {
        // Gizmo�� ������ ���� (Open�� ���� ������� ǥ��)
        Gizmos.color = Color.green;

        // nodeList�� ��ȸ�ϸ鼭 isOpen�� true�� ��常 �ð�ȭ
        foreach (var nodeState in nodeList)
        {
            if (nodeState.isOpen)
            {
                Vector3 nodePosition = GetNodePosition(nodeState.node);
                Gizmos.DrawSphere(nodePosition, 0.1f); // �� ��带 ��ü�� �ð�ȭ
            }
        }
    }

    

    // �� Node Enum ���� ���� ��ǥ�� �����ϰ� ȸ�� ����
    private Vector3 GetNodePosition(Node node)
    {
        Vector3 localNodePosition;

        switch (node)
        {
            case Node.Left_Up:
                localNodePosition = new Vector3(-0.5f, 0, -0.5f);  // ���� ��ǥ (���� ����)
                break;
            case Node.Left_Down:
                localNodePosition = new Vector3(0.5f, 0, -0.5f); // ���� ��ǥ (�Ʒ��� ����)
                break;
            case Node.Right_Up:
                localNodePosition = new Vector3(-0.5f, 0, 0.5f);   // ���� ��ǥ (���� ����)
                break;
            case Node.Right_Down:
                localNodePosition = new Vector3(0.5f, 0, 0.5f);  // ���� ��ǥ (�Ʒ��� ����)
                break;
            case Node.Left:
                localNodePosition = new Vector3(0, 0, -1);  // ���� ��ǥ (����)
                break;
            case Node.Right:
                localNodePosition = new Vector3(0, 0, 1);   // ���� ��ǥ (������)
                break;
            default:
                localNodePosition = Vector3.zero;
                break;
        }

        // ���� ��ǥ�� Ÿ���� ȸ������ �°� ��ȯ�Ͽ� ��ȯ (ȸ�� ����)
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
                    else// closenode�� ���
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
                {//opennode�� ���
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
            else // closenode �� ���
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

    [System.Serializable]  // Inspector���� ����ǵ��� [System.Serializable] �Ӽ� �߰�
    public class NodeState
    {
        public Node node;   // Node Enum ��
        public bool isOpen; // Open �������� ����
    }

    

}

