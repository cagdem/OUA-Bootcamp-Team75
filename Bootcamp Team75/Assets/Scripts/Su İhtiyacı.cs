using UnityEngine;
using System;

public class WateringManager : MonoBehaviour
{
    public GameObject[] driedFlowerStages; // Her büyüme aþamasýna karþýlýk gelen kurumuþ çiçek aþamalarý
    public GameObject emptyPotPrefab; // Boþ saksý prefabý
    public float timeToDryOut = 86400f; // Çiçeðin kurumasý için gereken süre (örneðin, 24 saat)
    public float timeToEmptyPot = 86400f; // Kuruduktan sonra çiçeðin boþ saksýya dönüþmesi için gereken süre
    private DateTime lastWateredTime;
    private DateTime driedOutTime;
    private bool isDriedOut = false;
    private bool isEmptyPot = false;

    private FlowersGrowth flowersGrowth;

    void Start()
    {
        flowersGrowth = GetComponent<FlowersGrowth>();

        // PlayerPrefs'den son sulama zamanýný yükle
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

        CheckWateringStatus();
    }

    void Update()
    {
        // Sulama durumunu periyodik olarak kontrol et
        CheckWateringStatus();
    }

    public void WaterPlant()
    {
        // Son sulama zamanýný þu anki zaman olarak güncelle
        lastWateredTime = DateTime.Now;
        PlayerPrefs.SetString("LastWateredTime", lastWateredTime.ToString());

        // Kuruma ve boþ saksý durumunu sýfýrla ve çiçek görünümünü güncelle
        isDriedOut = false;
        isEmptyPot = false;
        flowersGrowth.ResumeGrowth(); // Büyüme duraklatýlmýþsa devam et
        UpdatePlantAppearance();
    }

    private void CheckWateringStatus()
    {
        if (isEmptyPot) return; // Çiçek zaten boþ saksýysa kontrolü atla

        // Son sulamadan bu yana geçen süreyi hesapla
        TimeSpan timeSinceLastWatered = DateTime.Now - lastWateredTime;
        Debug.Log($"Son sulamadan bu yana geçen süre: {(int)timeSinceLastWatered.TotalSeconds} saniye");

        if (isDriedOut)
        {
            // Çiçek kuruduktan bu yana geçen süreyi hesapla
            TimeSpan timeSinceDriedOut = DateTime.Now - driedOutTime;
            if (timeSinceDriedOut.TotalSeconds >= timeToEmptyPot)
            {
                TurnIntoEmptyPot();
            }
        }
        else
        {
            // Eðer geçen süre kuruma süresini aþarsa, bitki durumunu güncelle
            if (timeSinceLastWatered.TotalSeconds >= timeToDryOut)
            {
                isDriedOut = true;
                driedOutTime = DateTime.Now;
                flowersGrowth.PauseGrowth(); // Çiçek kuruduðunda büyümeyi duraklat
                UpdatePlantAppearance();
            }
        }
    }

    private void UpdatePlantAppearance()
    {
        if (isDriedOut)
        {
            // FlowersGrowth scriptinden mevcut aþama indeksini al
            int currentStage = flowersGrowth.GetCurrentStage();

            // Mevcut aþama GameObject'ini yok et
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            // Karþýlýk gelen kurumuþ çiçek aþamasýný instantiate et
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
            Debug.Log("Çiçek kurumadý. Mevcut aþama gösteriliyor.");
        }
    }

    private void TurnIntoEmptyPot()
    {
        isEmptyPot = true;

        // Mevcut aþama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Boþ saksýyý instantiate et
        Instantiate(emptyPotPrefab, transform.position, Quaternion.identity, transform);
        Debug.Log("Çiçek boþ bir saksýya dönüþtü.");
    }

    public float GetTimeUntilEmptyPot()
    {
        if (isDriedOut)
        {
            TimeSpan timeSinceDriedOut = DateTime.Now - driedOutTime;
            return (float)(timeToEmptyPot - timeSinceDriedOut.TotalSeconds);
        }
        else
        {
            return timeToEmptyPot;
        }
    }
}
