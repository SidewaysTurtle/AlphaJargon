using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using PixelGame;

[MoonSharp.Interpreter.MoonSharpUserData]
public class PixelTextBox : PixelGameObject
{
    public TextMeshProUGUI[,] Textbox = new TextMeshProUGUI[8,8];

    public TextMeshProUGUI InstantiateContent(string content, int x, int y)
    {
        TextMeshProUGUI box = Textbox[x,y];
        PixelTransform pixelTransform = add("Transform","PixelTransform");
        
        x *= PixelScreen.CellSize; y *= PixelScreen.CellSize;

        box = gameObject.AddComponent<TextMeshProUGUI>();
        box.font = Resources.Load<TMP_FontAsset>("TextMeshPro/AprilSans");
        box.alignment = TextAlignmentOptions.Center;
        box.autoSizeTextContainer = true;
        
        pixelTransform.move(new PixelPosition(x,y));

        box.text = content;

        box.fontSize = 50;
        box.color = new Color(0,0,0);
        
        box.ForceMeshUpdate();

        return box;
    }
    
    
}
