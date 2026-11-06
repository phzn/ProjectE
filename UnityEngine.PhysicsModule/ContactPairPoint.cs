using System;

namespace UnityEngine
{
	// Token: 0x0200003D RID: 61
	public readonly struct ContactPairPoint
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00006BA9 File Offset: 0x00004DA9
		public Vector3 Position
		{
			get
			{
				return this.m_Position;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00006BB1 File Offset: 0x00004DB1
		public float Separation
		{
			get
			{
				return this.m_Separation;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00006BB9 File Offset: 0x00004DB9
		public Vector3 Normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00006BC1 File Offset: 0x00004DC1
		public Vector3 Impulse
		{
			get
			{
				return this.m_Impulse;
			}
		}

		// Token: 0x040000DF RID: 223
		internal readonly Vector3 m_Position;

		// Token: 0x040000E0 RID: 224
		internal readonly float m_Separation;

		// Token: 0x040000E1 RID: 225
		internal readonly Vector3 m_Normal;

		// Token: 0x040000E2 RID: 226
		internal readonly uint m_InternalFaceIndex0;

		// Token: 0x040000E3 RID: 227
		internal readonly Vector3 m_Impulse;

		// Token: 0x040000E4 RID: 228
		internal readonly uint m_InternalFaceIndex1;
	}
}
