using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	// Token: 0x02000018 RID: 24
	[RequireComponent(typeof(AudioBehaviour))]
	public sealed class AudioHighPassFilter : Behaviour
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000ED RID: 237
		// (set) Token: 0x060000EE RID: 238
		public extern float cutoffFrequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000EF RID: 239
		// (set) Token: 0x060000F0 RID: 240
		public extern float highpassResonanceQ
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
