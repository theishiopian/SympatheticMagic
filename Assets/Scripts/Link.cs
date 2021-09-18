using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LinkState
{
    UNLINKED, LINKING, LINKED
}

public class Link : MonoBehaviour
{
    public LineRenderer render;
    public SympatheticObject a, b;
    public ParticleSystem particles;
    public Gradient gradient;
    public LayerMask scanMask;

    [Header("Wave properties")]
    public float speed = 1;
    public float amplitude = 0.25f;
    public float frequency = 10;
    public float phase = 0;

    private new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        render.positionCount = 15;
        camera = Camera.main;
    }

    private LinkState state = LinkState.UNLINKED;

    private bool GetLinkable(Vector2 position, out SympatheticObject toLink)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.2f, scanMask);

        if(hit && hit.gameObject.TryGetComponent<SympatheticObject>(out toLink))
        {
            return true;
        }

        toLink = null;
        return false;
    }

    Vector2 mousePos;

    // Update is called once per frame
    void Update()
    {
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        //state
        if (Input.GetMouseButtonDown(1))
        {
            if(state == LinkState.UNLINKED)
            {
                //start linking
                SympatheticObject toLink;

                if(GetLinkable(mousePos, out toLink))
                {
                    a = toLink;
                    state = LinkState.LINKING;
                    render.enabled = true;
                    render.colorGradient = new Gradient();
                }
            }
            else if(state == LinkState.LINKED)
            {
                //unlink
                a.SetLink(null);
                b.SetLink(null);
                a = null;
                b = null;
                state = LinkState.UNLINKED;
                render.enabled = false;
            }
        }
        else if(Input.GetMouseButtonUp(1))
        {
            if (state == LinkState.LINKING)
            {
                //finish link or break link
                SympatheticObject toLink;

                if (GetLinkable(mousePos, out toLink))
                {
                    b = toLink;
                    state = LinkState.LINKED;
                    render.enabled = true;
                    render.colorGradient = gradient;
                    a.SetLink(b);
                }
                else
                {
                    if(a)a.SetLink(null);
                    if(b)b.SetLink(null);
                    a = null;
                    b = null;
                    state = LinkState.UNLINKED;
                    render.enabled = false;
                }
            }
        }

        //vfx

        Vector2 secondPos = (state == LinkState.LINKED ? ((Vector2)b.transform.position) : mousePos);
        float dist = 0;
        
        if(state != LinkState.UNLINKED)
        {
            dist = Vector2.Distance(a.transform.position, secondPos);

            render.SetPosition(0, Vector2.zero);

            render.SetPosition(14, Vector2.zero + (Vector2.up * dist));

            for (int i = 1; i < render.positionCount - 1; i++)
            {
                float t = ((float)i).Remap(0, 14, 0, 1);
                Vector2 pos = Vector2.Lerp(render.GetPosition(0), render.GetPosition(14), t);
                render.SetPosition(i, pos + Vector2.right * Mathf.Sin((t + phase) * frequency) * amplitude * (Mathf.PingPong(Time.time * speed, 2) - 1));
            }

            transform.position = a.transform.position;
            Vector2 dir = (secondPos - (Vector2)a.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        if(state == LinkState.LINKING)
        {
            Color c = gradient.Evaluate(Mathf.PingPong(Time.time, 1));
            render.SetColors(c,c);
        }

        if (state == LinkState.LINKED)
        {
            SympatheticObject max = Max(a, b);
            phase = Mathf.Repeat(Time.time, Time.deltaTime * 0.1f + max.GetRigidbody().velocity.sqrMagnitude) * (max.Equals(a) ? -1 : 1);
            //particles.transform.position = max.transform.position;

            //particles.startLifetime = dist * 0.3f;
            //particles.startSpeed = (max.Equals(a) ? 1 : -1) * 3;
            //particles.emissionRate = max.influence.sqrMagnitude * 0.25f;
        }
        else
        {
            phase = 0;

            //particles.emissionRate = 0;
        }
    }

    SympatheticObject Max(SympatheticObject a, SympatheticObject b)
    {
        if (a.GetRigidbody().velocity.sqrMagnitude > b.GetRigidbody().velocity.sqrMagnitude) return a;

        return b;
    }
}
