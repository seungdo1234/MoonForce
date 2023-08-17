using System.Collections;
using UnityEditor;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public Transform target; // Enemy

    public float rotationSpeed; // 회전 속도
    public float lerpTime; // Target을 향해 무한대 곡선으로 공격할 때 공격 시간


    [Header("# Bezier Curve")]
    [Range(0, 1)]
    public float curveValue;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
    public Vector3 p4;

    [Header("# Bezier Curve")]
    public Vector3 cloneP1;
    public Vector3 cloneP2;
    public Vector3 cloneP3;
    public Vector3 cloneP4;


    private void OnEnable()
    {
        int random = Random.Range(0, GameManager.instance.pool.pools[0].Count);
        target = GameManager.instance.pool.pools[0][random].transform;
        StartCoroutine(ThrowStart(0, 1));
    }


    private void Start()
    {
    }
    public Vector3 BezierCurve(Vector3 p_1, Vector3 p_2, Vector3 p_3, Vector3 p_4, float value)
    {
        Vector3 a = Vector3.Lerp(p_1, p_2, value);
        Vector3 b = Vector3.Lerp(p_2, p_3, value);
        Vector3 c = Vector3.Lerp(p_3, p_4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        Vector3 f = Vector3.Lerp(d, e, value);

        return f;
    }


    private IEnumerator ThrowStart(float start, float end)
    {
        float currentTime = 0f;
        cloneP1 = p1;
        cloneP2 = p2;
        cloneP3 = p3;
        cloneP4 = p4;


        while (true)
        {
            currentTime += Time.deltaTime;

            if (!target.gameObject.activeSelf)
            {
                while(true)
                {
                    int random = Random.Range(0, GameManager.instance.pool.pools[0].Count);

                    if (GameManager.instance.pool.pools[0][random].activeSelf)
                    {
                        target = GameManager.instance.pool.pools[0][random].transform;
                        break;
                    }
                }
            }

            if (currentTime > lerpTime)
            {
                currentTime = 0;
                start = start == 0 ? 1 : 0;
                end = end == 0 ? 1 : 0;

                p2.y *= -1;
                p3.y *= -1;
            }

            // 타겟의 위치를 매 프레임마다 업데이트
            Vector3 targetPosition = target.position;

            cloneP1 = targetPosition + p1;
            cloneP2 = targetPosition + p2;
            cloneP3 = targetPosition + p3;
            cloneP4 = targetPosition + p4;


            curveValue = Mathf.Lerp(start, end, currentTime / lerpTime);


            // 곡선을 따라 움직이는 위치 계산
            Vector3 newPosition = BezierCurve(cloneP1, cloneP2, cloneP3, cloneP4, curveValue);


            transform.position = newPosition;

            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(MagicBall))]
public class Rake_Editor : Editor
{
    private void OnSceneGUI()
    {
        MagicBall rake = (MagicBall)target;

        rake.p1 = Handles.PositionHandle(rake.p1, Quaternion.identity);
        rake.p2 = Handles.PositionHandle(rake.p2, Quaternion.identity);
        rake.p3 = Handles.PositionHandle(rake.p3, Quaternion.identity);
        rake.p4 = Handles.PositionHandle(rake.p4, Quaternion.identity);

        Handles.DrawLine(rake.p1, rake.p2);
        Handles.DrawLine(rake.p3, rake.p4);

        int count = 50;
        for (float i = 0; i < count; i++)
        {
            float value_Before = i / count;
            Vector3 before = rake.BezierCurve(rake.cloneP1, rake.cloneP2, rake.cloneP3, rake.cloneP4, value_Before);

            float value_After = (i + 1) / count;
            Vector3 after = rake.BezierCurve(rake.cloneP1, rake.cloneP2, rake.cloneP3, rake.cloneP4, value_After);

            Handles.DrawLine(before, after);
        }
    }
}