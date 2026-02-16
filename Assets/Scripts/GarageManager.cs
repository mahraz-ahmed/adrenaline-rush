using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GarageManager : MonoBehaviour
{
    // Display player balance
    public TextMeshProUGUI balanceText;

    // Buttons for each car: Buy/Equip
    public Button whiteCarBuyButton, whiteCarEquipButton;
    public Button yellowCarBuyButton, yellowCarEquipButton;
    public Button blueCarBuyButton, blueCarEquipButton;
    public Button matteBlueCarBuyButton, matteBlueCarEquipButton;
    public Button redCarEquipButton; 

    // Text fields for buy/equip buttons of each car
    public TextMeshProUGUI whiteCarBuyText, whiteCarEquipText;
    public TextMeshProUGUI yellowCarBuyText, yellowCarEquipText;
    public TextMeshProUGUI blueCarBuyText, blueCarEquipText;
    public TextMeshProUGUI matteBlueCarBuyText, matteBlueCarEquipText;
    public TextMeshProUGUI redCarEquipText;

    private int playerCoins; // Player's coin balance (saved to PlayerPrefs)
    private int equippedCar; // The index of the currently equipped car (0 = red, 1 = white, etc.)

    void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("PlayerCoins", 10000);
        PlayerPrefs.Save();
        // Load save data for coins and current equipped car
        LoadGarageData();


        // Manually assign OnClick listeners for Buy buttons
        whiteCarBuyButton.onClick.AddListener(() => BuyCar(1, 1000, whiteCarBuyButton, whiteCarBuyText));
        yellowCarBuyButton.onClick.AddListener(() => BuyCar(2, 2000, yellowCarBuyButton, yellowCarBuyText));
        blueCarBuyButton.onClick.AddListener(() => BuyCar(3, 3000, blueCarBuyButton, blueCarBuyText));
        matteBlueCarBuyButton.onClick.AddListener(() => BuyCar(4, 5000, matteBlueCarBuyButton, matteBlueCarBuyText));

        // Manually assign OnClick listeners for Equip buttons
        redCarEquipButton.onClick.AddListener(() => EquipCar(0, redCarEquipText));  // Equip for red car (always free)
        whiteCarEquipButton.onClick.AddListener(() => EquipCar(1, whiteCarEquipText));
        yellowCarEquipButton.onClick.AddListener(() => EquipCar(2, yellowCarEquipText));
        blueCarEquipButton.onClick.AddListener(() => EquipCar(3, blueCarEquipText));
        matteBlueCarEquipButton.onClick.AddListener(() => EquipCar(4, matteBlueCarEquipText));
    }

    // Method to load save data for player coins and current equipped car
    void LoadGarageData()
    {
        // Retrieve information from PlayerPrefs
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0); // Default to 0 if no value exists
        equippedCar = PlayerPrefs.GetInt("EquippedCar", 0); // Default to red car (index 0)

        Debug.Log("PlayerCoins = " + playerCoins);
        Debug.Log("EquippedCar = " + equippedCar);

        // Update balance on UI
        UpdateBalanceUI();

        // Update buy buttons for each car if already bought
        UpdateBuyButtons(1, whiteCarBuyButton, whiteCarBuyText);
        UpdateBuyButtons(2, yellowCarBuyButton, yellowCarBuyText);
        UpdateBuyButtons(3, blueCarBuyButton, blueCarBuyText);
        UpdateBuyButtons(4, matteBlueCarBuyButton, matteBlueCarBuyText);

        // Update equip buttons to show the correct status
        UpdateEquipButtons();
    }

    // Method to update UI for buy buttons
    void UpdateBuyButtons(int carIndex, Button buyButton, TextMeshProUGUI buyText)
    {
        // If car is already bought (saved to PlayerPrefs), disable buy button and mark as "Bought"
        if (PlayerPrefs.GetInt("CarBought_" + carIndex, 0) == 1)
        {
            Debug.Log("Car " + carIndex + " is already bought.");
            buyButton.interactable = false;
            buyText.text = "Bought"; 
        }
    }

    // Method to handle buying a car
    public void BuyCar(int carIndex, int price, Button buyButton, TextMeshProUGUI buyText)
    {
        // Check if player has enough coins to buy the car
        if (playerCoins >= price)
        {
            Debug.Log("Player has enough coins to buy car " + carIndex);

            // Deduct coins and save purchase status to PlayerPrefs
            playerCoins -= price;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins); // Update coin balance
            PlayerPrefs.SetInt("CarBought_" + carIndex, 1); // Mark car as bought in PlayerPrefs
            PlayerPrefs.Save(); // Save PlayerPrefs

            // Update UI to acknowledge purchase
            buyButton.interactable = false;
            buyText.text = "Bought";
            UpdateBalanceUI(); 
            UpdateEquipButtons();
        }
        else
        {
            // Send message to console if balance is insufficient
            Debug.Log("Not enough coins to buy car " + carIndex);
        }
    }

    // Method to handle equipping a car
    public void EquipCar(int carIndex, TextMeshProUGUI equipText)
    {
        // Red car (index 0) always free, no need to check for purchase status
        if (carIndex == 0 || PlayerPrefs.GetInt("CarBought_" + carIndex, 0) == 1)
        {
            Debug.Log("Car " + carIndex + " is owned. Equipping...");

            // Equip selected car
            equippedCar = carIndex;
            PlayerPrefs.SetInt("EquippedCar", equippedCar); // Save equipped car index
            PlayerPrefs.Save(); // Save PlayerPrefs
            Debug.Log("EquippedCar set to " + equippedCar + " and saved to PlayerPrefs.");
            UpdateEquipButtons(); // Acknowledge equip on UI
        }
        else
        {
            Debug.Log("Car " + carIndex + " not owned. Cannot equip.");
        }
    }

    // Method to update UI for equip buttons
    void UpdateEquipButtons()
    {
        redCarEquipText.text = "Equip";  // Red car is always available to equip
        redCarEquipButton.interactable = true;

        // Update equip buttons for other cars based on whether they are owned
        whiteCarEquipText.text = "Equip";
        whiteCarEquipButton.interactable = PlayerPrefs.GetInt("CarBought_1", 0) == 1;

        yellowCarEquipText.text = "Equip";
        yellowCarEquipButton.interactable = PlayerPrefs.GetInt("CarBought_2", 0) == 1;

        blueCarEquipText.text = "Equip";
        blueCarEquipButton.interactable = PlayerPrefs.GetInt("CarBought_3", 0) == 1;

        matteBlueCarEquipText.text = "Equip";
        matteBlueCarEquipButton.interactable = PlayerPrefs.GetInt("CarBought_4", 0) == 1;

        // Set equip button text to "Done" for current equipped car
        switch (equippedCar)
        {
            case 0:
                Debug.Log("Red car is equipped.");
                redCarEquipText.text = "Done"; 
                break;
            case 1:
                Debug.Log("White car is equipped.");
                whiteCarEquipText.text = "Done"; 
                break;
            case 2:
                Debug.Log("Yellow car is equipped.");
                yellowCarEquipText.text = "Done"; 
                break;
            case 3:
                Debug.Log("Blue car is equipped.");
                blueCarEquipText.text = "Done"; 
                break;
            case 4:
                Debug.Log("Matte blue car is equipped.");
                matteBlueCarEquipText.text = "Done";
                break;
        }
    }

    // Method to update UI for player coin balance
    void UpdateBalanceUI()
    {
        // Update balance text on UI
        balanceText.text = "Balance: " + playerCoins + " coins";
        Debug.Log("Balance updated to " + playerCoins + " coins.");
    }
}
