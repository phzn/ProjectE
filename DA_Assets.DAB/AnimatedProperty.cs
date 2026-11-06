using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public struct AnimatedProperty<T>
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000079 RID: 121 RVA: 0x0000313E File Offset: 0x0000133E
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003146 File Offset: 0x00001346
		public T Pressed
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000314F File Offset: 0x0000134F
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003157 File Offset: 0x00001357
		public T Highlighted
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

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003160 File Offset: 0x00001360
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003168 File Offset: 0x00001368
		public T Disabled
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

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003171 File Offset: 0x00001371
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003179 File Offset: 0x00001379
		public T Looped
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

		// Token: 0x04000041 RID: 65
		[SerializeField]
		private T pressed;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private T highlighted;

		// Token: 0x04000043 RID: 67
		[SerializeField]
		private T disabled;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private T looped;
	}
}
