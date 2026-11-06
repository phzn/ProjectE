using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x02000059 RID: 89
	public struct AsyncOperationHandle<TObject> : IEnumerator, IEquatable<AsyncOperationHandle<TObject>>
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000278 RID: 632 RVA: 0x00008C27 File Offset: 0x00006E27
		internal int Version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00008C2F File Offset: 0x00006E2F
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00008C37 File Offset: 0x00006E37
		internal string LocationName
		{
			get
			{
				return this.m_LocationName;
			}
			set
			{
				this.m_LocationName = value;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008C40 File Offset: 0x00006E40
		public static implicit operator AsyncOperationHandle(AsyncOperationHandle<TObject> obj)
		{
			return new AsyncOperationHandle(obj.m_InternalOp, obj.m_Version, obj.m_LocationName);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00008C59 File Offset: 0x00006E59
		internal AsyncOperationHandle(AsyncOperationBase<TObject> op)
		{
			this.m_InternalOp = op;
			this.m_Version = ((op != null) ? op.Version : 0);
			this.m_LocationName = null;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008C7B File Offset: 0x00006E7B
		public DownloadStatus GetDownloadStatus()
		{
			return this.InternalGetDownloadStatus(new HashSet<object>());
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00008C88 File Offset: 0x00006E88
		internal DownloadStatus InternalGetDownloadStatus(HashSet<object> visited)
		{
			if (visited == null)
			{
				visited = new HashSet<object>();
			}
			if (!visited.Add(this.InternalOp))
			{
				return new DownloadStatus
				{
					IsDone = this.IsDone
				};
			}
			return this.InternalOp.GetDownloadStatus(visited);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008CD0 File Offset: 0x00006ED0
		internal AsyncOperationHandle(IAsyncOperation op)
		{
			this.m_InternalOp = (AsyncOperationBase<TObject>)op;
			this.m_Version = ((op != null) ? op.Version : 0);
			this.m_LocationName = null;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00008CF7 File Offset: 0x00006EF7
		internal AsyncOperationHandle(IAsyncOperation op, int version)
		{
			this.m_InternalOp = (AsyncOperationBase<TObject>)op;
			this.m_Version = version;
			this.m_LocationName = null;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008D13 File Offset: 0x00006F13
		internal AsyncOperationHandle(IAsyncOperation op, string locationName)
		{
			this.m_InternalOp = (AsyncOperationBase<TObject>)op;
			this.m_Version = ((op != null) ? op.Version : 0);
			this.m_LocationName = locationName;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008D3A File Offset: 0x00006F3A
		internal AsyncOperationHandle(IAsyncOperation op, int version, string locationName)
		{
			this.m_InternalOp = (AsyncOperationBase<TObject>)op;
			this.m_Version = version;
			this.m_LocationName = locationName;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00008D56 File Offset: 0x00006F56
		internal AsyncOperationHandle<TObject> Acquire()
		{
			this.InternalOp.IncrementReferenceCount();
			return this;
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000284 RID: 644 RVA: 0x00008D69 File Offset: 0x00006F69
		// (remove) Token: 0x06000285 RID: 645 RVA: 0x00008D77 File Offset: 0x00006F77
		public event Action<AsyncOperationHandle<TObject>> Completed
		{
			add
			{
				this.InternalOp.Completed += value;
			}
			remove
			{
				this.InternalOp.Completed -= value;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008D85 File Offset: 0x00006F85
		public void ReleaseHandleOnCompletion()
		{
			this.Completed += delegate(AsyncOperationHandle<TObject> op)
			{
				op.Release();
			};
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000287 RID: 647 RVA: 0x00008DAC File Offset: 0x00006FAC
		// (remove) Token: 0x06000288 RID: 648 RVA: 0x00008DBA File Offset: 0x00006FBA
		public event Action<AsyncOperationHandle> CompletedTypeless
		{
			add
			{
				this.InternalOp.CompletedTypeless += value;
			}
			remove
			{
				this.InternalOp.CompletedTypeless -= value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000289 RID: 649 RVA: 0x00008DC8 File Offset: 0x00006FC8
		public string DebugName
		{
			get
			{
				if (!this.IsValid())
				{
					return "InvalidHandle";
				}
				return ((IAsyncOperation)this.InternalOp).DebugName;
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00008DE3 File Offset: 0x00006FE3
		public void GetDependencies(List<AsyncOperationHandle> deps)
		{
			this.InternalOp.GetDependencies(deps);
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600028B RID: 651 RVA: 0x00008DF1 File Offset: 0x00006FF1
		// (remove) Token: 0x0600028C RID: 652 RVA: 0x00008DFF File Offset: 0x00006FFF
		public event Action<AsyncOperationHandle> Destroyed
		{
			add
			{
				this.InternalOp.Destroyed += value;
			}
			remove
			{
				this.InternalOp.Destroyed -= value;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008E0D File Offset: 0x0000700D
		public bool Equals(AsyncOperationHandle<TObject> other)
		{
			return this.m_Version == other.m_Version && this.m_InternalOp == other.m_InternalOp;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00008E2D File Offset: 0x0000702D
		public override int GetHashCode()
		{
			if (this.m_InternalOp != null)
			{
				return this.m_InternalOp.GetHashCode() * 17 + this.m_Version;
			}
			return 0;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00008E50 File Offset: 0x00007050
		public TObject WaitForCompletion()
		{
			if (this.IsValid() && !this.InternalOp.IsDone)
			{
				this.InternalOp.WaitForCompletion();
			}
			AsyncOperationBase<TObject> internalOp = this.m_InternalOp;
			if (internalOp != null)
			{
				ResourceManager rm = internalOp.m_RM;
				if (rm != null)
				{
					rm.Update(Time.unscaledDeltaTime);
				}
			}
			if (this.IsValid())
			{
				return this.Result;
			}
			return default(TObject);
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00008EB6 File Offset: 0x000070B6
		internal AsyncOperationBase<TObject> InternalOp
		{
			get
			{
				if (this.m_InternalOp == null || this.m_InternalOp.Version != this.m_Version)
				{
					throw new Exception("Attempting to use an invalid operation handle");
				}
				return this.m_InternalOp;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00008EE4 File Offset: 0x000070E4
		public bool IsDone
		{
			get
			{
				return !this.IsValid() || this.InternalOp.IsDone;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00008EFB File Offset: 0x000070FB
		public bool IsValid()
		{
			return this.m_InternalOp != null && this.m_InternalOp.Version == this.m_Version;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00008F1A File Offset: 0x0000711A
		public Exception OperationException
		{
			get
			{
				return this.InternalOp.OperationException;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00008F27 File Offset: 0x00007127
		public float PercentComplete
		{
			get
			{
				return this.InternalOp.PercentComplete;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000295 RID: 661 RVA: 0x00008F34 File Offset: 0x00007134
		internal int ReferenceCount
		{
			get
			{
				return this.InternalOp.ReferenceCount;
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008F41 File Offset: 0x00007141
		public void Release()
		{
			this.InternalOp.DecrementReferenceCount();
			this.m_InternalOp = null;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000297 RID: 663 RVA: 0x00008F55 File Offset: 0x00007155
		public TObject Result
		{
			get
			{
				return this.InternalOp.Result;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00008F62 File Offset: 0x00007162
		public AsyncOperationStatus Status
		{
			get
			{
				return this.InternalOp.Status;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00008F6F File Offset: 0x0000716F
		public Task<TObject> Task
		{
			get
			{
				return this.InternalOp.Task;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00008F7C File Offset: 0x0000717C
		object IEnumerator.Current
		{
			get
			{
				return this.Result;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008F89 File Offset: 0x00007189
		bool IEnumerator.MoveNext()
		{
			return !this.IsDone;
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00008F94 File Offset: 0x00007194
		void IEnumerator.Reset()
		{
		}

		// Token: 0x040000FB RID: 251
		internal AsyncOperationBase<TObject> m_InternalOp;

		// Token: 0x040000FC RID: 252
		private int m_Version;

		// Token: 0x040000FD RID: 253
		private string m_LocationName;
	}
}
