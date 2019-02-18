using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Dictionary<string, List<GameObject>>을 기준으로 풀링
 */

public class Pooler : MonoBehaviour {
    #region Singleton
    public static Pooler _instance = null;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = new Pooler();
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    #endregion

    public void Pooling(Dictionary<string, List<GameObject>> Bookshelf, string key, GameObject prefab, int count, bool active = false, Material material = null) //키값으로, 원하는 갯수만큼 리스트로 등록
    {
        if (Bookshelf.ContainsKey(key)) 
        {
            AddObject(Bookshelf, key, count);
            return;
        }          

        List<GameObject> book = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject inst = Instantiate(prefab);
            inst.name = key + i;
            inst.SetActive(active);
            if (material != null)
            {
                inst.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = material;

                Debug.Log(inst.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial);
            }
               
            book.Add(inst);
        }

        Bookshelf.Add(key, book);
    }

    public List<GameObject> GetList(Dictionary<string, List<GameObject>> Bookshelf,string key) //딕셔너리에서 키값으로 리스트 가져오기
    {
        if (Bookshelf.ContainsKey(key))
            return Bookshelf[key];

        return null;
    }

    public void AddObject(Dictionary<string, List<GameObject>> Bookshelf,string key,int count = 1, bool active = false) // 오브젝트 더하기
    {
        if (Bookshelf.ContainsKey(key))
        {
            for (int i = 0; i < count; i++)
            {
                GameObject inst = Instantiate(Bookshelf[key][0]);
                inst.name = key + Bookshelf[key].Count;
                inst.SetActive(active);
                Bookshelf[key].Add(inst);
            }
        }
    }
    
    public void TurnOffObject(Dictionary<string, List<GameObject>> Bookshelf,string key) // 오브젝트 리스트 전부 비활성화
    {
        if (Bookshelf.ContainsKey(key))
        {
            for (int i = 0; i < Bookshelf[key].Count; i++)
            {
                Bookshelf[key][i].SetActive(false);
            }
        }
    }

    public void DeleteObject(Dictionary<string, List<GameObject>> Bookshelf, string key,int Delcount = 0)// 원하는 갯수만큼 오브젝트 파괴 및 레퍼런스 지우기
    {
        if (Bookshelf.ContainsKey(key))
        {
            for (int i = 0; i < Delcount; i++)
            {
                Destroy(Bookshelf[key][Bookshelf[key].Count - 1].gameObject);

                Bookshelf[key].RemoveAt(Bookshelf[key].Count - 1);
            }
        }
    }
}
