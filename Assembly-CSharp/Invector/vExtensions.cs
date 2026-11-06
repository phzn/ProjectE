using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

namespace Invector
{
	// Token: 0x02000497 RID: 1175
	public static class vExtensions
	{
		// Token: 0x06001CC5 RID: 7365 RVA: 0x000B1FE4 File Offset: 0x000B01E4
		public static void DebugPoint(this Vector3 vector, Color color, float time, float size = 0.25f)
		{
			Debug.DrawRay(vector, Vector3.right * size, color, time);
			Debug.DrawRay(vector, Vector3.left * size, color, time);
			Debug.DrawRay(vector, Vector3.forward * size, color, time);
			Debug.DrawRay(vector, Vector3.back * size, color, time);
			Debug.DrawRay(vector, Vector3.up * size, color, time);
			Debug.DrawRay(vector, Vector3.down * size, color, time);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000B2064 File Offset: 0x000B0264
		public static void DebugPoint(this Vector3 vector, Color color, float size = 0.25f)
		{
			Debug.DrawRay(vector, Vector3.right * size, color);
			Debug.DrawRay(vector, Vector3.left * size, color);
			Debug.DrawRay(vector, Vector3.forward * size, color);
			Debug.DrawRay(vector, Vector3.back * size, color);
			Debug.DrawRay(vector, Vector3.up * size, color);
			Debug.DrawRay(vector, Vector3.down * size, color);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000B20E0 File Offset: 0x000B02E0
		public static string InsertSpaceBeforeUpperCase(this string input)
		{
			string text = "";
			foreach (char c in input)
			{
				if (char.IsUpper(c) && !string.IsNullOrEmpty(text))
				{
					text += " ";
				}
				text += c.ToString();
			}
			return text;
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000B2139 File Offset: 0x000B0339
		public static string RemoveUnderline(this string input)
		{
			return input.Replace("_", "");
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000B214B File Offset: 0x000B034B
		public static string ToClearUpper(this string target)
		{
			return target.Replace(" ", string.Empty).ToUpper();
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000B2162 File Offset: 0x000B0362
		public static bool IsInSideRange(this float value, float min, float max)
		{
			return value >= min && value <= max;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000B2171 File Offset: 0x000B0371
		public static bool IsInSideRange(this float value, Vector2 minMaxRange)
		{
			return value >= minMaxRange.x && value <= minMaxRange.y;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000B218A File Offset: 0x000B038A
		public static bool IsVectorNaN(this Vector3 vector)
		{
			return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000B21B4 File Offset: 0x000B03B4
		public static Vector3[] MakeSmoothCurve(this Vector3[] pts, float smoothFactor = 0.25f)
		{
			smoothFactor = Mathf.Clamp(smoothFactor, 0.1f, 0.9f);
			Vector3[] array = new Vector3[(pts.Length - 2) * 2 + 2];
			try
			{
				array[0] = pts[0];
				array[array.Length - 1] = pts[pts.Length - 1];
				int num = 1;
				for (int i = 0; i < pts.Length - 2; i++)
				{
					array[num] = pts[i] + (pts[i + 1] - pts[i]) * (1f - smoothFactor);
					array[num + 1] = pts[i + 1] + (pts[i + 2] - pts[i + 1]) * smoothFactor;
					num += 2;
				}
			}
			catch
			{
				array = pts;
			}
			return array;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000B229C File Offset: 0x000B049C
		public static float GetLenght(this NavMeshPath path)
		{
			float num = 0f;
			if (path != null && path.corners.Length > 1)
			{
				Vector3 vector = path.corners[0];
				for (int i = 1; i < path.corners.Length; i++)
				{
					num += Vector3.Distance(vector, path.corners[i]);
					vector = path.corners[i];
				}
			}
			return num;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000B2300 File Offset: 0x000B0500
		public static List<Vector3> MakeSmoothCurve(this List<Vector3> pts, float smoothFactor = 0.25f)
		{
			smoothFactor = Mathf.Clamp(smoothFactor, 0.1f, 0.9f);
			List<Vector3> list = new List<Vector3>((pts.Count - 2) * 2 + 2);
			try
			{
				list[0] = pts[0];
				list[list.Count - 1] = pts[pts.Count - 1];
				int num = 1;
				for (int i = 0; i < pts.Count - 2; i++)
				{
					list[num] = pts[i] + (pts[i + 1] - pts[i]) * (1f - smoothFactor);
					list[num + 1] = pts[i + 1] + (pts[i + 2] - pts[i + 1]) * smoothFactor;
					num += 2;
				}
			}
			catch
			{
				list = pts;
			}
			return list;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000B23F4 File Offset: 0x000B05F4
		public static Vector3[] MakeSmoothCurveArray(this List<Vector3> pts, float smoothFactor = 0.25f)
		{
			smoothFactor = Mathf.Clamp(smoothFactor, 0.1f, 0.9f);
			Vector3[] array = new Vector3[(pts.Count - 2) * 2 + 2];
			try
			{
				array[0] = pts[0];
				array[array.Length - 1] = pts[pts.Count - 1];
				int num = 1;
				for (int i = 0; i < pts.Count - 2; i++)
				{
					array[num] = pts[i] + (pts[i + 1] - pts[i]) * (1f - smoothFactor);
					array[num + 1] = pts[i + 1] + (pts[i + 2] - pts[i + 1]) * smoothFactor;
					num += 2;
				}
			}
			catch
			{
				array = pts.vToArray<Vector3>();
			}
			return array;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000B24EC File Offset: 0x000B06EC
		public static void SetLayerRecursively(this GameObject obj, int layer)
		{
			obj.layer = layer;
			foreach (object obj2 in obj.transform)
			{
				((Transform)obj2).gameObject.SetLayerRecursively(layer);
			}
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000B2550 File Offset: 0x000B0750
		public static bool ContainsLayer(this LayerMask layermask, int layer)
		{
			return layermask == (layermask | (1 << layer));
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000B2568 File Offset: 0x000B0768
		public static void SetActiveChildren(this GameObject gameObjet, bool value)
		{
			foreach (object obj in gameObjet.transform)
			{
				((Transform)obj).gameObject.SetActive(value);
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000B25C4 File Offset: 0x000B07C4
		public static bool isChild(this Transform me, Transform target)
		{
			if (!target)
			{
				return false;
			}
			string name = target.gameObject.name;
			Transform transform = me.FindChildByNameRecursive(name);
			return !(transform == null) && transform.Equals(target);
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000B2604 File Offset: 0x000B0804
		private static Transform FindChildByNameRecursive(this Transform me, string name)
		{
			if (me.name == name)
			{
				return me;
			}
			for (int i = 0; i < me.childCount; i++)
			{
				Transform transform = me.GetChild(i).FindChildByNameRecursive(name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000B264C File Offset: 0x000B084C
		public static Vector3 NormalizeAngle(this Vector3 eulerAngle)
		{
			Vector3 vector = eulerAngle;
			if (vector.x > 180f)
			{
				vector.x -= 360f;
			}
			else if (vector.x < -180f)
			{
				vector.x += 360f;
			}
			if (vector.y > 180f)
			{
				vector.y -= 360f;
			}
			else if (vector.y < -180f)
			{
				vector.y += 360f;
			}
			if (vector.z > 180f)
			{
				vector.z -= 360f;
			}
			else if (vector.z < -180f)
			{
				vector.z += 360f;
			}
			return new Vector3(vector.x, vector.y, vector.z);
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000B2726 File Offset: 0x000B0926
		public static Vector3 Difference(this Vector3 vector, Vector3 otherVector)
		{
			return otherVector - vector;
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000B2730 File Offset: 0x000B0930
		public static Vector3 AngleFormOtherDirection(this Vector3 directionA, Vector3 directionB)
		{
			if (directionA.normalized.magnitude == 0f || directionB.normalized.magnitude == 0f)
			{
				return Vector3.zero;
			}
			return Quaternion.LookRotation(directionA).eulerAngles.AngleFormOtherEuler(Quaternion.LookRotation(directionB).eulerAngles);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000B2790 File Offset: 0x000B0990
		public static Vector3 AngleFormOtherDirection(this Vector3 directionA, Vector3 directionB, Vector3 up)
		{
			return Quaternion.LookRotation(directionA, up).eulerAngles.AngleFormOtherEuler(Quaternion.LookRotation(directionB, up).eulerAngles);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000B27C0 File Offset: 0x000B09C0
		public static Vector3 AngleFormOtherEuler(this Vector3 eulerA, Vector3 eulerB)
		{
			return eulerA.NormalizeAngle().Difference(eulerB.NormalizeAngle()).NormalizeAngle();
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000B27D8 File Offset: 0x000B09D8
		public static string ToStringColor(this bool value)
		{
			if (value)
			{
				return "<color=green>YES</color>";
			}
			return "<color=red>NO</color>";
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000B27E8 File Offset: 0x000B09E8
		public static float ClampAngle(float angle, float min, float max)
		{
			do
			{
				if (angle < -360f)
				{
					angle += 360f;
				}
				if (angle > 360f)
				{
					angle -= 360f;
				}
			}
			while (angle < -360f || angle > 360f);
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000B2824 File Offset: 0x000B0A24
		public static T[] Append<T>(this T[] arrayInitial, T[] arrayToAppend)
		{
			if (arrayToAppend == null)
			{
				throw new ArgumentNullException("The appended object cannot be null");
			}
			if (arrayInitial is string || arrayToAppend is string)
			{
				throw new ArgumentException("The argument must be an enumerable");
			}
			T[] array = new T[arrayInitial.Length + arrayToAppend.Length];
			arrayInitial.CopyTo(array, 0);
			arrayToAppend.CopyTo(array, arrayInitial.Length);
			return array;
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000B287C File Offset: 0x000B0A7C
		public static List<T> vCopy<T>(this List<T> list)
		{
			List<T> list2 = new List<T>();
			if (list == null || list.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(list[i]);
			}
			return list2;
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000B28BC File Offset: 0x000B0ABC
		public static List<T> vToList<T>(this T[] array)
		{
			List<T> list = new List<T>();
			if (array == null || array.Length == 0)
			{
				return list;
			}
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			return list;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000B28F4 File Offset: 0x000B0AF4
		public static T[] vToArray<T>(this List<T> list)
		{
			T[] array = new T[list.Count];
			if (list == null || list.Count == 0)
			{
				return array;
			}
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = list[i];
			}
			return array;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000B293C File Offset: 0x000B0B3C
		public static Vector3 BoxSize(this BoxCollider boxCollider)
		{
			float num = boxCollider.transform.lossyScale.x * boxCollider.size.x;
			float num2 = boxCollider.transform.lossyScale.z * boxCollider.size.z;
			float num3 = boxCollider.transform.lossyScale.y * boxCollider.size.y;
			return new Vector3(num, num3, num2);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000B29A8 File Offset: 0x000B0BA8
		public static bool IsClosed(this BoxCollider boxCollider, Vector3 position, Vector3 margin, Vector3 centerOffset)
		{
			Vector3 vector = boxCollider.BoxSize();
			float x = margin.x;
			float y = margin.y;
			float z = margin.z;
			Vector3 vector2 = boxCollider.center + centerOffset;
			Vector2 vector3 = new Vector2(vector2.x - vector.x * 0.5f - x, vector2.x + vector.x * 0.5f + x);
			Vector2 vector4 = new Vector2(vector2.y - vector.y * 0.5f - y, vector2.y + vector.y * 0.5f + y);
			Vector2 vector5 = new Vector2(vector2.z - vector.z * 0.5f - z, vector2.z + vector.z * 0.5f + z);
			position = boxCollider.transform.InverseTransformPoint(position);
			bool flag = (position.x * boxCollider.transform.lossyScale.x).IsInSideRange(vector3);
			bool flag2 = (position.y * boxCollider.transform.lossyScale.y).IsInSideRange(vector4);
			bool flag3 = (position.z * boxCollider.transform.lossyScale.z).IsInSideRange(vector5);
			return flag && flag2 && flag3;
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000B2AEA File Offset: 0x000B0CEA
		public static T ToEnum<T>(this string value, bool ignoreCase = true)
		{
			return (T)((object)Enum.Parse(typeof(T), value, ignoreCase));
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000B2B04 File Offset: 0x000B0D04
		public static bool TryGetField(this object obj, string name, out FieldInfo info)
		{
			Type type = obj.GetType();
			bool flag = false;
			info = null;
			do
			{
				FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field != null)
				{
					flag = true;
					info = field;
				}
				else
				{
					type = type.BaseType;
				}
			}
			while (!flag && type != null);
			return flag;
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000B2B4C File Offset: 0x000B0D4C
		public static bool Contains<T>(this Enum value, Enum lookingForFlag) where T : struct
		{
			int num = (int)value;
			int num2 = (int)lookingForFlag;
			return (num & num2) == num2;
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000B2B6C File Offset: 0x000B0D6C
		public static Vector3 DotVector(this Vector3 _vector, Vector3 _direction)
		{
			if (_direction.sqrMagnitude != 1f)
			{
				_direction.Normalize();
			}
			float num = Vector3.Dot(_vector, _direction);
			return _direction * num;
		}
	}
}
