using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile_RayCast : MonoBehaviour
{
    [System.Serializable]
    public class RaycastDirection
    {
        public DirectionType directionType; // Raycast�� ���� Ÿ��
        public float length; // Raycast�� ���� (�⺻�� 1)
        public float offsetZ; // Z�� ������ (Right/Left ���⿡ ���)
    }

    public enum DirectionType
    {
        Forward,
        Backward,
        Right,
        Left
    }

    public List<RaycastDirection> raycastDirections = new List<RaycastDirection>(); // Raycast ���� ����Ʈ
    public LayerMask wallLayer; // Wall ���̾� ����ũ ����

    // Ray�� ���� ��ġ�� ����ϴ� �޼��� (Local ��ǥ�� ����)
    private Vector3 GetRayStartPosition(RaycastDirection raycast)
    {
        // Local ��ǥ �������� ������ �����Ͽ� ���� ��ġ ���
        return transform.position + new Vector3(raycast.offsetZ, 0, 0);
    }

    // Ray�� ������ ����ϴ� �޼��� (Local ��ǥ�� ����)
    private Vector3 GetDirection(DirectionType directionType)
    {
        switch (directionType)
        {
            case DirectionType.Forward:
                return transform.forward; // �̹� Local ��ǥ�踦 �������� ����
            case DirectionType.Backward:
                return -transform.forward;
            case DirectionType.Right:
                return transform.right;
            case DirectionType.Left:
                return -transform.right;
            default:
                return Vector3.forward;
        }
    }

    // Raycast�� �����ϰ� ����� �����ϴ� �޼���
    public bool PerformRaycasts()
    {
        bool allClear = true;

        foreach (var raycast in raycastDirections)
        {
            Vector3 direction = GetDirection(raycast.directionType);

            // Ray���� ���������� offsetZ�� �����Ͽ� ���� ��ġ�� ���� (Local ��ǥ ����)
            Vector3 worldPosition = GetRayStartPosition(raycast);

            bool hitWall = CastRay(worldPosition, direction, raycast.length);

            // ���� Ray���� �±׸� ������Ʈ
            UpdateTag(worldPosition, direction, raycast.length, hitWall);

            if (!hitWall) allClear = false;
        }

        return allClear;
    }

    // �浹 ��ü�� TilePlaced�̸� �����ϰ�, �ٸ� ��ü�� �±׸� ������Ʈ�ϴ� �޼���
    public void UpdateTag(Vector3 startPosition, Vector3 direction, float length, bool hitWall)
    {
        if (!hitWall)
        {
            Ray ray = new Ray(startPosition, direction);

            // RaycastAll�� ����Ͽ� �浹�� ��� ��ü�� ������
            RaycastHit[] hits = Physics.RaycastAll(ray, length);

            foreach (var hit in hits)
            {
                // �浹�� ��ü�� "TilePlaced" �±׸� ���� ��� ����
                if (hit.transform.CompareTag("TilePlaced"))
                {
                    // ���� ��ü�� Ž��
                    continue;
                }

                // ù ��°�� "TilePlaced"�� �ƴ� ��ü�� �±׸� ������Ʈ
                Debug.Log($"Ray hit object: {hit.transform.gameObject.name} at position: {hit.point}");
                hit.transform.gameObject.tag = "TilePlaceable"; // �±׸� ������Ʈ

                // ù ��° ������ ��ü�� ���� �۾��� ������ �� ���� Ż��
                break;
            }
        }
    }

    // Raycast�� ���, Wall�� �浹 �� false�� ��ȯ�ϴ� �޼���
    private bool CastRay(Vector3 origin, Vector3 direction, float length)
    {
        Ray ray = new Ray(origin, direction); // ������ ����� origin���� Ray �߻�
        RaycastHit hit;

        // Ray�� �����̵� �浹���� �� ������ ���
        if (Physics.Raycast(ray, out hit, length))
        {
            Debug.Log($"Ray hit: {hit.collider.name} at position {hit.point} on layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            // �浹�� ��ü�� �� ���̾ ������ Ȯ��
            if (((1 << hit.collider.gameObject.layer) & wallLayer) != 0)
            {
                // ���� �浹���� ��� true ��ȯ
                return true;
            }
            else
            {
                // ���� �ƴ� �ٸ� ��ü�� �浹�� ��쿡�� ������ ����ϰ� false ��ȯ
                Debug.Log($"Ray hit non-wall object: {hit.collider.gameObject.name} at position {hit.point}");
                return false;
            }
        }

        return true; // �ƹ� �浹�� ���� ��� true ��ȯ (���� ���� ��ֹ��� ���ٴ� �ǹ�)
    }

    // Gizmos�� �׷� Scene �信�� Raycast�� �ð�ȭ (Local ��ǥ ����)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var raycast in raycastDirections)
        {
            Vector3 direction = GetDirection(raycast.directionType);

            // Ray���� ���������� offsetZ�� �����Ͽ� ���� ��ġ�� ���� (Local ��ǥ ����)
            Vector3 origin = GetRayStartPosition(raycast);

            // Gizmo�� Ray �ð�ȭ
            Gizmos.DrawRay(origin, direction.normalized * raycast.length);
        }
    }
}
