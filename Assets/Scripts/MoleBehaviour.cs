using UnityEngine;

[CreateAssetMenu(fileName = "New Mole Behaviour", menuName = "ScriptableObjects/MoleBehaviour", order = 1)]
public class MoleBehaviour : ScriptableObject
{
	[Header("Score adjustments")]
	public ScoreAdjustment AdjustmentOnClick;
	public ScoreAdjustment AdjustmentOnTimeout;

	[Header("Other settings")]
	public float ChanceForBehaviour;
	public GameObject Visual;
	public float ActiveTimeModifier;
}