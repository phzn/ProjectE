using System;
using System.Diagnostics;
using UnityEngine.InputSystem.Controls;

namespace UnityEngine.InputSystem
{
	// Token: 0x0200005D RID: 93
	[DebuggerDisplay("Value = {Get()}")]
	public class InputValue
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x000343B4 File Offset: 0x000325B4
		public object Get()
		{
			return this.m_Context.Value.ReadValueAsObject();
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000343D4 File Offset: 0x000325D4
		public TValue Get<TValue>() where TValue : struct
		{
			if (this.m_Context == null)
			{
				throw new InvalidOperationException("Values can only be retrieved while in message callbacks");
			}
			return this.m_Context.Value.ReadValue<TValue>();
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0003440C File Offset: 0x0003260C
		public bool isPressed
		{
			get
			{
				return this.Get<float>() >= ButtonControl.s_GlobalDefaultButtonPressPoint;
			}
		}

		// Token: 0x040002ED RID: 749
		internal InputAction.CallbackContext? m_Context;
	}
}
