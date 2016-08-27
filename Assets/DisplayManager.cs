using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayManager : MonoBehaviour {

    Animator anim;
    Display activeDisplay;
    [HideInInspector]
    public Display nextDisplay;
    public List<Display> displaySequence;
    int displayIndex = 0;

	// Use this for initialization
	void Start () {
        activeDisplay = displaySequence[0];
        foreach (Display dis in displaySequence) {
            dis.gameObject.SetActive(false);
        }
        activeDisplay.gameObject.SetActive(true);
        anim = GetComponent<Animator>();
        anim.SetTrigger("Open");
    }
	
    public void BeginDisplay() {
        activeDisplay.StartDisplay();
    }

	// Update is called once per frame
	void Update () {
	}

    public void StartNextDisplay() {
        activeDisplay.gameObject.SetActive(false);
        displayIndex++;
        if (displayIndex < displaySequence.Count) {
            activeDisplay = displaySequence[displayIndex];
            activeDisplay.gameObject.SetActive(true);
        }
    }
}
