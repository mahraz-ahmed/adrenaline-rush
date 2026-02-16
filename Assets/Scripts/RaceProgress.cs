using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RaceProgress : MonoBehaviour
{
    // Class to store individual race results
    [System.Serializable]
    public class RaceResult
    {
        public string date;  
        public string place;
        public string time;  
    }

    // List to store all race results
    public List<RaceResult> raceResults = new List<RaceResult>();

    // UI references
    public Transform contentPanel; 
    public GameObject rowPrefab; 

    // PlayerPrefs keys for saving and loading data
    private const string SaveKey = "RaceProgressData";   // Key for saving Progress Menu data
    private const string FinalPositionKey = "FinalPosition"; // Key for saving final race position
    private const string FinalTimeKey = "FinalTime";     // Key for saving final race time

    void Start()
    {
        LoadProgress();      // Load previous race progress from PlayerPrefs
        FetchRaceResults();  // Check if there's new race data to add
        DisplayProgress();   // Display all race results to UI
    }

    // Fetch latest race results from PlayerPrefs (if available)
    public void FetchRaceResults()
    {
        // Check if race position and time exist in PlayerPrefs
        if (PlayerPrefs.HasKey(FinalPositionKey) && PlayerPrefs.HasKey(FinalTimeKey))
        {
            string finalPosition = PlayerPrefs.GetString(FinalPositionKey);
            string finalTime = PlayerPrefs.GetString(FinalTimeKey);

            AddNewResult(finalPosition, finalTime); // Add new result

            // Clear saved values after fetch so they don't persist unnecessarily
            PlayerPrefs.DeleteKey(FinalPositionKey);
            PlayerPrefs.DeleteKey(FinalTimeKey);
            PlayerPrefs.Save();
        }
    }

    // Add new race result to list and update UI & storage
    public void AddNewResult(string place, string time)
    {
        RaceResult newResult = new RaceResult
        {
            date = DateTime.Now.ToString("dd/MM/yyyy"), // Store current date
            place = place,
            time = time
        };

        raceResults.Add(newResult); // Add to list
        SaveProgress();             // Save updated list
        DisplayProgress();          // Refresh UI to show new result
    }

    // Save race progress to PlayerPrefs in JSON format
    void SaveProgress()
    {
        string json = JsonUtility.ToJson(new SaveData(raceResults)); // Convert list to JSON
        PlayerPrefs.SetString(SaveKey, json); // Save to PlayerPrefs
        PlayerPrefs.Save();
    }

    // Load race progress from PlayerPrefs if it exists
    void LoadProgress()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey); // Get JSON data
            SaveData data = JsonUtility.FromJson<SaveData>(json); // Convert back to object
            raceResults = data.results; // Restore list from saved data
        }
    }

    // Display all race results in UI
    public void DisplayProgress()
    {
        // Clear existing UI rows before populating new ones
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Instantiate and populate a row for each race result
        foreach (RaceResult result in raceResults)
        {
            GameObject row = Instantiate(rowPrefab, contentPanel); // Create row in content panel
            TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();

            // Assign text values (date, position, and time)
            texts[0].text = result.date;
            texts[1].text = result.place;
            texts[2].text = result.time;
        }
    }

    // Class for saving race results in PlayerPrefs
    [System.Serializable]
    private class SaveData
    {
        public List<RaceResult> results; // List of results to be stored

        // Constructor to initialise save data
        public SaveData(List<RaceResult> raceResults) => results = raceResults;
    }
}
