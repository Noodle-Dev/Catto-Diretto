using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetExpressions : MonoBehaviour
{
    public GameObject[] petsExpresions;
    public Animator anim;
    [Header("Parameters")]
    public float minTime = 0.5f;
    public float maxTime = 2f;
    bool isIdle;
    public GameObject objetoEncima;

    [Header("Pet mechanic")]
     public int hungerLevel = 100;
    public int boredomLevel = 100;
    public Text hungerText;
    public Text boredomText;
    private float timeSinceLastHungerUpdate = 0.0f;
    private float timeSinceLastBoredomUpdate = 0.0f;
    private float hungerUpdateInterval = 10.0f; 
    private float boredomUpdateInterval = 15.0f;

    private void Start() {
        isIdle = true;
        InvokeRepeating("Parpadear", minTime, maxTime);
        boredomText.text = boredomLevel + "%".ToString();
        hungerText.text = hungerLevel + "%".ToString();
    }
    private void Update() {
        DecreaseActionsRandomly();
        hungerLevel = Mathf.Clamp(hungerLevel, 0, 100);
        boredomLevel = Mathf.Clamp(boredomLevel, 0, 100);
        boredomText.text = boredomLevel + "%".ToString();
        hungerText.text = hungerLevel + "%".ToString();

        if(hungerLevel < 50)
        {
            isIdle = false;
            anim.SetBool("isIdle", true);
            petsExpresions[6].SetActive(true);
        }
        else
        {
            isIdle = true;
            anim.SetBool("isIdle", false);
            petsExpresions[6].SetActive(false);


        }
          if(boredomLevel < 50)
        {
            isIdle = false;
            anim.SetBool("isIdle", true);
            petsExpresions[5].SetActive(true);
        }
        else
        {
            isIdle = true;
            anim.SetBool("isIdle", false);
            petsExpresions[5].SetActive(false);


        }
       
    }
    public void OnChatMessage(string pChatter, string pMessage)
    {
        if(pMessage.Contains("!pat"))
        {
            //isIdle = true;
            anim.SetTrigger("pet");
            StartCoroutine(PetManager(4));
            boredomLevel += Random.Range(1, 4);
            boredomText.text = boredomLevel + "%".ToString();
        }
        if(pMessage.Contains("!feed"))
        {
            //isIdle = true;
            anim.SetTrigger("feed");
            StartCoroutine(PetManager(3));
            hungerLevel += Random.Range(1, 4);
            hungerText.text = hungerLevel + "%".ToString();
        }
        if(pMessage.Contains("cheer"))
        {
            anim.SetTrigger("donation");
            StartCoroutine(PetManager(2));
        }
    }
    void Parpadear()
    {
        if(isIdle == true)
        {
            StartCoroutine(Blinker());
        }
        else if(isIdle == false)
        {
            objetoEncima.SetActive(false);
            petsExpresions[0].SetActive(false);
        }
        
    }
    IEnumerator Blinker()
    {
        objetoEncima.SetActive(false);
        petsExpresions[0].SetActive(true);
        yield return new WaitForSeconds(Random.Range(1, 1.5f));
        petsExpresions[0].SetActive(false);
        objetoEncima.SetActive(true);
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        objetoEncima.SetActive(false);
        petsExpresions[0].SetActive(true);
        yield return new WaitForSeconds(Random.Range(2, 5));
    }
    public void ResetPetValues()
    {
        hungerLevel = 100;
        boredomLevel = 100;
    }
IEnumerator PetManager(int petto)
{
    isIdle = false;
    for (int i = 0; i < petsExpresions.Length; i++)
    {
        petsExpresions[i].SetActive(false);    
    }
    petsExpresions[petto].SetActive(true);
    isIdle = false;
    yield return new WaitForSeconds(3f);
    petsExpresions[petto].SetActive(false);
    isIdle = true;
    petsExpresions[0].SetActive(true);
}
void DecreaseActionsRandomly()
{
     timeSinceLastHungerUpdate += Time.deltaTime;
     timeSinceLastBoredomUpdate += Time.deltaTime;
     if (timeSinceLastHungerUpdate > hungerUpdateInterval)
        {
            hungerLevel -= Random.Range(1, 7);
            timeSinceLastHungerUpdate = 0.0f;
            hungerText.text = hungerLevel + "%".ToString();
        }

        if (timeSinceLastBoredomUpdate > boredomUpdateInterval)
        {
            boredomLevel -= Random.Range(1, 6);
            timeSinceLastBoredomUpdate = 0.0f;
            boredomText.text = boredomLevel + "%".ToString();
        }
}
    
}
