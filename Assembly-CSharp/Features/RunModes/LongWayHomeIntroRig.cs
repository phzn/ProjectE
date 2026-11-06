using System;
using Features.PrisonIntro;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CD0 RID: 3280
	public sealed class LongWayHomeIntroRig : MonoBehaviour
	{
		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06005F59 RID: 24409 RVA: 0x0025C9A5 File Offset: 0x0025ABA5
		public PrisonIntroSequence Sequence
		{
			get
			{
				return this.sequence;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06005F5A RID: 24410 RVA: 0x0025C9AD File Offset: 0x0025ABAD
		public AudioClip VoiceClip
		{
			get
			{
				return this.voiceClip;
			}
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06005F5B RID: 24411 RVA: 0x0025C9B5 File Offset: 0x0025ABB5
		public AudioSource VoiceOutput
		{
			get
			{
				return this.voiceOutput;
			}
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06005F5C RID: 24412 RVA: 0x0025C9BD File Offset: 0x0025ABBD
		public Vector3 SourcePlayerPosition
		{
			get
			{
				return this.sourcePlayerPosition;
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06005F5D RID: 24413 RVA: 0x0025C9C5 File Offset: 0x0025ABC5
		public Quaternion SourcePlayerRotation
		{
			get
			{
				return Quaternion.Euler(this.sourcePlayerEulerAngles);
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06005F5E RID: 24414 RVA: 0x0025C9D2 File Offset: 0x0025ABD2
		public Vector3 PhoneGroundOffset
		{
			get
			{
				return this.phoneGroundOffset;
			}
		}

		// Token: 0x040060CC RID: 24780
		[SerializeField]
		private PrisonIntroSequence sequence;

		// Token: 0x040060CD RID: 24781
		[SerializeField]
		private AudioClip voiceClip;

		// Token: 0x040060CE RID: 24782
		[SerializeField]
		private AudioSource voiceOutput;

		// Token: 0x040060CF RID: 24783
		[SerializeField]
		private Vector3 sourcePlayerPosition = new Vector3(14.192f, 0.086f, 10.66f);

		// Token: 0x040060D0 RID: 24784
		[SerializeField]
		private Vector3 sourcePlayerEulerAngles = new Vector3(0f, -93.343f, 0f);

		// Token: 0x040060D1 RID: 24785
		[Tooltip("Ground position beside Ciel's head in the authored opening close-up, relative to her spawn heading.")]
		[SerializeField]
		private Vector3 phoneGroundOffset = new Vector3(-0.25f, 0f, -1.3f);
	}
}
