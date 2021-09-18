using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SympatheticObject : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SympatheticObject you;

    public bool isInfluenced = false;
    public float forceMultiplier = 1;
    public Vector2 influence = new Vector2();

    private void FixedUpdate()
    {
        influence = Vector2.zero;
    }

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
    }

    public void SetLink(SympatheticObject you)
    {
        this.you = you;
        if (you && !you.IsLinked()) you.SetLink(this);
    }

    public Rigidbody2D GetRigidbody()
    {
        return myBody;
    }

    public Rigidbody2D GetLinkedRigidbody()
    {
        return you.GetRigidbody();
    }

    public bool IsLinked()
    {
        return you != null;
    }

    public void AddForce(Vector2 force, ForceMode2D mode)
    {
        myBody.AddForce(force, mode);
        if (IsLinked())
        {
            you.GetRigidbody().AddForce(force * forceMultiplier, mode);
            influence += force;
        }
    }

    public void AddForce(Vector2 force)
    {
        AddForce(force, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision, true);
    }

    private void HandleCollision(Collision2D collision, bool addInfluence)
    {
        if (IsLinked() && !you.isInfluenced)
        {
            Vector2 impulse = ComputeTotalImpulse(collision, "CanPush", you);
            GetLinkedRigidbody().AddForce(impulse / 2 * GetLinkedRigidbody().mass / Time.fixedDeltaTime);
            if (addInfluence)influence += impulse / 2 * GetLinkedRigidbody().mass / Time.fixedDeltaTime;
        }
    }
    
    //TODO: cache vars for better performance
    private static Vector2 ComputeTotalImpulse(Collision2D collision, string tag, SympatheticObject toIgnore)
    {
        Vector2 impulse = Vector2.zero;

        int contactCount = collision.contactCount;

        for (int i = 0; i < contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(0);

            bool matchTag = contact.collider.CompareTag(tag);
            bool notOther = !contact.collider.gameObject.Equals(toIgnore.gameObject);

            if (matchTag && notOther)
            {
                Vector2 potentialImpulse = Vector2.zero;
                potentialImpulse += contact.normal * contact.normalImpulse;
                potentialImpulse.x += contact.tangentImpulse * contact.normal.y;
                potentialImpulse.y -= contact.tangentImpulse * contact.normal.x;

                //if you start seeing wierd impulses applied to linked rigidbodies, check here
                Rigidbody2D collided = contact.collider.attachedRigidbody;

                if(collided && collided.velocity.magnitude > 0.25f)impulse += potentialImpulse;
            }
        }
        

        return impulse;
    }
}
