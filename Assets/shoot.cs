using UnityEngine;
using TMPro;

public class ProjectileGunTutorial : MonoBehaviour
{

    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadtime, timeBetweenShots;
    public int magazinesize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //Bug fixing:
    public bool allowinvoke = true;

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazinesize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display ,if it exists :D
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazinesize / bulletsPerTap);
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazinesize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && !reloading && bulletsLeft <= 0) Reload();

        //shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //set bullets shot to 0
            bulletShot = 0;

            Shoot();

        }

    }
    private void Shoot()
    {
        readyToShoot = false;

        //Find the hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));//Just a ray through the middle of your
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
            targetpoint = hit.point;
        else
            targetpoint = ray.GetPoint(75);//Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetpoint - attackPoint.position;

        //Calculate spread
        float X = Random.Range(-spread, spread);
        float Y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread = new Vector3(X, Y, 0);//Just add spread to last direction

        //Instantiate bullet/projectile
        GameObject currenBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);//store instantiated bullet
        //Rotate bullet to shoot direction
        currenBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullect
        currenBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currenBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * shootForce, ForceMode.Impulse);

        //Instantiate muzzle flash,if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletShot++;

        //Invoke resetShot function (if not aleady invoked)

        if (allowinvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowinvoke = false;

            //Add recoil to player
            playerRb.AddForce(-directionWithoutSpread.normalized * recoilForce, ForceMode.Impulse);

        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShooting);
    }
    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowinvoke = true;

    }
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadtime);
    }
}