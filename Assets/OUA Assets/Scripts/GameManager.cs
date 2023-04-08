using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform player;
    public Button CodeBtn, DrawBtn, PMBtn;
    public TextMeshProUGUI whichLevelTxt;

    Vector2 playerPos;
    GameValues gv;
    int whichlevel, startCodeVal, startDrawVal, startPMVal;                       //bu k�s�m her level ba��nda tekrar ayarlanacak

    [Header("WorkTable Texts")]
    public TextMeshProUGUI remainTourTxt;
    public TextMeshProUGUI codeTxt, drawTxt, pmTxt;

    [Header("Stat Texts")]
    public TextMeshProUGUI codeStat;
    public TextMeshProUGUI drawStat, pmStat;


    [Header("Game Values")]
    int remainTour;                                  //geriye kalan tur say�s�

    void Start()
    {
        gv = GetComponentInParent<GameValues>();
        ResetLevel();

        playerPos = player.position;

        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Say�s�";

        PlayerStatSet();
    }
    void ResetLevel()
    {
        whichlevel = GetInt("whichLevel", 0);
        whichLevelTxt.text = whichlevel.ToString();

        remainTour = gv.levels[whichlevel].remainTourVal;
        startCodeVal = 0;
        startDrawVal = 0;
        startPMVal = 0;
    }

    public void BasicBtnFunc(string whichBtn)
    {
        player.DOBlendablePunchRotation(new(1, 1, 5), 1, 20, 1);        //sondaki 10 ve 1 de�erleri defaault de�erler
        player.DOBlendableMoveBy(new(-120, 0), 0.06f);
        player.DOBlendableMoveBy(new(120, 0), 0.2f).SetDelay(0.3f);

        CodeBtn.interactable = false;
        DrawBtn.interactable = false;
        PMBtn.interactable = false;

        remainTour--;
        remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";

        WorkValueStat(whichBtn);

        if (startCodeVal == gv.levels[whichlevel].codeWorkVal && startDrawVal == gv.levels[whichlevel].drawWorkVal && startPMVal == gv.levels[whichlevel].pmWorkVal)
        {
            WinScene();
        }
        else if (remainTour <= 0)
        {
            LoseScene();
        }

        Invoke(nameof(ActiveBtn), 1);
    }
    void ActiveBtn()
    {
        if (remainTour > 0)
        {
            CodeBtn.interactable = true;
            DrawBtn.interactable = true;
            PMBtn.interactable = true;
        }
    }

    void PlayerStatSet()
    {
        codeStat.text = GetInt("code", 1).ToString() + "   Yaz�l�m G�c�";
        drawStat.text = GetInt("draw", 1).ToString() + "   Sanat Tasar�m� G�c�";
        pmStat.text = GetInt("pm", 1).ToString() + "   Proje Y�netimi G�c�";


        codeTxt.text = "0/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";
        drawTxt.text = "0/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� G�c�";
        pmTxt.text = "0/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi G�c�";
    }
    void WorkValueStat(string whichBtn)
    {
        if (whichBtn == "code" && startCodeVal < gv.levels[whichlevel].codeWorkVal)
        {
            codeTxt.text = (GetInt("code", 1) + startCodeVal).ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";
            startCodeVal += GetInt("code", 1);
        }
        else if (whichBtn == "draw" && startDrawVal < gv.levels[whichlevel].drawWorkVal)
        {
            drawTxt.text = (GetInt("draw", 1) + startDrawVal).ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� G�c�";
            startDrawVal += GetInt("draw", 1);
        }
        else if (whichBtn == "pm" && startPMVal < gv.levels[whichlevel].pmWorkVal)
        {
            pmTxt.text = (GetInt("pm", 1) + startPMVal).ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi G�c�";
            startPMVal += GetInt("pm", 1);
        }
        else
        {
            //print(whichBtn + "  Hata, b�yle bir ifade yok. Butonlar�n unity'deki fonksiyonlar�n� kontrol et.");
        }
    }

    void WinScene()
    {
        SetInt("whichLevel", GetInt("whichLevel", 0) + 1);
        SceneManager.LoadScene("StateScene");
    }
    void LoseScene()
    {
        SceneManager.LoadScene("StateScene");
    }


    public static void SetInt(string a, int value)
    {
        PlayerPrefs.SetInt(a, value);
    }
    public static int GetInt(string a, int defaultVal)
    {
        return PlayerPrefs.GetInt(a, defaultVal);
    }
}
