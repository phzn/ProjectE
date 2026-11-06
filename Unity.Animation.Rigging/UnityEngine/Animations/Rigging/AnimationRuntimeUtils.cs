using System;
using Unity.Collections;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000074 RID: 116
	public static class AnimationRuntimeUtils
	{
		// Token: 0x060002EF RID: 751 RVA: 0x000098AC File Offset: 0x00007AAC
		public static void SolveTwoBoneIK(AnimationStream stream, ReadWriteTransformHandle root, ReadWriteTransformHandle mid, ReadWriteTransformHandle tip, ReadOnlyTransformHandle target, ReadOnlyTransformHandle hint, float posWeight, float rotWeight, float hintWeight, AffineTransform targetOffset)
		{
			Vector3 position = root.GetPosition(stream);
			Vector3 vector = mid.GetPosition(stream);
			Vector3 position2 = tip.GetPosition(stream);
			Vector3 vector2;
			Quaternion quaternion;
			target.GetGlobalTR(stream, out vector2, out quaternion);
			Vector3 vector3 = Vector3.Lerp(position2, vector2 + targetOffset.translation, posWeight);
			Quaternion quaternion2 = Quaternion.Lerp(tip.GetRotation(stream), quaternion * targetOffset.rotation, rotWeight);
			bool flag = hint.IsValid(stream) && hintWeight > 0f;
			Vector3 vector4 = vector - position;
			Vector3 vector5 = position2 - vector;
			Vector3 vector6 = position2 - position;
			Vector3 vector7 = vector3 - position;
			float magnitude = vector4.magnitude;
			float magnitude2 = vector5.magnitude;
			float magnitude3 = vector6.magnitude;
			float magnitude4 = vector7.magnitude;
			float num = AnimationRuntimeUtils.TriangleAngle(magnitude3, magnitude, magnitude2);
			float num2 = AnimationRuntimeUtils.TriangleAngle(magnitude4, magnitude, magnitude2);
			Vector3 vector8 = Vector3.Cross(vector4, vector5);
			if (vector8.sqrMagnitude < 1E-08f)
			{
				vector8 = (flag ? Vector3.Cross(hint.GetPosition(stream) - position, vector5) : Vector3.zero);
				if (vector8.sqrMagnitude < 1E-08f)
				{
					vector8 = Vector3.Cross(vector7, vector5);
				}
				if (vector8.sqrMagnitude < 1E-08f)
				{
					vector8 = Vector3.up;
				}
			}
			vector8 = Vector3.Normalize(vector8);
			float num3 = 0.5f * (num - num2);
			float num4 = Mathf.Sin(num3);
			float num5 = Mathf.Cos(num3);
			Quaternion quaternion3 = new Quaternion(vector8.x * num4, vector8.y * num4, vector8.z * num4, num5);
			mid.SetRotation(stream, quaternion3 * mid.GetRotation(stream));
			vector6 = tip.GetPosition(stream) - position;
			root.SetRotation(stream, QuaternionExt.FromToRotation(vector6, vector7) * root.GetRotation(stream));
			if (flag)
			{
				float sqrMagnitude = vector6.sqrMagnitude;
				if (sqrMagnitude > 0f)
				{
					vector = mid.GetPosition(stream);
					Vector3 position3 = tip.GetPosition(stream);
					vector4 = vector - position;
					vector6 = position3 - position;
					Vector3 vector9 = vector6 / Mathf.Sqrt(sqrMagnitude);
					Vector3 vector10 = hint.GetPosition(stream) - position;
					Vector3 vector11 = vector4 - vector9 * Vector3.Dot(vector4, vector9);
					Vector3 vector12 = vector10 - vector9 * Vector3.Dot(vector10, vector9);
					float num6 = magnitude + magnitude2;
					if (vector11.sqrMagnitude > num6 * num6 * 0.001f && vector12.sqrMagnitude > 0f)
					{
						Quaternion quaternion4 = QuaternionExt.FromToRotation(vector11, vector12);
						quaternion4.x *= hintWeight;
						quaternion4.y *= hintWeight;
						quaternion4.z *= hintWeight;
						quaternion4 = QuaternionExt.NormalizeSafe(quaternion4);
						root.SetRotation(stream, quaternion4 * root.GetRotation(stream));
					}
				}
			}
			tip.SetRotation(stream, quaternion2);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00009B98 File Offset: 0x00007D98
		public static void InverseSolveTwoBoneIK(AnimationStream stream, ReadOnlyTransformHandle root, ReadOnlyTransformHandle mid, ReadOnlyTransformHandle tip, ReadWriteTransformHandle target, ReadWriteTransformHandle hint, float posWeight, float rotWeight, float hintWeight, AffineTransform targetOffset)
		{
			Vector3 position = root.GetPosition(stream);
			Vector3 position2 = mid.GetPosition(stream);
			Vector3 vector;
			Quaternion quaternion;
			tip.GetGlobalTR(stream, out vector, out quaternion);
			Vector3 vector2;
			Quaternion quaternion2;
			target.GetGlobalTR(stream, out vector2, out quaternion2);
			bool flag = hint.IsValid(stream);
			Vector3 vector3 = Vector3.zero;
			if (flag)
			{
				vector3 = hint.GetPosition(stream);
			}
			AnimationRuntimeUtils.InverseSolveTwoBoneIK(position, position2, vector, quaternion, ref vector2, ref quaternion2, ref vector3, flag, posWeight, rotWeight, hintWeight, targetOffset);
			target.SetPosition(stream, vector2);
			target.SetRotation(stream, quaternion2);
			hint.SetPosition(stream, vector3);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00009C24 File Offset: 0x00007E24
		public static void InverseSolveTwoBoneIK(Vector3 rootPosition, Vector3 midPosition, Vector3 tipPosition, Quaternion tipRotation, ref Vector3 targetPosition, ref Quaternion targetRotation, ref Vector3 hintPosition, bool isHintValid, float posWeight, float rotWeight, float hintWeight, AffineTransform targetOffset)
		{
			targetPosition = ((posWeight > 0f) ? (tipPosition + targetOffset.translation) : targetPosition);
			targetRotation = ((rotWeight > 0f) ? (tipRotation * targetOffset.rotation) : targetRotation);
			if (isHintValid)
			{
				Vector3 vector = tipPosition - rootPosition;
				Vector3 vector2 = midPosition - rootPosition;
				Vector3 vector3 = tipPosition - midPosition;
				float magnitude = vector2.magnitude;
				float magnitude2 = vector3.magnitude;
				float num = Vector3.Dot(vector, vector);
				Vector3 vector4 = rootPosition;
				if (num > 1E-08f)
				{
					vector4 += Vector3.Dot(vector2 / num, vector) * vector;
				}
				Vector3 vector5 = midPosition - vector4;
				float num2 = magnitude + magnitude2;
				hintPosition = ((hintWeight > 0f) ? (vector4 + vector5.normalized * num2) : hintPosition);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009D19 File Offset: 0x00007F19
		private static float TriangleAngle(float aLen, float aLen1, float aLen2)
		{
			return Mathf.Acos(Mathf.Clamp((aLen1 * aLen1 + aLen2 * aLen2 - aLen * aLen) / (aLen1 * aLen2) / 2f, -1f, 1f));
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00009D44 File Offset: 0x00007F44
		public static bool SolveFABRIK(ref NativeArray<Vector3> linkPositions, ref NativeArray<float> linkLengths, Vector3 target, float tolerance, float maxReach, int maxIterations)
		{
			Vector3 vector = target - linkPositions[0];
			if (vector.sqrMagnitude > AnimationRuntimeUtils.Square(maxReach))
			{
				Vector3 normalized = vector.normalized;
				for (int i = 1; i < linkPositions.Length; i++)
				{
					linkPositions[i] = linkPositions[i - 1] + normalized * linkLengths[i - 1];
				}
				return true;
			}
			int num = linkPositions.Length - 1;
			float num2 = AnimationRuntimeUtils.Square(tolerance);
			if (AnimationRuntimeUtils.SqrDistance(linkPositions[num], target) > num2)
			{
				Vector3 vector2 = linkPositions[0];
				int num3 = 0;
				do
				{
					linkPositions[num] = target;
					for (int j = num - 1; j > -1; j--)
					{
						linkPositions[j] = linkPositions[j + 1] + (linkPositions[j] - linkPositions[j + 1]).normalized * linkLengths[j];
					}
					linkPositions[0] = vector2;
					for (int k = 1; k < linkPositions.Length; k++)
					{
						linkPositions[k] = linkPositions[k - 1] + (linkPositions[k] - linkPositions[k - 1]).normalized * linkLengths[k - 1];
					}
				}
				while (AnimationRuntimeUtils.SqrDistance(linkPositions[num], target) > num2 && ++num3 < maxIterations);
				return true;
			}
			return false;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009EC4 File Offset: 0x000080C4
		public static float SqrDistance(Vector3 lhs, Vector3 rhs)
		{
			return (rhs - lhs).sqrMagnitude;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00009EE0 File Offset: 0x000080E0
		public static float Square(float value)
		{
			return value * value;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00009EE5 File Offset: 0x000080E5
		public static Vector3 Lerp(Vector3 a, Vector3 b, Vector3 t)
		{
			return Vector3.Scale(a, Vector3.one - t) + Vector3.Scale(b, t);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00009F04 File Offset: 0x00008104
		public static float Select(float a, float b, float c)
		{
			if (c <= 0f)
			{
				return a;
			}
			return b;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00009F14 File Offset: 0x00008114
		public static Vector3 Select(Vector3 a, Vector3 b, Vector3 c)
		{
			return new Vector3(AnimationRuntimeUtils.Select(a.x, b.x, c.x), AnimationRuntimeUtils.Select(a.y, b.y, c.y), AnimationRuntimeUtils.Select(a.z, b.z, c.z));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00009F6C File Offset: 0x0000816C
		public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
		{
			float num = Vector3.Dot(planeNormal, planeNormal);
			float num2 = Vector3.Dot(vector, planeNormal);
			return new Vector3(vector.x - planeNormal.x * num2 / num, vector.y - planeNormal.y * num2 / num, vector.z - planeNormal.z * num2 / num);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00009FC4 File Offset: 0x000081C4
		internal static float Sum(AnimationJobCache cache, CacheIndex index, int count)
		{
			if (count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				num += cache.GetRaw(index, i);
			}
			return num;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00009FFC File Offset: 0x000081FC
		public static float Sum(NativeArray<float> floatBuffer)
		{
			if (floatBuffer.Length == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < floatBuffer.Length; i++)
			{
				num += floatBuffer[i];
			}
			return num;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A03C File Offset: 0x0000823C
		public static void PassThrough(AnimationStream stream, ReadWriteTransformHandle handle)
		{
			Vector3 vector;
			Quaternion quaternion;
			Vector3 vector2;
			handle.GetLocalTRS(stream, out vector, out quaternion, out vector2);
			handle.SetLocalTRS(stream, vector, quaternion, vector2, false);
		}

		// Token: 0x04000137 RID: 311
		private const float k_SqrEpsilon = 1E-08f;
	}
}
