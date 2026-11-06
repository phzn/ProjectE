using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000100 RID: 256
	public class MMInput : MonoBehaviour
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0001B2F0 File Offset: 0x000194F0
		public static MMInput.ButtonStates ProcessAxisAsButton(string axisName, float threshold, MMInput.ButtonStates currentState, MMInput.AxisTypes AxisType = MMInput.AxisTypes.Positive)
		{
			float axis = Input.GetAxis(axisName);
			MMInput.ButtonStates buttonStates;
			if ((AxisType == MMInput.AxisTypes.Positive) ? (axis < threshold) : (axis > threshold))
			{
				if (currentState == MMInput.ButtonStates.ButtonPressed)
				{
					buttonStates = MMInput.ButtonStates.ButtonUp;
				}
				else
				{
					buttonStates = MMInput.ButtonStates.Off;
				}
			}
			else if (currentState == MMInput.ButtonStates.Off)
			{
				buttonStates = MMInput.ButtonStates.ButtonDown;
			}
			else
			{
				buttonStates = MMInput.ButtonStates.ButtonPressed;
			}
			return buttonStates;
		}

		// Token: 0x0200042B RID: 1067
		public enum ButtonStates
		{
			// Token: 0x04001F69 RID: 8041
			Off,
			// Token: 0x04001F6A RID: 8042
			ButtonDown,
			// Token: 0x04001F6B RID: 8043
			ButtonPressed,
			// Token: 0x04001F6C RID: 8044
			ButtonUp
		}

		// Token: 0x0200042C RID: 1068
		public enum AxisTypes
		{
			// Token: 0x04001F6E RID: 8046
			Positive,
			// Token: 0x04001F6F RID: 8047
			Negative
		}

		// Token: 0x0200042D RID: 1069
		public class IMButton
		{
			// Token: 0x17000306 RID: 774
			// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0006C72A File Offset: 0x0006A92A
			// (set) Token: 0x06001A06 RID: 6662 RVA: 0x0006C732 File Offset: 0x0006A932
			public MMStateMachine<MMInput.ButtonStates> State { get; protected set; }

			// Token: 0x17000307 RID: 775
			// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0006C73B File Offset: 0x0006A93B
			public virtual float TimeSinceLastButtonDown
			{
				get
				{
					return Time.unscaledTime - this._lastButtonDownAt;
				}
			}

			// Token: 0x17000308 RID: 776
			// (get) Token: 0x06001A08 RID: 6664 RVA: 0x0006C749 File Offset: 0x0006A949
			public virtual float TimeSinceLastButtonUp
			{
				get
				{
					return Time.unscaledTime - this._lastButtonUpAt;
				}
			}

			// Token: 0x06001A09 RID: 6665 RVA: 0x0006C757 File Offset: 0x0006A957
			public virtual bool ButtonDownRecently(float time)
			{
				return this.TimeSinceLastButtonDown <= time;
			}

			// Token: 0x06001A0A RID: 6666 RVA: 0x0006C765 File Offset: 0x0006A965
			public virtual bool ButtonUpRecently(float time)
			{
				return this.TimeSinceLastButtonUp <= time;
			}

			// Token: 0x06001A0B RID: 6667 RVA: 0x0006C774 File Offset: 0x0006A974
			public IMButton(string playerID, string buttonID, MMInput.IMButton.ButtonDownMethodDelegate btnDown = null, MMInput.IMButton.ButtonPressedMethodDelegate btnPressed = null, MMInput.IMButton.ButtonUpMethodDelegate btnUp = null)
			{
				this.ButtonID = playerID + "_" + buttonID;
				this.ButtonDownMethod = btnDown;
				this.ButtonUpMethod = btnUp;
				this.ButtonPressedMethod = btnPressed;
				this.State = new MMStateMachine<MMInput.ButtonStates>(null, false);
				this.State.ChangeState(MMInput.ButtonStates.Off);
			}

			// Token: 0x17000309 RID: 777
			// (get) Token: 0x06001A0C RID: 6668 RVA: 0x0006C7C9 File Offset: 0x0006A9C9
			public virtual bool IsPressed
			{
				get
				{
					return this.State.CurrentState == MMInput.ButtonStates.ButtonPressed;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x06001A0D RID: 6669 RVA: 0x0006C7D9 File Offset: 0x0006A9D9
			public virtual bool IsDown
			{
				get
				{
					return this.State.CurrentState == MMInput.ButtonStates.ButtonDown;
				}
			}

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x06001A0E RID: 6670 RVA: 0x0006C7E9 File Offset: 0x0006A9E9
			public virtual bool IsUp
			{
				get
				{
					return this.State.CurrentState == MMInput.ButtonStates.ButtonUp;
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x06001A0F RID: 6671 RVA: 0x0006C7F9 File Offset: 0x0006A9F9
			public virtual bool IsOff
			{
				get
				{
					return this.State.CurrentState == MMInput.ButtonStates.Off;
				}
			}

			// Token: 0x06001A10 RID: 6672 RVA: 0x0006C809 File Offset: 0x0006AA09
			public virtual void TriggerButtonDown()
			{
				this._lastButtonDownAt = Time.unscaledTime;
				if (this.ButtonDownMethod == null)
				{
					this.State.ChangeState(MMInput.ButtonStates.ButtonDown);
					return;
				}
				this.ButtonDownMethod();
			}

			// Token: 0x06001A11 RID: 6673 RVA: 0x0006C836 File Offset: 0x0006AA36
			public virtual void TriggerButtonPressed()
			{
				if (this.ButtonPressedMethod == null)
				{
					this.State.ChangeState(MMInput.ButtonStates.ButtonPressed);
					return;
				}
				this.ButtonPressedMethod();
			}

			// Token: 0x06001A12 RID: 6674 RVA: 0x0006C858 File Offset: 0x0006AA58
			public virtual void TriggerButtonUp()
			{
				this._lastButtonUpAt = Time.unscaledTime;
				if (this.ButtonUpMethod == null)
				{
					this.State.ChangeState(MMInput.ButtonStates.ButtonUp);
					return;
				}
				this.ButtonUpMethod();
			}

			// Token: 0x04001F71 RID: 8049
			public string ButtonID;

			// Token: 0x04001F72 RID: 8050
			public MMInput.IMButton.ButtonDownMethodDelegate ButtonDownMethod;

			// Token: 0x04001F73 RID: 8051
			public MMInput.IMButton.ButtonPressedMethodDelegate ButtonPressedMethod;

			// Token: 0x04001F74 RID: 8052
			public MMInput.IMButton.ButtonUpMethodDelegate ButtonUpMethod;

			// Token: 0x04001F75 RID: 8053
			protected float _lastButtonDownAt;

			// Token: 0x04001F76 RID: 8054
			protected float _lastButtonUpAt;

			// Token: 0x020005A4 RID: 1444
			// (Invoke) Token: 0x06001DEF RID: 7663
			public delegate void ButtonDownMethodDelegate();

			// Token: 0x020005A5 RID: 1445
			// (Invoke) Token: 0x06001DF3 RID: 7667
			public delegate void ButtonPressedMethodDelegate();

			// Token: 0x020005A6 RID: 1446
			// (Invoke) Token: 0x06001DF7 RID: 7671
			public delegate void ButtonUpMethodDelegate();
		}
	}
}
