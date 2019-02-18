using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class UI_OptionArea : MonoBehaviour
{
    #region 변수
    JsonData Local = new JsonData();
    Resolution[] resolutions;
    List<Text> textWaffer = new List<Text>();
    GameSettings gameSettings;

    [HideInInspector] public string Curword;
    #endregion

    private void Start()
    {
        Local = GameManager.Instance.LocalizationData;
        resolutions = Screen.resolutions;
        gameSettings = GameManager.Instance.gameSettings;
        Dictionary<string, RectTransform> inst = UIManager.Instance.Contents;

        foreach (KeyValuePair<string, RectTransform> item in inst)
        {
            Text tx = item.Value.GetComponent<Text>();

            if (ReferenceEquals(tx, null))
            {
                continue;
            }

            textWaffer.Add(tx);
        }

        gameSettings.renderResIndex = resolutions.Length - 1;

        Curword = gameSettings.language;

        LanguageFunc(Curword);
    }

    void OptionApply_Default()
    {
        RenderResFunc("renderres_text", gameSettings.renderResIndex);

        FishFunc("fish_text",gameSettings.fishQuality);

        ShadowFunc("shadows_text",gameSettings.shadows);

        ReflectionsFunc("reflections_text",gameSettings.reflections);

        VSyncFunc("vsync_text",gameSettings.vSync);

        FullscreenFunc("fullsreen_text",gameSettings.fullscreen);

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #region UI_OptionArea

    #region RenderRes

    public void RenderRes()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            ++gameSettings.renderResIndex;

            gameSettings.renderResIndex %= resolutions.Length;

            RenderResFunc("renderres_text",gameSettings.renderResIndex); 
        }
    }

    void RenderResFunc(string UI_Name,int Index)
    {
        Resolution resol = resolutions[Index];

        UIManager.Instance.FindContents<Text>(UI_Name).text
            = resol.ToString().Substring(0, resol.ToString().Length - 7); ;

        Screen.SetResolution(resol.width, resol.height, Screen.fullScreen);

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion RenderRes

    #region FishQuality

    public void Fish()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            ++gameSettings.fishQuality;

            gameSettings.fishQuality %= Local["fish_text"][Curword].Count;

            FishFunc("fish_text",gameSettings.fishQuality);
        }
    }

    void FishFunc(string UI_Name, int Index)
    {
        string text = Local[UI_Name][Curword][Index].ToString();

        UIManager.Instance.FindContents<Text>(UI_Name).text = text;

        foreach (Material Mat in GameManager.Instance.FishMaterials)
        {
            Mat.shader = GameManager.Instance.FishShader[Index];
        }

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion FishQuality

    #region Shadows

    public void Shadows()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            ++gameSettings.shadows;

            gameSettings.shadows %= Local["shadows_text"][Curword].Count;

            ShadowFunc("shadows_text",gameSettings.shadows);
        }
    }

    void ShadowFunc(string UI_Name, int Index)
    {
        string text = Local[UI_Name][Curword][Index].ToString();

        UIManager.Instance.FindContents<Text>(UI_Name).text = text;

        QualitySettings.shadowResolution = (ShadowResolution)Index;

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion

    #region Reflections

    public void Reflections()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            ++gameSettings.reflections;

            gameSettings.reflections %= Local["reflections_text"][Curword].Count;

            ReflectionsFunc("reflections_text",gameSettings.reflections);
        }

    }

    void ReflectionsFunc(string UI_Name, int Index)
    {
        string text = Local[UI_Name][Curword][Index].ToString();

        UIManager.Instance.FindContents<Text>(UI_Name).text = text;

        // 비 구현 -->> 쉐이더 더 만들어야함
        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion

    #region VSync

    public void VSync()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            ++gameSettings.vSync;

            gameSettings.vSync %= Local["vsync_text"][Curword].Count;

            VSyncFunc("vsync_text",gameSettings.vSync);
        }
    }

    void VSyncFunc(string UI_Name, int Index)
    {
        string text = Local[UI_Name][Curword][Index].ToString();

        UIManager.Instance.FindContents<Text>(UI_Name).text = text;

        QualitySettings.vSyncCount = Index;

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion

    #region Fullscreen

    public void Fullscreen()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            gameSettings.fullscreen = !gameSettings.fullscreen;

            FullscreenFunc("fullsreen_text",gameSettings.fullscreen);
        }
    }

    void FullscreenFunc(string UI_Name, bool Toggle)
    {
        string text = Local[UI_Name][Curword][Toggle ? 0 : 1].ToString();

        UIManager.Instance.FindContents<Text>(UI_Name).text = text;

        Screen.SetResolution(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            Toggle
            );

        if (Toggle)
        {
            Resolution resol = resolutions[resolutions.Length - 1];

            gameSettings.renderResIndex = resolutions.Length - 1;

            UIManager.Instance.FindContents<Text>("renderres_text").text
                = resol.ToString().Substring(0, resol.ToString().Length - 7);
        }

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion

    #region Language

    public void Language()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            Curword = gameSettings.language = (Curword == "English") ? "한국어" : "English";

            LanguageFunc(Curword);
        }
    }

    void LanguageFunc(string CurSelect)
    {
        // 컨텐츠목록 이름 다바꾸기

        UIManager inst = UIManager.Instance;
        string textName;
        foreach (var item in textWaffer)
        {
            textName = item.name;

            if (Local[textName] != null)
            {
                inst.FindContents<Text>(textName).text = Local[textName][CurSelect].ToString();
            }
        }

        OptionApply_Default();

        GetComponent<UI_InGameArea>().OptionApply_Default();

        GameManager.Instance.gameSettings.SaveOption(gameSettings);
    }

    #endregion

    #region Back
    public void Back()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            GameManager.Instance.gameSettings.SaveOption(gameSettings);

            UIManager.Instance.Toggle(UIContents.Option);
            UIManager.Instance.Toggle(UIContents.MenuArea);
            UIManager.Instance.SetButton("Continue_Button");
        }
    }

    #endregion

    #endregion
}
