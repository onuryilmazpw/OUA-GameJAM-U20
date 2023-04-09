using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class StatueScripts : MonoBehaviour
{
    public static StatueScripts sc;
    public Transform workScene, statueScene;

    [Header("Stats Texts")]
    public TextMeshProUGUI codeStat;
    public TextMeshProUGUI drawStat, pmStat;

    [Header("StartScene")]
    public CanvasGroup startStory;


    private void Start()
    {
        if (PlayerPrefs.GetInt("firstOpen") == 0)
        {
            PlayerPrefs.SetInt("firstOpen", 1);
        }
        else
        {
            startStory.GetComponent<RectTransform>().DOScale(0, 0);
        }


        sc = this;
        PlayerStatSet();
    }

    public void GoEducationLevel(string levelName)
    {
        AudioManager.instance.PlaySound("NormalBtn");
        SceneManager.LoadScene(levelName);
    }
    public void GoWorkScene()
    {
        AudioManager.instance.PlaySound("NormalBtn");
        AudioManager.instance.PlaySound("Fight");
        AudioManager.instance.StopSound("Education");

        GameManager.gm.ResetLevel();
        GameManager.gm.PlayerStatSet();

        statueScene.DOBlendableLocalMoveBy(new(0, -1080), 0.6f);
        workScene.DOBlendableLocalMoveBy(new(0, -1080), 0.6f);
    }
    public void PlayerStatSet()
    {
        codeStat.text = GameManager.GetInt("code", 1).ToString() + "   Yazýlým Gücü";
        drawStat.text = GameManager.GetInt("draw", 1).ToString() + "   Sanat Tasarýmý Gücü";
        pmStat.text = GameManager.GetInt("pm", 1).ToString() + "   Proje Yönetimi Gücü";
    }

    public void StartPanelClose()
    {
        AudioManager.instance.PlaySound("NormalBtn");

        startStory.DOFade(0, 1);
        startStory.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(1f);
    }
}
