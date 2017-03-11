using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class VRSimpleAutowalk : MonoBehaviour {
    // How fast to move
    public float speed = 3.0F;
    // Should I move forward or not
    public bool moveForward;
    // A delay to ensure input doesn't occur too often, default 100 milliseconds
    public float stopGoDelay = 0.1f;
    // A timer to count down after input button has been pressed
    private float timer;
    // CharacterController script    
    private CharacterController controller;
    // VR Head
    private Transform vrHead;

    // Use this for initialization
    void Start() {
#if VR_GEARVR
        Debug.Log("GearVR Mode Enabled");
#elif VR_GOOGLEVR
        Debug.Log("GoogleVR Mode Enabled!");
#endif
        // Find the CharacterController
        controller = GetComponent<CharacterController>();
        // Find the VR Head
        vrHead = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        // If the timer has a value
        if (timer > 0f) {
            // Decrease by the amount of delta time
            timer -= Time.deltaTime;
        }

        // Wait for timer to expire before checking for input
        if (timer <= 0f) {
            // In the Google VR button, or the Gear VR touchpad is pressed
            if (Input.GetButtonDown("Fire1")) {
                // Change the state of moveForward
                moveForward = !moveForward;
                // Set the timer
                timer = stopGoDelay;
            }
        }

        // Check to see if I should move
        if (moveForward) {
            // Find the forward direction
            Vector3 forward = vrHead.TransformDirection(Vector3.forward);
            // Tell CharacterController to move forward
            controller.SimpleMove(forward * speed);
        }
    }
}