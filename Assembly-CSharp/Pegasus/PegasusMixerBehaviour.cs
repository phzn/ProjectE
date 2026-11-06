using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Pegasus
{
	// Token: 0x020001B8 RID: 440
	public class PegasusMixerBehaviour : PlayableBehaviour
	{
		// Token: 0x06000D16 RID: 3350 RVA: 0x0005A648 File Offset: 0x00058848
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			int inputCount = playable.GetInputCount<Playable>();
			float num = 0f;
			float num2 = 0f;
			PegasusManager pegasusManager = null;
			for (int i = 0; i < inputCount; i++)
			{
				float inputWeight = playable.GetInputWeight(i);
				PegasusBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour();
				if (pegasusManager == null)
				{
					pegasusManager = behaviour.pegasusManager;
				}
				num += behaviour.pegasusProgress * inputWeight;
				num2 += inputWeight;
			}
			if (!Mathf.Approximately(num2, 0f) && pegasusManager != null)
			{
				pegasusManager.MoveTargetTo(num);
			}
		}
	}
}
