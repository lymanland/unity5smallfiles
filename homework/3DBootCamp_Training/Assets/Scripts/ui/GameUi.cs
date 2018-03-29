using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUi : MonoBehaviour {
    private int totalScore = 0;

    public Text score;
    public Text highScore;

    // Use this for initialization
    void Start () {
        updateScoreText();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddScore()
    {
        totalScore++;
        updateScoreText();
    }

    void updateScoreText()
    {
        highScore.text = "总分:" + totalScore;
    }
}
