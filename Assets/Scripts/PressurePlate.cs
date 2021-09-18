using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Activatable
{
    public List<Activatable> controlledElements = new List<Activatable>();

    public int count = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        if (!active)
        {
            this.Activate(true);
            
            foreach (Activatable a in controlledElements)
            {
                a.Activate(true);
            }
        }

        //change vfx
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        count--;
        if (active)
        {
            
            if (count == 0)
            {
                this.Activate(false);
                foreach (Activatable a in controlledElements)
                {
                    a.Activate(false);
                }
            }
        }

        //change vfx
    }
}
