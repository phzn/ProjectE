using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000103 RID: 259
	public static class MMMaths
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0001B434 File Offset: 0x00019634
		private static float SpringVelocity(float currentValue, float targetValue, float velocity, float damping, float frequency, float deltaTime)
		{
			frequency = frequency * 2f * 3.1415927f;
			float num = frequency * frequency;
			float num2 = 2f * damping * frequency;
			float num3 = currentValue - targetValue;
			float num4 = -num * num3 - num2 * velocity;
			velocity += deltaTime * num4;
			return velocity;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001B478 File Offset: 0x00019678
		public static void Spring(ref float currentValue, float targetValue, ref float velocity, float damping, float frequency, float deltaTime)
		{
			float num = 0.016666668f;
			float num3;
			for (float num2 = deltaTime; num2 > 0f; num2 -= num3)
			{
				num3 = Mathf.Min(num2, num);
				velocity = MMMaths.SpringVelocity(currentValue, targetValue, velocity, damping, frequency, num3);
				currentValue += num3 * velocity;
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001B4C0 File Offset: 0x000196C0
		public static void Spring(ref Vector2 currentValue, Vector2 targetValue, ref Vector2 velocity, float damping, float frequency, float deltaTime)
		{
			float num = 0.016666668f;
			float num3;
			for (float num2 = deltaTime; num2 > 0f; num2 -= num3)
			{
				num3 = Mathf.Min(num2, num);
				velocity.x = MMMaths.SpringVelocity(currentValue.x, targetValue.x, velocity.x, damping, frequency, num3);
				velocity.y = MMMaths.SpringVelocity(currentValue.y, targetValue.y, velocity.y, damping, frequency, num3);
				currentValue += num3 * velocity;
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001B54C File Offset: 0x0001974C
		public static void Spring(ref Vector3 currentValue, Vector3 targetValue, ref Vector3 velocity, float damping, float frequency, float deltaTime)
		{
			float num = 0.016666668f;
			float num3;
			for (float num2 = deltaTime; num2 > 0f; num2 -= num3)
			{
				num3 = Mathf.Min(num2, num);
				velocity.x = MMMaths.SpringVelocity(currentValue.x, targetValue.x, velocity.x, damping, frequency, num3);
				velocity.y = MMMaths.SpringVelocity(currentValue.y, targetValue.y, velocity.y, damping, frequency, num3);
				velocity.z = MMMaths.SpringVelocity(currentValue.z, targetValue.z, velocity.z, damping, frequency, num3);
				currentValue += num3 * velocity;
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001B600 File Offset: 0x00019800
		public static void Spring(ref Vector4 currentValue, Vector4 targetValue, ref Vector4 velocity, float damping, float frequency, float deltaTime)
		{
			float num = 0.016666668f;
			float num3;
			for (float num2 = deltaTime; num2 > 0f; num2 -= num3)
			{
				num3 = Mathf.Min(num2, num);
				velocity.x = MMMaths.SpringVelocity(currentValue.x, targetValue.x, velocity.x, damping, frequency, num3);
				velocity.y = MMMaths.SpringVelocity(currentValue.y, targetValue.y, velocity.y, damping, frequency, num3);
				velocity.z = MMMaths.SpringVelocity(currentValue.z, targetValue.z, velocity.z, damping, frequency, num3);
				velocity.w = MMMaths.SpringVelocity(currentValue.w, targetValue.w, velocity.w, damping, frequency, num3);
				currentValue += num3 * velocity;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001B6D4 File Offset: 0x000198D4
		private static float LerpRate(float rate, float deltaTime)
		{
			rate = Mathf.Clamp01(rate);
			float num = -Mathf.Log(1f - rate, 2f) * 60f;
			return Mathf.Pow(2f, -num * deltaTime);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001B710 File Offset: 0x00019910
		public static float Lerp(float value, float target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Mathf.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001B72A File Offset: 0x0001992A
		public static Vector2 Lerp(Vector2 value, Vector2 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Vector2.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001B744 File Offset: 0x00019944
		public static Vector3 Lerp(Vector3 value, Vector3 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Vector3.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001B75E File Offset: 0x0001995E
		public static Vector4 Lerp(Vector4 value, Vector4 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Vector4.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001B778 File Offset: 0x00019978
		public static Quaternion Lerp(Quaternion value, Quaternion target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Quaternion.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001B792 File Offset: 0x00019992
		public static Color Lerp(Color value, Color target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Color.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001B7AC File Offset: 0x000199AC
		public static Color32 Lerp(Color32 value, Color32 target, float rate, float deltaTime)
		{
			if (deltaTime == 0f)
			{
				return value;
			}
			return Color32.Lerp(target, value, MMMaths.LerpRate(rate, deltaTime));
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001B7C8 File Offset: 0x000199C8
		public static float Clamp(float value, float min, float max, bool clampMin, bool clampMax)
		{
			float num = value;
			if (clampMin && num < min)
			{
				num = min;
			}
			if (clampMax && num > max)
			{
				num = max;
			}
			return num;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001B7EB File Offset: 0x000199EB
		public static float RoundToNearestHalf(float a)
		{
			return a -= a % 0.5f;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001B7F9 File Offset: 0x000199F9
		public static Quaternion LookAt2D(Vector2 direction)
		{
			return Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * 57.29578f, Vector3.forward);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B81C File Offset: 0x00019A1C
		public static Vector2 Vector3ToVector2(Vector3 target)
		{
			return new Vector2(target.x, target.y);
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001B82F File Offset: 0x00019A2F
		public static Vector3 Vector2ToVector3(Vector2 target)
		{
			return new Vector3(target.x, target.y, 0f);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001B847 File Offset: 0x00019A47
		public static Vector3 Vector2ToVector3(Vector2 target, float newZValue)
		{
			return new Vector3(target.x, target.y, newZValue);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001B85B File Offset: 0x00019A5B
		public static Vector3 RoundVector3(Vector3 vector)
		{
			return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001B883 File Offset: 0x00019A83
		public static Vector2 RandomVector2(Vector2 minimum, Vector2 maximum)
		{
			return new Vector2(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y));
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001B8AC File Offset: 0x00019AAC
		public static Vector3 RandomVector3(Vector3 minimum, Vector3 maximum)
		{
			return new Vector3(Random.Range(minimum.x, maximum.x), Random.Range(minimum.y, maximum.y), Random.Range(minimum.z, maximum.z));
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		public static Vector2 RandomPointOnCircle(float circleRadius)
		{
			return Random.insideUnitCircle.normalized * circleRadius;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001B908 File Offset: 0x00019B08
		public static Vector3 RandomPointOnSphere(float sphereRadius)
		{
			return Random.onUnitSphere * sphereRadius;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001B918 File Offset: 0x00019B18
		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
		{
			angle *= 0.017453292f;
			float num = Mathf.Cos(angle) * (point.x - pivot.x) - Mathf.Sin(angle) * (point.y - pivot.y) + pivot.x;
			float num2 = Mathf.Sin(angle) * (point.x - pivot.x) + Mathf.Cos(angle) * (point.y - pivot.y) + pivot.y;
			return new Vector3(num, num2, 0f);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001B99C File Offset: 0x00019B9C
		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 vector = point - pivot;
			vector = Quaternion.Euler(angle) * vector;
			point = vector + pivot;
			return point;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001B9C8 File Offset: 0x00019BC8
		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion quaternion)
		{
			Vector3 vector = point - pivot;
			vector = quaternion * vector;
			point = vector + pivot;
			return point;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0001B9F0 File Offset: 0x00019BF0
		public static Vector2 RotateVector2(Vector2 vector, float angle)
		{
			if (angle == 0f)
			{
				return vector;
			}
			float num = Mathf.Sin(angle * 0.017453292f);
			float num2 = Mathf.Cos(angle * 0.017453292f);
			float x = vector.x;
			float y = vector.y;
			vector.x = num2 * x - num * y;
			vector.y = num * x + num2 * y;
			return vector;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001BA4C File Offset: 0x00019C4C
		public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
		{
			float num = Vector2.Angle(vectorA, vectorB);
			if (Vector3.Cross(vectorA, vectorB).z > 0f)
			{
				num = 360f - num;
			}
			return num;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001BA87 File Offset: 0x00019C87
		public static float AngleDirection(Vector3 vectorA, Vector3 vectorB, Vector3 up)
		{
			return Vector3.Dot(Vector3.Cross(vectorA, vectorB), up);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001BA96 File Offset: 0x00019C96
		public static float DistanceBetweenPointAndLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
		{
			return Vector3.Magnitude(MMMaths.ProjectPointOnLine(point, lineStart, lineEnd) - point);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001BAAC File Offset: 0x00019CAC
		public static Vector3 ProjectPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
		{
			Vector3 vector = point - lineStart;
			Vector3 vector2 = lineEnd - lineStart;
			float magnitude = vector2.magnitude;
			Vector3 vector3 = vector2;
			if (magnitude > 1E-06f)
			{
				vector3 /= magnitude;
			}
			float num = Mathf.Clamp(Vector3.Dot(vector3, vector), 0f, magnitude);
			return lineStart + vector3 * num;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001BB08 File Offset: 0x00019D08
		public static int Sum(params int[] thingsToAdd)
		{
			int num = 0;
			for (int i = 0; i < thingsToAdd.Length; i++)
			{
				num += thingsToAdd[i];
			}
			return num;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001BB2C File Offset: 0x00019D2C
		public static int RollADice(int numberOfSides)
		{
			return Random.Range(1, numberOfSides + 1);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001BB37 File Offset: 0x00019D37
		public static bool Chance(int percent)
		{
			return Random.Range(0, 100) <= percent;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001BB47 File Offset: 0x00019D47
		public static float Approach(float from, float to, float amount)
		{
			if (Mathf.Approximately(from, to))
			{
				return from;
			}
			if (from < to)
			{
				from += amount;
				if (from > to)
				{
					return to;
				}
			}
			if (from > to)
			{
				from -= amount;
				if (from < to)
				{
					return to;
				}
			}
			return from;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001BB73 File Offset: 0x00019D73
		public static float Remap(float x, float A, float B, float C, float D)
		{
			return C + (x - A) / (B - A) * (D - C);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001BB83 File Offset: 0x00019D83
		public static float ClampAngle(float angle, float minimumAngle, float maximumAngle)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, minimumAngle, maximumAngle);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001BBAF File Offset: 0x00019DAF
		public static float RoundToDecimal(float value, int numberOfDecimals)
		{
			if (numberOfDecimals <= 0)
			{
				return Mathf.Round(value);
			}
			return Mathf.Round(value * 10f * (float)numberOfDecimals) / (10f * (float)numberOfDecimals);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001BBD4 File Offset: 0x00019DD4
		public static float RoundToClosest(float value, float[] possibleValues, bool pickSmallestDistance = false)
		{
			if (possibleValues.Length == 0)
			{
				return 0f;
			}
			float num = possibleValues[0];
			foreach (float num2 in possibleValues)
			{
				float num3 = Mathf.Abs(num - value);
				float num4 = Mathf.Abs(num2 - value);
				if (num3 > num4)
				{
					num = num2;
				}
				else if (num3 == num4 && ((pickSmallestDistance && num > num2) || (!pickSmallestDistance && num < num2)))
				{
					num = ((value < 0f) ? num : num2);
				}
			}
			return num;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001BC44 File Offset: 0x00019E44
		public static Vector3 DirectionFromAngle(float angle, float additionalAngle)
		{
			angle += additionalAngle;
			Vector3 zero = Vector3.zero;
			zero.x = Mathf.Sin(angle * 0.017453292f);
			zero.y = 0f;
			zero.z = Mathf.Cos(angle * 0.017453292f);
			return zero;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001BC90 File Offset: 0x00019E90
		public static Vector3 DirectionFromAngle2D(float angle, float additionalAngle)
		{
			angle += additionalAngle;
			Vector3 zero = Vector3.zero;
			zero.x = Mathf.Cos(angle * 0.017453292f);
			zero.y = Mathf.Sin(angle * 0.017453292f);
			zero.z = 0f;
			return zero;
		}
	}
}
