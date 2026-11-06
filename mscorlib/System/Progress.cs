using System;
using System.Threading;

namespace System
{
	/// <summary>Provides an <see cref="T:System.IProgress`1" /> that invokes callbacks for each reported progress value.</summary>
	/// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
	// Token: 0x02000170 RID: 368
	public class Progress<T> : IProgress<T>
	{
		/// <summary>Initializes the <see cref="T:System.Progress`1" /> object.</summary>
		// Token: 0x06000E8D RID: 3725 RVA: 0x0003BA25 File Offset: 0x00039C25
		public Progress()
		{
			this._synchronizationContext = SynchronizationContext.Current ?? ProgressStatics.DefaultContext;
			this._invokeHandlers = new SendOrPostCallback(this.InvokeHandlers);
		}

		/// <summary>Initializes the <see cref="T:System.Progress`1" /> object with the specified callback.</summary>
		/// <param name="handler">A handler to invoke for each reported progress value. This handler will be invoked in addition to any delegates registered with the <see cref="E:System.Progress`1.ProgressChanged" /> event. Depending on the <see cref="T:System.Threading.SynchronizationContext" /> instance captured by the <see cref="T:System.Progress`1" /> at construction, it is possible that this handler instance could be invoked concurrently with itself.</param>
		// Token: 0x06000E8E RID: 3726 RVA: 0x0003BA53 File Offset: 0x00039C53
		public Progress(Action<T> handler)
			: this()
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this._handler = handler;
		}

		/// <summary>Raised for each reported progress value.</summary>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000E8F RID: 3727 RVA: 0x0003BA70 File Offset: 0x00039C70
		// (remove) Token: 0x06000E90 RID: 3728 RVA: 0x0003BAA8 File Offset: 0x00039CA8
		public event EventHandler<T> ProgressChanged;

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		// Token: 0x06000E91 RID: 3729 RVA: 0x0003BAE0 File Offset: 0x00039CE0
		protected virtual void OnReport(T value)
		{
			bool handler = this._handler != null;
			EventHandler<T> progressChanged = this.ProgressChanged;
			if (handler || progressChanged != null)
			{
				this._synchronizationContext.Post(this._invokeHandlers, value);
			}
		}

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		// Token: 0x06000E92 RID: 3730 RVA: 0x0003BB16 File Offset: 0x00039D16
		void IProgress<T>.Report(T value)
		{
			this.OnReport(value);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0003BB20 File Offset: 0x00039D20
		private void InvokeHandlers(object state)
		{
			T t = (T)((object)state);
			Action<T> handler = this._handler;
			EventHandler<T> progressChanged = this.ProgressChanged;
			if (handler != null)
			{
				handler(t);
			}
			if (progressChanged != null)
			{
				progressChanged(this, t);
			}
		}

		// Token: 0x040012BF RID: 4799
		private readonly SynchronizationContext _synchronizationContext;

		// Token: 0x040012C0 RID: 4800
		private readonly Action<T> _handler;

		// Token: 0x040012C1 RID: 4801
		private readonly SendOrPostCallback _invokeHandlers;
	}
}
