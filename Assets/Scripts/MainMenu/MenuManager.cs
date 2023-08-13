using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateMouse();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("AlexScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ActivateMouse()
    {
        if (Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            
        }
    }
}
