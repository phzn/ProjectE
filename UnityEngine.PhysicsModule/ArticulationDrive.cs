using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000019 RID: 25
	[NativeHeader("Modules/Physics/ArticulationBody.h")]
	public struct ArticulationDrive
	{
		// Token: 0x04000071 RID: 113
		public float lowerLimit;

		// Token: 0x04000072 RID: 114
		public float upperLimit;

		// Token: 0x04000073 RID: 115
		public float stiffness;

		// Token: 0x04000074 RID: 116
		public float damping;

		// Token: 0x04000075 RID: 117
		public float forceLimit;

		// Token: 0x04000076 RID: 118
		public float target;

		// Token: 0x04000077 RID: 119
		public float targetVelocity;

		// Token: 0x04000078 RID: 120
		public ArticulationDriveType driveType;
	}
}
