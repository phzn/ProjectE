using System;

namespace System.Threading
{
	/// <summary>Represents pre-allocated state for native overlapped I/O operations.</summary>
	// Token: 0x020002B9 RID: 697
	public sealed class PreAllocatedOverlapped : IDisposable, IDeferredDisposable
	{
		// Token: 0x06001E70 RID: 7792 RVA: 0x000175B9 File Offset: 0x000157B9
		static PreAllocatedOverlapped()
		{
			if (!Environment.IsRunningOnWindows)
			{
				throw new PlatformNotSupportedException();
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.PreAllocatedOverlapped" /> class and specifies a delegate to invoke when each asynchronous I/O operation is complete, a user-provided object that provides context, and managed objects that serve as buffers.</summary>
		/// <param name="callback">A delegate that represents the callback method to invoke when each asynchronous I/O operation completes.</param>
		/// <param name="state">A user-supplied object that distinguishes the <see cref="T:System.Threading.NativeOverlapped" /> instance produced from this object from other <see cref="T:System.Threading.NativeOverlapped" /> instances. Its value can be <see langword="null" />.</param>
		/// <param name="pinData">An object or array of objects that represent the input or output buffer for the operations. Each object represents a buffer, such as an array of bytes. Its value can be <see langword="null" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callback" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">This method was called after the <see cref="T:System.Threading.ThreadPoolBoundHandle" /> was disposed.</exception>
		// Token: 0x06001E71 RID: 7793 RVA: 0x00070CCF File Offset: 0x0006EECF
		[CLSCompliant(false)]
		public PreAllocatedOverlapped(IOCompletionCallback callback, object state, object pinData)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this._overlapped = Win32ThreadPoolNativeOverlapped.Allocate(callback, state, pinData, this);
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x00070CF4 File Offset: 0x0006EEF4
		internal bool AddRef()
		{
			return this._lifetime.AddRef(this);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x00070D02 File Offset: 0x0006EF02
		internal void Release()
		{
			this._lifetime.Release(this);
		}

		/// <summary>Frees the resources associated with this <see cref="T:System.Threading.PreAllocatedOverlapped" /> instance.</summary>
		// Token: 0x06001E74 RID: 7796 RVA: 0x00070D10 File Offset: 0x0006EF10
		public void Dispose()
		{
			this._lifetime.Dispose(this);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x00070D24 File Offset: 0x0006EF24
		~PreAllocatedOverlapped()
		{
			if (!Environment.HasShutdownStarted)
			{
				this.Dispose();
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x00070D58 File Offset: 0x0006EF58
		unsafe void IDeferredDisposable.OnFinalRelease(bool disposed)
		{
			if (this._overlapped != null)
			{
				if (disposed)
				{
					Win32ThreadPoolNativeOverlapped.Free(this._overlapped);
					return;
				}
				*Win32ThreadPoolNativeOverlapped.ToNativeOverlapped(this._overlapped) = default(NativeOverlapped);
			}
		}

		// Token: 0x04001AB6 RID: 6838
		internal unsafe readonly Win32ThreadPoolNativeOverlapped* _overlapped;

		// Token: 0x04001AB7 RID: 6839
		private DeferredDisposableLifetime<PreAllocatedOverlapped> _lifetime;
	}
}
