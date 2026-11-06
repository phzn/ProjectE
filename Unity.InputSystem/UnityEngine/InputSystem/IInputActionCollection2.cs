using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000017 RID: 23
	public interface IInputActionCollection2 : IInputActionCollection, IEnumerable<InputAction>, IEnumerable
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600012A RID: 298
		IEnumerable<InputBinding> bindings { get; }

		// Token: 0x0600012B RID: 299
		InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false);

		// Token: 0x0600012C RID: 300
		int FindBinding(InputBinding mask, out InputAction action);
	}
}
