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

    private void Start()
    {
        sc = this;
        PlayerStatSet();
    }

    public void GoEducationLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void GoWorkScene()
    {
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
}
