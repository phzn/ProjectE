using System;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000015 RID: 21
	[Serializable]
	internal class AudioMixerProperties : PlayableBehaviour
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00006680 File Offset: 0x00004880
		public override void PrepareFrame(Playable playable, FrameData info)
		{
			if (!playable.IsValid<Playable>() || !playable.IsPlayableOfType<AudioMixerPlayable>())
			{
				return;
			}
			int inputCount = playable.GetInputCount<Playable>();
			for (int i = 0; i < inputCount; i++)
			{
				if (playable.GetInputWeight(i) > 0f)
				{
					Playable input = playable.GetInput(i);
					if (input.IsValid<Playable>() && input.IsPlayableOfType<AudioClipPlayable>())
					{
						AudioClipPlayable audioClipPlayable = (AudioClipPlayable)input;
						AudioClipProperties @object = input.GetHandle().GetObject<AudioClipProperties>();
						audioClipPlayable.SetVolume(Mathf.Clamp01(this.volume * @object.volume));
						audioClipPlayable.SetStereoPan(Mathf.Clamp(this.stereoPan, -1f, 1f));
						audioClipPlayable.SetSpatialBlend(Mathf.Clamp01(this.spatialBlend));
					}
				}
			}
		}

		// Token: 0x04000087 RID: 135
		[Range(0f, 1f)]
		public float volume = 1f;

		// Token: 0x04000088 RID: 136
		[Range(-1f, 1f)]
		public float stereoPan;

		// Token: 0x04000089 RID: 137
		[Range(0f, 1f)]
		public float spatialBlend;
	}
}
