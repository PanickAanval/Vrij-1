using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private TextScroll m_textScroll;
    [Space]
    [SerializeField] private string[] m_dialogue;

    [Header("SceneManagement:")]
    [SerializeField] private string m_mainScene;

    private void Start()
    {
        m_textScroll.Activate(m_dialogue, LoadMainScene);
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        m_textScroll.Tick(Time.deltaTime);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(m_mainScene);
    }
}
