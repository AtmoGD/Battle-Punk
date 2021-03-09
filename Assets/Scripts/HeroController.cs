using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeroController : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private int changeMaterialAtPosition = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject distancePrefab = null;
    [SerializeField] private Transform distanceSpawnPosition = null;
    [SerializeField] private float distanceCooldown = 0.5f;
    [SerializeField] private GameObject strongDistancePrefab = null;
    [SerializeField] private Transform strongDistanceSpawnPosition = null;
    [SerializeField] private float strongDistanceCooldown = 5f;
    [SerializeField] private GameObject nearPrefab = null;
    [SerializeField] private Transform nearSpawnPosition = null;
    [SerializeField] private float nearCooldown = 1.5f;
    [SerializeField] private GameObject shieldPrefab = null;
    [SerializeField] private Transform shieldSpawnPosition = null;
    [SerializeField] private float shieldCooldown = 1.5f;

    [Header("Behavior")]
    [SerializeField] private float rotationThreshold = 0.1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float distanceSpeed = 4f;
    [SerializeField] private float distancePower = 3f;
    [SerializeField] private float distanceAmount = 10f;
    [SerializeField] private float strongDistanceSpeed = 4f;
    [SerializeField] private float strongDistancePower = 3f;
    [SerializeField] private float strongDistanceAmount = 10f;

    public TeamColor color { get; private set; }
    public PlayerController player { get; private set; }

    public Vector2 Movement { get; set; }
    public Vector2 Rotation { get; set; }

    private Rigidbody rb = null;
    private float activeDistanceCooldown = 0f;
    private float activeStrongDistanceCooldown = 0f;
    private float activeNearCooldown = 0f;
    private float activeShieldCooldown = 0f;

    public void Init(PlayerController _player, Vector3 _pos)
    {
        rb = GetComponent<Rigidbody>();
        player = _player;

        MeshRenderer meshRen = GetComponent<MeshRenderer>();
        Material[] materials = meshRen.materials;
        materials[changeMaterialAtPosition] = player.accentMaterial;
        meshRen.materials = materials;
        SetPosition(_pos);
    }

    public void FixedUpdate()
    {
        CheckCooldowns();
        Move();
    }

    public void DistanceAttack()
    {
        
        if (activeDistanceCooldown > 0f) return;
        InstantiateAttack(distancePrefab, distanceSpawnPosition);
        activeDistanceCooldown = distanceCooldown;
    }

    private void InstantiateAttack(GameObject _prefab, Transform _position) {
        GameObject attack = Instantiate(_prefab, _position.position, _position.rotation);
        AttackController controller = attack.GetComponent<AttackController>();
        controller.Init(this);
    }

    private void Move()
    {
        if (!rb) return;

        Vector3 dir = new Vector3(Movement.x, 0, Movement.y).normalized * speed * Time.deltaTime;
        Vector3 newPos = transform.position + dir;
        rb.MovePosition(newPos);

        Vector3 targetDirection = (newPos - transform.position).normalized;

        if (Rotation.magnitude >= rotationThreshold)
            dir = new Vector3(Rotation.x, 0, Rotation.y).normalized;

        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            transform.rotation = newRotation;
        }
    }

    private void CheckCooldowns()
    {
        if (activeDistanceCooldown > 0f)
        {
            activeDistanceCooldown -= Time.deltaTime;
        }
        if (activeStrongDistanceCooldown > 0f)
        {
            activeStrongDistanceCooldown -= Time.deltaTime;
        }
        if (activeNearCooldown > 0f)
        {
            activeNearCooldown -= Time.deltaTime;
        }
        if (activeShieldCooldown > 0f)
        {
            activeShieldCooldown -= Time.deltaTime;
        }
    }

    private void SetPosition(Vector3 _pos)
    {
        float y = transform.position.y;
        transform.position = new Vector3(_pos.x, y, _pos.z);
    }

    public float GetDistanceSpeed() { return distanceSpeed; }
    public void SetDistanceSpeed(float _value) { distanceSpeed = _value; }
    public float GetDistancePower() { return distancePower; }
    public void SetDistancePower(float _value) { distancePower = _value; }
    public float GetDistanceAmount() { return distanceAmount; }
    public void SetDistanceAmount(float _value) { distanceAmount = _value; }


    public float GetStrongDistanceSpeed() { return strongDistanceSpeed; }
    public void SetStrongDistanceSpeed(float _value) { strongDistanceSpeed = _value; }
    public float GetStrongDistancePower() { return strongDistancePower; }
    public void SetStrongDistancePower(float _value) { strongDistancePower = _value; }
    public float GetStrongDistanceAmount() { return strongDistanceAmount; }
    public void SetStrongDistanceAmount(float _value) { strongDistanceAmount = _value; }
}
