using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FabricObject : MonoBehaviour, IInteractable
{

    public FabricData fabricData;

    public string GetInteractPrompt()
    {
        string str = $"{fabricData.displayName}\n{fabricData.description}";
        return str;
    }

    public void OnInteract()
    {

    }
}
