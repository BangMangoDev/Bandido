using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder_Node : MonoBehaviour
{

    [SerializeField] public List<NodeState> nodeList = new List<NodeState>();


    // NodeList가 변경될 때마다 호출되어 자동으로 enum 값을 추가하는 메서드
    private void OnValidate()
    {
        // enum의 모든 값을 순회
        foreach (Node node in Enum.GetValues(typeof(Node)))
        {
            // 만약 nodeList에 해당 Node가 없다면 추가
            if (!nodeList.Exists(n => n.node == node))
            {
                nodeList.Add(new NodeState { node = node, isOpen = true }); // 기본값으로 isOpen을 true로 설정
            }
        }
    }

    // Node enum 값을 받아서 해당하는 NodeState를 반환하는 메서드
    public NodeState GetNodeState(Node targetNode)
    {
        return nodeList.Find(n => n.node == targetNode);
    }
    public enum Node
    {
        Top,
        Bottom,
        Left,
        Right
    }
    [System.Serializable]
    public class NodeState
    {
        public Node node;   // Node Enum 값
        public bool isOpen; // Open 상태인지 여부, default로 open
    }


}
