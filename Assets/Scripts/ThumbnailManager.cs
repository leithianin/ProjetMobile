﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailManager : MonoBehaviour
{
    public static ThumbnailManager instance;

    public Life monsterLife;
    public GameManager gm;

    public List<GameObject> thumbnailsList;

    private int random;
    private float posStart;
    private float posEnd;
    private float swipeDifference;

    private int vignetteNb;

    public bool knead = false;
    public bool cook = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        ChooseThumbnail();
    }

    public void Update()
    {
        if (thumbnailsList.Count != 0 && vignetteNb < thumbnailsList.Count)
        {
            if (thumbnailsList[vignetteNb].gameObject != null)
            {
                if (thumbnailsList[vignetteNb].gameObject.name.Contains("KneadMaintain") && Input.GetMouseButton(0) && knead)
                {
                    transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition += Vector3.right;

                    if (transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.x > 40f)
                    {
                        ValideAction();
                    }
                }
                else if (thumbnailsList[vignetteNb].gameObject.name.Contains("KneadMaintain") && !knead && transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition.x > -50f)
                {
                    transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition -= Vector3.right;
                }

                if (thumbnailsList[vignetteNb].gameObject.name.Contains("CookMaintain") && Input.GetMouseButton(0) && cook)
                {
                    transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().localPosition += Vector3.right;

                    if (transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.x > 40f)
                    {
                        ValideAction();
                    }
                }
            }

            /*else if (thumbnailsList[0].gameObject.name.Contains("KneadMaintain") && Input.GetMouseButtonUp(0))
            {
                if (transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.x > 40f && transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.x < 50f)
                {
                    ValideAction();
                }
                else
                {
                    transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition = new Vector3(-50f, transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.y,
                                                                                                         transform.GetChild(0).GetChild(0).GetChild(1).transform.localPosition.z);
                }
            }*/

            //TOUCH
            if (thumbnailsList[vignetteNb].gameObject.name.Contains("WhipMixed") && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Began)
                {
                    posStart = touch.position.y;
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    posEnd = touch.position.y;
                }
                else
                {
                    return;
                }

                swipeDifference = Mathf.Abs(posStart - posEnd);

                if(posEnd < posStart && swipeDifference > 200f)
                {
                    ValideAction();
                }
                else
                {
                    return;
                }
            }
        }
    }

    public void ChooseThumbnail()
    {
        Instantiate(thumbnailsList[0], transform.GetChild(0).transform.position, Quaternion.identity, transform.GetChild(0));

        if (thumbnailsList.Count > 1)
        {
            Instantiate(thumbnailsList[1], transform.GetChild(1).transform.position, Quaternion.identity, transform.GetChild(1));
        }

        if (thumbnailsList.Count > 2)
        {
            Instantiate(thumbnailsList[2], transform.GetChild(2).transform.position, Quaternion.identity, transform.GetChild(2));
        }
    }

    public void CheckAction(int nbAction)
    {
        for (vignetteNb = 0; vignetteNb < 3; vignetteNb++)
        {
            if (!thumbnailsList[vignetteNb].gameObject.name.Contains("Lock") || !transform.GetChild(vignetteNb).GetChild(0).gameObject.GetComponent<Thumbnail>().isLocked)
            {
                break;
            }
        }

        switch(nbAction)
        {
            case 1:
                if (thumbnailsList[vignetteNb].gameObject.name.Contains("Whip"))
                {
                    if (thumbnailsList[vignetteNb].gameObject.name.Contains("WhipMix"))
                    {
                        return;
                    }
                    else
                    {
                        ValideAction();
                    }
                }
                else
                {
                    WrongAction();
                }
                break;
            case 2:
                    if (thumbnailsList[vignetteNb].gameObject.name.Contains("Knead"))
                    {
                        if (thumbnailsList[vignetteNb].gameObject.name.Contains("KneadMaintain"))
                        {
                            return;
                        }
                        else
                        {
                            ValideAction();
                        }
                    }
                    else
                    {
                        WrongAction();
                    }
                
                break;
            case 3:
                if (thumbnailsList[vignetteNb].gameObject.name.Contains("Cut"))
                {
                    if (transform.GetChild(vignetteNb).GetChild(0).transform.childCount > 1)
                    {
                        Debug.Log("Il a des enfants");
                        ValidateTemporary();
                    }
                    else
                    {
                        Debug.Log("Stop Coroutine Validation");
                        StopAllCoroutines();
                        ValideAction();
                    }
                }
                else
                {
                    WrongAction();
                }
                break;
            case 4:
                if (thumbnailsList[vignetteNb].gameObject.name.Contains("Cook"))
                {
                    if (thumbnailsList[vignetteNb].gameObject.name.Contains("CookMaintain"))
                    {
                        return;
                    }
                    else
                    {
                        ValideAction();
                    }
                }
                else
                {
                    WrongAction();
                }
                break;
        }
    }

    public void ValideAction()
    {
        thumbnailsList.RemoveAt(vignetteNb);
        transform.GetChild(vignetteNb).GetChild(0).gameObject.GetComponent<Thumbnail>().NeedToDestroy();

        if(vignetteNb != 0)
        {
            Unlock();
        }

        HitMonster();
        Heal();
    }

    public void Unlock(GameObject go = null)
    {
        if (go != null)
        {
            go.GetComponent<Thumbnail>().isLocked = false;
            Destroy(go.transform.Find("Lock").gameObject);
        }
        else
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<Thumbnail>().isLocked = false;
            Destroy(transform.GetChild(0).GetChild(0).Find("Lock").gameObject);
        }
    }

    public void ValidateTemporary()
    {
        transform.GetChild(vignetteNb).GetChild(0).GetChild(0).gameObject.GetComponent<Thumbnail>().NeedToDestroy();

        StartCoroutine("Compteur");
    }

    public IEnumerator Compteur()
    {
        Debug.Log("Rentré");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Temps écoulé");
        Destroy(transform.GetChild(vignetteNb).GetChild(0).gameObject);
        GameObject inst = Instantiate(thumbnailsList[vignetteNb], transform.GetChild(vignetteNb).transform.position, Quaternion.identity, transform.GetChild(vignetteNb));

        if(thumbnailsList.Count < 2 )
        {
            Unlock(inst);
        }
    }

    public void WrongAction()
    {
        Damage();
    }

    public void NewWave()
    {
        if (thumbnailsList.Count != 0)
        {
            transform.GetChild(1).GetChild(0).gameObject.GetComponent<Thumbnail>().NeedToMove(transform.GetChild(0).transform.position);
            //StartCoroutine(MoveTo(transform.GetChild(1).GetChild(0).gameObject, transform.GetChild(0).transform.position));
            transform.GetChild(1).GetChild(0).transform.SetParent(transform.GetChild(0));
        }

        if (thumbnailsList.Count > 1)
        {
            transform.GetChild(2).GetChild(0).gameObject.GetComponent<Thumbnail>().NeedToMove(transform.GetChild(1).transform.position);
            //StartCoroutine(MoveTo(transform.GetChild(2).GetChild(0).gameObject, transform.GetChild(1).transform.position));
            transform.GetChild(2).GetChild(0).transform.SetParent(transform.GetChild(1));
        }

        if (thumbnailsList.Count > 2)
        {
            Instantiate(thumbnailsList[2], transform.GetChild(2).transform.position, Quaternion.identity, transform.GetChild(2));
        }
    }

    public void HitMonster()
    {
        monsterLife.nbIconCurrent--;

        if(monsterLife.nbIconCurrent == 0)
        {
            LevelManager.instance.NextMonster();
        }
        else
        {
            NewWave();
        }
    }

    public void Heal()
    {
        gm.lifeFill.fillAmount = gm.lifeFill.fillAmount + (1f * 0.02f);
    }

    public void Damage()
    {
        gm.lifeFill.fillAmount = gm.lifeFill.fillAmount - (1f * 0.20f);
    }

    /*public IEnumerator MoveTo(GameObject go, Vector3 targetPos)
    {
        go.transform.position = Vector3.MoveTowards(go.transform.position, targetPos, 0.5f);

        if(Vector3.Distance(go.transform.position, targetPos) > 0.5f)
        {
            StartCoroutine(MoveTo(go, targetPos));
        }
        else
        {
            //End
        }

        yield return new WaitForSeconds(0.1f);
    }*/
}
