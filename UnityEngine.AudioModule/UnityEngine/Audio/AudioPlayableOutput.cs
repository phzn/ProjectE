using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	// Token: 0x02000034 RID: 52
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioPlayableOutput.bindings.h")]
	[NativeHeader("Modules/Audio/Public/Director/AudioPlayableOutput.h")]
	[StaticAccessor("AudioPlayableOutputBindings", StaticAccessorType.DoubleColon)]
	[RequiredByNativeCode]
	[NativeHeader("Modules/Audio/Public/AudioSource.h")]
	public struct AudioPlayableOutput : IPlayableOutput
	{
		// Token: 0x06000216 RID: 534 RVA: 0x00003C04 File Offset: 0x00001E04
		public static AudioPlayableOutput Create(PlayableGraph graph, string name, AudioSource target)
		{
			PlayableOutputHandle playableOutputHandle;
			bool flag = !AudioPlayableGraphExtensions.InternalCreateAudioOutput(ref graph, name, out playableOutputHandle);
			AudioPlayableOutput audioPlayableOutput;
			if (flag)
			{
				audioPlayableOutput = AudioPlayableOutput.Null;
			}
			else
			{
				AudioPlayableOutput audioPlayableOutput2 = new AudioPlayableOutput(playableOutputHandle);
				audioPlayableOutput2.SetTarget(target);
				audioPlayableOutput = audioPlayableOutput2;
			}
			return audioPlayableOutput;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00003C44 File Offset: 0x00001E44
		internal AudioPlayableOutput(PlayableOutputHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOutputOfType<AudioPlayableOutput>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AudioPlayableOutput.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00003C80 File Offset: 0x00001E80
		public static AudioPlayableOutput Null
		{
			get
			{
				return new AudioPlayableOutput(PlayableOutputHandle.Null);
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00003C9C File Offset: 0x00001E9C
		public PlayableOutputHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public static implicit operator PlayableOutput(AudioPlayableOutput output)
		{
			return new PlayableOutput(output.GetHandle());
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00003CD4 File Offset: 0x00001ED4
		public static explicit operator AudioPlayableOutput(PlayableOutput output)
		{
			return new AudioPlayableOutput(output.GetHandle());
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public AudioSource GetTarget()
		{
			return AudioPlayableOutput.InternalGetTarget(ref this.m_Handle);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00003D11 File Offset: 0x00001F11
		public void SetTarget(AudioSource value)
		{
			AudioPlayableOutput.InternalSetTarget(ref this.m_Handle, value);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00003D24 File Offset: 0x00001F24
		public bool GetEvaluateOnSeek()
		{
			return AudioPlayableOutput.InternalGetEvaluateOnSeek(ref this.m_Handle);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00003D41 File Offset: 0x00001F41
		public void SetEvaluateOnSeek(bool value)
		{
			AudioPlayableOutput.InternalSetEvaluateOnSeek(ref this.m_Handle, value);
		}

		// Token: 0x06000220 RID: 544
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioSource InternalGetTarget(ref PlayableOutputHandle output);

		// Token: 0x06000221 RID: 545
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetTarget(ref PlayableOutputHandle output, AudioSource target);

		// Token: 0x06000222 RID: 546
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalGetEvaluateOnSeek(ref PlayableOutputHandle output);

		// Token: 0x06000223 RID: 547
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetEvaluateOnSeek(ref PlayableOutputHandle output, bool value);

		// Token: 0x04000078 RID: 120
		private PlayableOutputHandle m_Handle;
	}
}
