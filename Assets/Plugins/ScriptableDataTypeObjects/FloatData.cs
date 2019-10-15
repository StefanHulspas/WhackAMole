using UnityEngine;

namespace ScriptableDataType
{
	[CreateAssetMenu(fileName = "New FloatData", menuName = "ScriptableObjects/VarData/Float", order = 1)]
	public class FloatData : VarData<float>
	{
		public FloatData(float b) : base(b) { }

		public static float operator +(FloatData a, float b)
		{
			return a.Value + b;
		}

		public static float operator +(float a, FloatData b)
		{
			return a + b.Value;
		}

		public static float operator +(FloatData a, FloatData b)
		{
			return a.Value + b.Value;
		}

		public static float operator -(FloatData a, float b)
		{
			return a.Value - b;
		}

		public static float operator -(float a, FloatData b)
		{
			return a - b.Value;
		}

		public static float operator -(FloatData a, FloatData b)
		{
			return a.Value - b.Value;
		}

		public static FloatData operator ++(FloatData a)
		{
			a.Value++;
			return a;
		}

		public static FloatData operator --(FloatData a)
		{
			a.Value--;
			return a;
		}
	}
}