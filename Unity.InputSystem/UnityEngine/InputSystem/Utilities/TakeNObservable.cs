using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000141 RID: 321
	internal class TakeNObservable<TValue> : IObservable<TValue>
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x000545F8 File Offset: 0x000527F8
		public TakeNObservable(IObservable<TValue> source, int count)
		{
			this.m_Source = source;
			this.m_Count = count;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0005460E File Offset: 0x0005280E
		public IDisposable Subscribe(IObserver<TValue> observer)
		{
			return this.m_Source.Subscribe(new TakeNObservable<TValue>.Take(this, observer));
		}

		// Token: 0x04000721 RID: 1825
		private IObservable<TValue> m_Source;

		// Token: 0x04000722 RID: 1826
		private int m_Count;

		// Token: 0x0200024F RID: 591
		private class Take : IObserver<TValue>
		{
			// Token: 0x06001653 RID: 5715 RVA: 0x00066B14 File Offset: 0x00064D14
			public Take(TakeNObservable<TValue> observable, IObserver<TValue> observer)
			{
				this.m_Observer = observer;
				this.m_Remaining = observable.m_Count;
			}

			// Token: 0x06001654 RID: 5716 RVA: 0x00066B2F File Offset: 0x00064D2F
			public void OnCompleted()
			{
			}

			// Token: 0x06001655 RID: 5717 RVA: 0x00066B31 File Offset: 0x00064D31
			public void OnError(Exception error)
			{
				Debug.LogException(error);
			}

			// Token: 0x06001656 RID: 5718 RVA: 0x00066B39 File Offset: 0x00064D39
			public void OnNext(TValue evt)
			{
				if (this.m_Remaining <= 0)
				{
					return;
				}
				this.m_Remaining--;
				this.m_Observer.OnNext(evt);
				if (this.m_Remaining == 0)
				{
					this.m_Observer.OnCompleted();
					this.m_Observer = null;
				}
			}

			// Token: 0x04000C98 RID: 3224
			private IObserver<TValue> m_Observer;

			// Token: 0x04000C99 RID: 3225
			private int m_Remaining;
		}
	}
}
