using extOSC;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

public class CubeController : MonoBehaviour
{
    public TextMeshProUGUI instructions;
    public Button calibrateButton;
    public RollerBall ball;
    OSCReceiver osc;
    public ScoreBoard scoreBoard;
    float x = 0.507f;
    float y = 0.482f;
    float z = -0.002f;
    float lastX = 0;
    float lastY = 0;
    float lastZ = 0;
    bool started;
    int numCoins;

    // Use this for initialization
    void Start()
    {
        started = false;
        osc = this.gameObject.AddComponent<OSCReceiver>();
        osc.Bind("/mrmr accelerometer 0 jedermann", onMessageAccel);
        osc.Bind("/mrmr pushbutton 1 jedermann", onMessageButton);
        osc.Bind("/mrmr tactile3D 3 jedermann", writeToLog);
       // string path = Application.dataPath + "/Log.txt";
       //// if (!File.Exists(path))
       // //{
       //     File.WriteAllText(path, "Log file created");
       // //}

        numCoins = 0;
    }

    void writeToLog(OSCMessage msg)
    {
        File.AppendAllText(Application.dataPath + "/Log.txt", msg.ToString() + Environment.NewLine);
    }

    void onMessageButton(OSCMessage msg)
    {
        ball.onMessage(msg);
    }

    void onMessageAccel(OSCMessage msg)
    {
        Debug.LogFormat("msg {0}\n", msg);
        //this.gameObject.transform.Rotate.
        //if ((lastX == 0) && (lastY == 0) && (lastZ == 0))
        //{
        //    this.gameObject.transform.Rotate(180*(msg.Values[1].FloatValue - y), 180 * (msg.Values[2].FloatValue - z), 180 * (msg.Values[0].FloatValue - x));

        //}
        //else
        //{
        if (started) {
            this.gameObject.transform.Rotate(90 * (msg.Values[1].FloatValue - lastY), 90 * (msg.Values[2].FloatValue - lastZ), 90 * (msg.Values[0].FloatValue - lastX));
        }
        lastX = msg.Values[0].FloatValue;
        lastY = msg.Values[1].FloatValue;
        lastZ = msg.Values[2].FloatValue;
    }

    public void CalibrationPress()
    {
        started = true;
        instructions.text = "Collect all the coins in the maze\nUse phone to tilt the maze \nand the push button to jump the ball";
        calibrateButton.gameObject.SetActive(false);
        scoreBoard.startNewGame();

        setNumCoins(GameObject.FindGameObjectsWithTag("Coin").Length);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("demo");
    }

    // Update is called once per frame
    void Update()
    {
        if (numCoins != 0)
        {
            int numCollected = ball.getCollectedCoins();
            File.AppendAllText(Application.dataPath + "/Log.txt", "Collected " + numCollected.ToString() + " of total " + numCoins.ToString() + Environment.NewLine);

            if (numCollected == numCoins)
            {
                File.AppendAllText(Application.dataPath + "/Log.txt", "Collected all " + Environment.NewLine);

                scoreBoard.endGame();
                RestartLevel();
            }
        } else
        {
            File.AppendAllText(Application.dataPath + "/Log.txt", "Updating but numcoins is 0" + Environment.NewLine);

        }
    }

    public void setNumCoins(int num)
    {
        File.AppendAllText(Application.dataPath + "/Log.txt", "num coins is set to " + num.ToString() + Environment.NewLine);

        numCoins = num;
    }
}
