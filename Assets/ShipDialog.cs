using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ShipDialog : Display {

    [HideInInspector]
    public static float[] compPower = { 1.0f, 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static float[] compCond = { 1.0f, 1.0f, 1.0f, 1.0f };
    [HideInInspector]
    public static float[] compDecay = { 0.0f, 0.0f, 0.0f, 0.0f };
    [HideInInspector]
    public static int repair = -1;
    [HideInInspector]
    public static int year;
    [HideInInspector]
    public static int prevYear = 0;
    [HideInInspector]
    public static int eta = -1;
    [HideInInspector]
    public static int remainingPassengers;
    [HideInInspector]
    public static int deadPassengers = 0;
    [HideInInspector]
    public static bool spouseDiedThisTurn = false;
    [HideInInspector]
    public static bool detectedPlanet = false;
    [HideInInspector]
    public static bool gameComplete = false;

    public int startPassengers = 200;
    public float repairStrength = 0.3f;

    public Text yearText;
    public Image shipImg;
    public Text helpText;

    public DisplayManager manager;

    public ManifestDisplay manifestDisplay;

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
            yearText.text += "Approx. " + eta + " years";
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
                effectSliders[i].value = Mathf.Sqrt(compPower[i] * compCond[i]);
                effectText[i].text = (int)(Mathf.Sqrt(compCond[i] * compPower[i]) * 100) + "%";
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
        string[] firstnames = Resources.Load<TextAsset>("firstnames").text.Split("\n"[0]);
        string[] lastnames = Resources.Load<TextAsset>("lastnames").text.Split("\n"[0]);

        for (int i = 0; i < startPassengers; i++) {
            manifest.Add(new Passenger());
            int r = Random.Range(0, firstnames.Length - 1);
            manifest[i].firstName = firstnames[r];
            r = Random.Range(0, lastnames.Length - 1);
            manifest[i].lastName = lastnames[r];
            r = Random.Range(12, 60);
            manifest[i].age = r;
        }
        int player = Random.Range(0, startPassengers - 1);
        int spouse = -1;

        while (spouse < 0 || spouse == player)
            spouse = Random.Range(0, startPassengers - 1);

        manifest[player].isPlayer = true;
        manifest[player].firstName = "<ERROR>";
        manifest[player].lastName = "<ERROR>";
        manifest[player].age = -1;

        manifest[spouse].isSpouse = true;
        manifest[spouse].firstName = "<ERROR>";
        manifest[spouse].lastName = "<ERROR>";
        manifest[spouse].age = -1;

        manifestDisplay.UpdateLists();
    }

    public void NextPhase() {

        for (int i = 0; i < 4; i++) {
            compDecay[i] = compCond[i];
        }

        if (repair >= 0) {
            float oldCond = compCond[repair];
            if (repair == 0 || repair == 2) {
                compCond[repair] += repairStrength * Mathf.Sqrt(compCond[1] * compPower[1]);
            }
            else if (repair == 1) {
                compCond[repair] += repairStrength * 0.8f;
            }
            else if (repair == 3) {
                compCond[repair] += repairStrength * compCond[1];
            }
            Mathf.Clamp01(compCond[repair]);

            if (repair == 3) {
                for(int i = 0; i < 3; i++) {
                    compPower[i] *= compCond[repair] / oldCond;
                }
            }
        }

        remainingPassengers = 0;
        deadPassengers = 0;
        spouseDiedThisTurn = false;
        for (int i = 0; i < startPassengers; i++) {
            if (manifest[i].isAlive) {
                float rnd = Random.Range(0.0f, 1.0f);
                if (rnd > Mathf.Sqrt(compCond[0] * compPower[0]) && !manifest[i].isPlayer) {
                    manifest[i].isAlive = false;
                    deadPassengers++;

                    if (manifest[i].isSpouse) {
                        spouseDiedThisTurn = true;
                    }
                }
                else {
                    remainingPassengers++;
                }
            }
        }

        float totalPower = compPower[3] * compCond[3];

        float randFactor = Mathf.Lerp(0.35f, 0.07f, Mathf.Sqrt(compCond[2] * compPower[2]) * 0.7f + (1 - compPower[0]) * 0.3f);
        compCond[0] -= Random.Range(randFactor / 5f, randFactor);
        compCond[0] = Mathf.Clamp01(compCond[0]);

        randFactor = Mathf.Lerp(0.35f, 0.07f, Mathf.Sqrt(compCond[2] * compPower[2]) * 0.7f + (1 - compPower[1]) * 0.3f);
        compCond[1] -= Random.Range(randFactor / 5f, randFactor);
        compCond[1] = Mathf.Clamp01(compCond[1]);

        randFactor = Mathf.Lerp(0.35f, 0.07f, Mathf.Sqrt(compCond[2] * compPower[2]) * 0.5f + (1 - compPower[3]) * 0.5f);
        compCond[3] -= Random.Range(randFactor / 5f, randFactor);
        compCond[3] = Mathf.Clamp01(compCond[3]);

        randFactor = Mathf.Lerp(0.35f, 0.07f, Mathf.Sqrt(compCond[2] * compPower[2]) * 0.7f + (1 - compPower[2]) * 0.3f);
        compCond[2] -= Random.Range(randFactor / 5f, randFactor);
        compCond[2] = Mathf.Clamp01(compCond[2]);

        for (int i = 0; i < 3; i++) {
            if (totalPower != 0) {
                compPower[i] *= (compPower[3] * compCond[3]) / totalPower;
            }
            else {
                compPower[i] = 0.0f;
            }
        }

        for (int i = 0; i < 4; i++) {
            compDecay[i] = compDecay[i] - compCond[i];
        }

        prevYear = year;
        year += Random.Range(450, 550);

        if (year > 5000) {
            eta = (10000 - year) / 100 + 1;
            eta *= 100;
        }

        manifestDisplay.UpdateLists();

        manager.StartNextDisplay();
    }

    public override void StartDisplay() {
    }

    public void ChangePower(int index) {
        if (powerSliders[index].value > compCond[3] * 3) {
            powerSliders[index].value = compCond[3] * 3;
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

    public void SetRepair (int index) {
        if (index != repair)
            repair = index;
        else
            repair = -1;
    }

    public void SetHelpText (int index) {
        switch (index) {
            case 0:
                helpText.text = "> Cryopods\n" +
                                "> Cryopods hold the passengers of the ship in an extended stasis. If properly maintained, they can keep a human alive for thousands of years.\n" +
                                "> Thawing is a carefully controlled process. Uncontrolled thawing to due to equipment failure or loss of power has a 100% mortality rate for the subject.\n"+
                                "> The United Nations Humanity Conservation Agency reminds you that passenger survival, and thus, proper operation of the UNSS Teba's cryopods, is of the utmost importance for mission success.";
                break;
            case 1:
                helpText.text = "> Life Support\n" +
                                "> Life Support consists of all systems used to keep crew alive while they are not frozen so that they can perform their tasks.\n" +
                                "> While life support is not vital to continuing function of the UNSS Teba, damaged or low powered life support will hamper your ability to repair other systems.\n" +
                                "> TECHNICAL NOTE: Repair crew may use the attached personal systems during life support repairs, so that malfunctioning life support does not hamper your ability to repair the life support itself.";
                break;
            case 2:
                helpText.text = "> Radiation Shielding\n" +
                                "> Radiation Shielding protect ship systems from damaging cosmic radiation.\n" +
                                "> Due to the carefully controlled enivronment and sustainable design on the UNSS Teba, most environmental factors that would cause system damage over time are eliminated.\n" +
                                "> However, radiation that is normally blocked by Earth's atmosphere and magnetic field can cause significant damage to sensitive equipment over sufficiently long periods of time.\n" +
                                "> When the UNSS's Teba's radiation shielding is powered and functioning, it will block almost all of this radiation, though system decay can not be prevented entirely.";
                break;
            case 3:
                helpText.text = "> Generators\n" +
                                "> Generators provide electricity to all ship systems.\n" +
                                "> All of the systems on the UNSS Teba require electricy to function properly, generated by the on-board fusion generators.\n" +
                                "> Generators in poor condition can not provide as much power, preventing all other systems from functioning as effectively.\n" +
                                "> TECHNICAL NOTE: Generators wear down more quickly when placed under heavy load. Reducing power allotment to other systems can slow generator degradation.";
                break;
            case 4:
                helpText.text = "> The UNSS Teba is a state of the art long-distance spacecraft, designed to ferry up to 200 humans in cryostasis for millenia.\n" +
                                "> This screen is your maintenance planner. It shows the major subsystems of the ship. For all but the Generators, you can control the power allotment with the left slider, and monitor the condition and effectiveness of the system with the right two sliders.\n" +
                                "> The Generators show the current Load on the power system as well as their own condition.\n" +
                                "> Below each of these windows you can schedule repairs for that system, improving its condition.";
                break;
        }
    }
}
