using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScore : MonoBehaviour
{
    private Animator AnimBestScore;
    void Start()
    {
        AnimBestScore = GetComponent<Animator>();
    }
    void Update()
    {
        if (GameObject.Find("Game Controller").GetComponent<GameController>().AnimScoreStart == true)
            AnimBestScore.SetBool("BestNew", true);
    }
}
