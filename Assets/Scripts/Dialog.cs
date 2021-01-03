using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public int index;
    public float typingSpeed;
    public GameObject continueButton, Player;
    public int playerNotes, playerReactors;
    public bool played1, played2;


    void Start()
    {
        continueButton.SetActive(false);
        StartCoroutine(Type());
        playerNotes = 0;
        playerReactors = 0;

        played1= false;
        played2 = false;

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
                    sentences = new string[] {"Hey Doc left a note...", "It says there were 4 reactors down here"};
                    break;
                case 2:
                    sentences = new string[] {"Seems like the reactors were being used to power the city"};
                    break;
                case 3:
                    sentences = new string[] {"I guess he knew the risks when he built this damn thing", "And now I'm the one left to clean it up"};
                    break;
                case 4:
                    sentences = new string[] {"Doc sure had some terrible handwriting"};
                    break;
                case 5:
                    sentences = new string[] {"Seems like these were left in a rush"};
                    break;
                case 6:
                    sentences = new string[] {"I just feel bad for the kids"};
                    break;
                case 7:
                    sentences = new string[] {"Things are never gonna be the same around these parts"};
                    break;
                case 8:
                    sentences = new string[] {"*Quietly Sobs*"};
                    break;
                default:
                    break;
            }
            playerNotes = Player.GetComponent<PlayerController>().note;
            index = 0;
            StartCoroutine(Type());
        }

        if(playerReactors < Player.GetComponent<PlayerController>().reactors)
        {
            switch (Player.GetComponent<PlayerController>().reactors)
            {
                case 1:
                    sentences = new string[] {"This radiation is getting the better of me", "but at least I can move faster now", "<PRESS Q/LEFT CLICK while moving to Dash>"};
                    break;
                case 2:
                    sentences = new string[] {"It's getting hotter, I've gotta go faster", "<PRESS SPACE again after a jump to Double Jump>"};
                    break;
                case 3:
                    sentences = new string[] {"I can feel my grip getting stronger", "Maybe I can climb up that wall", "<MOVE UP/DOWN while attached to a wall to Climb>"};
                    break;
                case 4:
                    sentences = new string[] {"Thats the last one", "now I can send a message to the others through the Doc's PC...", "If I can find it"};
                    break;
                default:
                    break;
            }
            playerReactors = Player.GetComponent<PlayerController>().reactors;
            index = 0;
            StartCoroutine(Type());
        }


        if(Player.GetComponent<PlayerController>().moreReactors && !played1)
        {

            sentences = new string[] {"Damn! I haven't shut off enough reactors", "I'm done for down here anyway", " I may as well save the world" };
            index = 0;
            StartCoroutine(Type());
            played1= true;
        

        }

        if(Player.GetComponent<PlayerController>().end  && !played2)
        {

            sentences = new string[] {"There is no time.", "The concept in your head is far gone.", "The words fled the world that couldn't be described.", "But now.", "It'll change.", "With a hope given by you" };
            index = 0;
            StartCoroutine(Type());
            played2= true;
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
