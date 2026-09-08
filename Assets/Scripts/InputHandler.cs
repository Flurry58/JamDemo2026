using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System.Linq;

public class InputHandler : MonoBehaviour
{
    public static Dictionary<string, Key> savedBindings = new Dictionary<string, Key>();
    private Dictionary<Key, Action> keybindings = new Dictionary<Key, Action>();
    private static readonly HashSet<Key> wasdKeys = new HashSet<Key> { Key.W, Key.A, Key.S, Key.D };


    private bool onKeyDetected = false;
    private Action tobind;
    private Action keybounded;
    private GameObject buttonspawn;

    public bool RegisterByName(string actionName, Action callback)
    {
        if (!savedBindings.TryGetValue(actionName, out Key keyToUse))
        {
            return false; // no saved key for this action
        }

        keybindings[keyToUse] = callback;
        return true;
    }

    public void RebindByName(string actionName, Action callback, Key newKey)
    {
        if (savedBindings.TryGetValue(actionName, out Key oldKey))
        {
            keybindings.Remove(oldKey);
        }

        keybindings[newKey] = callback;
        savedBindings[actionName] = newKey;
    }
    public void RemoveBinding(string actionName)
    {
        if (savedBindings.TryGetValue(actionName, out Key boundKey))
        {
            keybindings.Remove(boundKey);
            savedBindings.Remove(actionName);

            Debug.Log($"Removed binding for '{actionName}' (was {boundKey})");
        }
        else
        {
            Debug.LogWarning($"No binding found for '{actionName}'");
        }
    }

    public void DetectNextKey(Action boundaction, GameObject uidisplay, Action KeyBounded)
    {
        onKeyDetected = true;
        tobind = boundaction;
        buttonspawn = uidisplay;
        keybounded = KeyBounded;
        Debug.Log("Waiting for key...");
    }

    private void Update()
    {
        // --------------------------------
        // REBINDING MODE
        // --------------------------------

        if (onKeyDetected == true)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if (!key.wasPressedThisFrame || key.synthetic)
                    continue;

                Key detectedKey = key.keyCode;
                if (!wasdKeys.Contains(detectedKey) && !keybindings.ContainsKey(detectedKey))
                {
                    onKeyDetected = false;
                    
                    Destroy(buttonspawn);
                    RebindByName(tobind.Method.Name, tobind, detectedKey);
                    
                    keybounded();
                    // Only use the first key pressed for rebinding.
                    return;
                }
            }

            return;
        }

        // --------------------------------
        // NORMAL INPUT
        // --------------------------------

        foreach (KeyControl key in Keyboard.current.allKeys)
        {
            if (!key.wasPressedThisFrame || key.synthetic)
                continue;

            if (keybindings.TryGetValue(key.keyCode, out Action action))
            {
                Debug.Log($"Executing action for {key.keyCode}");

                action();
            }
        }
    }
}

