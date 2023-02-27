using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelText : PixelComponent
    {
        PixelTextBox textBox;
        TextMeshProUGUI TextBox;
        public string content;

        public PixelText add(string content, PixelPosition PP)
        {
            return add(content, PP.x, PP.y);
        }
        public PixelText add(string content, int x, int y)
        {
            this.content = content;
            textBox.InstantiateContent(content, x, y);
            return this;
        }

        public override void Create(PixelGameObject parent)
        {
            // FIXME: add to PixelGameObject instead as "Child"
            textBox = Instantiate<PixelTextBox>(Resources.Load<PixelTextBox>("Prefabs/Game/PixelTextBox"),parent.gameObject.transform);
        }
    }
}