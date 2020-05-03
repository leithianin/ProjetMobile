﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteLevel : MonoBehaviour
{
    public List<GameObject> monstersList;

    public List<GameObject> cutThumbnailsList;
    public List<GameObject> whipThumbnailsList;
    public List<GameObject> kneadThumbnailsList;
    public List<GameObject> cookThumbnailsList;
    public List<GameObject> boilThumbnailsList;

    public List<GameObject> randomThumbnailsList;

    public List<string> buttonString       = new List<string>() {"cut","whip","knead","cook","boil"};
    public List<string> buttonStringRemove = new List<string>() { "cut", "whip", "knead", "cook", "boil" };

    public string buttonName;
    private GameObject thumbnailChoosen;

    public int nbFloor = 1;

    public GameObject AddMonster()
    {
        buttonStringRemove = new List<string>(buttonString);
        randomThumbnailsList.Clear();
        GameObject monster = RandomMonster();
        monster.transform.GetChild(3).GetComponent<ThumbnailManager>().thumbnailsList = RandomThumbnails();
        return monster;
    }

    public GameObject RandomMonster()
    {
        
        GameObject randomMonster = monstersList[Random.Range(0, monstersList.Count)];
        return randomMonster;
    }

    public List<GameObject> RandomThumbnails()
    {
        RandomButtons();

        for (int i = 1; i <= nbFloor; i++)
        {
            buttonName = buttonStringRemove[Random.Range(0, 3)];

            switch(buttonName)
            {
                case "cut":
                    if (i == nbFloor){thumbnailChoosen = cutThumbnailsList[Random.Range(0, cutThumbnailsList.Count-2)]; }
                    else             {thumbnailChoosen = cutThumbnailsList[Random.Range(0, cutThumbnailsList.Count   )];}
                    break;
                case "whip":
                    if (i == nbFloor) { thumbnailChoosen = whipThumbnailsList[Random.Range(0, whipThumbnailsList.Count - 2)]; }
                    else              { thumbnailChoosen = whipThumbnailsList[Random.Range(0, whipThumbnailsList.Count    )]; }
                    break;
                case "knead":
                    if (i == nbFloor) { thumbnailChoosen = kneadThumbnailsList[Random.Range(0, kneadThumbnailsList.Count - 2)]; }
                    else              { thumbnailChoosen = kneadThumbnailsList[Random.Range(0, kneadThumbnailsList.Count    )]; }
                    break;
                case "cook":
                    if (i == nbFloor) { thumbnailChoosen = cookThumbnailsList[Random.Range(0, cookThumbnailsList.Count - 2)]; }
                    else              { thumbnailChoosen = cookThumbnailsList[Random.Range(0, cookThumbnailsList.Count    )]; }
                    break;
                case "boil":
                    if (i == nbFloor) { thumbnailChoosen = boilThumbnailsList[Random.Range(0, boilThumbnailsList.Count - 1)]; }
                    else              { thumbnailChoosen = boilThumbnailsList[Random.Range(0, boilThumbnailsList.Count    )]; }
                    break;
            }
            randomThumbnailsList.Add(thumbnailChoosen);
        }

        return randomThumbnailsList;
    }

    public void RandomButtons()
    {
        buttonStringRemove.RemoveAt(Random.Range(0, buttonStringRemove.Count));
        buttonStringRemove.RemoveAt(Random.Range(0, buttonStringRemove.Count));
    }

    public void WhichButtonList()
    {

    }
}
