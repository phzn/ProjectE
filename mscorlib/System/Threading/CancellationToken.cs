using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	/// <summary>Propagates notification that operations should be canceled.</summary>
	// Token: 0x0200029E RID: 670
	[DebuggerDisplay("IsCancellationRequested = {IsCancellationRequested}")]
	public readonly struct CancellationToken
	{
		/// <summary>Returns an empty <see cref="T:System.Threading.CancellationToken" /> value.</summary>
		/// <returns>An empty cancellation token.</returns>
		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0006EB58 File Offset: 0x0006CD58
		public static CancellationToken None
		{
			get
			{
				return default(CancellationToken);
			}
		}

		/// <summary>Gets whether cancellation has been requested for this token.</summary>
		/// <returns>
		///   <see langword="true" /> if cancellation has been requested for this token; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x0006EB6E File Offset: 0x0006CD6E
		public bool IsCancellationRequested
		{
			get
			{
				return this._source != null && this._source.IsCancellationRequested;
			}
		}

		/// <summary>Gets whether this token is capable of being in the canceled state.</summary>
		/// <returns>
		///   <see langword="true" /> if this token is capable of being in the canceled state; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001DC3 RID: 7619 RVA: 0x0006EB85 File Offset: 0x0006CD85
		public bool CanBeCanceled
		{
			get
			{
				return this._source != null;
			}
		}

		/// <summary>Gets a <see cref="T:System.Threading.WaitHandle" /> that is signaled when the token is canceled.</summary>
		/// <returns>A <see cref="T:System.Threading.WaitHandle" /> that is signaled when the token is canceled.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x0006EB90 File Offset: 0x0006CD90
		public WaitHandle WaitHandle
		{
			get
			{
				return (this._source ?? CancellationTokenSource.s_neverCanceledSource).WaitHandle;
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006EBA6 File Offset: 0x0006CDA6
		internal CancellationToken(CancellationTokenSource source)
		{
			this._source = source;
		}

		/// <summary>Initializes the <see cref="T:System.Threading.CancellationToken" />.</summary>
		/// <param name="canceled">The canceled state for the token.</param>
		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006EBAF File Offset: 0x0006CDAF
		public CancellationToken(bool canceled)
		{
			this = new CancellationToken(canceled ? CancellationTokenSource.s_canceledSource : null);
		}

		/// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
		/// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
		/// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callback" /> is null.</exception>
		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006EBC2 File Offset: 0x0006CDC2
		public CancellationTokenRegistration Register(Action callback)
		{
			Action<object> action = CancellationToken.s_actionToActionObjShunt;
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return this.Register(action, callback, false, true);
		}

		/// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
		/// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
		/// <param name="useSynchronizationContext">A value that indicates whether to capture the current <see cref="T:System.Threading.SynchronizationContext" /> and use it when invoking the <paramref name="callback" />.</param>
		/// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callback" /> is null.</exception>
		// Token: 0x06001DC8 RID: 7624 RVA: 0x0006EBE1 File Offset: 0x0006CDE1
		public CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext)
		{
			Action<object> action = CancellationToken.s_actionToActionObjShunt;
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			return this.Register(action, callback, useSynchronizationContext, true);
		}

		/// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
		/// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
		/// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked. This may be null.</param>
		/// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callback" /> is null.</exception>
		// Token: 0x06001DC9 RID: 7625 RVA: 0x0006EC00 File Offset: 0x0006CE00
		public CancellationTokenRegistration Register(Action<object> callback, object state)
		{
			return this.Register(callback, state, false, true);
		}

		/// <summary>Registers a delegate that will be called when this <see cref="T:System.Threading.CancellationToken" /> is canceled.</summary>
		/// <param name="callback">The delegate to be executed when the <see cref="T:System.Threading.CancellationToken" /> is canceled.</param>
		/// <param name="state">The state to pass to the <paramref name="callback" /> when the delegate is invoked. This may be null.</param>
		/// <param name="useSynchronizationContext">A Boolean value that indicates whether to capture the current <see cref="T:System.Threading.SynchronizationContext" /> and use it when invoking the <paramref name="callback" />.</param>
		/// <returns>The <see cref="T:System.Threading.CancellationTokenRegistration" /> instance that can be used to unregister the callback.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="callback" /> is null.</exception>
		// Token: 0x06001DCA RID: 7626 RVA: 0x0006EC0C File Offset: 0x0006CE0C
		public CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext)
		{
			return this.Register(callback, state, useSynchronizationContext, true);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x0006EC18 File Offset: 0x0006CE18
		internal CancellationTokenRegistration InternalRegisterWithoutEC(Action<object> callback, object state)
		{
			return this.Register(callback, state, false, false);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0006EC24 File Offset: 0x0006CE24
		[MethodImpl(MethodImplOptions.NoInlining)]
		public CancellationTokenRegistration Register(Action<object> callback, object state, bool useSynchronizationContext, bool useExecutionContext)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			CancellationTokenSource source = this._source;
			if (source == null)
			{
				return default(CancellationTokenRegistration);
			}
			return source.InternalRegister(callback, state, useSynchronizationContext ? SynchronizationContext.Current : null, useExecutionContext ? ExecutionContext.Capture() : null);
		}

		/// <summary>Determines whether the current <see cref="T:System.Threading.CancellationToken" /> instance is equal to the specified token.</summary>
		/// <param name="other">The other <see cref="T:System.Threading.CancellationToken" /> to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if the instances are equal; otherwise, <see langword="false" />. See the Remarks section for more information.</returns>
		// Token: 0x06001DCD RID: 7629 RVA: 0x0006EC72 File Offset: 0x0006CE72
		public bool Equals(CancellationToken other)
		{
			return this._source == other._source;
		}

		/// <summary>Determines whether the current <see cref="T:System.Threading.CancellationToken" /> instance is equal to the specified <see cref="T:System.Object" />.</summary>
		/// <param name="other">The other object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="other" /> is a <see cref="T:System.Threading.CancellationToken" /> and if the two instances are equal; otherwise, <see langword="false" />. See the Remarks section for more information.</returns>
		/// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		// Token: 0x06001DCE RID: 7630 RVA: 0x0006EC82 File Offset: 0x0006CE82
		public override bool Equals(object other)
		{
			return other is CancellationToken && this.Equals((CancellationToken)other);
		}

		/// <summary>Serves as a hash function for a <see cref="T:System.Threading.CancellationToken" />.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Threading.CancellationToken" /> instance.</returns>
		// Token: 0x06001DCF RID: 7631 RVA: 0x0006EC9A File Offset: 0x0006CE9A
		public override int GetHashCode()
		{
			return (this._source ?? CancellationTokenSource.s_neverCanceledSource).GetHashCode();
		}

		/// <summary>Determines whether two <see cref="T:System.Threading.CancellationToken" /> instances are equal.</summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>
		///   <see langword="true" /> if the instances are equal; otherwise, <see langword="false" /> See the Remarks section for more information.</returns>
		/// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		// Token: 0x06001DD0 RID: 7632 RVA: 0x0006ECB0 File Offset: 0x0006CEB0
		public static bool operator ==(CancellationToken left, CancellationToken right)
		{
			return left.Equals(right);
		}

		/// <summary>Determines whether two <see cref="T:System.Threading.CancellationToken" /> instances are not equal.</summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>
		///   <see langword="true" /> if the instances are not equal; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">An associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		// Token: 0x06001DD1 RID: 7633 RVA: 0x0006ECBA File Offset: 0x0006CEBA
		public static bool operator !=(CancellationToken left, CancellationToken right)
		{
			return !left.Equals(right);
		}

		/// <summary>Throws a <see cref="T:System.OperationCanceledException" /> if this token has had cancellation requested.</summary>
		/// <exception cref="T:System.OperationCanceledException">The token has had cancellation requested.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The associated <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		// Token: 0x06001DD2 RID: 7634 RVA: 0x0006ECC7 File Offset: 0x0006CEC7
		public void ThrowIfCancellationRequested()
		{
			if (this.IsCancellationRequested)
			{
				this.ThrowOperationCanceledException();
			}
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x0006ECD7 File Offset: 0x0006CED7
		private void ThrowOperationCanceledException()
		{
			throw new OperationCanceledException("The operation was canceled.", this);
		}

		// Token: 0x04001A56 RID: 6742
		private readonly CancellationTokenSource _source;

		// Token: 0x04001A57 RID: 6743
		private static readonly Action<object> s_actionToActionObjShunt = delegate(object obj)
		{
			((Action)obj)();
		};
	}
}
