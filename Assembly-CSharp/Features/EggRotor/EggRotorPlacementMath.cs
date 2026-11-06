using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001233 RID: 4659
	public static class EggRotorPlacementMath
	{
		// Token: 0x060090A4 RID: 37028 RVA: 0x003B392F File Offset: 0x003B1B2F
		public static float Ease(float t)
		{
			t = EggRotorPlacementMath.Clamp01Finite(t);
			return t * t * (3f - 2f * t);
		}

		// Token: 0x060090A5 RID: 37029 RVA: 0x003B394C File Offset: 0x003B1B4C
		public static float Advance(float progress, float deltaTime, float seconds)
		{
			progress = EggRotorPlacementMath.Clamp01Finite(progress);
			if (!EggRotorPlacementMath.IsFinite(deltaTime) || deltaTime <= 0f)
			{
				return progress;
			}
			if (!EggRotorPlacementMath.IsFinite(seconds) || seconds <= 0.0001f)
			{
				return 1f;
			}
			return Mathf.Min(1f, progress + deltaTime / seconds);
		}

		// Token: 0x060090A6 RID: 37030 RVA: 0x003B3998 File Offset: 0x003B1B98
		public static float Retreat(float progress, float deltaTime, float seconds)
		{
			progress = EggRotorPlacementMath.Clamp01Finite(progress);
			if (!EggRotorPlacementMath.IsFinite(deltaTime) || deltaTime <= 0f)
			{
				return progress;
			}
			if (!EggRotorPlacementMath.IsFinite(seconds) || seconds <= 0.0001f)
			{
				return 0f;
			}
			return Mathf.Max(0f, progress - deltaTime / seconds);
		}

		// Token: 0x060090A7 RID: 37031 RVA: 0x003B39E4 File Offset: 0x003B1BE4
		public static float InsideOpening(float easedInsertion, float weight)
		{
			if (!EggRotorPlacementMath.IsFinite(weight) || weight <= 0f)
			{
				return 0f;
			}
			return Mathf.Min(weight, 100f) * EggRotorPlacementMath.Clamp01Finite(easedInsertion);
		}

		// Token: 0x060090A8 RID: 37032 RVA: 0x003B3A10 File Offset: 0x003B1C10
		public static float InsideOpening(float easedInsertion, float heldWeight, float peakWeight, float peakAt)
		{
			if (!EggRotorPlacementMath.IsFinite(heldWeight) || heldWeight <= 0f)
			{
				return 0f;
			}
			float num = Mathf.Min(heldWeight, 100f);
			float num2 = (EggRotorPlacementMath.IsFinite(peakWeight) ? Mathf.Clamp(peakWeight, num, 100f) : num);
			float num3 = EggRotorPlacementMath.Clamp01Finite(easedInsertion);
			float num4 = (EggRotorPlacementMath.IsFinite(peakAt) ? Mathf.Clamp(peakAt, 0.05f, 1f) : 1f);
			if (num3 <= num4)
			{
				return num2 * (num3 / num4);
			}
			return Mathf.Lerp(num2, num, EggRotorPlacementMath.Ease((num3 - num4) / (1f - num4)));
		}

		// Token: 0x060090A9 RID: 37033 RVA: 0x003B3AA0 File Offset: 0x003B1CA0
		public static float WidestPassesOpening(Pose entry, Pose inside, float eggLength, Vector3 opening, Vector3 canalAxis)
		{
			EggRotorPlacementMath.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.eggLength = eggLength;
			CS$<>8__locals1.opening = opening;
			if (CS$<>8__locals1.eggLength <= 0f || !EggRotorPlacementMath.IsFinite(canalAxis) || canalAxis.sqrMagnitude < 1E-10f)
			{
				return 1f;
			}
			CS$<>8__locals1.axis = canalAxis.normalized;
			float num = EggRotorPlacementMath.<WidestPassesOpening>g__Depth|5_0(entry, ref CS$<>8__locals1);
			float num2 = EggRotorPlacementMath.<WidestPassesOpening>g__Depth|5_0(inside, ref CS$<>8__locals1);
			float num3 = CS$<>8__locals1.eggLength * 0.5f;
			if (num2 <= num || num2 <= num3)
			{
				return 1f;
			}
			return Mathf.Clamp01((num3 - num) / (num2 - num));
		}

		// Token: 0x060090AA RID: 37034 RVA: 0x003B3B30 File Offset: 0x003B1D30
		public static void WrapTape(Vector3[] vertices, Vector3[] normals, Vector3[] outVertices, Vector3[] outNormals, float shaftRadius, float flatHalfWidth, float blendWidth)
		{
			if (vertices == null || outVertices == null)
			{
				return;
			}
			float num = (EggRotorPlacementMath.IsFinite(shaftRadius) ? Mathf.Max(0f, shaftRadius) : 0f);
			float num2 = (EggRotorPlacementMath.IsFinite(flatHalfWidth) ? Mathf.Max(0f, flatHalfWidth) : 0f);
			float num3 = (EggRotorPlacementMath.IsFinite(blendWidth) ? Mathf.Max(1E-05f, blendWidth) : 1E-05f);
			int num4 = 0;
			while (num4 < vertices.Length && num4 < outVertices.Length)
			{
				Vector3 vector = vertices[num4];
				float num6;
				float num5 = EggRotorPlacementMath.TapeDrop(Mathf.Abs(vector.x), num, num2, num3, out num6);
				outVertices[num4] = new Vector3(vector.x, vector.y - num5, vector.z);
				if (normals != null && outNormals != null && num4 < normals.Length && num4 < outNormals.Length)
				{
					Vector3 vector2 = normals[num4];
					Vector3 vector3 = new Vector3(vector2.x + num6 * Mathf.Sign(vector.x) * vector2.y, vector2.y, vector2.z);
					outNormals[num4] = ((vector3.sqrMagnitude > 1E-12f) ? vector3.normalized : vector2);
				}
				num4++;
			}
		}

		// Token: 0x060090AB RID: 37035 RVA: 0x003B3C6C File Offset: 0x003B1E6C
		public static float TapeDrop(float x, float shaftRadius, float flatHalfWidth, float blendWidth, out float slope)
		{
			slope = 0f;
			if (shaftRadius <= 0f || x <= flatHalfWidth)
			{
				return 0f;
			}
			float num = Mathf.Min(x, shaftRadius * 0.98f);
			float num2 = Mathf.Sqrt(Mathf.Max(1E-12f, shaftRadius * shaftRadius - num * num));
			float num3 = shaftRadius - num2;
			float num4 = ((x < shaftRadius * 0.98f) ? (num / num2) : 0f);
			float num5 = Mathf.Clamp01((x - flatHalfWidth) / Mathf.Max(1E-05f, blendWidth));
			float num6 = num5 * num5 * (3f - 2f * num5);
			float num7 = ((x - flatHalfWidth < blendWidth) ? (6f * num5 * (1f - num5) / Mathf.Max(1E-05f, blendWidth)) : 0f);
			slope = num7 * num3 + num6 * num4;
			return num6 * num3;
		}

		// Token: 0x060090AC RID: 37036 RVA: 0x003B3D40 File Offset: 0x003B1F40
		public static float FadeOpening(float current, float target, float deltaTime, float fadeSeconds, float fullWeight)
		{
			current = (EggRotorPlacementMath.IsFinite(current) ? Mathf.Clamp(current, 0f, 100f) : 0f);
			target = (EggRotorPlacementMath.IsFinite(target) ? Mathf.Clamp(target, 0f, 100f) : 0f);
			if (!EggRotorPlacementMath.IsFinite(fadeSeconds) || fadeSeconds <= 0.0001f)
			{
				return target;
			}
			if (!EggRotorPlacementMath.IsFinite(deltaTime) || deltaTime <= 0f)
			{
				return current;
			}
			float num = Mathf.Max(Mathf.Max(1f, EggRotorPlacementMath.IsFinite(fullWeight) ? fullWeight : 0f), current);
			return Mathf.MoveTowards(current, target, num / fadeSeconds * deltaTime);
		}

		// Token: 0x060090AD RID: 37037 RVA: 0x003B3DE2 File Offset: 0x003B1FE2
		public static bool UsesMuffledLoops(bool eggInside, bool covered)
		{
			return eggInside || covered;
		}

		// Token: 0x060090AE RID: 37038 RVA: 0x003B3DE7 File Offset: 0x003B1FE7
		public static Pose Blend(Pose from, Pose to, float t)
		{
			t = EggRotorPlacementMath.Clamp01Finite(t);
			return new Pose(Vector3.Lerp(from.position, to.position, t), Quaternion.Slerp(from.rotation, to.rotation, t));
		}

		// Token: 0x060090AF RID: 37039 RVA: 0x003B3E1A File Offset: 0x003B201A
		public static Vector3 BodyToLocalPosition(Transform parent, Vector3 bodyUnits, float bodyScale)
		{
			if (parent == null)
			{
				return bodyUnits * bodyScale;
			}
			return parent.InverseTransformVector(parent.rotation * (bodyUnits * bodyScale));
		}

		// Token: 0x060090B0 RID: 37040 RVA: 0x003B3E48 File Offset: 0x003B2048
		public static Vector3 WorldToBodyPosition(Transform parent, Vector3 world, float bodyScale)
		{
			if (parent == null)
			{
				return world;
			}
			float num = ((bodyScale > 1E-06f && EggRotorPlacementMath.IsFinite(bodyScale)) ? bodyScale : 1f);
			return Quaternion.Inverse(parent.rotation) * (world - parent.position) / num;
		}

		// Token: 0x060090B1 RID: 37041 RVA: 0x003B3E9C File Offset: 0x003B209C
		public static float LocalScaleFor(Transform parent, float bodyScale, float scale)
		{
			float num = Mathf.Max(0.0001f, bodyScale * scale);
			float num2 = ((parent != null) ? Mathf.Abs(parent.lossyScale.x) : 1f);
			if (num2 <= 1E-06f || !EggRotorPlacementMath.IsFinite(num2))
			{
				return num;
			}
			return num / num2;
		}

		// Token: 0x060090B2 RID: 37042 RVA: 0x003B3EF0 File Offset: 0x003B20F0
		public static void ApplyBodyPose(Transform target, Transform parent, Pose bodyPose, float bodyScale, float scale)
		{
			if (target == null)
			{
				return;
			}
			target.localPosition = EggRotorPlacementMath.BodyToLocalPosition(parent, bodyPose.position, bodyScale);
			target.localRotation = bodyPose.rotation;
			target.localScale = Vector3.one * EggRotorPlacementMath.LocalScaleFor(parent, bodyScale, scale);
		}

		// Token: 0x060090B3 RID: 37043 RVA: 0x003B3F40 File Offset: 0x003B2140
		public static Pose SwingRootPose(Vector3 socketPosition, Quaternion socketRotation, float rootLocalScale, Quaternion endRotation)
		{
			Quaternion quaternion = endRotation * Quaternion.Inverse(socketRotation);
			return new Pose(-(quaternion * (socketPosition * rootLocalScale)), quaternion);
		}

		// Token: 0x060090B4 RID: 37044 RVA: 0x003B3F72 File Offset: 0x003B2172
		public static bool IsFinite(Vector3 value)
		{
			return EggRotorPlacementMath.IsFinite(value.x) && EggRotorPlacementMath.IsFinite(value.y) && EggRotorPlacementMath.IsFinite(value.z);
		}

		// Token: 0x060090B5 RID: 37045 RVA: 0x003B3F9B File Offset: 0x003B219B
		public static bool IsFinite(Quaternion value)
		{
			return EggRotorPlacementMath.IsFinite(value.x) && EggRotorPlacementMath.IsFinite(value.y) && EggRotorPlacementMath.IsFinite(value.z) && EggRotorPlacementMath.IsFinite(value.w);
		}

		// Token: 0x060090B6 RID: 37046 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool IsFinite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x060090B7 RID: 37047 RVA: 0x003B3FD1 File Offset: 0x003B21D1
		private static float Clamp01Finite(float value)
		{
			if (!EggRotorPlacementMath.IsFinite(value))
			{
				return 0f;
			}
			return Mathf.Clamp01(value);
		}

		// Token: 0x060090B8 RID: 37048 RVA: 0x003B3FE7 File Offset: 0x003B21E7
		[CompilerGenerated]
		internal static float <WidestPassesOpening>g__Depth|5_0(Pose pose, ref EggRotorPlacementMath.<>c__DisplayClass5_0 A_1)
		{
			return Vector3.Dot(pose.position + pose.rotation * Vector3.back * A_1.eggLength - A_1.opening, A_1.axis);
		}
	}
}
