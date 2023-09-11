using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private CameraShaker camShaker;

    [SerializeField]
    private BarrelRecoil barrelRecoil;

    [SerializeField]
    private Transform barrelHinge;

    [SerializeField]
    private Transform projectileSpawner;

    [SerializeField]
    private CannonCube projectilePrefab;

    [SerializeField]
    private GameObject flarePrefab;

    [SerializeField]
    private TrajectoryRenderer trajectoryRenderer;

    [SerializeField]
    private float baseRotationSpeed = 50f;

    public float shotForce = 450f;

    public float minShotForce = 100f;

    public float maxShotForce = 1000f;

    private Vector2 rotation = new Vector2(90f, 0f);

    private void Update()
    {
        var velocity = this.shotForce / 1f * Time.fixedDeltaTime;

        this.trajectoryRenderer.SetTrajectory(
            this.projectileSpawner.position, this.projectileSpawner.forward * velocity);
    }

    public void Rotate(Vector2 input)
    {
        this.rotation.y += input.x * Time.deltaTime * this.baseRotationSpeed;
        this.rotation.x -= input.y * Time.deltaTime * this.baseRotationSpeed;
        this.rotation.x = Mathf.Clamp(this.rotation.x, 90f, 170f);

        this.transform.rotation = Quaternion.Euler(0, this.rotation.y, 0);
        this.barrelHinge.localRotation = Quaternion.Euler(0, 0, this.rotation.x);
    }

    public void Shot()
    {
        var projectile =
            Instantiate(this.projectilePrefab, this.projectileSpawner.position, Quaternion.identity);
        projectile.Init(this.projectileSpawner.forward * this.shotForce);

        if(this.flarePrefab)
        {
            Instantiate(this.flarePrefab, this.projectileSpawner.position,
                Quaternion.FromToRotation(Vector3.up, this.projectileSpawner.forward));
        }

        if(this.camShaker)
        {
            this.camShaker.Shake();
        }

        if(this.barrelRecoil)
        {
            this.barrelRecoil.Recoil();
        }
    }

    public void SetShotForce(float val)
    {
        this.shotForce = this.minShotForce + (this.maxShotForce - this.minShotForce) * val;
    }
}
