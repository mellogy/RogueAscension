using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    private List<Steppable> objectsToStep = new List<Steppable>();


    public void Step()
    {
        objectsToStep.Clear();
        Steppable[] step = FindObjectsOfType<Steppable>();
        foreach (Steppable p in step)
        {
            objectsToStep.Add(p);
        }

        foreach (Steppable o in objectsToStep)
        {
            o.Step();
        }
    }
}
