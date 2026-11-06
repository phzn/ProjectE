using System;
using UnityEngine;
using UnityEngine.Events;

namespace MoreMountains.Tools
{
	// Token: 0x02000049 RID: 73
	public class MMDebugMenuSwitch : MMTouchButton
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00007720 File Offset: 0x00005920
		protected override void Initialization()
		{
			base.Initialization();
			this.SwitchState = this.InitialState;
			this.InitializeState();
			this.SwitchKnob.Initialization();
			if (this.InitialState)
			{
				this.SwitchKnob.SwitchToOnSprite();
				return;
			}
			this.SwitchKnob.SwitchToOffSprite();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000776F File Offset: 0x0000596F
		public virtual void InitializeState()
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007774 File Offset: 0x00005974
		public virtual void SetTrue()
		{
			this.SwitchState = true;
			if (this._animator != null)
			{
				this._animator.SetTrigger("Right");
			}
			this.SwitchKnob.SwitchToOnSprite();
			if (this.OnSwitchOn != null)
			{
				this.OnSwitchOn.Invoke();
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000077C4 File Offset: 0x000059C4
		public virtual void SetFalse()
		{
			this.SwitchState = false;
			if (this._animator != null)
			{
				this._animator.SetTrigger("Left");
			}
			this.SwitchKnob.SwitchToOffSprite();
			if (this.OnSwitchOff != null)
			{
				this.OnSwitchOff.Invoke();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007814 File Offset: 0x00005A14
		public virtual void ToggleState()
		{
			if (!this.SwitchState)
			{
				this.SetTrue();
			}
			else
			{
				this.SetFalse();
			}
			UnityEvent<bool> onSwitchToggle = this.OnSwitchToggle;
			if (onSwitchToggle == null)
			{
				return;
			}
			onSwitchToggle.Invoke(this.SwitchState);
		}

		// Token: 0x040001E0 RID: 480
		[Header("Switch")]
		public MMDebugMenuSpriteReplace SwitchKnob;

		// Token: 0x040001E1 RID: 481
		[MMReadOnly]
		public bool SwitchState;

		// Token: 0x040001E2 RID: 482
		public bool InitialState;

		// Token: 0x040001E3 RID: 483
		[Header("Binding")]
		public UnityEvent OnSwitchOn;

		// Token: 0x040001E4 RID: 484
		public UnityEvent OnSwitchOff;

		// Token: 0x040001E5 RID: 485
		public UnityEvent<bool> OnSwitchToggle;
	}
}
