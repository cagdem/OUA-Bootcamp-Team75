using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject potPanel;
    public GameObject seedPanel;
    public Image[] seedImages;

    public GameObject newPlant;
    public Transform location3D;
    public GameObject tableSlot;
    private RectTransform location2D;
    public bool isTableSlotEmpty = true;
    private string selectedPot;
    private string selectedSeed;

    [Serializable]
    public class KeyGameObjectArrayPair
    {
        public string key;
        public KeyGameObjectPair[] val;
    }

    [Serializable]
    public class KeyGameObjectPair
    {
        public string key;
        public GameObject[] prefabs3D;
        public GameObject[] prefabs2D;
    }

    [Serializable]
    public class KeySpritePair
    {
        public string key;
        public Sprite[] val;
    }

    [Serializable]
    public class KeyDurationPair
    {
        public string key;
        public float[] val;
    }

    public List<KeyGameObjectArrayPair> potsAndPlantsList = new();
    Dictionary<string, Dictionary<string, GameObject[]>> potsAndPlants3DDic = new();
    Dictionary<string, Dictionary<string, GameObject[]>> potsAndPlants2DDic = new();

    public List<KeySpritePair> potsAndSeedSpriteList = new();
    Dictionary<string, Sprite[]> potsAndSeedSpriteDic = new();

    public List<KeyDurationPair> plantDurationList = new();
    Dictionary<string, float[]> plantDurationDic = new();

    void Awake()
    {
        potsAndPlants3DDic = potsAndPlantsList.ToDictionary(x => x.key, x => x.val.ToDictionary(y => y.key, y => y.prefabs3D));
        potsAndPlants2DDic = potsAndPlantsList.ToDictionary(x => x.key, x => x.val.ToDictionary(y => y.key, y => y.prefabs2D));
        potsAndSeedSpriteDic = potsAndSeedSpriteList.ToDictionary(x => x.key, x => x.val);
        plantDurationDic = plantDurationList.ToDictionary(x => x.key, x => x.val);
        location2D = tableSlot.GetComponent<RectTransform>();
    }

    void Update()
    {
        isTableSlotEmpty = tableSlot.transform.childCount == 0;
    }


    public void NewPot()
    {
        if (isTableSlotEmpty)
        {
            potPanel.SetActive(true);  
        }
    }

    public void ChoosePot(TextMeshProUGUI potName)
    {
        selectedPot = potName.text.Trim();
        potPanel.SetActive(false);
        seedPanel.SetActive(true);
        if (potsAndSeedSpriteDic.TryGetValue(selectedPot, out var seedSprites))
        {
            for (int i = 0; i < seedImages.Length; i++)
            {
                seedImages[i].sprite = seedSprites[i];
            }
        }
    }

    public void ChooseSeed(TextMeshProUGUI seedName)
    {
        selectedSeed = seedName.text.Trim();
        seedPanel.SetActive(false);
        CreateNewPlant();
        ClearSelected();
    }

    private void CreateNewPlant()
    {
        if (selectedPot is null || selectedSeed is null)
        { return; }

        if(potsAndPlants3DDic.TryGetValue(selectedPot, out var plantTypeDic))
        {
            if (plantTypeDic.TryGetValue(selectedSeed, out var plantStates))
            {
                GameObject plant = new(selectedPot+selectedSeed);

                plant.transform.SetParent(location3D);
                plant.transform.localPosition = new Vector3(0, 0, 0);

                GameObject plantInstance = Instantiate(newPlant, plant.transform);
                plantInstance.GetComponent<FlowersGrowth>().flowerStages = plantStates;
                if (plantDurationDic.TryGetValue(selectedSeed, out var durations))
                {
                    plantInstance.GetComponent<FlowersGrowth>().growthTimes = durations;
                }

                if (potsAndPlants2DDic.TryGetValue(selectedPot, out var plantTypeDic2D))
                {
                    if (plantTypeDic2D.TryGetValue(selectedSeed, out var plantStates2D))
                    {
                        GameObject plant2D = new(selectedPot + selectedSeed + "2D");
                        plant2D.transform.SetParent(location2D);
                        plant2D.transform.localPosition = new Vector3(0, 0, 0);

                        GameObject plantInstance2D = Instantiate(newPlant, plant2D.transform);
                        plantInstance2D.GetComponent<FlowersGrowth>().flowerStages = plantStates2D;
                        plantInstance2D.GetComponent<FlowersGrowth>().growthTimes = durations;
                        plantInstance2D.GetComponent<FlowersGrowth>().is2D = true;
                    }
                }
            }
        }
    }

    private void ClearSelected()
    {
        selectedPot = null;
        selectedSeed = null;
    }
}
