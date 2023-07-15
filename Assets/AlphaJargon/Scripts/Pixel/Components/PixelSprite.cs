using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using MoonSharp.Interpreter;

using UnityEngine;
using UnityEngine.UI;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelSprite : PixelComponent
    {
        // Psuedo Sprite
        public PixelScreen screen; // Only use this to show on screen
        public override PixelGameObject parent{get;set;}
        void OnEnable()
        {
            AlphaJargon.startGameEvent += AddScreenToScreenManager;
        }
        void OnDisable()
        {
            AlphaJargon.startGameEvent -= AddScreenToScreenManager;
        }
        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
            screen = Instantiate<PixelScreen>(Resources.Load<PixelScreen>("Prefabs/Game/PixelScreen"),parent.gameObject.transform);
        }
        public void AddScreenToScreenManager()
        {
            PixelScreen.onPixelScreenCreateEvent?.Invoke(parent,screen);
        }
        public override void Remove()
        {
            PixelScreen.onPixelScreenDeleteEvent?.Invoke(parent,screen);
            Destroy(screen);
            Destroy(this);
        }
        public PixelSprite add(DynValue dynValue)
        {
            string inputString = new string(dynValue.ToString()
                .Where(c => !Char.IsWhiteSpace(c) && c != '\"')
                .ToArray());
            var outputStrings = Enumerable.Range(0, inputString.Length / PixelScreen.GridSideSize)
                .Select(i => inputString.Substring(i * PixelScreen.GridSideSize, PixelScreen.GridSideSize));

            string[] stringArray = outputStrings.ToArray();

            Array.Reverse(stringArray); 
            StringBuilder sb = new StringBuilder();

            foreach (var str in stringArray)
            {
                sb.Append(str);
            }
            string reversedString = sb.ToString();
            // slow
                // char[] charArray = reversedString.ToCharArray();
                // Array.Reverse(charArray);
            string finalString = new string(reversedString);
            return add(finalString);
        }
        public PixelSprite add(string SpriteString)
        {
            if(SpriteString != "")
            {
                AddSpriteStringToScreen(SpriteString);
            }
            else
            {
                // Slow
                    // AddSpriteStringToScreen(String.Concat(Enumerable.Repeat("o", screen.grid.Count)));
                string oString = "";
                for (int i = 0; i < screen.grid.Count; i++)
                    oString += "o";
                AddSpriteStringToScreen(oString);
            }
            return this;
        }
        public PixelScreen AddSpriteStringToScreen(string SpriteString)
        {
            // slow
                // Enumerable.Range(0, SpriteString.Length)
                //     .ToList()
                //     .ForEach(index => CharToPixel(screen.grid[index], SpriteString[index])
                // );

            for(int index = 0; index < SpriteString.Length; index++)
                CharToPixel(screen.grid[index], SpriteString[index]);
            return screen;
        }
        // Use pseudo signed bit of 1
        private void CharToPixel(Pixel pixel, char letter)
        {
            if (System.Enum.TryParse<PixelColor>(letter.ToString().ToLower(), out PixelColor pixelColor))
            {
                pixel.Image.color = RGBToColor((long)pixelColor);
            }
            pixel.Sprite = this;
        }

        public static Color RGBToColor(long rgb)
        {  
            //    r   g   b
            // 1 000 000 000
            byte r = byte.Parse(rgb.ToString().Substring(1,3), System.Globalization.NumberStyles.Integer);
            byte g = byte.Parse(rgb.ToString().Substring(4,3), System.Globalization.NumberStyles.Integer);
            byte b = byte.Parse(rgb.ToString().Substring(7,3), System.Globalization.NumberStyles.Integer);
            byte a = byte.Parse(rgb.ToString().Substring(10,3), System.Globalization.NumberStyles.Integer);
            return new Color32(r,g,b,a);
        }
    }
}

public enum PixelColor : long
{
    o = 1000000000000,
    p = 1255160122255,
    y = 1255255000255,
    b = 1164219232255
}