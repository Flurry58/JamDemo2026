using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    private Dictionary<Key, Action> keybindings;

    private Action<Key> onKeyDetected;

    private void Awake()
    {
        keybindings = new Dictionary<Key, Action>();
    }

    public void RegisterKey(Action callback, Key keyToBind)
    {
        keybindings[keyToBind] = callback;

        Debug.Log($"Registered {keyToBind}");
    }

    public void UnRegisterKey(Action callback, Key keyToBind)
    {
        if (keybindings.TryGetValue(keyToBind, out Action registeredAction) &&
            registeredAction == callback)
        {
            keybindings.Remove(keyToBind);
        }
    }

    public void DetectNextKey(Action<Key> callback)
    {
        onKeyDetected = callback;

        Debug.Log("Waiting for key...");
    }

    private void Update()
    {
        // --------------------------------
        // REBINDING MODE
        // --------------------------------

        if (onKeyDetected != null)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if (!key.wasPressedThisFrame || key.synthetic)
                    continue;

                Key detectedKey = key.keyCode;

                Action<Key> callback = onKeyDetected;
                onKeyDetected = null;

                callback(detectedKey);

                // Only use the first key pressed for rebinding.
                return;
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

