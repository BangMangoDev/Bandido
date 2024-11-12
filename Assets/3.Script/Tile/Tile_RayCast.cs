using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile_RayCast : MonoBehaviour
{
    [System.Serializable]
    public class RaycastDirection
    {
        public DirectionType directionType; // Raycast의 방향 타입
        public float length; // Raycast의 길이 (기본값 1)
        public float offsetZ; // Z축 오프셋 (Right/Left 방향에 사용)
    }

    public enum DirectionType
    {
        Forward,
        Backward,
        Right,
        Left
    }

    public List<RaycastDirection> raycastDirections = new List<RaycastDirection>(); // Raycast 방향 리스트
    public LayerMask wallLayer; // Wall 레이어 마스크 설정

    // Ray의 시작 위치를 계산하는 메서드 (Local 좌표계 적용)
    private Vector3 GetRayStartPosition(RaycastDirection raycast)
    {
        // Local 좌표 기준으로 오프셋 적용하여 시작 위치 계산
        return transform.position + new Vector3(raycast.offsetZ, 0, 0);
    }

    // Ray의 방향을 계산하는 메서드 (Local 좌표계 적용)
    private Vector3 GetDirection(DirectionType directionType)
    {
        switch (directionType)
        {
            case DirectionType.Forward:
                return transform.forward; // 이미 Local 좌표계를 기준으로 계산됨
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

    // Raycast를 실행하고 결과를 갱신하는 메서드
    public bool PerformRaycasts()
    {
        bool allClear = true;

        foreach (var raycast in raycastDirections)
        {
            Vector3 direction = GetDirection(raycast.directionType);

            // Ray마다 개별적으로 offsetZ를 적용하여 시작 위치를 설정 (Local 좌표 기준)
            Vector3 worldPosition = GetRayStartPosition(raycast);

            bool hitWall = CastRay(worldPosition, direction, raycast.length);

            // 개별 Ray마다 태그를 업데이트
            UpdateTag(worldPosition, direction, raycast.length, hitWall);

            if (!hitWall) allClear = false;
        }

        return allClear;
    }

    // 충돌 객체가 TilePlaced이면 무시하고, 다른 객체에 태그를 업데이트하는 메서드
    public void UpdateTag(Vector3 startPosition, Vector3 direction, float length, bool hitWall)
    {
        if (!hitWall)
        {
            Ray ray = new Ray(startPosition, direction);

            // RaycastAll을 사용하여 충돌한 모든 객체를 가져옴
            RaycastHit[] hits = Physics.RaycastAll(ray, length);

            foreach (var hit in hits)
            {
                // 충돌한 객체가 "TilePlaced" 태그를 가진 경우 무시
                if (hit.transform.CompareTag("TilePlaced"))
                {
                    // 다음 객체를 탐색
                    continue;
                }

                // 첫 번째로 "TilePlaced"가 아닌 객체에 태그를 업데이트
                Debug.Log($"Ray hit object: {hit.transform.gameObject.name} at position: {hit.point}");
                hit.transform.gameObject.tag = "TilePlaceable"; // 태그를 업데이트

                // 첫 번째 적합한 객체에 대해 작업을 수행한 후 루프 탈출
                break;
            }
        }
    }

    // Raycast를 쏘고, Wall과 충돌 시 false를 반환하는 메서드
    private bool CastRay(Vector3 origin, Vector3 direction, float length)
    {
        Ray ray = new Ray(origin, direction); // 오프셋 적용된 origin에서 Ray 발사
        RaycastHit hit;

        // Ray가 무엇이든 충돌했을 때 정보를 출력
        if (Physics.Raycast(ray, out hit, length))
        {
            Debug.Log($"Ray hit: {hit.collider.name} at position {hit.point} on layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

            // 충돌한 객체가 벽 레이어에 속한지 확인
            if (((1 << hit.collider.gameObject.layer) & wallLayer) != 0)
            {
                // 벽에 충돌했을 경우 true 반환
                return true;
            }
            else
            {
                // 벽이 아닌 다른 객체에 충돌한 경우에도 정보를 출력하고 false 반환
                Debug.Log($"Ray hit non-wall object: {hit.collider.gameObject.name} at position {hit.point}");
                return false;
            }
        }

        return true; // 아무 충돌이 없을 경우 true 반환 (벽과 같은 장애물이 없다는 의미)
    }

    // Gizmos를 그려 Scene 뷰에서 Raycast를 시각화 (Local 좌표 기준)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        foreach (var raycast in raycastDirections)
        {
            Vector3 direction = GetDirection(raycast.directionType);

            // Ray마다 개별적으로 offsetZ를 적용하여 시작 위치를 설정 (Local 좌표 기준)
            Vector3 origin = GetRayStartPosition(raycast);

            // Gizmo로 Ray 시각화
            Gizmos.DrawRay(origin, direction.normalized * raycast.length);
        }
    }
}
