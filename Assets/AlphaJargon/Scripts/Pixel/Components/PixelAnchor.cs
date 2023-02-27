using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    public class PixelAnchor : PixelComponent
    {
        public PixelPosition position;
        public PixelSprite sprite;

        public override void Create(PixelGameObject parent)
        {
            position = new PixelPosition(0,0);
        }

        public void add(PixelSprite ps, PixelPosition pp)
        {
            sprite = ps;
            position = pp;
        }
    }
}