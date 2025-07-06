using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] RectTransform[] options;
    [SerializeField] private AudioClip changesfx;
    [SerializeField] private AudioClip interactsfx;
    private RectTransform rect;
    private int currPos;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //change pos for pointer
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePos(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePos(1);
        
        //interact
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void ChangePos(int _change)
    {
        currPos += _change;
        
        if (_change != 0)
            SoundManager.instance.PlaySound(changesfx);
        
        if (currPos < 0)
            currPos = options.Length - 1;
        else if (currPos > options.Length - 1)
            currPos = 0;
        
        rect.position = new Vector3(rect.position.x, options[currPos].position.y, 0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactsfx);
        //access -> call
        options[currPos].GetComponent<Button>().onClick.Invoke();
    }
}
