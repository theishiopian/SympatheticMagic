using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : Activatable
{
    public float pushForce = 1;

    private ParticleSystem system;

    private List<SympatheticObject> toBlow = new List<SympatheticObject>();

    private void Start()
    {
        base.Start();
        system = GetComponent<ParticleSystem>();
        if (active) system.Play(); else system.Stop();
    }

    public override void Activate(bool active)
    {
        base.Activate(active);
        if (active) system.Play(); else system.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SympatheticObject toAdd;

        if(collision.CompareTag("CanPush") && collision.gameObject.TryGetComponent(out toAdd))
        {
            toBlow.Add(toAdd);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SympatheticObject toRemove;

        if(collision.gameObject.TryGetComponent(out toRemove))
        {
            toBlow.Remove(toRemove);
        }
    }

    private void FixedUpdate()
    {
        if(active)
        {
            foreach (SympatheticObject toPush in toBlow)
            {
                toPush.AddForce(transform.up * pushForce / Time.fixedDeltaTime);
            }
        }
    }
}
