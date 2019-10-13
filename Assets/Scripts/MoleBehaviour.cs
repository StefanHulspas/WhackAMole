using UnityEngine;

[CreateAssetMenu(fileName = "New Mole Behaviour", menuName = "ScriptableObjects/MoleBehaviour", order = 1)]
public class MoleBehaviour : ScriptableObject
{
    [Header("On click settings")]
    public int _scoreAdjustmentOnClick;
    public bool _resetScoreMultiplierOnClick;

    [Header("On timeout settings")]
    public int _scoreAdjustmentOnTimeout;
    public bool _resetScoreMultiplierOnTimeout;

    [Header("Other settings")]
    public float _ChanceForBehaviour;
}
