using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTeleport : ITileEffect {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    enum TeleportType {
        Down = -1,
        Up = 1
    }

    [SerializeField]
    private TeleportType teleportType;
    [SerializeField]
    private LayerMask cubeMask;
    [SerializeField]
    private float teleportDelay;
    private Transform playerTransform;
    private Vector3 linkedTeleporterPos;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake() {
        GameObject player = GameObject.Find("PlayerVR");
        playerTransform = player.transform;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public override void PlayEffect() {
        FindLinkedTeleporter();
        StartCoroutine(TeleportAfterDelay());
    }


    private void FindLinkedTeleporter() {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, Vector3.up * (float)teleportType, cubeMask);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];;

            TileTeleport otherTeleporter = hit.transform.gameObject.GetComponent<TileTeleport>();
            if (otherTeleporter) {
                linkedTeleporterPos = hit.transform.position;
            }
        }
    }


    private IEnumerator TeleportAfterDelay() {
        yield return new WaitForSeconds(teleportDelay);
        playerTransform.position = playerTransform.position + (linkedTeleporterPos - this.transform.position);
    }
}
