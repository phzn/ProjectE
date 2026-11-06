using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x0200005D RID: 93
	internal class GroupOperation : AsyncOperationBase<IList<AsyncOperationHandle>>, ICachable
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x000092AA File Offset: 0x000074AA
		public GroupOperation()
		{
			this.m_InternalOnComplete = new Action<AsyncOperationHandle>(this.OnOperationCompleted);
			base.Result = new List<AsyncOperationHandle>();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000092DC File Offset: 0x000074DC
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone || base.Result == null)
			{
				return true;
			}
			foreach (AsyncOperationHandle asyncOperationHandle in base.Result)
			{
				asyncOperationHandle.WaitForCompletion();
				if (base.Result == null)
				{
					return true;
				}
			}
			ResourceManager rm = this.m_RM;
			if (rm != null)
			{
				rm.Update(Time.unscaledDeltaTime);
			}
			if (!base.IsDone && base.Result != null)
			{
				this.Execute();
			}
			ResourceManager rm2 = this.m_RM;
			if (rm2 != null)
			{
				rm2.Update(Time.unscaledDeltaTime);
			}
			return base.IsDone;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00009394 File Offset: 0x00007594
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x0000939C File Offset: 0x0000759C
		IOperationCacheKey ICachable.Key { get; set; }

		// Token: 0x060002C4 RID: 708 RVA: 0x000093A5 File Offset: 0x000075A5
		internal IList<AsyncOperationHandle> GetDependentOps()
		{
			return base.Result;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000093AD File Offset: 0x000075AD
		public override void GetDependencies(List<AsyncOperationHandle> deps)
		{
			deps.AddRange(base.Result);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000093BC File Offset: 0x000075BC
		internal override void ReleaseDependencies()
		{
			for (int i = 0; i < base.Result.Count; i++)
			{
				if (base.Result[i].IsValid())
				{
					base.Result[i].Release();
				}
			}
			base.Result.Clear();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00009414 File Offset: 0x00007614
		internal override DownloadStatus GetDownloadStatus(HashSet<object> visited)
		{
			DownloadStatus downloadStatus = new DownloadStatus
			{
				IsDone = base.IsDone
			};
			for (int i = 0; i < base.Result.Count; i++)
			{
				if (base.Result[i].IsValid())
				{
					DownloadStatus downloadStatus2 = base.Result[i].InternalGetDownloadStatus(visited);
					downloadStatus.DownloadedBytes += downloadStatus2.DownloadedBytes;
					downloadStatus.TotalBytes += downloadStatus2.TotalBytes;
				}
			}
			return downloadStatus;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000094A0 File Offset: 0x000076A0
		private bool DependenciesAreUnchanged(List<AsyncOperationHandle> deps)
		{
			if (this.m_CachedDependencyLocations.Count != deps.Count)
			{
				return false;
			}
			foreach (AsyncOperationHandle asyncOperationHandle in deps)
			{
				if (!this.m_CachedDependencyLocations.Contains(asyncOperationHandle.LocationName))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x00009518 File Offset: 0x00007718
		protected override string DebugName
		{
			get
			{
				List<AsyncOperationHandle> list = new List<AsyncOperationHandle>();
				this.GetDependencies(list);
				if (list.Count == 0)
				{
					return "Dependencies";
				}
				if (this.debugName != null && this.DependenciesAreUnchanged(list))
				{
					return this.debugName;
				}
				this.m_CachedDependencyLocations.Clear();
				string text = "Dependencies [";
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = list[i].LocationName;
					this.m_CachedDependencyLocations.Add(text2);
					if (text2 != null)
					{
						if (text2.Length > 45)
						{
							text2 = AsyncOperationBase<object>.ShortenPath(text2, true);
							text2 = text2.Substring(0, Math.Min(45, text2.Length)) + "...";
						}
						if (i == list.Count - 1)
						{
							text += text2;
						}
						else
						{
							text = text + text2 + ", ";
						}
					}
				}
				text += "]";
				this.debugName = text;
				return this.debugName;
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00009618 File Offset: 0x00007818
		protected override void Execute()
		{
			this.m_LoadedCount = 0;
			for (int i = 0; i < base.Result.Count; i++)
			{
				if (base.Result[i].IsDone)
				{
					this.m_LoadedCount++;
				}
				else
				{
					base.Result[i].Completed += this.m_InternalOnComplete;
				}
			}
			this.CompleteIfDependenciesComplete();
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00009688 File Offset: 0x00007888
		private void CompleteIfDependenciesComplete()
		{
			if (this.m_LoadedCount == base.Result.Count)
			{
				bool flag = true;
				OperationException ex = null;
				if (!this.m_Settings.HasFlag(GroupOperation.GroupOperationSettings.AllowFailedDependencies))
				{
					for (int i = 0; i < base.Result.Count; i++)
					{
						if (base.Result[i].Status != AsyncOperationStatus.Succeeded)
						{
							flag = false;
							ex = new OperationException("GroupOperation failed because one of its dependencies failed", base.Result[i].OperationException);
							break;
						}
					}
				}
				base.Complete(base.Result, flag, ex, this.m_Settings.HasFlag(GroupOperation.GroupOperationSettings.ReleaseDependenciesOnFailure));
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000973B File Offset: 0x0000793B
		protected override void Destroy()
		{
			this.ReleaseDependencies();
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00009744 File Offset: 0x00007944
		protected override float Progress
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < base.Result.Count; i++)
				{
					AsyncOperationHandle asyncOperationHandle = base.Result[i];
					if (!asyncOperationHandle.IsDone)
					{
						num += asyncOperationHandle.PercentComplete;
					}
					else
					{
						num += 1f;
					}
				}
				return num / (float)base.Result.Count;
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000097A5 File Offset: 0x000079A5
		public void Init(List<AsyncOperationHandle> operations, bool releaseDependenciesOnFailure = true, bool allowFailedDependencies = false)
		{
			base.Result = new List<AsyncOperationHandle>(operations);
			this.m_Settings = (releaseDependenciesOnFailure ? GroupOperation.GroupOperationSettings.ReleaseDependenciesOnFailure : GroupOperation.GroupOperationSettings.None);
			if (allowFailedDependencies)
			{
				this.m_Settings |= GroupOperation.GroupOperationSettings.AllowFailedDependencies;
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000097D1 File Offset: 0x000079D1
		public void Init(List<AsyncOperationHandle> operations, GroupOperation.GroupOperationSettings settings)
		{
			base.Result = new List<AsyncOperationHandle>(operations);
			this.m_Settings = settings;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000097E6 File Offset: 0x000079E6
		private void OnOperationCompleted(AsyncOperationHandle op)
		{
			this.m_LoadedCount++;
			this.CompleteIfDependenciesComplete();
		}

		// Token: 0x04000108 RID: 264
		private Action<AsyncOperationHandle> m_InternalOnComplete;

		// Token: 0x04000109 RID: 265
		private int m_LoadedCount;

		// Token: 0x0400010A RID: 266
		private GroupOperation.GroupOperationSettings m_Settings;

		// Token: 0x0400010B RID: 267
		private string debugName;

		// Token: 0x0400010C RID: 268
		private const int k_MaxDisplayedLocationLength = 45;

		// Token: 0x0400010E RID: 270
		private HashSet<string> m_CachedDependencyLocations = new HashSet<string>();

		// Token: 0x02000083 RID: 131
		[Flags]
		public enum GroupOperationSettings
		{
			// Token: 0x0400018F RID: 399
			None = 0,
			// Token: 0x04000190 RID: 400
			ReleaseDependenciesOnFailure = 1,
			// Token: 0x04000191 RID: 401
			AllowFailedDependencies = 2
		}
	}
}
