using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public override void Activate(bool active)
    {
        if (inverse) active = !active;

        base.Activate(active);
        collider.enabled = active;

        if (active)
        {
            system.Play();
        }
        else
        {
            system.Stop();
        }
    }

    public ParticleSystem system;

    private new BoxCollider2D collider;
    private bool inverse;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        collider = GetComponent<BoxCollider2D>();
        Activate(active);
        if (active) inverse = true; else inverse = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
