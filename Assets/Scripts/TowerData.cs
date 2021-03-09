using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Tower")]
public class TowerData : ScriptableObject {
    public new string name;
    public GameObject prefab;
    public int costs;
}
