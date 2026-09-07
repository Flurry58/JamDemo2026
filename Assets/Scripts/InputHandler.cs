using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    private Dictionary<Key, Action> keybindings;

    private Action<Key> onKeyDetected;

    void Awake()
    {
        keybindings = new Dictionary<Key, Action>();
    }

    public void RegisterKey(Action callback, Key keyToBind)
    {
        keybindings[keyToBind] = callback;
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
    }
    //AI GENERATED CODE BELOW
    void Update()
    {
        // Are we currently waiting for a key?
        if (onKeyDetected != null)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                KeyControl pressedKey = Keyboard.current.allKeys
                    .FirstOrDefault(k => k.wasPressedThisFrame && !k.synthetic);

                if (pressedKey != null)
                {
                    Key key = pressedKey.keyCode;

                    Action<Key> callback = onKeyDetected;
                    onKeyDetected = null;

                    callback(key);
                }
            }

            // Don't process the detected key as a normal binding.
            return;
        }

        // Normal registered key handling
        if (!Keyboard.current.anyKey.wasPressedThisFrame)
            return;

        KeyControl keyPressed = Keyboard.current.allKeys
            .FirstOrDefault(k => k.wasPressedThisFrame);

        if (keyPressed != null &&
            keybindings.TryGetValue(keyPressed.keyCode, out Action action))
        {
            action();
        }
    }
}
