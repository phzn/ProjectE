using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GPUInstancerPro
{
	// Token: 0x02000007 RID: 7
	public abstract class GPUIInputHandler : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000022EA File Offset: 0x000004EA
		public Vector3 MousePosition
		{
			get
			{
				return Input.mousePosition;
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022F1 File Offset: 0x000004F1
		protected virtual void Start()
		{
			this._mouse = Mouse.current;
			this._keyboard = Keyboard.current;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002309 File Offset: 0x00000509
		public bool GetMouseButton(int button)
		{
			return Input.GetMouseButton(button);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002311 File Offset: 0x00000511
		public bool GetMouseButtonUp(int button)
		{
			return Input.GetMouseButtonUp(button);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002319 File Offset: 0x00000519
		public float GetAxis(string axisName)
		{
			if (string.IsNullOrEmpty(axisName))
			{
				return 0f;
			}
			return Input.GetAxis(axisName);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000232F File Offset: 0x0000052F
		public bool GetKey(KeyCode key)
		{
			return Input.GetKey(key);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002337 File Offset: 0x00000537
		public bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUp(key);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000233F File Offset: 0x0000053F
		public bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDown(key);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002348 File Offset: 0x00000548
		private string GetKeyString(KeyCode key)
		{
			if (key <= KeyCode.E)
			{
				if (key == KeyCode.Space)
				{
					return "Space";
				}
				switch (key)
				{
				case KeyCode.Alpha0:
					return "0";
				case KeyCode.Alpha1:
					return "1";
				case KeyCode.Alpha2:
					return "2";
				case KeyCode.Alpha3:
					return "3";
				default:
					switch (key)
					{
					case KeyCode.A:
						return "A";
					case KeyCode.D:
						return "D";
					case KeyCode.E:
						return "E";
					}
					break;
				}
			}
			else if (key <= KeyCode.S)
			{
				if (key == KeyCode.Q)
				{
					return "Q";
				}
				if (key == KeyCode.S)
				{
					return "S";
				}
			}
			else
			{
				if (key == KeyCode.W)
				{
					return "W";
				}
				if (key == KeyCode.LeftShift)
				{
					return "LeftShift";
				}
			}
			string text = key.ToString();
			if (text.StartsWith("Alpha"))
			{
				text = text.Substring(5);
			}
			return text;
		}

		// Token: 0x04000009 RID: 9
		private Mouse _mouse;

		// Token: 0x0400000A RID: 10
		private Keyboard _keyboard;
	}
}
