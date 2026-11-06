using System;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000018 RID: 24
	public interface IInputInteraction
	{
		// Token: 0x0600012D RID: 301
		void Process(ref InputInteractionContext context);

		// Token: 0x0600012E RID: 302
		void Reset();
	}
}
