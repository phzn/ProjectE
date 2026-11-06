using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Playables;
using UnityEngine.Scripting;

namespace UnityEngine.Audio
{
	// Token: 0x0200002C RID: 44
	[RequiredByNativeCode]
	[StaticAccessor("AudioClipPlayableBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Director/Core/HPlayable.h")]
	[NativeHeader("Modules/Audio/Public/Director/AudioClipPlayable.h")]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioClipPlayable.bindings.h")]
	public struct AudioClipPlayable : IPlayable, IEquatable<AudioClipPlayable>
	{
		// Token: 0x060001CE RID: 462 RVA: 0x00003540 File Offset: 0x00001740
		public static AudioClipPlayable Create(PlayableGraph graph, AudioClip clip, bool looping)
		{
			PlayableHandle playableHandle = AudioClipPlayable.CreateHandle(graph, clip, looping);
			AudioClipPlayable audioClipPlayable = new AudioClipPlayable(playableHandle);
			bool flag = clip != null;
			if (flag)
			{
				audioClipPlayable.SetDuration((double)clip.length);
			}
			return audioClipPlayable;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00003580 File Offset: 0x00001780
		private static PlayableHandle CreateHandle(PlayableGraph graph, AudioClip clip, bool looping)
		{
			PlayableHandle @null = PlayableHandle.Null;
			bool flag = !AudioClipPlayable.InternalCreateAudioClipPlayable(ref graph, clip, looping, ref @null);
			PlayableHandle playableHandle;
			if (flag)
			{
				playableHandle = PlayableHandle.Null;
			}
			else
			{
				playableHandle = @null;
			}
			return playableHandle;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x000035B4 File Offset: 0x000017B4
		internal AudioClipPlayable(PlayableHandle handle)
		{
			bool flag = handle.IsValid();
			if (flag)
			{
				bool flag2 = !handle.IsPlayableOfType<AudioClipPlayable>();
				if (flag2)
				{
					throw new InvalidCastException("Can't set handle: the playable is not an AudioClipPlayable.");
				}
			}
			this.m_Handle = handle;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000035F0 File Offset: 0x000017F0
		public PlayableHandle GetHandle()
		{
			return this.m_Handle;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00003608 File Offset: 0x00001808
		public static implicit operator Playable(AudioClipPlayable playable)
		{
			return new Playable(playable.GetHandle());
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00003628 File Offset: 0x00001828
		public static explicit operator AudioClipPlayable(Playable playable)
		{
			return new AudioClipPlayable(playable.GetHandle());
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00003648 File Offset: 0x00001848
		public bool Equals(AudioClipPlayable other)
		{
			return this.GetHandle() == other.GetHandle();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000366C File Offset: 0x0000186C
		public AudioClip GetClip()
		{
			return AudioClipPlayable.GetClipInternal(ref this.m_Handle);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00003689 File Offset: 0x00001889
		public void SetClip(AudioClip value)
		{
			AudioClipPlayable.SetClipInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000369C File Offset: 0x0000189C
		public bool GetLooped()
		{
			return AudioClipPlayable.GetLoopedInternal(ref this.m_Handle);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000036B9 File Offset: 0x000018B9
		public void SetLooped(bool value)
		{
			AudioClipPlayable.SetLoopedInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000036CC File Offset: 0x000018CC
		internal float GetVolume()
		{
			return AudioClipPlayable.GetVolumeInternal(ref this.m_Handle);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000036EC File Offset: 0x000018EC
		internal void SetVolume(float value)
		{
			bool flag = value < 0f || value > 1f;
			if (flag)
			{
				throw new ArgumentException("Trying to set AudioClipPlayable volume outside of range (0.0 - 1.0): " + value.ToString());
			}
			AudioClipPlayable.SetVolumeInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00003738 File Offset: 0x00001938
		internal float GetStereoPan()
		{
			return AudioClipPlayable.GetStereoPanInternal(ref this.m_Handle);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00003758 File Offset: 0x00001958
		internal void SetStereoPan(float value)
		{
			bool flag = value < -1f || value > 1f;
			if (flag)
			{
				throw new ArgumentException("Trying to set AudioClipPlayable stereo pan outside of range (-1.0 - 1.0): " + value.ToString());
			}
			AudioClipPlayable.SetStereoPanInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000037A4 File Offset: 0x000019A4
		internal float GetSpatialBlend()
		{
			return AudioClipPlayable.GetSpatialBlendInternal(ref this.m_Handle);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000037C4 File Offset: 0x000019C4
		internal void SetSpatialBlend(float value)
		{
			bool flag = value < 0f || value > 1f;
			if (flag)
			{
				throw new ArgumentException("Trying to set AudioClipPlayable spatial blend outside of range (0.0 - 1.0): " + value.ToString());
			}
			AudioClipPlayable.SetSpatialBlendInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00003810 File Offset: 0x00001A10
		[Obsolete("IsPlaying() has been deprecated. Use IsChannelPlaying() instead (UnityUpgradable) -> IsChannelPlaying()", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsPlaying()
		{
			return this.IsChannelPlaying();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsChannelPlaying()
		{
			return AudioClipPlayable.GetIsChannelPlayingInternal(ref this.m_Handle);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00003848 File Offset: 0x00001A48
		public double GetStartDelay()
		{
			return AudioClipPlayable.GetStartDelayInternal(ref this.m_Handle);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00003865 File Offset: 0x00001A65
		internal void SetStartDelay(double value)
		{
			AudioClipPlayable.SetStartDelayInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00003878 File Offset: 0x00001A78
		public double GetPauseDelay()
		{
			return AudioClipPlayable.GetPauseDelayInternal(ref this.m_Handle);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00003898 File Offset: 0x00001A98
		internal void GetPauseDelay(double value)
		{
			double pauseDelayInternal = AudioClipPlayable.GetPauseDelayInternal(ref this.m_Handle);
			bool flag = this.m_Handle.GetPlayState() == PlayState.Playing && (value < 0.05 || (pauseDelayInternal != 0.0 && pauseDelayInternal < 0.05));
			if (flag)
			{
				throw new ArgumentException("AudioClipPlayable.pauseDelay: Setting new delay when existing delay is too small or 0.0 (" + pauseDelayInternal.ToString() + "), audio system will not be able to change in time");
			}
			AudioClipPlayable.SetPauseDelayInternal(ref this.m_Handle, value);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000391A File Offset: 0x00001B1A
		public void Seek(double startTime, double startDelay)
		{
			this.Seek(startTime, startDelay, 0.0);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00003930 File Offset: 0x00001B30
		public void Seek(double startTime, double startDelay, [DefaultValue("0")] double duration)
		{
			AudioClipPlayable.SetStartDelayInternal(ref this.m_Handle, startDelay);
			bool flag = duration > 0.0;
			if (flag)
			{
				double num = startDelay + duration;
				bool flag2 = num >= this.m_Handle.GetDuration();
				if (flag2)
				{
					this.m_Handle.SetDone(true);
				}
				this.m_Handle.SetDuration(duration + startTime);
				AudioClipPlayable.SetPauseDelayInternal(ref this.m_Handle, startDelay + duration);
			}
			else
			{
				this.m_Handle.SetDone(true);
				this.m_Handle.SetDuration(double.MaxValue);
				AudioClipPlayable.SetPauseDelayInternal(ref this.m_Handle, 0.0);
			}
			this.m_Handle.SetTime(startTime);
			this.m_Handle.Play();
		}

		// Token: 0x060001E7 RID: 487
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioClip GetClipInternal(ref PlayableHandle hdl);

		// Token: 0x060001E8 RID: 488
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetClipInternal(ref PlayableHandle hdl, AudioClip clip);

		// Token: 0x060001E9 RID: 489
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetLoopedInternal(ref PlayableHandle hdl);

		// Token: 0x060001EA RID: 490
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLoopedInternal(ref PlayableHandle hdl, bool looped);

		// Token: 0x060001EB RID: 491
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetVolumeInternal(ref PlayableHandle hdl);

		// Token: 0x060001EC RID: 492
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetVolumeInternal(ref PlayableHandle hdl, float volume);

		// Token: 0x060001ED RID: 493
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetStereoPanInternal(ref PlayableHandle hdl);

		// Token: 0x060001EE RID: 494
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStereoPanInternal(ref PlayableHandle hdl, float stereoPan);

		// Token: 0x060001EF RID: 495
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float GetSpatialBlendInternal(ref PlayableHandle hdl);

		// Token: 0x060001F0 RID: 496
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSpatialBlendInternal(ref PlayableHandle hdl, float spatialBlend);

		// Token: 0x060001F1 RID: 497
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsChannelPlayingInternal(ref PlayableHandle hdl);

		// Token: 0x060001F2 RID: 498
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetStartDelayInternal(ref PlayableHandle hdl);

		// Token: 0x060001F3 RID: 499
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStartDelayInternal(ref PlayableHandle hdl, double delay);

		// Token: 0x060001F4 RID: 500
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern double GetPauseDelayInternal(ref PlayableHandle hdl);

		// Token: 0x060001F5 RID: 501
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPauseDelayInternal(ref PlayableHandle hdl, double delay);

		// Token: 0x060001F6 RID: 502
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalCreateAudioClipPlayable(ref PlayableGraph graph, AudioClip clip, bool looping, ref PlayableHandle handle);

		// Token: 0x060001F7 RID: 503
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ValidateType(ref PlayableHandle hdl);

		// Token: 0x04000073 RID: 115
		private PlayableHandle m_Handle;
	}
}
