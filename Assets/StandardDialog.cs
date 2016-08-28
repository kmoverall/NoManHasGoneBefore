using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StandardDialog : Display {

    public List<TypeOutText> textBlocks;
    DisplayManager manager;
    public float initialWait = 0.5f;
    public float inbetweenWait = 0.5f;
    public float endWait = 0f;
    bool isReadyToClose = false;
    public bool autoStart = true;

    void OnEnable() {
        manager = FindObjectOfType<DisplayManager>();
        if (autoStart) {
            StartDisplay();
        }
    }

    public override void StartDisplay() {
        StartCoroutine("BeginScene");
    }

    void Update() {
        if (isReadyToClose == true && Input.anyKeyDown) {
            manager.StartNextDisplay();
        }
    }

    IEnumerator BeginScene() {
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

        if (endWait > 0) {
            yield return new WaitForSeconds(endWait);

            manager.StartNextDisplay();
        }
    }
}
