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
    int whichlevel, startCodeVal, startDrawVal, startPMVal;                       //bu kýsým her level baþýnda tekrar ayarlanacak

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
    int remainTour;                                  //geriye kalan tur sayýsý

    public void ResetGameEX()
    {
        PlayerPrefs.SetInt("firstOpen", 0);

        SetInt("code", 1);
        SetInt("draw", 1);
        SetInt("pm", 1);
        SetInt("whichLevel", 0);

    }
    void Start()
    {
        //bu alttaki dört satýr oyunun deneme aþamasýnda, deðiþkenleri her seferinde sýfýrlamak için yapýlmýþtýr. Buil alýnýrken silinecektir
        //PlayerPrefs.SetInt("firstOpen", 0);

        //SetInt("code", 1);
        //SetInt("draw", 1);
        //SetInt("pm", 1);
        //SetInt("whichLevel", 0);            //0. level aslýnda 1. level oluyor (Levellerde dizi mantýðý var)*/

        AudioManager.instance.PlaySound("Education");

        endBtn.DOFade(0, 0);
        endBtn.GetComponent<RectTransform>().DOScale(0, 0);

        chancePanel.DOFade(0, 0);
        chancePanel.GetComponent<RectTransform>().DOScale(0, 0);

        gm = this;
        gv = GetComponentInParent<GameValues>();

        playerPos = player.position;

        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Sayýsý";

        ResetLevel();
        PlayerStatSet();
    }
    public void ResetLevel()
    {
        whichlevel = GetInt("whichLevel", 0);
        whichLevelTxt.text = (whichlevel + 1).ToString() + ". Seviye";

        remainTour = gv.levels[whichlevel].remainTourVal;
        remainTourTxt.text = remainTour.ToString() + "  Kalan Tur Sayýsý";

        startCodeVal = 0;
        startDrawVal = 0;
        startPMVal = 0;

        codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";
        drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý Ýþi";
        pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";
    }

    public void BasicBtnFunc(string whichBtn)
    {
        if (Random.Range(0,2) == 0)
            AudioManager.instance.PlaySound("AtkBtn1");
        else
            AudioManager.instance.PlaySound("AtkBtn2");

        player.DOBlendablePunchRotation(new(1, 1, 5), 1, 20, 1);        //sondaki 10 ve 1 deðerleri defaault deðerler
        player.DOBlendableMoveBy(new(-120, 0), 0.06f);
        player.DOBlendableMoveBy(new(120, 0), 0.2f).SetDelay(0.3f);

        CodeBtn.interactable = false;
        DrawBtn.interactable = false;
        PMBtn.interactable = false;

        remainTour--;
        remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Sayýsý";

        int a = Random.Range(0, badChance);      //her turda %20 þans ile kötü durumlar oluþur
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
        if (whichlevel >= 4)
        {
            SceneManager.LoadScene("End");
        }
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
        codeStat.text = GetInt("code", 1).ToString() + "   Yazýlým Gücü";
        drawStat.text = GetInt("draw", 1).ToString() + "   Sanat Tasarýmý Gücü";
        pmStat.text = GetInt("pm", 1).ToString() + "   Proje Yönetimi Gücü";


        codeTxt.text = "0/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";
        drawTxt.text = "0/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý iþi";
        pmTxt.text = "0/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";
    }
    void WorkValueStat(string whichBtn)
    {
        if (whichBtn == "code" && startCodeVal < gv.levels[whichlevel].codeWorkVal)
        {
            codeTxt.text = (GetInt("code", 1) + startCodeVal).ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";
            startCodeVal += GetInt("code", 1);
        }
        else if (whichBtn == "draw" && startDrawVal < gv.levels[whichlevel].drawWorkVal)
        {
            drawTxt.text = (GetInt("draw", 1) + startDrawVal).ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý Ýþi";
            startDrawVal += GetInt("draw", 1);
        }
        else if (whichBtn == "pm" && startPMVal < gv.levels[whichlevel].pmWorkVal)
        {
            pmTxt.text = (GetInt("pm", 1) + startPMVal).ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";
            startPMVal += GetInt("pm", 1);
        }
        else
        {
            //print(whichBtn + "  Hata, böyle bir ifade yok. Butonlarýn unity'deki fonksiyonlarýný kontrol et.");
        }
    }

    void WinScene()
    {
        AudioManager.instance.PlaySound("WinGame");

        SetInt("whichLevel", GetInt("whichLevel", 0) + 1);
        
        endBtn.DOFade(1, 1);
        endBtn.GetComponent<RectTransform>().DOScale(1, 0);

        endTxt.color = Color.green;
        endTxt.text = "Tebrikler!\nBu ayýn görevlerini baþarýyla bitirdin.";

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
        endTxt.text = "Kaybettin!\nYeteneklerini yükselt ve tekrar dene";

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
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";

                    return "Küçük bir bug oluþtu. Bu seni yavaþlattý ama durdurmaya yetmez. \n- Yazýlým ilerlemen 1 azaldý.";

                case 1:
                    startCodeVal -= 2;
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";

                    return "Bir bugla karþýlaþtýn. Bunu çözmek oldukça vaktini harcadý.\n- Yazýlým ilerlemen 2 azaldý.";

                case 2:
                    //Ýlerleyemedin
                    return "Ekranýn her yeri 'ERROR' yazýlarýyla doldu. Ne olduðu hakkýnda hiç bir fikrin yok.\n- Bu turda hiç bir ilerleme katedemediniz.";

                case 3:
                    remainTour--;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Sayýsý";

                    return "Takým arkadaþlarýn ile ayný anda ayný dosyayý düzeltmeye çalýþtýn. Olamaz bir Git Conflict hatasý aldýn. \n- Ýlerleme kat edemediniz. \n- Ayrýca tur hakkýnýzda 1 azaldý";

                case 4:
                    startCodeVal -= 3;
                    codeTxt.text = startCodeVal.ToString() + "/" + gv.levels[whichlevel].codeWorkVal + "   Yazýlým Ýþi";

                    return "Takým arkadaþýnla kavga ettin ve arkadaþýn kodlarýn bir kýsmýný sildi.\n- Bu tur boþa gittiði gibi Yazýlým ilerlemen 3 geriledi.";
            }
        }
        else if (whichWork == "draw")    //çizim bugu çýkar
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    startDrawVal -= 1;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý Ýþi";

                    return "Renkleri seçmekte kararsýz kaldýn ve doðru renkleri bulman oldukça vaktini harcadý. \n- Sanat tasarýmý ilerlemen 1 azaldý.";

                case 1:
                    startDrawVal -= 2;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý Ýþi";

                    return "Animasyonda kaydýrma yapmýþsýn. Bu seni yavaþlattý ama durdurmaya yetmez. \n- Sanat tasarýmý ilerlemen 2 azaldý.";

                case 2:
                    remainTour--;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Sayýsý";
                    return "Çizimin bitmek üzereyken bu korkunç þeyi çizdiðine üzüldün ve çizime baþtan baþladýn.\n - Ýlerleme kat edemediniz. \n- Ayrýca tur hakkýnýzda 1 azaldý";

                case 3:
                    startDrawVal -= 3;
                    drawTxt.text = startDrawVal.ToString() + "/" + gv.levels[whichlevel].drawWorkVal + "   Sanat Tasarýmý Ýþi";
                    return "Takým arkadaþlarýn tasarýmýný beðenmedi. Bu tasarýmý oyuna eklemekten vazgeçtiniz. \n- Sanat tasarýmý ilerlemen 3 azaldý.";

                case 4:
                    remainTour -= 2;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Sayýsý";
                    return "Takým arkadaþlarýnla oyun temasýný deðiþtirme kararý aldýnýz.\n- Tur hakkýnýzda 2 azaldý";
            }
        }
        else if (whichWork == "pm")    //proje yönetimi bugu çýkar
        {
            switch (Random.Range(0, 5))
            {
                case 0:
                    startPMVal -= 1;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";

                    return "Notlarýn birbirine girmiþ. Bu seni yavaþlattý ama durdurmaya yetmez.\n- Proje Yönetim ilerlemen 1 azaldý.";

                case 1:
                    startPMVal -= 2;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";

                    return "Bugün telefonun hiç susmadý. Görüþmeler oldukça vaktini harcadý.\n- Proje Yönetim ilerlemen 2 azaldý.";

                case 2:
                    return "Mail listelerini karýþtýrdýn. Herkese düzeltme maili göndermek zorunda kaldýn.\n- Bu turda hiç bir ilerleme katedemediniz.";

                case 3:
                    remainTour -= 2;
                    remainTourTxt.text = " " + remainTour.ToString() + "   Kalan Tur Sayýsý";

                    return "Projenin kilit elemaný hastalandý. Ýyileþmesini beklemekten baþka çaren yok.\n- Tur hakkýnýzda 2 azaldý";

                case 4:
                    startPMVal -= 3;
                    pmTxt.text = startPMVal.ToString() + "/" + gv.levels[whichlevel].pmWorkVal + "   Proje Yönetimi Ýþi";

                    return "Proje çalýþanlarýndan birisi ayrýldý. Olamaz giderken yaptýðý iþlerin bir kýsmýný götürmüþ.\n- Proje Yönetim ilerlemen 3 azaldý.";
            }
        }
        return "Korkma hiç birþe olmadý";
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
