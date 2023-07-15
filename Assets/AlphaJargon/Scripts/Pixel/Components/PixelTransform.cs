using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Habrador_Computational_Geometry;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelTransform : PixelComponent
    {
        public override PixelGameObject parent{get;set;}

        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
        }

        public PixelTransform add(int x, int y, bool ActivateEvent = true)
        {
            return add(new PixelPosition(x,y), ActivateEvent);
        }

        public PixelTransform add(PixelPosition pp /*hehe*/, bool ActivateEvent = true)
        {
            move(pp, ActivateEvent);
            return this;
        }

        public PixelPosition move(PixelPosition pixelPosition, bool ActivateEvent = true)
        {
            return move(pixelPosition.x, pixelPosition.y, ActivateEvent);
        }

        public PixelPosition move(int x, int y, bool ActivateEvent = true)
        {
            Vector3 trans = new Vector3(x * PixelScreen.CellSize, y * PixelScreen.CellSize);
            PixelPosition translation = new PixelPosition(x,y);
            if(!PixelCollider.CheckCollision(translation, parent, ActivateEvent))
            {
                transform.Translate(trans);
                parent.position += translation;
            }
            return parent.position;
        }

        public override void Remove()
        {
            Destroy(this);
        }
    }
}