using ScriptableDataType;
using TMPro;
using UnityEngine;

public class ShowFloatData : MonoBehaviour
{
    [SerializeField]
    private FloatData _floatValue = default;
    [SerializeField]
    private string _valueFormat = "F";

    private TMP_Text _textField;

    private void OnEnable()
    {
        _textField = GetComponent<TMP_Text>();
        _floatValue.AddListener(UpdateValue);
    }

    private void OnDisable()
    {
        _floatValue.RemoveListener(UpdateValue);
    }

    private void UpdateValue(float newValue)
    {
        _textField.text = newValue.ToString(_valueFormat);
    }
}
