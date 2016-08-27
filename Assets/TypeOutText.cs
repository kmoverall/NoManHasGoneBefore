using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TypeOutText : MonoBehaviour {

    Text textObject;
    string stringToPrint;

    public float typingSpeed = 0.1f;
    [HideInInspector]
    public bool isFinished = false;

    // Use this for initialization
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
            yield return new WaitForSeconds(typingSpeed);
        }
        isFinished = true;
    }
}
