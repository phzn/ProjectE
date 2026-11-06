using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Cinemachine
{
	// Token: 0x0200004F RID: 79
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineAlternativeInput.html")]
	public class CinemachineInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
	{
		// Token: 0x06000362 RID: 866 RVA: 0x0001522C File Offset: 0x0001342C
		public virtual float GetAxisValue(int axis)
		{
			if (base.enabled)
			{
				InputAction inputAction = this.ResolveForPlayer(axis, (axis == 2) ? this.ZAxis : this.XYAxis);
				if (inputAction != null)
				{
					switch (axis)
					{
					case 0:
						return inputAction.ReadValue<Vector2>().x;
					case 1:
						return inputAction.ReadValue<Vector2>().y;
					case 2:
						return inputAction.ReadValue<float>();
					}
				}
			}
			return 0f;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00015298 File Offset: 0x00013498
		protected InputAction ResolveForPlayer(int axis, InputActionReference actionRef)
		{
			if (axis < 0 || axis >= 3)
			{
				return null;
			}
			if (actionRef == null || actionRef.action == null)
			{
				return null;
			}
			if (this.m_cachedActions == null || this.m_cachedActions.Length != 3)
			{
				this.m_cachedActions = new InputAction[3];
			}
			if (this.m_cachedActions[axis] != null && actionRef.action.id != this.m_cachedActions[axis].id)
			{
				this.m_cachedActions[axis] = null;
			}
			if (this.m_cachedActions[axis] == null)
			{
				this.m_cachedActions[axis] = actionRef.action;
				if (this.PlayerIndex != -1)
				{
					InputAction[] cachedActions = this.m_cachedActions;
					InputUser inputUser = InputUser.all[this.PlayerIndex];
					cachedActions[axis] = CinemachineInputProvider.<ResolveForPlayer>g__GetFirstMatch|7_0(in inputUser, actionRef);
				}
				if (this.AutoEnableInputs && actionRef != null && actionRef.action != null)
				{
					actionRef.action.Enable();
				}
			}
			if (this.m_cachedActions[axis] != null && this.m_cachedActions[axis].enabled != actionRef.action.enabled)
			{
				if (actionRef.action.enabled)
				{
					this.m_cachedActions[axis].Enable();
				}
				else
				{
					this.m_cachedActions[axis].Disable();
				}
			}
			return this.m_cachedActions[axis];
		}

		// Token: 0x06000364 RID: 868 RVA: 0x000153D1 File Offset: 0x000135D1
		protected virtual void OnDisable()
		{
			this.m_cachedActions = null;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000153F0 File Offset: 0x000135F0
		[CompilerGenerated]
		internal static InputAction <ResolveForPlayer>g__GetFirstMatch|7_0(in InputUser user, InputActionReference aRef)
		{
			InputUser inputUser = user;
			return inputUser.actions.First<InputAction>((InputAction x) => x.id == aRef.action.id);
		}

		// Token: 0x0400023C RID: 572
		[Tooltip("Leave this at -1 for single-player games.  For multi-player games, set this to be the player index, and the actions will be read from that player's controls")]
		public int PlayerIndex = -1;

		// Token: 0x0400023D RID: 573
		[Tooltip("If set, Input Actions will be auto-enabled at start")]
		public bool AutoEnableInputs = true;

		// Token: 0x0400023E RID: 574
		[Tooltip("Vector2 action for XY movement")]
		public InputActionReference XYAxis;

		// Token: 0x0400023F RID: 575
		[Tooltip("Float action for Z movement")]
		public InputActionReference ZAxis;

		// Token: 0x04000240 RID: 576
		private const int NUM_AXES = 3;

		// Token: 0x04000241 RID: 577
		private InputAction[] m_cachedActions;
	}
}
