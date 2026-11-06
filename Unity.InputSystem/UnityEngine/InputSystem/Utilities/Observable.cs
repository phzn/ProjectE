using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.LowLevel;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x0200013E RID: 318
	public static class Observable
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x00054430 File Offset: 0x00052630
		public static IObservable<TValue> Where<TValue>(this IObservable<TValue> source, Func<TValue, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (predicate == null)
			{
				throw new ArgumentNullException("predicate");
			}
			return new WhereObservable<TValue>(source, predicate);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00054455 File Offset: 0x00052655
		public static IObservable<TResult> Select<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> filter)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			return new SelectObservable<TSource, TResult>(source, filter);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0005447A File Offset: 0x0005267A
		public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> filter)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			return new SelectManyObservable<TSource, TResult>(source, filter);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0005449F File Offset: 0x0005269F
		public static IObservable<TValue> Take<TValue>(this IObservable<TValue> source, int count)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return new TakeNObservable<TValue>(source, count);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000544C5 File Offset: 0x000526C5
		public static IObservable<InputEventPtr> ForDevice(this IObservable<InputEventPtr> source, InputDevice device)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new ForDeviceEventObservable(source, null, device);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x000544DD File Offset: 0x000526DD
		public static IObservable<InputEventPtr> ForDevice<TDevice>(this IObservable<InputEventPtr> source) where TDevice : InputDevice
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return new ForDeviceEventObservable(source, typeof(TDevice), null);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00054500 File Offset: 0x00052700
		public static IDisposable CallOnce<TValue>(this IObservable<TValue> source, Action<TValue> action)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			IDisposable subscription = null;
			subscription = source.Take<TValue>(1).Subscribe(new Observer<TValue>(action, delegate
			{
				IDisposable subscription2 = subscription;
				if (subscription2 == null)
				{
					return;
				}
				subscription2.Dispose();
			}));
			return subscription;
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00054560 File Offset: 0x00052760
		public static IDisposable Call<TValue>(this IObservable<TValue> source, Action<TValue> action)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			return source.Subscribe(new Observer<TValue>(action, null));
		}
	}
}
