using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BuildingBlocks.DataTypes;

public class SpriteChars : MonoBehaviour
{
    // Singleton instance
    public static SpriteChars instance;

    // List of chars and color values
    public InspectableDictionary<char, long> charColors;

    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // Set instance to this
            instance = this;

            // Initialize charColors list
            charColors = new InspectableDictionary<char, long>();
        }
        else
        {
            // Destroy duplicate instance
            Destroy(gameObject);
        }

        // Don't destroy on scene change
        DontDestroyOnLoad(gameObject);
        
    }
}