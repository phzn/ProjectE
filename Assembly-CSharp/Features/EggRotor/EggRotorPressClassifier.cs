using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x0200122F RID: 4655
	public static class EggRotorPressClassifier
	{
		// Token: 0x06009090 RID: 37008 RVA: 0x003B32C0 File Offset: 0x003B14C0
		public static EggRotorPressResult Step(ref EggRotorPressState state, bool pressedDown, bool held, bool allowed, float now, float holdSeconds = 0.45f)
		{
			if (!allowed || float.IsNaN(now) || float.IsInfinity(now))
			{
				state = default(EggRotorPressState);
				return EggRotorPressResult.None;
			}
			if (pressedDown)
			{
				state.Active = true;
				state.HoldFired = false;
				state.StartTime = now;
			}
			if (!state.Active)
			{
				return EggRotorPressResult.None;
			}
			float num = (float.IsNaN(holdSeconds) ? 0.45f : Mathf.Max(0f, holdSeconds));
			float num2 = Mathf.Max(0f, now - state.StartTime);
			if (!held)
			{
				bool holdFired = state.HoldFired;
				state = default(EggRotorPressState);
				if (holdFired)
				{
					return EggRotorPressResult.None;
				}
				if (num2 < num)
				{
					return EggRotorPressResult.Tap;
				}
				return EggRotorPressResult.Hold;
			}
			else
			{
				if (state.HoldFired || num2 < num)
				{
					return EggRotorPressResult.None;
				}
				state.HoldFired = true;
				return EggRotorPressResult.Hold;
			}
		}
	}
}
