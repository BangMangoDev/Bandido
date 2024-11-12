using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder_Node : MonoBehaviour
{

    [SerializeField] public List<NodeState> nodeList = new List<NodeState>();


    // NodeList�� ����� ������ ȣ��Ǿ� �ڵ����� enum ���� �߰��ϴ� �޼���
    private void OnValidate()
    {
        // enum�� ��� ���� ��ȸ
        foreach (Node node in Enum.GetValues(typeof(Node)))
        {
            // ���� nodeList�� �ش� Node�� ���ٸ� �߰�
            if (!nodeList.Exists(n => n.node == node))
            {
                nodeList.Add(new NodeState { node = node, isOpen = true }); // �⺻������ isOpen�� true�� ����
            }
        }
    }

    // Node enum ���� �޾Ƽ� �ش��ϴ� NodeState�� ��ȯ�ϴ� �޼���
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
        public Node node;   // Node Enum ��
        public bool isOpen; // Open �������� ����, default�� open
    }


}
