using UnityEngine;

public class GameState : MonoBehaviour
{
    public static bool Started = false;

    public GameObject mainMenu;
    
    public void StartGame()
    {
        Debug.Log("Lets go!");
        Started = true;
        mainMenu.SetActive(false);
    }
}
