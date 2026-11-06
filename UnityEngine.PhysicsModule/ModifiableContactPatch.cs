using System;

namespace UnityEngine
{
	// Token: 0x02000024 RID: 36
	internal struct ModifiableContactPatch
	{
		// Token: 0x040000A7 RID: 167
		public ModifiableMassProperties massProperties;

		// Token: 0x040000A8 RID: 168
		public Vector3 normal;

		// Token: 0x040000A9 RID: 169
		public float restitution;

		// Token: 0x040000AA RID: 170
		public float dynamicFriction;

		// Token: 0x040000AB RID: 171
		public float staticFriction;

		// Token: 0x040000AC RID: 172
		public byte startContactIndex;

		// Token: 0x040000AD RID: 173
		public byte contactCount;

		// Token: 0x040000AE RID: 174
		public byte materialFlags;

		// Token: 0x040000AF RID: 175
		public byte internalFlags;

		// Token: 0x040000B0 RID: 176
		public ushort materialIndex;

		// Token: 0x040000B1 RID: 177
		public ushort otherMaterialIndex;

		// Token: 0x02000025 RID: 37
		public enum Flags
		{
			// Token: 0x040000B3 RID: 179
			HasFaceIndices = 1,
			// Token: 0x040000B4 RID: 180
			HasModifiedMassRatios = 8,
			// Token: 0x040000B5 RID: 181
			HasTargetVelocity = 16,
			// Token: 0x040000B6 RID: 182
			HasMaxImpulse = 32,
			// Token: 0x040000B7 RID: 183
			RegeneratePatches = 64
		}
	}
}
