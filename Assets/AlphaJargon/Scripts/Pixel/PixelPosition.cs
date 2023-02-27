using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelPosition
    {
        public int x;
        public int y;

        public PixelPosition()
        {
            x = 0;
            y = 0;
        }

        public PixelPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}