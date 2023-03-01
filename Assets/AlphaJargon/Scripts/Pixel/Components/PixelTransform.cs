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
        public delegate void OnWinLevel();
        public static OnWinLevel OnWinLevelEvent;

        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
        }
        public PixelTransform add(PixelPosition pp /*hehe*/)
        {
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
            Vector3 trans = new Vector3(x * PixelScreen.CellSize,y * PixelScreen.CellSize);
            PixelPosition translation = new PixelPosition(x,y);
            if(!CheckCollision(translation))
            {
                transform.Translate(trans);
                parent.position += translation;
            }
            return parent.position;
        }

        private bool CheckCollision(PixelPosition translation)
        {
            // List<KeyValuePair<PixelPosition, Pixel>>
            List<KeyValuePair<PixelPosition, Pixel>> selfpixels = PixelScreenManager.Instance.GetPixelsWithCollider(parent, translation);
            List<KeyValuePair<PixelPosition, Pixel>> otherpixels = PixelScreenManager.Instance.GetPixelsWithColliderOtherThan(parent);


            foreach(KeyValuePair<PixelPosition, Pixel> self in selfpixels)
            {
                foreach(KeyValuePair<PixelPosition, Pixel> other in otherpixels)
                {
                    if(self.Key == other.Key)
                    {
                        if(other.Value.Collider.isTrigger)
                        {
                            // foreach(Pixel sprite in PixelScreenManager.Instance.GetSpritePixelsAtPosition(other.Key))
                            // {
                            //     if(sprite.isWin)
                            //     {
                            //         PixelTransform.OnWinLevelEvent?.Invoke();
                            //         return false;
                            //     }
                            // }
                            PixelCollider.onTriggerEvent?.Invoke(other.Value, parent);
                            return false;
                        }
                        else
                        {
                            PixelCollider.onCollisionEvent?.Invoke(other.Value, parent);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override void Remove()
        {
            Destroy(this);
        }
    }
}