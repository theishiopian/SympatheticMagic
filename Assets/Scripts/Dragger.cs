using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        main = Camera.main;
    }

    bool isDragging = false;
    SympatheticObject toDrag;
    Camera main;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D target = Physics2D.Raycast(main.ScreenToWorldPoint(Input.mousePosition), Vector2.one, 0.1f);
            Component c;
            if (target.collider && target.collider.gameObject.TryGetComponent(typeof(SympatheticObject), out c))
            {
                isDragging = true;
                toDrag = c as SympatheticObject;
                toDrag.isInfluenced = true;
            }
        }
        else if((!Input.GetMouseButton(0) || (toDrag && Vector2.Distance(toDrag.transform.position, target.position) > 1.5f)) && isDragging)
        {
            isDragging = false;
            if (toDrag)
            {
                toDrag.isInfluenced = false;
            }
            toDrag = null;
        }
    }

    private void FixedUpdate()
    {
        if(isDragging)
        {
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            toDrag.AddForce((mouseDelta * toDrag.GetRigidbody().mass / Time.deltaTime) - Physics2D.gravity);
        }
    }

    private void AddForce(Vector2 force, GameObject target)
    {
        Component targetS;
        if(target.TryGetComponent(typeof(SympatheticObject), out targetS))
        {
            (targetS as SympatheticObject).AddForce(force);
        }
        else
        {
            target.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}
