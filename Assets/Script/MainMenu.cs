using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {

    }
    public void StartButton()
    {
        SceneManager.LoadScene("l");
    }

    public void ExitButton()
    {
        Application.Quit();   
         
    }

    public void ToMenu ()
    {
        SceneManager.LoadScene("Menu");
    }
}
