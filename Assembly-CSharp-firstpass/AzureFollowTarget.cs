using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	public sealed class AzureFollowTarget
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003009 File Offset: 0x00001209
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003011 File Offset: 0x00001211
		public Transform follower
		{
			get
			{
				return this.m_follower;
			}
			set
			{
				this.m_follower = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000301A File Offset: 0x0000121A
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00003022 File Offset: 0x00001222
		public Transform target
		{
			get
			{
				return this.m_target;
			}
			set
			{
				this.m_target = value;
			}
		}

		// Token: 0x0400006C RID: 108
		[SerializeField]
		private Transform m_follower;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private Transform m_target;
	}
}
