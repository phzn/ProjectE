using System;

namespace UnityEngine
{
	// Token: 0x02000023 RID: 35
	internal struct ModifiableContact
	{
		// Token: 0x0400009C RID: 156
		public Vector3 contact;

		// Token: 0x0400009D RID: 157
		public float separation;

		// Token: 0x0400009E RID: 158
		public Vector3 targetVelocity;

		// Token: 0x0400009F RID: 159
		public float maxImpulse;

		// Token: 0x040000A0 RID: 160
		public Vector3 normal;

		// Token: 0x040000A1 RID: 161
		public float restitution;

		// Token: 0x040000A2 RID: 162
		public uint materialFlags;

		// Token: 0x040000A3 RID: 163
		public ushort materialIndex;

		// Token: 0x040000A4 RID: 164
		public ushort otherMaterialIndex;

		// Token: 0x040000A5 RID: 165
		public float staticFriction;

		// Token: 0x040000A6 RID: 166
		public float dynamicFriction;
	}
}
