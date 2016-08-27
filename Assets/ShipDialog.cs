using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipDialog : Display {

    [HideInInspector]
    public static float[] compPower = { 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static float[] compCond = { 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static int repair = -1;
    
    public List<Slider> powerSliders;
    public List<Slider> conditionSliders;
    public List<Slider> effectSliders;

	// Use this for initialization
	void Start () {
	}

    public override void StartDisplay() {
    }

    // Update is called once per frame
    void Update () {
	
	}
}
