using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGroupAI : MonoBehaviour
{
    #region 외부 링크 변수
    public List<Transform> Children = new List<Transform>(); //그룹 아이들
    static float DenseRadius = 3.5f; //초기 밀집 계수 --> 추후에도 간격띄워서 이동시켜야함

    public readonly float FriendDist = 4f; // 떨어지지않게 유지시켜주는 친구끼리의 거리
    [HideInInspector] public float gSpeed = 0.1f; // 그룹 평균 속도

    public List<Transform> path = new List<Transform>(); //물고기들이 이벤트 할 경로
    public FishAI.Fish_STATE CurGroupState = FishAI.Fish_STATE.PATH_FOLLOWING;

    [HideInInspector] public List<Transform> path_shuffle = new List<Transform>(); // 랜덤 경로
    [HideInInspector] public Vector3 G_goalPos = Vector3.zero;
    [HideInInspector] public Vector3 Center = Vector3.zero;
    #endregion

    public void InitPos(Vector3 worldpos) // 외부에서 호출
    {
        path = FishManager._instance.path; //이벤트 경로 넘겨주기

        shuffle(); //경로 섞기

        foreach (var item in Children)
        {
            item.position = worldpos + Random.insideUnitSphere * DenseRadius;
            item.GetComponent<Animation>()["Motion"].speed = Random.Range(0.3f, 1f);
        }
    }

    private void Start()// Test
    {
        //StartCoroutine(checkState());
    }

    IEnumerator checkState() //Test
    {
        while (true)
        {
            ComandGroupState(CurGroupState);

            yield return null;
        }
    }

    void shuffle()
    {
        path_shuffle.Clear();

        for (int i = 0; i < path.Count; i++)
        {
            path_shuffle.Add(path[i]); //값 복사
        }

        for (int i = 0; i < path_shuffle.Count; i++) //셔플
        {
            int rand = Random.Range(i, path_shuffle.Count);

            Transform item = path_shuffle[i];

            path_shuffle[i] = path_shuffle[rand];

            path_shuffle[rand] = item;
        }
    }

    void GroupRules()
    {
        //Vector3 Avoid = Vector3.zero;

        float dist = 0; //가비지 컬렉터 때문에 외부에서 선언

        int groupSize = 0;

        foreach (Transform friend in Children) // 친구들이랑 속도 맞추기
        {
            dist = Mathf.Abs((friend.position - transform.position).sqrMagnitude); // 친구들이랑 거리재기
            if (dist <= FriendDist * FriendDist)
            {
                Center += friend.position;
                groupSize++;

                //if (dist < 1.0f) //그룹 정중앙 회피 코드
                //    Avoid += Avoid + (transform.position - friend.position);

                gSpeed += friend.GetComponent<FishAI>().speed / 2;
            }
        }

        if (groupSize > 0) // 그룹이 많아졌다면
        {
            Center = Center / groupSize; //그룹의 중심점 찾기
            gSpeed = gSpeed / groupSize; // 그룹의 평균속도
        }
    }

    void ComandGroupState(FishAI.Fish_STATE _STATE)
    {
        foreach (var item in Children)
        {
            item.GetComponent<FishAI>().CUR_STATE = _STATE;
        }
    }
}
