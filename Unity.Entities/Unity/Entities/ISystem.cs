using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020001BF RID: 447
	[RequireImplementors]
	public interface ISystem
	{
		// Token: 0x06000A50 RID: 2640 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		void OnCreate(ref SystemState state)
		{
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		void OnDestroy(ref SystemState state)
		{
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00002287 File Offset: 0x00000487
		[RequiredMember]
		void OnUpdate(ref SystemState state)
		{
		}
	}
}
