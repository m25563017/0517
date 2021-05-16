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

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions) Explode();

        //Count down lifttime
        maxLifttime -= Time.deltaTime;
        if (maxLifttime <= 0) Explode();
    }
    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }
    private void Setup()
    {
        //Create a new physic material
        physic_mat = new PhysicMaterial();
        physic_mat.bounciness = bounciness;
        physic_mat.frictionCombine = PhysicMaterialCombine.Maximum;
        physic_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material collider
        GetComponent<SphereCollider>().material = physic_mat;

        //Sat gravity
        rb.useGravity = useGravity;

    }
}

