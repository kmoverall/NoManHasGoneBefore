using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeOutText : MonoBehaviour {

    Text textObject;
    string stringToPrint = "";

    public float typingSpeed = 0.01f;
    public float newLineDelay = 0.5f;
    [HideInInspector]
    public bool isFinished = false;
    public bool autoReset = false;

    void OnEnable() {
        if (autoReset || stringToPrint == "")
            PrepText();
    }

    public void PrepText() {
        textObject = GetComponent<Text>();
        stringToPrint = textObject.text;
        textObject.text = "";
    }

    // Update is called once per frame
    public void StartTyping() {
        StartCoroutine("TypeText");
    }

    public void Interrupt() {
        StopCoroutine("TypeText");
        textObject.text = stringToPrint;
        isFinished = true;
    }

    IEnumerator TypeText() {
        isFinished = false;
        textObject.text = "";
        for (int i = 0; i < stringToPrint.Length; i++) {
            textObject.text += stringToPrint[i];
            if (stringToPrint[i] == '\n') {
                yield return new WaitForSeconds(newLineDelay);
            }
            else {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isFinished = true;
    }
}
