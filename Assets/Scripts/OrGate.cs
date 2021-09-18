using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrGate : Activatable
{
    public List<Activatable> controlledElements = new List<Activatable>();

    [SerializeField]
    public int activations = 0;//only deactivate if activations is 0;

    public override void Activate(bool active)
    {
        //base.Activate(active);
        int old = activations;

        if (active) activations++;
        else activations--;

        if (activations == 0)
        {
            //deactivate

            foreach (Activatable a in controlledElements)
            {
                a.Activate(false);
            }
        }
        else if(activations > 0 && old == 0)
        {
            //activate

            foreach (Activatable a in controlledElements)
            {
                a.Activate(true);
            }
        }
    }
}
