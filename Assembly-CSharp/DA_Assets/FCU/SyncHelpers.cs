using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using UnityEngine;

namespace DA_Assets.FCU
{
	// Token: 0x020002E8 RID: 744
	[Serializable]
	public class SyncHelpers : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060014AE RID: 5294 RVA: 0x00086008 File Offset: 0x00084208
		public void DestroySyncHelpers()
		{
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.DestroySyncHelpersAsync(cancellationTokenSource);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00086030 File Offset: 0x00084230
		private async Task DestroySyncHelpersAsync(CancellationTokenSource cts)
		{
			SyncHelper[] syncHelpers = this.GetAllSyncHelpers();
			for (int i = 0; i < syncHelpers.Length; i++)
			{
				syncHelpers[i].Destroy();
				await Task.Yield();
			}
			DALogger.Log(FcuLocKey.log_current_canvas_metas_destroy.Localize(new object[]
			{
				this.monoBeh.GetInstanceID(),
				syncHelpers.Length,
				"SyncHelper"
			}));
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00086073 File Offset: 0x00084273
		public SyncHelper[] GetAllSyncHelpers()
		{
			return this.monoBeh.gameObject.GetComponentsInReverseOrder<SyncHelper>().ToArray();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0008608C File Offset: 0x0008428C
		public bool IsExistsOnCurrentCanvas(FObject fobject, out SyncHelper syncObject)
		{
			foreach (SyncHelper syncHelper in this.GetAllSyncHelpers())
			{
				if (syncHelper.Data.Id == fobject.Id)
				{
					syncObject = syncHelper;
					return true;
				}
			}
			syncObject = null;
			return false;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000860D4 File Offset: 0x000842D4
		public SyncData GetRootFrame(SyncData syncData)
		{
			GameObject gameObject = syncData.GameObject;
			while (gameObject != null)
			{
				SyncHelper component = gameObject.GetComponent<SyncHelper>();
				if (component != null && component.ContainsTag(FcuTag.Frame))
				{
					return component.Data;
				}
				Transform parent = gameObject.transform.parent;
				gameObject = ((parent != null) ? parent.gameObject : null);
			}
			return null;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0008612C File Offset: 0x0008432C
		public void RestoreRootFrames(SyncHelper[] syncHelpers)
		{
			List<SyncHelper> list = new List<SyncHelper>();
			foreach (SyncHelper syncHelper in syncHelpers)
			{
				if (syncHelper.ContainsTag(FcuTag.Frame))
				{
					list.Add(syncHelper);
				}
			}
			foreach (SyncHelper syncHelper2 in list)
			{
				this.SetRootFrameToAllChilds(syncHelper2.gameObject, syncHelper2.Data);
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x000861B4 File Offset: 0x000843B4
		public void SetRootFrameToAllChilds(GameObject @object, SyncData rootFrame)
		{
			if (@object == null)
			{
				return;
			}
			foreach (object obj in @object.transform)
			{
				Transform transform = (Transform)obj;
				if (!(transform == null))
				{
					SyncHelper syncHelper;
					if (transform.TryGetComponentSafe<SyncHelper>(out syncHelper))
					{
						syncHelper.Data.RootFrame = rootFrame;
					}
					this.SetRootFrameToAllChilds(transform.gameObject, rootFrame);
				}
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0008623C File Offset: 0x0008443C
		public void OptimizeSyncHelpers()
		{
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.OptimizeSyncHelpersAsync(cancellationTokenSource);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x00086264 File Offset: 0x00084464
		private async Task OptimizeSyncHelpersAsync(CancellationTokenSource cts)
		{
			SyncHelpers.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.counter = 0;
			SyncHelpers.<OptimizeSyncHelpersAsync>g__OptimizeAllChilds|8_0(this.monoBeh.gameObject, ref CS$<>8__locals1);
			await Task.Delay(100);
			DALogger.Log(FcuLocKey.log_fcu_assigned.Localize(new object[]
			{
				CS$<>8__locals1.counter,
				"FigmaConverterUnity",
				this.monoBeh.GetInstanceID()
			}));
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x000862A8 File Offset: 0x000844A8
		public void SetFcuToAllSyncHelpers()
		{
			CancellationTokenSource cancellationTokenSource = this.monoBeh.CancellationTokenController.CreateNew(TokenType.Import);
			this.SetFcuToAllSyncHelpersAsync(cancellationTokenSource);
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x000862D0 File Offset: 0x000844D0
		public async Task SetFcuToAllSyncHelpersAsync(CancellationTokenSource cts)
		{
			int counter = 0;
			this.SetFcuToAllChilds(this.monoBeh.gameObject, ref counter);
			await Task.Delay(100);
			DALogger.Log(FcuLocKey.log_fcu_assigned.Localize(new object[]
			{
				counter,
				"FigmaConverterUnity",
				this.monoBeh.GetInstanceID()
			}));
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00086314 File Offset: 0x00084514
		public void SetFcuToAllChilds(GameObject @object, ref int counter)
		{
			if (@object == null)
			{
				return;
			}
			foreach (object obj in @object.transform)
			{
				Transform transform = (Transform)obj;
				if (!(transform == null))
				{
					SyncHelper syncHelper;
					if (transform.TryGetComponentSafe<SyncHelper>(out syncHelper))
					{
						counter++;
						syncHelper.Data.FigmaConverterUnity = this.monoBeh;
					}
					this.SetFcuToAllChilds(transform.gameObject, ref counter);
				}
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000863A8 File Offset: 0x000845A8
		[CompilerGenerated]
		internal static void <OptimizeSyncHelpersAsync>g__OptimizeAllChilds|8_0(GameObject @object, ref SyncHelpers.<>c__DisplayClass8_0 A_1)
		{
			if (@object == null)
			{
				return;
			}
			foreach (object obj in @object.transform)
			{
				Transform transform = (Transform)obj;
				if (!(transform == null))
				{
					SyncHelper syncHelper;
					if (transform.TryGetComponentSafe<SyncHelper>(out syncHelper))
					{
						int counter = A_1.counter;
						A_1.counter = counter + 1;
						syncHelper.Data.HashData = null;
						syncHelper.Data.HashDataTree = null;
					}
					SyncHelpers.<OptimizeSyncHelpersAsync>g__OptimizeAllChilds|8_0(transform.gameObject, ref A_1);
				}
			}
		}
	}
}
