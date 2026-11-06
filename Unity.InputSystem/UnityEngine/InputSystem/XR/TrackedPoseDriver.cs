using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.XR;

namespace UnityEngine.InputSystem.XR
{
	// Token: 0x02000068 RID: 104
	[AddComponentMenu("XR/Tracked Pose Driver (Input System)")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.inputsystem@1.14/manual/TrackedInputDevices.html#tracked-pose-driver")]
	[Serializable]
	public class TrackedPoseDriver : MonoBehaviour, ISerializationCallbackReceiver
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x000371D3 File Offset: 0x000353D3
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x000371DB File Offset: 0x000353DB
		public TrackedPoseDriver.TrackingType trackingType
		{
			get
			{
				return this.m_TrackingType;
			}
			set
			{
				this.m_TrackingType = value;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x000371E4 File Offset: 0x000353E4
		// (set) Token: 0x060009F6 RID: 2550 RVA: 0x000371EC File Offset: 0x000353EC
		public TrackedPoseDriver.UpdateType updateType
		{
			get
			{
				return this.m_UpdateType;
			}
			set
			{
				this.m_UpdateType = value;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060009F7 RID: 2551 RVA: 0x000371F5 File Offset: 0x000353F5
		// (set) Token: 0x060009F8 RID: 2552 RVA: 0x000371FD File Offset: 0x000353FD
		public bool ignoreTrackingState
		{
			get
			{
				return this.m_IgnoreTrackingState;
			}
			set
			{
				this.m_IgnoreTrackingState = value;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00037206 File Offset: 0x00035406
		// (set) Token: 0x060009FA RID: 2554 RVA: 0x0003720E File Offset: 0x0003540E
		public InputActionProperty positionInput
		{
			get
			{
				return this.m_PositionInput;
			}
			set
			{
				if (Application.isPlaying)
				{
					this.UnbindPosition();
				}
				this.m_PositionInput = value;
				if (Application.isPlaying && base.isActiveAndEnabled)
				{
					this.BindPosition();
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00037239 File Offset: 0x00035439
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00037241 File Offset: 0x00035441
		public InputActionProperty rotationInput
		{
			get
			{
				return this.m_RotationInput;
			}
			set
			{
				if (Application.isPlaying)
				{
					this.UnbindRotation();
				}
				this.m_RotationInput = value;
				if (Application.isPlaying && base.isActiveAndEnabled)
				{
					this.BindRotation();
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0003726C File Offset: 0x0003546C
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x00037274 File Offset: 0x00035474
		public InputActionProperty trackingStateInput
		{
			get
			{
				return this.m_TrackingStateInput;
			}
			set
			{
				if (Application.isPlaying)
				{
					this.UnbindTrackingState();
				}
				this.m_TrackingStateInput = value;
				if (Application.isPlaying && base.isActiveAndEnabled)
				{
					this.BindTrackingState();
				}
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0003729F File Offset: 0x0003549F
		private void BindActions()
		{
			this.BindPosition();
			this.BindRotation();
			this.BindTrackingState();
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x000372B3 File Offset: 0x000354B3
		private void UnbindActions()
		{
			this.UnbindPosition();
			this.UnbindRotation();
			this.UnbindTrackingState();
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x000372C8 File Offset: 0x000354C8
		private void BindPosition()
		{
			if (this.m_PositionBound)
			{
				return;
			}
			InputAction action = this.m_PositionInput.action;
			if (action == null)
			{
				return;
			}
			action.performed += this.OnPositionPerformed;
			action.canceled += this.OnPositionCanceled;
			this.m_PositionBound = true;
			if (this.m_PositionInput.reference == null)
			{
				TrackedPoseDriver.RenameAndEnable(action, base.gameObject.name + " - TPD - Position");
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00037348 File Offset: 0x00035548
		private void BindRotation()
		{
			if (this.m_RotationBound)
			{
				return;
			}
			InputAction action = this.m_RotationInput.action;
			if (action == null)
			{
				return;
			}
			action.performed += this.OnRotationPerformed;
			action.canceled += this.OnRotationCanceled;
			this.m_RotationBound = true;
			if (this.m_RotationInput.reference == null)
			{
				TrackedPoseDriver.RenameAndEnable(action, base.gameObject.name + " - TPD - Rotation");
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000373C8 File Offset: 0x000355C8
		private void BindTrackingState()
		{
			if (this.m_TrackingStateBound)
			{
				return;
			}
			InputAction action = this.m_TrackingStateInput.action;
			if (action == null)
			{
				return;
			}
			action.performed += this.OnTrackingStatePerformed;
			action.canceled += this.OnTrackingStateCanceled;
			this.m_TrackingStateBound = true;
			if (this.m_TrackingStateInput.reference == null)
			{
				TrackedPoseDriver.RenameAndEnable(action, base.gameObject.name + " - TPD - Tracking State");
			}
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00037447 File Offset: 0x00035647
		private static void RenameAndEnable(InputAction action, string name)
		{
			action.Rename(name);
			action.Enable();
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00037458 File Offset: 0x00035658
		private void UnbindPosition()
		{
			if (!this.m_PositionBound)
			{
				return;
			}
			InputAction action = this.m_PositionInput.action;
			if (action == null)
			{
				return;
			}
			if (this.m_PositionInput.reference == null)
			{
				action.Disable();
			}
			action.performed -= this.OnPositionPerformed;
			action.canceled -= this.OnPositionCanceled;
			this.m_PositionBound = false;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x000374C4 File Offset: 0x000356C4
		private void UnbindRotation()
		{
			if (!this.m_RotationBound)
			{
				return;
			}
			InputAction action = this.m_RotationInput.action;
			if (action == null)
			{
				return;
			}
			if (this.m_RotationInput.reference == null)
			{
				action.Disable();
			}
			action.performed -= this.OnRotationPerformed;
			action.canceled -= this.OnRotationCanceled;
			this.m_RotationBound = false;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00037530 File Offset: 0x00035730
		private void UnbindTrackingState()
		{
			if (!this.m_TrackingStateBound)
			{
				return;
			}
			InputAction action = this.m_TrackingStateInput.action;
			if (action == null)
			{
				return;
			}
			if (this.m_TrackingStateInput.reference == null)
			{
				action.Disable();
			}
			action.performed -= this.OnTrackingStatePerformed;
			action.canceled -= this.OnTrackingStateCanceled;
			this.m_TrackingStateBound = false;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0003759A File Offset: 0x0003579A
		private void OnPositionPerformed(InputAction.CallbackContext context)
		{
			this.m_CurrentPosition = context.ReadValue<Vector3>();
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000375A9 File Offset: 0x000357A9
		private void OnPositionCanceled(InputAction.CallbackContext context)
		{
			this.m_CurrentPosition = Vector3.zero;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x000375B6 File Offset: 0x000357B6
		private void OnRotationPerformed(InputAction.CallbackContext context)
		{
			this.m_CurrentRotation = context.ReadValue<Quaternion>();
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x000375C5 File Offset: 0x000357C5
		private void OnRotationCanceled(InputAction.CallbackContext context)
		{
			this.m_CurrentRotation = Quaternion.identity;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000375D2 File Offset: 0x000357D2
		private void OnTrackingStatePerformed(InputAction.CallbackContext context)
		{
			this.m_CurrentTrackingState = (TrackedPoseDriver.TrackingStates)context.ReadValue<int>();
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000375E1 File Offset: 0x000357E1
		private void OnTrackingStateCanceled(InputAction.CallbackContext context)
		{
			this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.None;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000375EC File Offset: 0x000357EC
		protected void Reset()
		{
			this.m_PositionInput = new InputActionProperty(new InputAction("Position", InputActionType.Value, null, null, null, "Vector3"));
			this.m_RotationInput = new InputActionProperty(new InputAction("Rotation", InputActionType.Value, null, null, null, "Quaternion"));
			this.m_TrackingStateInput = new InputActionProperty(new InputAction("Tracking State", InputActionType.Value, null, null, null, "Integer"));
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00037654 File Offset: 0x00035854
		protected virtual void Awake()
		{
			Camera camera;
			if (this.HasStereoCamera(out camera))
			{
				XRDevice.DisableAutoXRCameraTracking(camera, true);
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00037672 File Offset: 0x00035872
		protected void OnEnable()
		{
			InputSystem.onAfterUpdate += this.UpdateCallback;
			InputSystem.onDeviceChange += this.OnDeviceChanged;
			this.BindActions();
			this.m_IsFirstUpdate = true;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x000376A3 File Offset: 0x000358A3
		protected void OnDisable()
		{
			this.UnbindActions();
			InputSystem.onAfterUpdate -= this.UpdateCallback;
			InputSystem.onDeviceChange -= this.OnDeviceChanged;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000376D0 File Offset: 0x000358D0
		protected virtual void OnDestroy()
		{
			Camera camera;
			if (this.HasStereoCamera(out camera))
			{
				XRDevice.DisableAutoXRCameraTracking(camera, false);
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000376F0 File Offset: 0x000358F0
		protected void UpdateCallback()
		{
			if (this.m_IsFirstUpdate)
			{
				if (TrackedPoseDriver.HasResolvedControl(this.m_PositionInput.action))
				{
					this.m_CurrentPosition = this.m_PositionInput.action.ReadValue<Vector3>();
				}
				else
				{
					this.m_CurrentPosition = base.transform.localPosition;
				}
				if (TrackedPoseDriver.HasResolvedControl(this.m_RotationInput.action))
				{
					this.m_CurrentRotation = this.m_RotationInput.action.ReadValue<Quaternion>();
				}
				else
				{
					this.m_CurrentRotation = base.transform.localRotation;
				}
				this.ReadTrackingState();
				this.m_IsFirstUpdate = false;
			}
			if (InputState.currentUpdateType == InputUpdateType.BeforeRender)
			{
				this.OnBeforeRender();
				return;
			}
			this.OnUpdate();
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000377A0 File Offset: 0x000359A0
		private void OnDeviceChanged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
		{
			if (this.m_IsFirstUpdate)
			{
				return;
			}
			this.ReadTrackingStateWithoutTrackingAction();
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000377B4 File Offset: 0x000359B4
		private void ReadTrackingStateWithoutTrackingAction()
		{
			InputAction action = this.m_TrackingStateInput.action;
			if (action != null && action.m_BindingsCount != 0)
			{
				return;
			}
			bool flag = TrackedPoseDriver.HasResolvedControl(this.m_PositionInput.action);
			bool flag2 = TrackedPoseDriver.HasResolvedControl(this.m_RotationInput.action);
			if (flag && flag2)
			{
				this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.Position | TrackedPoseDriver.TrackingStates.Rotation;
				return;
			}
			if (flag)
			{
				this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.Position;
				return;
			}
			if (flag2)
			{
				this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.Rotation;
				return;
			}
			this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.None;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00037828 File Offset: 0x00035A28
		private void ReadTrackingState()
		{
			InputAction action = this.m_TrackingStateInput.action;
			if (action != null && !action.enabled)
			{
				this.m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.None;
				return;
			}
			if (TrackedPoseDriver.HasResolvedControl(action))
			{
				this.m_CurrentTrackingState = (TrackedPoseDriver.TrackingStates)action.ReadValue<int>();
				return;
			}
			this.ReadTrackingStateWithoutTrackingAction();
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0003786F File Offset: 0x00035A6F
		protected virtual void OnUpdate()
		{
			if (this.m_UpdateType == TrackedPoseDriver.UpdateType.Update || this.m_UpdateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender)
			{
				this.PerformUpdate();
			}
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00037888 File Offset: 0x00035A88
		protected virtual void OnBeforeRender()
		{
			if (this.m_UpdateType == TrackedPoseDriver.UpdateType.BeforeRender || this.m_UpdateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender)
			{
				this.PerformUpdate();
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x000378A1 File Offset: 0x00035AA1
		protected virtual void PerformUpdate()
		{
			this.SetLocalTransform(this.m_CurrentPosition, this.m_CurrentRotation);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000378B8 File Offset: 0x00035AB8
		protected virtual void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
		{
			bool flag = this.m_IgnoreTrackingState || (this.m_CurrentTrackingState & TrackedPoseDriver.TrackingStates.Position) > TrackedPoseDriver.TrackingStates.None;
			bool flag2 = this.m_IgnoreTrackingState || (this.m_CurrentTrackingState & TrackedPoseDriver.TrackingStates.Rotation) > TrackedPoseDriver.TrackingStates.None;
			if (this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationAndPosition && flag2 && flag)
			{
				base.transform.SetLocalPositionAndRotation(newPosition, newRotation);
				return;
			}
			if (flag2 && (this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationAndPosition || this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationOnly))
			{
				base.transform.localRotation = newRotation;
			}
			if (flag && (this.m_TrackingType == TrackedPoseDriver.TrackingType.RotationAndPosition || this.m_TrackingType == TrackedPoseDriver.TrackingType.PositionOnly))
			{
				base.transform.localPosition = newPosition;
			}
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00037950 File Offset: 0x00035B50
		private bool HasStereoCamera(out Camera cameraComponent)
		{
			return base.TryGetComponent<Camera>(out cameraComponent) && cameraComponent.stereoEnabled;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00037964 File Offset: 0x00035B64
		private unsafe static bool HasResolvedControl(InputAction action)
		{
			if (action == null)
			{
				return false;
			}
			InputActionMap orCreateActionMap = action.GetOrCreateActionMap();
			orCreateActionMap.ResolveBindingsIfNecessary();
			InputActionState state = orCreateActionMap.m_State;
			if (state == null)
			{
				return false;
			}
			int actionIndexInState = action.m_ActionIndexInState;
			int totalBindingCount = state.totalBindingCount;
			for (int i = 0; i < totalBindingCount; i++)
			{
				ref InputActionState.BindingState ptr = ref state.bindingStates[i];
				if (ptr.actionIndex == actionIndexInState && !ptr.isComposite && ptr.controlCount > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000379DA File Offset: 0x00035BDA
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000379E7 File Offset: 0x00035BE7
		public InputAction positionAction
		{
			get
			{
				return this.m_PositionInput.action;
			}
			set
			{
				this.positionInput = new InputActionProperty(value);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000379F5 File Offset: 0x00035BF5
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x00037A02 File Offset: 0x00035C02
		public InputAction rotationAction
		{
			get
			{
				return this.m_RotationInput.action;
			}
			set
			{
				this.rotationInput = new InputActionProperty(value);
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00037A10 File Offset: 0x00035C10
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00037A14 File Offset: 0x00035C14
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.m_PositionInput.serializedReference == null && this.m_PositionInput.serializedAction == null && this.m_PositionAction != null)
			{
				this.m_PositionInput = new InputActionProperty(this.m_PositionAction);
			}
			if (this.m_RotationInput.serializedReference == null && this.m_RotationInput.serializedAction == null && this.m_RotationAction != null)
			{
				this.m_RotationInput = new InputActionProperty(this.m_RotationAction);
			}
		}

		// Token: 0x0400034D RID: 845
		[SerializeField]
		[Tooltip("Which Transform properties to update.")]
		private TrackedPoseDriver.TrackingType m_TrackingType;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		[Tooltip("Updates the Transform properties after these phases of Input System event processing.")]
		private TrackedPoseDriver.UpdateType m_UpdateType;

		// Token: 0x0400034F RID: 847
		[SerializeField]
		[Tooltip("Ignore Tracking State and always treat the input pose as valid.")]
		private bool m_IgnoreTrackingState;

		// Token: 0x04000350 RID: 848
		[SerializeField]
		[Tooltip("The input action to read the position value of a tracked device. Must be a Vector 3 control type.")]
		private InputActionProperty m_PositionInput;

		// Token: 0x04000351 RID: 849
		[SerializeField]
		[Tooltip("The input action to read the rotation value of a tracked device. Must be a Quaternion control type.")]
		private InputActionProperty m_RotationInput;

		// Token: 0x04000352 RID: 850
		[SerializeField]
		[Tooltip("The input action to read the tracking state value of a tracked device. Identifies if position and rotation have valid data. Must be an Integer control type.")]
		private InputActionProperty m_TrackingStateInput;

		// Token: 0x04000353 RID: 851
		private Vector3 m_CurrentPosition = Vector3.zero;

		// Token: 0x04000354 RID: 852
		private Quaternion m_CurrentRotation = Quaternion.identity;

		// Token: 0x04000355 RID: 853
		private TrackedPoseDriver.TrackingStates m_CurrentTrackingState = TrackedPoseDriver.TrackingStates.Position | TrackedPoseDriver.TrackingStates.Rotation;

		// Token: 0x04000356 RID: 854
		private bool m_RotationBound;

		// Token: 0x04000357 RID: 855
		private bool m_PositionBound;

		// Token: 0x04000358 RID: 856
		private bool m_TrackingStateBound;

		// Token: 0x04000359 RID: 857
		private bool m_IsFirstUpdate = true;

		// Token: 0x0400035A RID: 858
		[Obsolete]
		[SerializeField]
		[HideInInspector]
		private InputAction m_PositionAction;

		// Token: 0x0400035B RID: 859
		[Obsolete]
		[SerializeField]
		[HideInInspector]
		private InputAction m_RotationAction;

		// Token: 0x020001C5 RID: 453
		public enum TrackingType
		{
			// Token: 0x0400095D RID: 2397
			RotationAndPosition,
			// Token: 0x0400095E RID: 2398
			RotationOnly,
			// Token: 0x0400095F RID: 2399
			PositionOnly
		}

		// Token: 0x020001C6 RID: 454
		[Flags]
		private enum TrackingStates
		{
			// Token: 0x04000961 RID: 2401
			None = 0,
			// Token: 0x04000962 RID: 2402
			Position = 1,
			// Token: 0x04000963 RID: 2403
			Rotation = 2
		}

		// Token: 0x020001C7 RID: 455
		public enum UpdateType
		{
			// Token: 0x04000965 RID: 2405
			UpdateAndBeforeRender,
			// Token: 0x04000966 RID: 2406
			Update,
			// Token: 0x04000967 RID: 2407
			BeforeRender
		}
	}
}
