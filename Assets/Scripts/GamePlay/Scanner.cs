using UnityEngine;
using System.Collections.Generic;
public class Scanner : MonoBehaviour
{
    // 범위, 레이어, 스캔 결과 배열, 가장 가까운 타겟
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform[] nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        // CircleCastAll(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    // 가장 가까운 Enemy Transform 정보 반환
    private Transform[] GetNearest()
    {
        Transform[] result = new Transform[GameManager.instance.statManager.weaponNum]; // 발사하는 탄환의 갯수 만큼 초기회
        List<Transform> uniqueTargets = new List<Transform>(); // 이미 Target으로 설정된 Enemy 리스트

        for (int i = 0; i < GameManager.instance.statManager.weaponNum; i++)
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (RaycastHit2D target in targets)
            {
                if (uniqueTargets.Contains(target.transform)) // 중복 여부
                {
                    continue;
                }

                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            if (closestTarget != null)
            {
                uniqueTargets.Add(closestTarget);
                result[i] = closestTarget;
            }

            if(targets.Length == i + 1)
            {
                break;
            }
        }

        return result;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
