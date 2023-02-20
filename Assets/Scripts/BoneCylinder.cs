using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCylinder : MonoBehaviour
{
    [Header("Bones")]
    public GameObject A = null;
    public GameObject B = null;
    public GameObject C = null;
    
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 100f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.2f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Cylinder;
    public float ColliderSize = 0.002f;
    public float RigidbodyMass = 1f; 
   
    private void Start()
    {
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.None);

        Softbody.AddCollider(ref A);
        Softbody.AddCollider(ref B);
        Softbody.AddCollider(ref C);
        
        //Add springs at joints
        Softbody.AddSpring(ref A, ref B);
        Softbody.AddSpring(ref B, ref C);
        

    }
}