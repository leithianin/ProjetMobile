﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public int nbIconMax = 10;
    public int nbIconCurrent = 10;

    public float nb;

    public void Update()
    {
        nb = 1 / (float)nbIconMax;
        GetComponent<Image>().fillAmount = nb * nbIconCurrent; 

        if(GetComponent<Image>().fillAmount == 0)
        {
            Restart();
        }
    }

    public void Restart()
    {
        nbIconCurrent = 10;
        ThumbnailManager.instance.ChooseThumbnail();
    }
}
