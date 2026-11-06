using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000176 RID: 374
	public sealed class CallResult<T> : CallResult, IDisposable
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060008B5 RID: 2229 RVA: 0x0000CE60 File Offset: 0x0000B060
		// (remove) Token: 0x060008B6 RID: 2230 RVA: 0x0000CE98 File Offset: 0x0000B098
		private event CallResult<T>.APIDispatchDelegate m_Func;

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0000CECD File Offset: 0x0000B0CD
		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0000CED5 File Offset: 0x0000B0D5
		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0000CEDD File Offset: 0x0000B0DD
		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0000CEF8 File Offset: 0x0000B0F8
		~CallResult()
		{
			this.Dispose();
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0000CF24 File Offset: 0x0000B124
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Cancel();
			this.m_bDisposed = true;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0000CF44 File Offset: 0x0000B144
		public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or via Set()");
			}
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				CallbackDispatcher.Unregister(this.m_hAPICall, this);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != SteamAPICall_t.Invalid)
			{
				CallbackDispatcher.Register(hAPICall, this);
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0000CFA7 File Offset: 0x0000B1A7
		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0000CFB9 File Offset: 0x0000B1B9
		public void Cancel()
		{
			if (this.IsActive())
			{
				CallbackDispatcher.Unregister(this.m_hAPICall, this);
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0000CFCF File Offset: 0x0000B1CF
		protected internal override Type GetCallbackType()
		{
			return typeof(T);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0000CFDC File Offset: 0x0000B1DC
		protected internal override void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall_)
		{
			if ((SteamAPICall_t)hSteamAPICall_ == this.m_hAPICall)
			{
				try
				{
					this.m_Func(Marshal.PtrToStructure<T>(pvParam), bFailed);
				}
				catch (Exception ex)
				{
					CallbackDispatcher.ExceptionHandler(ex);
				}
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0000D030 File Offset: 0x0000B230
		protected internal override void SetUnregistered()
		{
			this.m_hAPICall = SteamAPICall_t.Invalid;
		}

		// Token: 0x04000B0D RID: 2829
		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		// Token: 0x04000B0E RID: 2830
		private bool m_bDisposed;

		// Token: 0x020001CA RID: 458
		// (Invoke) Token: 0x06000B9A RID: 2970
		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
