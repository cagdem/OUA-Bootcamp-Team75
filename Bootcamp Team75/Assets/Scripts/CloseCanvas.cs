using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCanvas : MonoBehaviour
{
    public GameObject kapaliAlan;
    public GameObject bahce;

    public void CloseKapaliAlan()
    {
        kapaliAlan.SetActive(false);
    }

    public void OpenKapaliAlan()
    {
        kapaliAlan.SetActive(true);
    }
    public void CloseBahce()
    {
        bahce.SetActive(false);
    }

    public void OpenBahce()
    {
        bahce.SetActive(true);
    }
}
