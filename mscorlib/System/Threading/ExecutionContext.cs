using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
	/// <summary>Manages the execution context for the current thread. This class cannot be inherited.</summary>
	// Token: 0x020002CB RID: 715
	[Serializable]
	public sealed class ExecutionContext : IDisposable, ISerializable
	{
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x00072D23 File Offset: 0x00070F23
		// (set) Token: 0x06001EF1 RID: 7921 RVA: 0x00072D30 File Offset: 0x00070F30
		internal bool isNewCapture
		{
			get
			{
				return (this._flags & (ExecutionContext.Flags)5) > ExecutionContext.Flags.None;
			}
			set
			{
				if (value)
				{
					this._flags |= ExecutionContext.Flags.IsNewCapture;
					return;
				}
				this._flags &= (ExecutionContext.Flags)(-2);
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x00072D53 File Offset: 0x00070F53
		// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x00072D60 File Offset: 0x00070F60
		internal bool isFlowSuppressed
		{
			get
			{
				return (this._flags & ExecutionContext.Flags.IsFlowSuppressed) > ExecutionContext.Flags.None;
			}
			set
			{
				if (value)
				{
					this._flags |= ExecutionContext.Flags.IsFlowSuppressed;
					return;
				}
				this._flags &= (ExecutionContext.Flags)(-3);
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001EF4 RID: 7924 RVA: 0x00072D83 File Offset: 0x00070F83
		internal static ExecutionContext PreAllocatedDefault
		{
			[SecuritySafeCritical]
			get
			{
				return ExecutionContext.s_dummyDefaultEC;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001EF5 RID: 7925 RVA: 0x00072D8A File Offset: 0x00070F8A
		internal bool IsPreAllocatedDefault
		{
			get
			{
				return (this._flags & ExecutionContext.Flags.IsPreAllocatedDefault) != ExecutionContext.Flags.None;
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0000259F File Offset: 0x0000079F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext()
		{
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x00072D99 File Offset: 0x00070F99
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal ExecutionContext(bool isPreAllocatedDefault)
		{
			if (isPreAllocatedDefault)
			{
				this._flags = ExecutionContext.Flags.IsPreAllocatedDefault;
			}
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00072DAC File Offset: 0x00070FAC
		[SecurityCritical]
		internal static object GetLocalValue(IAsyncLocal local)
		{
			return Thread.CurrentThread.GetExecutionContextReader().GetLocalValue(local);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00072DCC File Offset: 0x00070FCC
		[SecurityCritical]
		internal static void SetLocalValue(IAsyncLocal local, object newValue, bool needChangeNotifications)
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			object obj = null;
			bool flag = mutableExecutionContext._localValues != null && mutableExecutionContext._localValues.TryGetValue(local, out obj);
			if (obj == newValue)
			{
				return;
			}
			if (mutableExecutionContext._localValues == null)
			{
				mutableExecutionContext._localValues = new Dictionary<IAsyncLocal, object>();
			}
			else
			{
				mutableExecutionContext._localValues = new Dictionary<IAsyncLocal, object>(mutableExecutionContext._localValues);
			}
			mutableExecutionContext._localValues[local] = newValue;
			if (needChangeNotifications)
			{
				if (!flag)
				{
					if (mutableExecutionContext._localChangeNotifications == null)
					{
						mutableExecutionContext._localChangeNotifications = new List<IAsyncLocal>();
					}
					else
					{
						mutableExecutionContext._localChangeNotifications = new List<IAsyncLocal>(mutableExecutionContext._localChangeNotifications);
					}
					mutableExecutionContext._localChangeNotifications.Add(local);
				}
				local.OnValueChanged(obj, newValue, false);
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00072E7C File Offset: 0x0007107C
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		internal static void OnAsyncLocalContextChanged(ExecutionContext previous, ExecutionContext current)
		{
			List<IAsyncLocal> list = ((previous == null) ? null : previous._localChangeNotifications);
			if (list != null)
			{
				foreach (IAsyncLocal asyncLocal in list)
				{
					object obj = null;
					if (previous != null && previous._localValues != null)
					{
						previous._localValues.TryGetValue(asyncLocal, out obj);
					}
					object obj2 = null;
					if (current != null && current._localValues != null)
					{
						current._localValues.TryGetValue(asyncLocal, out obj2);
					}
					if (obj != obj2)
					{
						asyncLocal.OnValueChanged(obj, obj2, true);
					}
				}
			}
			List<IAsyncLocal> list2 = ((current == null) ? null : current._localChangeNotifications);
			if (list2 != null && list2 != list)
			{
				try
				{
					foreach (IAsyncLocal asyncLocal2 in list2)
					{
						object obj3 = null;
						if (previous == null || previous._localValues == null || !previous._localValues.TryGetValue(asyncLocal2, out obj3))
						{
							object obj4 = null;
							if (current != null && current._localValues != null)
							{
								current._localValues.TryGetValue(asyncLocal2, out obj4);
							}
							if (obj3 != obj4)
							{
								asyncLocal2.OnValueChanged(obj3, obj4, true);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Environment.FailFast(Environment.GetResourceString("An exception was not handled in an AsyncLocal<T> notification callback."), ex);
				}
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001EFB RID: 7931 RVA: 0x00072FE4 File Offset: 0x000711E4
		// (set) Token: 0x06001EFC RID: 7932 RVA: 0x00072FFF File Offset: 0x000711FF
		internal LogicalCallContext LogicalCallContext
		{
			[SecurityCritical]
			get
			{
				if (this._logicalCallContext == null)
				{
					this._logicalCallContext = new LogicalCallContext();
				}
				return this._logicalCallContext;
			}
			[SecurityCritical]
			set
			{
				this._logicalCallContext = value;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001EFD RID: 7933 RVA: 0x00073008 File Offset: 0x00071208
		// (set) Token: 0x06001EFE RID: 7934 RVA: 0x00073023 File Offset: 0x00071223
		internal IllogicalCallContext IllogicalCallContext
		{
			get
			{
				if (this._illogicalCallContext == null)
				{
					this._illogicalCallContext = new IllogicalCallContext();
				}
				return this._illogicalCallContext;
			}
			set
			{
				this._illogicalCallContext = value;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001EFF RID: 7935 RVA: 0x0007302C File Offset: 0x0007122C
		// (set) Token: 0x06001F00 RID: 7936 RVA: 0x00073034 File Offset: 0x00071234
		internal SynchronizationContext SynchronizationContext
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._syncContext;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._syncContext = value;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001F01 RID: 7937 RVA: 0x0007303D File Offset: 0x0007123D
		// (set) Token: 0x06001F02 RID: 7938 RVA: 0x00073045 File Offset: 0x00071245
		internal SynchronizationContext SynchronizationContextNoFlow
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._syncContextNoFlow;
			}
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			set
			{
				this._syncContextNoFlow = value;
			}
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ExecutionContext" /> class.</summary>
		// Token: 0x06001F03 RID: 7939 RVA: 0x0007304E File Offset: 0x0007124E
		public void Dispose()
		{
			bool isPreAllocatedDefault = this.IsPreAllocatedDefault;
		}

		/// <summary>Runs a method in a specified execution context on the current thread.</summary>
		/// <param name="executionContext">The <see cref="T:System.Threading.ExecutionContext" /> to set.</param>
		/// <param name="callback">A <see cref="T:System.Threading.ContextCallback" /> delegate that represents the method to be run in the provided execution context.</param>
		/// <param name="state">The object to pass to the callback method.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="executionContext" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="executionContext" /> was not acquired through a capture operation.  
		/// -or-  
		/// <paramref name="executionContext" /> has already been used as the argument to a <see cref="M:System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,System.Threading.ContextCallback,System.Object)" /> call.</exception>
		// Token: 0x06001F04 RID: 7940 RVA: 0x00073057 File Offset: 0x00071257
		[SecurityCritical]
		public static void Run(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (executionContext == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cannot call Set on a null context"));
			}
			if (!executionContext.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cannot apply a context that has been marshaled across AppDomains, that was not acquired through a Capture operation or that has already been the argument to a Set call."));
			}
			ExecutionContext.Run(executionContext, callback, state, false);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0007308D File Offset: 0x0007128D
		[SecurityCritical]
		[FriendAccessAllowed]
		internal static void Run(ExecutionContext executionContext, ContextCallback callback, object state, bool preserveSyncCtx)
		{
			ExecutionContext.RunInternal(executionContext, callback, state, preserveSyncCtx);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x00073098 File Offset: 0x00071298
		internal static void RunInternal(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			ExecutionContext.RunInternal(executionContext, callback, state, false);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x000730A4 File Offset: 0x000712A4
		[SecurityCritical]
		[HandleProcessCorruptedStateExceptions]
		internal static void RunInternal(ExecutionContext executionContext, ContextCallback callback, object state, bool preserveSyncCtx)
		{
			if (!executionContext.IsPreAllocatedDefault)
			{
				executionContext.isNewCapture = false;
			}
			Thread currentThread = Thread.CurrentThread;
			ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				ExecutionContext.Reader executionContextReader = currentThread.GetExecutionContextReader();
				if ((executionContextReader.IsNull || executionContextReader.IsDefaultFTContext(preserveSyncCtx)) && executionContext.IsDefaultFTContext(preserveSyncCtx) && executionContextReader.HasSameLocalValues(executionContext))
				{
					ExecutionContext.EstablishCopyOnWriteScope(currentThread, true, ref executionContextSwitcher);
				}
				else
				{
					if (executionContext.IsPreAllocatedDefault)
					{
						executionContext = new ExecutionContext();
					}
					executionContextSwitcher = ExecutionContext.SetExecutionContext(executionContext, preserveSyncCtx);
				}
				callback(state);
			}
			finally
			{
				executionContextSwitcher.Undo();
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00073144 File Offset: 0x00071344
		internal static void RunInternal<TState>(ExecutionContext executionContext, ContextCallback<TState> callback, ref TState state)
		{
			ExecutionContext.RunInternal<TState>(executionContext, callback, ref state, false);
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00073150 File Offset: 0x00071350
		[SecurityCritical]
		[HandleProcessCorruptedStateExceptions]
		internal static void RunInternal<TState>(ExecutionContext executionContext, ContextCallback<TState> callback, ref TState state, bool preserveSyncCtx)
		{
			if (!executionContext.IsPreAllocatedDefault)
			{
				executionContext.isNewCapture = false;
			}
			Thread currentThread = Thread.CurrentThread;
			ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				ExecutionContext.Reader executionContextReader = currentThread.GetExecutionContextReader();
				if ((executionContextReader.IsNull || executionContextReader.IsDefaultFTContext(preserveSyncCtx)) && executionContext.IsDefaultFTContext(preserveSyncCtx) && executionContextReader.HasSameLocalValues(executionContext))
				{
					ExecutionContext.EstablishCopyOnWriteScope(currentThread, true, ref executionContextSwitcher);
				}
				else
				{
					if (executionContext.IsPreAllocatedDefault)
					{
						executionContext = new ExecutionContext();
					}
					executionContextSwitcher = ExecutionContext.SetExecutionContext(executionContext, preserveSyncCtx);
				}
				callback(ref state);
			}
			finally
			{
				executionContextSwitcher.Undo();
			}
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x000731F0 File Offset: 0x000713F0
		[SecurityCritical]
		internal static void EstablishCopyOnWriteScope(ref ExecutionContextSwitcher ecsw)
		{
			ExecutionContext.EstablishCopyOnWriteScope(Thread.CurrentThread, false, ref ecsw);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000731FE File Offset: 0x000713FE
		[SecurityCritical]
		private static void EstablishCopyOnWriteScope(Thread currentThread, bool knownNullWindowsIdentity, ref ExecutionContextSwitcher ecsw)
		{
			ecsw.outerEC = currentThread.GetExecutionContextReader();
			ecsw.outerECBelongsToScope = currentThread.ExecutionContextBelongsToCurrentScope;
			currentThread.ExecutionContextBelongsToCurrentScope = false;
			ecsw.thread = currentThread;
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00073228 File Offset: 0x00071428
		[SecurityCritical]
		[HandleProcessCorruptedStateExceptions]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static ExecutionContextSwitcher SetExecutionContext(ExecutionContext executionContext, bool preserveSyncCtx)
		{
			ExecutionContextSwitcher executionContextSwitcher = default(ExecutionContextSwitcher);
			Thread currentThread = Thread.CurrentThread;
			ExecutionContext.Reader executionContextReader = currentThread.GetExecutionContextReader();
			executionContextSwitcher.thread = currentThread;
			executionContextSwitcher.outerEC = executionContextReader;
			executionContextSwitcher.outerECBelongsToScope = currentThread.ExecutionContextBelongsToCurrentScope;
			if (preserveSyncCtx)
			{
				executionContext.SynchronizationContext = executionContextReader.SynchronizationContext;
			}
			executionContext.SynchronizationContextNoFlow = executionContextReader.SynchronizationContextNoFlow;
			currentThread.SetExecutionContext(executionContext, true);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				ExecutionContext.OnAsyncLocalContextChanged(executionContextReader.DangerousGetRawExecutionContext(), executionContext);
			}
			catch
			{
				executionContextSwitcher.UndoNoThrow();
				throw;
			}
			return executionContextSwitcher;
		}

		/// <summary>Creates a copy of the current execution context.</summary>
		/// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the current execution context.</returns>
		/// <exception cref="T:System.InvalidOperationException">This context cannot be copied because it is used. Only newly captured contexts can be copied.</exception>
		// Token: 0x06001F0D RID: 7949 RVA: 0x000732BC File Offset: 0x000714BC
		[SecuritySafeCritical]
		public ExecutionContext CreateCopy()
		{
			if (!this.isNewCapture)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Only newly captured contexts can be copied"));
			}
			ExecutionContext executionContext = new ExecutionContext();
			executionContext.isNewCapture = true;
			executionContext._syncContext = ((this._syncContext == null) ? null : this._syncContext.CreateCopy());
			executionContext._localValues = this._localValues;
			executionContext._localChangeNotifications = this._localChangeNotifications;
			if (this._logicalCallContext != null)
			{
				executionContext.LogicalCallContext = (LogicalCallContext)this.LogicalCallContext.Clone();
			}
			return executionContext;
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00073344 File Offset: 0x00071544
		[SecuritySafeCritical]
		internal ExecutionContext CreateMutableCopy()
		{
			ExecutionContext executionContext = new ExecutionContext();
			executionContext._syncContext = this._syncContext;
			executionContext._syncContextNoFlow = this._syncContextNoFlow;
			if (this._logicalCallContext != null)
			{
				executionContext.LogicalCallContext = (LogicalCallContext)this.LogicalCallContext.Clone();
			}
			if (this._illogicalCallContext != null)
			{
				executionContext.IllogicalCallContext = this.IllogicalCallContext.CreateCopy();
			}
			executionContext._localValues = this._localValues;
			executionContext._localChangeNotifications = this._localChangeNotifications;
			executionContext.isFlowSuppressed = this.isFlowSuppressed;
			return executionContext;
		}

		/// <summary>Suppresses the flow of the execution context across asynchronous threads.</summary>
		/// <returns>An <see cref="T:System.Threading.AsyncFlowControl" /> structure for restoring the flow.</returns>
		/// <exception cref="T:System.InvalidOperationException">The context flow is already suppressed.</exception>
		// Token: 0x06001F0F RID: 7951 RVA: 0x000733CC File Offset: 0x000715CC
		[SecurityCritical]
		public static AsyncFlowControl SuppressFlow()
		{
			if (ExecutionContext.IsFlowSuppressed())
			{
				throw new InvalidOperationException(Environment.GetResourceString("Context flow is already suppressed."));
			}
			AsyncFlowControl asyncFlowControl = default(AsyncFlowControl);
			asyncFlowControl.Setup();
			return asyncFlowControl;
		}

		/// <summary>Restores the flow of the execution context across asynchronous threads.</summary>
		/// <exception cref="T:System.InvalidOperationException">The context flow cannot be restored because it is not being suppressed.</exception>
		// Token: 0x06001F10 RID: 7952 RVA: 0x00073400 File Offset: 0x00071600
		[SecuritySafeCritical]
		public static void RestoreFlow()
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			if (!mutableExecutionContext.isFlowSuppressed)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cannot restore context flow when it is not suppressed."));
			}
			mutableExecutionContext.isFlowSuppressed = false;
		}

		/// <summary>Indicates whether the flow of the execution context is currently suppressed.</summary>
		/// <returns>
		///   <see langword="true" /> if the flow is suppressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F11 RID: 7953 RVA: 0x0007342C File Offset: 0x0007162C
		public static bool IsFlowSuppressed()
		{
			return Thread.CurrentThread.GetExecutionContextReader().IsFlowSuppressed;
		}

		/// <summary>Captures the execution context from the current thread.</summary>
		/// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the execution context for the current thread.</returns>
		// Token: 0x06001F12 RID: 7954 RVA: 0x0007344C File Offset: 0x0007164C
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ExecutionContext Capture()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ExecutionContext.Capture(ref stackCrawlMark, ExecutionContext.CaptureOptions.None);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x00073464 File Offset: 0x00071664
		[FriendAccessAllowed]
		[SecuritySafeCritical]
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static ExecutionContext FastCapture()
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return ExecutionContext.Capture(ref stackCrawlMark, ExecutionContext.CaptureOptions.IgnoreSyncCtx | ExecutionContext.CaptureOptions.OptimizeDefaultCase);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0007347C File Offset: 0x0007167C
		[SecurityCritical]
		internal static ExecutionContext Capture(ref StackCrawlMark stackMark, ExecutionContext.CaptureOptions options)
		{
			ExecutionContext.Reader executionContextReader = Thread.CurrentThread.GetExecutionContextReader();
			if (executionContextReader.IsFlowSuppressed)
			{
				return null;
			}
			SynchronizationContext synchronizationContext = null;
			LogicalCallContext logicalCallContext = null;
			if (!executionContextReader.IsNull)
			{
				if ((options & ExecutionContext.CaptureOptions.IgnoreSyncCtx) == ExecutionContext.CaptureOptions.None)
				{
					synchronizationContext = ((executionContextReader.SynchronizationContext == null) ? null : executionContextReader.SynchronizationContext.CreateCopy());
				}
				if (executionContextReader.LogicalCallContext.HasInfo)
				{
					logicalCallContext = executionContextReader.LogicalCallContext.Clone();
				}
			}
			Dictionary<IAsyncLocal, object> dictionary = null;
			List<IAsyncLocal> list = null;
			if (!executionContextReader.IsNull)
			{
				dictionary = executionContextReader.DangerousGetRawExecutionContext()._localValues;
				list = executionContextReader.DangerousGetRawExecutionContext()._localChangeNotifications;
			}
			if ((options & ExecutionContext.CaptureOptions.OptimizeDefaultCase) != ExecutionContext.CaptureOptions.None && synchronizationContext == null && (logicalCallContext == null || !logicalCallContext.HasInfo) && dictionary == null && list == null)
			{
				return ExecutionContext.s_dummyDefaultEC;
			}
			return new ExecutionContext
			{
				_syncContext = synchronizationContext,
				LogicalCallContext = logicalCallContext,
				_localValues = dictionary,
				_localChangeNotifications = list,
				isNewCapture = true
			};
		}

		/// <summary>Sets the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of the current execution context.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure representing the destination context of the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x06001F15 RID: 7957 RVA: 0x0007355F File Offset: 0x0007175F
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this._logicalCallContext != null)
			{
				info.AddValue("LogicalCallContext", this._logicalCallContext, typeof(LogicalCallContext));
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x00073594 File Offset: 0x00071794
		[SecurityCritical]
		private ExecutionContext(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("LogicalCallContext"))
				{
					this._logicalCallContext = (LogicalCallContext)enumerator.Value;
				}
			}
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000735DB File Offset: 0x000717DB
		[SecurityCritical]
		internal bool IsDefaultFTContext(bool ignoreSyncCtx)
		{
			return (ignoreSyncCtx || this._syncContext == null) && (this._logicalCallContext == null || !this._logicalCallContext.HasInfo) && (this._illogicalCallContext == null || !this._illogicalCallContext.HasUserData);
		}

		// Token: 0x04001AF5 RID: 6901
		private SynchronizationContext _syncContext;

		// Token: 0x04001AF6 RID: 6902
		private SynchronizationContext _syncContextNoFlow;

		// Token: 0x04001AF7 RID: 6903
		[SecurityCritical]
		private LogicalCallContext _logicalCallContext;

		// Token: 0x04001AF8 RID: 6904
		private IllogicalCallContext _illogicalCallContext;

		// Token: 0x04001AF9 RID: 6905
		private ExecutionContext.Flags _flags;

		// Token: 0x04001AFA RID: 6906
		private Dictionary<IAsyncLocal, object> _localValues;

		// Token: 0x04001AFB RID: 6907
		private List<IAsyncLocal> _localChangeNotifications;

		// Token: 0x04001AFC RID: 6908
		private static readonly ExecutionContext s_dummyDefaultEC = new ExecutionContext(true);

		// Token: 0x04001AFD RID: 6909
		internal static readonly ExecutionContext Default = new ExecutionContext();

		// Token: 0x020002CC RID: 716
		private enum Flags
		{
			// Token: 0x04001AFF RID: 6911
			None,
			// Token: 0x04001B00 RID: 6912
			IsNewCapture,
			// Token: 0x04001B01 RID: 6913
			IsFlowSuppressed,
			// Token: 0x04001B02 RID: 6914
			IsPreAllocatedDefault = 4
		}

		// Token: 0x020002CD RID: 717
		internal struct Reader
		{
			// Token: 0x06001F19 RID: 7961 RVA: 0x00073630 File Offset: 0x00071830
			public Reader(ExecutionContext ec)
			{
				this.m_ec = ec;
			}

			// Token: 0x06001F1A RID: 7962 RVA: 0x00073639 File Offset: 0x00071839
			public ExecutionContext DangerousGetRawExecutionContext()
			{
				return this.m_ec;
			}

			// Token: 0x170003B1 RID: 945
			// (get) Token: 0x06001F1B RID: 7963 RVA: 0x00073641 File Offset: 0x00071841
			public bool IsNull
			{
				get
				{
					return this.m_ec == null;
				}
			}

			// Token: 0x06001F1C RID: 7964 RVA: 0x0007364C File Offset: 0x0007184C
			[SecurityCritical]
			public bool IsDefaultFTContext(bool ignoreSyncCtx)
			{
				return this.m_ec.IsDefaultFTContext(ignoreSyncCtx);
			}

			// Token: 0x170003B2 RID: 946
			// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0007365A File Offset: 0x0007185A
			public bool IsFlowSuppressed
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return !this.IsNull && this.m_ec.isFlowSuppressed;
				}
			}

			// Token: 0x06001F1E RID: 7966 RVA: 0x00073671 File Offset: 0x00071871
			public bool IsSame(ExecutionContext.Reader other)
			{
				return this.m_ec == other.m_ec;
			}

			// Token: 0x170003B3 RID: 947
			// (get) Token: 0x06001F1F RID: 7967 RVA: 0x00073681 File Offset: 0x00071881
			public SynchronizationContext SynchronizationContext
			{
				get
				{
					if (!this.IsNull)
					{
						return this.m_ec.SynchronizationContext;
					}
					return null;
				}
			}

			// Token: 0x170003B4 RID: 948
			// (get) Token: 0x06001F20 RID: 7968 RVA: 0x00073698 File Offset: 0x00071898
			public SynchronizationContext SynchronizationContextNoFlow
			{
				get
				{
					if (!this.IsNull)
					{
						return this.m_ec.SynchronizationContextNoFlow;
					}
					return null;
				}
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x06001F21 RID: 7969 RVA: 0x000736AF File Offset: 0x000718AF
			public LogicalCallContext.Reader LogicalCallContext
			{
				[SecurityCritical]
				get
				{
					return new LogicalCallContext.Reader(this.IsNull ? null : this.m_ec.LogicalCallContext);
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x06001F22 RID: 7970 RVA: 0x000736CC File Offset: 0x000718CC
			public IllogicalCallContext.Reader IllogicalCallContext
			{
				[SecurityCritical]
				get
				{
					return new IllogicalCallContext.Reader(this.IsNull ? null : this.m_ec.IllogicalCallContext);
				}
			}

			// Token: 0x06001F23 RID: 7971 RVA: 0x000736EC File Offset: 0x000718EC
			[SecurityCritical]
			public object GetLocalValue(IAsyncLocal local)
			{
				if (this.IsNull)
				{
					return null;
				}
				if (this.m_ec._localValues == null)
				{
					return null;
				}
				object obj;
				this.m_ec._localValues.TryGetValue(local, out obj);
				return obj;
			}

			// Token: 0x06001F24 RID: 7972 RVA: 0x00073728 File Offset: 0x00071928
			[SecurityCritical]
			public bool HasSameLocalValues(ExecutionContext other)
			{
				Dictionary<IAsyncLocal, object> dictionary = (this.IsNull ? null : this.m_ec._localValues);
				Dictionary<IAsyncLocal, object> dictionary2 = ((other == null) ? null : other._localValues);
				return dictionary == dictionary2;
			}

			// Token: 0x06001F25 RID: 7973 RVA: 0x0007375B File Offset: 0x0007195B
			[SecurityCritical]
			public bool HasLocalValues()
			{
				return !this.IsNull && this.m_ec._localValues != null;
			}

			// Token: 0x04001B03 RID: 6915
			private ExecutionContext m_ec;
		}

		// Token: 0x020002CE RID: 718
		[Flags]
		internal enum CaptureOptions
		{
			// Token: 0x04001B05 RID: 6917
			None = 0,
			// Token: 0x04001B06 RID: 6918
			IgnoreSyncCtx = 1,
			// Token: 0x04001B07 RID: 6919
			OptimizeDefaultCase = 2
		}
	}
}
