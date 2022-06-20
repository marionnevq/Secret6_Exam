using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Peg currentPeg;
    public Peg lastPeg;
    public Peg closestPeg;
    public int size;
    public bool isTop;
    public bool isGrabbed = false;
    
}
