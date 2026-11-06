using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine.Audio
{
	// Token: 0x0200002F RID: 47
	[NativeHeader("Modules/Audio/Public/AudioMixerGroup.h")]
	public class AudioMixerGroup : Object, ISubAssetNotDuplicatable
	{
		// Token: 0x06000206 RID: 518 RVA: 0x000039F5 File Offset: 0x00001BF5
		internal AudioMixerGroup()
		{
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000207 RID: 519
		[NativeProperty]
		public extern AudioMixer audioMixer
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
