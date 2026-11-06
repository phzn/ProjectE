using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000065 RID: 101
	public static class Damper
	{
		// Token: 0x060003DC RID: 988 RVA: 0x00017592 File Offset: 0x00015792
		private static float DecayConstant(float time, float residual)
		{
			return Mathf.Log(1f / residual) / time;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000175A2 File Offset: 0x000157A2
		private static float DecayedRemainder(float initial, float decayConstant, float deltaTime)
		{
			return initial / Mathf.Exp(decayConstant * deltaTime);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000175B0 File Offset: 0x000157B0
		public static float Damp(float initial, float dampTime, float deltaTime)
		{
			if (dampTime < 0.0001f || Mathf.Abs(initial) < 0.0001f)
			{
				return initial;
			}
			if (deltaTime < 0.0001f)
			{
				return 0f;
			}
			float num = 4.6051702f / dampTime;
			return initial * (1f - Mathf.Exp(-num * deltaTime));
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000175FC File Offset: 0x000157FC
		public static Vector3 Damp(Vector3 initial, Vector3 dampTime, float deltaTime)
		{
			for (int i = 0; i < 3; i++)
			{
				initial[i] = Damper.Damp(initial[i], dampTime[i], deltaTime);
			}
			return initial;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00017634 File Offset: 0x00015834
		public static Vector3 Damp(Vector3 initial, float dampTime, float deltaTime)
		{
			for (int i = 0; i < 3; i++)
			{
				initial[i] = Damper.Damp(initial[i], dampTime, deltaTime);
			}
			return initial;
		}

		// Token: 0x04000297 RID: 663
		private const float Epsilon = 0.0001f;

		// Token: 0x04000298 RID: 664
		public const float kNegligibleResidual = 0.01f;

		// Token: 0x04000299 RID: 665
		private const float kLogNegligibleResidual = -4.6051702f;
	}
}
