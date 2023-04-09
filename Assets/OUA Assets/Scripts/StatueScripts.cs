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
    }

    public void GoEducationLevel(string levelName)
    {
        //SceneManager.LoadScene(levelName);
        GameManager.SetInt(levelName, GameManager.GetInt("code", 1) + 1);
    }
    public void GoWorkScene()
    {
        GameManager.gm.PlayerStatSet();
        GameManager.gm.ResetLevel();

        statueScene.DOBlendableLocalMoveBy(new(0, -1080), 0.6f);
        workScene.DOBlendableLocalMoveBy(new(0, -1080), 0.6f);
    }

    public void PlayerStatSet()
    {
        codeStat.text = GameManager.GetInt("code", 1).ToString() + "   Yaz�l�m G�c�";
        drawStat.text = GameManager.GetInt("draw", 1).ToString() + "   Sanat Tasar�m� G�c�";
        pmStat.text = GameManager.GetInt("pm", 1).ToString() + "   Proje Y�netimi G�c�";
    }
}
