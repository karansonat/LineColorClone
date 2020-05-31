using System;
using UnityEngine;

public class OnTriggerEnterHandler : MonoBehaviour
{
    public Action HitToObstacle;
    public Action FinishLinePassed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            if (HitToObstacle != null)
                HitToObstacle.Invoke();
        }

        if (other.tag == "Finish")
        {
            if (FinishLinePassed != null)
                FinishLinePassed.Invoke();
        }
    }
}
