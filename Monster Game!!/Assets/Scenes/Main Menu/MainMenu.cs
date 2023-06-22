using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string m_introScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(m_introScene);
    }

    public void Quit()
    {
        Debug.Log("Doei");
        Application.Quit();
    }
}