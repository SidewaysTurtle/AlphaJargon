using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelGame
{
    public class PixelRigidBody : PixelComponent
    {
        public Rigidbody2D rb {get; protected set;}
        int gravityScale = 1;
        float mass = 0.001f;
        bool useAutoMass = false;
        bool useGravity = false;
        public override void Create(PixelGameObject parent)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.isKinematic = false;
            
            rb.gravityScale = gravityScale;
            rb.mass = mass;
            rb.useAutoMass = useAutoMass;

            this.useGravity = true; 
        }

        // FIXME:
        // cheap work around because I cant make them optional towards the this. version of itself because it must be compile time constant
        // I dont want to make a bunch of of the same named function 
        // speaking of named, named parameters are useful here
        //
        // in hindsight i could have used params
        public void add(float? mass = null, int? gravityScale = null, bool? useAutoMass = null, bool? useGravity = null)
        {
            rb.gravityScale = this.gravityScale = gravityScale ?? this.gravityScale;
            rb.mass = this.mass = mass ?? this.mass;
            rb.useAutoMass = this.useAutoMass = useAutoMass ?? this.useAutoMass;
            
            this.useGravity = useGravity ?? this.useGravity;
            rb.isKinematic = !this.useGravity;
        }
    }
}