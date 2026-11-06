using System;
using System.Collections.Generic;
using System.Text;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000078 RID: 120
	public class ClothManager : IManager, IDisposable, IValid
	{
		// Token: 0x060001BE RID: 446 RVA: 0x00010720 File Offset: 0x0000E920
		public void Dispose()
		{
			this.isValid = false;
			this.clothSet.Clear();
			this.boneClothSet.Clear();
			this.meshClothSet.Clear();
			this.animatorVisibleDict.Clear();
			this.rendererVisibleDict.Clear();
			MagicaManager.afterEarlyUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterEarlyUpdateDelegate, new MagicaManager.UpdateMethod(this.OnEarlyClothUpdate));
			MagicaManager.firstPreUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.firstPreUpdateDelegate, new MagicaManager.UpdateMethod(this.OnFirstPreUpdate));
			MagicaManager.afterLateUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterLateUpdateDelegate, new MagicaManager.UpdateMethod(this.OnAfterLateUpdate));
			MagicaManager.beforeLateUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.beforeLateUpdateDelegate, new MagicaManager.UpdateMethod(this.OnBeforeLateUpdate));
		}

		// Token: 0x060001BF RID: 447 RVA: 0x000107EB File Offset: 0x0000E9EB
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000107F4 File Offset: 0x0000E9F4
		public void Initialize()
		{
			this.clothSet.Clear();
			this.boneClothSet.Clear();
			this.meshClothSet.Clear();
			MagicaManager.afterEarlyUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterEarlyUpdateDelegate, new MagicaManager.UpdateMethod(this.OnEarlyClothUpdate));
			MagicaManager.firstPreUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.firstPreUpdateDelegate, new MagicaManager.UpdateMethod(this.OnFirstPreUpdate));
			MagicaManager.afterLateUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterLateUpdateDelegate, new MagicaManager.UpdateMethod(this.OnAfterLateUpdate));
			MagicaManager.beforeLateUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.beforeLateUpdateDelegate, new MagicaManager.UpdateMethod(this.OnBeforeLateUpdate));
			this.isValid = true;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000108A9 File Offset: 0x0000EAA9
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000108B1 File Offset: 0x0000EAB1
		private void ClearMasterJob()
		{
			this.masterJob = default(JobHandle);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000108BF File Offset: 0x0000EABF
		private void CompleteMasterJob()
		{
			this.masterJob.Complete();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000108CC File Offset: 0x0000EACC
		internal int AddCloth(ClothProcess cprocess, in ClothParameters clothParams)
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = MagicaManager.Team.AddTeam(cprocess, clothParams);
			if (num == 0)
			{
				return 0;
			}
			this.clothSet.Add(cprocess);
			ClothProcess.ClothType clothType = cprocess.clothType;
			if (clothType != ClothProcess.ClothType.MeshCloth)
			{
				if (clothType == ClothProcess.ClothType.BoneCloth || clothType == ClothProcess.ClothType.BoneSpring)
				{
					this.boneClothSet.Add(cprocess);
				}
				else
				{
					object obj = string.Format("Invalid cloth type! :{0}", cprocess.clothType);
					Develop.LogError(in obj);
				}
			}
			else
			{
				this.meshClothSet.Add(cprocess);
			}
			MagicaManager.Team.comp2TeamIdMap.Add(cprocess.cloth.GetInstanceID(), num);
			return num;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00010974 File Offset: 0x0000EB74
		internal void RemoveCloth(ClothProcess cprocess)
		{
			if (!this.isValid)
			{
				return;
			}
			MagicaManager.Team.RemoveTeam(cprocess.TeamId);
			this.clothSet.Remove(cprocess);
			this.boneClothSet.Remove(cprocess);
			this.meshClothSet.Remove(cprocess);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000109C4 File Offset: 0x0000EBC4
		private void OnEarlyClothUpdate()
		{
			if (MagicaManager.Team.TrueTeamCount > 0)
			{
				if (MagicaManager.Team.ActiveTeamCount > 0)
				{
					MagicaManager.Team.CameraCullingPostProcess();
				}
				this.ClearMasterJob();
				this.masterJob = MagicaManager.Bone.RestoreTransform(this.masterJob);
				this.CompleteMasterJob();
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00010A18 File Offset: 0x0000EC18
		private void OnFirstPreUpdate()
		{
			if (MagicaManager.Team.TrueTeamCount > 0 && MagicaManager.Time.FixedUpdateCount == 0 && MagicaManager.Bone.existFixedTeam.Value)
			{
				this.ClearMasterJob();
				this.masterJob = MagicaManager.Bone.RestoreBaseTransform(this.masterJob);
				this.CompleteMasterJob();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00010A71 File Offset: 0x0000EC71
		private void OnBeforeLateUpdate()
		{
			if (MagicaManager.Time.updateLocation == TimeManager.UpdateLocation.BeforeLateUpdate)
			{
				this.ClothUpdate();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00010A86 File Offset: 0x0000EC86
		private void OnAfterLateUpdate()
		{
			if (MagicaManager.Time.updateLocation == TimeManager.UpdateLocation.AfterLateUpdate)
			{
				this.ClothUpdate();
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00010A9C File Offset: 0x0000EC9C
		private void ClothUpdate()
		{
			if (!MagicaManager.IsPlaying())
			{
				return;
			}
			TeamManager team = MagicaManager.Team;
			if (team.TrueTeamCount == 0)
			{
				return;
			}
			Action onPreSimulation = MagicaManager.OnPreSimulation;
			if (onPreSimulation != null)
			{
				onPreSimulation();
			}
			SimulationManager simulation = MagicaManager.Simulation;
			TransformManager bone = MagicaManager.Bone;
			WindManager wind = MagicaManager.Wind;
			MagicaManager.Time.FrameUpdate();
			team.AlwaysTeamUpdate();
			if (team.ActiveTeamCount == 0)
			{
				return;
			}
			wind.AlwaysWindUpdate();
			simulation.WorkBufferUpdate();
			this.ClearMasterJob();
			this.masterJob = bone.ReadTransformSchedule(this.masterJob);
			this.masterJob = simulation.ClothSimulationSchedule(this.masterJob);
			team.CameraCullingPreProcess();
			this.CompleteMasterJob();
			Action onPostSimulation = MagicaManager.OnPostSimulation;
			if (onPostSimulation == null)
			{
				return;
			}
			onPostSimulation();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00010B4C File Offset: 0x0000ED4C
		internal void ClearVisibleDict()
		{
			this.animatorVisibleDict.Clear();
			this.rendererVisibleDict.Clear();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00010B64 File Offset: 0x0000ED64
		internal bool CheckVisible(Animator ani, List<Renderer> renderers)
		{
			if (!ani)
			{
				return this.CheckRendererVisible(renderers);
			}
			int instanceID = ani.GetInstanceID();
			if (this.animatorVisibleDict.ContainsKey(instanceID))
			{
				return this.animatorVisibleDict[instanceID];
			}
			bool flag = this.CheckRendererVisible(renderers);
			this.animatorVisibleDict.Add(instanceID, flag);
			return flag;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00010BBC File Offset: 0x0000EDBC
		private bool CheckRendererVisible(List<Renderer> renderers)
		{
			foreach (Renderer renderer in renderers)
			{
				if (renderer)
				{
					int instanceID = renderer.GetInstanceID();
					bool flag;
					if (this.rendererVisibleDict.ContainsKey(instanceID))
					{
						flag = this.rendererVisibleDict[instanceID];
					}
					else
					{
						flag = renderer.isVisible;
						this.rendererVisibleDict.Add(instanceID, flag);
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000706D File Offset: 0x0000526D
		public void InformationLog(StringBuilder allsb)
		{
		}

		// Token: 0x04000347 RID: 839
		internal HashSet<ClothProcess> clothSet = new HashSet<ClothProcess>(256);

		// Token: 0x04000348 RID: 840
		internal HashSet<ClothProcess> boneClothSet = new HashSet<ClothProcess>();

		// Token: 0x04000349 RID: 841
		internal HashSet<ClothProcess> meshClothSet = new HashSet<ClothProcess>();

		// Token: 0x0400034A RID: 842
		private Dictionary<int, bool> animatorVisibleDict = new Dictionary<int, bool>(30);

		// Token: 0x0400034B RID: 843
		private Dictionary<int, bool> rendererVisibleDict = new Dictionary<int, bool>(100);

		// Token: 0x0400034C RID: 844
		private JobHandle masterJob;

		// Token: 0x0400034D RID: 845
		private bool isValid;

		// Token: 0x0400034E RID: 846
		private static readonly ProfilerMarker startClothUpdateTimeProfiler = new ProfilerMarker("StartClothUpdate.Time");

		// Token: 0x0400034F RID: 847
		private static readonly ProfilerMarker startClothUpdateTeamProfiler = new ProfilerMarker("StartClothUpdate.Team");

		// Token: 0x04000350 RID: 848
		private static readonly ProfilerMarker startClothUpdatePrePareProfiler = new ProfilerMarker("StartClothUpdate.Prepare");

		// Token: 0x04000351 RID: 849
		private static readonly ProfilerMarker startClothUpdateScheduleProfiler = new ProfilerMarker("StartClothUpdate.Schedule");
	}
}
