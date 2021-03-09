using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HeroController : MonoBehaviour, Attackable
{
    [Header("Materials")]
    [SerializeField] private int changeMaterialAtPosition = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject diePrefab = null;
    [SerializeField] private GameObject distancePrefab = null;
    [SerializeField] private Transform distanceSpawnPosition = null;
    [SerializeField] public float distanceCooldown = 0.5f;
    [SerializeField] private GameObject strongDistancePrefab = null;
    [SerializeField] private Transform strongDistanceSpawnPosition = null;
    [SerializeField] public float strongDistanceCooldown = 5f;
    [SerializeField] private GameObject nearPrefab = null;
    [SerializeField] private Transform nearSpawnPosition = null;
    [SerializeField] public float nearCooldown = 1.5f;
    [SerializeField] private GameObject shieldPrefab = null;
    [SerializeField] private Transform shieldSpawnPosition = null;
    [SerializeField] public float shieldCooldown = 1.5f;

    [Header("Behavior")]
    [SerializeField] public float healthPointsMax = 200f;
    [SerializeField] public float healthPoints = 200f;
    [SerializeField] private float rotationThreshold = 0.1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float distanceSpeed = 4f;
    [SerializeField] private float distancePower = 3f;
    [SerializeField] private float distanceAmount = 10f;
    [SerializeField] private float strongDistanceSpeed = 4f;
    [SerializeField] private float strongDistancePower = 3f;
    [SerializeField] private float strongDistanceAmount = 10f;
    [SerializeField] private float shieldTime = 2f;
    [SerializeField] private float shieldRadius = 2f;
    [SerializeField] private float nearAttackTime = 1f;
    [SerializeField] private float nearAttackStartRadius = 1f;
    [SerializeField] private float nearAttackEndRadius = 1f;
    [SerializeField] private float nearAttackPower = 1f;

    public TeamColor color { get; private set; }
    public PlayerController player { get; private set; }

    public Vector2 Movement { get; set; }
    public Vector2 Rotation { get; set; }
    // public bool Attack { get; private set; }
    private Rigidbody rb = null;
    public float activeDistanceCooldown = 0f;
    public float activeStrongDistanceCooldown = 0f;
    public float activeNearCooldown = 0f;
    public float activeShieldCooldown = 0f;

    public void Init(PlayerController _player, Vector3 _pos)
    {
        rb = GetComponent<Rigidbody>();
        player = _player;
        healthPoints = healthPointsMax;
        // Attack = false;

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
        // if(Attack)
        //     DoDistanceAttack();
    }

    // public void DistanceAttack()
    // {
    //     Attack = !Attack;
    // }

    public void DistanceAttack() {
        if (activeDistanceCooldown > 0f) return;

        InstantiateAttack(distancePrefab, distanceSpawnPosition);
        activeDistanceCooldown = distanceCooldown;
    }

    public void StrongDistanceAttack()
    {
        if (activeStrongDistanceCooldown > 0f) return;

        InstantiateAttack(strongDistancePrefab, strongDistanceSpawnPosition);
        activeStrongDistanceCooldown = strongDistanceCooldown;
    }

    public void Shield()
    {
        if (activeShieldCooldown > 0f) return;

        InstantiateAttack(shieldPrefab, shieldSpawnPosition);
        activeShieldCooldown = shieldCooldown;
    }

    public void NearAttack()
    {
        if (activeNearCooldown > 0f) return;

        InstantiateAttack(nearPrefab, nearSpawnPosition);
        activeNearCooldown = nearCooldown;
    }

    public void TakeDamage(HeroController _hero, AttackType _type)
    {
        switch (_type)
        {
            case AttackType.DISTANCE:
                healthPoints -= _hero.GetDistancePower();
                break;
            case AttackType.STRONGDISTANCE:
                healthPoints -= _hero.GetStrongDistancePower();
                break;
            case AttackType.NEAR:
                healthPoints -= _hero.GetNearAttackPower();
                break;
        }

        if (healthPoints <= 0)
            Die();
    }

    public void Die() {
        GameObject heroDiedAnim = Instantiate(diePrefab, transform.position, transform.rotation);
        player.Died();
        Destroy(this.gameObject);
    }

    private void InstantiateAttack(GameObject _prefab, Transform _position)
    {
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

    public void SetPosition(Vector3 _pos)
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


    public float GetShieldTime() { return shieldTime; }
    public void SetShieldTime(float _value) { shieldTime = _value; }
    public float GetShieldRadius() { return shieldRadius; }
    public void SetShieldRadius(float _value) { shieldRadius = _value; }

    public float GetNearAttackTime() { return nearAttackTime; }
    public void SetNearAttackTime(float _value) { nearAttackTime = _value; }
    public float GetNearAttackStartRadius() { return nearAttackStartRadius; }
    public void SetNearAttackStartRadius(float _value) { nearAttackStartRadius = _value; }
    public float GetNearAttackEndRadius() { return nearAttackEndRadius; }
    public void SetNearAttackEndRadius(float _value) { nearAttackEndRadius = _value; }
    public float GetNearAttackPower() { return nearAttackPower; }
    public void SetNearAttackPower(float _value) { nearAttackPower = _value; }
}
