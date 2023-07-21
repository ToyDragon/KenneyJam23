using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMaker : MonoBehaviour
{
    public List<GameObject> trailPieces = new List<GameObject>();
    public int maxTrailPieces = 30;
    public static List<GameObject> TRAIL_PIECE_POOL = new List<GameObject>();
    public GameObject trailPiecePrefab;
    private Vector3 lastTrailPosition;
    public float trailPieceLength = .25f;
    public static GameObject ROOT_TRAIL_PIECE;
    private GameObject ReserveTrailPiece() {
        if (ROOT_TRAIL_PIECE == null) {
            ROOT_TRAIL_PIECE = new GameObject("Trail Pieces");
        }
        GameObject piece = null;
        if (TRAIL_PIECE_POOL.Count > 0) {
            piece = TRAIL_PIECE_POOL[TRAIL_PIECE_POOL.Count - 1];
            TRAIL_PIECE_POOL.RemoveAt(TRAIL_PIECE_POOL.Count - 1);
            piece.SetActive(true);
            trailPieces.Add(piece);
            return piece;
        }
        piece = GameObject.Instantiate(trailPiecePrefab);
        trailPieces.Add(piece);
        piece.transform.SetParent(ROOT_TRAIL_PIECE.transform);
        return piece;
    }
    private void ReturnTrailPiece(GameObject piece) {
        piece.SetActive(false);
        TRAIL_PIECE_POOL.Add(piece);
    }
    void OnDisable() {
        foreach (var piece in trailPieces) {
            Destroy(piece);
        }
        trailPieces.Clear();
    }
    void Update()
    {
        var groundedPos = transform.position - Vector3.up * .1f;
        var posDelta = (groundedPos - lastTrailPosition);
        if (posDelta.magnitude > trailPieceLength) {
            var newPiece = ReserveTrailPiece();
            newPiece.transform.position = groundedPos - posDelta*.5f + Vector3.up * 0.06f;
            newPiece.transform.localScale = new Vector3(
                newPiece.transform.localScale.x,
                newPiece.transform.localScale.y,
                trailPieceLength * .75f
            );
            newPiece.transform.LookAt(groundedPos + Vector3.up * 0.06f);
            
            lastTrailPosition = groundedPos;

            if (trailPieces.Count > maxTrailPieces) {
                ReturnTrailPiece(trailPieces[0]);
                trailPieces.RemoveAt(0);
            }
        }
    }
}
