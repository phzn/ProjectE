using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Audio
{
	// Token: 0x0200002E RID: 46
	[NativeHeader("Modules/Audio/Public/AudioMixer.h")]
	[ExcludeFromObjectFactory]
	[ExcludeFromPreset]
	[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioMixer.bindings.h")]
	public class AudioMixer : Object
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x000039F5 File Offset: 0x00001BF5
		internal AudioMixer()
		{
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001F9 RID: 505
		// (set) Token: 0x060001FA RID: 506
		[NativeProperty]
		public extern AudioMixerGroup outputAudioMixerGroup
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060001FB RID: 507
		[NativeMethod("FindSnapshotFromName")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AudioMixerSnapshot FindSnapshot(string name);

		// Token: 0x060001FC RID: 508
		[NativeMethod("AudioMixerBindings::FindMatchingGroups", IsFreeFunction = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AudioMixerGroup[] FindMatchingGroups(string subPath);

		// Token: 0x060001FD RID: 509 RVA: 0x00003A00 File Offset: 0x00001C00
		internal void TransitionToSnapshot(AudioMixerSnapshot snapshot, float timeToReach)
		{
			bool flag = snapshot == null;
			if (flag)
			{
				throw new ArgumentException("null Snapshot passed to AudioMixer.TransitionToSnapshot of AudioMixer '" + base.name + "'");
			}
			bool flag2 = snapshot.audioMixer != this;
			if (flag2)
			{
				throw new ArgumentException(string.Concat(new string[] { "Snapshot '", snapshot.name, "' passed to AudioMixer.TransitionToSnapshot is not a snapshot from AudioMixer '", base.name, "'" }));
			}
			this.TransitionToSnapshotInternal(snapshot, timeToReach);
		}

		// Token: 0x060001FE RID: 510
		[NativeMethod("TransitionToSnapshot")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TransitionToSnapshotInternal(AudioMixerSnapshot snapshot, float timeToReach);

		// Token: 0x060001FF RID: 511
		[NativeMethod("AudioMixerBindings::TransitionToSnapshots", IsFreeFunction = true, HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TransitionToSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float timeToReach);

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000200 RID: 512
		// (set) Token: 0x06000201 RID: 513
		[NativeProperty]
		public extern AudioMixerUpdateMode updateMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000202 RID: 514
		[NativeMethod]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetFloat(string name, float value);

		// Token: 0x06000203 RID: 515
		[NativeMethod]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ClearFloat(string name);

		// Token: 0x06000204 RID: 516
		[NativeMethod]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetFloat(string name, out float value);

		// Token: 0x06000205 RID: 517
		[NativeMethod("AudioMixerBindings::GetAbsoluteAudibilityFromGroup", HasExplicitThis = true, IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetAbsoluteAudibilityFromGroup(AudioMixerGroup group);
	}
}
