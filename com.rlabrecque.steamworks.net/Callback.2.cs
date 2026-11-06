using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000174 RID: 372
	public sealed class Callback<T> : Callback, IDisposable
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060008A4 RID: 2212 RVA: 0x0000CCCC File Offset: 0x0000AECC
		// (remove) Token: 0x060008A5 RID: 2213 RVA: 0x0000CD04 File Offset: 0x0000AF04
		private event Callback<T>.DispatchDelegate m_Func;

		// Token: 0x060008A6 RID: 2214 RVA: 0x0000CD39 File Offset: 0x0000AF39
		public static Callback<T> Create(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, false);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0000CD42 File Offset: 0x0000AF42
		public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, true);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0000CD4B File Offset: 0x0000AF4B
		public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
		{
			this.m_bGameServer = bGameServer;
			this.Register(func);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0000CD64 File Offset: 0x0000AF64
		~Callback()
		{
			this.Dispose();
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0000CD90 File Offset: 0x0000AF90
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this.m_bIsRegistered)
			{
				this.Unregister();
			}
			this.m_bDisposed = true;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0000CDB6 File Offset: 0x0000AFB6
		public void Register(Callback<T>.DispatchDelegate func)
		{
			if (func == null)
			{
				throw new Exception("Callback function must not be null.");
			}
			if (this.m_bIsRegistered)
			{
				this.Unregister();
			}
			this.m_Func = func;
			CallbackDispatcher.Register(this);
			this.m_bIsRegistered = true;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0000CDE8 File Offset: 0x0000AFE8
		public void Unregister()
		{
			CallbackDispatcher.Unregister(this);
			this.m_bIsRegistered = false;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x0000CDF7 File Offset: 0x0000AFF7
		public override bool IsGameServer
		{
			get
			{
				return this.m_bGameServer;
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0000CDFF File Offset: 0x0000AFFF
		internal override Type GetCallbackType()
		{
			return typeof(T);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0000CE0C File Offset: 0x0000B00C
		internal override void OnRunCallback(IntPtr pvParam)
		{
			try
			{
				this.m_Func(Marshal.PtrToStructure<T>(pvParam));
			}
			catch (Exception ex)
			{
				CallbackDispatcher.ExceptionHandler(ex);
			}
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0000CE4C File Offset: 0x0000B04C
		internal override void SetUnregistered()
		{
			this.m_bIsRegistered = false;
		}

		// Token: 0x04000B09 RID: 2825
		private bool m_bGameServer;

		// Token: 0x04000B0A RID: 2826
		private bool m_bIsRegistered;

		// Token: 0x04000B0B RID: 2827
		private bool m_bDisposed;

		// Token: 0x020001C9 RID: 457
		// (Invoke) Token: 0x06000B96 RID: 2966
		public delegate void DispatchDelegate(T param);
	}
}
