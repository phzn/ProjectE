using System;
using UnityEngine.Scripting;

namespace Unity.Entities
{
	// Token: 0x020001C2 RID: 450
	[RequireImplementors]
	public interface ISystemStartStop
	{
		// Token: 0x06000A57 RID: 2647
		[RequiredMember]
		void OnStartRunning(ref SystemState state);

		// Token: 0x06000A58 RID: 2648
		[RequiredMember]
		void OnStopRunning(ref SystemState state);
	}
}
