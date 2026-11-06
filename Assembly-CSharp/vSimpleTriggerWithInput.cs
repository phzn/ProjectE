using System;
using Invector;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200010A RID: 266
[vClassHeader("Simple Trigger Input", true, "icon_v2", false, "")]
public class vSimpleTriggerWithInput : vSimpleTrigger
{
	// Token: 0x0600098C RID: 2444 RVA: 0x00044188 File Offset: 0x00042388
	private void Update()
	{
		if (!this.other)
		{
			this._currentInputDelay = this.inputDelay;
			return;
		}
		if (this.inputType == vSimpleTriggerWithInput.InputType.GetButtonDown)
		{
			if (this.actionInput.GetButtonDown())
			{
				this.OnPressButton.Invoke();
				return;
			}
		}
		else if (this.inputType == vSimpleTriggerWithInput.InputType.GetDoubleButton)
		{
			if (this.actionInput.GetDoubleButtonDown(this.doubleButtomTime))
			{
				this.OnPressButton.Invoke();
				return;
			}
		}
		else if (this.inputType == vSimpleTriggerWithInput.InputType.GetButtonTimer)
		{
			if (this._currentInputDelay <= 0f)
			{
				bool flag = false;
				float num = 0f;
				if (this.actionInput.GetButtonTimer(ref num, ref flag, this.buttonTimer))
				{
					this._currentInputDelay = this.inputDelay;
					this.OnPressButton.Invoke();
				}
				if (this.actionInput.inButtomTimer)
				{
					this.UpdateButtonTimer(num);
				}
				if (flag)
				{
					this.CancelButtonTimer();
					return;
				}
			}
			else
			{
				this._currentInputDelay -= Time.deltaTime;
			}
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0004427A File Offset: 0x0004247A
	public void UpdateButtonTimer(float value)
	{
		if (value != this.currentButtonTimer)
		{
			this.currentButtonTimer = value;
			this.OnUpdateButtonTimer.Invoke(value);
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x00044298 File Offset: 0x00042498
	private void CancelButtonTimer()
	{
		this.OnCancelButtonTimer.Invoke();
		this._currentInputDelay = this.inputDelay;
		this.UpdateButtonTimer(0f);
	}

	// Token: 0x040008CC RID: 2252
	public vSimpleTriggerWithInput.InputType inputType;

	// Token: 0x040008CD RID: 2253
	[Tooltip("Input to make the action")]
	public GenericInput actionInput = new GenericInput("E", "A", "A");

	// Token: 0x040008CE RID: 2254
	[vHelpBox("Time you have to hold the button *Only for GetButtonTimer*", vHelpBoxAttribute.MessageType.None)]
	public float buttonTimer = 3f;

	// Token: 0x040008CF RID: 2255
	[vHelpBox("Add delay to start the input count *Only for GetButtonTimer*", vHelpBoxAttribute.MessageType.None)]
	public float inputDelay = 0.1f;

	// Token: 0x040008D0 RID: 2256
	[vHelpBox("Time to press the button twice *Only for GetDoubleButton*", vHelpBoxAttribute.MessageType.None)]
	public float doubleButtomTime = 0.25f;

	// Token: 0x040008D1 RID: 2257
	public float _currentInputDelay;

	// Token: 0x040008D2 RID: 2258
	public float currentButtonTimer;

	// Token: 0x040008D3 RID: 2259
	public UnityEvent OnPressButton;

	// Token: 0x040008D4 RID: 2260
	public UnityEvent OnCancelButtonTimer;

	// Token: 0x040008D5 RID: 2261
	public vSimpleTriggerWithInput.OnUpdateValue OnUpdateButtonTimer;

	// Token: 0x0200010B RID: 267
	public enum InputType
	{
		// Token: 0x040008D7 RID: 2263
		GetButtonDown,
		// Token: 0x040008D8 RID: 2264
		GetDoubleButton,
		// Token: 0x040008D9 RID: 2265
		GetButtonTimer
	}

	// Token: 0x0200010C RID: 268
	[Serializable]
	public class OnUpdateValue : UnityEvent<float>
	{
	}
}
