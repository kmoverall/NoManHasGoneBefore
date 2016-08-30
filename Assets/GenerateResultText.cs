using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenerateResultText : MonoBehaviour {

    public Text titleText;
    public Text bodyText;

    void OnEnable() {
        Generate();
    }

    void Generate() {
        if (ShipDialog.year < 10000 && ShipDialog.remainingPassengers > 1) {
            titleText.text = "UNSS TEBA AUTOMATED REPORT: MISSION YEARS " + ShipDialog.prevYear + " - " + ShipDialog.year;
            bodyText.text = "";
            if (ShipDialog.compDecay[0] > 0)
                bodyText.text += "> Cryopod condition deteriorated by " + (int)(ShipDialog.compDecay[0] * 100) + "%\n";
            if (ShipDialog.compDecay[1] > 0)
                bodyText.text += "> Life Support condition deteriorated by " + (int)(ShipDialog.compDecay[1] * 100) + "%\n";
            if (ShipDialog.compDecay[2] > 0)
                bodyText.text += "> Radiation Shielding condition deteriorated by " + (int)(ShipDialog.compDecay[2] * 100) + "%\n";
            if (ShipDialog.compDecay[3] > 0)
                bodyText.text += "> Generator condition deteriorated by " + (int)(ShipDialog.compDecay[3] * 100) + "%\n";

            bodyText.text += "> " + ShipDialog.deadPassengers + " passengers deceased.\n";
            if (ShipDialog.deadPassengers == 0) {
                bodyText.text += "> The United Nations commends your efforts towards the preservation of humanity.\n";
            }
            else if (ShipDialog.spouseDiedThisTurn) {
                bodyText.text += "> The United Nations Human Conservation Agency regrets to inform you that your spouse was among the deceased. We offer our condolences.\n";
            }
            else {
                bodyText.text += "> Consult the ship manifest for further information.\n";
            }

            if (ShipDialog.year > 5000 && !ShipDialog.detectedPlanet) {
                bodyText.text += "> The UNSS Teba's sensors have detected a potentially habitable planet. As celebration, you are allowed one extra nutrient ration for this shift only.\n";
                ShipDialog.detectedPlanet = true;
            }

            bodyText.text += "\n> Press any key to continue.";

            titleText.GetComponent<TypeOutText>().PrepText();
            bodyText.GetComponent<TypeOutText>().PrepText();

            GetComponent<StandardDialog>().StartDisplay();
        }
        else if (ShipDialog.year >= 10000 && ShipDialog.remainingPassengers > 1) {
            ShipDialog.gameComplete = true;
            titleText.text = "UNSS TEBA AUTOMATED REPORT: MISSION YEARS " + ShipDialog.prevYear + " - " + ShipDialog.year;
            bodyText.text = "> MISSION SUCCESS";
            bodyText.text += "> The UNSS Teba has landed upon a habitable planet.\n";
            bodyText.text += "> The United Nations Humanity Conservation Agency commends you for safely delivering " + ShipDialog.remainingPassengers + " people to a new planet\n";
            bodyText.text += "> Due to your determination and ingenuity, you have given humanity a second chance at life as a species. Words can not capture your heroism.\n";
            bodyText.text += "> We wish you luck in your colonization efforts. We have faith in your success.\n";
            bodyText.text += "\n> Press any key to leave.";

            titleText.GetComponent<TypeOutText>().PrepText();
            bodyText.GetComponent<TypeOutText>().PrepText();

            GetComponent<StandardDialog>().StartDisplay();
        }
        else if (ShipDialog.remainingPassengers == 1 && ShipDialog.year < 10000) {
            ShipDialog.gameComplete = true;
            titleText.text = "UNSS TEBA AUTOMATED REPORT: MISSION YEARS " + ShipDialog.prevYear + " - " + ShipDialog.year;

            bodyText.text = "> MISSION FAILURE";
            bodyText.text += "> " + ShipDialog.deadPassengers + 1 + " passengers deceased.\n";
            bodyText.text += "> After 250000 years, the last human has breathed his last. With this ship, humanity's final resting place, let it be known that we met the end not with acceptance, but with defiance.\n";
            bodyText.text += "> Earth may have been our home, but the stars are our grave.\n";
            bodyText.text += "\n> BEGINNING MEMORIALIZATION PROTOCOLS...";

            GetComponent<StandardDialog>().endWait = 10.0f;

            titleText.GetComponent<TypeOutText>().PrepText();
            bodyText.GetComponent<TypeOutText>().PrepText();

            GetComponent<StandardDialog>().StartDisplay();
        }
        else if (ShipDialog.remainingPassengers == 1 && ShipDialog.year >= 10000) {
            ShipDialog.gameComplete = true;
            titleText.text = "UNSS TEBA AUTOMATED REPORT: MISSION YEARS " + ShipDialog.prevYear + " - " + ShipDialog.year;
            bodyText.text = "> MISSION FAILURE\n";
            bodyText.text += "> The UNSS Teba has landed upon a habitable planet.\n";
            bodyText.text += "> Alas, the last human passed away quietly in cryostasis shortly before landing. We can only hope that some other species will someday find this planet, and find this ship. For then, we will have someone to mourn us.\n";
            bodyText.text += "\n> BEGINNING MEMORIALIZATION PROTOCOLS...";

            GetComponent<StandardDialog>().endWait = 10.0f;

            titleText.GetComponent<TypeOutText>().PrepText();
            bodyText.GetComponent<TypeOutText>().PrepText();

            GetComponent<StandardDialog>().StartDisplay();
        }
    }
}
