using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BIgFishAI : MonoBehaviour
{
    #region 변수
    public float speed = 0.1f; // 이동속도

    public float rotationSpeed = 3f; //회전속도

    public float CheckRange = 3f;

    public Transform BehaviorArea;

    Vector3 goalPos = new Vector3(); //그룹이 가지는 목표지점
    
    #endregion

    void OnEnable()
    {
        StartCoroutine(IDENTITY());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    //방향 회전함수
    #region TurningDir

    void TurningDir(Vector3 _direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(_direction),
                                                     rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region SetGoalPos
    void SetGoalPos()
    {
        float size = BehaviorArea.lossyScale.x / 2;
        goalPos.x = BehaviorArea.transform.position.x + Random.Range(-size, size);

        size = BehaviorArea.lossyScale.y / 2;
        goalPos.y = BehaviorArea.transform.position.y + Random.Range(-size, size);

        size = BehaviorArea.lossyScale.z / 2;
        goalPos.z = BehaviorArea.transform.position.z + Random.Range(-size, size);
    }

    #endregion 

    #region Fish_STATE 함수 모음

    #region IDENTITY
    IEnumerator IDENTITY()
    {
        SetGoalPos();

        while (true)
        {
            if( (goalPos - transform.position).sqrMagnitude <= CheckRange * CheckRange)
            {
                SetGoalPos();
            }

            TurningDir(goalPos - transform.position);

            yield return null;
        }
    }
    #endregion IDENTITY

    #endregion Fish_STATE 함수 모음
}
