using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	// Token: 0x02000030 RID: 48
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioMixerPlayable.bindings.h")]
	[NativeHeader("Modules/Audio/Public/Director/AudioMixerPlayable.h")]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[StaticAccessor("AudioMixerPlayableBindings", StaticAccessorType.DoubleColon)]
	[RequiredByNativeCode]
	public struct AudioMixerPlayable : IPlayable, IEquatable<AudioMixerPlayable>
	{
		// Token: 0x06000208 RID: 520 RVA: 0x00003A88 File Offset: 0x00001C88
		public static AudioMixerPlayable Create(PlayableGraph graph, int inputCount = 0, bool normalizeInputVolumes = false)
		{
			PlayableHandle playableHandle = AudioMixerPlayable.CreateHandle(graph, inputCount, normalizeInputVolumes);
			return new AudioMixerPlayable(playableHandle);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00003AAC File Offset: 0x00001CAC
		private static PlayableHandle CreateHandle(PlayableGraph graph, int inputCount, bool normalizeInputVolumes)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !AudioMixerPlayable.CreateAudioMixerPlayableInternal(ref graph, normalizeInputVolumes, ref @null);
			PlayableHandle playableHandle;
			if (flag)
			{
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				@null.SetInputCount(inputCount);
				playableHandle = @null;
			}
			return playableHandle;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00003AE8 File Offset: 0x00001CE8
		internal AudioMixerPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<AudioMixerPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AudioMixerPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00003B24 File Offset: 0x00001D24
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00003B3C File Offset: 0x00001D3C
		public static implicit operator Playable(AudioMixerPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00003B5C File Offset: 0x00001D5C
		public static explicit operator AudioMixerPlayable(Playable playable)
		{
			return new AudioMixerPlayable(playable.GetHandle());
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00003B7C File Offset: 0x00001D7C
		public bool Equals(AudioMixerPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x0600020F RID: 527
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CreateAudioMixerPlayableInternal(ref PlayableGraph graph, bool normalizeInputVolumes, ref PlayableHandle handle);

		// Token: 0x04000077 RID: 119
		private PlayableHandle m_Handle;
	}
}
