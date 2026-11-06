using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000149 RID: 329
	internal sealed class SavedStructState<T> : ISavedState where T : struct
	{
		// Token: 0x0600120C RID: 4620 RVA: 0x00055AE0 File Offset: 0x00053CE0
		internal SavedStructState(ref T state, SavedStructState<T>.TypedRestore restoreAction, Action staticDisposeCurrentState = null)
		{
			this.m_State = state;
			this.m_RestoreAction = restoreAction;
			this.m_StaticDisposeCurrentState = staticDisposeCurrentState;
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00055B02 File Offset: 0x00053D02
		public void StaticDisposeCurrentState()
		{
			if (this.m_StaticDisposeCurrentState != null)
			{
				this.m_StaticDisposeCurrentState();
				this.m_StaticDisposeCurrentState = null;
			}
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00055B1E File Offset: 0x00053D1E
		public void RestoreSavedState()
		{
			this.m_RestoreAction(ref this.m_State);
			this.m_RestoreAction = null;
		}

		// Token: 0x04000739 RID: 1849
		private T m_State;

		// Token: 0x0400073A RID: 1850
		private SavedStructState<T>.TypedRestore m_RestoreAction;

		// Token: 0x0400073B RID: 1851
		private Action m_StaticDisposeCurrentState;

		// Token: 0x02000253 RID: 595
		// (Invoke) Token: 0x06001668 RID: 5736
		public delegate void TypedRestore(ref T state);
	}
}
