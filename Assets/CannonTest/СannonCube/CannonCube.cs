using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshFilter))]
public class CannonCube : MonoBehaviour
{
    [SerializeField]
    private float mass = 1f;

    [SerializeField]
    private bool useGravity;

    [SerializeField]
    private float lifeDuration = 5f;

    [SerializeField]
    private float minRandomSize = 0.5f;

    [SerializeField]
    private float maxRandomSize = 1f;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private GameObject trailPrefab;

    private List<Force> forces;

    private Vector3 velocity;

    private float lifeTime;

    private struct Force
    {
        public Vector3 value;

        public ForceMode mode;
    }

    private class DeathData
    {
        public Vector3 position;

        public Vector3 normal;
    }

    private void Awake()
    {
        this.forces = new List<Force>();
        this.lifeTime = this.lifeDuration;
    }

    private void Start()
    {
        this.GetComponent<MeshFilter>().mesh = new CubeGenerator(this.minRandomSize, this.maxRandomSize).Generate();
    }

    public void Init(Vector3 shotForce)
    {
        this.AddForce(shotForce);
    }
    
    private void Update()
    {
        if(this.lifeTime <= 0f)
        {
            this.Death();
        }
        else
        {
            this.lifeTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(this.useGravity)
        {
            this.AddForce(Physics.gravity, ForceMode.Acceleration);
        }

        foreach(var force in this.forces)
        {
            switch(force.mode)
            {
                case ForceMode.Acceleration:
                    this.velocity += force.value * Time.fixedDeltaTime;
                    break;
                case ForceMode.VelocityChange:
                    this.velocity += force.value;
                    break;
                case ForceMode.Force:
                default:
                    this.velocity += force.value / this.mass * Time.fixedDeltaTime;
                    break;
            }
        }

        this.forces.Clear();

        var currentPosition = this.transform.position;
        var predictedPosition = currentPosition + this.velocity * Time.fixedDeltaTime;
        var distance = Vector3.Distance(currentPosition, predictedPosition);
        var ray = new Ray(currentPosition, this.velocity);
        var isHit = Physics.Raycast(ray, out var hit, distance);

        if(isHit)
        {
            this.velocity = Vector3.Reflect(this.velocity / 2, hit.normal);
            predictedPosition = currentPosition + this.velocity * Time.fixedDeltaTime;

            if(Vector3.Angle(this.velocity, hit.normal) < 45f)
            {
                this.Death(new DeathData { position = hit.point, normal = hit.normal });
            }
            else
            {
                this.transform.position = predictedPosition;
            }
        }
        else
        {
            this.transform.position = predictedPosition;
        }
    }

    private void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        this.forces.Add(new Force{ value = force, mode = mode });
    }

    private void Death(DeathData deathData = null)
    {
        var normal = Vector3.up;

        if(deathData != null)
        {
            normal = deathData.normal;

            if(this.trailPrefab)
            {
                Instantiate(this.trailPrefab, deathData.position + normal * 0.01f,
                    Quaternion.FromToRotation(Vector3.up, normal));
            }
        }

        if(this.explosionPrefab)
        {
            Instantiate(this.explosionPrefab, this.transform.position, Quaternion.FromToRotation(Vector3.up, normal));
        }

        Destroy(this.gameObject);
    }
}
