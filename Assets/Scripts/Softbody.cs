
using UnityEngine;

public static class Softbody 
{
    public enum ColliderShape
    {
        Cylinder
    }

    public static ColliderShape Shape;
    public static float ColliderSize;
    public static float RigidbodyMass;    
    public static float Spring;
    public static float Damper;
    public static RigidbodyConstraints Constraints;

    public static void Init(ColliderShape shape, float collidersize, float rigidbodymass, float spring, float damper, RigidbodyConstraints constraints)
    {
        Shape = shape;
        ColliderSize = collidersize;
        RigidbodyMass = rigidbodymass;
        Spring = spring;
        Damper = damper;
        Constraints = constraints;
    }
    
    //Overload for adding collider without passing all parameters
    public static Rigidbody AddCollider(ref GameObject go)
    {
        return AddCollider(ref go, Shape, ColliderSize, RigidbodyMass);        
    }
    //Overload for adding spring without passing all parameters
    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2)
    {
        SpringJoint sp = AddSpring(ref go1, ref go2, Spring, Damper);
        return sp;
    }
   
    //Real collider and spring functions
    public static Rigidbody AddCollider(ref GameObject go, ColliderShape shape, float size, float mass)
    {
        //Might need to play with the collider size
        CapsuleCollider c = go.AddComponent<CapsuleCollider>();
        c.radius = size;
        c.height = size * 2f;

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.drag = 0f;
        rb.angularDrag = 10f;
        rb.constraints = Constraints;
        return rb;
    }

    public static SpringJoint AddSpring(ref GameObject go1, ref GameObject go2, float spring, float damper)
    {
        SpringJoint sp = go1.AddComponent<SpringJoint>();
        sp.connectedBody = go2.GetComponent<Rigidbody>();
        sp.spring = spring;
        sp.damper = damper;
        return sp;
    }
    
}