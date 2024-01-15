using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuanManager : MonoBehaviour
{
    private int toplamPuan;
    private int puanArtisi;

    [SerializeField]
    private TextMeshProUGUI puanText;

    public void PuaniArtir(string zorlukSeviyesi, int bolenSayi, int dogruSonuc)
    {
        switch (zorlukSeviyesi)
        {
            case "kolay":
                puanArtisi = 5;
                break;

            case "orta":
                puanArtisi = 10;
                break;

            case "zor":
                puanArtisi = 15;
                break;
        }

        toplamPuan += puanArtisi;

        // Özel işlemler ekleyebilirsiniz.
        // Örneğin, bolenSayi ve dogruSonuc değerlerini kullanarak özel bir şeyler yapabilirsiniz.

        puanText.text = toplamPuan.ToString();
    }
}
