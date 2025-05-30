using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public Text messageText;
    public float displayTime = 2f;

    private Coroutine currentCoroutine;

    private void OnEnable()
    {
        PickUpMessage.OnPickupMessage += ShowMessage;
    }

    private void OnDisable()
    {
        PickUpMessage.OnPickupMessage -= ShowMessage;
    }

    public void ShowMessage(string itemName)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(DisplayMessageCoroutine(itemName));
    }

    private IEnumerator DisplayMessageCoroutine(string itemName)
    {
        messageText.text = $"{itemName}";
        messageText.enabled = true;

        yield return new WaitForSeconds(displayTime);

        messageText.enabled = false;
    }
}
