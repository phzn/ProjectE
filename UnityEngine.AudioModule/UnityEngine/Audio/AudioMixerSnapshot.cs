using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine.Audio
{
	// Token: 0x02000031 RID: 49
	[NativeHeader("Modules/Audio/Public/AudioMixerSnapshot.h")]
	public class AudioMixerSnapshot : Object, ISubAssetNotDuplicatable
	{
		// Token: 0x06000210 RID: 528 RVA: 0x000039F5 File Offset: 0x00001BF5
		internal AudioMixerSnapshot()
		{
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000211 RID: 529
		[NativeProperty]
		public extern AudioMixer audioMixer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public void TransitionTo(float timeToReach)
		{
			this.audioMixer.TransitionToSnapshot(this, timeToReach);
		}
	}
}
