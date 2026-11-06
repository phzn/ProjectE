using System;
using System.Runtime.InteropServices;
using Unity;

namespace System.Threading
{
	/// <summary>Represents a handle that has been registered when calling <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" />. This class cannot be inherited.</summary>
	// Token: 0x020002F7 RID: 759
	[ComVisible(true)]
	public sealed class RegisteredWaitHandle : MarshalByRefObject
	{
		// Token: 0x0600211F RID: 8479 RVA: 0x00077914 File Offset: 0x00075B14
		internal RegisteredWaitHandle(WaitHandle waitObject, WaitOrTimerCallback callback, object state, TimeSpan timeout, bool executeOnlyOnce)
		{
			this._waitObject = waitObject;
			this._callback = callback;
			this._state = state;
			this._timeout = timeout;
			this._executeOnlyOnce = executeOnlyOnce;
			this._finalEvent = null;
			this._cancelEvent = new ManualResetEvent(false);
			this._callsInProcess = 0;
			this._unregistered = false;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x00077970 File Offset: 0x00075B70
		internal void Wait(object state)
		{
			bool flag = false;
			try
			{
				this._waitObject.SafeWaitHandle.DangerousAddRef(ref flag);
				RegisteredWaitHandle registeredWaitHandle;
				try
				{
					WaitHandle[] array = new WaitHandle[] { this._waitObject, this._cancelEvent };
					do
					{
						int num = WaitHandle.WaitAny(array, this._timeout, false);
						if (!this._unregistered)
						{
							registeredWaitHandle = this;
							lock (registeredWaitHandle)
							{
								this._callsInProcess++;
							}
							ThreadPool.QueueUserWorkItem(new WaitCallback(this.DoCallBack), num == 258);
						}
					}
					while (!this._unregistered && !this._executeOnlyOnce);
				}
				catch
				{
				}
				registeredWaitHandle = this;
				lock (registeredWaitHandle)
				{
					this._unregistered = true;
					if (this._callsInProcess == 0 && this._finalEvent != null)
					{
						NativeEventCalls.SetEvent(this._finalEvent.SafeWaitHandle);
						this._finalEvent = null;
					}
				}
			}
			catch (ObjectDisposedException)
			{
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					this._waitObject.SafeWaitHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00077AC4 File Offset: 0x00075CC4
		private void DoCallBack(object timedOut)
		{
			try
			{
				if (this._callback != null)
				{
					this._callback(this._state, (bool)timedOut);
				}
			}
			finally
			{
				lock (this)
				{
					this._callsInProcess--;
					if (this._unregistered && this._callsInProcess == 0 && this._finalEvent != null)
					{
						NativeEventCalls.SetEvent(this._finalEvent.SafeWaitHandle);
						this._finalEvent = null;
					}
				}
			}
		}

		/// <summary>Cancels a registered wait operation issued by the <see cref="M:System.Threading.ThreadPool.RegisterWaitForSingleObject(System.Threading.WaitHandle,System.Threading.WaitOrTimerCallback,System.Object,System.UInt32,System.Boolean)" /> method.</summary>
		/// <param name="waitObject">The <see cref="T:System.Threading.WaitHandle" /> to be signaled.</param>
		/// <returns>
		///   <see langword="true" /> if the function succeeds; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002122 RID: 8482 RVA: 0x00077B68 File Offset: 0x00075D68
		[ComVisible(true)]
		public bool Unregister(WaitHandle waitObject)
		{
			bool flag2;
			lock (this)
			{
				if (this._unregistered)
				{
					flag2 = false;
				}
				else
				{
					this._finalEvent = waitObject;
					this._unregistered = true;
					this._cancelEvent.Set();
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000173AD File Offset: 0x000155AD
		internal RegisteredWaitHandle()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001B7D RID: 7037
		private WaitHandle _waitObject;

		// Token: 0x04001B7E RID: 7038
		private WaitOrTimerCallback _callback;

		// Token: 0x04001B7F RID: 7039
		private object _state;

		// Token: 0x04001B80 RID: 7040
		private WaitHandle _finalEvent;

		// Token: 0x04001B81 RID: 7041
		private ManualResetEvent _cancelEvent;

		// Token: 0x04001B82 RID: 7042
		private TimeSpan _timeout;

		// Token: 0x04001B83 RID: 7043
		private int _callsInProcess;

		// Token: 0x04001B84 RID: 7044
		private bool _executeOnlyOnce;

		// Token: 0x04001B85 RID: 7045
		private bool _unregistered;
	}
}
