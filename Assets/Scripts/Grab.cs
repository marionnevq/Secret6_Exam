using UnityEngine;

public class Grab : MonoBehaviour
{
    private GameObject selected;

    [SerializeField] private float floorBounds;
    [SerializeField] private LayerMask rayCastMask;
    [SerializeField] private AudioSource pickup;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (selected == null)
            {
                //picking up piece
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }
                    else
                    {
                        if (hit.collider.gameObject.GetComponent<Piece>().isTop)
                        {
                            pickup.Stop();
                            pickup.Play();
                            selected = hit.collider.gameObject;
                            var piece = selected.GetComponent<Piece>();
                            piece.currentPeg.PopPiece();
                            piece.isGrabbed = true;
                            Cursor.visible = false;
                        }
                    }

                }

            }
            else
            {
                var piece = selected.GetComponent<Piece>();
                var peg = piece.closestPeg;
                if (peg != null)
                {
                    if (peg.CheckPushPiece(piece))
                    {
                        pickup.Stop();
                        pickup.Play();
                        selected.transform.position = peg.GetPiecePosition();
                        piece.isGrabbed = false;
                        selected = null;
                        Cursor.visible = true;
                    }

                }

            }
        }

        if (selected != null)
        {
            MoveObjectToMousePos();
        }
    }

    private void MoveObjectToMousePos()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selected.transform.position).z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        selected.transform.position = new Vector3(worldPos.x, Mathf.Clamp(worldPos.y, floorBounds, 100f), 0f);
    }
    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;

        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit, Mathf.Infinity, -rayCastMask);

        return hit;
    }
}
