using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour, Collectable
{
    [SerializeField] private float collectSpeed = 3f;
    [SerializeField] private int goldAmount = 50;
    [SerializeField] private GameObject dieObject = null;
    PlayerController sender = null;
    PlayerController target = null;
    public void TakeSender(PlayerController _sender)
    {
        sender = _sender;
    }
    public void TakeTarget(PlayerController _target)
    {
        target = _target;
    }
    public PlayerController GetSender()
    {
        return sender;
    }
    public PlayerController GetTarget()
    {
        return target;
    }
    public void Collect(PlayerController _player)
    {
        if (target)
        {
            if (target != _player)
                return;
        }

        _player.gold += goldAmount;

        if (dieObject)
            Instantiate(dieObject, target.transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
    private void Update()
    {
        if (!target) return;
        transform.position = Vector3.Lerp(transform.position, target.hero.transform.position, Time.deltaTime * collectSpeed);
        if((target.hero.transform.position - transform.position).magnitude < 1f) {
            Collect(target);
        }
    }
}
