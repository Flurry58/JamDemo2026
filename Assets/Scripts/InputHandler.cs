using UnityEngine;
using System;
using System.Linq;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class InputHandler : MonoBehaviour
{
    Dictionary<Key, Action> keybindings;

    public void RegisterKey(Action callback, Key keytobind)
    {
        keybindings.Add(keytobind, callback);
    }

    public void UnRegisterKey(Action callback, Key keytobind)
    {
        var keyToRemove = keybindings.FirstOrDefault(kvp => kvp.Value == callback).Key;
        keybindings.Remove(keyToRemove);
    }

    void Awake()
    {
        keybindings = new Dictionary<Key, Action>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Keyboard.current.anyKey.wasPressedThisFrame)
        return;

        var pressedKey = Keyboard.current.allKeys
            .FirstOrDefault(k => k.wasPressedThisFrame);

        if (pressedKey != null && keybindings.TryGetValue(pressedKey.keyCode, out Action action))
        {
            action();
        }
    }
}
