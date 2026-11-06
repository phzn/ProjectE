using System;
using Unity.Mathematics;

namespace Pathfinding.PID
{
	// Token: 0x02000262 RID: 610
	public static class AnglePIDController
	{
		// Token: 0x06000F17 RID: 3863 RVA: 0x0005DDEC File Offset: 0x0005BFEC
		public static float ApproximateTurningRadius(float followingStrength)
		{
			float num = 2f * math.sqrt(math.abs(followingStrength)) * 1f;
			return 1f / (num * 1.5707964f);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0005DE20 File Offset: 0x0005C020
		public static float RotationSpeedToFollowingStrength(float speed, float maxRotationSpeed)
		{
			float num = maxRotationSpeed / (6.2831855f * speed * 1f);
			return num * num;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0005DE33 File Offset: 0x0005C033
		public static float FollowingStrengthToRotationSpeed(float followingStrength)
		{
			return 1f / (AnglePIDController.ApproximateTurningRadius(followingStrength) * 0.5f);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0005DE48 File Offset: 0x0005C048
		public static AnglePIDControlOutput2D Control(ref PIDMovement settings, float followingStrength, float angle, float curveAngle, float curveCurvature, float curveDistanceSigned, float speed, float remainingDistance, float minRotationSpeed, bool isStationary, float dt)
		{
			float num = math.min(followingStrength, 10000f);
			float num2 = 2f * math.sqrt(math.abs(num)) * 1f;
			float num3 = 1f;
			float num4 = AstarMath.DeltaAngle(angle, curveAngle);
			float num5 = curveAngle + math.sign(curveDistanceSigned) * 3.1415927f * 0.5f;
			float num6 = AstarMath.DeltaAngle(angle, num5);
			float num7 = num * math.abs(curveDistanceSigned) * num6;
			float num8 = num7 * speed * dt;
			float num9 = num2 * num4;
			float num10 = num2 + num * math.abs(curveDistanceSigned);
			float num11 = ((num10 > 1.1754944E-38f) ? ((num9 + num7) / num10) : 0f);
			float.IsFinite(num11);
			isStationary = settings.allowRotatingOnSpot && (math.abs(num11) > 2.0941856f || (isStationary && math.abs(num11) > 0.1f));
			if (!isStationary)
			{
				speed = math.min(settings.Speed(remainingDistance), settings.Accelerate(speed, settings.slowdownTime, dt));
				if (math.abs(num4) > 1.5707964f)
				{
					num8 = 0f;
				}
				if (math.abs(num9) > 0.0001f)
				{
					num9 = math.max(math.abs(num9), minRotationSpeed) * math.sign(num9);
				}
				float num12 = num9 * speed * dt;
				float num13 = math.abs(num8 / num6);
				float num14 = math.abs(num12 / num4);
				float num15 = 1f;
				float num16 = math.max(0f, math.cos(num4));
				float num17 = 1f;
				float num18 = speed * num17 * dt;
				float num19 = curveCurvature * num18;
				float num20 = num3 * num19 * num16;
				float num21 = math.max(1f, math.max(num13, math.max(num14, num15)));
				float num22 = (num20 + num12 + num8) / num21;
				float num23 = math.radians(settings.maxRotationSpeed);
				float num24 = math.max(0.1f, math.min(1f, num23 * dt / math.abs(num22)));
				return new AnglePIDControlOutput2D(angle, angle + num11, num22 * num24, num18 * num24);
			}
			float num25 = settings.Accelerate(speed, settings.slowdownTimeWhenTurningOnSpot, -dt);
			float num26 = math.radians(settings.maxOnSpotRotationSpeed);
			bool flag = num26 * dt > math.abs(num11);
			if (num25 > 0f && !flag)
			{
				return AnglePIDControlOutput2D.WithMovementAtEnd(angle, angle, 0f, num25 * dt);
			}
			return AnglePIDControlOutput2D.WithMovementAtEnd(angle, angle + num11, math.clamp(num11, -num26 * dt, num26 * dt), flag ? (speed * dt) : 0f);
		}

		// Token: 0x04000B61 RID: 2913
		private const float DampingRatio = 1f;
	}
}
