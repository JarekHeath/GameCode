using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefCode : MonoBehaviour
{
    [SerializeField]
    private int score;

    [SerializeField]
    private int highScore;

    public void LoadSettings()
    {
        score = PlayerPrefs.GetInt("score");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("score", score);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
