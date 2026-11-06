using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x0200005A RID: 90
	public struct AsyncOperationHandle : IEnumerator
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00008F96 File Offset: 0x00007196
		internal int Version
		{
			get
			{
				return this.m_Version;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008F9E File Offset: 0x0000719E
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00008FA6 File Offset: 0x000071A6
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

		// Token: 0x060002A0 RID: 672 RVA: 0x00008FAF File Offset: 0x000071AF
		internal AsyncOperationHandle(IAsyncOperation op)
		{
			this.m_InternalOp = op;
			this.m_Version = ((op != null) ? op.Version : 0);
			this.m_LocationName = null;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008FD1 File Offset: 0x000071D1
		internal AsyncOperationHandle(IAsyncOperation op, int version)
		{
			this.m_InternalOp = op;
			this.m_Version = version;
			this.m_LocationName = null;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008FE8 File Offset: 0x000071E8
		internal AsyncOperationHandle(IAsyncOperation op, string locationName)
		{
			this.m_InternalOp = op;
			this.m_Version = ((op != null) ? op.Version : 0);
			this.m_LocationName = locationName;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000900A File Offset: 0x0000720A
		internal AsyncOperationHandle(IAsyncOperation op, int version, string locationName)
		{
			this.m_InternalOp = op;
			this.m_Version = version;
			this.m_LocationName = locationName;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009021 File Offset: 0x00007221
		internal AsyncOperationHandle Acquire()
		{
			this.InternalOp.IncrementReferenceCount();
			return this;
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060002A5 RID: 677 RVA: 0x00009034 File Offset: 0x00007234
		// (remove) Token: 0x060002A6 RID: 678 RVA: 0x00009042 File Offset: 0x00007242
		public event Action<AsyncOperationHandle> Completed
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

		// Token: 0x060002A7 RID: 679 RVA: 0x00009050 File Offset: 0x00007250
		public void ReleaseHandleOnCompletion()
		{
			this.Completed += delegate(AsyncOperationHandle op)
			{
				op.Release();
			};
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00009077 File Offset: 0x00007277
		public AsyncOperationHandle<T> Convert<T>()
		{
			return new AsyncOperationHandle<T>(this.InternalOp, this.m_Version, this.m_LocationName);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00009090 File Offset: 0x00007290
		public bool Equals(AsyncOperationHandle other)
		{
			return this.m_Version == other.m_Version && this.m_InternalOp == other.m_InternalOp;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002AA RID: 682 RVA: 0x000090B0 File Offset: 0x000072B0
		public string DebugName
		{
			get
			{
				if (!this.IsValid())
				{
					return "InvalidHandle";
				}
				return this.InternalOp.DebugName;
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060002AB RID: 683 RVA: 0x000090CB File Offset: 0x000072CB
		// (remove) Token: 0x060002AC RID: 684 RVA: 0x000090D9 File Offset: 0x000072D9
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

		// Token: 0x060002AD RID: 685 RVA: 0x000090E7 File Offset: 0x000072E7
		public void GetDependencies(List<AsyncOperationHandle> deps)
		{
			this.InternalOp.GetDependencies(deps);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000090F5 File Offset: 0x000072F5
		public override int GetHashCode()
		{
			if (this.m_InternalOp != null)
			{
				return this.m_InternalOp.GetHashCode() * 17 + this.m_Version;
			}
			return 0;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002AF RID: 687 RVA: 0x00009116 File Offset: 0x00007316
		private IAsyncOperation InternalOp
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00009144 File Offset: 0x00007344
		public bool IsDone
		{
			get
			{
				return !this.IsValid() || this.InternalOp.IsDone;
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000915B File Offset: 0x0000735B
		public bool IsValid()
		{
			return this.m_InternalOp != null && this.m_InternalOp.Version == this.m_Version;
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000917A File Offset: 0x0000737A
		public Exception OperationException
		{
			get
			{
				return this.InternalOp.OperationException;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00009187 File Offset: 0x00007387
		public float PercentComplete
		{
			get
			{
				return this.InternalOp.PercentComplete;
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009194 File Offset: 0x00007394
		public DownloadStatus GetDownloadStatus()
		{
			return this.InternalGetDownloadStatus(new HashSet<object>());
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000091A4 File Offset: 0x000073A4
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

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000091EC File Offset: 0x000073EC
		internal int ReferenceCount
		{
			get
			{
				return this.InternalOp.ReferenceCount;
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000091F9 File Offset: 0x000073F9
		public void Release()
		{
			this.InternalOp.DecrementReferenceCount();
			this.m_InternalOp = null;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000920D File Offset: 0x0000740D
		public object Result
		{
			get
			{
				return this.InternalOp.GetResultAsObject();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000921A File Offset: 0x0000741A
		public AsyncOperationStatus Status
		{
			get
			{
				return this.InternalOp.Status;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00009227 File Offset: 0x00007427
		public Task<object> Task
		{
			get
			{
				return this.InternalOp.Task;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00009234 File Offset: 0x00007434
		object IEnumerator.Current
		{
			get
			{
				return this.Result;
			}
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000923C File Offset: 0x0000743C
		bool IEnumerator.MoveNext()
		{
			return !this.IsDone;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00009247 File Offset: 0x00007447
		void IEnumerator.Reset()
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00009249 File Offset: 0x00007449
		public object WaitForCompletion()
		{
			if (this.IsValid() && !this.InternalOp.IsDone)
			{
				this.InternalOp.WaitForCompletion();
			}
			if (this.IsValid())
			{
				return this.Result;
			}
			return null;
		}

		// Token: 0x040000FE RID: 254
		internal IAsyncOperation m_InternalOp;

		// Token: 0x040000FF RID: 255
		private int m_Version;

		// Token: 0x04000100 RID: 256
		private string m_LocationName;
	}
}
