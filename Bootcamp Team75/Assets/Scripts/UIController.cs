using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject potPanel;
    public GameObject seedPanel;
    public Image[] seedImages;

    public GameObject newPlant;
    public Transform location;
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
        public GameObject[] val;
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
    Dictionary<string, Dictionary<string, GameObject[]>> potsAndPlantsDic = new();

    public List<KeySpritePair> potsAndSeedSpriteList = new();
    Dictionary<string, Sprite[]> potsAndSeedSpriteDic = new();

    public List<KeyDurationPair> plantDurationList = new();
    Dictionary<string, float[]> plantDurationDic = new();

    void Awake()
    {
        potsAndPlantsDic = potsAndPlantsList.ToDictionary(x => x.key, x => x.val.ToDictionary(y => y.key, y => y.val));
        potsAndSeedSpriteDic = potsAndSeedSpriteList.ToDictionary(x => x.key, x => x.val);
        plantDurationDic = plantDurationList.ToDictionary(x => x.key, x => x.val);
    }

    public void NewPot()
    {
        potPanel.SetActive(true);
        
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

        if(potsAndPlantsDic.TryGetValue(selectedPot, out var plantTypeDic))
        {
            if (plantTypeDic.TryGetValue(selectedSeed, out var plantStates))
            {
                GameObject plant = new(selectedPot+selectedSeed);

                plant.transform.SetParent(location);
                plant.transform.localPosition = new Vector3(0, 0, 0);

                GameObject plantInstance = Instantiate(newPlant, plant.transform);
                plantInstance.GetComponent<FlowersGrowth>().flowerStages = plantStates;
                if (plantDurationDic.TryGetValue(selectedSeed, out var durations))
                {
                    plantInstance.GetComponent<FlowersGrowth>().growthTimes = durations;
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
