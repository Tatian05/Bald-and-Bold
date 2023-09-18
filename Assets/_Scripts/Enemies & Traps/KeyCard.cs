using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    Animator _doorAnim;
    private void Start()
    {
        _doorAnim = GameObject.Find("DoorKeyCard").GetComponent<Animator>();

        if (_doorAnim == null) Debug.LogError("NO DOOR ANIMATOR");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _doorAnim.Play("Open");
        Destroy(gameObject);
    }
}
