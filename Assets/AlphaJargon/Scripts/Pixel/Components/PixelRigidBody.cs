using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp;
using SimpleMan.CoroutineExtensions;

namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelRigidBody : PixelComponent
    {
        public Rigidbody2D rb {get; protected set;}
        public override PixelGameObject parent {get;set;}
        int gravityScale = 1;
        float gravityTick = 0.1f;
        bool gravityOn = false;

        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
            // rb = gameObject.AddComponent<Rigidbody2D>();
            // rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            // rb.bodyType = RigidbodyType2D.Dynamic;
            // rb.isKinematic = true;
        }
        // FIXME:
        // cheap work around because I cant make them optional towards the this. version of itself because it must be compile time constant
        // I dont want to make a bunch of of the same named function 
        // speaking of named, named parameters are useful here
        //
        // in hindsight i could have used params
            // public void add(float? mass = null, int? gravityScale = null, bool? useGravity = null)
            // {
            //     rb.gravityScale = this.gravityScale = gravityScale ?? this.gravityScale;
            //     rb.mass = this.mass = mass ?? this.mass;
                
            //     rb.isKinematic = this.useGravity = useGravity ?? this.useGravity;
            //     this.useGravity = useGravity ?? this.useGravity;
            // }

        PixelTransform trans;
        // FIXME: this sucks.
        public void add(params object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if(list[i] is float)
                    gravityTick = (float)list[i];
                if(list[i] is int)
                    gravityScale = (int)list[i];
                if(list[i] is bool)
                    gravityOn = (bool)list[i];
            }

            // // trying to do this in Start will break it because collision tests cant be done since the parents screen layers dont exist yet.
            trans = parent[typeof(PixelTransform)];
            if(!trans)
                trans = parent.add("Transform","PixelTransform");
            this.RepeatUntil(() => gravityOn == true, () => trans.move(0, -gravityScale, false), () => print("GravityOff"), gravityTick);
        }

        public override void Remove()
        {
            Destroy(this);
        }
    }
}