using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RaceManager : MonoBehaviour
{

    // Define initial variables
    public TextMeshProUGUI countdownText;
    public Canvas RaceUI;
    public Canvas RaceEndUI;
    public Canvas DifficultyUI;
    public RaceUIMenu raceUIMenu; // Reference to Race UI for updating UI elements
    public PosManager posManager;

    private bool isCountingDown = false;
    public static bool raceStarted = false; // Check if race has started
    public bool raceEnded = false;

    public float countdownTime = 3f; // Countdown duration in secs
    public float raceTimer = 0;
    public int coinsTotal; // Player's total coin count
    public int coinsEarnt; // Coins earnt that race

    // String elements
    public string finalPosition;
    public string finalTime;
    public TextMeshProUGUI finalPosHolder;
    public TextMeshProUGUI finalTimeHolder;
    public TextMeshProUGUI finalCoinHolder;

    void Start()
    {
        countdownText.gameObject.SetActive(false); // Hide UI elements before 
        RaceEndUI.gameObject.SetActive(false);
        RaceUI.gameObject.SetActive(false);
        DifficultyUI.gameObject.SetActive(true); // Show diff menu at beginning
        Time.timeScale = 0f; // Pause game initially
        LapCounter.lapCount = 0; // Initialise lap counter
        raceStarted = false;
        coinsEarnt = 0;

    }

    void Update()
    {
        if (isCountingDown == true)
        {
            countdownTime -= Time.deltaTime; // Decrease countdown by time passed

            if (countdownTime <= 0f && raceEnded == false)
            {
                countdownTime = 0f; // Make sure time isn't negative
                StartRace(); // Start race when countdown finishes
            }

            // Update the countdown display
            countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Use Mathf.Ceil to only show whole nums
        }

        if (LapCounter.lapCount == 4 && !raceEnded)
        {
            EndRace(); // If lap 3 ended, conclude race
        }

    }

    public void StartCountdown()
    {
        isCountingDown = true; // Begin countdown
        countdownText.gameObject.SetActive(true); // Make sure countdown is visible at the start
        RaceUI.gameObject.SetActive(false); // Hide Race UI before race starts

    }

    void StartRace()
    {
        countdownText.gameObject.SetActive(false); // Hide countdown text when finished
        RaceUI.gameObject.SetActive(true); // Show Race UI upon race start
        raceStarted = true; // Indicate race has started
        raceTimer += Time.deltaTime;

        raceUIMenu.UpdateLapText(LapCounter.lapCount); // Fetch lap count
        raceUIMenu.UpdateTimeText(raceTimer);          // Fetch timer
        raceUIMenu.UpdateSpeedText(CarController.speed); // Fetch speed
    }

    void EndRace()
    {
        isCountingDown = false;
        countdownText.gameObject.SetActive(true); // Make sure countdown is visible at the start
        raceStarted = false; // Alert program that race has ended and act accordingly
        raceEnded = true;
        countdownText.text = "Race End!";
        RaceUI.gameObject.SetActive(false); // Hide Race UI after race ends
        Invoke("ShowEndMenu", 3f);

        finalPosition = posManager.positionText.text;  // Get final position
        finalTime = raceUIMenu.timeText.text;  // Get race time 
        finalPosHolder.text = finalPosition;
        finalTimeHolder.text = finalTime;
        LoadCoins(); // Load player's coin balance
        EarnCoins(finalPosition); // Add coins to balance
        PlayerPrefs.SetInt("PlayerCoins", coinsTotal); // Save player's coin count
        finalCoinHolder.text = ("Coins Earnt: " + coinsEarnt); // Display coins earnt this race

        // Save final position and time to PlayerPrefs for use in Progress Menu
        PlayerPrefs.SetString("FinalPosition", finalPosition);
        PlayerPrefs.SetString("FinalTime", finalTime);
        PlayerPrefs.Save(); 
    }

    void ShowEndMenu()
    {
        RaceEndUI.gameObject.SetActive(true);
    }

    public void EarnCoins(string racePosition)
    {
        if (racePosition == "Pos: 1")
        {
            coinsTotal += 100; // Add coins to player balance
            coinsEarnt = 100; // Display coins acquired this race
            return;
        } 
        else if (racePosition == "Pos: 2")
        {
            coinsTotal += 50;
            coinsEarnt = 50;
            return;
        }
        else if (racePosition == "Pos: 3")
        {
            coinsTotal += 25;
            coinsEarnt = 25;
            return;
        }
        else
        {
            coinsTotal += 10; // Lower than 3rd place gets 10 coins
            coinsEarnt = 10;
            return;
        }
    }

    public void LoadCoins()
    {
       
        coinsTotal = PlayerPrefs.GetInt("PlayerCoins", 0); // Default to 0 if no coins are saved
        Debug.Log("Coins Balance: " + coinsTotal);
    }

}