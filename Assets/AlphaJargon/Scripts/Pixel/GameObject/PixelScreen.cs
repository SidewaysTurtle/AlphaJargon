using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using BuildingBlocks.DataTypes;

using PixelGame;

public class PixelScreen : MonoBehaviour, IPixelObject
{
    public delegate void onPixelScreenChangeDelegate(PixelGameObject parent,PixelScreen pixelScreen);
    public static onPixelScreenChangeDelegate onPixelScreenCreateEvent, onPixelScreenDeleteEvent;
    void OnEnable()
    {
        gridLayout = gameObject.GetComponent<GridLayoutGroup>();
        CellSize = (int)gridLayout.cellSize.x;
        GridSideSize = (int)gridLayout.constraintCount;
    }
    // Every Physical Pixel 
    public GridLayoutGroup gridLayout;
    public InspectableDictionary<int, Pixel> grid;
    public static int CellSize;
    public static int GridSideSize;

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
}
