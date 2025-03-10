using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fabric", menuName = "New Fabric")]

public class FabricData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
}
