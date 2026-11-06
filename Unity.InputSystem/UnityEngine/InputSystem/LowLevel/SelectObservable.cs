using System;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000FE RID: 254
	internal class SelectObservable<TSource, TResult> : IObservable<TResult>
	{
		// Token: 0x06000F08 RID: 3848 RVA: 0x00049EA7 File Offset: 0x000480A7
		public SelectObservable(IObservable<TSource> source, Func<TSource, TResult> filter)
		{
			this.m_Source = source;
			this.m_Filter = filter;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00049EBD File Offset: 0x000480BD
		public IDisposable Subscribe(IObserver<TResult> observer)
		{
			return this.m_Source.Subscribe(new SelectObservable<TSource, TResult>.Select(this, observer));
		}

		// Token: 0x04000646 RID: 1606
		private readonly IObservable<TSource> m_Source;

		// Token: 0x04000647 RID: 1607
		private readonly Func<TSource, TResult> m_Filter;

		// Token: 0x0200022A RID: 554
		private class Select : IObserver<TSource>
		{
			// Token: 0x0600157F RID: 5503 RVA: 0x000638ED File Offset: 0x00061AED
			public Select(SelectObservable<TSource, TResult> observable, IObserver<TResult> observer)
			{
				this.m_Observable = observable;
				this.m_Observer = observer;
			}

			// Token: 0x06001580 RID: 5504 RVA: 0x00063903 File Offset: 0x00061B03
			public void OnCompleted()
			{
			}

			// Token: 0x06001581 RID: 5505 RVA: 0x00063905 File Offset: 0x00061B05
			public void OnError(Exception error)
			{
				Debug.LogException(error);
			}

			// Token: 0x06001582 RID: 5506 RVA: 0x00063910 File Offset: 0x00061B10
			public void OnNext(TSource evt)
			{
				TResult tresult = this.m_Observable.m_Filter(evt);
				this.m_Observer.OnNext(tresult);
			}

			// Token: 0x04000BD7 RID: 3031
			private SelectObservable<TSource, TResult> m_Observable;

			// Token: 0x04000BD8 RID: 3032
			private readonly IObserver<TResult> m_Observer;
		}
	}
}
