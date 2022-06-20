using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peg : MonoBehaviour
{
    [SerializeField] private Transform piecePosition;
    [SerializeField] private Vector3 ogPosition;
    public Stack<Piece> pieces;
    [SerializeField] private GameObject highlight;
    
    public Vector3 GetPiecePosition()
    {
        return piecePosition.position;
    }

    public void ResetPeg()
    {
        pieces.Clear();
        piecePosition.localPosition = ogPosition;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "drag")
        {
            Piece p = other.gameObject.GetComponent<Piece>();
            if (p.isGrabbed)
            {
                highlight.SetActive(true);
                p.closestPeg = this;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "drag")
        {
            Piece p = other.gameObject.GetComponent<Piece>();
            if (p.isGrabbed)
            {
                highlight.SetActive(false);
                p.closestPeg = null;

            }
        }
    }

    public bool CheckPushPiece(Piece p)
    {
        if (pieces.Count == 0)
        {
            PushPiece(p);
            p.currentPeg = this;
            if (p.currentPeg != p.lastPeg)
            {
                GameManager.instance.score++;
            }
            return true;
        }
        else
        {
            //Check if piece is eligible
            if (p.size < pieces.Peek().size)
            {
                p.currentPeg = this;

                PushPiece(p);
                if (p.currentPeg != p.lastPeg)
                {
                    GameManager.instance.score++;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    private void PushPiece(Piece p)
    {
        if (highlight.activeInHierarchy)
        {
            highlight.SetActive(false);
        }
        Piece x;
        if (pieces.TryPeek(out x))
        {
            pieces.Peek().isTop = false;
            pieces.Push(p);
            p.isTop = true;
        }
        else
        {
            pieces.Push(p);
            p.isTop = true;
        }
        piecePosition.position += new Vector3(0f, 5f, 0f);
    }

    public void PopPiece()
    {
        Piece p = pieces.Pop();
        p.lastPeg = p.currentPeg;
        p.currentPeg = null;

        Piece x;
        if (pieces.TryPeek(out x))
        {
            x.isTop = true;
        }
        piecePosition.position -= new Vector3(0f, 5f, 0f);




    }
}
