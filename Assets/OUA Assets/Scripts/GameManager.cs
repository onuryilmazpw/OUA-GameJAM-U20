using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public Transform player;
    public Button CodeBtn, DrawBtn, PMBtn;
    public TextMeshProUGUI whichLevelTxt;

    Vector2 playerPos;
    [HideInInspector] public GameValues gv;
    int whichlevel, startCodeVal, startDrawVal, startPMVal;                       //bu k�s�m her level ba��nda tekrar ayarlanacak

    [Header("Scenes")]
    public Transform workScene;
    public Transform statueScene;


    [Header("WorkTable Texts")]
    public TextMeshProUGUI remainTourTxt;
    public TextMeshProUGUI codeTxt, drawTxt, pmTxt;

    [Header("Stat Texts")]
    public TextMeshProUGUI codeStat;
    public TextMeshProUGUI drawStat, pmStat;


    [Header("Scene End Pop-Up")]
    public CanvasGroup endBtn;
    public TextMeshProUGUI endTxt;

    //Game Values
    int remainTour;                                  //geriye kalan tur say�s�

    void Start()
    {
        //bu alttaki d�rt sat�r oyunun deneme a�amas�nda, de�i�kenleri her seferinde s�f�rlamak i�in yap�lm��t�r. Buil al�n�rken silinecektir
        SetInt("code", 1);
        SetInt("draw", 1);
        SetInt("pm", 1);
        SetInt("whichLevel", 0);            //0. level asl�nda 1. level oluyor (Levellerde dizi mant��� var)

        endBtn.DOFade(0, 0);
        endBtn.GetComponent<RectTransform>().DOScale(0, 0);

        gm = this;
        gv = GetComponentInParent<GameValues>();
        ResetLevel();

        playerPos = player.position;

        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Say�s�";

        PlayerStatSet();
    }
    public void ResetLevel()
    {
        whichlevel = GetInt("whichLevel", 0);
        whichLevelTxt.text = (whichlevel + 1).ToString() + ". Seviye";

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
    public void GoEduScene()
    {
        StatueScripts.sc.PlayerStatSet();

        statueScene.DOBlendableLocalMoveBy(new(0, 1080), 0.6f);
        workScene.DOBlendableLocalMoveBy(new(0, 1080), 0.6f);

        endBtn.DOFade(0, 0).SetDelay(0.65f);
        endBtn.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.65f);
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

    public void PlayerStatSet()
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

        endBtn.DOFade(1, 1);
        endBtn.GetComponent<RectTransform>().DOScale(1, 0);

        endTxt.color = Color.green;
        endTxt.text = "Tebrikler!\nBu g�n�n g�revlerini ba�ar�yla bitirdin";

        Invoke(nameof(ActiveBtn), 1);
        Invoke(nameof(GoEduScene), 2f);
    }
    void LoseScene()
    {
        endBtn.DOFade(1, 1);
        endBtn.GetComponent<RectTransform>().DOScale(1, 0);

        endTxt.color = Color.red;
        endTxt.text = "Kaybettin!\nYeteneklerini y�kselt ve tekrar dene";

        Invoke(nameof(ActiveBtn), 1);
        Invoke(nameof(GoEduScene), 2f);
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
