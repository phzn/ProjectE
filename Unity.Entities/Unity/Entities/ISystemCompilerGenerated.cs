using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020001C3 RID: 451
	[RequireImplementors]
	public interface ISystemCompilerGenerated
	{
		// Token: 0x06000A59 RID: 2649
		[RequiredMember]
		void OnCreateForCompiler(ref SystemState state);
	}
}
