using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000006 RID: 6
	internal class AnimationOutputWeightProcessor : ITimelineEvaluateCallback
	{
		// Token: 0x06000014 RID: 20 RVA: 0x000022AD File Offset: 0x000004AD
		public AnimationOutputWeightProcessor(AnimationPlayableOutput output)
		{
			this.m_Output = output;
			output.SetWeight(0f);
			this.FindMixers();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000022D8 File Offset: 0x000004D8
		private void FindMixers()
		{
			Playable sourcePlayable = this.m_Output.GetSourcePlayable<AnimationPlayableOutput>();
			int sourceOutputPort = this.m_Output.GetSourceOutputPort<AnimationPlayableOutput>();
			this.m_Mixers.Clear();
			this.FindMixers(sourcePlayable, sourceOutputPort, sourcePlayable.GetInput(sourceOutputPort));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002318 File Offset: 0x00000518
		private void FindMixers(Playable parent, int port, Playable node)
		{
			if (!node.IsValid<Playable>())
			{
				return;
			}
			Type playableType = node.GetPlayableType();
			if (playableType == typeof(AnimationMixerPlayable) || playableType == typeof(AnimationLayerMixerPlayable))
			{
				int inputCount = node.GetInputCount<Playable>();
				for (int i = 0; i < inputCount; i++)
				{
					this.FindMixers(node, i, node.GetInput(i));
				}
				AnimationOutputWeightProcessor.WeightInfo weightInfo = new AnimationOutputWeightProcessor.WeightInfo
				{
					parentMixer = parent,
					mixer = node,
					port = port
				};
				this.m_Mixers.Add(weightInfo);
				return;
			}
			int inputCount2 = node.GetInputCount<Playable>();
			for (int j = 0; j < inputCount2; j++)
			{
				this.FindMixers(parent, port, node.GetInput(j));
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000023D8 File Offset: 0x000005D8
		public void Evaluate()
		{
			float num = 1f;
			this.m_Output.SetWeight(1f);
			for (int i = 0; i < this.m_Mixers.Count; i++)
			{
				AnimationOutputWeightProcessor.WeightInfo weightInfo = this.m_Mixers[i];
				num = WeightUtility.NormalizeMixer(weightInfo.mixer);
				weightInfo.parentMixer.SetInputWeight(weightInfo.port, num);
			}
			if (Application.isPlaying)
			{
				this.m_Output.SetWeight(num);
			}
		}

		// Token: 0x04000006 RID: 6
		private AnimationPlayableOutput m_Output;

		// Token: 0x04000007 RID: 7
		private AnimationMotionXToDeltaPlayable m_MotionXPlayable;

		// Token: 0x04000008 RID: 8
		private readonly List<AnimationOutputWeightProcessor.WeightInfo> m_Mixers = new List<AnimationOutputWeightProcessor.WeightInfo>();

		// Token: 0x02000059 RID: 89
		private struct WeightInfo
		{
			// Token: 0x04000117 RID: 279
			public Playable mixer;

			// Token: 0x04000118 RID: 280
			public Playable parentMixer;

			// Token: 0x04000119 RID: 281
			public int port;
		}
	}
}
