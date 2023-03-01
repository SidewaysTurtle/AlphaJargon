using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelText : PixelComponent
    {
        public override PixelGameObject parent{get;set;}
        PixelTextBox pixelTextBox;
        TextMeshProUGUI TextBox;
        public string content;

        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
            pixelTextBox = Instantiate<PixelTextBox>(Resources.Load<PixelTextBox>("Prefabs/Game/PixelTextBox"),parent.gameObject.transform);
        }
        public PixelText add(string content, PixelPosition PP)
        {
            return add(content, PP.x, PP.y);
        }
        public PixelText add(string content, int x, int y)
        {
            this.content = content;
            pixelTextBox.InstantiateContent(content, x, y);
            return this;
        }
        public override void Remove()
        {
            Destroy(pixelTextBox);
            Destroy(this);
        }
    }
}