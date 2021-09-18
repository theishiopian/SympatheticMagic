using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Activatable : MonoBehaviour
{
    [SerializeField]
    protected bool active = false;

    public Sprite inactiveSprite;
    public Sprite activeSprite;

    protected SpriteRenderer render;

    public virtual void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public virtual void Activate(bool active)
    {
        this.active = active;
        render.sprite = active ? activeSprite : inactiveSprite;
    }
}