using System;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A50 RID: 2640
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-750)]
	public sealed class ControllerWorldPointer : MonoBehaviour
	{
		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x001D4173 File Offset: 0x001D2373
		public static bool IsWorldPointerActive
		{
			get
			{
				return ControllerWorldPointer._current != null && ControllerWorldPointer._current.isActiveAndEnabled && ControllerInput.IsActive && Time.frameCount - ControllerWorldPointer._current._lastOwnerFrame <= 1;
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x001D41AD File Offset: 0x001D23AD
		public static bool WantsUIFocus
		{
			get
			{
				return ControllerWorldPointer.IsWorldPointerActive && ControllerWorldPointer._current._wantsUIFocus;
			}
		}

		// Token: 0x17000CB5 RID: 3253
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x001D41C2 File Offset: 0x001D23C2
		public static bool BlocksContextInput
		{
			get
			{
				return GameplayInputGate.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen || TextInputFocus.HasFocusedField || ControllerInput.UiInputConsumedThisFrame;
			}
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x001D41E0 File Offset: 0x001D23E0
		public static Vector2 Position(Component owner)
		{
			if (!ControllerInput.IsActive || owner == null)
			{
				return Input.mousePosition;
			}
			ControllerWorldPointer controllerWorldPointer = owner.GetComponent<ControllerWorldPointer>();
			if (controllerWorldPointer == null)
			{
				controllerWorldPointer = owner.gameObject.AddComponent<ControllerWorldPointer>();
			}
			controllerWorldPointer._lastOwnerFrame = Time.frameCount;
			return controllerWorldPointer.Read();
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x001D4235 File Offset: 0x001D2435
		private void Update()
		{
			if (ControllerWorldPointer._current == this && ControllerWorldPointer.IsWorldPointerActive)
			{
				this.Read();
			}
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x001D4254 File Offset: 0x001D2454
		public static void Release(Component owner)
		{
			if (owner == null)
			{
				return;
			}
			ControllerWorldPointer component = owner.GetComponent<ControllerWorldPointer>();
			if (component != null)
			{
				component.OnDisable();
			}
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x001D4284 File Offset: 0x001D2484
		private Vector2 Read()
		{
			ControllerWorldPointer._current = this;
			if (Time.frameCount - this._lastFrame > 1)
			{
				this._wantsUIFocus = false;
			}
			if (!this._initialized)
			{
				this._position = new Vector2((float)Screen.width, (float)Screen.height) * 0.5f;
				this._initialized = true;
			}
			if (this._lastFrame != Time.frameCount)
			{
				if (!ControllerWorldPointer.BlocksContextInput && ControllerInput.GetButtonDown(ControllerButton.Select))
				{
					this._wantsUIFocus = !this._wantsUIFocus;
					ControllerInput.ConsumeUIInput();
				}
				else if (this._wantsUIFocus && !ControllerWorldPointer.BlocksContextInput && ControllerInput.GetButtonDown(ControllerButton.East))
				{
					this._wantsUIFocus = false;
					EscapeInputCoordinator.Consume(this);
					ControllerInput.ConsumeUIInput();
				}
				Vector2 vector = ((this._wantsUIFocus || ControllerWorldPointer.BlocksContextInput || ControllerInput.GetButton(ControllerButton.LeftTrigger)) ? Vector2.zero : Vector2.ClampMagnitude(ControllerInput.Look, 1f));
				this._position = ControllerWorldPointer.ClampPosition(this._position + vector * ((float)Mathf.Min(Screen.width, Screen.height) * 0.65f * Time.unscaledDeltaTime), new Vector2((float)Screen.width, (float)Screen.height));
				this._lastFrame = Time.frameCount;
			}
			return this._position;
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x001D43C4 File Offset: 0x001D25C4
		public static Vector2 ClampPosition(Vector2 position, Vector2 screenSize)
		{
			return new Vector2(Mathf.Clamp(position.x, 0f, Mathf.Max(0f, screenSize.x - 1f)), Mathf.Clamp(position.y, 0f, Mathf.Max(0f, screenSize.y - 1f)));
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x001D4422 File Offset: 0x001D2622
		public static bool Held(ControllerButton button)
		{
			return ControllerInput.IsActive && !ControllerWorldPointer.WantsUIFocus && !ControllerWorldPointer.BlocksContextInput && ControllerInput.GetButton(button);
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x001D4441 File Offset: 0x001D2641
		public static bool Down(ControllerButton button)
		{
			return ControllerInput.IsActive && !ControllerWorldPointer.WantsUIFocus && !ControllerWorldPointer.BlocksContextInput && ControllerInput.GetButtonDown(button) && ((button != ControllerButton.East && button != ControllerButton.Start) || EscapeInputCoordinator.TryConsume(null));
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x001D4474 File Offset: 0x001D2674
		private void OnGUI()
		{
			if (!ControllerInput.IsActive || ControllerWorldPointer.BlocksContextInput || Time.frameCount - this._lastOwnerFrame > 1)
			{
				return;
			}
			Color color = GUI.color;
			float x = this._position.x;
			float num = (float)Screen.height - this._position.y;
			GUI.Label(new Rect(24f, (float)Screen.height - 54f, (float)Screen.width - 48f, 32f), ControllerGlyphs.Plain(this._wantsUIFocus ? "View: return to activity    A: select    B: back" : "RS: point    RT: use    LS: orbit    LB / RB: height    LT + RS: zoom    View: controls", ControllerGlyphs.Family));
			if (this._wantsUIFocus)
			{
				return;
			}
			GUI.color = Color.black;
			GUI.DrawTexture(new Rect(x - 9f, num - 2f, 18f, 4f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x - 2f, num - 9f, 4f, 18f), Texture2D.whiteTexture);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(x - 8f, num - 1f, 16f, 2f), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(x - 1f, num - 8f, 2f, 16f), Texture2D.whiteTexture);
			GUI.color = color;
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x001D45CD File Offset: 0x001D27CD
		private void OnDisable()
		{
			if (ControllerWorldPointer._current == this)
			{
				ControllerWorldPointer._current = null;
			}
			this._wantsUIFocus = false;
			this._lastFrame = -2;
			this._lastOwnerFrame = -2;
		}

		// Token: 0x04004D11 RID: 19729
		private Vector2 _position;

		// Token: 0x04004D12 RID: 19730
		private int _lastFrame = -2;

		// Token: 0x04004D13 RID: 19731
		private int _lastOwnerFrame = -2;

		// Token: 0x04004D14 RID: 19732
		private bool _initialized;

		// Token: 0x04004D15 RID: 19733
		private bool _wantsUIFocus;

		// Token: 0x04004D16 RID: 19734
		private static ControllerWorldPointer _current;
	}
}
