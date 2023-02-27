using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using BuildingBlocks.DataTypes;

using PixelGame;

public class PixelScreen : PixelGameObject
{
    // Every Physical Pixel 
    public InspectableDictionary<int, Pixel> grid;

    public Pixel this[int index]
    {
        get
        {
            return grid[index];
        }
        set
        {
            grid[index] = value;
        }
    }

    public PixelScreen ConvertSpriteStringToScreen(string SpriteString)
    {
        for(int index = 0; index < SpriteString.Length; index++)
        {
            try
            {
                grid[index].CharToPixel(SpriteString[index]);
            }
            catch(System.Exception e)
            {
                Debug.Log($"SpriteString must be 64 Characters long.\n{e}");
            }            
        }
        return this;
    }
}
