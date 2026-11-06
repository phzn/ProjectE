using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x0200002C RID: 44
	[CustomStyle("SignalEmitter")]
	[ExcludeFromPreset]
	[Serializable]
	public class SignalEmitter : Marker, INotification, INotificationOptionProvider
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000086C7 File Offset: 0x000068C7
		// (set) Token: 0x06000249 RID: 585 RVA: 0x000086CF File Offset: 0x000068CF
		public bool retroactive
		{
			get
			{
				return this.m_Retroactive;
			}
			set
			{
				this.m_Retroactive = value;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000086D8 File Offset: 0x000068D8
		// (set) Token: 0x0600024B RID: 587 RVA: 0x000086E0 File Offset: 0x000068E0
		public bool emitOnce
		{
			get
			{
				return this.m_EmitOnce;
			}
			set
			{
				this.m_EmitOnce = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000086E9 File Offset: 0x000068E9
		// (set) Token: 0x0600024D RID: 589 RVA: 0x000086F1 File Offset: 0x000068F1
		public SignalAsset asset
		{
			get
			{
				return this.m_Asset;
			}
			set
			{
				this.m_Asset = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000086FA File Offset: 0x000068FA
		PropertyName INotification.id
		{
			get
			{
				if (this.m_Asset != null)
				{
					return new PropertyName(this.m_Asset.name);
				}
				return new PropertyName(string.Empty);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00008725 File Offset: 0x00006925
		NotificationFlags INotificationOptionProvider.flags
		{
			get
			{
				return (this.retroactive ? NotificationFlags.Retroactive : ((NotificationFlags)0)) | (this.emitOnce ? NotificationFlags.TriggerOnce : ((NotificationFlags)0)) | NotificationFlags.TriggerInEditMode;
			}
		}

		// Token: 0x040000CD RID: 205
		[SerializeField]
		private bool m_Retroactive;

		// Token: 0x040000CE RID: 206
		[SerializeField]
		private bool m_EmitOnce;

		// Token: 0x040000CF RID: 207
		[SerializeField]
		private SignalAsset m_Asset;
	}
}
