using System;

namespace UnityEngine.XR
{
	// Token: 0x02000008 RID: 8
	internal enum InputFeatureType : uint
	{
		// Token: 0x0400002D RID: 45
		Custom,
		// Token: 0x0400002E RID: 46
		Binary,
		// Token: 0x0400002F RID: 47
		DiscreteStates,
		// Token: 0x04000030 RID: 48
		Axis1D,
		// Token: 0x04000031 RID: 49
		Axis2D,
		// Token: 0x04000032 RID: 50
		Axis3D,
		// Token: 0x04000033 RID: 51
		Rotation,
		// Token: 0x04000034 RID: 52
		Hand,
		// Token: 0x04000035 RID: 53
		Bone,
		// Token: 0x04000036 RID: 54
		Eyes,
		// Token: 0x04000037 RID: 55
		kUnityXRInputFeatureTypeInvalid = 4294967295U
	}
}
