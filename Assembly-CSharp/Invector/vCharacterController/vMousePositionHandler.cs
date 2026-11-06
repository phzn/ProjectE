using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006AE RID: 1710
	public class vMousePositionHandler : MonoBehaviour
	{
		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002992 RID: 10642 RVA: 0x000EA024 File Offset: 0x000E8224
		public static vMousePositionHandler Instance
		{
			get
			{
				if (vMousePositionHandler._instance == null)
				{
					vMousePositionHandler._instance = Object.FindObjectOfType<vMousePositionHandler>();
				}
				if (vMousePositionHandler._instance == null)
				{
					vMousePositionHandler._instance = new GameObject("MousePositionHandler").AddComponent<vMousePositionHandler>();
					vMousePositionHandler._instance.mainCamera = Camera.main;
				}
				return vMousePositionHandler._instance;
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000EA07D File Offset: 0x000E827D
		public virtual void SetMousePosition(Vector2 pos)
		{
			this.joystickMousePos = pos;
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000EA088 File Offset: 0x000E8288
		public virtual Vector2 mousePosition
		{
			get
			{
				switch (vInput.instance.inputDevice)
				{
				case InputDevice.MouseKeyboard:
					return Input.mousePosition;
				case InputDevice.Joystick:
				{
					this.joystickMousePos.x = this.joystickMousePos.x + Input.GetAxis("RightAnalogHorizontal") * this.joystickSensitivity;
					this.joystickMousePos.x = Mathf.Clamp(this.joystickMousePos.x, -((float)Screen.width * 0.5f), (float)Screen.width * 0.5f);
					this.joystickMousePos.y = this.joystickMousePos.y + Input.GetAxis("RightAnalogVertical") * this.joystickSensitivity;
					this.joystickMousePos.y = Mathf.Clamp(this.joystickMousePos.y, -((float)Screen.height * 0.5f), (float)Screen.height * 0.5f);
					Vector2 vector = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
					Vector2 vector2 = this.joystickMousePos + vector;
					vector2.x = Mathf.Clamp(vector2.x, 0f, (float)Screen.width);
					vector2.y = Mathf.Clamp(vector2.y, 0f, (float)Screen.height);
					return vector2;
				}
				case InputDevice.Mobile:
					if (this.clampScreen)
					{
						return Input.GetTouch(0).deltaPosition;
					}
					return this.joystickMousePos;
				default:
					return Input.mousePosition;
				}
			}
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000EA1FC File Offset: 0x000E83FC
		public virtual Vector3 WorldMousePosition(LayerMask castLayer, out Collider collider)
		{
			if (!this.mainCamera)
			{
				if (!Camera.main)
				{
					Debug.LogWarning("Trying to get the world mouse position but a MainCamera is missing from the scene");
					collider = null;
					return Vector3.zero;
				}
				this.mainCamera = Camera.main;
				collider = null;
				return Vector3.zero;
			}
			else
			{
				Ray ray = this.mainCamera.ScreenPointToRay(this.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, this.mainCamera.farClipPlane, castLayer))
				{
					collider = raycastHit.collider;
					return raycastHit.point;
				}
				collider = null;
				return ray.GetPoint(this.mainCamera.farClipPlane);
			}
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x000EA2A4 File Offset: 0x000E84A4
		public virtual bool CastWorldMousePosition(LayerMask castLayer, out RaycastHit hit, float distance = 0f)
		{
			if (!this.mainCamera)
			{
				if (!Camera.main)
				{
					Debug.LogWarning("Trying to get the world mouse position but a MainCamera is missing from the scene");
					hit = default(RaycastHit);
					return false;
				}
				this.mainCamera = Camera.main;
				hit = default(RaycastHit);
				return false;
			}
			else
			{
				if (Physics.Raycast(this.mainCamera.ScreenPointToRay(this.mousePosition), out hit, (distance == 0f) ? this.mainCamera.farClipPlane : distance, castLayer))
				{
					return true;
				}
				hit = default(RaycastHit);
				return false;
			}
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x000EA338 File Offset: 0x000E8538
		public virtual bool CastWorldMousePosition(LayerMask castLayer, out RaycastHit hit, float distance = 0f, float radius = 0f)
		{
			if (!this.mainCamera)
			{
				if (!Camera.main)
				{
					Debug.LogWarning("Trying to get the world mouse position but a MainCamera is missing from the scene");
					hit = default(RaycastHit);
					return false;
				}
				this.mainCamera = Camera.main;
				hit = default(RaycastHit);
				return false;
			}
			else
			{
				if (radius == 0f)
				{
					return this.CastWorldMousePosition(castLayer, out hit, distance);
				}
				if (Physics.SphereCast(this.mainCamera.ScreenPointToRay(this.mousePosition), radius, out hit, (distance == 0f) ? this.mainCamera.farClipPlane : distance, castLayer))
				{
					return true;
				}
				hit = default(RaycastHit);
				return false;
			}
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000EA3E0 File Offset: 0x000E85E0
		public virtual Vector3 WorldMousePosition(LayerMask castLayer)
		{
			if (!this.mainCamera)
			{
				if (!Camera.main)
				{
					Debug.LogWarning("Trying to get the world mouse position but a MainCamera is missing from the scene");
					return Vector3.zero;
				}
				this.mainCamera = Camera.main;
				return Vector3.zero;
			}
			else
			{
				Ray ray = this.mainCamera.ScreenPointToRay(this.mousePosition);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, this.mainCamera.farClipPlane, castLayer))
				{
					return raycastHit.point;
				}
				return ray.GetPoint(this.mainCamera.farClipPlane);
			}
		}

		// Token: 0x040029B9 RID: 10681
		public Camera mainCamera;

		// Token: 0x040029BA RID: 10682
		protected static vMousePositionHandler _instance;

		// Token: 0x040029BB RID: 10683
		public string joystickHorizontalAxis = "RightAnalogHorizontal";

		// Token: 0x040029BC RID: 10684
		public string joystickVerticalAxis = "RightAnalogVertical";

		// Token: 0x040029BD RID: 10685
		public float joystickSensitivity = 25f;

		// Token: 0x040029BE RID: 10686
		public bool clampScreen = true;

		// Token: 0x040029BF RID: 10687
		public Vector2 joystickMousePos;
	}
}
