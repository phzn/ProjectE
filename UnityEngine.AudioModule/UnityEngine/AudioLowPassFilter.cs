using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000017 RID: 23
	[RequireComponent(typeof(AudioBehaviour))]
	public sealed class AudioLowPassFilter : Behaviour
	{
		// Token: 0x060000E4 RID: 228
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationCurve GetCustomLowpassLevelCurveCopy();

		// Token: 0x060000E5 RID: 229
		[NativeMethod(Name = "AudioLowPassFilterBindings::SetCustomLowpassLevelCurveHelper", IsFreeFunction = true)]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCustomLowpassLevelCurveHelper([NotNull("NullExceptionObject")] AudioLowPassFilter source, AnimationCurve curve);

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00002AA0 File Offset: 0x00000CA0
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00002AB8 File Offset: 0x00000CB8
		public AnimationCurve customCutoffCurve
		{
			get
			{
				return this.GetCustomLowpassLevelCurveCopy();
			}
			set
			{
				AudioLowPassFilter.SetCustomLowpassLevelCurveHelper(this, value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E8 RID: 232
		// (set) Token: 0x060000E9 RID: 233
		public extern float cutoffFrequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000EA RID: 234
		// (set) Token: 0x060000EB RID: 235
		public extern float lowpassResonanceQ
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
