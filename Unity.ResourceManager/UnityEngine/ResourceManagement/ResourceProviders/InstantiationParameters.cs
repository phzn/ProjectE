using System;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x02000036 RID: 54
	public struct InstantiationParameters
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00006864 File Offset: 0x00004A64
		public Vector3 Position
		{
			get
			{
				return this.m_Position;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000170 RID: 368 RVA: 0x0000686C File Offset: 0x00004A6C
		public Quaternion Rotation
		{
			get
			{
				return this.m_Rotation;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00006874 File Offset: 0x00004A74
		public Transform Parent
		{
			get
			{
				return this.m_Parent;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000172 RID: 370 RVA: 0x0000687C File Offset: 0x00004A7C
		public bool InstantiateInWorldPosition
		{
			get
			{
				return this.m_InstantiateInWorldPosition;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006884 File Offset: 0x00004A84
		public bool SetPositionRotation
		{
			get
			{
				return this.m_SetPositionRotation;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000688C File Offset: 0x00004A8C
		public InstantiationParameters(Transform parent, bool instantiateInWorldSpace)
		{
			this.m_Position = Vector3.zero;
			this.m_Rotation = Quaternion.identity;
			this.m_Parent = parent;
			this.m_InstantiateInWorldPosition = instantiateInWorldSpace;
			this.m_SetPositionRotation = false;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000068B9 File Offset: 0x00004AB9
		public InstantiationParameters(Vector3 position, Quaternion rotation, Transform parent)
		{
			this.m_Position = position;
			this.m_Rotation = rotation;
			this.m_Parent = parent;
			this.m_InstantiateInWorldPosition = false;
			this.m_SetPositionRotation = true;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000068E0 File Offset: 0x00004AE0
		public TObject Instantiate<TObject>(TObject source) where TObject : Object
		{
			TObject tobject;
			if (this.m_Parent == null)
			{
				if (this.m_SetPositionRotation)
				{
					tobject = Object.Instantiate<TObject>(source, this.m_Position, this.m_Rotation);
				}
				else
				{
					tobject = Object.Instantiate<TObject>(source);
				}
			}
			else if (this.m_SetPositionRotation)
			{
				tobject = Object.Instantiate<TObject>(source, this.m_Position, this.m_Rotation, this.m_Parent);
			}
			else
			{
				tobject = Object.Instantiate<TObject>(source, this.m_Parent, this.m_InstantiateInWorldPosition);
			}
			return tobject;
		}

		// Token: 0x04000091 RID: 145
		private Vector3 m_Position;

		// Token: 0x04000092 RID: 146
		private Quaternion m_Rotation;

		// Token: 0x04000093 RID: 147
		private Transform m_Parent;

		// Token: 0x04000094 RID: 148
		private bool m_InstantiateInWorldPosition;

		// Token: 0x04000095 RID: 149
		private bool m_SetPositionRotation;
	}
}
