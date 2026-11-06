using System;
using System.Diagnostics.CodeAnalysis;
using Unity.Collections;

namespace UnityEngine.Rendering
{
	// Token: 0x020000E6 RID: 230
	public class KeyframeUtility
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x000252D4 File Offset: 0x000234D4
		public static void ResetAnimationCurve(AnimationCurve curve)
		{
			curve.ClearKeys();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000252DC File Offset: 0x000234DC
		private static Keyframe LerpSingleKeyframe(Keyframe lhs, Keyframe rhs, float t)
		{
			return new Keyframe
			{
				time = Mathf.Lerp(lhs.time, rhs.time, t),
				value = Mathf.Lerp(lhs.value, rhs.value, t),
				inTangent = Mathf.Lerp(lhs.inTangent, rhs.inTangent, t),
				outTangent = Mathf.Lerp(lhs.outTangent, rhs.outTangent, t),
				inWeight = Mathf.Lerp(lhs.inWeight, rhs.inWeight, t),
				outWeight = Mathf.Lerp(lhs.outWeight, rhs.outWeight, t),
				weightedMode = lhs.weightedMode
			};
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x000253A4 File Offset: 0x000235A4
		private static Keyframe GetKeyframeAndClampEdge([DisallowNull] NativeArray<Keyframe> keys, int index)
		{
			int num = keys.Length - 1;
			if (index < 0 || index > num)
			{
				Debug.LogWarning("Invalid index in GetKeyframeAndClampEdge. This is likely a bug.");
				return default(Keyframe);
			}
			Keyframe keyframe = keys[index];
			if (index == 0)
			{
				keyframe.inTangent = 0f;
			}
			if (index == num)
			{
				keyframe.outTangent = 0f;
			}
			return keyframe;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00025400 File Offset: 0x00023600
		private static Keyframe FetchKeyFromIndexClampEdge([DisallowNull] NativeArray<Keyframe> keys, int index, float segmentStartTime, float segmentEndTime)
		{
			float num = Mathf.Min(segmentStartTime, keys[0].time);
			float num2 = Mathf.Max(segmentEndTime, keys[keys.Length - 1].time);
			float value = keys[0].value;
			float value2 = keys[keys.Length - 1].value;
			Keyframe keyframeAndClampEdge;
			if (index < 0)
			{
				keyframeAndClampEdge = new Keyframe(num, value, 0f, 0f);
			}
			else if (index >= keys.Length)
			{
				Keyframe keyframe = keys[keys.Length - 1];
				keyframeAndClampEdge = new Keyframe(num2, value2, 0f, 0f);
			}
			else
			{
				keyframeAndClampEdge = KeyframeUtility.GetKeyframeAndClampEdge(keys, index);
			}
			return keyframeAndClampEdge;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x000254C8 File Offset: 0x000236C8
		private static void EvalCurveSegmentAndDeriv(out float dstValue, out float dstDeriv, Keyframe lhsKey, Keyframe rhsKey, float desiredTime)
		{
			float num = Mathf.Clamp(desiredTime, lhsKey.time, rhsKey.time);
			float num2 = Mathf.Max(rhsKey.time - lhsKey.time, 0.0001f);
			float num3 = rhsKey.value - lhsKey.value;
			float num4 = 1f / num2;
			float num5 = num4 * num4;
			float outTangent = lhsKey.outTangent;
			float inTangent = rhsKey.inTangent;
			float num6 = outTangent * num2;
			float num7 = inTangent * num2;
			float num8 = (num6 + num7 - num3 - num3) * num5 * num4;
			float num9 = (num3 + num3 + num3 - num6 - num6 - num7) * num5;
			float num10 = outTangent;
			float value = lhsKey.value;
			float num11 = Mathf.Clamp(num - lhsKey.time, 0f, num2);
			dstValue = num11 * (num11 * (num11 * num8 + num9) + num10) + value;
			dstDeriv = num11 * (3f * num11 * num8 + 2f * num9) + num10;
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x000255B0 File Offset: 0x000237B0
		private static Keyframe EvalKeyAtTime([DisallowNull] NativeArray<Keyframe> keys, int lhsIndex, int rhsIndex, float startTime, float endTime, float currTime)
		{
			Keyframe keyframe = KeyframeUtility.FetchKeyFromIndexClampEdge(keys, lhsIndex, startTime, endTime);
			Keyframe keyframe2 = KeyframeUtility.FetchKeyFromIndexClampEdge(keys, rhsIndex, startTime, endTime);
			float num;
			float num2;
			KeyframeUtility.EvalCurveSegmentAndDeriv(out num, out num2, keyframe, keyframe2, currTime);
			return new Keyframe(currTime, num, num2, num2);
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x000255EC File Offset: 0x000237EC
		public static void InterpAnimationCurve(ref AnimationCurve lhsAndResultCurve, [DisallowNull] AnimationCurve rhsCurve, float t)
		{
			if (t > 0f && rhsCurve.length != 0)
			{
				if (t >= 1f || lhsAndResultCurve.length == 0)
				{
					lhsAndResultCurve.CopyFrom(rhsCurve);
					return;
				}
				NativeArray<Keyframe> nativeArray = new NativeArray<Keyframe>(lhsAndResultCurve.length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				NativeArray<Keyframe> nativeArray2 = new NativeArray<Keyframe>(rhsCurve.length, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < lhsAndResultCurve.length; i++)
				{
					nativeArray[i] = lhsAndResultCurve[i];
				}
				for (int j = 0; j < rhsCurve.length; j++)
				{
					nativeArray2[j] = rhsCurve[j];
				}
				float num = Mathf.Min(nativeArray[0].time, nativeArray2[0].time);
				float num2 = Mathf.Max(nativeArray[lhsAndResultCurve.length - 1].time, nativeArray2[rhsCurve.length - 1].time);
				int num3 = lhsAndResultCurve.length + rhsCurve.length;
				int num4 = 0;
				NativeArray<Keyframe> nativeArray3 = new NativeArray<Keyframe>(num3, Allocator.Temp, NativeArrayOptions.ClearMemory);
				int num5 = 0;
				int num6 = 0;
				while (num5 < nativeArray.Length || num6 < nativeArray2.Length)
				{
					bool flag = num5 < nativeArray.Length;
					bool flag2 = num6 < nativeArray2.Length;
					Keyframe keyframe = default(Keyframe);
					Keyframe keyframe2 = default(Keyframe);
					if (flag && flag2)
					{
						keyframe = KeyframeUtility.GetKeyframeAndClampEdge(nativeArray, num5);
						keyframe2 = KeyframeUtility.GetKeyframeAndClampEdge(nativeArray2, num6);
						if (keyframe.time == keyframe2.time)
						{
							num5++;
							num6++;
						}
						else if (keyframe.time < keyframe2.time)
						{
							keyframe2 = KeyframeUtility.EvalKeyAtTime(nativeArray2, num6 - 1, num6, num, num2, keyframe.time);
							num5++;
						}
						else
						{
							keyframe = KeyframeUtility.EvalKeyAtTime(nativeArray, num5 - 1, num5, num, num2, keyframe2.time);
							num6++;
						}
					}
					else if (flag)
					{
						keyframe = KeyframeUtility.GetKeyframeAndClampEdge(nativeArray, num5);
						keyframe2 = KeyframeUtility.EvalKeyAtTime(nativeArray2, num6 - 1, num6, num, num2, keyframe.time);
						num5++;
					}
					else
					{
						keyframe2 = KeyframeUtility.GetKeyframeAndClampEdge(nativeArray2, num6);
						keyframe = KeyframeUtility.EvalKeyAtTime(nativeArray, num5 - 1, num5, num, num2, keyframe2.time);
						num6++;
					}
					Keyframe keyframe3 = KeyframeUtility.LerpSingleKeyframe(keyframe, keyframe2, t);
					nativeArray3[num4] = keyframe3;
					num4++;
				}
				KeyframeUtility.ResetAnimationCurve(lhsAndResultCurve);
				for (int k = 0; k < num4; k++)
				{
					lhsAndResultCurve.AddKey(nativeArray3[k]);
				}
				nativeArray3.Dispose();
			}
		}
	}
}
