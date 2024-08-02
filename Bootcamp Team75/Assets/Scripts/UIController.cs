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
    public GameObject newPlant2d;
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
        public Sprite[] stateImages;
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
    Dictionary<string, Dictionary<string, Sprite[]>> potsAndPlantsImagesDic = new();

    public List<KeySpritePair> potsAndSeedSpriteList = new();
    Dictionary<string, Sprite[]> potsAndSeedSpriteDic = new();

    public List<KeyDurationPair> plantDurationList = new();
    Dictionary<string, float[]> plantDurationDic = new();

    void Awake()
    {
        potsAndPlantsImagesDic = potsAndPlantsList.ToDictionary(x => x.key, x => x.val.ToDictionary(y => y.key, y => y.stateImages));
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

        if (potsAndPlantsImagesDic.TryGetValue(selectedPot, out var plantTypeDic))
        {
            if (plantTypeDic.TryGetValue(selectedSeed, out var plantStates))
            {
                GameObject plant = new(selectedPot + selectedSeed);

                plant.transform.localPosition = new Vector3(0, 0, 0);

                GameObject plantInstance = Instantiate(newPlant, location3D);
                plantInstance.name = selectedPot + selectedSeed;
                Debug.Log("3D: " + plantInstance.transform.localPosition.ToString());

                plantInstance.GetComponent<FlowersGrowth>().flowerStages = plantStates;
                if (plantDurationDic.TryGetValue(selectedSeed, out var durations))
                {
                    plantInstance.GetComponent<FlowersGrowth>().growthTimes = durations;
                }

                GameObject plantInstance2D = Instantiate(newPlant2d, location2D);
                plantInstance2D.name = selectedPot + selectedSeed + "2D";
                Debug.Log("2D: " + plantInstance2D.transform.localPosition.ToString());
                plantInstance2D.GetComponent<FlowersGrowth>().flowerStages = plantStates;
                plantInstance2D.GetComponent<FlowersGrowth>().growthTimes = durations;
                plantInstance2D.GetComponent<FlowersGrowth>().is2dFlower = true;
            }
        }
    }

    private void ClearSelected()
    {
        selectedPot = null;
        selectedSeed = null;
    }
}
