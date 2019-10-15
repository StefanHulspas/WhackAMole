using UnityEngine;

namespace ScriptableDataType
{
    [CreateAssetMenu(fileName = "New IntData", menuName = "ScriptableObjects/VarData/Int", order = 1)]
    public class IntData : VarData<int>
    {
        public IntData(int b) : base(b) { }

        public static int operator +(IntData a, int b)
        {
            return a.Value + b;
        }

        public static int operator +(int a, IntData b)
        {
            return a + b.Value;
        }

        public static int operator +(IntData a, IntData b)
        {
            return a.Value + b.Value;
        }

        public static int operator -(IntData a, int b)
        {
            return a.Value - b;
        }

        public static int operator -(int a, IntData b)
        {
            return a - b.Value;
        }

        public static int operator -(IntData a, IntData b)
        {
            return a.Value - b.Value;
        }

        public static IntData operator ++(IntData a)
        {
            a.Value ++;
            return a;
        }

        public static IntData operator --(IntData a)
        {
            a.Value--;
            return a;
        }
    }
}



