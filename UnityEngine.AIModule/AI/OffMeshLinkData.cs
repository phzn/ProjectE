using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x0200000E RID: 14
	[NativeHeader("Modules/AI/Components/OffMeshLink.bindings.h")]
	[MovedFrom("UnityEngine")]
	public struct OffMeshLinkData
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002AF0 File Offset: 0x00000CF0
		public bool valid
		{
			get
			{
				return this.m_Valid != 0;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00002AFB File Offset: 0x00000CFB
		public bool activated
		{
			get
			{
				return this.m_Activated != 0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002B06 File Offset: 0x00000D06
		public OffMeshLinkType linkType
		{
			get
			{
				return this.m_LinkType;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00002B0E File Offset: 0x00000D0E
		public Vector3 startPos
		{
			get
			{
				return this.m_StartPos;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002B16 File Offset: 0x00000D16
		public Vector3 endPos
		{
			get
			{
				return this.m_EndPos;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00002B1E File Offset: 0x00000D1E
		public OffMeshLink offMeshLink
		{
			get
			{
				return OffMeshLinkData.GetOffMeshLinkInternal(this.m_InstanceID);
			}
		}

		// Token: 0x060000DA RID: 218
		[FreeFunction("OffMeshLinkScriptBindings::GetOffMeshLinkInternal")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern OffMeshLink GetOffMeshLinkInternal(int instanceID);

		// Token: 0x04000022 RID: 34
		internal int m_Valid;

		// Token: 0x04000023 RID: 35
		internal int m_Activated;

		// Token: 0x04000024 RID: 36
		internal int m_InstanceID;

		// Token: 0x04000025 RID: 37
		internal OffMeshLinkType m_LinkType;

		// Token: 0x04000026 RID: 38
		internal Vector3 m_StartPos;

		// Token: 0x04000027 RID: 39
		internal Vector3 m_EndPos;
	}
}
