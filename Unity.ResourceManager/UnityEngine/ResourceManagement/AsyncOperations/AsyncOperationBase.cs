using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000058 RID: 88
	public abstract class AsyncOperationBase<TObject> : IAsyncOperation
	{
		// Token: 0x06000230 RID: 560
		protected abstract void Execute();

		// Token: 0x06000231 RID: 561 RVA: 0x0000823E File Offset: 0x0000643E
		protected virtual void Destroy()
		{
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00008240 File Offset: 0x00006440
		protected virtual float Progress
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00008247 File Offset: 0x00006447
		protected virtual string DebugName
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000824F File Offset: 0x0000644F
		public virtual void GetDependencies(List<AsyncOperationHandle> dependencies)
		{
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00008251 File Offset: 0x00006451
		// (set) Token: 0x06000236 RID: 566 RVA: 0x00008259 File Offset: 0x00006459
		public TObject Result { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00008262 File Offset: 0x00006462
		internal int Version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000826A File Offset: 0x0000646A
		internal bool CompletedEventHasListeners
		{
			get
			{
				return this.m_CompletedActionT != null && this.m_CompletedActionT.Count > 0;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00008284 File Offset: 0x00006484
		internal bool DestroyedEventHasListeners
		{
			get
			{
				return this.m_DestroyedAction != null && this.m_DestroyedAction.Count > 0;
			}
		}

		// Token: 0x1700008C RID: 140
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000829E File Offset: 0x0000649E
		internal Action<IAsyncOperation> OnDestroy
		{
			set
			{
				this.m_OnDestroyAction = value;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600023B RID: 571 RVA: 0x000082A8 File Offset: 0x000064A8
		// (remove) Token: 0x0600023C RID: 572 RVA: 0x000082E0 File Offset: 0x000064E0
		internal event Action Executed;

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00008315 File Offset: 0x00006515
		protected internal int ReferenceCount
		{
			get
			{
				return this.m_referenceCount;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000831D File Offset: 0x0000651D
		// (set) Token: 0x0600023F RID: 575 RVA: 0x00008325 File Offset: 0x00006525
		public bool IsRunning { get; internal set; }

		// Token: 0x06000240 RID: 576 RVA: 0x0000832E File Offset: 0x0000652E
		protected AsyncOperationBase()
		{
			this.m_UpdateCallback = new Action<float>(this.UpdateCallback);
			this.m_dependencyCompleteAction = delegate(AsyncOperationHandle o)
			{
				this.InvokeExecute();
			};
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008364 File Offset: 0x00006564
		internal static string ShortenPath(string p, bool keepExtension)
		{
			int num = p.LastIndexOf('/');
			if (num > 0)
			{
				p = p.Substring(num + 1);
			}
			if (!keepExtension)
			{
				num = p.LastIndexOf('.');
				if (num > 0)
				{
					p = p.Substring(0, num);
				}
			}
			return p;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000083A4 File Offset: 0x000065A4
		public void WaitForCompletion()
		{
			if (PlatformUtilities.PlatformUsesMultiThreading(Application.platform))
			{
				while (!this.InvokeWaitForCompletion())
				{
				}
				return;
			}
			throw new Exception(string.Format("{0} does not support synchronous Addressable loading.  Please do not use WaitForCompletion on the {1} platform.", Application.platform, Application.platform));
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000083DE File Offset: 0x000065DE
		protected virtual bool InvokeWaitForCompletion()
		{
			return true;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000083E4 File Offset: 0x000065E4
		protected internal void IncrementReferenceCount()
		{
			if (this.m_referenceCount == 0)
			{
				throw new Exception(string.Format("Cannot increment reference count on operation {0} because it has already been destroyed", this));
			}
			this.m_referenceCount++;
			if (this.m_RM != null && this.m_RM.postProfilerEvents)
			{
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationReferenceCount, this.m_referenceCount, null, null));
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000844C File Offset: 0x0000664C
		protected internal void DecrementReferenceCount()
		{
			if (this.m_referenceCount <= 0)
			{
				throw new Exception(string.Format("Cannot decrement reference count for operation {0} because it is already 0", this));
			}
			this.m_referenceCount--;
			if (this.m_RM != null && this.m_RM.postProfilerEvents)
			{
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationReferenceCount, this.m_referenceCount, null, null));
			}
			if (this.m_referenceCount == 0)
			{
				if (this.m_RM != null && this.m_RM.postProfilerEvents)
				{
					this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationDestroy, 0, null, null));
				}
				if (this.m_DestroyedAction != null)
				{
					this.m_DestroyedAction.Invoke(this.Handle);
					this.m_DestroyedAction.Clear();
				}
				this.Destroy();
				this.Result = default(TObject);
				this.m_referenceCount = 1;
				this.m_Status = AsyncOperationStatus.None;
				this.m_taskCompletionSource = null;
				this.m_taskCompletionSourceTypeless = null;
				this.m_Error = null;
				this.m_Version++;
				this.m_RM = null;
				if (this.m_OnDestroyAction != null)
				{
					this.m_OnDestroyAction(this);
					this.m_OnDestroyAction = null;
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00008580 File Offset: 0x00006780
		internal Task<TObject> Task
		{
			get
			{
				if (this.m_taskCompletionSource == null)
				{
					this.m_taskCompletionSource = new TaskCompletionSource<TObject>(TaskCreationOptions.RunContinuationsAsynchronously);
					if (this.IsDone && !this.CompletedEventHasListeners)
					{
						this.m_taskCompletionSource.SetResult(this.Result);
					}
				}
				return this.m_taskCompletionSource.Task;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000247 RID: 583 RVA: 0x000085D0 File Offset: 0x000067D0
		Task<object> IAsyncOperation.Task
		{
			get
			{
				if (this.m_taskCompletionSourceTypeless == null)
				{
					this.m_taskCompletionSourceTypeless = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
					if (this.IsDone && !this.CompletedEventHasListeners)
					{
						this.m_taskCompletionSourceTypeless.SetResult(this.Result);
					}
				}
				return this.m_taskCompletionSourceTypeless.Task;
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008624 File Offset: 0x00006824
		public override string ToString()
		{
			string text = "";
			Object @object = this.Result as Object;
			if (@object != null)
			{
				text = "(" + @object.GetInstanceID().ToString() + ")";
			}
			string text2 = "{0}, result='{1}', status='{2}'";
			object obj = base.ToString();
			Object object2 = @object;
			return string.Format(text2, obj, ((object2 != null) ? object2.ToString() : null) + text, this.m_Status);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000869D File Offset: 0x0000689D
		private void RegisterForDeferredCallbackEvent(bool incrementReferenceCount = true)
		{
			if (this.IsDone && !this.m_InDeferredCallbackQueue)
			{
				this.m_InDeferredCallbackQueue = true;
				ResourceManager rm = this.m_RM;
				if (rm == null)
				{
					return;
				}
				rm.RegisterForDeferredCallback(this, incrementReferenceCount);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600024A RID: 586 RVA: 0x000086C8 File Offset: 0x000068C8
		// (remove) Token: 0x0600024B RID: 587 RVA: 0x000086F0 File Offset: 0x000068F0
		internal event Action<AsyncOperationHandle<TObject>> Completed
		{
			add
			{
				if (this.m_CompletedActionT == null)
				{
					this.m_CompletedActionT = DelegateList<AsyncOperationHandle<TObject>>.CreateWithGlobalCache();
				}
				this.m_CompletedActionT.Add(value);
				this.RegisterForDeferredCallbackEvent(true);
			}
			remove
			{
				DelegateList<AsyncOperationHandle<TObject>> completedActionT = this.m_CompletedActionT;
				if (completedActionT == null)
				{
					return;
				}
				completedActionT.Remove(value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600024C RID: 588 RVA: 0x00008703 File Offset: 0x00006903
		// (remove) Token: 0x0600024D RID: 589 RVA: 0x00008724 File Offset: 0x00006924
		internal event Action<AsyncOperationHandle> Destroyed
		{
			add
			{
				if (this.m_DestroyedAction == null)
				{
					this.m_DestroyedAction = DelegateList<AsyncOperationHandle>.CreateWithGlobalCache();
				}
				this.m_DestroyedAction.Add(value);
			}
			remove
			{
				DelegateList<AsyncOperationHandle> destroyedAction = this.m_DestroyedAction;
				if (destroyedAction == null)
				{
					return;
				}
				destroyedAction.Remove(value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600024E RID: 590 RVA: 0x00008738 File Offset: 0x00006938
		// (remove) Token: 0x0600024F RID: 591 RVA: 0x00008764 File Offset: 0x00006964
		internal event Action<AsyncOperationHandle> CompletedTypeless
		{
			add
			{
				this.Completed += delegate(AsyncOperationHandle<TObject> s)
				{
					value(s);
				};
			}
			remove
			{
				this.Completed -= delegate(AsyncOperationHandle<TObject> s)
				{
					value(s);
				};
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00008790 File Offset: 0x00006990
		internal AsyncOperationStatus Status
		{
			get
			{
				return this.m_Status;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00008798 File Offset: 0x00006998
		// (set) Token: 0x06000252 RID: 594 RVA: 0x000087A0 File Offset: 0x000069A0
		internal Exception OperationException
		{
			get
			{
				return this.m_Error;
			}
			private set
			{
				this.m_Error = value;
				if (this.m_Error != null && ResourceManager.ExceptionHandler != null)
				{
					ResourceManager.ExceptionHandler(new AsyncOperationHandle(this), value);
				}
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x000087C9 File Offset: 0x000069C9
		internal bool MoveNext()
		{
			return !this.IsDone;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x000087D4 File Offset: 0x000069D4
		internal void Reset()
		{
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000255 RID: 597 RVA: 0x000087D6 File Offset: 0x000069D6
		internal object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000256 RID: 598 RVA: 0x000087D9 File Offset: 0x000069D9
		internal bool IsDone
		{
			get
			{
				return this.Status == AsyncOperationStatus.Failed || this.Status == AsyncOperationStatus.Succeeded;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000257 RID: 599 RVA: 0x000087F0 File Offset: 0x000069F0
		internal float PercentComplete
		{
			get
			{
				if (this.m_Status == AsyncOperationStatus.None)
				{
					try
					{
						return this.Progress;
					}
					catch
					{
						return 0f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00008830 File Offset: 0x00006A30
		internal void InvokeCompletionEvent()
		{
			if (this.m_CompletedActionT != null)
			{
				this.m_CompletedActionT.Invoke(this.Handle);
				this.m_CompletedActionT.Clear();
			}
			if (this.m_taskCompletionSource != null)
			{
				this.m_taskCompletionSource.TrySetResult(this.Result);
			}
			if (this.m_taskCompletionSourceTypeless != null)
			{
				this.m_taskCompletionSourceTypeless.TrySetResult(this.Result);
			}
			this.m_InDeferredCallbackQueue = false;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000259 RID: 601 RVA: 0x000088A1 File Offset: 0x00006AA1
		internal AsyncOperationHandle<TObject> Handle
		{
			get
			{
				return new AsyncOperationHandle<TObject>(this);
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000088A9 File Offset: 0x00006AA9
		private void UpdateCallback(float unscaledDeltaTime)
		{
			(this as IUpdateReceiver).Update(unscaledDeltaTime);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000088B7 File Offset: 0x00006AB7
		public void Complete(TObject result, bool success, string errorMsg)
		{
			this.Complete(result, success, errorMsg, true);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000088C3 File Offset: 0x00006AC3
		public void Complete(TObject result, bool success, string errorMsg, bool releaseDependenciesOnFailure)
		{
			this.Complete(result, success, (!string.IsNullOrEmpty(errorMsg)) ? new OperationException(errorMsg, null) : null, releaseDependenciesOnFailure);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000088E4 File Offset: 0x00006AE4
		public void Complete(TObject result, bool success, Exception exception, bool releaseDependenciesOnFailure = true)
		{
			if (this.IsDone)
			{
				return;
			}
			IUpdateReceiver updateReceiver = this as IUpdateReceiver;
			if (this.m_UpdateCallbacks != null && updateReceiver != null)
			{
				this.m_UpdateCallbacks.Remove(this.m_UpdateCallback);
			}
			this.Result = result;
			this.m_Status = (success ? AsyncOperationStatus.Succeeded : AsyncOperationStatus.Failed);
			if (this.m_RM != null && this.m_RM.postProfilerEvents)
			{
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationPercentComplete, 1, null, null));
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationComplete, 0, null, null));
			}
			if (this.m_Status == AsyncOperationStatus.Failed || exception != null)
			{
				if (exception == null || string.IsNullOrEmpty(exception.Message))
				{
					this.OperationException = new OperationException("Unknown error in AsyncOperation : " + this.DebugName, null);
				}
				else
				{
					this.OperationException = exception;
				}
			}
			if (this.m_Status == AsyncOperationStatus.Failed)
			{
				if (releaseDependenciesOnFailure)
				{
					this.ReleaseDependencies();
				}
				if (this.m_RM != null && this.m_RM.postProfilerEvents)
				{
					this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationFail, 0, (exception != null) ? exception.ToString() : null, null));
				}
				ICachable cachable = this as ICachable;
				if (((cachable != null) ? cachable.Key : null) != null)
				{
					ResourceManager rm = this.m_RM;
					if (rm != null)
					{
						rm.RemoveOperationFromCache(cachable.Key);
					}
				}
				this.RegisterForDeferredCallbackEvent(false);
			}
			else
			{
				this.InvokeCompletionEvent();
				this.DecrementReferenceCount();
			}
			this.IsRunning = false;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00008A54 File Offset: 0x00006C54
		internal void Start(ResourceManager rm, AsyncOperationHandle dependency, DelegateList<float> updateCallbacks)
		{
			this.m_RM = rm;
			this.IsRunning = true;
			this.HasExecuted = false;
			if (this.m_RM != null && this.m_RM.postProfilerEvents)
			{
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationCreate, 0, null, null));
				this.m_RM.PostDiagnosticEvent(new ResourceManager.DiagnosticEventContext(new AsyncOperationHandle(this), ResourceManager.DiagnosticEventType.AsyncOperationPercentComplete, 0, null, null));
			}
			this.IncrementReferenceCount();
			this.m_UpdateCallbacks = updateCallbacks;
			if (dependency.IsValid() && !dependency.IsDone)
			{
				dependency.Completed += this.m_dependencyCompleteAction;
				return;
			}
			this.InvokeExecute();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00008AF2 File Offset: 0x00006CF2
		internal void InvokeExecute()
		{
			this.Execute();
			this.HasExecuted = true;
			if (this is IUpdateReceiver && !this.IsDone)
			{
				this.m_UpdateCallbacks.Add(this.m_UpdateCallback);
			}
			Action executed = this.Executed;
			if (executed == null)
			{
				return;
			}
			executed();
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000260 RID: 608 RVA: 0x00008B32 File Offset: 0x00006D32
		// (remove) Token: 0x06000261 RID: 609 RVA: 0x00008B3B File Offset: 0x00006D3B
		event Action<AsyncOperationHandle> IAsyncOperation.CompletedTypeless
		{
			add
			{
				this.CompletedTypeless += value;
			}
			remove
			{
				this.CompletedTypeless -= value;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000262 RID: 610 RVA: 0x00008B44 File Offset: 0x00006D44
		// (remove) Token: 0x06000263 RID: 611 RVA: 0x00008B4D File Offset: 0x00006D4D
		event Action<AsyncOperationHandle> IAsyncOperation.Destroyed
		{
			add
			{
				this.Destroyed += value;
			}
			remove
			{
				this.Destroyed -= value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00008B56 File Offset: 0x00006D56
		int IAsyncOperation.Version
		{
			get
			{
				return this.Version;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00008B5E File Offset: 0x00006D5E
		int IAsyncOperation.ReferenceCount
		{
			get
			{
				return this.ReferenceCount;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00008B66 File Offset: 0x00006D66
		float IAsyncOperation.PercentComplete
		{
			get
			{
				return this.PercentComplete;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00008B6E File Offset: 0x00006D6E
		AsyncOperationStatus IAsyncOperation.Status
		{
			get
			{
				return this.Status;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00008B76 File Offset: 0x00006D76
		Exception IAsyncOperation.OperationException
		{
			get
			{
				return this.OperationException;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00008B7E File Offset: 0x00006D7E
		bool IAsyncOperation.IsDone
		{
			get
			{
				return this.IsDone;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00008B86 File Offset: 0x00006D86
		AsyncOperationHandle IAsyncOperation.Handle
		{
			get
			{
				return this.Handle;
			}
		}

		// Token: 0x1700009E RID: 158
		// (set) Token: 0x0600026B RID: 619 RVA: 0x00008B93 File Offset: 0x00006D93
		Action<IAsyncOperation> IAsyncOperation.OnDestroy
		{
			set
			{
				this.OnDestroy = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00008B9C File Offset: 0x00006D9C
		string IAsyncOperation.DebugName
		{
			get
			{
				return this.DebugName;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00008BA4 File Offset: 0x00006DA4
		object IAsyncOperation.GetResultAsObject()
		{
			return this.Result;
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00008BB1 File Offset: 0x00006DB1
		Type IAsyncOperation.ResultType
		{
			get
			{
				return typeof(TObject);
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008BBD File Offset: 0x00006DBD
		void IAsyncOperation.GetDependencies(List<AsyncOperationHandle> deps)
		{
			this.GetDependencies(deps);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008BC6 File Offset: 0x00006DC6
		void IAsyncOperation.DecrementReferenceCount()
		{
			this.DecrementReferenceCount();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008BCE File Offset: 0x00006DCE
		void IAsyncOperation.IncrementReferenceCount()
		{
			this.IncrementReferenceCount();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008BD6 File Offset: 0x00006DD6
		void IAsyncOperation.InvokeCompletionEvent()
		{
			this.InvokeCompletionEvent();
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008BDE File Offset: 0x00006DDE
		void IAsyncOperation.Start(ResourceManager rm, AsyncOperationHandle dependency, DelegateList<float> updateCallbacks)
		{
			this.Start(rm, dependency, updateCallbacks);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008BE9 File Offset: 0x00006DE9
		internal virtual void ReleaseDependencies()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008BEB File Offset: 0x00006DEB
		DownloadStatus IAsyncOperation.GetDownloadStatus(HashSet<object> visited)
		{
			return this.GetDownloadStatus(visited);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008BF4 File Offset: 0x00006DF4
		internal virtual DownloadStatus GetDownloadStatus(HashSet<object> visited)
		{
			visited.Add(this);
			return new DownloadStatus
			{
				IsDone = this.IsDone
			};
		}

		// Token: 0x040000EA RID: 234
		private int m_referenceCount = 1;

		// Token: 0x040000EB RID: 235
		internal AsyncOperationStatus m_Status;

		// Token: 0x040000EC RID: 236
		internal Exception m_Error;

		// Token: 0x040000ED RID: 237
		internal ResourceManager m_RM;

		// Token: 0x040000EE RID: 238
		internal int m_Version;

		// Token: 0x040000EF RID: 239
		private DelegateList<AsyncOperationHandle> m_DestroyedAction;

		// Token: 0x040000F0 RID: 240
		private DelegateList<AsyncOperationHandle<TObject>> m_CompletedActionT;

		// Token: 0x040000F1 RID: 241
		private Action<IAsyncOperation> m_OnDestroyAction;

		// Token: 0x040000F2 RID: 242
		private Action<AsyncOperationHandle> m_dependencyCompleteAction;

		// Token: 0x040000F3 RID: 243
		protected internal bool HasExecuted;

		// Token: 0x040000F6 RID: 246
		private TaskCompletionSource<TObject> m_taskCompletionSource;

		// Token: 0x040000F7 RID: 247
		private TaskCompletionSource<object> m_taskCompletionSourceTypeless;

		// Token: 0x040000F8 RID: 248
		private bool m_InDeferredCallbackQueue;

		// Token: 0x040000F9 RID: 249
		private DelegateList<float> m_UpdateCallbacks;

		// Token: 0x040000FA RID: 250
		private Action<float> m_UpdateCallback;
	}
}
