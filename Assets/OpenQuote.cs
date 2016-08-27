using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OpenQuote : Display {

    public TypeOutText quote;
    public TypeOutText attribution;
    public float initialWait;
    public float inbetweenWait;

    public override void StartDisplay() {
        StartCoroutine("BeginScene");
    }

    IEnumerator BeginScene() {
        yield return new WaitForSeconds(initialWait);
        quote.StartTyping();
        while(!quote.isFinished) {
            yield return null;
        }

        yield return new WaitForSeconds(inbetweenWait);
        attribution.StartTyping();
        while (!attribution.isFinished) {
            yield return null;
        }

        isReadyToClose = true;
    }
}
