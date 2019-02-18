using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InitScene : MonoBehaviour
{
    public GameObject   MainCanvas;
    public GameObject   inGameAreaUI;
    public GameObject   menuAreaUI;
    public GameObject   optionUI;
    public EventSystem  @event;
    public GameObject   YesNO;
    public Player       player;
    public SkinnedMeshRenderer pi;
    [HideInInspector]public CanvasGroup  canvasGroup;

    public GameObject Camerapivot;

    public GameObject Object1;
    public GameObject Object2;
    public GameObject Object3;
    public GameObject Object4;
    public GameObject Object5;

    void Awake ()
    {
        GameManager G = GameManager.Instance;

        G.player = player;
        G.Camerapivot = Camerapivot;

        G.Object1 = Object1;
        G.Object2 = Object2;
        G.Object3 = Object3;
        G.Object4 = Object4;
        G.Object5 = Object5;

        UIManager i = UIManager.Instance;

        i.MainCanvas = MainCanvas;
        i.inGameAreaUI = inGameAreaUI;
        i.menuAreaUI = menuAreaUI;
        i.optionUI = optionUI;
        i.@event = @event;
        i.YesNO = YesNO;
        i.pikaRender = pi;
        i.canvasGroup = MainCanvas.GetComponent<CanvasGroup>();

        i.Setdictionary();

        // 오프닝 거쳐서 재진입 시 세팅
        if (i.isplay == true && i.isNew == true)
        {
            i.Toggle(UIContents.Canvas);
            i.Toggle(UIContents.MenuArea);
            G.player.enabled = false;
        }

        i.StartCoroutine(i.ButtonHighLightResset());
    }
}
