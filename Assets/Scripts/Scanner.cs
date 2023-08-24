using UnityEngine;
using System.Collections.Generic;
public class Scanner : MonoBehaviour
{
    // ����, ���̾�, ��ĵ ��� �迭, ���� ����� Ÿ��
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform[] nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll : ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        // CircleCastAll(ĳ���� ���� ��ġ, ���� ������, ĳ���� ����, ĳ���� ����, ��� ���̾�)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    // ���� ����� Enemy Transform ���� ��ȯ
    private Transform[] GetNearest()
    {
        Transform[] result = new Transform[GameManager.instance.statManager.weaponNum]; // �߻��ϴ� źȯ�� ���� ��ŭ �ʱ�ȸ
        List<Transform> uniqueTargets = new List<Transform>(); // �̹� Target���� ������ Enemy ����Ʈ

        for (int i = 0; i < GameManager.instance.statManager.weaponNum; i++)
        {
            float closestDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (RaycastHit2D target in targets)
            {
                if (uniqueTargets.Contains(target.transform)) // �ߺ� ����
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
