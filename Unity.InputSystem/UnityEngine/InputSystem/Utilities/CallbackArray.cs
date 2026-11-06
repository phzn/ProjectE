using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200012C RID: 300
	internal struct CallbackArray<TDelegate> where TDelegate : Delegate
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00051333 File Offset: 0x0004F533
		public int length
		{
			get
			{
				return this.m_Callbacks.length;
			}
		}

		// Token: 0x170004D3 RID: 1235
		public TDelegate this[int index]
		{
			get
			{
				return this.m_Callbacks[index];
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0005134E File Offset: 0x0004F54E
		public void Clear()
		{
			this.m_Callbacks.Clear();
			this.m_CallbacksToAdd.Clear();
			this.m_CallbacksToRemove.Clear();
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00051374 File Offset: 0x0004F574
		public void AddCallback(TDelegate dlg)
		{
			if (!this.m_CannotMutateCallbacksArray)
			{
				if (!this.m_Callbacks.Contains(dlg))
				{
					this.m_Callbacks.AppendWithCapacity(dlg, 4);
				}
				return;
			}
			if (this.m_CallbacksToAdd.Contains(dlg))
			{
				return;
			}
			int num = this.m_CallbacksToRemove.IndexOf(dlg);
			if (num != -1)
			{
				this.m_CallbacksToRemove.RemoveAtByMovingTailWithCapacity(num);
			}
			this.m_CallbacksToAdd.AppendWithCapacity(dlg, 10);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000513E4 File Offset: 0x0004F5E4
		public void RemoveCallback(TDelegate dlg)
		{
			if (!this.m_CannotMutateCallbacksArray)
			{
				int num = this.m_Callbacks.IndexOf(dlg);
				if (num >= 0)
				{
					this.m_Callbacks.RemoveAtWithCapacity(num);
				}
				return;
			}
			if (this.m_CallbacksToRemove.Contains(dlg))
			{
				return;
			}
			int num2 = this.m_CallbacksToAdd.IndexOf(dlg);
			if (num2 != -1)
			{
				this.m_CallbacksToAdd.RemoveAtByMovingTailWithCapacity(num2);
			}
			this.m_CallbacksToRemove.AppendWithCapacity(dlg, 10);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00051452 File Offset: 0x0004F652
		public void LockForChanges()
		{
			this.m_CannotMutateCallbacksArray = true;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0005145C File Offset: 0x0004F65C
		public void UnlockForChanges()
		{
			this.m_CannotMutateCallbacksArray = false;
			for (int i = 0; i < this.m_CallbacksToRemove.length; i++)
			{
				this.RemoveCallback(this.m_CallbacksToRemove[i]);
			}
			for (int j = 0; j < this.m_CallbacksToAdd.length; j++)
			{
				this.AddCallback(this.m_CallbacksToAdd[j]);
			}
			this.m_CallbacksToAdd.Clear();
			this.m_CallbacksToRemove.Clear();
		}

		// Token: 0x04000705 RID: 1797
		private bool m_CannotMutateCallbacksArray;

		// Token: 0x04000706 RID: 1798
		private InlinedArray<TDelegate> m_Callbacks;

		// Token: 0x04000707 RID: 1799
		private InlinedArray<TDelegate> m_CallbacksToAdd;

		// Token: 0x04000708 RID: 1800
		private InlinedArray<TDelegate> m_CallbacksToRemove;
	}
}
