using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public Text messageText;          // Reference to the UI Text element used to display messages
    public float displayTime = 2f;    // Duration to show the message

    private Coroutine currentCoroutine; // Reference to the currently running message coroutine

    private void OnEnable()
    {
        // Subscribe to the pickup message event when this object becomes active
        PickUpMessage.OnPickupMessage += ShowMessage;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or unintended calls
        PickUpMessage.OnPickupMessage -= ShowMessage;
    }

    // Called when an item is picked up and a message needs to be shown
    public void ShowMessage(string itemName)
    {
        // Stop any existing message coroutine before starting a new one
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        // Start showing the new message
        currentCoroutine = StartCoroutine(DisplayMessageCoroutine(itemName));
    }

    // Coroutine that handles displaying the message for a certain duration
    private IEnumerator DisplayMessageCoroutine(string itemName)
    {
        messageText.text = $"{itemName}";   // Set the message text
        messageText.enabled = true;         // Show the text

        yield return new WaitForSeconds(displayTime); // Wait for the display duration

        messageText.enabled = false;        // Hide the text after time is up
    }
}
