using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	// Token: 0x02000019 RID: 25
	[RequireComponent(typeof(AudioBehaviour))]
	public sealed class AudioDistortionFilter : Behaviour
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000F2 RID: 242
		// (set) Token: 0x060000F3 RID: 243
		public extern float distortionLevel
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
