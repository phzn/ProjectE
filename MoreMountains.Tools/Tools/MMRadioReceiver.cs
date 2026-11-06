using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000129 RID: 297
	[MMRequiresConstantRepaint]
	public class MMRadioReceiver : MMMonoBehaviour
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x0002194E File Offset: 0x0001FB4E
		protected virtual void Awake()
		{
			this.Receiver.Initialization(base.gameObject);
			if (!this._listeningToEvents && this.CanListen)
			{
				this.StartListening();
			}
			this.GenerateRandomLevelMultiplier();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002197D File Offset: 0x0001FB7D
		public virtual void GenerateRandomLevelMultiplier()
		{
			if (this.RandomizeLevel)
			{
				this._randomLevelMultiplier = Random.Range(this.MinRandomLevelMultiplier, this.MaxRandomLevelMultiplier);
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002199E File Offset: 0x0001FB9E
		public virtual void SetLevel(float newLevel)
		{
			this.Receiver.SetLevel(newLevel);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000219AC File Offset: 0x0001FBAC
		protected virtual void OnRadioLevelEvent(int channel, float level)
		{
			if (channel != this.Channel)
			{
				return;
			}
			if (this.RandomizeLevel)
			{
				level *= this._randomLevelMultiplier;
			}
			this.SetLevel(level);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000219D1 File Offset: 0x0001FBD1
		protected virtual void OnDestroy()
		{
			this._listeningToEvents = false;
			this.StopListening();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000219E0 File Offset: 0x0001FBE0
		public virtual void StartListening()
		{
			this._listeningToEvents = true;
			MMRadioLevelEvent.Register(new MMRadioLevelEvent.Delegate(this.OnRadioLevelEvent));
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000219FB File Offset: 0x0001FBFB
		public virtual void StopListening()
		{
			this._listeningToEvents = false;
			MMRadioLevelEvent.Unregister(new MMRadioLevelEvent.Delegate(this.OnRadioLevelEvent));
		}

		// Token: 0x04000857 RID: 2135
		[Header("Target")]
		public MMPropertyReceiver Receiver;

		// Token: 0x04000858 RID: 2136
		[Header("Channel")]
		public bool CanListen = true;

		// Token: 0x04000859 RID: 2137
		[MMCondition("CanListen", true)]
		public int Channel;

		// Token: 0x0400085A RID: 2138
		[Header("Modifiers")]
		public bool RandomizeLevel;

		// Token: 0x0400085B RID: 2139
		[MMCondition("RandomizeLevel", true)]
		public float MinRandomLevelMultiplier;

		// Token: 0x0400085C RID: 2140
		[MMCondition("RandomizeLevel", true)]
		public float MaxRandomLevelMultiplier = 1f;

		// Token: 0x0400085D RID: 2141
		protected bool _listeningToEvents;

		// Token: 0x0400085E RID: 2142
		protected float _randomLevelMultiplier = 1f;

		// Token: 0x0400085F RID: 2143
		protected float _lastLevel;
	}
}
