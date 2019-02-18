using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_MenuArea : MonoBehaviour
{
    GameObject YesNO;

    private void Start()
    {
        YesNO = UIManager.Instance.YesNO;
    }
    public void CONTINUE()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            UIManager.Instance.isplay = true;
            UIManager.Instance.isNew = false;

            GameManager.Instance.player.enabled = false;

            UIManager.Instance.Toggle(UIContents.Canvas);
            UIManager.Instance.pikaRender.enabled = true;
            if (UIManager.Instance.stPlayer != null)
            {
                UIManager.Instance.stPlayer.CamAct();
            }
        }
    }
    public void NEWGAME()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            UIManager.Instance.isplay = true;
            UIManager.Instance.isNew = true;

            SceneManager.LoadScene("NewGameOpening");
        }
    }
    public void OPTION()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            UIManager.Instance.Toggle(UIContents.Option);
            UIManager.Instance.Toggle(UIContents.MenuArea);
        }
    }
    public void EXIT()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            YesNO.SetActive(true);
            UIManager.Instance.SetButton("No_Button");
        }
    }
    public void YES()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            Application.Quit();
        }
    }
    public void NO()
    {
        if (UIManager.Instance.canvasGroup.alpha > 0.1f)
        {
            UIManager.Instance.SetButton("Exit_Button");
            YesNO.SetActive(false);
        }
    }
}
