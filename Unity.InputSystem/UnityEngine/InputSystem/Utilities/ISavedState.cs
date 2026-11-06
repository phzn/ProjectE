using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000148 RID: 328
	internal interface ISavedState
	{
		// Token: 0x0600120A RID: 4618
		void StaticDisposeCurrentState();

		// Token: 0x0600120B RID: 4619
		void RestoreSavedState();
	}
}
