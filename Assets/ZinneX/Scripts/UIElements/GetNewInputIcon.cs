using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetNewInputIcon : MonoBehaviour {
    [SerializeField] private InputActionReference action;

    private void Start() {
        GetNewIcon();
    }

    public void GetNewIcon() {
        TMP_Text TMPText = GetComponent<TMP_Text>();
        string text = TMPText.text;
        
        string pattern = @"<sprite\s+name=""[^""]+""\s*/?>";
        text = Regex.Replace(text, pattern, $"<sprite name=\"{action.action.GetBindingDisplayString()}\">");

        TMPText.text = text;
    }
}
