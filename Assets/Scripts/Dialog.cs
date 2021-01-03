using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public GameObject continueButton, Player;
    public int playerNotes;

    void Start()
    {
        StartCoroutine(Type());
        playerNotes = 0;

    }

    void Update(){
        if(textDisplay.text == sentences[index]){
            continueButton.SetActive(true);
        }

        if(playerNotes < Player.GetComponent<PlayerController>().note)
        {
            switch (Player.GetComponent<PlayerController>().note)
            {
                case 1:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 2:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 3:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 4:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 5:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 6:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 7:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                case 8:
                    sentences = new string[] {"hello1", "hello2", "hello3", "hello4", "hello5" };
                    break;
                default:
                    break;
            }
            playerNotes = Player.GetComponent<PlayerController>().note;
            index = 0;
            StartCoroutine(Type());
        }

        



    }

    IEnumerator Type(){

        foreach (char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

    public void NextSentence(){

        continueButton.SetActive(false);

        if(index < sentences.Length - 1){
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else{
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }
}
