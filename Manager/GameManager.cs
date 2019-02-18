using System.Collections.Generic;
using UnityEngine;
using LitJson;


// 특정 지역 갈때마다 오브젝트 켜주고 꺼주기
// 게임 도착 지점 오면 크레딧 장소로 넘어가기
// 게임 시작과 끝을 알림
// 아 자고싶다 슈밤
// 

public class GameManager : MonoBehaviour
{

    #region Singleton
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (ReferenceEquals(_instance, null))
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LocalizationData = JsonMapper.ToObject(UI_localDataPath.text);

        gameSettings = gameSettings.LoadOption();

        if (ReferenceEquals(gameSettings, null))
        {
            gameSettings = new GameSettings();
        }

        gameSettings.SaveOption(gameSettings);
    }
    #endregion Singleton

    #region 변수

    public TextAsset UI_localDataPath;
    [HideInInspector] public JsonData LocalizationData;
    [HideInInspector] public GameSettings gameSettings;

    public Player player;
    public GameObject Camerapivot;

    public List<Material> FishMaterials = new List<Material>();
    public List<Shader> FishShader = new List<Shader>();

    [HideInInspector] public enum Stage { St1, St2, St3, St4, St5 }
    public Material mainskybox;
    public Material templeskybox;

    [HideInInspector] public GameObject Object1;
    [HideInInspector] public GameObject Object2;
    [HideInInspector] public GameObject Object3;
    [HideInInspector] public GameObject Object4;
    [HideInInspector] public GameObject Object5;

    #endregion

    public void ChangeStage(Stage stage)
    {
        switch (stage)
        {
            case Stage.St1: Object1.SetActive(true); Object2.SetActive(true); break;

            case Stage.St2: Object1.SetActive(false); Object3.SetActive(true); break;

            case Stage.St3: Object2.SetActive(false); Object4.SetActive(true); break;

            case Stage.St4: Object3.SetActive(false); Object5.SetActive(true); break;

            case Stage.St5:
                Object4.SetActive(false);
                player.enabled = false;
                UIManager.Instance.isplay = false;

                Camerapivot.transform.parent = player.gameObject.transform;

                player.transform.position = GameObject.Find("FinalePos").transform.position;
                player.transform.rotation = GameObject.Find("FinalePos").transform.rotation;
                player.GetComponent<EndingCreditMoving>().enabled = true;

                break;

            default: break;
        }
    }

    public void ChangeSkybox()
    {
        RenderSettings.skybox = (RenderSettings.skybox != mainskybox) ? mainskybox : templeskybox;
    }
}
