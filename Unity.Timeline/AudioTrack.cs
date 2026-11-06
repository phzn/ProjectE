using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000017 RID: 23
	[TrackClipType(typeof(AudioPlayableAsset), false)]
	[TrackBindingType(typeof(AudioSource))]
	[ExcludeFromPreset]
	[Serializable]
	public class AudioTrack : TrackAsset
	{
		// Token: 0x0600019A RID: 410 RVA: 0x0000684C File Offset: 0x00004A4C
		public TimelineClip CreateClip(AudioClip clip)
		{
			if (clip == null)
			{
				return null;
			}
			TimelineClip timelineClip = base.CreateDefaultClip();
			AudioPlayableAsset audioPlayableAsset = timelineClip.asset as AudioPlayableAsset;
			if (audioPlayableAsset != null)
			{
				audioPlayableAsset.clip = clip;
			}
			timelineClip.duration = (double)clip.length;
			timelineClip.displayName = clip.name;
			return timelineClip;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000068A0 File Offset: 0x00004AA0
		internal override Playable CompileClips(PlayableGraph graph, GameObject go, IList<TimelineClip> timelineClips, IntervalTree<RuntimeElement> tree)
		{
			AudioMixerPlayable audioMixerPlayable = AudioMixerPlayable.Create(graph, timelineClips.Count, false);
			if (base.hasCurves)
			{
				audioMixerPlayable.GetHandle().SetScriptInstance(this.m_TrackProperties.Clone());
			}
			for (int i = 0; i < timelineClips.Count; i++)
			{
				TimelineClip timelineClip = timelineClips[i];
				PlayableAsset playableAsset = timelineClip.asset as PlayableAsset;
				if (!(playableAsset == null))
				{
					float num = 0.1f;
					AudioPlayableAsset audioPlayableAsset = timelineClip.asset as AudioPlayableAsset;
					if (audioPlayableAsset != null)
					{
						num = audioPlayableAsset.bufferingTime;
					}
					Playable playable = playableAsset.CreatePlayable(graph, go);
					if (playable.IsValid<Playable>())
					{
						if (playable.IsPlayableOfType<AudioClipPlayable>())
						{
							AudioClipPlayable audioClipPlayable = (AudioClipPlayable)playable;
							AudioClipProperties @object = audioClipPlayable.GetHandle().GetObject<AudioClipProperties>();
							audioClipPlayable.SetVolume(Mathf.Clamp01(this.m_TrackProperties.volume * @object.volume));
							audioClipPlayable.SetStereoPan(Mathf.Clamp(this.m_TrackProperties.stereoPan, -1f, 1f));
							audioClipPlayable.SetSpatialBlend(Mathf.Clamp01(this.m_TrackProperties.spatialBlend));
						}
						tree.Add(new ScheduleRuntimeClip(timelineClip, playable, audioMixerPlayable, (double)num, 0.1));
						graph.Connect<Playable, AudioMixerPlayable>(playable, 0, audioMixerPlayable, i);
						playable.SetSpeed(timelineClip.timeScale);
						playable.SetDuration(timelineClip.extrapolatedDuration);
						audioMixerPlayable.SetInputWeight(playable, 1f);
					}
				}
			}
			base.ConfigureTrackAnimation(tree, go, audioMixerPlayable);
			return audioMixerPlayable;
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006A38 File Offset: 0x00004C38
		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				yield return AudioPlayableBinding.Create(base.name, this);
				yield break;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006A48 File Offset: 0x00004C48
		private void OnValidate()
		{
			this.m_TrackProperties.volume = Mathf.Clamp01(this.m_TrackProperties.volume);
			this.m_TrackProperties.stereoPan = Mathf.Clamp(this.m_TrackProperties.stereoPan, -1f, 1f);
			this.m_TrackProperties.spatialBlend = Mathf.Clamp01(this.m_TrackProperties.spatialBlend);
		}

		// Token: 0x0400008E RID: 142
		[SerializeField]
		private AudioMixerProperties m_TrackProperties = new AudioMixerProperties();
	}
}
