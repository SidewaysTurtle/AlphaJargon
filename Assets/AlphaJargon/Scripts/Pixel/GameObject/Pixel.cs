using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelGame;

// Physical Pixels
public class Pixel : PixelGameObject
{
    public bool isOn
    {
        get
        {
            return Image.color.a == 255f;
        }
        set
        {
            Color C = Image.color;
            C.a = value ? 255f : 0f; 
            Image.color = C;
        }
    }
    public Image Image;
    Color RGBToColor(long rgb){ //000 000 000
        byte r = byte.Parse(rgb.ToString().Substring(1,3), System.Globalization.NumberStyles.Integer);
        byte g = byte.Parse(rgb.ToString().Substring(4,3), System.Globalization.NumberStyles.Integer);
        byte b = byte.Parse(rgb.ToString().Substring(7,3), System.Globalization.NumberStyles.Integer);
        byte a = byte.Parse(rgb.ToString().Substring(10,3), System.Globalization.NumberStyles.Integer);
        return new Color32(r,g,b,a);
    }

    // Use pseudo signed bit of 1
    enum PixelColor : long
    {
        o = 1000000000000,
        r = 1255160122255,
        c = 1255255000255,
        b = 1164219232255
    }
    public void CharToPixel(char letter)
    {
        if (System.Enum.TryParse<PixelColor>(letter.ToString().ToLower(), out PixelColor pixelColor))
        {
            Image.color = RGBToColor((long)pixelColor);
        }
    }
}
