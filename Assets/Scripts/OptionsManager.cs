using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    // UI and audio mixer references
    public AudioMixer audioMixer; 
    public Slider masterSlider, bgmSlider, sfxSlider; 

    // UI elements for graphics quality and UI scale
    public Button lowQualityButton, mediumQualityButton, highQualityButton; // Buttons for setting graphics quality
    public Slider uiScaleSlider; 
    public CanvasScaler canvasScaler; // Reference to CanvasScaler for dynamic UI scaling

    void Start()
    {
        LoadSettings(); // Load saved settings (graphics quality and UI scale) on start
        ApplyVolume(); // Apply loaded volume settings to Audio Mixer

        // Manually add listeners to sliders for volume control
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        // Add event listeners for quality buttons
        lowQualityButton.onClick.AddListener(() => SetQuality(0));
        mediumQualityButton.onClick.AddListener(() => SetQuality(1));
        highQualityButton.onClick.AddListener(() => SetQuality(2)); // Set quality to High
        uiScaleSlider.onValueChanged.AddListener(SetUIScale); // Adjust UI scale dynamically
    }

    // Volume control methods
    public void SetMasterVolume(float volume)
    {
        Debug.Log($"Setting Master Volume to: {volume}");
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
        Debug.Log($"Master Volume saved to PlayerPrefs: {volume}");
    }

    public void SetBGMVolume(float volume)
    {
        Debug.Log($"Setting BGM Volume to: {volume}");
        audioMixer.SetFloat("BGMVolume", volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
        Debug.Log($"BGM Volume saved to PlayerPrefs: {volume}");
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log($"Setting SFX Volume to: {volume}");
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
        Debug.Log($"SFX Volume saved to PlayerPrefs: {volume}");
    }

    // Apply loaded volume settings to Audio Mixer
    public void ApplyVolume()
    {
        SetMasterVolume(masterSlider.value);
        SetBGMVolume(bgmSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    // Graphics quality control
    public void SetQuality(int qualityIndex)
    {
        Debug.Log($"Setting Graphics Quality to index: {qualityIndex}");
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("GraphicsQuality", qualityIndex);
        PlayerPrefs.Save();
        Debug.Log($"Graphics Quality saved to PlayerPrefs: {QualitySettings.names[qualityIndex]} (Index: {qualityIndex})");
    }

    // UI scale control
    public void SetUIScale(float scale)
    {
        Debug.Log($"Setting UI Scale to: {scale}");
        canvasScaler.scaleFactor = scale;
        PlayerPrefs.SetFloat("UIScale", scale);
        PlayerPrefs.Save();
        Debug.Log($"UI Scale saved to PlayerPrefs: {scale}");
    }

    // Load saved settings (graphics quality and UI scale)
    void LoadSettings()
    {
        Debug.Log("Loading volume settings from PlayerPrefs.");
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0); // Default to 0 if not saved
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0); 
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0);
        Debug.Log($"Loaded Master: {masterSlider.value}, BGM: {bgmSlider.value}, SFX: {sfxSlider.value}");

        // Load graphics quality
        int savedQuality = PlayerPrefs.GetInt("GraphicsQuality", QualitySettings.GetQualityLevel()); // Default to current quality level
        Debug.Log($"[AudioSettings] Loaded Graphics Quality: {QualitySettings.names[savedQuality]} (Index: {savedQuality})");
        QualitySettings.SetQualityLevel(savedQuality); // Apply saved quality level

        // Load UI scale
        float savedScale = PlayerPrefs.GetFloat("UIScale", 1.0f); // Default to 1.0 if not saved
        Debug.Log($"[AudioSettings] Loaded UI Scale: {savedScale}");
        uiScaleSlider.value = savedScale; // Set slider value
        canvasScaler.scaleFactor = savedScale; // Apply saved UI scale
    }
}