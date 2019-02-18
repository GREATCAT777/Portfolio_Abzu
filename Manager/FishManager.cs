using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishManager : MonoBehaviour {
    #region Singleton
    public static FishManager _instance = null;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SmallFish = Resources.LoadAll<GameObject>(path_S);
    }
    #endregion

    #region Fishes //스테이지별 물고기들의 그룹 카운트를 테이블로 관리한다 --> 테이블 만들어야지 슈발
    public Transform player;
    GameObject[] SmallFish; //
    public string path_S;       //이름 경로
    public int PoolCount_S; // 풀링할 마릿수
    public int GroupCount_S; //그룹당 마릿수

    public List<Transform> path = new List<Transform>(); //물고기들이 이벤트 할 경로

    Dictionary<string, List<GameObject>> Fishes = new Dictionary<string, List<GameObject>>(); //물고기 도감
    #endregion

    [HideInInspector]public List<GameObject> CurFishGroups = new List<GameObject>(); //현재 씬에 돌아다니는 물고기 그룹들
    
    private void Start() //스테이지 변경될때 로딩하기
    {
        // 풀카운트 넘길떄 json 파일로 변경후 인자로 넣어주기
        FishPool_Grouping(SmallFish, PoolCount_S, GroupCount_S);
        //SetPath();
    }

    void FishPool_Grouping(GameObject[] prefabArr,int Count,int GroupCount) //스테이지 바뀌는 로직에서 선언해줄녀석 --> 스테이지별 그룹핑을 해라
    {
        for (int i = 0; i < prefabArr.Length; ++i)
        {
            Pooler._instance.Pooling(Fishes, prefabArr[i].name, prefabArr[i], Count,false);// 저장할 책장, 키값, 프리팹, 풀링 카운트
        }

        Grouping(prefabArr, GroupCount);
    }

    public void Grouping(GameObject[] prefabArr, int GroupCount) //그룹 만들기
    {
        for (int i = 0; i < prefabArr.Length; i++) //종류마다 그룹핑
        {
            int namecount = 0;
            for (int j = 0; j < Fishes[prefabArr[i].name].Count ; j+= GroupCount)// 한종류당 원하는 만큼 그룹핑
            {
                GameObject Group = new GameObject(prefabArr[i].name + namecount.ToString("00")); // 그룹생성
                FishGroupAI G_AI = Group.AddComponent<FishGroupAI>();

                Vector3 Wolrdpos = path[(i + j) / path.Count].transform.position + Random.insideUnitSphere * 20;

                for (int k = 0; k < GroupCount; k++) // 그룹묶기
                {
                    if ( j + k < Fishes[prefabArr[i].name].Count) //한 그룹의 물고기의 마릿수가 최대 마리수보다 적으면 그룹핑 
                    {
                        Fishes[prefabArr[i].name]                       [j + k].transform.SetParent(Group.transform); //부모 설정
                        G_AI.Children.Add(Fishes[prefabArr[i].name]     [j + k].transform); // 그룹AI 리스트에 등록
                        Fishes[prefabArr[i].name]                       [j + k].SetActive(true); // 활성화
                    }
                }

                G_AI.InitPos(Wolrdpos); //호출 순서의 문제때문에 (위에서 GroupAI컴포넌트를 등록한뒤로 Start또는 WakeUp에서 초기화가 불가능) public함수로 초기화 시켜준다
                // 또한 그룹당 월드좌표값을 넘겨줘야해서 vector3.zero 대신에 지정한 월드좌표를 넘겨줘야한다
                CurFishGroups.Add(Group);
                namecount++;
            }
        }
    }

    public void DeGrouping() //그룹 해제
    {
        for (int i = 0; i < CurFishGroups.Count;  i++)
        {
            foreach (var item in CurFishGroups[i].GetComponentsInChildren<Transform>())
                item.SetParent(null); //그룹부모설정 해제

            Destroy(CurFishGroups[i].gameObject); //빈오브젝트 파괴
        }

        CurFishGroups.Clear(); //현재 목록 초기화
    }

    void SetPath()
    {
        int i = 0;
        foreach(GameObject group in CurFishGroups)
        {
            group.GetComponent<FishGroupAI>().InitPos(path[i % path.Count].position);
            i++;
        }
    }
}
