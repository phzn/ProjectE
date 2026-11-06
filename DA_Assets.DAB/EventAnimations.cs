using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000012 RID: 18
	[Serializable]
	public struct EventAnimations
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000030B6 File Offset: 0x000012B6
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000030BE File Offset: 0x000012BE
		public AnimationItem Pressed
		{
			get
			{
				return this.pressed;
			}
			set
			{
				this.pressed = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000030C7 File Offset: 0x000012C7
		// (set) Token: 0x0600006C RID: 108 RVA: 0x000030CF File Offset: 0x000012CF
		public AnimationItem Highlighted
		{
			get
			{
				return this.highlighted;
			}
			set
			{
				this.highlighted = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000030D8 File Offset: 0x000012D8
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000030E0 File Offset: 0x000012E0
		public AnimationItem Disabled
		{
			get
			{
				return this.disabled;
			}
			set
			{
				this.disabled = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000030E9 File Offset: 0x000012E9
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000030F1 File Offset: 0x000012F1
		public AnimationItem Looped
		{
			get
			{
				return this.looped;
			}
			set
			{
				this.looped = value;
			}
		}

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private AnimationItem pressed;

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private AnimationItem highlighted;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private AnimationItem disabled;

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private AnimationItem looped;
	}
}
