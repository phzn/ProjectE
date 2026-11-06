using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x02000010 RID: 16
	[MovedFrom("UnityEngine")]
	public struct NavMeshHit
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00002B50 File Offset: 0x00000D50
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00002B68 File Offset: 0x00000D68
		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00002B74 File Offset: 0x00000D74
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00002B8C File Offset: 0x00000D8C
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
			set
			{
				this.m_Normal = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00002B98 File Offset: 0x00000D98
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00002BB0 File Offset: 0x00000DB0
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				this.m_Distance = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00002BBC File Offset: 0x00000DBC
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public int mask
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00002BE0 File Offset: 0x00000DE0
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00002BFB File Offset: 0x00000DFB
		public bool hit
		{
			get
			{
				return this.m_Hit != 0;
			}
			set
			{
				this.m_Hit = (value ? 1 : 0);
			}
		}

		// Token: 0x04000028 RID: 40
		private Vector3 m_Position;

		// Token: 0x04000029 RID: 41
		private Vector3 m_Normal;

		// Token: 0x0400002A RID: 42
		private float m_Distance;

		// Token: 0x0400002B RID: 43
		private int m_Mask;

		// Token: 0x0400002C RID: 44
		private int m_Hit;
	}
}
