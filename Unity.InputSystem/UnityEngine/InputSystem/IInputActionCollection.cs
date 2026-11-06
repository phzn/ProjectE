using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000016 RID: 22
	public interface IInputActionCollection : IEnumerable<InputAction>, IEnumerable
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000122 RID: 290
		// (set) Token: 0x06000123 RID: 291
		InputBinding? bindingMask { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000124 RID: 292
		// (set) Token: 0x06000125 RID: 293
		ReadOnlyArray<InputDevice>? devices { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000126 RID: 294
		ReadOnlyArray<InputControlScheme> controlSchemes { get; }

		// Token: 0x06000127 RID: 295
		bool Contains(InputAction action);

		// Token: 0x06000128 RID: 296
		void Enable();

		// Token: 0x06000129 RID: 297
		void Disable();
	}
}
