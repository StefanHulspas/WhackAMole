using ScriptableDataType;
using TMPro;
using UnityEngine;

public class ShowIntData : MonoBehaviour
{
    [SerializeField]
    private IntData _intValue;

    private TMP_Text _textField;

    private void OnEnable()
    {
        _textField = GetComponent<TMP_Text>();
        _intValue.AddListener(UpdateValue);
    }

    private void OnDisable()
    {
        _intValue.RemoveListener(UpdateValue);
    }

    private void UpdateValue(int newValue)
    {
        _textField.text = newValue.ToString();
    }
}
