using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000127 RID: 295
	[MMRequiresConstantRepaint]
	public class MMRadioBroadcaster : MMMonoBehaviour
	{
		// Token: 0x0600069E RID: 1694 RVA: 0x0002180C File Offset: 0x0001FA0C
		protected virtual void Awake()
		{
			this.Emitter.Initialization(base.gameObject);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002181F File Offset: 0x0001FA1F
		protected virtual void Update()
		{
			this.ProcessBroadcast();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00021828 File Offset: 0x0001FA28
		protected virtual void ProcessBroadcast()
		{
			if (this.Emitter == null)
			{
				return;
			}
			float level = this.Emitter.GetLevel();
			if (level != this._levelLastFrame)
			{
				MMRadioBroadcaster.OnValueChangeDelegate onValueChange = this.OnValueChange;
				if (onValueChange != null)
				{
					onValueChange();
				}
				foreach (MMRadioReceiver mmradioReceiver in this.Receivers)
				{
					if (mmradioReceiver != null)
					{
						mmradioReceiver.SetLevel(level);
					}
				}
				if (this.BroadcastOnChannel)
				{
					MMRadioLevelEvent.Trigger(this.Channel, level);
				}
			}
			this._levelLastFrame = level;
		}

		// Token: 0x0400084F RID: 2127
		[Header("Source")]
		public MMPropertyEmitter Emitter;

		// Token: 0x04000850 RID: 2128
		[Header("Destinations")]
		public MMRadioReceiver[] Receivers;

		// Token: 0x04000851 RID: 2129
		[Header("Channel Broadcasting")]
		public bool BroadcastOnChannel = true;

		// Token: 0x04000852 RID: 2130
		[MMCondition("BroadcastOnChannel", true)]
		public int Channel;

		// Token: 0x04000853 RID: 2131
		[MMCondition("BroadcastOnChannel", true)]
		public bool OnlyBroadcastOnValueChange = true;

		// Token: 0x04000854 RID: 2132
		public MMRadioBroadcaster.OnValueChangeDelegate OnValueChange;

		// Token: 0x04000855 RID: 2133
		protected float _levelLastFrame;

		// Token: 0x02000440 RID: 1088
		// (Invoke) Token: 0x06001A2E RID: 6702
		public delegate void OnValueChangeDelegate();
	}
}
