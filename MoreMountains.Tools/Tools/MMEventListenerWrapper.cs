using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000DF RID: 223
	public class MMEventListenerWrapper<TOwner, TTarget, TEvent> : MMEventListener<TEvent>, MMEventListenerBase, IDisposable where TEvent : struct
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x00016682 File Offset: 0x00014882
		public MMEventListenerWrapper(TOwner owner, Action<TTarget> callback)
		{
			this._owner = owner;
			this._callback = callback;
			this.RegisterCallbacks(true);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001669F File Offset: 0x0001489F
		public void Dispose()
		{
			this.RegisterCallbacks(false);
			this._callback = null;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000166B0 File Offset: 0x000148B0
		protected virtual TTarget OnEvent(TEvent eventType)
		{
			return default(TTarget);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000166C8 File Offset: 0x000148C8
		public void OnMMEvent(TEvent eventType)
		{
			TTarget ttarget = this.OnEvent(eventType);
			Action<TTarget> callback = this._callback;
			if (callback == null)
			{
				return;
			}
			callback(ttarget);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000166EE File Offset: 0x000148EE
		private void RegisterCallbacks(bool b)
		{
			if (b)
			{
				this.MMEventStartListening<TEvent>();
				return;
			}
			this.MMEventStopListening<TEvent>();
		}

		// Token: 0x04000636 RID: 1590
		private Action<TTarget> _callback;

		// Token: 0x04000637 RID: 1591
		private TOwner _owner;
	}
}
