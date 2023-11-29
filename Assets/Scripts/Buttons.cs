using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{
    
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }

    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void New(bool answer)
    {
        if (answer)
        {
            PlayerPrefs.SetInt("New", 0);
        }
        else
        {
            PlayerPrefs.SetInt("New", 1);
        }

    }

    public void File(int file)
    {
        PlayerPrefs.SetInt("File", file);
    }
}