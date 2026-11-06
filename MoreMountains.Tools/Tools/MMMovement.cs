using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000104 RID: 260
	public static class MMMovement
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0001BCDB File Offset: 0x00019EDB
		public static IEnumerator MoveFromTo(GameObject movingObject, Vector3 pointA, Vector3 pointB, float duration, AnimationCurve curve = null)
		{
			float journey = 0f;
			while (journey < duration)
			{
				float num = Mathf.Clamp01(journey / duration);
				Vector3 vector = Vector3.Lerp(pointA, pointB, curve.Evaluate(num));
				movingObject.transform.position = vector;
				journey += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001BD07 File Offset: 0x00019F07
		public static IEnumerator AnimateScale(Transform targetTransform, Vector3 vector, float duration, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, float multiplier = 1f)
		{
			if (targetTransform == null)
			{
				yield break;
			}
			if (curveX == null || curveY == null || curveZ == null)
			{
				yield break;
			}
			if (duration == 0f)
			{
				yield break;
			}
			float journey = 0f;
			while (journey < duration)
			{
				float num = Mathf.Clamp01(journey / duration);
				vector.x = curveX.Evaluate(num);
				vector.y = curveY.Evaluate(num);
				vector.z = curveZ.Evaluate(num);
				targetTransform.localScale = multiplier * vector;
				journey += Time.deltaTime;
				yield return null;
			}
			yield return null;
			yield break;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001BD43 File Offset: 0x00019F43
		public static IEnumerator AnimateRotation(Transform targetTransform, Vector3 vector, float duration, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, float multiplier)
		{
			if (targetTransform == null)
			{
				yield break;
			}
			if (curveX == null || curveY == null || curveZ == null)
			{
				yield break;
			}
			if (duration == 0f)
			{
				yield break;
			}
			float journey = 0f;
			while (journey < duration)
			{
				float num = Mathf.Clamp01(journey / duration);
				vector.x = curveX.Evaluate(num) * multiplier;
				vector.y = curveY.Evaluate(num) * multiplier;
				vector.z = curveZ.Evaluate(num) * multiplier;
				targetTransform.localEulerAngles = vector;
				journey += Time.deltaTime;
				yield return null;
			}
			yield return null;
			yield break;
		}
	}
}
