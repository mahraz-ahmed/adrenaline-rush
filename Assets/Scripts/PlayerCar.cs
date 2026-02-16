using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    // Materials for each car variant
    public Material defaultMaterial; 
    public Material whiteMaterial;    
    public Material yellowMaterial; 
    public Material blueMaterial;      
    public Material matteBlueMaterial;

    // Reference to 'BodyMain' MeshRenderer to change car material
    private MeshRenderer meshRenderer;

    void Start()
    {
        // Find 'BodyMain' object in hierarchy, contains car MeshRenderer
        Transform bodyMainTransform = transform.Find("Player Car Body/BodyMain");
      
        // Get MeshRenderer component attached to 'BodyMain'
        meshRenderer = bodyMainTransform.GetComponent<MeshRenderer>();
        
        // Load currently equipped car index from PlayerPrefs (default is 0)
        int equippedCar = PlayerPrefs.GetInt("EquippedCar", 0);
        Debug.Log("EquippedCar loaded from PlayerPrefs: " + equippedCar);

        // Apply corresponding material based on equipped car index
        ApplyCarMaterial(equippedCar);
    }

    // Apply car material based on selected car index
    void ApplyCarMaterial(int carIndex)
    {
        Debug.Log("Applying material for car index " + carIndex);

        // Apply correct material based on equipped car index
        switch (carIndex)
        {
            case 0:  // Default car (red)
                Debug.Log("Applying default material.");
                meshRenderer.material = defaultMaterial;
                break;
            case 1:  
                Debug.Log("Applying white material.");
                meshRenderer.material = whiteMaterial;
                break;
            case 2: 
                Debug.Log("Applying yellow material.");
                meshRenderer.material = yellowMaterial;
                break;
            case 3:
                Debug.Log("Applying blue material.");
                meshRenderer.material = blueMaterial;
                break;
            case 4: 
                Debug.Log("Applying matte blue material.");
                meshRenderer.material = matteBlueMaterial;
                break;
        }
    }
}
