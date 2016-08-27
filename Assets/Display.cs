using UnityEngine;
using System.Collections;

public abstract class Display : MonoBehaviour {

    public bool isReadyToClose = false;

    // Use this for initialization
    public abstract void StartDisplay();

}
