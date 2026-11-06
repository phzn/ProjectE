using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013F RID: 319
	internal class Observer<TValue> : IObserver<TValue>
	{
		// Token: 0x060011A1 RID: 4513 RVA: 0x0005458B File Offset: 0x0005278B
		public Observer(Action<TValue> onNext, Action onCompleted = null)
		{
			this.m_OnNext = onNext;
			this.m_OnCompleted = onCompleted;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000545A1 File Offset: 0x000527A1
		public void OnCompleted()
		{
			Action onCompleted = this.m_OnCompleted;
			if (onCompleted == null)
			{
				return;
			}
			onCompleted();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x000545B3 File Offset: 0x000527B3
		public void OnError(Exception error)
		{
			Debug.LogException(error);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000545BB File Offset: 0x000527BB
		public void OnNext(TValue evt)
		{
			Action<TValue> onNext = this.m_OnNext;
			if (onNext == null)
			{
				return;
			}
			onNext(evt);
		}

		// Token: 0x0400071D RID: 1821
		private Action<TValue> m_OnNext;

		// Token: 0x0400071E RID: 1822
		private Action m_OnCompleted;
	}
}
