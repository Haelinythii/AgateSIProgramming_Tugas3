using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int score;
    private int lastScoreHighlight;

    public CharacterSoundController characterSoundController;
    public int scoreHighlightRange;

    private void Start()
    {
        score = 0;
        lastScoreHighlight = 0;
    }

    public float GetCurrentScore()
    {
        return score;
    }

    public void IncreaseCurrentScore(int scoreIncrement)
    {
        score += scoreIncrement;

        if (score - lastScoreHighlight > scoreHighlightRange)
        {
            characterSoundController.PlayScoreHighlight();
            //panggil character movement untuk menaikkan max speed player
            characterSoundController.GetComponent<CharacterMovement>().IncreaseMaxSpeed();
            lastScoreHighlight += scoreHighlightRange;
        }
    }

    public void FinishScoring()
    {
        if(score > ScoreData.highScore)
        {
            ScoreData.highScore = score;
        }
    }
}
