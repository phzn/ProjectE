using System;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000AF RID: 175
	public class TimeManager : IManager, IDisposable, IValid
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0001D623 File Offset: 0x0001B823
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0001D62B File Offset: 0x0001B82B
		internal int FixedUpdateCount { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0001D634 File Offset: 0x0001B834
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0001D63C File Offset: 0x0001B83C
		internal float SimulationDeltaTime { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0001D645 File Offset: 0x0001B845
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0001D64D File Offset: 0x0001B84D
		internal float MaxDeltaTime { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0001D656 File Offset: 0x0001B856
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0001D65E File Offset: 0x0001B85E
		internal float4 SimulationPower { get; private set; }

		// Token: 0x060002D4 RID: 724 RVA: 0x0001D668 File Offset: 0x0001B868
		public void Dispose()
		{
			this.isValid = true;
			this.GlobalTimeScale = 1f;
			this.FixedUpdateCount = 0;
			this.SimulationPower = 1f;
			MagicaManager.afterFixedUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterFixedUpdateDelegate, new MagicaManager.UpdateMethod(this.AfterFixedUpdate));
			MagicaManager.afterRenderingDelegate = (MagicaManager.UpdateMethod)Delegate.Remove(MagicaManager.afterRenderingDelegate, new MagicaManager.UpdateMethod(this.AfterRenderring));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0001D6DE File Offset: 0x0001B8DE
		public void EnterdEditMode()
		{
			this.Dispose();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001D6E8 File Offset: 0x0001B8E8
		public void Initialize()
		{
			this.GlobalTimeScale = 1f;
			this.FixedUpdateCount = 0;
			this.SimulationPower = 1f;
			MagicaManager.afterFixedUpdateDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterFixedUpdateDelegate, new MagicaManager.UpdateMethod(this.AfterFixedUpdate));
			MagicaManager.afterRenderingDelegate = (MagicaManager.UpdateMethod)Delegate.Combine(MagicaManager.afterRenderingDelegate, new MagicaManager.UpdateMethod(this.AfterRenderring));
			this.isValid = true;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001D75E File Offset: 0x0001B95E
		public bool IsValid()
		{
			return this.isValid;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001D768 File Offset: 0x0001B968
		private void AfterFixedUpdate()
		{
			int fixedUpdateCount = this.FixedUpdateCount;
			this.FixedUpdateCount = fixedUpdateCount + 1;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001D785 File Offset: 0x0001B985
		private void AfterRenderring()
		{
			this.FixedUpdateCount = 0;
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001D790 File Offset: 0x0001B990
		internal void FrameUpdate()
		{
			this.simulationFrequency = Mathf.Clamp(this.simulationFrequency, 30, 150);
			this.maxSimulationCountPerFrame = Mathf.Clamp(this.maxSimulationCountPerFrame, 1, 5);
			this.GlobalTimeScale = Mathf.Clamp01(this.GlobalTimeScale);
			this.SimulationDeltaTime = 1f / (float)this.simulationFrequency;
			this.MaxDeltaTime = this.SimulationDeltaTime * (float)this.maxSimulationCountPerFrame;
			float num = 90f / (float)this.simulationFrequency;
			this.SimulationPower = new float4(num, (num > 1f) ? Mathf.Pow(num, 0.5f) : num, (num > 1f) ? Mathf.Pow(num, 0.3f) : num, Mathf.Pow(num, 1.8f));
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001D854 File Offset: 0x0001BA54
		public void InformationLog(StringBuilder allsb)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("========== Time Manager ==========");
			if (!this.IsValid())
			{
				stringBuilder.AppendLine("Time Manager. Invalid");
			}
			else
			{
				stringBuilder.AppendLine(string.Format("SimulationFrequency:{0}", this.simulationFrequency));
				stringBuilder.AppendLine(string.Format("MaxSimulationCountPerFrame:{0}", this.maxSimulationCountPerFrame));
				stringBuilder.AppendLine(string.Format("GlobalTimeScale:{0}", this.GlobalTimeScale));
				stringBuilder.AppendLine(string.Format("SimulationDeltaTime:{0}", this.SimulationDeltaTime));
				stringBuilder.AppendLine(string.Format("SimulationPower:{0}", this.SimulationPower));
			}
			stringBuilder.AppendLine();
			Debug.Log(stringBuilder.ToString());
			allsb.Append(stringBuilder);
		}

		// Token: 0x0400062E RID: 1582
		internal int simulationFrequency = 90;

		// Token: 0x0400062F RID: 1583
		internal int maxSimulationCountPerFrame = 3;

		// Token: 0x04000630 RID: 1584
		internal TimeManager.UpdateLocation updateLocation;

		// Token: 0x04000631 RID: 1585
		private bool isValid;

		// Token: 0x04000633 RID: 1587
		internal float GlobalTimeScale = 1f;

		// Token: 0x020000B0 RID: 176
		public enum UpdateLocation
		{
			// Token: 0x04000638 RID: 1592
			AfterLateUpdate,
			// Token: 0x04000639 RID: 1593
			BeforeLateUpdate
		}
	}
}
