using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CB1 RID: 3249
	public static class LongWayHomeCinematicPath
	{
		// Token: 0x06005E52 RID: 24146 RVA: 0x00254E08 File Offset: 0x00253008
		public static Vector3[] RoundCorners(Vector3[] path, int iterations = 3)
		{
			if (path == null || path.Length < 3)
			{
				return path;
			}
			List<Vector3> list = new List<Vector3>(path);
			for (int i = 0; i < iterations; i++)
			{
				List<Vector3> list2 = new List<Vector3>(list.Count * 2) { list[0] };
				for (int j = 0; j < list.Count - 1; j++)
				{
					if (j > 0)
					{
						list2.Add(Vector3.Lerp(list[j], list[j + 1], 0.25f));
					}
					if (j < list.Count - 2)
					{
						list2.Add(Vector3.Lerp(list[j], list[j + 1], 0.75f));
					}
				}
				list2.Add(list[list.Count - 1]);
				list = list2;
			}
			return list.ToArray();
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x00254ED4 File Offset: 0x002530D4
		public static float[] CumulativeLengths(Vector3[] path)
		{
			float[] array = new float[path.Length];
			for (int i = 1; i < path.Length; i++)
			{
				Vector3 vector = path[i] - path[i - 1];
				vector.y = 0f;
				array[i] = array[i - 1] + vector.magnitude;
			}
			return array;
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x00254F2C File Offset: 0x0025312C
		public static Vector3 Sample(Vector3[] path, float[] along, float distance)
		{
			if (distance <= 0f)
			{
				return path[0];
			}
			for (int i = 1; i < path.Length; i++)
			{
				if (distance <= along[i])
				{
					float num = along[i] - along[i - 1];
					return Vector3.Lerp(path[i - 1], path[i], (num > 0f) ? ((distance - along[i - 1]) / num) : 1f);
				}
			}
			return path[path.Length - 1];
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x00254FA0 File Offset: 0x002531A0
		public static Vector3[] BackAndForthLeg(Vector3 from, Vector3 to, float turnRadius, int turnPoints = 8)
		{
			Vector3 vector = to - from;
			vector.y = 0f;
			vector = ((vector.sqrMagnitude > 1E-06f) ? vector.normalized : Vector3.forward);
			Vector3 vector2 = new Vector3(-vector.z, 0f, vector.x);
			turnPoints = Mathf.Max(2, turnPoints);
			List<Vector3> list = new List<Vector3>(turnPoints + 2) { from - vector2 * turnRadius };
			for (int i = 0; i <= turnPoints; i++)
			{
				float num = 3.1415927f * (float)i / (float)turnPoints;
				list.Add(to + vector2 * (-turnRadius * Mathf.Cos(num)) + vector * (turnRadius * Mathf.Sin(num)));
			}
			return list.ToArray();
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x0025506C File Offset: 0x0025326C
		public static float BendDegrees(Vector3[] path, float[] along, float distance, float lookAhead)
		{
			float num = along[along.Length - 1];
			Vector3 vector = LongWayHomeCinematicPath.Sample(path, along, Mathf.Min(num, distance + 0.2f)) - LongWayHomeCinematicPath.Sample(path, along, distance);
			Vector3 vector2 = LongWayHomeCinematicPath.Sample(path, along, Mathf.Min(num, distance + lookAhead + 0.2f)) - LongWayHomeCinematicPath.Sample(path, along, Mathf.Min(num, distance + lookAhead));
			vector.y = (vector2.y = 0f);
			if (vector.sqrMagnitude >= 1E-06f && vector2.sqrMagnitude >= 1E-06f)
			{
				return Vector3.Angle(vector, vector2);
			}
			return 0f;
		}
	}
}
