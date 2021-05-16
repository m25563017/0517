using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamge;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public float maxLifttime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physic_mat;

    private void Setup()
    {
        //Create a new physic material
        physic_mat = new PhysicMaterial();

    }
}

