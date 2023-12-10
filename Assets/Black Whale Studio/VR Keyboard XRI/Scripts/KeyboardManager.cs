/*
 * Copyright (c) 2023 Black Whale Studio. All rights reserved.
 *
 * This software is the intellectual property of Black Whale Studio. Direct use, copying, or distribution of this code in its original or only slightly modified form is strictly prohibited. Significant modifications or derivations are required for any use.
 *
 * If this code is intended to be used in a commercial setting, you must contact Black Whale Studio for explicit permission.
 *
 * For the full licensing terms and conditions, visit:
 * https://blackwhale.dev/
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT ANY WARRANTIES OR CONDITIONS.
 *
 * For questions or to join our community, please visit our Discord: https://discord.gg/55gtTryfWw
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        [Header("Keyboard Setup")]
        [SerializeField] private KeyChannel keyChannel;
        [SerializeField] private Button spacebarButton;
        [SerializeField] private Button speechButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button switchButton;

        private TextMeshProUGUI switchButtonText;
        
        [Header("Keyboards")]
        [SerializeField] private GameObject numbersKeyboard;

        [Header("Shift/Caps Lock Button")] 
        [SerializeField] internal bool autoCapsAtStart = true;
        [SerializeField] private Button shiftButton;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite activeSprite;
        
        [Header("Switch Number/Special Button")]
        [SerializeField] private Button switchNumberSpecialButton;

        private TextMeshProUGUI switchNumSpecButtonText;
        
        [Header("Keyboard Button Colors")]
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color highlightedColor = Color.yellow;
        [SerializeField] private Color pressedColor = Color.red;
        [SerializeField] private Color selectedColor = Color.blue;
        
        [Header("Output Field Settings")]
        [SerializeField] private TMP_InputField outputField;
        [SerializeField] private Button enterButton;
        [SerializeField] private int maxCharacters = 15;

        private ColorBlock shiftButtonColors;
        private bool isFirstKeyPress = true;
        private bool keyHasBeenPressed;
        private bool shiftActive;
        private bool capsLockActive;
        private bool specialCharactersActive;
        private float lastShiftClickTime;
        private float shiftDoubleClickDelay = 0.5f;

        public UnityEvent onKeyboardModeChanged;

        private void Awake()
        {
            
            CheckTextLength();
            numbersKeyboard.SetActive(true);
            keyChannel.RaiseKeyColorsChangedEvent(normalColor, highlightedColor, pressedColor, selectedColor);
            numbersKeyboard.SetActive(true);

            if (!autoCapsAtStart) return;
            ActivateShift();
        }


        private void OnEnable() => keyChannel.OnKeyPressed += KeyPress;

        private void OnDisable() => keyChannel.OnKeyPressed -= KeyPress;

        private void KeyPress(string key)
        {
            keyHasBeenPressed = true;

            if (outputField.text.Length < 3)
            {
                outputField.text = "192.168.1." + key;
            } else if (outputField.text.Length < 13)
            {
                outputField.text = outputField.text + key;
            }
            else
            {
                outputField.text = "192.168.1." + key;
            }

            if (isFirstKeyPress)
            {
                isFirstKeyPress = false;
                keyChannel.onFirstKeyPress.Invoke();
            }
    
        }

        private void OnSpacePress()
        {
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(startPos, " ");

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + 1;
            
            CheckTextLength();
        }

        private void OnDeletePress()
        {
            if (string.IsNullOrEmpty(outputField.text)) return;
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            if (endPos > startPos)
            {
                outputField.text = outputField.text.Remove(startPos, endPos - startPos);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos;
            }
            else if (startPos > 0)
            {
                outputField.text = outputField.text.Remove(startPos - 1, 1);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos - 1;
            }
            
            CheckTextLength();
        }

        private void CheckTextLength()
        {
            int currentLength = outputField.text.Length;

            // Raise event to enable or disable keys based on the text length
            bool keysEnabled = currentLength < maxCharacters;
            keyChannel.RaiseKeysStateChangeEvent(keysEnabled);
            
            // Disable shift/caps lock if maximum text length is reached
            if (currentLength != maxCharacters) return;
            DeactivateShift();
            capsLockActive = false;
        }


        private void OnShiftPress()
        {
            if (capsLockActive)
            {
                // If Caps Lock is active, deactivate it
                capsLockActive = false;
                shiftActive = false;
            }
            else switch (shiftActive)
            {
                case true when !keyHasBeenPressed && Time.time - lastShiftClickTime < shiftDoubleClickDelay:
                    // If Shift is active, a key has not been pressed, and Shift button was double clicked, activate Caps Lock
                    capsLockActive = true;
                    shiftActive = false;
                    break;
                case true when !keyHasBeenPressed:
                    // If Shift is active, a key has not been pressed, deactivate Shift
                    shiftActive = false;
                    break;
                case false:
                    // If Shift is not active and Shift button was clicked once, activate Shift
                    shiftActive = true;
                    break;
            }

            lastShiftClickTime = Time.time;
            onKeyboardModeChanged?.Invoke();
        }

        private void ActivateShift()
        {
            if (!capsLockActive) shiftActive = true;

            onKeyboardModeChanged?.Invoke();
        }

        public void DeactivateShift()
        {
            if (shiftActive && !capsLockActive && keyHasBeenPressed)
            {
                shiftActive = false;
                onKeyboardModeChanged?.Invoke();
            }

            keyHasBeenPressed = false;
        }

        public bool IsShiftActive() => shiftActive;

        public bool IsCapsLockActive() => capsLockActive;
    }
}