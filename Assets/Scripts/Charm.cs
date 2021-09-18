using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : MonoBehaviour
{
    public Transform follow;
    public SympatheticObject body;

    private void FixedUpdate()
    {
        body.AddForce((follow.position - transform.position) / Time.fixedDeltaTime * 10);
    }
}
