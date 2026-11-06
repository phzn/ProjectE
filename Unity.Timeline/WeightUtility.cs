using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	// Token: 0x02000055 RID: 85
	internal static class WeightUtility
	{
		// Token: 0x0600030F RID: 783 RVA: 0x0000B168 File Offset: 0x00009368
		public static float NormalizeMixer(Playable mixer)
		{
			if (!mixer.IsValid<Playable>())
			{
				return 0f;
			}
			int inputCount = mixer.GetInputCount<Playable>();
			float num = 0f;
			for (int i = 0; i < inputCount; i++)
			{
				num += mixer.GetInputWeight(i);
			}
			if (num > Mathf.Epsilon && num < 1f)
			{
				for (int j = 0; j < inputCount; j++)
				{
					mixer.SetInputWeight(j, mixer.GetInputWeight(j) / num);
				}
			}
			return Mathf.Clamp01(num);
		}
	}
}
