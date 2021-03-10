using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Collectable
{
    void TakeSender(PlayerController _sender);
    void TakeTarget(PlayerController _target);

    PlayerController GetSender();
    PlayerController GetTarget();
    void Collect(PlayerController _player);
}
