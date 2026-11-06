using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200019B RID: 411
	public class ValidateCommandEvent : CommandEventBase<ValidateCommandEvent>
	{
		// Token: 0x06000C99 RID: 3225 RVA: 0x0003211B File Offset: 0x0003031B
		static ValidateCommandEvent()
		{
			EventBase<ValidateCommandEvent>.SetCreateFunction(() => new ValidateCommandEvent());
		}
	}
}
