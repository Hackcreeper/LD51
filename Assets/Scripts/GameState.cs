using Ui;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool Started = false;

    public GameObject mainMenu;
    public Tutorial tutorial;
    
    public void StartGame()
    {
        Started = true;
        mainMenu.SetActive(false);
        tutorial.StartTutorial();
    }
}
