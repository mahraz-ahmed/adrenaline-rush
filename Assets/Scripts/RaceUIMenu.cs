using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class RaceUIMenu : MonoBehaviour
{
    // Elements to assign in Inspector
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;

    // Update lap number on UI
    public void UpdateLapText(float lapCount)
    {
        lapText.text = "Lap: " + lapCount.ToString() + "/3";
    }

    // Update race time on UI
    public void UpdateTimeText(float raceTime)
    {
        timeText.text = FormatTime(raceTime);
    }

    // Update speed on UI
    public void UpdateSpeedText(float speed)
    {
        speedText.text = ((int)speed).ToString() + " mph";
    }

    // Format time into (mins):(secs):(milli)
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F); // Convert to mins
        int seconds = Mathf.FloorToInt(time % 60F); // Convert to secs
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F); // Convert to millisecs
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds); // Combine
    }
}
