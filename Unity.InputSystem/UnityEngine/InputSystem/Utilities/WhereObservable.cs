using System;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000142 RID: 322
	internal class WhereObservable<TValue> : IObservable<TValue>
	{
		// Token: 0x060011A9 RID: 4521 RVA: 0x00054622 File Offset: 0x00052822
		public WhereObservable(IObservable<TValue> source, Func<TValue, bool> predicate)
		{
			this.m_Source = source;
			this.m_Predicate = predicate;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00054638 File Offset: 0x00052838
		public IDisposable Subscribe(IObserver<TValue> observer)
		{
			return this.m_Source.Subscribe(new WhereObservable<TValue>.Where(this, observer));
		}

		// Token: 0x04000723 RID: 1827
		private readonly IObservable<TValue> m_Source;

		// Token: 0x04000724 RID: 1828
		private readonly Func<TValue, bool> m_Predicate;

		// Token: 0x02000250 RID: 592
		private class Where : IObserver<TValue>
		{
			// Token: 0x06001657 RID: 5719 RVA: 0x00066B79 File Offset: 0x00064D79
			public Where(WhereObservable<TValue> observable, IObserver<TValue> observer)
			{
				this.m_Observable = observable;
				this.m_Observer = observer;
			}

			// Token: 0x06001658 RID: 5720 RVA: 0x00066B8F File Offset: 0x00064D8F
			public void OnCompleted()
			{
			}

			// Token: 0x06001659 RID: 5721 RVA: 0x00066B91 File Offset: 0x00064D91
			public void OnError(Exception error)
			{
				Debug.LogException(error);
			}

			// Token: 0x0600165A RID: 5722 RVA: 0x00066B99 File Offset: 0x00064D99
			public void OnNext(TValue evt)
			{
				if (this.m_Observable.m_Predicate(evt))
				{
					this.m_Observer.OnNext(evt);
				}
			}

			// Token: 0x04000C9A RID: 3226
			private WhereObservable<TValue> m_Observable;

			// Token: 0x04000C9B RID: 3227
			private readonly IObserver<TValue> m_Observer;
		}
	}
}
