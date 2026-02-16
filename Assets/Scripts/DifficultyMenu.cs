using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class DifficultyMenu : MonoBehaviour
{
    public Canvas DifficultyUI; // Reference to difficulty selection menu
    public RaceManager raceManager; // Reference to RaceManager

    // Hold selected difficulty
    public static string selectedDifficulty;

    public void SetEasy()
    {
        selectedDifficulty = "easy";
        Debug.Log("Difficulty selected: " + selectedDifficulty);
        StartCountdown(); // Start countdown after difficulty selection
    }

    public void SetMedium()
    {
        selectedDifficulty = "medium";
        Debug.Log("Difficulty selected: " + selectedDifficulty);
        StartCountdown(); 
    }

    public void SetHard()
    {
        selectedDifficulty = "hard";
        Debug.Log("Difficulty selected: " + selectedDifficulty);
        StartCountdown(); 
    }

    // Hide difficulty menu and start countdown
    void StartCountdown()
    {
        Time.timeScale = 1f;
        DifficultyUI.gameObject.SetActive(false); // Hide difficulty menu
        raceManager.StartCountdown(); // Trigger countdown in RaceManager
    }
}
