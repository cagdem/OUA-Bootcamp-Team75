using UnityEngine;

public class FlowersGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // Çiçek aþamalarýný inspectorda atayýn (4 aþama bekleniyor)
    public float[] growthTimes; // Büyüme geçiþ sürelerini inspectorda atayýn (3 süre bekleniyor)
    private int currentStage = 0;
    private float growthTimer = 0f;
    public Transform sunnyZone; // Güneþli bölgeyi inspectorda atayýn
    private bool isInSunnyZone = false;
    private bool previouslyInSunnyZone = false;
    private float sunnyMultiplier = 3f;
    private Quaternion startRotation;
    private bool isGrowthPaused = false; // Büyümeyi duraklatmak için yeni bayrak
    public bool is2D = false; // 2D

    void Start()
    {
        if (is2D)
        {
            startRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            startRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
        }

        // growthTimes dizisinin doðru sayýda eleman içerdiðinden emin olun
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("Büyüme süreleri dizisi, çiçek aþamalarý dizisinden bir eksik element içermelidir!");
            return;
        }

        // Ýlk tohum aþamasýný instantiate et
        Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
    }

    void Update()
    {
        if (isGrowthPaused) return; // Büyüme duraklatýlmýþsa update'i atla

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

            // Kalan süreyi debug için logla
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

        // Bir sonraki aþamayý instantiate et
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        growthTimer *= transitionFactor;
    }

    // Görünümü sýfýrlamak için bu yöntemi ekleyin
    public void ResetAppearance()
    {
        // Mevcut aþama GameObject'ini yok et
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Mevcut aþamayý instantiate et
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, startRotation, transform);
        }
    }

    // Mevcut aþama indeksini almak için bu yöntemi ekleyin
    public int GetCurrentStage()
    {
        return currentStage;
    }

    // Büyümeyi duraklatmak ve devam ettirmek için bu yöntemleri ekleyin
    public void PauseGrowth()
    {
        isGrowthPaused = true;
    }

    public void ResumeGrowth()
    {
        isGrowthPaused = false;
    }
}
