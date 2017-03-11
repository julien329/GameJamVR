using UnityEngine;

public class CanvasSelector : MonoBehaviour {


    public GameObject VRCanvas;
    public GameObject ComputerCanvas;
    
    void Start () {
        #if UNITY_STANDALONE
            ComputerCanvas.SetActive(true);
            VRCanvas.SetActive(false);
        #endif
        #if UNITY_ANDROID
            VRCanvas.SetActive(true);
            ComputerCanvas.SetActive(false);
        #endif
    }
}
