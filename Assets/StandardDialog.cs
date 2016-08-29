using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StandardDialog : Display {

    public List<TypeOutText> textBlocks;
    protected DisplayManager manager;
    public float screenShowWait = 0.0f;
    public float initialWait = 0.5f;
    public float inbetweenWait = 0.5f;
    public float endWait = 0f;
    bool isReadyToClose = false;
    public bool autoStart = true;
    public bool playOutOnce = false;
    bool played = false;

    void OnEnable() {
        manager = FindObjectOfType<DisplayManager>();
        if (screenShowWait > 0.0f) {
            GetComponent<Image>().enabled = false;
        }
        if (autoStart) {
            StartDisplay();
        }
    }

    public override void StartDisplay() {
        if (!(playOutOnce && played)) {
            StartCoroutine("BeginScene");
        }
    }

    void Update() {
        if (isReadyToClose == true && Input.anyKeyDown) {
            manager.StartNextDisplay();
        }
        else if (Input.anyKeyDown && GetComponent<Image>().enabled) {
            StopCoroutine("BeginScene");
            for (int i = 0; i < textBlocks.Count; i++) {
                textBlocks[i].Interrupt();
            }
            isReadyToClose = true;
            played = true;
            StartCoroutine("TimedClose");
        }
    }

    IEnumerator BeginScene() {
        yield return new WaitForSeconds(screenShowWait);
        GetComponent<Image>().enabled = true;
        isReadyToClose = false;

        yield return new WaitForSeconds(initialWait);
        for (int i = 0; i < textBlocks.Count; i++) {
            textBlocks[i].StartTyping();
            while (!textBlocks[i].isFinished) {
                yield return null;
            }

            if (i != textBlocks.Count - 1)
                yield return new WaitForSeconds(inbetweenWait);
        }

        isReadyToClose = true;
        played = true;

        if (endWait > 0) {
            yield return new WaitForSeconds(endWait);

            if (gameObject.activeInHierarchy)
                manager.StartNextDisplay();
        }
    }

    IEnumerator TimedClose() {
        if (endWait > 0) {
            yield return new WaitForSeconds(endWait);

            if (gameObject.activeInHierarchy)
                manager.StartNextDisplay();
        }
    }
}
