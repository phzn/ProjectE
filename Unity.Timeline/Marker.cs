using System;

namespace UnityEngine.Timeline
{
	// Token: 0x02000027 RID: 39
	public abstract class Marker : ScriptableObject, IMarker
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000830E File Offset: 0x0000650E
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00008316 File Offset: 0x00006516
		public TrackAsset parent { get; private set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000831F File Offset: 0x0000651F
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00008327 File Offset: 0x00006527
		public double time
		{
			get
			{
				return this.m_Time;
			}
			set
			{
				this.m_Time = Math.Max(value, 0.0);
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008340 File Offset: 0x00006540
		void IMarker.Initialize(TrackAsset parentTrack)
		{
			if (this.parent == null)
			{
				this.parent = parentTrack;
				try
				{
					this.OnInitialize(parentTrack);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message, this);
				}
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008388 File Offset: 0x00006588
		public virtual void OnInitialize(TrackAsset aPent)
		{
		}

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		[TimeField(TimeFieldAttribute.UseEditMode.ApplyEditMode)]
		[Tooltip("Time for the marker")]
		private double m_Time;
	}
}
