using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habrador_Computational_Geometry;

using MoonSharp.Interpreter.Interop;

namespace PixelGame
{
    public class PixelCollider : PixelComponent
    {
        public delegate void OnTriggerDelegate(Collider2D other, PixelGameObject parent);
        public static OnTriggerDelegate onTriggerEnter, onTriggerStay, onTriggerExit;
        public delegate void OnCollisionDelegate(Collision2D other, PixelGameObject parent);
        public static OnCollisionDelegate onCollisionEnter, onCollisionStay, onCollisionExit;

        //
        PixelGameObject parent;
        List<PolygonCollider2D> pixelCollider;

        public override void Create(PixelGameObject parent)
        {
            pixelCollider = new List<PolygonCollider2D>();
            this.parent = parent;
        }

        public PixelComponent add(List<PixelPosition> pixelPositions, bool isTrigger = false)
        {
            // convert all the pixel positions to coords
            List<MyVector2> Points = new List<MyVector2>();
            foreach (PixelPosition pixelPosition in pixelPositions)
            {
                Points.Add(new MyVector2(pixelPosition.x * 100 - 400, pixelPosition.y * 100 - 400));
                Points.Add(new MyVector2(pixelPosition.x * 100 - 400, (pixelPosition.y + 1) * 100 - 400));
                Points.Add(new MyVector2((pixelPosition.x + 1) * 100 - 400, (pixelPosition.y + 1) * 100 - 400));
                Points.Add(new MyVector2((pixelPosition.x + 1) * 100 - 400, pixelPosition.y * 100 - 400));
            }
            
            // get the perimeter using 'quickhull' convex hull algorithm
            PolygonCollider2D pc2d = gameObject.AddComponent<PolygonCollider2D>();
            pc2d.SetPath(0, MyVector2ToVector2(QuickhullAlgorithm2D.GenerateConvexHull(Points, false)));
            pc2d.isTrigger = isTrigger;
            pixelCollider.Add(pc2d);

            return this;
        }

        List<Vector2> MyVector2ToVector2(List<MyVector2> myVector2List)
        {
            List<Vector2> vector2List = new List<Vector2>();
            foreach (MyVector2 myVector2 in myVector2List)
            {
                vector2List.Add(new Vector2(myVector2.x, myVector2.y));
            }
            return vector2List;
        }

        // Trigger and Collision
        void OnTriggerEnter2D(Collider2D other)
        {
            onTriggerEnter?.Invoke(other, this.parent);
        }
        void OnTriggerStay2D(Collider2D other)
        {
            onTriggerStay?.Invoke(other, this.parent);
        }
        void OnTriggerExit2D(Collider2D other)
        {
            onTriggerExit?.Invoke(other, this.parent);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            onCollisionEnter?.Invoke(other, this.parent);
        }
        void OnCollisionStay2D(Collision2D other)
        {
            onCollisionStay?.Invoke(other, this.parent);
        }
        void OnCollisionExit2D(Collision2D other)
        {
            onCollisionExit?.Invoke(other, this.parent);
        }
    }
}