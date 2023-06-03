using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBlock : MonoBehaviour
{
    private Vector3 nextDestination;

    public Vector3 getNextDestination() { return nextDestination; }
    public void setNextDestination(Vector3 d) { nextDestination = d; }
}