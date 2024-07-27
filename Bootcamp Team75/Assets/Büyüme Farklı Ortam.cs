using UnityEngine;

public class FlowersGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // Çiçeğin evresini denetleyicide atayın (planlanan 4)
    public float[] growthTimes; //  Geçiş süresini denetleyicide atayın (planlanan 3)
    private int currentStage = 0;
    private float growthTimer = 0f;
    public Transform sunnyZone; // Güneşli bölgeyi denetleyicide atayın

    private bool isInSunnyZone = false;
    private bool previouslyInSunnyZone = false;
    private float sunnyMultiplier = 3f;

    void Start()
    {
        // growthTimes dizisinin doğru sayıda elemana sahip olduğundan emin olun
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("Büyüme zamanı, Büyüme aşamasından 1 element az olmalı!");
            return;
        }

        // Başlangıçta tohum evresini oluşturun
        Instantiate(flowerStages[currentStage], transform.position, Quaternion.identity, transform);

    }

    void Update()
    {
        // Çiçeğin güneşli bölge içinde olup olmadığını kontrol edin
        isInSunnyZone = Vector3.Distance(transform.position, sunnyZone.position) < sunnyZone.localScale.x / 2;

        // Büyüme zamanlayıcısını güncelleyin
        float currentGrowthTime = growthTimes[currentStage];
        if (isInSunnyZone)
        {
            currentGrowthTime /= sunnyMultiplier;
        }

        // Bölgeler arasında geçiş yaparken büyüme zamanlayıcısını ayarlayın
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

        // Kalan süreyi hesaplayın
        float remainingTime = currentGrowthTime - growthTimer;

        // Kalan süreyi bir tam sayı olarak hata ayıklama günlüğüne yazdırın
        
        Debug.Log($"Kalan Süre: {Mathf.CeilToInt(remainingTime)} saniye");

        if (currentStage < growthTimes.Length && growthTimer >= currentGrowthTime)
        {
            // Bir sonraki aşamaya geçin
            Grow();
            growthTimer = 0f; // Zamanlayıcıyı sıfırlayın
        }
    }

    void Grow()
    {
        // Geçerli aşama GameObject'ini yok edin
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Aşamayı artırın
        currentStage++;

        // Bir sonraki aşamayı oluşturun
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, Quaternion.identity, transform);
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        growthTimer *= transitionFactor;
    }
}
