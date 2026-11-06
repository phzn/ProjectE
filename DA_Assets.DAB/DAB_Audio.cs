using System;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public struct DAB_Audio
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003072 File Offset: 0x00001272
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000307A File Offset: 0x0000127A
		public AudioSource AudioSource
		{
			get
			{
				return this.audioSource;
			}
			set
			{
				this.audioSource = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003083 File Offset: 0x00001283
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000308B File Offset: 0x0000128B
		public AudioClip OnPointerClick
		{
			get
			{
				return this.onPointerClick;
			}
			set
			{
				this.onPointerClick = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00003094 File Offset: 0x00001294
		// (set) Token: 0x06000066 RID: 102 RVA: 0x0000309C File Offset: 0x0000129C
		public AudioClip OnPointerEnter
		{
			get
			{
				return this.onPointerEnter;
			}
			set
			{
				this.onPointerEnter = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000030A5 File Offset: 0x000012A5
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000030AD File Offset: 0x000012AD
		public AudioClip OnPointerExit
		{
			get
			{
				return this.onPointerExit;
			}
			set
			{
				this.onPointerExit = value;
			}
		}

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private AudioClip onPointerClick;

		// Token: 0x0400002B RID: 43
		[SerializeField]
		private AudioClip onPointerEnter;

		// Token: 0x0400002C RID: 44
		[SerializeField]
		private AudioClip onPointerExit;
	}
}
