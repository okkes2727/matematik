using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject karePrefab;
    public Transform karelerPaneli;
    public GameObject[] karelerDizisi = new GameObject[25];

    List<int> bolumDegerleriListesi = new List<int>();

    int bolunenSayi, bolenSayi;
    int kacinciSoru;
    int dogruSonuc;

    [SerializeField]
    private Transform soruPaneli;
    [SerializeField]
    private TextMeshProUGUI soruText;

    bool soruHazir = false;

    int kalanHak;

    kalanHaklarManager kalanHaklarManager;
    PuanManager puanManager;

    [SerializeField]
    private Sprite[] kareSprites;

    GameObject gecerliKare;
    
    [SerializeField]
    private GameObject sonucPaneli;

    private void Awake()
    {
        kalanHak = 3;
        
     soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;

        kalanHaklarManager = Object.FindObjectOfType<kalanHaklarManager>();
        puanManager = Object.FindObjectOfType<PuanManager>();

        kalanHaklarManager.KalanHaklariKontrolEt(kalanHak);
    }

    void Start()
    {
         sonucPaneli.GetComponent<RectTransform>().localScale=new Vector3(0,0,0);


        kareleriOlustur();
    }

    public void kareleriOlustur()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, Vector3.zero, Quaternion.identity);
            kare.transform.SetParent(karelerPaneli, false);
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi(kare));
            karelerDizisi[i] = kare;
        }

        BolumDegerleriniTexteYazdir();

        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc", 2f);
    }

   void ButonaBasildi(GameObject tiklananButon)
{
    if (soruHazir)
    {
        gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (tiklananButon.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            int butonDegeri = int.Parse(tiklananButon.GetComponentInChildren<TextMeshProUGUI>().text);
            Debug.Log("Tıklanan Buton Değeri: " + butonDegeri);

            if (butonDegeri == dogruSonuc)
            {
                // Doğru sonuç ise yapılacak işlemleri buraya ekleyebilirsiniz.
                puanManager.PuaniArtir(ZorlukSeviyesiniBelirle(bolunenSayi), bolenSayi, dogruSonuc);

                if (bolumDegerleriListesi.Count > 0)
                {
                    GecerliKareyiGizle(); // GecerliKareyiGizle fonksiyonunu çağırarak gecerliKare'yi gizle
                    SoruPaneliniAc();
                }
                else
                {
                    Debug.Log("oyun bitti");
                }
            }
            else
            {
                kalanHak--;
                kalanHaklarManager.KalanHaklariKontrolEt(kalanHak);

                if (kalanHak <= 0)
                {
                    sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);

                    Debug.Log("oyun bitti");
                    // Oyun bittiğinde yapılacak işlemleri ekleyebilirsin.
                }

                // Yanlış cevapta SoruyuSor fonksiyonunu çağırmayı kaldır.
            }
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found in the child object!");
        }
    }
}

    string ZorlukSeviyesiniBelirle(int bolunen)
    {
        if (bolunen <= 40)
        {
            return "kolay";
        }
        else if (bolunen > 40 && bolunen <= 80)
        {
            return "orta";
        }
        else
        {
            return "zor";
        }
    }

    IEnumerator DoFadeRoutine()
    {
        foreach (var kare in karelerDizisi)
        {
            CanvasGroup canvasGroup = kare.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(1, 0.2f);
            }

            yield return new WaitForSeconds(0.07f);
        }
    }

    void BolumDegerleriniTexteYazdir()
    {
        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);

            // TextMeshProUGUI bileşenini al
            TextMeshProUGUI textMeshPro = kare.GetComponentInChildren<TextMeshProUGUI>();

            if (textMeshPro != null)
            {
                textMeshPro.text = rastgeleDeger.ToString();
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found in the child object!");
            }
        }
    }

    void GecerliKareyiGizle()
    {
        gecerliKare.GetComponent<Image>().sprite = kareSprites[Random.Range(0, kareSprites.Length)];
        gecerliKare.GetComponentInChildren<TextMeshProUGUI>().text = ""; // Sayıyı gizle
        gecerliKare.transform.GetComponent<Button>().interactable = false; // Butonu etkileşimsiz yap
    }

    void SoruPaneliniAc()
    {
        SoruyuSor(); // SoruyuSor fonksiyonunu çağırarak soruyu hazırla
        soruPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);
        soruHazir = true;
    }
//  önceden soru ve cevapları oluşturup liste içinde tut .  haklar konusu .  
    void SoruyuSor()
    {
        bolenSayi = Random.Range(2, 11);

        kacinciSoru = Random.Range(0, bolumDegerleriListesi.Count);
        bolunenSayi = bolenSayi * bolumDegerleriListesi[kacinciSoru];
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];

        soruText.text = bolunenSayi.ToString() + " : ? = " + bolenSayi.ToString();
    }

    void Oyunbitti(){
       
      sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.3f).SetEase(Ease.OutBack);

    }


} 