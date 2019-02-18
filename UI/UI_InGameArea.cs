using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
public class UI_InGameArea : MonoBehaviour
{

    [HideInInspector] public float SP;
    [HideInInspector] public float CP;
    [HideInInspector] public float CY;

    public UI_OptionArea optionArea;
    GameSettings gameSettings;
    Player player;
    JsonData Local = new JsonData();

    private void Start()
    {
        Local = GameManager.Instance.LocalizationData;
        gameSettings = GameManager.Instance.gameSettings;
        player = GameManager.Instance.player;

        player.SP = GameManager.Instance.gameSettings.swimPitch;
        player.CP = GameManager.Instance.gameSettings.cameraPitch;
        player.CY = GameManager.Instance.gameSettings.cameraYaw;

        OptionApply_Default();
    }

    public void OptionApply_Default()
    {
        UIManager.Instance.FindContents<Text>("swimptch_text").text = Local["swimptch_text"][optionArea.Curword][player.SP ? 1 : 0].ToString();
        UIManager.Instance.FindContents<Text>("camerapitch_text").text = Local["camerapitch_text"][optionArea.Curword][player.CP ? 1 : 0].ToString();
        UIManager.Instance.FindContents<Text>("camerayaw_text").text = Local["camerayaw_text"][optionArea.Curword][player.CY ? 1 : 0].ToString();
    }

    #region UI_InGameArea

    public void Resume()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            UIManager.Instance.Toggle(UIContents.Canvas);
            gameSettings.SaveOption(gameSettings);
        }
    }

    public void SwimPitch()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            player.SP = GameManager.Instance.gameSettings.swimPitch = !player.SP;
            UIManager.Instance.FindContents<Text>("swimptch_text").text = Local["swimptch_text"][optionArea.Curword][player.SP ? 1:0].ToString();
            GameManager.Instance.gameSettings.SaveOption(gameSettings);
        }
    }

    public void CameraPitch()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            player.CP = GameManager.Instance.gameSettings.cameraPitch = !player.CP;
            UIManager.Instance.FindContents<Text>("camerapitch_text").text = Local["camerapitch_text"][optionArea.Curword][player.CP ? 1 : 0].ToString();
            GameManager.Instance.gameSettings.SaveOption(gameSettings);
        }
    }

    public void CameraYaw()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            player.CY = GameManager.Instance.gameSettings.cameraYaw = !player.CY;
            UIManager.Instance.FindContents<Text>("camerayaw_text").text = Local["camerayaw_text"][optionArea.Curword][player.CY ? 1 : 0].ToString();
            GameManager.Instance.gameSettings.SaveOption(gameSettings);
        }
    }

    public void MainMenu()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            GameManager.Instance.gameSettings.SaveOption(gameSettings);

            UIManager.Instance.isplay = false;

            UIManager.Instance.pikaRender.enabled = false;

            Camera.main.transform.parent.position += player.transform.rotation * new Vector3(0f, 7f, -1f);

            UIManager.Instance.Toggle(UIContents.InGame);
            UIManager.Instance.Toggle(UIContents.MenuArea);
        }
    }

    #endregion
}
     