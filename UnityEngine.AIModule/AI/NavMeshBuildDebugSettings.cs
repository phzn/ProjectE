using System;
using UnityEngine.Bindings;

namespace UnityEngine.AI
{
	// Token: 0x02000021 RID: 33
	[NativeHeader("Modules/AI/Public/NavMeshBuildDebugSettings.h")]
	public struct NavMeshBuildDebugSettings
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000389C File Offset: 0x00001A9C
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x000038B4 File Offset: 0x00001AB4
		public NavMeshBuildDebugFlags flags
		{
			get
			{
				return (NavMeshBuildDebugFlags)this.m_Flags;
			}
			set
			{
				this.m_Flags = (byte)value;
			}
		}

		// Token: 0x04000078 RID: 120
		private byte m_Flags;
	}
}
