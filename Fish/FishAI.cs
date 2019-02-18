using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FishAI : MonoBehaviour
{
    #region 변수
    [HideInInspector] public float speed = 5f; // 이동속도

    float rotationSpeed  //회전속도
    {
        get
        {
            switch (CUR_STATE)
            {
                case Fish_STATE.FLOCKING_IDENTITY: return 2f;
                case Fish_STATE.CALL: return 3f;
                case Fish_STATE.PATH_FOLLOWING: return 5f;
                case Fish_STATE.AVOID_PLAYER: return 4f;
                case Fish_STATE.SET_RANDPOS: return 3f;
                case Fish_STATE.PATH_FOLLOWING_RAND: return 2f;
                case Fish_STATE.CHASHING_GROUP: return 2f;
                case Fish_STATE.Ending: return 5f;
                default: return 4f;
            }
        }
    }

    static float checkRange = 5f; //경로 도달 체크 범위 - 원형

    //static float BehaviourRange = 100f; //행동범위
    //static float Distraction = 200f; //산만도

    int curNum = 0; // 현재 경로 카운트
    //bool isGroup = false; //그룹원인지

    Vector3 goalPos = new Vector3(); //그룹이 가지는 목표지점

    [HideInInspector] FishGroupAI parentAI; //부모 AI 컨포넌트

    public enum Fish_STATE { FLOCKING_IDENTITY, CALL, PATH_FOLLOWING, AVOID_PLAYER, SET_RANDPOS, PATH_FOLLOWING_RAND, CHASHING_GROUP, PATH_FOLLOWING_WARF ,Ending}

    /* [HideInInspector] */
    public Fish_STATE CUR_STATE;
    Fish_STATE Buffer_STATE;

    public delegate IEnumerator MonoBehaviour_Selector();
    public MonoBehaviour_Selector selector;
    float FlowTime = 0f;
    WaitForSeconds wait = new WaitForSeconds(0.02f);
    #endregion

    void Start()
    {
        parentAI = transform.parent.GetComponent<FishGroupAI>();

        GetComponent<Animation>()["Motion"].speed = Random.Range(0.3f, 1f);

        GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        Buffer_STATE = Fish_STATE.PATH_FOLLOWING;

        selector = PATH_FOLLOWING;

        StartCoroutine(selector());
    }

    private void Update()
    {
        CheckState();

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    //상태 체크 머신 이상하면 아니면 외부에서 불러주면된다
    #region CheckState

    void CheckState()
    {
        if (CUR_STATE != Buffer_STATE)
        {
            curNum = 0;

            StopAllCoroutines();

            selector = null;

            switch (CUR_STATE)
            {
                //case Fish_STATE.FLOCKING_IDENTITY:      selector = FLOCKING_IDENTITY; break;
                case Fish_STATE.CALL: selector = CALL; break;
                case Fish_STATE.PATH_FOLLOWING: selector = PATH_FOLLOWING; break;
                case Fish_STATE.AVOID_PLAYER: selector = AVOID_PLAYER; break;
                //case Fish_STATE.SET_RANDPOS:            selector = SET_RANDPOS; break;
                case Fish_STATE.PATH_FOLLOWING_RAND: selector = PATH_FOLLOWING_RAND; break;
                case Fish_STATE.CHASHING_GROUP: selector = CHASHING_GROUP; break;
                case Fish_STATE.PATH_FOLLOWING_WARF: selector = PATH_FOLLOWING_WARF; break;
                case Fish_STATE.Ending: selector = Ending; break;
                default: break;
            }
            Buffer_STATE = CUR_STATE;

            StartCoroutine(selector());
        }
    }

    #endregion

    //방향 회전함수
    #region TurningDir

    void TurningDir(Vector3 _direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                     Quaternion.LookRotation(_direction),
                                                     rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Fish_STATE 함수 모음

    #region FLOCKING_IDENTITY
    //IEnumerator FLOCKING_IDENTITY()
    //{
    //    while (true)
    //    {
    //        if (Vector3.Distance(transform.position, parentAI.Center) > parentAI.FriendDist) // 무리랑 멀어졌으면
    //            isGroup = false;
    //        else
    //            isGroup = true;
    //        if (!isGroup) // 쫒아 가야한다
    //        {
    //            TurningDir(parentAI.Center - transform.position);
    //            speed = 3;
    //        }
    //        else //그룹에 속해있다면
    //        {
    //            TurningDir(parentAI.G_goalPos - transform.position);
    //            speed = parentAI.gSpeed;
    //        }
    //        yield return;
    //    }
    //}
    #endregion FLOCKING_IDENTITY

    #region CALL

    IEnumerator CALL()
    {
        Vector3 playerPos = new Vector3();
        while (true)
        {
            playerPos = GameManager.Instance.player.transform.position + Random.insideUnitSphere * 5f;

            TurningDir(playerPos - transform.position); //플레이어 쪽으로 방향틀기

            FlowTime += Time.deltaTime;
            if(FlowTime >= 3.0f)
            {
                FlowTime = 0f;
                CUR_STATE = Fish_STATE.AVOID_PLAYER;
            }

             yield return null;
        }
    }
    #endregion CALL

    #region PATH_FOLLOWING

    IEnumerator PATH_FOLLOWING()
    {
        goalPos = parentAI.path[0].position;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - goalPos) < checkRange * checkRange)
            {
                goalPos = parentAI.path[++curNum % parentAI.path.Count].position + Random.insideUnitSphere * checkRange; //다음 목표지점 설정
            }

            TurningDir(goalPos - transform.position);

            speed = Random.Range(4f, 5f);

             yield return null;
        }
    }

    #endregion PATH_FOLLOWING

    #region AVOID_PLAYER

    IEnumerator AVOID_PLAYER()
    {
        speed = 5f;
        while (true)
        {
            TurningDir(transform.position - GameManager.Instance.player.transform.position); //플레이어와 반대방향으로 틀기

            FlowTime += Time.deltaTime;
            if (FlowTime >= 1.0f)
            {
                FlowTime = 0f;
                CUR_STATE = Fish_STATE.PATH_FOLLOWING_RAND;
            }

             yield return null;  
        }
    }

    #endregion AVOID_PLAYER

    #region SET_RANDPOS

    //IEnumerator SET_RANDPOS()
    //{
    //    while (true)
    //    {
    //        if (Random.Range(0, Distraction) < 1 || Vector3.SqrMagnitude(transform.position - goalPos) < checkRange) //목표 지점에 어느정도 도달했거나 랜덤값 당첨이라면 목표지점 갱신
    //        {
    //            goalPos = transform.parent.position + Random.insideUnitSphere * BehaviourRange; //부모에서 반경 몇 정도로 움직임
    //        }

    //        speed = 3f;

    //        yield return;
    //    }
    //}

    #endregion SET_RANDPOS

    #region PATH_FOLLOWING_RAND

    IEnumerator PATH_FOLLOWING_RAND()
    {
        goalPos = parentAI.path_shuffle[0].position;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - goalPos) < checkRange)
            {
                goalPos = transform.parent.GetComponent<FishGroupAI>()
                    .path_shuffle[++curNum % transform.parent.GetComponent<FishGroupAI>().path_shuffle.Count]
                    .position + Random.insideUnitSphere * checkRange; //다음 목표지점 설정
            }

            TurningDir(goalPos - transform.position);

            speed = Random.Range(4.5f, 5f);

             yield return null;
        }
    }

    #endregion PATH_FOLLOWING

    #region CHASHING_GROUP
    IEnumerator CHASHING_GROUP()
    {
        while (true)
        {
            TurningDir(parentAI.transform.position - transform.position);

            if ((parentAI.transform.position - transform.position).sqrMagnitude <= parentAI.FriendDist)
            {
                CUR_STATE = Fish_STATE.PATH_FOLLOWING;
            }

             yield return null;
        }
    }
    #endregion CHASHING_GROUP

    #region PATH_FOLLOWING_WARF

    IEnumerator PATH_FOLLOWING_WARF()
    {
        goalPos = parentAI.path[0].position + Random.insideUnitSphere * 50;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - goalPos) < checkRange)
            {
                goalPos = parentAI.path[++curNum % parentAI.path.Count].position + Random.insideUnitSphere * 50; //다음 목표지점 설정

                if(curNum == parentAI.path.Count)
                    yield break;
            }

            TurningDir(goalPos - transform.position);

            speed = 7f;

             yield return null;
        }
    }

    #endregion PATH_FOLLOWING_WARF


    #region PATH_FOLLOWING

    IEnumerator Ending()
    {
        goalPos = parentAI.path[0].position;

        while (true)
        {
            if (Vector3.SqrMagnitude(transform.position - goalPos) < 7f * 7f)
            {
                goalPos = parentAI.path[++curNum % parentAI.path.Count].position + Random.insideUnitSphere * 7f; //다음 목표지점 설정
            }

            TurningDir(goalPos - transform.position);

            speed = Random.Range(8f, 10f);

             yield return null;
        }
    }

    #endregion PATH_FOLLOWING

    #endregion Fish_STATE 함수 모음
}
