using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    public CheckTrial checkTrialScript;
    private Dictionary<string, Action> keywordActions = new Dictionary<string, Action>(); //Takes a string and associates it with the action (using System)
    private KeywordRecognizer keywordRecognizer;

    void Start()
    {
        keywordActions.Add("Fire", checkTrialScript.SetCircleShifa);
        keywordActions.Add("Thunder", checkTrialScript.SetTriangleShifa);
        keywordActions.Add("Ice", checkTrialScript.SetRectangleShifa);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray()); //(using System.Linq)
        keywordRecognizer.OnPhraseRecognized += activatedSpeech; //Gets called when a phrase is recognized
        keywordRecognizer.Start(); // Activates the voice input
    }

    private void activatedSpeech(PhraseRecognizedEventArgs speechActivated)
    { //Called when recognizes a voice input
        Debug.Log("Speech recognized: " + speechActivated.text);

        keywordActions[speechActivated.text].Invoke();

    }

    private void keywordUp()
    { //Inputs can be in their own separate script
        transform.Translate(0, 1, 0);
    }
    private void keywordDown()
    {
        transform.Translate(0, -1, 0);
    }
    private void keywordDelete()
    {
        gameObject.SetActive(false);
    }
}
