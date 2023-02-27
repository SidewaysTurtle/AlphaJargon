using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelTransform : PixelComponent
    {
        PixelGameObject self;
        PixelPosition position;

        public override void Create(PixelGameObject parent)
        {
            position = new PixelPosition(0,0);
        }

        public PixelTransform add(PixelPosition pp /*hehe*/)
        {
            position = pp;
            move(pp);
            return this;
        }

        public PixelTransform add(int x, int y)
        {
            return add(new PixelPosition(x,y));
        }

        public PixelPosition move(PixelPosition pixelPosition)
        {
            return move(pixelPosition.x, pixelPosition.y);
        }

        public PixelPosition move(int x, int y)
        {
            // FIXME:
            // funky stuff happens when I try to make this 
            // gameobject.transform.Translate
            // no idea why
            PixelPosition target = new PixelPosition(x,y);
            transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition,new Vector3(target.x,target.y), Time.deltaTime * 1f);
            position = new PixelPosition((int)(gameObject.transform.localPosition.x / 100f),(int)(gameObject.transform.localPosition.y / 100f));
            return position;
        }
    }
}