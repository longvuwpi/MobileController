using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;

public class ScoreBoard : MonoBehaviour
{

    public TextMeshProUGUI current;
    public TextMeshProUGUI best;

    float startTime;
    static float currentBest = 0f;
    bool isStarted;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        isStarted = false;
        if (currentBest == 0f)
        {
            string path = Application.dataPath + "/Log.txt";
            // if (!File.Exists(path))
            //{
            File.WriteAllText(path, "Log file created" + Environment.NewLine);
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        String bestText = "Best:      ";
        if (currentBest > 0f) bestText += currentBest.ToString("#.00") + "s";
        else bestText += "0s";
        best.text = bestText;

        if (isStarted)
        {
            if (!current.gameObject.active)
            {
                current.gameObject.SetActive(true);
            }
            float currentTime = Time.time - startTime;
            current.text = "Current: " + currentTime.ToString("#.00") + "s";
        }
    }

    public void startNewGame()
    {
        startTime = Time.time;
        isStarted = true;
    }

    public void endGame()
    {
        float runTime = Time.time - startTime;
        if ((currentBest == 0f) || (currentBest > runTime))
        {
            currentBest = runTime;
        }
        isStarted = false;
        current.gameObject.SetActive(false);
    }
}
