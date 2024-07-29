using UnityEngine;
using System;

public class WateringManager : MonoBehaviour
{
    public GameObject[] driedFlowerStages; // Her büyüme aþamasýna karþýlýk gelen kurumuþ çiçek aþamalarý
    public float timeToDryOut = 86.400f; // Çiçeðin kurumaya baþlayacaðý süre (örneðin, 24 saat)
    private DateTime lastWateredTime; // Son sulama zamaný
    private bool isDriedOut = false; // Çiçeðin kurumuþ olup olmadýðýný belirtir

    private FlowersGrowth flowersGrowth; // FlowersGrowth bileþeni

    void Start()
    {
        flowersGrowth = GetComponent<FlowersGrowth>();

        // PlayerPrefs'ten son sulama zamanýný yükle
        if (PlayerPrefs.HasKey("LastWateredTime"))
        {
            string lastWateredTimeStr = PlayerPrefs.GetString("LastWateredTime");
            lastWateredTime = DateTime.Parse(lastWateredTimeStr);
        }
        else
        {
            // Kaydedilmiþ bir sulama zamaný yoksa, þu anki zamaný son sulama zamaný olarak ayarla
            lastWateredTime = DateTime.Now;
            PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());
        }

        CheckWateringStatus(); // Sulama durumunu kontrol et
    }

    void Update()
    {
        // Sulama durumunu belirli aralýklarla kontrol et
        CheckWateringStatus();
    }

    public void WaterPlant()
    {
        // Son sulama zamanýný þu anki zamanla güncelle
        lastWateredTime = DateTime.Now;
        PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());

        // Kuruma durumunu sýfýrla ve bitkinin görünümünü güncelle
        isDriedOut = false;
        UpdatePlantAppearance();
    }

    private void CheckWateringStatus()
    {
        // Son sulamadan bu yana geçen süreyi hesapla
        TimeSpan timeSinceLastWatered = DateTime.Now - lastWateredTime;
        Debug.Log($"Son sulamadan bu yana geçen süre: {timeSinceLastWatered.TotalSeconds} saniye");

        // Geçen süre kurumaya kadar olan süreyi aþarsa, bitki durumunu güncelle
        if (timeSinceLastWatered.TotalSeconds >= timeToDryOut)
        {
            isDriedOut = true;
        }

        UpdatePlantAppearance(); // Bitkinin görünümünü güncelle
    }

    private void UpdatePlantAppearance()
    {
        if (isDriedOut)
        {
            // FlowersGrowth script'inden mevcut aþama indeksini al
            int currentStage = flowersGrowth.GetCurrentStage();

            // Mevcut aþama GameObject'ini yok et
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Karþýlýk gelen kurumuþ çiçek aþamasýný oluþtur
            if (currentStage < driedFlowerStages.Length)
            {
                Instantiate(driedFlowerStages[currentStage], transform.position, Quaternion.identity, transform);
                Debug.Log("Çiçek kurudu. Kurumuþ aþama gösteriliyor.");
            }
        }
        else
        {
            // Mevcut büyüme aþamasýnýn görünümünü sýfýrla
            flowersGrowth.ResetAppearance();
            Debug.Log("Çiçek kurumamýþ. Mevcut aþama gösteriliyor.");
        }
    }
}
