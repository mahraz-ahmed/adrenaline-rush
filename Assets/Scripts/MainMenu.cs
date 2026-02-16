using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void QuickRaceButton()
    {
        SceneManager.LoadScene("Quick Race");
        /* Uses 'SceneManager' class built into
        Unity to load main game scene */
    }

    public void ExitButton()
    {
        Application.Quit();
        /* Closes application when running as 
        standalone app via Unity 'Application' 
        class */
        Debug.Log("Player quit");
        // Sends a log to console for testing
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }

    public void GarageButton()
    {
        SceneManager.LoadScene("Garage");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void ProgressButton()
    {
        SceneManager.LoadScene("Progress");
    }
}
