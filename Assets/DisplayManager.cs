using UnityEngine;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    Animator anim;
    public Display activeDisplay;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetTrigger("Open");
    }
	
    public void BeginDisplay() {
        activeDisplay.StartDisplay();
    }

	// Update is called once per frame
	void Update () {
	    if (activeDisplay.isReadyToClose && Input.anyKeyDown) {
            anim.SetTrigger("Close");
        }
	}

    void GetNextDisplay() {
        activeDisplay.gameObject.SetActive(false);
    }
}
