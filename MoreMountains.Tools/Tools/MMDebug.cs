using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x020000F9 RID: 249
	public static class MMDebug
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000192F0 File Offset: 0x000174F0
		public static MethodInfo[] Commands
		{
			get
			{
				if (MMDebug._commands == null)
				{
					MMDebug._commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany<Assembly, MethodInfo>((Assembly m) => m.GetTypes().SelectMany<Type, MethodInfo>((Type n) => from o in n.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
						where o.GetCustomAttribute<MMDebugLogCommandAttribute>() != null
						select o)).ToArray<MethodInfo>();
				}
				return MMDebug._commands;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00019344 File Offset: 0x00017544
		public static void DebugLogCommand(string command)
		{
			if (command == string.Empty || command == null)
			{
				MMDebug.LogCommand("", "#ff2a00");
				return;
			}
			string[] array = command.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array == null || array.Length == 0)
			{
				MMDebug.LogCommand("Empty command", "#ff2a00");
				return;
			}
			string commandFirst = MMString.UppercaseFirst(array[0]);
			MethodInfo[] array2 = MMDebug.Commands.Where<MethodInfo>((MethodInfo m) => m.Name == commandFirst).ToArray<MethodInfo>();
			if (array2.Length == 0)
			{
				MMDebug.LogCommand("Command " + commandFirst + " not found.", "#ff2a00");
				return;
			}
			object[] array3 = null;
			if (array.Length > 1)
			{
				MethodInfo methodInfo = array2.Where<MethodInfo>((MethodInfo m) => m.GetParameters().Length != 0).FirstOrDefault<MethodInfo>();
				if (methodInfo == null)
				{
					MMDebug.LogCommand("A version of command " + commandFirst + " with arguments could not be found. Maybe try without arguments.", "#ff2a00");
					return;
				}
				MMDebugLogCommandArgumentCountAttribute mmdebugLogCommandArgumentCountAttribute = methodInfo.GetCustomAttributes<MMDebugLogCommandArgumentCountAttribute>(true).FirstOrDefault<MMDebugLogCommandArgumentCountAttribute>();
				if (mmdebugLogCommandArgumentCountAttribute != null && mmdebugLogCommandArgumentCountAttribute.ArgumentCount > array.Length - 1)
				{
					MMDebug.LogCommand(string.Concat(new string[]
					{
						"A version of command ",
						commandFirst,
						" needs at least ",
						mmdebugLogCommandArgumentCountAttribute.ArgumentCount.ToString(),
						" arguments."
					}), "#ff2a00");
					return;
				}
				array3 = new object[] { array };
			}
			else
			{
				MethodInfo methodInfo = array2.Where<MethodInfo>((MethodInfo m) => m.GetParameters().Length == 0).FirstOrDefault<MethodInfo>();
				if (methodInfo == null)
				{
					MMDebug.LogCommand("A version of command " + commandFirst + " without arguments could not be found.", "#ff2a00");
					return;
				}
			}
			MMDebug.LogCommand(command, "#FFC400");
			array2[0].Invoke(null, array3);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00019530 File Offset: 0x00017730
		private static void LogCommand(string command, string color)
		{
			MMDebug.DebugLogItem debugLogItem = new MMDebug.DebugLogItem(command, color, Time.frameCount, Time.time, 3, true);
			MMDebug.LogHistory.Add(debugLogItem);
			MMDebug.MMDebugLogEvent.Trigger(new MMDebug.DebugLogItem(null, "", Time.frameCount, Time.time, 0, false));
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001957C File Offset: 0x0001777C
		public static string LogHistoryText
		{
			get
			{
				string text = "";
				string text2 = "";
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < MMDebug.LogHistory.Count; i++)
				{
					if (!string.IsNullOrEmpty(MMDebug.LogHistory[i].Color))
					{
						text = "<color=" + MMDebug.LogHistory[i].Color + ">";
						text2 = "</color>";
					}
					if (MMDebug.LogHistory[i].DisplayFrameCount)
					{
						stringBuilder.Append("<color=#82d3f9>[" + MMDebug.LogHistory[i].Framecount.ToString() + "]</color> ");
					}
					stringBuilder.Append("<color=#f9a682>[" + MMTime.FloatToTimeString(MMDebug.LogHistory[i].Time, false, true, true, true) + "]</color> ");
					StringBuilder stringBuilder2 = stringBuilder;
					string text3 = text;
					object message = MMDebug.LogHistory[i].Message;
					stringBuilder2.Append(text3 + ((message != null) ? message.ToString() : null) + text2);
					stringBuilder.Append(Environment.NewLine);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001969F File Offset: 0x0001789F
		public static void DebugLogClear()
		{
			MMDebug.LogHistory.Clear();
			MMDebug.MMDebugLogEvent.Trigger(new MMDebug.DebugLogItem(null, "", Time.frameCount, Time.time, 0, false));
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x000196C7 File Offset: 0x000178C7
		public static void DebugLogInfo(object message, string color = "", int timePrecision = 3, bool displayFrameCount = true)
		{
			MMDebug.DebugLogTime(message, color, timePrecision, displayFrameCount);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x000196D4 File Offset: 0x000178D4
		public static void DebugLogTime(object message, string color = "", int timePrecision = 3, bool displayFrameCount = true)
		{
			if (!MMDebug.DebugLogsEnabled)
			{
				return;
			}
			string name = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Name;
			color = ((color == "") ? "#00FFFF" : color);
			string text = "";
			string text2 = "";
			if (!string.IsNullOrEmpty(color))
			{
				text = "<color=" + color + ">";
				text2 = "</color>";
			}
			string text3 = "";
			if (displayFrameCount)
			{
				text3 = text3 + "<color=#82d3f9>[f" + Time.frameCount.ToString() + "]</color> ";
			}
			text3 = text3 + "<color=#f9a682>[" + MMTime.FloatToTimeString(Time.time, false, true, true, true) + "]</color> ";
			text3 = text3 + name + " : ";
			text3 = text3 + text + ((message != null) ? message.ToString() : null) + text2;
			Debug.Log(text3);
			MMDebug.LogDebugToConsole(message, color, timePrecision, displayFrameCount);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x000197C0 File Offset: 0x000179C0
		public static MMDebug.DebugLogItem LogDebugToConsole(object message, string color, int timePrecision, bool displayFrameCount)
		{
			MMDebug.DebugLogItem debugLogItem = new MMDebug.DebugLogItem(message, color, Time.frameCount, Time.time, timePrecision, displayFrameCount);
			if (MMDebug.LogHistory.Count > MMDebug._logHistoryMaxLength)
			{
				MMDebug.LogHistory.RemoveAt(0);
			}
			MMDebug.LogHistory.Add(debugLogItem);
			MMDebug.MMDebugLogEvent.Trigger(debugLogItem);
			return debugLogItem;
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00019810 File Offset: 0x00017A10
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x0001985E File Offset: 0x00017A5E
		public static bool DebugLogsEnabled
		{
			get
			{
				if (MMDebug._debugLogEnabledSet)
				{
					return MMDebug._debugLogEnabled;
				}
				if (PlayerPrefs.HasKey("DebugLogsEnabled"))
				{
					MMDebug._debugLogEnabled = PlayerPrefs.GetInt("DebugLogsEnabled") != 0;
				}
				else
				{
					MMDebug._debugLogEnabled = true;
				}
				MMDebug._debugLogEnabledSet = true;
				return MMDebug._debugLogEnabled;
			}
			private set
			{
				MMDebug._debugLogEnabledSet = true;
				MMDebug._debugLogEnabled = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001986C File Offset: 0x00017A6C
		// (set) Token: 0x06000544 RID: 1348 RVA: 0x0001986F File Offset: 0x00017A6F
		public static bool DebugDrawEnabled
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00019871 File Offset: 0x00017A71
		public static void SetDebugLogsEnabled(bool status)
		{
			MMDebug.DebugLogsEnabled = status;
			MMDebug._debugLogEnabled = status;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001987F File Offset: 0x00017A7F
		public static void SetDebugDrawEnabled(bool status)
		{
			MMDebug.DebugDrawEnabled = status;
			MMDebug._debugDrawEnabled = status;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001988D File Offset: 0x00017A8D
		public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && MMDebug.DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000198C4 File Offset: 0x00017AC4
		public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float length, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && MMDebug.DebugDrawEnabled)
			{
				Quaternion quaternion = Quaternion.Euler(0f, 0f, angle);
				Vector3[] array = new Vector3[8];
				float num = size.x / 2f;
				float num2 = size.y / 2f;
				array[0] = quaternion * (origin + Vector2.left * num + Vector2.up * num2);
				array[1] = quaternion * (origin + Vector2.right * num + Vector2.up * num2);
				array[2] = quaternion * (origin + Vector2.right * num - Vector2.up * num2);
				array[3] = quaternion * (origin + Vector2.left * num - Vector2.up * num2);
				array[4] = quaternion * (origin + Vector2.left * num + Vector2.up * num2 + length * direction);
				array[5] = quaternion * (origin + Vector2.right * num + Vector2.up * num2 + length * direction);
				array[6] = quaternion * (origin + Vector2.right * num - Vector2.up * num2 + length * direction);
				array[7] = quaternion * (origin + Vector2.left * num - Vector2.up * num2 + length * direction);
				Debug.DrawLine(array[0], array[1], color);
				Debug.DrawLine(array[1], array[2], color);
				Debug.DrawLine(array[2], array[3], color);
				Debug.DrawLine(array[3], array[0], color);
				Debug.DrawLine(array[4], array[5], color);
				Debug.DrawLine(array[5], array[6], color);
				Debug.DrawLine(array[6], array[7], color);
				Debug.DrawLine(array[7], array[4], color);
				Debug.DrawLine(array[0], array[4], color);
				Debug.DrawLine(array[1], array[5], color);
				Debug.DrawLine(array[2], array[6], color);
				Debug.DrawLine(array[3], array[7], color);
			}
			return Physics2D.BoxCast(origin, size, angle, direction, length, mask);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00019BF0 File Offset: 0x00017DF0
		public static RaycastHit2D MonoRayCastNonAlloc(RaycastHit2D[] array, Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
		{
			if (drawGizmo && MMDebug.DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			if (Physics2D.RaycastNonAlloc(rayOriginPoint, rayDirection, array, rayDistance, mask) > 0)
			{
				return array[0];
			}
			return default(RaycastHit2D);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00019C48 File Offset: 0x00017E48
		public static RaycastHit Raycast3D(Vector3 rayOriginPoint, Vector3 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
		{
			if (drawGizmo && MMDebug.DebugDrawEnabled)
			{
				Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
			}
			RaycastHit raycastHit;
			Physics.Raycast(rayOriginPoint, rayDirection, out raycastHit, rayDistance, mask, queryTriggerInteraction);
			return raycastHit;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00019C83 File Offset: 0x00017E83
		public static void DebugOnScreen(string message)
		{
			if (!MMDebug.DebugLogsEnabled)
			{
				return;
			}
			MMDebug.InstantiateOnScreenConsole(25);
			MMDebug._console.AddMessage(message, "", 30);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00019CA6 File Offset: 0x00017EA6
		public static void DebugOnScreen(string label, object value, int fontSize = 25)
		{
			if (!MMDebug.DebugLogsEnabled)
			{
				return;
			}
			MMDebug.InstantiateOnScreenConsole(fontSize);
			MMDebug._console.AddMessage(label, value, fontSize);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00019CC4 File Offset: 0x00017EC4
		public static void InstantiateOnScreenConsole(int fontSize = 25)
		{
			if (!MMDebug.DebugLogsEnabled)
			{
				return;
			}
			if (MMDebug._console == null)
			{
				MMDebug._console = (MMDebugOnScreenConsole)Object.FindObjectOfType(typeof(MMDebugOnScreenConsole));
			}
			if (MMDebug._console == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(Resources.Load("MMDebugOnScreenConsole") as GameObject);
				gameObject.name = "MMDebugOnScreenConsole";
				MMDebug._console = gameObject.GetComponent<MMDebugOnScreenConsole>();
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00019D35 File Offset: 0x00017F35
		public static void SetOnScreenConsole(MMDebugOnScreenConsole newConsole)
		{
			MMDebug._console = newConsole;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00019D3D File Offset: 0x00017F3D
		public static void DrawGizmoArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 3f, float arrowHeadAngle = 25f)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Gizmos.color = color;
			Gizmos.DrawRay(origin, direction);
			MMDebug.DrawArrowEnd(true, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00019D60 File Offset: 0x00017F60
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowHeadLength = 0.2f, float arrowHeadAngle = 35f)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Debug.DrawRay(origin, direction, color);
			MMDebug.DrawArrowEnd(false, origin, direction, color, arrowHeadLength, arrowHeadAngle);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00019D7E File Offset: 0x00017F7E
		public static void DebugDrawArrow(Vector3 origin, Vector3 direction, Color color, float arrowLength, float arrowHeadLength = 0.2f, float arrowHeadAngle = 35f)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Debug.DrawRay(origin, direction * arrowLength, color);
			MMDebug.DrawArrowEnd(false, origin, direction * arrowLength, color, arrowHeadLength, arrowHeadAngle);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00019DAC File Offset: 0x00017FAC
		public static void DebugDrawCross(Vector3 spot, float crossSize, Color color)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			zero.x = spot.x - crossSize / 2f;
			zero.y = spot.y - crossSize / 2f;
			zero.z = spot.z;
			zero2.x = 1f;
			zero2.y = 1f;
			zero2.z = 0f;
			Debug.DrawRay(zero, zero2 * crossSize, color);
			zero.x = spot.x - crossSize / 2f;
			zero.y = spot.y + crossSize / 2f;
			zero.z = spot.z;
			zero2.x = 1f;
			zero2.y = -1f;
			zero2.z = 0f;
			Debug.DrawRay(zero, zero2 * crossSize, color);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00019EA0 File Offset: 0x000180A0
		private static void DrawArrowEnd(bool drawGizmos, Vector3 arrowEndPosition, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 40f)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			if (direction == Vector3.zero)
			{
				return;
			}
			Vector3 vector = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0f, 0f) * Vector3.back;
			Vector3 vector2 = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0f, 0f) * Vector3.back;
			Vector3 vector3 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, arrowHeadAngle, 0f) * Vector3.back;
			Vector3 vector4 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, -arrowHeadAngle, 0f) * Vector3.back;
			if (drawGizmos)
			{
				Gizmos.color = color;
				Gizmos.DrawRay(arrowEndPosition + direction, vector * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, vector2 * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, vector3 * arrowHeadLength);
				Gizmos.DrawRay(arrowEndPosition + direction, vector4 * arrowHeadLength);
				return;
			}
			Debug.DrawRay(arrowEndPosition + direction, vector * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, vector2 * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, vector3 * arrowHeadLength, color);
			Debug.DrawRay(arrowEndPosition + direction, vector4 * arrowHeadLength, color);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001A00F File Offset: 0x0001820F
		public static void DrawHandlesBounds(Bounds bounds, Color color)
		{
			bool debugDrawEnabled = MMDebug.DebugDrawEnabled;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001A017 File Offset: 0x00018217
		public static void DrawSolidRectangle(Vector3 position, Vector3 size, Color borderColor, Color solidColor)
		{
			bool debugDrawEnabled = MMDebug.DebugDrawEnabled;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001A01F File Offset: 0x0001821F
		public static void DrawGizmoPoint(Vector3 position, float size, Color color)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Gizmos.color = color;
			Gizmos.DrawWireSphere(position, size);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001A038 File Offset: 0x00018238
		public static void DrawCube(Vector3 position, Color color, Vector3 size)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3 vector = size / 2f;
			Vector3[] array = new Vector3[]
			{
				position + new Vector3(vector.x, vector.y, vector.z),
				position + new Vector3(-vector.x, vector.y, vector.z),
				position + new Vector3(-vector.x, -vector.y, vector.z),
				position + new Vector3(vector.x, -vector.y, vector.z),
				position + new Vector3(vector.x, vector.y, -vector.z),
				position + new Vector3(-vector.x, vector.y, -vector.z),
				position + new Vector3(-vector.x, -vector.y, -vector.z),
				position + new Vector3(vector.x, -vector.y, -vector.z)
			};
			Debug.DrawLine(array[0], array[1], color);
			Debug.DrawLine(array[1], array[2], color);
			Debug.DrawLine(array[2], array[3], color);
			Debug.DrawLine(array[3], array[0], color);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001A1DC File Offset: 0x000183DC
		public static void DrawGizmoCube(Transform transform, Vector3 offset, Vector3 cubeSize, bool wireOnly)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Gizmos.matrix = transform.localToWorldMatrix;
			if (wireOnly)
			{
				Gizmos.DrawWireCube(offset, cubeSize);
				return;
			}
			Gizmos.DrawCube(offset, cubeSize);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001A204 File Offset: 0x00018404
		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Color color)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Gizmos.color = color;
			Vector3 vector = new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, 0f);
			Vector3 vector2 = new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, 0f);
			Vector3 vector3 = new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, 0f);
			Vector3 vector4 = new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, 0f);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001A304 File Offset: 0x00018504
		public static void DrawGizmoRectangle(Vector2 center, Vector2 size, Matrix4x4 rotationMatrix, Color color)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			GL.PushMatrix();
			Gizmos.color = color;
			Vector3 vector = rotationMatrix * new Vector3(center.x - size.x / 2f, center.y + size.y / 2f, 0f);
			Vector3 vector2 = rotationMatrix * new Vector3(center.x + size.x / 2f, center.y + size.y / 2f, 0f);
			Vector3 vector3 = rotationMatrix * new Vector3(center.x + size.x / 2f, center.y - size.y / 2f, 0f);
			Vector3 vector4 = rotationMatrix * new Vector3(center.x - size.x / 2f, center.y - size.y / 2f, 0f);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
			GL.PopMatrix();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001A44C File Offset: 0x0001864C
		public static void DrawRectangle(Rect rectangle, Color color)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3 vector = new Vector3(rectangle.x + rectangle.width / 2f, rectangle.y + rectangle.height / 2f, 0f);
			Vector3 vector2 = new Vector3(rectangle.width, rectangle.height, 0f);
			MMDebug.DrawRectangle(vector, color, vector2);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001A4B8 File Offset: 0x000186B8
		public static void DrawRectangle(Vector3 position, Color color, Vector3 size)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3 vector = size / 2f;
			Vector3[] array = new Vector3[]
			{
				position + new Vector3(vector.x, vector.y, vector.z),
				position + new Vector3(-vector.x, vector.y, vector.z),
				position + new Vector3(-vector.x, -vector.y, vector.z),
				position + new Vector3(vector.x, -vector.y, vector.z)
			};
			Debug.DrawLine(array[0], array[1], color);
			Debug.DrawLine(array[1], array[2], color);
			Debug.DrawLine(array[2], array[3], color);
			Debug.DrawLine(array[3], array[0], color);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001A5C4 File Offset: 0x000187C4
		public static void DrawPoint(Vector3 position, Color color, float size)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3[] array = new Vector3[]
			{
				position + Vector3.up * size,
				position - Vector3.up * size,
				position + Vector3.right * size,
				position - Vector3.right * size,
				position + Vector3.forward * size,
				position - Vector3.forward * size
			};
			Debug.DrawLine(array[0], array[1], color);
			Debug.DrawLine(array[2], array[3], color);
			Debug.DrawLine(array[4], array[5], color);
			Debug.DrawLine(array[0], array[2], color);
			Debug.DrawLine(array[0], array[3], color);
			Debug.DrawLine(array[0], array[4], color);
			Debug.DrawLine(array[0], array[5], color);
			Debug.DrawLine(array[1], array[2], color);
			Debug.DrawLine(array[1], array[3], color);
			Debug.DrawLine(array[1], array[4], color);
			Debug.DrawLine(array[1], array[5], color);
			Debug.DrawLine(array[4], array[2], color);
			Debug.DrawLine(array[4], array[3], color);
			Debug.DrawLine(array[5], array[2], color);
			Debug.DrawLine(array[5], array[3], color);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001A79C File Offset: 0x0001899C
		public static void DrawGizmoPoint(Vector3 position, Color color, float size)
		{
			if (!MMDebug.DebugDrawEnabled)
			{
				return;
			}
			Vector3[] array = new Vector3[]
			{
				position + Vector3.up * size,
				position - Vector3.up * size,
				position + Vector3.right * size,
				position - Vector3.right * size,
				position + Vector3.forward * size,
				position - Vector3.forward * size
			};
			Gizmos.color = color;
			Gizmos.DrawLine(array[0], array[1]);
			Gizmos.DrawLine(array[2], array[3]);
			Gizmos.DrawLine(array[4], array[5]);
			Gizmos.DrawLine(array[0], array[2]);
			Gizmos.DrawLine(array[0], array[3]);
			Gizmos.DrawLine(array[0], array[4]);
			Gizmos.DrawLine(array[0], array[5]);
			Gizmos.DrawLine(array[1], array[2]);
			Gizmos.DrawLine(array[1], array[3]);
			Gizmos.DrawLine(array[1], array[4]);
			Gizmos.DrawLine(array[1], array[5]);
			Gizmos.DrawLine(array[4], array[2]);
			Gizmos.DrawLine(array[4], array[3]);
			Gizmos.DrawLine(array[5], array[2]);
			Gizmos.DrawLine(array[5], array[3]);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001A96C File Offset: 0x00018B6C
		public static string GetSystemInfo()
		{
			string text = "SYSTEM INFO";
			text = text + "\n<color=#FFFFFF>Device Type :</color> " + SystemInfo.deviceType.ToString();
			text = text + "\n<color=#FFFFFF>OS Version :</color> " + SystemInfo.operatingSystem;
			text = text + "\n<color=#FFFFFF>System Memory Size :</color> " + SystemInfo.systemMemorySize.ToString();
			text = string.Concat(new string[]
			{
				text,
				"\n<color=#FFFFFF>Graphic Device Name :</color> ",
				SystemInfo.graphicsDeviceName,
				" (version ",
				SystemInfo.graphicsDeviceVersion,
				")"
			});
			text = text + "\n<color=#FFFFFF>Graphic Memory Size :</color> " + SystemInfo.graphicsMemorySize.ToString();
			text = text + "\n<color=#FFFFFF>Graphic Max Texture Size :</color> " + SystemInfo.maxTextureSize.ToString();
			text = text + "\n<color=#FFFFFF>Graphic Shader Level :</color> " + SystemInfo.graphicsShaderLevel.ToString();
			text = text + "\n<color=#FFFFFF>Compute Shader Support :</color> " + SystemInfo.supportsComputeShaders.ToString();
			text = text + "\n<color=#FFFFFF>Processor Count :</color> " + SystemInfo.processorCount.ToString();
			text = text + "\n<color=#FFFFFF>Processor Type :</color> " + SystemInfo.processorType;
			text = text + "\n<color=#FFFFFF>3D Texture Support :</color> " + SystemInfo.supports3DTextures.ToString();
			text = text + "\n<color=#FFFFFF>Shadow Support :</color> " + SystemInfo.supportsShadows.ToString();
			text = text + "\n<color=#FFFFFF>Platform :</color> " + Application.platform.ToString();
			text = string.Concat(new string[]
			{
				text,
				"\n<color=#FFFFFF>Screen Size :</color> ",
				Screen.width.ToString(),
				" x ",
				Screen.height.ToString()
			});
			return text + "\n<color=#FFFFFF>DPI :</color> " + Screen.dpi.ToString();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001AB3C File Offset: 0x00018D3C
		public static void ClearConsole()
		{
			Type type = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
			if (type != null)
			{
				MethodInfo method = type.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
		}

		// Token: 0x040006D5 RID: 1749
		private static MethodInfo[] _commands;

		// Token: 0x040006D6 RID: 1750
		private static readonly int _logHistoryMaxLength = 256;

		// Token: 0x040006D7 RID: 1751
		private static bool _debugDrawEnabled = false;

		// Token: 0x040006D8 RID: 1752
		private static bool _debugLogEnabled = false;

		// Token: 0x040006D9 RID: 1753
		private static bool _debugLogEnabledSet = false;

		// Token: 0x040006DA RID: 1754
		public static List<MMDebug.DebugLogItem> LogHistory = new List<MMDebug.DebugLogItem>(MMDebug._logHistoryMaxLength);

		// Token: 0x040006DB RID: 1755
		private const string _editorPrefsDebugLogs = "DebugLogsEnabled";

		// Token: 0x040006DC RID: 1756
		private const string _editorPrefsDebugDraws = "DebugDrawsEnabled";

		// Token: 0x040006DD RID: 1757
		public static MMDebugOnScreenConsole _console;

		// Token: 0x040006DE RID: 1758
		private const string _debugConsolePrefabPath = "MMDebugOnScreenConsole";

		// Token: 0x02000421 RID: 1057
		public struct DebugLogItem
		{
			// Token: 0x060019D6 RID: 6614 RVA: 0x0006BF38 File Offset: 0x0006A138
			public DebugLogItem(object message, string color, int framecount, float time, int timePrecision, bool displayFrameCount)
			{
				this.Message = message;
				this.Color = color;
				this.Framecount = framecount;
				this.Time = time;
				this.TimePrecision = timePrecision;
				this.DisplayFrameCount = displayFrameCount;
			}

			// Token: 0x04001F32 RID: 7986
			public object Message;

			// Token: 0x04001F33 RID: 7987
			public string Color;

			// Token: 0x04001F34 RID: 7988
			public int Framecount;

			// Token: 0x04001F35 RID: 7989
			public float Time;

			// Token: 0x04001F36 RID: 7990
			public int TimePrecision;

			// Token: 0x04001F37 RID: 7991
			public bool DisplayFrameCount;
		}

		// Token: 0x02000422 RID: 1058
		public struct MMDebugLogEvent
		{
			// Token: 0x1400004F RID: 79
			// (add) Token: 0x060019D7 RID: 6615 RVA: 0x0006BF68 File Offset: 0x0006A168
			// (remove) Token: 0x060019D8 RID: 6616 RVA: 0x0006BF9C File Offset: 0x0006A19C
			private static event MMDebug.MMDebugLogEvent.Delegate OnEvent;

			// Token: 0x060019D9 RID: 6617 RVA: 0x0006BFCF File Offset: 0x0006A1CF
			[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
			private static void RuntimeInitialization()
			{
				MMDebug.MMDebugLogEvent.OnEvent = null;
			}

			// Token: 0x060019DA RID: 6618 RVA: 0x0006BFD7 File Offset: 0x0006A1D7
			public static void Register(MMDebug.MMDebugLogEvent.Delegate callback)
			{
				MMDebug.MMDebugLogEvent.OnEvent += callback;
			}

			// Token: 0x060019DB RID: 6619 RVA: 0x0006BFDF File Offset: 0x0006A1DF
			public static void Unregister(MMDebug.MMDebugLogEvent.Delegate callback)
			{
				MMDebug.MMDebugLogEvent.OnEvent -= callback;
			}

			// Token: 0x060019DC RID: 6620 RVA: 0x0006BFE7 File Offset: 0x0006A1E7
			public static void Trigger(MMDebug.DebugLogItem item)
			{
				MMDebug.MMDebugLogEvent.Delegate onEvent = MMDebug.MMDebugLogEvent.OnEvent;
				if (onEvent == null)
				{
					return;
				}
				onEvent(item);
			}

			// Token: 0x020005A3 RID: 1443
			// (Invoke) Token: 0x06001DEB RID: 7659
			public delegate void Delegate(MMDebug.DebugLogItem item);
		}
	}
}
