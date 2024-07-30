using UnityEngine;

public class FlowerGrowth : MonoBehaviour
{
    public GameObject[] flowerStages; // Assign the 4 stages of the flower in the inspector
    public float[] growthTimes; // Assign 3 transition times in the inspector
    private int currentStage = 0;
    private float growthTimer = 0f;

    void Start()
    {
        // Ensure that growthIntervals has the correct number of elements
        if (growthTimes.Length != flowerStages.Length - 1)
        {
            Debug.LogError("Growth intervals array length must be one less than flower stages array length.");
            return;
        }

        // Instantiate the seed stage at the start
        Instantiate(flowerStages[currentStage], transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        // Update the growth timer
        growthTimer += Time.deltaTime;

        if (currentStage < growthTimes.Length && growthTimer >= growthTimes[currentStage])
        {
            // Proceed to the next stage
            Grow();
            growthTimer = 0f; // Reset the timer
        }
    }

    void Grow()
    {
        // Destroy the current stage GameObject
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Increment the stage
        currentStage++;

        // Instantiate the next stage
        if (currentStage < flowerStages.Length)
        {
            Instantiate(flowerStages[currentStage], transform.position, Quaternion.identity, transform);
        }
    }
}
