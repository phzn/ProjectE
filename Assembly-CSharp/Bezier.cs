using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E9 RID: 233
public static class Bezier
{
	// Token: 0x0600093C RID: 2364 RVA: 0x00043330 File Offset: 0x00041530
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		return num * num * p0 + 2f * num * t * p1 + t * t * p2;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00043378 File Offset: 0x00041578
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		return 2f * (1f - t) * (p1 - p0) + 2f * t * (p2 - p1);
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x000433AC File Offset: 0x000415AC
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		return num * num * num * p0 + 3f * num * num * t * p1 + 3f * num * t * t * p2 + t * t * t * p3;
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00043418 File Offset: 0x00041618
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		return 3f * num * num * (p1 - p0) + 6f * num * t * (p2 - p1) + 3f * t * t * (p3 - p2);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x00043484 File Offset: 0x00041684
	public static Vector3 CalculateBezierPoint(Vector3 startPoint, Vector3 startTangent, Vector3 endTangent, Vector3 endPoint, float t)
	{
		return (((-startPoint + 3f * (startTangent - endTangent) + endPoint) * t + (3f * (startPoint + endTangent) - 6f * startTangent)) * t + 3f * (startTangent - startPoint)) * t + startPoint;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0004350C File Offset: 0x0004170C
	public static Vector3[] GetPoints(Vector3 startPoint, Vector3 startTangent, Vector3 endTangent, Vector3 endPoint, float space, float accuracy)
	{
		Vector3 vector = startPoint;
		Vector3 vector2 = vector;
		List<Vector3> list = new List<Vector3>();
		list.Add(vector);
		float num = 0f;
		for (float num2 = accuracy; num2 <= 1f; num2 += accuracy)
		{
			Vector3 point = Bezier.GetPoint(startPoint, startTangent, endTangent, endPoint, num2);
			num += Vector3.Distance(point, vector2);
			vector2 = point;
		}
		num += Vector3.Distance(endPoint, vector2);
		Mathf.FloorToInt(num / space);
		float num3 = num / space;
		if (num3 - (float)((int)num3) > 0.5f)
		{
			num3 = (float)((int)num3 + 1);
		}
		else
		{
			num3 = (float)((int)num3);
		}
		float num4 = num / num3;
		for (float num5 = 0f; num5 <= 1f; num5 += accuracy)
		{
			Vector3 point2 = Bezier.GetPoint(startPoint, startTangent, endTangent, endPoint, num5);
			if (Vector3.Distance(point2, vector) >= num4)
			{
				vector += (point2 - vector).normalized * num4;
				list.Add(vector);
			}
		}
		return list.ToArray();
	}
}
