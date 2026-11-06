using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Audio
{
	// Token: 0x0200002B RID: 43
	[NativeHeader("Modules/Audio/Public/ScriptBindings/Audio.bindings.h")]
	internal sealed class AudioManagerTestProxy
	{
		// Token: 0x060001CC RID: 460
		[NativeMethod(Name = "AudioManagerTestProxy::ComputeAudibilityConsistency", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ComputeAudibilityConsistency();
	}
}
