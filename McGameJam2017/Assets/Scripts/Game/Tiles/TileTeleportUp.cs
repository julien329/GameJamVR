using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTeleportUp : ITileEffect {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    enum TeleportType {
        Down = -1,
        Up = 1
    }

    [SerializeField]
    private TeleportType teleportType;
    private float teleportDelay;
    private Transform playerTransform;
    private Transform linkedTeleporter;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake() {
        GameObject player = GameObject.Find("PlayerVR");
        playerTransform = player.transform;
    }

    void Start() {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 100.0F);

        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            Renderer rend = hit.transform.GetComponent<Renderer>();

            if (rend) {
                // Change the material of all hit colliders
                // to use a transparent shader.
                rend.material.shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = rend.material.color;
                tempColor.a = 0.3F;
                rend.material.color = tempColor;
            }
        }
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public override void PlayEffect() {
        StartCoroutine(TeleportAfterDelay());
    }


    private void FindLinkedTeleporter() {

    }


    private IEnumerator TeleportAfterDelay() {
        yield return new WaitForSeconds(teleportDelay);
        playerTransform.position = playerTransform.position + (linkedTeleporter.position - this.transform.position);
    }
}
