using System;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x0200028D RID: 653
	public static class MovementUtilities
	{
		// Token: 0x0600104F RID: 4175 RVA: 0x00062AC8 File Offset: 0x00060CC8
		public static float FilterRotationDirection(ref Vector2 state, ref Vector2 state2, Vector2 deltaPosition, float threshold, float deltaTime, bool avoidingOtherAgents)
		{
			Vector2 vector = state;
			if (!avoidingOtherAgents)
			{
				state += deltaPosition * 10f;
			}
			else
			{
				state += deltaPosition;
			}
			state *= Mathf.Clamp01(1f - deltaTime * 0.5f);
			float num = state.magnitude;
			if (num > threshold * 2f)
			{
				state *= threshold * 2f / num;
				num = threshold * 2f;
			}
			state2 += (state - vector) * 0.5f;
			state2 *= Mathf.Clamp01(1f - deltaTime * 0.5f);
			if (num <= threshold)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00062BC4 File Offset: 0x00060DC4
		public static Vector2 ClampVelocity(Vector2 velocity, float maxSpeed, float speedLimitFactor, bool slowWhenNotFacingTarget, bool preventMovingBackwards, Vector2 forward)
		{
			float num = maxSpeed * speedLimitFactor;
			if (!slowWhenNotFacingTarget || (forward.x == 0f && forward.y == 0f))
			{
				return Vector2.ClampMagnitude(velocity, num);
			}
			float num2;
			Vector2 vector = VectorMath.Normalize(velocity, out num2);
			float num3 = Vector2.Dot(vector, forward);
			float num4 = Mathf.Clamp(num3 + 0.707f, 0.2f, 1f);
			num *= num4;
			num2 = Mathf.Min(num2, num);
			if (preventMovingBackwards)
			{
				float num5 = Mathf.Min(Mathf.Acos(Mathf.Clamp(num3, -1f, 1f)), (20f + 180f * (1f - speedLimitFactor * speedLimitFactor)) * 0.017453292f);
				float num6 = Mathf.Sin(num5);
				float num7 = Mathf.Cos(num5);
				num6 *= Mathf.Sign(vector.x * forward.y - vector.y * forward.x);
				return new Vector2(forward.x * num7 + forward.y * num6, forward.y * num7 - forward.x * num6) * num2;
			}
			return vector * num2;
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00062CE8 File Offset: 0x00060EE8
		public static Vector2 CalculateAccelerationToReachPoint(Vector2 deltaPosition, Vector2 targetVelocity, Vector2 currentVelocity, float forwardsAcceleration, float rotationSpeed, float maxSpeed, Vector2 forwardsVector)
		{
			if (forwardsAcceleration <= 0f)
			{
				return Vector2.zero;
			}
			float magnitude = currentVelocity.magnitude;
			float num = magnitude * rotationSpeed * 0.017453292f;
			num = Mathf.Max(num, forwardsAcceleration);
			deltaPosition = VectorMath.ComplexMultiplyConjugate(deltaPosition, forwardsVector);
			targetVelocity = VectorMath.ComplexMultiplyConjugate(targetVelocity, forwardsVector);
			currentVelocity = VectorMath.ComplexMultiplyConjugate(currentVelocity, forwardsVector);
			float num2 = 1f / (forwardsAcceleration * forwardsAcceleration);
			float num3 = 1f / (num * num);
			if (targetVelocity == Vector2.zero)
			{
				float num4 = 0.01f;
				float num5 = 10f;
				while (num5 - num4 > 0.01f)
				{
					float num6 = (num5 + num4) * 0.5f;
					Vector2 vector = (6f * deltaPosition - 4f * num6 * currentVelocity) / (num6 * num6);
					Vector2 vector2 = 6f * (num6 * currentVelocity - 2f * deltaPosition) / (num6 * num6 * num6);
					Vector2 vector3 = vector + vector2 * num6;
					if (vector.x * vector.x * num2 + vector.y * vector.y * num3 > 1f || vector3.x * vector3.x * num2 + vector3.y * vector3.y * num3 > 1f)
					{
						num4 = num6;
					}
					else
					{
						num5 = num6;
					}
				}
				Vector2 vector4 = (6f * deltaPosition - 4f * num5 * currentVelocity) / (num5 * num5);
				vector4.y *= 2f;
				float num7 = vector4.x * vector4.x * num2 + vector4.y * vector4.y * num3;
				if (num7 > 1f)
				{
					vector4 /= Mathf.Sqrt(num7);
				}
				return VectorMath.ComplexMultiply(vector4, forwardsVector);
			}
			float num8;
			Vector2 vector5 = VectorMath.Normalize(targetVelocity, out num8);
			float magnitude2 = deltaPosition.magnitude;
			Vector2 vector6 = ((deltaPosition - vector5 * Math.Min(0.5f * magnitude2 * num8 / (magnitude + num8), maxSpeed * 1.5f)).normalized * maxSpeed - currentVelocity) * 10f;
			float num9 = vector6.x * vector6.x * num2 + vector6.y * vector6.y * num3;
			if (num9 > 1f)
			{
				vector6 /= Mathf.Sqrt(num9);
			}
			return VectorMath.ComplexMultiply(vector6, forwardsVector);
		}
	}
}
