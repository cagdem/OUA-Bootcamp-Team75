using UnityEngine;

public class FlowersGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // Çiçek aþamalarýný denetleyicide atayýn (beklenen 4 aþama)
    public GameObject[] driedFlowerStages; // Kurumuþ çiçek aþamalarýný denetleyicide atayýn (beklenen 4 aþama)
    public float[] growthTimes; // Büyüme geçiþ sürelerini denetleyicide atayýn (beklenen 3 süre)
    private int currentStage = 0; // Mevcut aþama indeksi
    private float growthTimer = 0f; // Büyüme zamanlayýcýsý
    public Transform sunnyZone; // Güneþli bölgeyi denetleyicide atayýn

    private bool isInSunnyZone = false; // Güneþli bölgede olup olmadýðýný belirtir
    private bool previouslyInSunnyZone = false; // Daha önce güneþli bölgede olup olmadýðýný belirtir
    private float sunnyMultiplier = 3f; // Güneþli bölgede büyüme hýzlandýrýcý katsayý
    private Quaternion startRotation; // Baþlangýç rotasyonu

    void Start()
    {
        startRotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        // growthTimes dizisinin doðru sayýda eleman içerdiðinden emin ol
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("Büyüme süreleri dizisi, çiçek aþamalarý dizisinden bir eksik eleman içermelidir!");
            return;
        }

        // Ýlk tohum aþamasýný oluþtur
        Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
    }

    void Update()
    {
        if (sunnyZone != null)
        {
            // Çiçeðin güneþli bölgede olup olmadýðýný kontrol et
            isInSunnyZone = Vector3.Distance(transform.position, sunnyZone.position) < sunnyZone.localScale.x / 2;
        }

        if (currentStage < growthTimes.Length)
        {
            // Büyüme zamanlayýcýsýný güncelle
            float currentGrowthTime = growthTimes[currentStage];
            if (isInSunnyZone)
            {
                currentGrowthTime /= sunnyMultiplier;
            }

            // Bölgeler arasýnda geçiþ yaparken büyüme zamanlayýcýsýný ayarla
            if (isInSunnyZone && !previouslyInSunnyZone)
            {
                growthTimer /= sunnyMultiplier;
            }
            else if (!isInSunnyZone && previouslyInSunnyZone)
            {
                growthTimer *= sunnyMultiplier;
            }

            previouslyInSunnyZone = isInSunnyZone;

            growthTimer += Time.deltaTime;

            // Kalan süreyi hesapla
            float remainingTime = currentGrowthTime - growthTimer;

            // Debugging için kalan süreyi logla
            Debug.Log($"Kalan Süre: {Mathf.CeilToInt(remainingTime)} saniye");

            if (growthTimer >= currentGrowthTime)
            {
                // Bir sonraki aþamaya geç
                Grow();
                growthTimer = 0f; // Zamanlayýcýyý sýfýrla
            }
        }
    }

    public void Grow()
    {
        // Mevcut aþama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Aþamayý artýr
        currentStage++;

        // Bir sonraki aþamayý oluþtur
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        growthTimer *= transitionFactor;
    }

    // Bu metodu ekleyerek görünümü sýfýrla
    public void ResetAppearance()
    {
        // Mevcut aþama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Mevcut aþamayý oluþtur
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    // Bu metodu ekleyerek mevcut aþama indeksini al
    public int GetCurrentStage()
    {
        return currentStage;
    }
}
