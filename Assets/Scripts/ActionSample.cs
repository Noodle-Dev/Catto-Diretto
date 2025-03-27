using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSample : MonoBehaviour
{
    public GameObject squared;
    public int Pats;
    public void OnChatMessage(string pChatter, string pMessage)
    {
        if(pMessage.Contains("!Blue"))
        {
            squared.GetComponent<SpriteRenderer>().color = Color.blue;
            squared.SetActive(false);
        }
        if(pMessage.Contains("!Red"))
        {
            squared.GetComponent<SpriteRenderer>().color = Color.red;
            squared.SetActive(true);

        }
    }
}
