using System;
using System.Collections.Generic;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000140 RID: 320
	internal class SelectManyObservable<TSource, TResult> : IObservable<TResult>
	{
		// Token: 0x060011A5 RID: 4517 RVA: 0x000545CE File Offset: 0x000527CE
		public SelectManyObservable(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> filter)
		{
			this.m_Source = source;
			this.m_Filter = filter;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x000545E4 File Offset: 0x000527E4
		public IDisposable Subscribe(IObserver<TResult> observer)
		{
			return this.m_Source.Subscribe(new SelectManyObservable<TSource, TResult>.Select(this, observer));
		}

		// Token: 0x0400071F RID: 1823
		private readonly IObservable<TSource> m_Source;

		// Token: 0x04000720 RID: 1824
		private readonly Func<TSource, IEnumerable<TResult>> m_Filter;

		// Token: 0x0200024E RID: 590
		private class Select : IObserver<TSource>
		{
			// Token: 0x0600164F RID: 5711 RVA: 0x00066A92 File Offset: 0x00064C92
			public Select(SelectManyObservable<TSource, TResult> observable, IObserver<TResult> observer)
			{
				this.m_Observable = observable;
				this.m_Observer = observer;
			}

			// Token: 0x06001650 RID: 5712 RVA: 0x00066AA8 File Offset: 0x00064CA8
			public void OnCompleted()
			{
			}

			// Token: 0x06001651 RID: 5713 RVA: 0x00066AAA File Offset: 0x00064CAA
			public void OnError(Exception error)
			{
				Debug.LogException(error);
			}

			// Token: 0x06001652 RID: 5714 RVA: 0x00066AB4 File Offset: 0x00064CB4
			public void OnNext(TSource evt)
			{
				foreach (TResult tresult in this.m_Observable.m_Filter(evt))
				{
					this.m_Observer.OnNext(tresult);
				}
			}

			// Token: 0x04000C96 RID: 3222
			private SelectManyObservable<TSource, TResult> m_Observable;

			// Token: 0x04000C97 RID: 3223
			private readonly IObserver<TResult> m_Observer;
		}
	}
}
