using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayManager : MonoBehaviour {

    Animator anim;
    Display activeDisplay;
    public List<Display> displaySequence;
    public int loopBackTo = 2;
    public int startIndex = 0;
    int displayIndex = 0;

	// Use this for initialization
	void Start () {
        displayIndex = startIndex;
        activeDisplay = displaySequence[displayIndex];
        
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

        if (displayIndex < displaySequence.Count-1) {
            displayIndex++;
        }
        else {
            displayIndex = loopBackTo;
        }
        activeDisplay = displaySequence[displayIndex];
        activeDisplay.gameObject.SetActive(true);
    }
}
