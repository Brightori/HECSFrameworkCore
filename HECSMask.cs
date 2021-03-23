﻿namespace HECSFramework.Core 
{
    public partial struct HECSMask
    {
        public static HECSMask Empty => TypesMap.MaskProvider.Empty();

        public int Index;
        public ulong Mask01;

        public static HECSMask operator +(HECSMask l, HECSMask r) => TypesMap.MaskProvider.GetPlus(l, r);
        public static HECSMask operator -(HECSMask l, HECSMask r) => TypesMap.MaskProvider.GetMinus(l, r);
        public bool Contain(ref HECSMask mask) => TypesMap.MaskProvider.Contains(ref this, ref mask);

        public override bool Equals(object obj)
        {
            return obj is HECSMask mask &&
                   Index == mask.Index;
        }

        public override int GetHashCode()
        {
            return -2134847229 + Index.GetHashCode();
        }
    }
}