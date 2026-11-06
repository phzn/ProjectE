using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000072 RID: 114
	[RequireComponent(typeof(ParticleSystem))]
	public class MMRuntimeParticleControl : MonoBehaviour
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000CB5E File Offset: 0x0000AD5E
		protected virtual void Awake()
		{
			this._particleSystem = base.GetComponent<ParticleSystem>();
			this._mainModule = this._particleSystem.main;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000CB7D File Offset: 0x0000AD7D
		protected virtual void Play()
		{
			this._particleSystem.Play();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000CB8A File Offset: 0x0000AD8A
		protected virtual void Pause()
		{
			this._particleSystem.Pause();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000CB97 File Offset: 0x0000AD97
		protected virtual void Stop()
		{
			this._particleSystem.Stop();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
		protected virtual void Simulate()
		{
			this._particleSystem.Simulate(this.TargetTimestamp, true, true);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		protected void OnValidate()
		{
			float num = ((this.TrackerMode == MMRuntimeParticleControl.TrackerModes.Basic) ? 0f : this.MinBound);
			float num2 = ((this.TrackerMode == MMRuntimeParticleControl.TrackerModes.Basic) ? this._mainModule.duration : this.MaxBound);
			this.Timestamp = MMMaths.Remap(this.Tracker, 0f, 1f, num, num2);
			this._particleSystem.Simulate(this.Timestamp, true, true);
		}

		// Token: 0x040003CD RID: 973
		[Header("Base Controls")]
		[MMInspectorButton("Play")]
		public bool PlayButton;

		// Token: 0x040003CE RID: 974
		[MMInspectorButton("Pause")]
		public bool PauseButton;

		// Token: 0x040003CF RID: 975
		[MMInspectorButton("Stop")]
		public bool StopButton;

		// Token: 0x040003D0 RID: 976
		[Header("Simulate")]
		public float TargetTimestamp = 1f;

		// Token: 0x040003D1 RID: 977
		[MMInspectorButton("Simulate")]
		public bool FastForwardToTimeButton;

		// Token: 0x040003D2 RID: 978
		[Header("Tracker")]
		public MMRuntimeParticleControl.TrackerModes TrackerMode;

		// Token: 0x040003D3 RID: 979
		[MMEnumCondition("TrackerMode", new int[] { 1 })]
		public float MinBound;

		// Token: 0x040003D4 RID: 980
		[MMEnumCondition("TrackerMode", new int[] { 1 })]
		public float MaxBound;

		// Token: 0x040003D5 RID: 981
		[Range(0f, 1f)]
		public float Tracker;

		// Token: 0x040003D6 RID: 982
		[MMReadOnly]
		public float Timestamp;

		// Token: 0x040003D7 RID: 983
		protected ParticleSystem _particleSystem;

		// Token: 0x040003D8 RID: 984
		protected ParticleSystem.MainModule _mainModule;

		// Token: 0x020003E6 RID: 998
		public enum TrackerModes
		{
			// Token: 0x04001E58 RID: 7768
			Basic,
			// Token: 0x04001E59 RID: 7769
			ForcedBounds
		}
	}
}
