using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShipDialog : Display {

    [HideInInspector]
    public static float[] compPower = { 1.0f, 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static float[] compCond = { 1.0f, 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static int repair = -1;
    [HideInInspector]
    public static int year;
    [HideInInspector]
    public static int eta = -1;
    [HideInInspector]
    public static int remainingPassengers;
    public static int startPassengers = 200;

    public Text yearText;
    public Image shipImg;

    public DisplayManager manager;

    public float repairStrength = 0.5f;

    public List<Slider> powerSliders;
    public List<Text> powerText;
    public List<Slider> conditionSliders;
    public List<Text> conditionText;
    public List<Text> conditionLabels;
    public List<Slider> effectSliders;
    public List<Text> effectText;
    public List<Text> effectLabels;

    [HideInInspector]
    public static List<Passenger> manifest = new List<Passenger>();

	// Use this for initialization
	void Start () {
        year = Random.Range(450, 550);
        remainingPassengers = startPassengers;
        GenerateManifest();
    }

    void Update() {
        yearText.text = "Time Since Departure: "+year+" years. ETA: ";
        if (eta < 0) {
            yearText.text += "Unkown";
        }
        else {
            yearText.text += eta + " years";
        }
        yearText.text += "\n" + remainingPassengers + " passengers still alive";

        Color effectColor = Color.black;
        Color conditionColor = Color.black;
        float shipCond = 0.0f;
        Color shipColor = Color.black;
        Image[] sliderImages;

        for (int i = 0; i < compCond.Length; i++) {
            powerSliders[i].value = compPower[i];
            powerText[i].text = (int)(compPower[i] * 100) + "%";

            if (i < 3) {
                effectSliders[i].value = compPower[i] * compCond[i];
                effectText[i].text = (int)(compCond[i] * compPower[i] * 100) + "%";
                effectColor.r = Mathf.Clamp01(-2 * compPower[i] * compCond[i] + 2);
                effectColor.g = Mathf.Clamp01(2 * compPower[i] * compCond[i]);
                effectText[i].color = effectColor;
                effectLabels[i].color = effectColor;
                sliderImages = effectSliders[i].gameObject.GetComponentsInChildren<Image>();
                foreach (Image im in sliderImages) {
                    im.color = effectColor;
                }
            }
            
            conditionSliders[i].value = compCond[i];
            conditionText[i].text = (int)(compCond[i] * 100) + "%";
            conditionColor.r = Mathf.Clamp01(-2 * compCond[i] + 2);
            conditionColor.g = Mathf.Clamp01(2 * compCond[i]);
            conditionText[i].color = conditionColor;
            conditionLabels[i].color = conditionColor;
            sliderImages = conditionSliders[i].gameObject.GetComponentsInChildren<Image>();
            foreach (Image im in sliderImages) {
                im.color = conditionColor;
            }

            shipCond += compCond[i] / 4;
        }

        shipColor.r = Mathf.Clamp01(-2 * shipCond + 2);
        shipColor.g = Mathf.Clamp01(2 * shipCond);
        shipImg.color = shipColor;
    }

    public void GenerateManifest() {
        for (int i = 0; i < startPassengers; i++) {
            manifest.Add(new Passenger());
        }
    }

    public void NextPhase() {
        if (repair >= 0) {
            float oldCond = compCond[repair];
            compCond[repair] += 0.25f * compCond[1] * compPower[1];
            Mathf.Clamp01(compCond[repair]);

            if (repair == 3) {
                for(int i = 0; i < 3; i++) {
                    compPower[i] *= compCond[repair] / oldCond;
                }
            }
        }

        float totalPower = compPower[3] * compCond[3];

        float randFactor = Mathf.Lerp(0.25f, 0.05f, compCond[2] * compPower[2] * 0.8f + (1 - compPower[0]) * 0.2f);
        compCond[0] -= Random.Range(0.0f, randFactor);
        compCond[0] = Mathf.Clamp01(compCond[0]);

        randFactor = Mathf.Lerp(0.25f, 0.05f, compCond[2] * compPower[2] * 0.8f + (1 - compPower[1]) * 0.2f);
        compCond[1] -= Random.Range(0.0f, randFactor);
        compCond[1] = Mathf.Clamp01(compCond[1]);

        randFactor = Mathf.Lerp(0.25f, 0.05f, compCond[2] * compPower[2] * 0.8f + (1 - compPower[3]) * 0.2f);
        compCond[3] -= Random.Range(0.0f, randFactor);
        compCond[3] = Mathf.Clamp01(compCond[3]);

        randFactor = Mathf.Lerp(0.25f, 0.05f, compCond[2] * compPower[2] * 0.8f + (1 - compPower[2]) * 0.2f);
        compCond[2] -= Random.Range(0.0f, randFactor);
        compCond[2] = Mathf.Clamp01(compCond[2]);

        for (int i = 0; i < 3; i++) {
            if (totalPower != 0) {
                compPower[i] *= (compPower[3] * compCond[3]) / totalPower;
            }
            else {
                compPower[i] = 0.0f;
            }
        }

        year += Random.Range(450, 550);

        remainingPassengers = 0;
        for (int i = 0; i < startPassengers; i++) {
            if (manifest[i].isAlive) {
                float rnd = Random.Range(0.0f, 1.0f);
                if (rnd > compCond[0] * compPower[0]) {
                    manifest[i].isAlive = false;
                }
                else {
                    remainingPassengers++;
                }
            }
        }

        manager.StartNextDisplay();
    }

    public override void StartDisplay() {
    }

    public void ChangePower(int index) {
        if (powerSliders[index].value > compCond[3]) {
            powerSliders[index].value = compCond[3];
        }

        compPower[index] = powerSliders[index].value;

        compPower[3] = (compPower[0] + compPower[1] + compPower[2]) / 3;

        if (compPower[3] > compCond[3]) {
            for (int i = 0; i < 3; i++) {
                if (i != index) {
                    compPower[i] -= (compPower[3] - compCond[3]) / 2;
                    Mathf.Clamp01(compPower[i]);
                }
            }
        }

        if (compCond[3] > 0) {
            compPower[3] = (compPower[0] + compPower[1] + compPower[2]) / 3;
            compPower[3] /= compCond[3];
        }
        else {
            compPower[3] = 0;
        }
    }

    public void SetRepair(int index) {
        if (index != repair)
            repair = index;
        else
            repair = -1;
    }
}
