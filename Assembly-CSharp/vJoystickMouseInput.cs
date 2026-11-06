using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200012F RID: 303
public class vJoystickMouseInput : BaseInput
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x00045DB4 File Offset: 0x00043FB4
	protected override void OnEnable()
	{
		if (this.inputModule)
		{
			this.inputModule.inputOverride = this;
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00045DCF File Offset: 0x00043FCF
	protected override void OnDisable()
	{
		if (this.inputModule)
		{
			this.inputModule.inputOverride = this.oldOverride;
		}
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00045DF0 File Offset: 0x00043FF0
	protected override void Awake()
	{
		base.Awake();
		if (!this.inputModule)
		{
			this.inputModule = Object.FindObjectOfType<StandaloneInputModule>();
		}
		if (this.inputModule)
		{
			this.oldOverride = this.inputModule.inputOverride;
			this.inputModule.inputOverride = this;
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00045E48 File Offset: 0x00044048
	public override Vector2 mousePosition
	{
		get
		{
			if (vJoystickMouseInput.ForceCursorHidden || Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.visible = false;
				if (this.cursor && this.cursor.gameObject.activeSelf)
				{
					this.cursor.gameObject.SetActive(false);
				}
				this.CursorPosition = base.mousePosition;
				return this.CursorPosition;
			}
			if (vInput.instance.inputDevice == InputDevice.Joystick)
			{
				if (this.cursor && (!this.cursor.gameObject.activeSelf || Cursor.visible))
				{
					Cursor.visible = false;
					this.CursorPosition = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
					this.cursor.gameObject.SetActive(true);
					EventSystem.current.SetSelectedGameObject(null);
				}
				this.CursorPosition.x = this.CursorPosition.x + this.joystickAxisInput.horizontalAxis * this.mouseSpeed;
				this.CursorPosition.x = Mathf.Clamp(this.CursorPosition.x, 0f, (float)Screen.width);
				this.CursorPosition.y = this.CursorPosition.y + this.joystickAxisInput.verticalAxis * this.mouseSpeed;
				this.CursorPosition.y = Mathf.Clamp(this.CursorPosition.y, 0f, (float)Screen.height);
			}
			else
			{
				if (this.cursor && this.cursor.gameObject.activeSelf)
				{
					Cursor.visible = true;
					this.cursor.gameObject.SetActive(false);
				}
				this.CursorPosition = base.mousePosition;
			}
			if (this.cursor)
			{
				this.cursor.position = this.CursorPosition;
			}
			return this.CursorPosition;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00046023 File Offset: 0x00044223
	public virtual string submitButton
	{
		get
		{
			return this.inputModule.submitButton;
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x00046030 File Offset: 0x00044230
	public override bool GetMouseButton(int button)
	{
		if (vInput.instance.inputDevice != InputDevice.Joystick)
		{
			return base.GetMouseButton(button);
		}
		if (button == 0)
		{
			return Input.GetButton(this.submitButton);
		}
		return base.GetMouseButton(button);
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0004605D File Offset: 0x0004425D
	public override bool GetMouseButtonUp(int button)
	{
		if (vInput.instance.inputDevice != InputDevice.Joystick)
		{
			return base.GetMouseButtonUp(button);
		}
		if (button == 0)
		{
			return Input.GetButtonUp(this.submitButton);
		}
		return base.GetMouseButtonUp(button);
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0004608A File Offset: 0x0004428A
	public override bool GetMouseButtonDown(int button)
	{
		if (vInput.instance.inputDevice != InputDevice.Joystick)
		{
			return base.GetMouseButtonDown(button);
		}
		if (button == 0)
		{
			return Input.GetButtonDown(this.submitButton);
		}
		return base.GetMouseButtonDown(button);
	}

	// Token: 0x0400094C RID: 2380
	public static bool ForceCursorHidden;

	// Token: 0x0400094D RID: 2381
	public StandaloneInputModule inputModule;

	// Token: 0x0400094E RID: 2382
	public RectTransform cursor;

	// Token: 0x0400094F RID: 2383
	public vJoystickMouseInput.JoystickAxisInput joystickAxisInput;

	// Token: 0x04000950 RID: 2384
	public BaseInput oldOverride;

	// Token: 0x04000951 RID: 2385
	protected Vector2 CursorPosition = Vector2.zero;

	// Token: 0x04000952 RID: 2386
	public float mouseSpeed = 4f;

	// Token: 0x02000130 RID: 304
	[Serializable]
	public class JoystickAxisInput
	{
		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x000460D5 File Offset: 0x000442D5
		public float horizontalAxis
		{
			get
			{
				return Input.GetAxis(this.horizontal);
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000460E2 File Offset: 0x000442E2
		public float verticalAxis
		{
			get
			{
				return Input.GetAxis(this.vertical);
			}
		}

		// Token: 0x04000953 RID: 2387
		public string vertical = "LeftAnalogVertical";

		// Token: 0x04000954 RID: 2388
		public string horizontal = "LeftAnalogHorizontal";
	}
}
