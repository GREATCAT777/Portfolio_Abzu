using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum UIContents
{
    Canvas,
    InGame,
    MenuArea,
    Option,
    NULL
}

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance = null;

    public static UIManager Instance
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
    }
    #endregion Singleton

    #region 변수
    public GameObject MainCanvas;

    [HideInInspector] public bool isplay = false;
    [HideInInspector] public bool isNew = true;
    public GameObject inGameAreaUI;
    public GameObject menuAreaUI;
    public GameObject optionUI;
    public EventSystem @event;
    public GameObject YesNO;
    public SkinnedMeshRenderer pikaRender;

    public StartGame_Player stPlayer = null;

    [HideInInspector] public CanvasGroup canvasGroup;
    [HideInInspector] public Dictionary<string, RectTransform> Contents = new Dictionary<string, RectTransform>();

    #endregion 변수

    private void Update()
    {
        Cursor.visible = false;

        if (isplay && Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle(UIContents.Canvas);
        }
        
    }

    #region 함수

    public void Toggle(UIContents content)
    {
        switch (content)
        {
            case UIContents.Canvas:

                canvasGroup.alpha = (canvasGroup.alpha == 0) ? 255f : 0f;

                if (canvasGroup.alpha > 0.1)
                {
                    inGameAreaUI.SetActive(false);
                    menuAreaUI.SetActive(false);
                    optionUI.SetActive(false);
                    GameManager.Instance.player.enabled = false;

                    Toggle(isplay ? UIContents.InGame : UIContents.MenuArea);
                }
                else
                {
                    GameManager.Instance.player.enabled = true;
                    YesNO.SetActive(false);
                }
                break;

            case UIContents.InGame:
                inGameAreaUI.SetActive(!inGameAreaUI.activeInHierarchy);

                if (inGameAreaUI.activeInHierarchy)
                    SetButton("InGameNull_Button");
                break;
            case UIContents.MenuArea:
                menuAreaUI.SetActive(!menuAreaUI.activeInHierarchy);

                if (menuAreaUI.activeInHierarchy)
                    SetButton("MenuNull_Button");

                break;
            case UIContents.Option:
                optionUI.SetActive(!optionUI.activeInHierarchy);

                if (optionUI.activeInHierarchy)
                    SetButton("OptionNull_Button");

                break;

            default: break;
        }
    }

    public void Setdictionary()
    {
        List<RectTransform> Contentslist = new List<RectTransform>();

        MainCanvas.GetComponentsInChildren(true, Contentslist);

        Contents.Clear();

        foreach (var item in Contentslist)
        {
            Contents.Add(item.name, item);
        }
    }

    public T FindContents<T>(string _name) where T : class
    {
        if (Contents.ContainsKey(_name))
            return Contents[_name].GetComponent<T>();

        return null;
    }

    public void SetButton(string _name)
    {
        @event.firstSelectedGameObject = FindContents<Button>(_name).gameObject;
        @event.SetSelectedGameObject(@event.firstSelectedGameObject);
    }

    public IEnumerator ButtonHighLightResset()
    {
        while (true)
        {
            if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)))
            {
                if (inGameAreaUI.activeSelf)
                {
                    SetButton("InGameNull_Button");
                }

                else if (menuAreaUI.activeSelf)
                {
                    SetButton("MenuNull_Button");
                }

                else if (optionUI.activeSelf)
                {
                    SetButton("OptionNull_Button");
                }
            }
            yield return null;
        }
    }

    #endregion
}
