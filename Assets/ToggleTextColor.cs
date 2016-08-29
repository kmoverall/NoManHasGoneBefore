using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToggleTextColor : MonoBehaviour {

    public Toggle toggle;

    public Text textObj;
    public Color onColor;
    public Material onMaterial;
    public Color offColor;
    public Material offMaterial;
	
	// Update is called once per frame
	void Update () {
	    if (toggle.isOn) {
            textObj.color = onColor;
            textObj.material = onMaterial;
        }
        else {
            textObj.color = offColor;
            textObj.material = offMaterial;
        }
	}
}
