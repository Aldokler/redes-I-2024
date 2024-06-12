using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class General_logic : MonoBehaviour
{
    public GameObject game_over_screen;
    public GameObject you_win_screen;
    public GameObject menu_screen;

    public void irAEscena(int sceneID)
    {
        SceneManager.LoadScene(sceneID);

    }

    public void you_lose()
    {
        game_over_screen.SetActive(true);
    }

    public void you_win()
    {
        you_win_screen.SetActive(true);
    }
    public void goToMenu()
    {
        menu_screen.SetActive(true);
    }
}
