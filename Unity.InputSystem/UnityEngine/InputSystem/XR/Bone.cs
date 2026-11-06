using System;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x0200006F RID: 111
	public struct Bone
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x000382C2 File Offset: 0x000364C2
		// (set) Token: 0x06000A32 RID: 2610 RVA: 0x000382CA File Offset: 0x000364CA
		public uint parentBoneIndex
		{
			get
			{
				return this.m_ParentBoneIndex;
			}
			set
			{
				this.m_ParentBoneIndex = value;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x000382D3 File Offset: 0x000364D3
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x000382DB File Offset: 0x000364DB
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

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x000382E4 File Offset: 0x000364E4
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x000382EC File Offset: 0x000364EC
		public Quaternion rotation
		{
			get
			{
				return this.m_Rotation;
			}
			set
			{
				this.m_Rotation = value;
			}
		}

		// Token: 0x0400037A RID: 890
		public uint m_ParentBoneIndex;

		// Token: 0x0400037B RID: 891
		public Vector3 m_Position;

		// Token: 0x0400037C RID: 892
		public Quaternion m_Rotation;
	}
}
