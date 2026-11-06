using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000082 RID: 130
	[AddComponentMenu("MagicaCloth2/MagicaSettings")]
	[HelpURL("https://magicasoft.jp/en/mc2_settings_component/")]
	public class MagicaSettings : ClothBehaviour
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00011C3B File Offset: 0x0000FE3B
		protected void Awake()
		{
			if (this.refreshMode == MagicaSettings.RefreshMode.OnAwake)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00011C4B File Offset: 0x0000FE4B
		protected void Start()
		{
			if (this.refreshMode == MagicaSettings.RefreshMode.OnStart)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00011C5C File Offset: 0x0000FE5C
		protected void Update()
		{
			if (this.refreshMode == MagicaSettings.RefreshMode.EveryFrame)
			{
				this.Refresh();
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00011C6D File Offset: 0x0000FE6D
		protected void OnValidate()
		{
			if (Application.isPlaying)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00011C7C File Offset: 0x0000FE7C
		public void Refresh()
		{
			if (MagicaManager.IsPlaying())
			{
				this.simulationFrequency = Mathf.Clamp(this.simulationFrequency, 30, 150);
				this.maxSimulationCountPerFrame = Mathf.Clamp(this.maxSimulationCountPerFrame, 1, 5);
				MagicaManager.SetSimulationFrequency(this.simulationFrequency);
				MagicaManager.SetMaxSimulationCountPerFrame(this.maxSimulationCountPerFrame);
				MagicaManager.SetInitializationLocation(this.initializationLocation);
				MagicaManager.SetUpdateLocation(this.updateLocation);
				MagicaManager.SetSplitProxyMeshVertexCount(this.splitProxyMeshVertexCount);
				if (this.monitorPlayerLoop)
				{
					MagicaManager.InitCustomGameLoop();
					return;
				}
			}
			else
			{
				object obj = "MagicaManager is not starting!";
				Develop.LogError(in obj);
			}
		}

		// Token: 0x0400037C RID: 892
		public MagicaSettings.RefreshMode refreshMode;

		// Token: 0x0400037D RID: 893
		[Range(30f, 150f)]
		public int simulationFrequency = 90;

		// Token: 0x0400037E RID: 894
		[Range(1f, 5f)]
		public int maxSimulationCountPerFrame = 3;

		// Token: 0x0400037F RID: 895
		public MagicaManager.InitializationLocation initializationLocation;

		// Token: 0x04000380 RID: 896
		public TimeManager.UpdateLocation updateLocation;

		// Token: 0x04000381 RID: 897
		public bool monitorPlayerLoop;

		// Token: 0x04000382 RID: 898
		[Min(0f)]
		public int splitProxyMeshVertexCount = 300;

		// Token: 0x02000083 RID: 131
		public enum RefreshMode
		{
			// Token: 0x04000384 RID: 900
			OnAwake,
			// Token: 0x04000385 RID: 901
			EveryFrame,
			// Token: 0x04000386 RID: 902
			OnStart,
			// Token: 0x04000387 RID: 903
			Manual
		}
	}
}
