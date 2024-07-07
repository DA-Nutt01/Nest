using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPanel : MonoBehaviour
{

    [SerializeField][Tooltip("Reference to the point text game object, assign in editor")]
   private TextMeshProUGUI _controlStateText;

    // Update is called once per frame
    void Update()
    {
        ControlState controlState = PlayerController.Instance.GetControlState();
        _controlStateText.text = "Control State: " + controlState.ToString();
    }
}
