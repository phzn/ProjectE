using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000013 RID: 19
	internal class SendMouseEvents
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00002890 File Offset: 0x00000A90
		private static void UpdateMouse()
		{
			bool flag = SendMouseEvents.s_GetMouseState != null;
			if (flag)
			{
				KeyValuePair<int, Vector2> keyValuePair = SendMouseEvents.s_GetMouseState();
				SendMouseEvents.s_MousePosition = keyValuePair.Value;
				SendMouseEvents.s_MouseButtonPressedThisFrame = keyValuePair.Key == 2;
				SendMouseEvents.s_MouseButtonIsPressed = keyValuePair.Key != 0;
			}
			else
			{
				bool flag2 = !Input.CheckDisabled();
				if (flag2)
				{
					SendMouseEvents.s_MousePosition = Input.mousePosition;
					SendMouseEvents.s_MouseButtonPressedThisFrame = Input.GetMouseButtonDown(0);
					SendMouseEvents.s_MouseButtonIsPressed = Input.GetMouseButton(0);
				}
				else
				{
					SendMouseEvents.s_MousePosition = default(Vector2);
					SendMouseEvents.s_MouseButtonPressedThisFrame = false;
					SendMouseEvents.s_MouseButtonIsPressed = false;
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002931 File Offset: 0x00000B31
		[RequiredByNativeCode]
		private static void SetMouseMoved()
		{
			SendMouseEvents.s_MouseUsed = true;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000293C File Offset: 0x00000B3C
		[RequiredByNativeCode]
		private static void DoSendMouseEvents(int skipRTCameras)
		{
			SendMouseEvents.UpdateMouse();
			Vector2 vector = SendMouseEvents.s_MousePosition;
			int allCamerasCount = Camera.allCamerasCount;
			bool flag = SendMouseEvents.m_Cameras == null || SendMouseEvents.m_Cameras.Length != allCamerasCount;
			if (flag)
			{
				SendMouseEvents.m_Cameras = new Camera[allCamerasCount];
			}
			Camera.GetAllCameras(SendMouseEvents.m_Cameras);
			for (int i = 0; i < SendMouseEvents.m_CurrentHit.Length; i++)
			{
				SendMouseEvents.m_CurrentHit[i] = default(SendMouseEvents.HitInfo);
			}
			bool flag2 = !SendMouseEvents.s_MouseUsed;
			if (flag2)
			{
				foreach (Camera camera in SendMouseEvents.m_Cameras)
				{
					bool flag3 = camera == null || (skipRTCameras != 0 && camera.targetTexture != null);
					if (!flag3)
					{
						int targetDisplay = camera.targetDisplay;
						Vector3 vector2 = Display.RelativeMouseAt(vector);
						bool flag4 = vector2 != Vector3.zero;
						if (flag4)
						{
							int num = (int)vector2.z;
							bool flag5 = num != targetDisplay;
							if (flag5)
							{
								goto IL_0368;
							}
							float num2 = (float)Screen.width;
							float num3 = (float)Screen.height;
							bool flag6 = targetDisplay > 0 && targetDisplay < Display.displays.Length;
							if (flag6)
							{
								num2 = (float)Display.displays[targetDisplay].systemWidth;
								num3 = (float)Display.displays[targetDisplay].systemHeight;
							}
							Vector2 vector3 = new Vector2(vector2.x / num2, vector2.y / num3);
							bool flag7 = vector3.x < 0f || vector3.x > 1f || vector3.y < 0f || vector3.y > 1f;
							if (flag7)
							{
								goto IL_0368;
							}
						}
						else
						{
							vector2 = vector;
						}
						bool flag8 = !camera.pixelRect.Contains(vector2);
						if (!flag8)
						{
							bool flag9 = camera.eventMask == 0;
							if (!flag9)
							{
								Ray ray = camera.ScreenPointToRay(vector2);
								float z = ray.direction.z;
								float num4 = (Mathf.Approximately(0f, z) ? float.PositiveInfinity : Mathf.Abs((camera.farClipPlane - camera.nearClipPlane) / z));
								GameObject gameObject = CameraRaycastHelper.RaycastTry(camera, ray, num4, camera.cullingMask & camera.eventMask);
								bool flag10 = gameObject != null;
								if (flag10)
								{
									SendMouseEvents.m_CurrentHit[1].target = gameObject;
									SendMouseEvents.m_CurrentHit[1].camera = camera;
								}
								else
								{
									bool flag11 = camera.clearFlags == CameraClearFlags.Skybox || camera.clearFlags == CameraClearFlags.Color;
									if (flag11)
									{
										SendMouseEvents.m_CurrentHit[1].target = null;
										SendMouseEvents.m_CurrentHit[1].camera = null;
									}
								}
								GameObject gameObject2 = CameraRaycastHelper.RaycastTry2D(camera, ray, num4, camera.cullingMask & camera.eventMask);
								bool flag12 = gameObject2 != null;
								if (flag12)
								{
									SendMouseEvents.m_CurrentHit[2].target = gameObject2;
									SendMouseEvents.m_CurrentHit[2].camera = camera;
								}
								else
								{
									bool flag13 = camera.clearFlags == CameraClearFlags.Skybox || camera.clearFlags == CameraClearFlags.Color;
									if (flag13)
									{
										SendMouseEvents.m_CurrentHit[2].target = null;
										SendMouseEvents.m_CurrentHit[2].camera = null;
									}
								}
							}
						}
					}
					IL_0368:;
				}
			}
			for (int k = 0; k < SendMouseEvents.m_CurrentHit.Length; k++)
			{
				SendMouseEvents.SendEvents(k, SendMouseEvents.m_CurrentHit[k]);
			}
			SendMouseEvents.s_MouseUsed = false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00002CFC File Offset: 0x00000EFC
		private static void SendEvents(int i, SendMouseEvents.HitInfo hit)
		{
			bool flag = SendMouseEvents.s_MouseButtonPressedThisFrame;
			bool flag2 = SendMouseEvents.s_MouseButtonIsPressed;
			bool flag3 = flag;
			if (flag3)
			{
				bool flag4 = hit;
				if (flag4)
				{
					SendMouseEvents.m_MouseDownHit[i] = hit;
					SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseDown");
				}
			}
			else
			{
				bool flag5 = !flag2;
				if (flag5)
				{
					bool flag6 = SendMouseEvents.m_MouseDownHit[i];
					if (flag6)
					{
						bool flag7 = SendMouseEvents.HitInfo.Compare(hit, SendMouseEvents.m_MouseDownHit[i]);
						if (flag7)
						{
							SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseUpAsButton");
						}
						SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseUp");
						SendMouseEvents.m_MouseDownHit[i] = default(SendMouseEvents.HitInfo);
					}
				}
				else
				{
					bool flag8 = SendMouseEvents.m_MouseDownHit[i];
					if (flag8)
					{
						SendMouseEvents.m_MouseDownHit[i].SendMessage("OnMouseDrag");
					}
				}
			}
			bool flag9 = SendMouseEvents.HitInfo.Compare(hit, SendMouseEvents.m_LastHit[i]);
			if (flag9)
			{
				bool flag10 = hit;
				if (flag10)
				{
					hit.SendMessage("OnMouseOver");
				}
			}
			else
			{
				bool flag11 = SendMouseEvents.m_LastHit[i];
				if (flag11)
				{
					SendMouseEvents.m_LastHit[i].SendMessage("OnMouseExit");
				}
				bool flag12 = hit;
				if (flag12)
				{
					hit.SendMessage("OnMouseEnter");
					hit.SendMessage("OnMouseOver");
				}
			}
			SendMouseEvents.m_LastHit[i] = hit;
		}

		// Token: 0x0400004E RID: 78
		private const int m_HitIndexGUI = 0;

		// Token: 0x0400004F RID: 79
		private const int m_HitIndexPhysics3D = 1;

		// Token: 0x04000050 RID: 80
		private const int m_HitIndexPhysics2D = 2;

		// Token: 0x04000051 RID: 81
		private static bool s_MouseUsed = false;

		// Token: 0x04000052 RID: 82
		private static readonly SendMouseEvents.HitInfo[] m_LastHit = new SendMouseEvents.HitInfo[3];

		// Token: 0x04000053 RID: 83
		private static readonly SendMouseEvents.HitInfo[] m_MouseDownHit = new SendMouseEvents.HitInfo[3];

		// Token: 0x04000054 RID: 84
		private static readonly SendMouseEvents.HitInfo[] m_CurrentHit = new SendMouseEvents.HitInfo[3];

		// Token: 0x04000055 RID: 85
		private static Camera[] m_Cameras;

		// Token: 0x04000056 RID: 86
		public static Func<KeyValuePair<int, Vector2>> s_GetMouseState;

		// Token: 0x04000057 RID: 87
		private static Vector2 s_MousePosition;

		// Token: 0x04000058 RID: 88
		private static bool s_MouseButtonPressedThisFrame;

		// Token: 0x04000059 RID: 89
		private static bool s_MouseButtonIsPressed;

		// Token: 0x02000014 RID: 20
		private struct HitInfo
		{
			// Token: 0x060000B0 RID: 176 RVA: 0x00002EB9 File Offset: 0x000010B9
			public void SendMessage(string name)
			{
				this.target.SendMessage(name, null, SendMessageOptions.DontRequireReceiver);
			}

			// Token: 0x060000B1 RID: 177 RVA: 0x00002ECC File Offset: 0x000010CC
			public static implicit operator bool(SendMouseEvents.HitInfo exists)
			{
				return exists.target != null && exists.camera != null;
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x00002EFC File Offset: 0x000010FC
			public static bool Compare(SendMouseEvents.HitInfo lhs, SendMouseEvents.HitInfo rhs)
			{
				return lhs.target == rhs.target && lhs.camera == rhs.camera;
			}

			// Token: 0x0400005A RID: 90
			public GameObject target;

			// Token: 0x0400005B RID: 91
			public Camera camera;
		}

		// Token: 0x02000015 RID: 21
		public enum LeftMouseButtonState
		{
			// Token: 0x0400005D RID: 93
			NotPressed,
			// Token: 0x0400005E RID: 94
			Pressed,
			// Token: 0x0400005F RID: 95
			PressedThisFrame
		}
	}
}
