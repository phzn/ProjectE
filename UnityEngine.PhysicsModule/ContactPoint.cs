using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000036 RID: 54
	[UsedByNativeCode]
	[NativeHeader("Modules/Physics/MessageParameters.h")]
	public struct ContactPoint
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00006064 File Offset: 0x00004264
		public Vector3 point
		{
			get
			{
				return this.m_Point;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0000607C File Offset: 0x0000427C
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00006094 File Offset: 0x00004294
		public Vector3 impulse
		{
			get
			{
				return this.m_Impulse;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000060AC File Offset: 0x000042AC
		public Collider thisCollider
		{
			get
			{
				return Physics.GetColliderByInstanceID(this.m_ThisColliderInstanceID);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000060CC File Offset: 0x000042CC
		public Collider otherCollider
		{
			get
			{
				return Physics.GetColliderByInstanceID(this.m_OtherColliderInstanceID);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000060EC File Offset: 0x000042EC
		public float separation
		{
			get
			{
				return this.m_Separation;
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00006104 File Offset: 0x00004304
		internal ContactPoint(Vector3 point, Vector3 normal, Vector3 impulse, float separation, int thisInstanceID, int otherInstenceID)
		{
			this.m_Point = point;
			this.m_Normal = normal;
			this.m_Impulse = impulse;
			this.m_Separation = separation;
			this.m_ThisColliderInstanceID = thisInstanceID;
			this.m_OtherColliderInstanceID = otherInstenceID;
		}

		// Token: 0x040000C1 RID: 193
		internal Vector3 m_Point;

		// Token: 0x040000C2 RID: 194
		internal Vector3 m_Normal;

		// Token: 0x040000C3 RID: 195
		internal Vector3 m_Impulse;

		// Token: 0x040000C4 RID: 196
		internal int m_ThisColliderInstanceID;

		// Token: 0x040000C5 RID: 197
		internal int m_OtherColliderInstanceID;

		// Token: 0x040000C6 RID: 198
		internal float m_Separation;
	}
}
