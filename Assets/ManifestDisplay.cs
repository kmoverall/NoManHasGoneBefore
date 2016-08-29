using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ManifestDisplay : MonoBehaviour {

    static List<Text> aliveNameList = new List<Text>();
    static List<Text> aliveAgeList = new List<Text>();
    static List<Text> deadNameList = new List<Text>();
    static List<Text> deadAgeList = new List<Text>();

    public Text aliveListItem;
    public Text deadListItem;

    public RectTransform aliveList;
    public RectTransform deadList;

    public void UpdateLists() {
        for (int i = 0; i < ShipDialog.manifest.Count; i++) {
            if (ShipDialog.manifest[i].isAlive) {
                aliveNameList.Add(Instantiate(aliveListItem));
                aliveAgeList.Add(Instantiate(aliveListItem));
                aliveNameList[aliveNameList.Count - 1].text = ShipDialog.manifest[i].lastName + ", " + ShipDialog.manifest[i].firstName;
                aliveAgeList[aliveAgeList.Count - 1].text = ShipDialog.manifest[i].age != -1 ? "" + ShipDialog.manifest[i].age  : "<ERROR>";

                aliveNameList[aliveNameList.Count - 1].rectTransform.anchoredPosition = new Vector2(20, -20 - 48 * (aliveNameList.Count - 1));
                aliveAgeList[aliveAgeList.Count - 1].rectTransform.anchoredPosition = new Vector2(620, -20 - 48 * (aliveAgeList.Count - 1));

                aliveNameList[aliveNameList.Count - 1].rectTransform.SetParent(aliveList, false);
                aliveAgeList[aliveAgeList.Count - 1].rectTransform.SetParent(aliveList, false);
            }
            else {
                deadNameList.Add(Instantiate(deadListItem));
                deadAgeList.Add(Instantiate(deadListItem));
                deadNameList[deadNameList.Count - 1].text = ShipDialog.manifest[i].lastName + ", " + ShipDialog.manifest[i].firstName;
                deadAgeList[deadAgeList.Count - 1].text = ShipDialog.manifest[i].age != -1 ? "" + ShipDialog.manifest[i].age : "<ERROR>";

                deadNameList[deadNameList.Count - 1].rectTransform.anchoredPosition = new Vector2(20, -20 - 48 * (deadNameList.Count - 1));
                deadAgeList[deadAgeList.Count - 1].rectTransform.anchoredPosition = new Vector2(620, -20 - 48 * (deadAgeList.Count - 1));

                deadNameList[deadNameList.Count - 1].rectTransform.SetParent(deadList, false);
                deadAgeList[deadAgeList.Count - 1].rectTransform.SetParent(deadList, false);
            }
        }

        aliveList.sizeDelta = new Vector2(0, 20 + 48 * aliveNameList.Count);
        deadList.sizeDelta = new Vector2(0, 20 + 48 * deadNameList.Count);
    }
}
