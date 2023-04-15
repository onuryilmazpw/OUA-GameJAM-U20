using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Bad Chance")]
    public CanvasGroup chancePanel;
    public TextMeshProUGUI chanceTxt;
    public int badChance;

    //Game Values
    int remainTour;                                  //geriye kalan tur say�s�

    public void ResetGameEX()
    {
        PlayerPrefs.SetInt("firstOpen", 0);

        SetInt("code", 1);
        SetInt("draw", 1);
        SetInt("pm", 1);
        SetInt("whichLevel", 0);
    }

    public void Update()
    {
        if (whichlevel >= 5)
        {
            ResetGameEX();
            SceneManager.LoadScene("End");
        }
    }

    void Start()
    {
        AudioManager.instance.PlaySound("Education");

        endBtn.DOFade(0, 0);
        endBtn.GetComponent<RectTransform>().DOScale(0, 0);

        chancePanel.DOFade(0, 0);
        chancePanel.GetComponent<RectTransform>().DOScale(0, 0);

        gm = this;
        gv = GetComponentInParent<GameValues>();

        playerPos = player.position;

        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Say�s�";

        ResetLevel();
        PlayerStatSet();
    }
    public void ResetLevel()
    {
        whichlevel = GetInt("whichLevel", 0);
        whichLevelTxt.text = (whichlevel + 1).ToString() + ". Seviye";

        remainTour = gv.levels[whichlevel].remainTourVal;
        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Say�s�";

        startCodeVal = 0;
        startDrawVal = 0;
        startPMVal = 0;

        codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";
        drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� ��i";
        pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";
    }

    public void BasicBtnFunc(string whichBtn)
    {
        if (Random.Range(0,2) == 0)
            AudioManager.instance.PlaySound("AtkBtn1");
        else
            AudioManager.instance.PlaySound("AtkBtn2");

        player.DOBlendablePunchRotation(new(1, 1, 5), 1, 20, 1);        //sondaki 10 ve 1 de�erleri defaault de�erler
        player.DOBlendableMoveBy(new(-120, 0), 0.06f);
        player.DOBlendableMoveBy(new(120, 0), 0.2f).SetDelay(0.3f);

        CodeBtn.interactable = false;
        DrawBtn.interactable = false;
        PMBtn.interactable = false;

        remainTour--;
        remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";

        int a = Random.Range(0, badChance);      //her turda %20 �ans ile k�t� durumlar olu�ur
        if (a == 0)
        {
            BadChance(whichBtn);
            return;
        }

        WorkValueStat(whichBtn);

        if (startCodeVal >= gv.levels[whichlevel].codeWorkVal && startDrawVal >= gv.levels[whichlevel].drawWorkVal && startPMVal >= gv.levels[whichlevel].pmWorkVal)
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
        AudioManager.instance.PlaySound("NormalBtn");
        AudioManager.instance.StopSound("Fight");
        AudioManager.instance.PlaySound("Education");

        statueScene.DOBlendableLocalMoveBy(new(0, 1080), 0.6f);
        workScene.DOBlendableLocalMoveBy(new(0, 1080), 0.6f);

        endBtn.DOFade(0, 0).SetDelay(0.65f);
        endBtn.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.65f);

        BadPanelClose();
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
        drawTxt.text = "0/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� i�i";
        pmTxt.text = "0/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";
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
            drawTxt.text = (GetInt("draw", 1) + startDrawVal).ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� ��i";
            startDrawVal += GetInt("draw", 1);
        }
        else if (whichBtn == "pm" && startPMVal < gv.levels[whichlevel].pmWorkVal)
        {
            pmTxt.text = (GetInt("pm", 1) + startPMVal).ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";
            startPMVal += GetInt("pm", 1);
        }
        else
        {
            //print(whichBtn + "  Hata, b�yle bir ifade yok. Butonlar�n unity'deki fonksiyonlar�n� kontrol et.");
        }
    }

    void WinScene()
    {
        AudioManager.instance.PlaySound("WinGame");

        SetInt("whichLevel", GetInt("whichLevel", 0) + 1);
        
        endBtn.DOFade(1, 1);
        endBtn.GetComponent<RectTransform>().DOScale(1, 0);

        endTxt.color = Color.green;
        endTxt.text = "Tebrikler!\nBu ay�n g�revlerini ba�ar�yla bitirdin.";

        Invoke(nameof(ActiveBtn), 1);
        Invoke(nameof(GoEduScene), 2f);

        chancePanel.DOFade(0, 0f);
        chancePanel.GetComponent<RectTransform>().DOScale(0, 0);
    }
    void LoseScene()
    {
        AudioManager.instance.PlaySound("LoseGame");

        endBtn.DOFade(1, 1);
        endBtn.GetComponent<RectTransform>().DOScale(1, 0);

        endTxt.color = Color.red;
        endTxt.text = "Kaybettin!\nYeteneklerini y�kselt ve tekrar dene";

        Invoke(nameof(ActiveBtn), 1);
        Invoke(nameof(GoEduScene), 2f);

        chancePanel.DOFade(0, 0f);
        chancePanel.GetComponent<RectTransform>().DOScale(0, 0);
    }


    void BadChance(string whichWork)
    {
        AudioManager.instance.PlaySound("BadThings");

        chancePanel.DOFade(1, 1);
        chancePanel.GetComponent<RectTransform>().DOScale(1, 0);

        chanceTxt.text = Olaylar(whichWork);
    }
    public void BadPanelClose()
    {
        chancePanel.DOFade(0, 0.5f);
        chancePanel.GetComponent<RectTransform>().DOScale(0, 0).SetDelay(0.51f);

        ActiveBtn();
    }
    string Olaylar(string whichWork)
    {
        if (whichWork == "code")     //kodlama bugu 
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    startCodeVal -= 1;
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";

                    return "K���k bir bug olu�tu. Bu seni yava�latt� ama durdurmaya yetmez. \n- Yaz�l�m ilerlemen 1 azald�.";

                case 1:
                    startCodeVal -= 2;
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";

                    return "Bir bugla kar��la�t�n. Bunu ��zmek olduk�a vaktini harcad�.\n- Yaz�l�m ilerlemen 2 azald�.";

                case 2:
                    //�lerleyemedin
                    return "Ekran�n her yeri 'ERROR' yaz�lar�yla doldu. Ne oldu�u hakk�nda hi� bir fikrin yok.\n- Bu turda hi� bir ilerleme katedemediniz.";

                case 3:
                    remainTour--;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";

                    return "Tak�m arkada�lar�n ile ayn� anda ayn� dosyay� d�zeltmeye �al��t�n. Olamaz bir Git Conflict hatas� ald�n. \n- �lerleme kat edemediniz. \n- Ayr�ca tur hakk�n�zda 1 azald�";

                case 4:
                    startCodeVal -= 3;
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yaz�l�m ��i";

                    return "Tak�m arkada��nla kavga ettin ve arkada��n kodlar�n bir k�sm�n� sildi.\n- Bu tur bo�a gitti�i gibi Yaz�l�m ilerlemen 3 geriledi.";
            }
        }
        else if (whichWork == "draw")    //�izim bugu ��kar
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    startDrawVal -= 1;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� ��i";

                    return "Renkleri se�mekte karars�z kald�n ve do�ru renkleri bulman olduk�a vaktini harcad�. \n- Sanat tasar�m� ilerlemen 1 azald�.";

                case 1:
                    startDrawVal -= 2;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� ��i";

                    return "Animasyonda kayd�rma yapm��s�n. Bu seni yava�latt� ama durdurmaya yetmez. \n- Sanat tasar�m� ilerlemen 2 azald�.";

                case 2:
                    remainTour--;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";
                    return "�izimin bitmek �zereyken bu korkun� �eyi �izdi�ine �z�ld�n ve �izime ba�tan ba�lad�n.\n - �lerleme kat edemediniz. \n- Ayr�ca tur hakk�n�zda 1 azald�";

                case 3:
                    startDrawVal -= 3;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasar�m� ��i";
                    return "Tak�m arkada�lar�n tasar�m�n� be�enmedi. Bu tasar�m� oyuna eklemekten vazge�tiniz. \n- Sanat tasar�m� ilerlemen 3 azald�.";

                case 4:
                    remainTour -= 2;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";
                    return "Tak�m arkada�lar�nla oyun temas�n� de�i�tirme karar� ald�n�z.\n- Tur hakk�n�zda 2 azald�";
            }
        }
        else if (whichWork == "pm")    //proje y�netimi bugu ��kar
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    startPMVal -= 1;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";

                    return "Notlar�n birbirine girmi�. Bu seni yava�latt� ama durdurmaya yetmez.\n- Proje Y�netim ilerlemen 1 azald�.";

                case 1:
                    startPMVal -= 2;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";

                    return "Bug�n telefonun hi� susmad�. G�r��meler olduk�a vaktini harcad�.\n- Proje Y�netim ilerlemen 2 azald�.";

                case 2:
                    return "Mail listelerini kar��t�rd�n. Herkese d�zeltme maili g�ndermek zorunda kald�n.\n- Bu turda hi� bir ilerleme katedemediniz.";

                case 3:
                    remainTour -= 2;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Say�s�";

                    return "Projenin kilit eleman� hastaland�. �yile�mesini beklemekten ba�ka �aren yok.\n- Tur hakk�n�zda 2 azald�";

                case 4:
                    startPMVal -= 3;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Y�netimi ��i";

                    return "Proje �al��anlar�ndan birisi ayr�ld�. Olamaz giderken yapt��� i�lerin bir k�sm�n� g�t�rm��.\n- Proje Y�netim ilerlemen 3 azald�.";
            }
        }
        return "Korkma hi� bir�e olmad�";
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
