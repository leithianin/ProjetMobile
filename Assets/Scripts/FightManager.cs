﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;

    public GameObject attackText;
    public GameObject defendText;

    public float percentFight = 0.15f;
    public float percentLife;
    public float lifeGoal;

    //Monster
    public bool isMonsterAttacking = false;

    //Player
    public bool isPlayerDefending;
    public bool isWaiting;

    public int rand;

    private bool isAttacked = false;

    public float timerAttack = 1.5f;
    private float timeCooldown = 5f;

    public float timeBetweenAction = 4f;

    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (ThumbnailManager.instance.phase1)
        {
            //Tout les 15% ou 5 secondes mais 1 à 4 secondes apres Actions il peut rien faire
            if (isMonsterAttacking)
            {
                timerAttack -= Time.deltaTime;
                attackText.transform.GetChild(0).GetComponent<Text>().text = timerAttack.ToString("F2");

                if (timerAttack <= 0)
                {
                    CheckDamage();
                }
            }
            else if (ThumbnailManager.instance.monsterLife.currentLife <= lifeGoal || timeCooldown <= 0)
            {
                Debug.Log(ThumbnailManager.instance.monsterLife.currentLife + " <= " + lifeGoal);
                Debug.Log(timeCooldown);
                Action();
            }
            else
            {
                timeCooldown -= Time.deltaTime;
            }
        }
    }

    public void CheckDamage()
    {
        LevelManager.instance.monsterParent.transform.GetChild(1).GetComponent<Animator>().SetBool("isAttack", false);
        isMonsterAttacking = false;
        attackText.SetActive(false);
        SetTimeAction();
        SetLifeGoal();
        timerAttack = 1.5f;

        if (!isPlayerDefending)
        {
            Debug.Log("Degat ! ");
            GameManager.instance.lifeFill.fillAmount -= 0.33f;
        }
    }

    public void ChoosePatterns()
    {
        rand = UnityEngine.Random.Range(1,6);
        SetLifeGoal();

        switch(rand)
        {
            case 1:
                ThumbnailManager.instance.pattern = ThumbnailManager.patterns.Neutral;
                break;
            case 2:
                ThumbnailManager.instance.pattern = ThumbnailManager.patterns.Agressive;
                break;
            case 3:
                ThumbnailManager.instance.pattern = ThumbnailManager.patterns.Scared;
                break;
            case 4:
                ThumbnailManager.instance.pattern = ThumbnailManager.patterns.Anxious;
                break;
            case 5:
                ThumbnailManager.instance.pattern = ThumbnailManager.patterns.Bipolar;
                break;
        }
    }

    public void Action()
    {
        switch(ThumbnailManager.instance.pattern)
        {
            case ThumbnailManager.patterns.Neutral:
                Rand(1, 2);                                                                                     //1 chance(Att) sur 2
                break;
            case ThumbnailManager.patterns.Agressive:                                                                            // 3 chance(Att) sur 4
                Rand(3, 4);
                break;
            case ThumbnailManager.patterns.Scared:                                                                               // 1 chance(Att) sur 4
                Rand(1, 4);
                break;
            case ThumbnailManager.patterns.Anxious:                                                                              // 4 chance(Att) sur 5 PUIS 1 chance(Att) sur 5
                if (ThumbnailManager.instance.monsterLife.currentLife > ThumbnailManager.instance.monsterLife.lifeMax / 2)
                {
                    Rand(4, 5);
                }
                else
                {
                    Rand(1, 5);
                }
                break;
            case ThumbnailManager.patterns.Bipolar:                                                                              // 1 chance(Att) sur 10 PUIS 9 chance(Att) sur 10
                if (isAttacked)
                {
                    Rand(1, 10);
                }
                else
                {
                    Rand(9, 10);
                }
                break;
        }
    }

    public void SetLifeGoal()
    {
        lifeGoal = ThumbnailManager.instance.monsterLife.currentLife - percentLife;
    }

    public void Rand(int chance, int surCombien)
    {
        rand = UnityEngine.Random.Range(1, surCombien+1);
        //Debug.Log(rand + " = " + chance + " chance sur " + surCombien);
        if(rand <= chance || ThumbnailManager.instance.monsterLife.currentShield > 0)
        {
            MonsterAttack();
        }
        else
        {
            MonsterDefend();
        }
    }

    public void MonsterAttack()
    {
        LevelManager.instance.monsterParent.transform.GetChild(1).GetComponent<Animator>().SetBool("isAttack", true);
        timeCooldown = timerAttack;
        attackText.SetActive(true);
        isMonsterAttacking = true;
    }

    public void MonsterDefend()
    {
        SetTimeAction();
        SetLifeGoal();
        ThumbnailManager.instance.monsterLife.currentShield = ThumbnailManager.instance.monsterLife.countShield;
        defendText.SetActive(true);
        defendText.transform.GetChild(0).GetComponent<Text>().text = ThumbnailManager.instance.monsterLife.currentShield.ToString();
        isMonsterAttacking = false;
    }

    public void Attack()
    {
        isPlayerDefending = false;

        if (ThumbnailManager.instance.monsterLife.currentShield <= 0)
        {
            ThumbnailManager.instance.monsterLife.currentLife--;
        }
    }

    public void Defend()
    {
        isPlayerDefending = true;
    }

    public void DontDefend()
    {
        isPlayerDefending = false;
    }

    public void Eat()
    {
        ThumbnailManager.instance.monsterLife.currentShield--;
        defendText.transform.GetChild(0).GetComponent<Text>().text = ThumbnailManager.instance.monsterLife.currentShield.ToString();

        if(ThumbnailManager.instance.monsterLife.currentShield == 0)
        {
            defendText.SetActive(false);
        }
    }

    public void SetTimeAction()
    {
        timeBetweenAction = UnityEngine.Random.Range(2f, 5f);
        timeCooldown = timeBetweenAction;
    }
}
