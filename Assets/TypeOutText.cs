using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeOutText : MonoBehaviour {

    Text textObject;
    string stringToPrint;

    public float typingSpeed = 0.01f;
    public float newLineDelay = 0.5f;
    [HideInInspector]
    public bool isFinished = false;
    
    void Start() {
        textObject = GetComponent<Text>();
        stringToPrint = textObject.text;
        textObject.text = "";
    }

    // Update is called once per frame
    public void StartTyping() {
        StartCoroutine("TypeText");
    }

    IEnumerator TypeText() {
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
