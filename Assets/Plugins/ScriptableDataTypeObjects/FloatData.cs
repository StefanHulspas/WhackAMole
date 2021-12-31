using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New FloatData", menuName = "ScriptableObjects/VarData/Float", order = 1)]
	public class FloatData : VarData<float>
	{
		public FloatData(float b) : base(b) { }

		public static implicit operator FloatData (float data) {
			return new FloatData(data);
		}
	}
}