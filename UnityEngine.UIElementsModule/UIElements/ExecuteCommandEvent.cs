using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200019D RID: 413
	public class ExecuteCommandEvent : CommandEventBase<ExecuteCommandEvent>
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x00032150 File Offset: 0x00030350
		static ExecuteCommandEvent()
		{
			EventBase<ExecuteCommandEvent>.SetCreateFunction(() => new ExecuteCommandEvent());
		}
	}
}
