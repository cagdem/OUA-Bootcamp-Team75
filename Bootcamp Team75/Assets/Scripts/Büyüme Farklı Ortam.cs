using UnityEngine;
using UnityEngine.UI;
using System;

public class FlowersGrowth : MonoBehaviour
{
    private Quaternion startRotation;
    public Sprite[] flowerStages; // Çiçek aþamalarýnýn sprite'larýný inspectorda atayýn (4 aþama bekleniyor)
    public float[] growthTimes; // Büyüme geçiþ sürelerini inspectorda atayýn (3 süre bekleniyor)
    private int currentStage = 0;
    private DateTime startTime;
    public Transform sunnyZone; // Güneþli bölgeyi inspectorda atayýn
    private bool isInSunnyZone = false;
    private float sunnyMultiplier = 3f;
    private bool isGrowthPaused = false; // Büyümeyi duraklatmak için yeni bayrak
    private Image flowerImage; // Çiçeðin image bileþeni
    private TimeSpan pausedDuration = TimeSpan.Zero; // Büyüme duraklatma süresi
    private DateTime? pauseStartTime = null; // Büyüme duraklatýldýðýnda zamaný kaydetmek için deðiþken
    public bool is2dFlower = false; // 2D çiçekler için bayrak
    private SpriteRenderer spriteRenderer; // 3D çiçekler için spriteRenderer bileþeni
    void Start()
    {
        // growthTimes dizisinin doðru sayýda eleman içerdiðinden emin olun
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("Büyüme süreleri dizisi, çiçek aþamalarý dizisinden bir eksik element içermelidir!");
            return;
        }

        // Baþlangýç zamanýný ayarla
        startTime = DateTime.Now;

        // Ýlk tohum aþamasýný ayarla
        if(is2dFlower)
        {
            startRotation = Quaternion.Euler(0f, 0f, 0f);

            flowerImage = GetComponent<Image>();
            flowerImage.sprite = flowerStages[currentStage];
            flowerImage.preserveAspect = true;
        }
        else
        {

            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z));

            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = flowerStages[currentStage];
        }
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
            // Toplam büyüme süresini hesapla
            TimeSpan totalGrowthTime = TimeSpan.FromSeconds(growthTimes[currentStage]);
            if (isInSunnyZone)
            {
                totalGrowthTime = TimeSpan.FromSeconds(growthTimes[currentStage] / sunnyMultiplier);
            }

            // Geçen süreyi hesapla
            TimeSpan elapsedTime = (DateTime.Now - startTime) - pausedDuration;

            // Kalan süreyi hesapla
            TimeSpan remainingTime = totalGrowthTime - elapsedTime;

            // Kalan süreyi debug için logla
            Debug.Log($"Kalan Süre: {Mathf.CeilToInt((float)remainingTime.TotalSeconds)} saniye");

            if (elapsedTime >= totalGrowthTime)
            {
                // Bir sonraki aþamaya geç
                Grow();
                startTime = DateTime.Now; // Baþlangýç zamanýný güncelle
                pausedDuration = TimeSpan.Zero; // Duraklama süresini sýfýrla
            }
        }
    }

    public void Grow()
    {
        // Aþamayý artýr
        currentStage++;

        // Bir sonraki aþamayý ayarla
        if (currentStage < flowerStages.Length)
        {
            if (is2dFlower)
            {
                flowerImage.sprite = flowerStages[currentStage];
            }
            else
            {
                spriteRenderer.sprite = flowerStages[currentStage];
            }
        }
    }

    public void AdjustGrowthTimer(float transitionFactor)
    {
        // Geçerli aþama için büyüme süresini ayarla
        growthTimes[currentStage] *= transitionFactor;
    }

    // Görünümü sýfýrlamak için bu yöntemi ekleyin
    public void ResetAppearance()
    {
        // Mevcut aþamayý ayarla
        if (currentStage < flowerStages.Length)
        {
            if (is2dFlower)
            {
                flowerImage.sprite = flowerStages[currentStage];
            }else
            {
                spriteRenderer.sprite = flowerStages[currentStage];
            }
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
        if (!isGrowthPaused)
        {
            isGrowthPaused = true;
            pauseStartTime = DateTime.Now;
        }
    }

    public void ResumeGrowth()
    {
        if (isGrowthPaused)
        {
            isGrowthPaused = false;
            if (pauseStartTime.HasValue)
            {
                pausedDuration += DateTime.Now - pauseStartTime.Value;
            }
            pauseStartTime = null;
        }
    }
}
