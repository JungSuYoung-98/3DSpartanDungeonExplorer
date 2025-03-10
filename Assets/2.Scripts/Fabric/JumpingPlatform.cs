using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class JumpingPlatform : MonoBehaviour
{

    [Header("JumpingPlatform")]
    public int jumpPower;

    private void OnCollisionEnter(Collision collision)
    {
        CharacterManager.Instance.Player.controller.JumpingPlayform(jumpPower);
    }

}
