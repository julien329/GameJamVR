using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// VARIABLES
    ////////////////////////////////////////////////////////////////////////////////////////////////

    [SerializeField]
    private RawImage hidePanel;
    [SerializeField]
    private RawImage banner;
    [SerializeField]
    private Text diedText;
    [SerializeField]
    private float fadeInSpeed;
    [SerializeField]
    private float delayBeforeMenu;

    private AudioSource audioSource;


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// UNITY
    ////////////////////////////////////////////////////////////////////////////////////////////////

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////
    /// METHODS
    ////////////////////////////////////////////////////////////////////////////////////////////////

    public void ShowGameOver(float delay) {
        StartCoroutine(FadeInHideScreen(delay));
    }


    IEnumerator FadeInHideScreen(float delay) {
        yield return new WaitForSeconds(delay);

        banner.enabled = true;
        diedText.enabled = true;
        hidePanel.enabled = true;
        audioSource.Play();

        Color color = hidePanel.color;
        color.a = 0.0f;
        hidePanel.color = color;

        while (color.a <= 1.0f) {
            color.a += Time.deltaTime * fadeInSpeed;
            hidePanel.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeMenu);
        SceneManager.LoadScene("MenuScene");
    }
}
