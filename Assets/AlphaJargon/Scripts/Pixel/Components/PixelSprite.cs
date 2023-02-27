using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelSprite : PixelComponent
    {
        // Psuedo Sprite
        PixelScreen sprite; // Only use this to show on screen
        public string SpriteString = "oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo";
        public PixelSprite add(string SpriteString)
        {
            if(SpriteString != "")
            {
                this.SpriteString = CombineString(SpriteString,this.SpriteString);
                sprite.ConvertSpriteStringToScreen(this.SpriteString);
            }
            return this;
        }

        public override void Create(PixelGameObject parent)
        {
            // FIXME: add to PixelGameObject instead as "Child"
            sprite = Instantiate<PixelScreen>(Resources.Load<PixelScreen>("Prefabs/Game/PixelScreen"),parent.gameObject.transform);
        }

        string CombineString(string s1, string s2)
        {
            string s3 = "";
            for (int i = 0; i < 64; i++)
            {
                if (s1[i] == 'o' && s2[i] == 'o')
                {
                    s3 += "o";
                }
                else if (s1[i] == 'o' && s2[i] != 'o')
                {
                    s3 += s2[i];
                }
                else if (s1[i] != 'o' && s2[i] == 'o')
                {
                    s3 += s1[i];
                }
                else
                {
                    s3 += s1[i];
                }
            }
            return s3;
        }
    }
}