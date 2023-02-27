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
        
        x *= 100; y *= 100;

        box = gameObject.AddComponent<TextMeshProUGUI>();
        box.font = Resources.Load<TMP_FontAsset>("TextMeshPro/AprilSans");
        box.alignment = TextAlignmentOptions.Center;
        box.autoSizeTextContainer = true;
        
        // FIXME:
        // this is gross but ill work for now
        box.transform.localPosition = new Vector3(-350,-350);
        box.transform.localPosition += new Vector3(x,y);

        box.text = content;

        box.fontSize = 50;
        box.color = new Color(0,0,0);
        
        box.ForceMeshUpdate();

        return box;
    }
    
    
}
