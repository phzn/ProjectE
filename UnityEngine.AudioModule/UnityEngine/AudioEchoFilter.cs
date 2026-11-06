using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	// Token: 0x0200001A RID: 26
	[RequireComponent(typeof(AudioBehaviour))]
	public sealed class AudioEchoFilter : Behaviour
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000F5 RID: 245
		// (set) Token: 0x060000F6 RID: 246
		public extern float delay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F7 RID: 247
		// (set) Token: 0x060000F8 RID: 248
		public extern float decayRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F9 RID: 249
		// (set) Token: 0x060000FA RID: 250
		public extern float dryMix
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000FB RID: 251
		// (set) Token: 0x060000FC RID: 252
		public extern float wetMix
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
