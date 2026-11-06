using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BoZo.CharacterCreator;
using Features.CielVoice;
using Features.Localization;
using Features.MainMenu;
using MagicaCloth2;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Features.BoobTouch
{
	// Token: 0x020014E6 RID: 5350
	[DefaultExecutionOrder(110)]
	[RequireComponent(typeof(BoobTouchModeController))]
	public class GropingModeController : MonoBehaviour
	{
		// Token: 0x0600A4C9 RID: 42185 RVA: 0x0044BCA8 File Offset: 0x00449EA8
		public static string GetVaginalEndpointPositionKey(int endpointIndex, string axis)
		{
			return "Groping.VaginalCalibration.Endpoint" + ((endpointIndex <= 0) ? 0 : 1).ToString() + ".Position." + axis;
		}

		// Token: 0x0600A4CA RID: 42186 RVA: 0x0044BCD8 File Offset: 0x00449ED8
		public static string GetVaginalEndpointEulerKey(int endpointIndex, string axis)
		{
			return "Groping.VaginalCalibration.Endpoint" + ((endpointIndex <= 0) ? 0 : 1).ToString() + ".Euler." + axis;
		}

		// Token: 0x0600A4CB RID: 42187 RVA: 0x0044BD08 File Offset: 0x00449F08
		public static string GetPinchCalibrationPositionKey(int targetIndex, string axis)
		{
			return "Groping.PinchCalibration.Target" + GropingModeController.CalibrationProfileIndex(targetIndex).ToString() + ".Position." + axis;
		}

		// Token: 0x0600A4CC RID: 42188 RVA: 0x0044BD34 File Offset: 0x00449F34
		public static string GetPinchCalibrationEulerKey(int targetIndex, string axis)
		{
			return "Groping.PinchCalibration.Target" + GropingModeController.CalibrationProfileIndex(targetIndex).ToString() + ".Euler." + axis;
		}

		// Token: 0x0600A4CD RID: 42189 RVA: 0x0044BD60 File Offset: 0x00449F60
		public static string GetPinchCalibrationPositionKey(int boobsType, int targetIndex, string axis)
		{
			return string.Concat(new string[]
			{
				"Groping.PinchCalibration.Size",
				Mathf.Clamp(boobsType, 0, 5).ToString(),
				".Target",
				GropingModeController.CalibrationProfileIndex(targetIndex).ToString(),
				".Position.",
				axis
			});
		}

		// Token: 0x0600A4CE RID: 42190 RVA: 0x0044BDB8 File Offset: 0x00449FB8
		public static string GetPinchCalibrationEulerKey(int boobsType, int targetIndex, string axis)
		{
			return string.Concat(new string[]
			{
				"Groping.PinchCalibration.Size",
				Mathf.Clamp(boobsType, 0, 5).ToString(),
				".Target",
				GropingModeController.CalibrationProfileIndex(targetIndex).ToString(),
				".Euler.",
				axis
			});
		}

		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600A4CF RID: 42191 RVA: 0x0044BE10 File Offset: 0x0044A010
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x0600A4D0 RID: 42192 RVA: 0x0044BE18 File Offset: 0x0044A018
		public bool IsPinchCalibrationActive
		{
			get
			{
				return this._pinchCalibrationActive;
			}
		}

		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x0600A4D1 RID: 42193 RVA: 0x0044BE20 File Offset: 0x0044A020
		public bool IsVaginalCalibrationActive
		{
			get
			{
				return this._vaginalCalibrationActive;
			}
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x0600A4D2 RID: 42194 RVA: 0x0044BE28 File Offset: 0x0044A028
		public Vector3 RuntimeVaginalAnchorLocalOffset
		{
			get
			{
				return this._runtimeVaginalAnchorLocalOffset;
			}
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x0600A4D3 RID: 42195 RVA: 0x0044BE30 File Offset: 0x0044A030
		public Vector3 RuntimeVaginalHandEuler
		{
			get
			{
				return this._runtimeVaginalHandEuler;
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600A4D4 RID: 42196 RVA: 0x0044BE38 File Offset: 0x0044A038
		public Vector3 RuntimeVaginalEndpointPosition
		{
			get
			{
				return this._runtimeVaginalEndpointPositions[this._vaginalCalibrationInsertedPreview ? 1 : 0];
			}
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600A4D5 RID: 42197 RVA: 0x0044BE51 File Offset: 0x0044A051
		public Vector3 RuntimeVaginalEndpointEuler
		{
			get
			{
				return this._runtimeVaginalEndpointEulers[this._vaginalCalibrationInsertedPreview ? 1 : 0];
			}
		}

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x0600A4D6 RID: 42198 RVA: 0x0044BE6A File Offset: 0x0044A06A
		public Vector3 RuntimePinchCalibrationPosition
		{
			get
			{
				return this.GetRuntimePinchCalibrationPosition(this._pinchCalibrationTargetIndex);
			}
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600A4D7 RID: 42199 RVA: 0x0044BE78 File Offset: 0x0044A078
		public Vector3 RuntimePinchCalibrationEuler
		{
			get
			{
				return this.GetRuntimePinchCalibrationEuler(this._pinchCalibrationTargetIndex);
			}
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600A4D8 RID: 42200 RVA: 0x0044BE86 File Offset: 0x0044A086
		public int HandCount
		{
			get
			{
				return this._hands.Count;
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x0600A4D9 RID: 42201 RVA: 0x0044BE93 File Offset: 0x0044A093
		public int CurrentReactionFaceIndex
		{
			get
			{
				return this._currentReactionFaceIndex;
			}
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600A4DA RID: 42202 RVA: 0x0044BE9B File Offset: 0x0044A09B
		public bool IsOrgasmReactionActive
		{
			get
			{
				return this._orgasmReactionActive;
			}
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600A4DB RID: 42203 RVA: 0x0044BEA3 File Offset: 0x0044A0A3
		public bool IsFaceCameraVisible
		{
			get
			{
				return this.faceCameraOverlay != null && this.faceCameraOverlay.IsActuallyVisible;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600A4DC RID: 42204 RVA: 0x0044BEC0 File Offset: 0x0044A0C0
		public bool IsNipplePinchVoiceLoopPlaying
		{
			get
			{
				return this._nipplePinchVoiceLoop != null && this._nipplePinchVoiceLoop.IsPlaying;
			}
		}

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x0600A4DD RID: 42205 RVA: 0x0044BED7 File Offset: 0x0044A0D7
		public bool IsVaginalVoiceLoopPlaying
		{
			get
			{
				return this._vaginalVoiceLoop != null && this._vaginalVoiceLoop.IsPlaying;
			}
		}

		// Token: 0x1700196B RID: 6507
		// (get) Token: 0x0600A4DE RID: 42206 RVA: 0x0044BEEE File Offset: 0x0044A0EE
		public float CurrentNipplePinchVoiceVolume
		{
			get
			{
				return this._nipplePinchVoiceVolume;
			}
		}

		// Token: 0x1700196C RID: 6508
		// (get) Token: 0x0600A4DF RID: 42207 RVA: 0x0044BEF6 File Offset: 0x0044A0F6
		public float CurrentVaginalVoiceVolume
		{
			get
			{
				return this._vaginalVoiceVolume;
			}
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600A4E0 RID: 42208 RVA: 0x0044BEFE File Offset: 0x0044A0FE
		public string CurrentVoiceChannel
		{
			get
			{
				return this._activeVoiceChannel.ToString();
			}
		}

		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600A4E1 RID: 42209 RVA: 0x0044BF11 File Offset: 0x0044A111
		public int AudibleVoiceChannelCount
		{
			get
			{
				return this.CountAudibleVoiceChannels();
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x0600A4E2 RID: 42210 RVA: 0x0044BF19 File Offset: 0x0044A119
		public int MaximumSimultaneousVoiceChannels
		{
			get
			{
				return this._maximumSimultaneousVoiceChannels;
			}
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x0600A4E3 RID: 42211 RVA: 0x0044BF21 File Offset: 0x0044A121
		public GropingModeController.GrabPoseMode SelectedGrabMode
		{
			get
			{
				return this._selectedGrabMode;
			}
		}

		// Token: 0x0600A4E4 RID: 42212 RVA: 0x0044BF2C File Offset: 0x0044A12C
		public bool RequestStimulationVoice(CielVoiceSO voice, float subtitleHoldDuration)
		{
			if (!this._active || voice == null || CielVoiceController.Instance == null)
			{
				return false;
			}
			this._queuedStimulationVoice = voice;
			this._queuedStimulationSubtitleHold = Mathf.Max(0.1f, subtitleHoldDuration);
			this._stimulationVoiceRequested = true;
			return true;
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x0600A4E5 RID: 42213 RVA: 0x0044BF7C File Offset: 0x0044A17C
		public int LatchedGrabCount
		{
			get
			{
				int num = 0;
				using (List<GropingModeController.RuntimeHand>.Enumerator enumerator = this._hands.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsGrabLatched)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x0600A4E6 RID: 42214 RVA: 0x0044BFD8 File Offset: 0x0044A1D8
		public bool ActiveHandHasSurfaceContact
		{
			get
			{
				return this._activeHand != null && this._activeHand.HasSurfaceContact;
			}
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x0600A4E7 RID: 42215 RVA: 0x0044BFEF File Offset: 0x0044A1EF
		public bool IsAwaitingPointerMove
		{
			get
			{
				return this._activeHandAwaitingPointerMove;
			}
		}

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600A4E8 RID: 42216 RVA: 0x0044BFF8 File Offset: 0x0044A1F8
		public int CollisionVolumeCount
		{
			get
			{
				int num = 0;
				foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
				{
					num += runtimeHand.ColliderCount;
				}
				return num;
			}
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x0600A4E9 RID: 42217 RVA: 0x0044C050 File Offset: 0x0044A250
		public int EnabledCollisionVolumeCount
		{
			get
			{
				int num = 0;
				foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
				{
					num += runtimeHand.EnabledColliderCount;
				}
				return num;
			}
		}

		// Token: 0x0600A4EA RID: 42218 RVA: 0x0044C0A8 File Offset: 0x0044A2A8
		private void Awake()
		{
			this._backend = base.GetComponent<BoobTouchModeController>();
			this._stimulation = base.GetComponent<SoapNippleStimulation>();
			if (this.faceCameraOverlay == null)
			{
				this.faceCameraOverlay = base.GetComponent<GropingFaceCameraOverlay>();
			}
			if (this.faceCameraOverlay == null)
			{
				this.faceCameraOverlay = base.gameObject.AddComponent<GropingFaceCameraOverlay>();
			}
			this.EnsureActionVoiceLoops();
			if (this._backend != null)
			{
				this._backend.ExternalBreastGrabSolved += this.HandleExternalBreastGrabSolved;
				this._activeBoobsType = this._backend.CurrentBoobsTypeValue;
			}
			this.LoadPinchCalibration();
		}

		// Token: 0x0600A4EB RID: 42219 RVA: 0x0044C148 File Offset: 0x0044A348
		private void OnDestroy()
		{
			if (this._backend != null)
			{
				this._backend.ExternalBreastGrabSolved -= this.HandleExternalBreastGrabSolved;
			}
			this.StopActionVoiceLoops();
		}

		// Token: 0x0600A4EC RID: 42220 RVA: 0x0044C178 File Offset: 0x0044A378
		private void Update()
		{
			this.TickActionVoiceLoops();
			if (!this._active)
			{
				if (Time.unscaledTime <= this._manualEntryRequestUntil && this.EnterMode())
				{
					this._manualEntryRequestUntil = -1f;
				}
				return;
			}
			if (this._backend == null || !this._backend.IsExternalInteractorMode)
			{
				this.FinishExit(false);
				return;
			}
			ControllerWorldPointer.Position(this);
			if (this.ExitPressed())
			{
				this.ExitMode();
				return;
			}
			if ((this._vaginalCalibrationActive || this._pinchCalibrationActive) && EscapeInputCoordinator.TryConsume(this))
			{
				if (this._vaginalCalibrationActive)
				{
					this.SetVaginalCalibrationActive(false);
					return;
				}
				if (this._pinchCalibrationActive)
				{
					this.SetPinchCalibrationActive(false);
					return;
				}
			}
			this.RefreshActiveBreastSizeIfChanged();
			this.EnforceGrabModeAvailability();
			if (this.pinchCalibrationKey != KeyCode.None && GameplayInput.GetKeyDown(this.pinchCalibrationKey))
			{
				this.SetPinchCalibrationActive(!this._pinchCalibrationActive);
			}
			if (this.vaginalCalibrationKey != KeyCode.None && GameplayInput.GetKeyDown(this.vaginalCalibrationKey))
			{
				this.SetVaginalCalibrationActive(!this._vaginalCalibrationActive);
			}
			this.ReadModeCommands();
			float num;
			if (this._pinchCalibrationActive)
			{
				this.UpdatePinchCalibrationInput();
				this._activeGripRequested = false;
				num = this.pinchCalibrationGrip;
			}
			else if (this._vaginalCalibrationActive)
			{
				this.UpdateVaginalCalibrationInput();
				this._activeGripRequested = false;
				num = 1f;
			}
			else
			{
				bool flag = (ControllerInput.IsActive ? ControllerWorldPointer.Held(ControllerButton.RightTrigger) : GameplayInput.GetMouseButton(0));
				if (!this._initialGrabInputArmed)
				{
					if (!flag)
					{
						this._initialGrabInputArmed = true;
					}
					this._activeGripRequested = false;
				}
				else
				{
					this._activeGripRequested = this._activeHand != null && flag;
				}
				num = (this._activeGripRequested ? 1f : 0f);
			}
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				runtimeHand.TickGrip((runtimeHand == this._activeHand) ? num : runtimeHand.LockedGrip, this.gripInSpeed, this.gripOutSpeed, Time.unscaledDeltaTime);
			}
			if (!this._vaginalCalibrationActive)
			{
				this.UpdateVaginalInsertionInput();
			}
		}

		// Token: 0x0600A4ED RID: 42221 RVA: 0x0044C388 File Offset: 0x0044A588
		private void LateUpdate()
		{
			if (!this._active || this._backend == null)
			{
				return;
			}
			Transform interactionChestAnchor = this._backend.InteractionChestAnchor;
			Camera interactionCamera = this._backend.InteractionCamera;
			if (interactionChestAnchor == null || interactionCamera == null)
			{
				this._backend.ReportExternalBreastTouch(0f);
				this.ResetInteractionPresentation();
				return;
			}
			Vector2 boundedInteractionScreenPoint = this.GetBoundedInteractionScreenPoint(interactionCamera);
			this._interactionScreenPoint = boundedInteractionScreenPoint;
			if (this._activeHand != null && this._activeHandAwaitingPointerMove)
			{
				Vector3 vector = interactionCamera.WorldToScreenPoint(interactionChestAnchor.TransformPoint(this._activeHandNeutralLocalPoint));
				this._neutralHandScreenPoint = this.ClampInteractionScreenPoint(interactionCamera, new Vector2(vector.x, vector.y));
				this._interactionScreenPoint = this._neutralHandScreenPoint;
				Vector2 vector2 = boundedInteractionScreenPoint - this._pointerPositionAtHandSpawn;
				this._pointerPositionAtHandSpawn = boundedInteractionScreenPoint;
				if (Time.unscaledTime >= this._pointerSettlesAt)
				{
					float num = Mathf.Max(80f, (float)Mathf.Min(interactionCamera.pixelWidth, interactionCamera.pixelHeight) * 0.15f);
					if (vector2.sqrMagnitude > num * num)
					{
						this._pointerIntentDistance = 0f;
						this._pointerSettlesAt = Time.unscaledTime + 0.05f;
					}
					else
					{
						this._pointerIntentDistance += vector2.magnitude;
						if (this._pointerIntentDistance >= Mathf.Max(0f, this.initialPointerWakeDistance))
						{
							this._activeHandAwaitingPointerMove = false;
							this._interactionScreenPoint = boundedInteractionScreenPoint;
						}
					}
				}
			}
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				if (runtimeHand.IsPlaced)
				{
					runtimeHand.UpdatePlacedTarget(interactionChestAnchor);
				}
				runtimeHand.ApplyFingerPose(this.proximalCurlDegrees, this.middleCurlDegrees, this.distalCurlDegrees, this.thumbCurlDegrees, this.pinchIndexProximalDegrees, this.pinchIndexMiddleDegrees, this.pinchIndexDistalDegrees, this.pinchThumbDegrees, this.pinchFoldedProximalDegrees, this.pinchFoldedMiddleDegrees, this.pinchFoldedDistalDegrees, this.vaginalFoldedProximalDegrees, this.vaginalFoldedMiddleDegrees, this.vaginalFoldedDistalDegrees, this.vaginalFoldedThumbDegrees, this.pinchTargetSpan, this.pinchConvergence);
				runtimeHand.ApplyColliderMode();
			}
			if (this._activeHand != null && this._activeHand.IsGrabLatched && !this._activeGripRequested)
			{
				this._activeHand.ReleaseGrab(this._backend);
			}
			if (this._activeHand != null && this._activeHand.IsVaginalLatched && !this._vaginalCalibrationActive && !this._activeGripRequested && this._activeHand.VaginalInsertionAmount <= 0.001f && this.HasVaginalFingerExited(this._activeHand))
			{
				this._activeHand.ReleaseVaginalInsertion();
			}
			if (this._pinchCalibrationActive && this._activeHand != null)
			{
				this.UpdatePinchCalibrationPreview(interactionCamera, interactionChestAnchor);
			}
			else if (this._activeHand != null && this._activeHand.IsVaginalLatched)
			{
				this.UpdateLatchedVaginalHand(interactionCamera);
			}
			else if (this._activeHand != null && this._activeHand.IsGrabLatched)
			{
				this._activeHand.UpdateGrabPlaneTarget(interactionCamera, this._interactionScreenPoint, this._backend);
			}
			else if (this._activeHand != null)
			{
				Vector3 zero = Vector3.zero;
				Vector3 up = Vector3.up;
				bool flag = this.TryGetChestSurface(interactionCamera, interactionChestAnchor, this._interactionScreenPoint, out zero, out up);
				if (!flag && !this._activeHand.HasSurfaceContact && !this._activeHandAwaitingPointerMove && this._backend.InteractionSurfaceReady)
				{
					if (this._activeHand.PoseMode == GropingModeController.GrabPoseMode.VaginalFingers)
					{
						flag = this.TryGetVaginalFrame(out zero, out up);
					}
					else
					{
						float num2 = (this._activeHand.IsLeft ? (-0.08f) : 0.08f);
						flag = this.TryGetDefaultBreastSurface(interactionCamera, interactionChestAnchor, num2, out zero, out up);
					}
				}
				if (flag)
				{
					if (this._activeHand.PoseMode == GropingModeController.GrabPoseMode.VaginalFingers)
					{
						this.UpdateFreeVaginalHand(interactionCamera, zero, up);
					}
					else
					{
						this.UpdateFreeHandOnSurface(interactionCamera, interactionChestAnchor, zero, up);
					}
				}
			}
			foreach (GropingModeController.RuntimeHand runtimeHand2 in this._hands)
			{
				if (runtimeHand2.IsGrabLatched)
				{
					this._backend.UpdateExternalBreastGrab(runtimeHand2.OwnerId, runtimeHand2.GrabTargetIndex, runtimeHand2.BackendGrabKind, runtimeHand2.GrabTargetPointWorld, Mathf.Clamp01(runtimeHand2.GripAmount / 0.5f));
				}
				this.SynchronizeNippleHandToCurrentAnchor(runtimeHand2);
				runtimeHand2.SyncCollisionVolumes(this.colliderSurfaceBias);
			}
			this.UpdateVaginalBlendShape();
			bool flag2 = false;
			if (!this._pinchCalibrationActive && !this._vaginalCalibrationActive && !this._activeHandAwaitingPointerMove)
			{
				foreach (GropingModeController.RuntimeHand runtimeHand3 in this._hands)
				{
					int num3;
					Vector3 vector3;
					Vector3 vector4;
					if (runtimeHand3.IsTouchingSurface && (runtimeHand3.IsGrabLatched || this._backend.TryAcquireExternalBreastGrab(runtimeHand3.SurfacePoint, BoobTouchModeController.ExternalGrabKind.Breast, out num3, out vector3, out vector4)))
					{
						flag2 = true;
						break;
					}
				}
			}
			this._backend.ReportExternalBreastTouch(flag2 ? 1f : 0f);
			this.UpdateInteractionPresentation();
		}

		// Token: 0x0600A4EE RID: 42222 RVA: 0x0044C8C4 File Offset: 0x0044AAC4
		private void UpdateInteractionPresentation()
		{
			if (this._backend == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				if (runtimeHand != null)
				{
					if (runtimeHand.IsGrabLatched && runtimeHand.BackendGrabKind == BoobTouchModeController.ExternalGrabKind.Nipple)
					{
						flag = true;
					}
					if (runtimeHand.IsVaginalLatched)
					{
						flag2 = true;
					}
				}
			}
			if (this._pinchCalibrationActive || this._vaginalCalibrationActive)
			{
				flag = false;
				flag2 = false;
			}
			bool flag3 = flag2 && this._activeHand != null && this._activeHand.IsVaginalLatched && this._activeGripRequested;
			this.SetActionVoiceTargets(flag, flag3);
			this._orgasmReactionActive = flag || flag2;
			bool flag4 = this._stimulation != null && this._stimulation.IsStimulating;
			if (this._orgasmReactionActive)
			{
				this._currentReactionFaceIndex = this.orgasmFaceIndex;
				this._backend.SetModeFaceOverride(this.orgasmFaceIndex);
				this._backend.SetModeEyesOpenOverride(false);
			}
			else
			{
				this._currentReactionFaceIndex = this.hornyFaceIndex;
				this._backend.SetModeFaceOverride(this.hornyFaceIndex);
				this._backend.SetModeEyesOpenOverride(flag4);
			}
			bool flag5 = flag2 && this._activeHand != null && this._activeHand.IsVaginalLatched && this._activeGripRequested && !this._vaginalCalibrationActive;
			if (this.faceCameraOverlay != null)
			{
				this.faceCameraOverlay.SetVisible(flag5, this._backend.InteractionCamera);
			}
		}

		// Token: 0x0600A4EF RID: 42223 RVA: 0x0044CA60 File Offset: 0x0044AC60
		private void ResetInteractionPresentation()
		{
			this._currentReactionFaceIndex = -1;
			this._orgasmReactionActive = false;
			this.SetActionVoiceTargets(false, false);
			this.CancelStimulationVoiceRequest();
			if (this.faceCameraOverlay != null)
			{
				this.faceCameraOverlay.SetVisible(false, (this._backend != null) ? this._backend.InteractionCamera : null);
			}
			if (this._backend != null)
			{
				this._backend.SetModeEyesOpenOverride(false);
				this._backend.ClearModeFaceOverride();
			}
		}

		// Token: 0x0600A4F0 RID: 42224 RVA: 0x0044CAE4 File Offset: 0x0044ACE4
		private void EnsureActionVoiceLoops()
		{
			if (this.nipplePinchVoiceLoopClip != null && this._nipplePinchVoiceLoop == null)
			{
				this._nipplePinchVoiceSourceA = this.CreateActionVoiceSource("Groping Nipple Voice A", this.nipplePinchVoiceLoopClip);
				this._nipplePinchVoiceSourceB = this.CreateActionVoiceSource("Groping Nipple Voice B", this.nipplePinchVoiceLoopClip);
				this._nipplePinchVoiceLoop = new CrossfadeLoopPlayer(this._nipplePinchVoiceSourceA, this._nipplePinchVoiceSourceB, this.nipplePinchVoiceLoopClip, this.actionVoiceLoopCrossfade, 0f, 0f);
			}
			if (this.vaginalVoiceLoopClip != null && this._vaginalVoiceLoop == null)
			{
				this._vaginalVoiceSourceA = this.CreateActionVoiceSource("Groping Vaginal Voice A", this.vaginalVoiceLoopClip);
				this._vaginalVoiceSourceB = this.CreateActionVoiceSource("Groping Vaginal Voice B", this.vaginalVoiceLoopClip);
				this._vaginalVoiceLoop = new CrossfadeLoopPlayer(this._vaginalVoiceSourceA, this._vaginalVoiceSourceB, this.vaginalVoiceLoopClip, this.actionVoiceLoopCrossfade, 0f, 0f);
			}
		}

		// Token: 0x0600A4F1 RID: 42225 RVA: 0x0044CBD4 File Offset: 0x0044ADD4
		private AudioSource CreateActionVoiceSource(string sourceName, AudioClip clip)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.clip = clip;
			audioSource.volume = 0f;
			return audioSource;
		}

		// Token: 0x0600A4F2 RID: 42226 RVA: 0x0044CC29 File Offset: 0x0044AE29
		private void SetActionVoiceTargets(bool nipplePinchActive, bool vaginalActive)
		{
			this._vaginalVoiceRequested = vaginalActive;
			this._nipplePinchVoiceRequested = nipplePinchActive && !vaginalActive;
			if (this._vaginalVoiceRequested || this._nipplePinchVoiceRequested)
			{
				this.CancelStimulationVoiceRequest();
			}
		}

		// Token: 0x0600A4F3 RID: 42227 RVA: 0x0044CC58 File Offset: 0x0044AE58
		private void TickActionVoiceLoops()
		{
			this.RecordVoiceChannelDiagnostics();
			GropingModeController.GropingVoiceChannel gropingVoiceChannel = this.ResolveRequestedVoiceChannel();
			if ((this._activeVoiceChannel == GropingModeController.GropingVoiceChannel.NipplePinch || this._activeVoiceChannel == GropingModeController.GropingVoiceChannel.Vaginal) && GropingModeController.IsCentralVoiceSourcePlaying())
			{
				if (this.FadeOutActiveVoiceChannel())
				{
					this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.None;
				}
				this.RecordVoiceChannelDiagnostics();
				return;
			}
			if (this._activeVoiceChannel != GropingModeController.GropingVoiceChannel.None && this._activeVoiceChannel != gropingVoiceChannel)
			{
				if (!this.FadeOutActiveVoiceChannel())
				{
					this.RecordVoiceChannelDiagnostics();
					return;
				}
				this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.None;
			}
			if (this._activeVoiceChannel == GropingModeController.GropingVoiceChannel.None && (gropingVoiceChannel == GropingModeController.GropingVoiceChannel.None || GropingModeController.IsCentralVoiceSourcePlaying() || !this.StartVoiceChannel(gropingVoiceChannel)))
			{
				this.RecordVoiceChannelDiagnostics();
				return;
			}
			this.TickActiveVoiceChannel();
			this.RecordVoiceChannelDiagnostics();
		}

		// Token: 0x0600A4F4 RID: 42228 RVA: 0x0044CCF6 File Offset: 0x0044AEF6
		private GropingModeController.GropingVoiceChannel ResolveRequestedVoiceChannel()
		{
			if (this._vaginalVoiceRequested)
			{
				return GropingModeController.GropingVoiceChannel.Vaginal;
			}
			if (this._nipplePinchVoiceRequested)
			{
				return GropingModeController.GropingVoiceChannel.NipplePinch;
			}
			if (this._stimulationVoiceRequested && (this._queuedStimulationVoice != null || this._activeVoiceChannel == GropingModeController.GropingVoiceChannel.TouchReaction))
			{
				return GropingModeController.GropingVoiceChannel.TouchReaction;
			}
			return GropingModeController.GropingVoiceChannel.None;
		}

		// Token: 0x0600A4F5 RID: 42229 RVA: 0x0044CD30 File Offset: 0x0044AF30
		private bool StartVoiceChannel(GropingModeController.GropingVoiceChannel channel)
		{
			switch (channel)
			{
			case GropingModeController.GropingVoiceChannel.TouchReaction:
			{
				CielVoiceController instance = CielVoiceController.Instance;
				CielVoiceSO queuedStimulationVoice = this._queuedStimulationVoice;
				if (instance == null || queuedStimulationVoice == null)
				{
					this.CancelStimulationVoiceRequest();
					return false;
				}
				bool flag = instance.Play(queuedStimulationVoice, true, this._queuedStimulationSubtitleHold);
				this._queuedStimulationVoice = null;
				if (!flag)
				{
					this._stimulationVoiceRequested = false;
					return false;
				}
				this._ownedStimulationVoice = queuedStimulationVoice;
				this._stimulationVoiceSource = instance.VoiceSource;
				this._stimulationVoiceStopRequested = false;
				if (this._stimulationVoiceSource != null)
				{
					this._stimulationVoiceTargetVolume = this._stimulationVoiceSource.volume;
					this._stimulationVoiceSource.volume = 0f;
				}
				this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.TouchReaction;
				return true;
			}
			case GropingModeController.GropingVoiceChannel.NipplePinch:
				if (this._nipplePinchVoiceLoop == null)
				{
					return false;
				}
				this._nipplePinchVoiceLoop.Resume();
				this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.NipplePinch;
				return true;
			case GropingModeController.GropingVoiceChannel.Vaginal:
				if (this._vaginalVoiceLoop == null)
				{
					return false;
				}
				this._vaginalVoiceLoop.Resume();
				this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.Vaginal;
				return true;
			default:
				return false;
			}
		}

		// Token: 0x0600A4F6 RID: 42230 RVA: 0x0044CE2C File Offset: 0x0044B02C
		private void TickActiveVoiceChannel()
		{
			switch (this._activeVoiceChannel)
			{
			case GropingModeController.GropingVoiceChannel.TouchReaction:
				this.TickTouchReactionVoice();
				return;
			case GropingModeController.GropingVoiceChannel.NipplePinch:
				this.TickActionVoiceLoop(this._nipplePinchVoiceLoop, true, this.nipplePinchVoiceLoopVolume, ref this._nipplePinchVoiceVolume);
				return;
			case GropingModeController.GropingVoiceChannel.Vaginal:
				this.TickActionVoiceLoop(this._vaginalVoiceLoop, true, this.vaginalVoiceLoopVolume, ref this._vaginalVoiceVolume);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600A4F7 RID: 42231 RVA: 0x0044CE90 File Offset: 0x0044B090
		private void TickTouchReactionVoice()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			bool flag = instance != null && instance.CurrentVoice == this._ownedStimulationVoice;
			if (this._stimulationVoiceSource == null || !this._stimulationVoiceSource.isPlaying || !flag)
			{
				this.ClearTouchReactionVoiceState();
				this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.None;
				return;
			}
			float num = Mathf.Max(0f, this.actionVoiceFadeIn);
			this._stimulationVoiceSource.volume = ((num <= 0f) ? this._stimulationVoiceTargetVolume : Mathf.MoveTowards(this._stimulationVoiceSource.volume, this._stimulationVoiceTargetVolume, Mathf.Max(0.001f, this._stimulationVoiceTargetVolume) / num * Time.unscaledDeltaTime));
		}

		// Token: 0x0600A4F8 RID: 42232 RVA: 0x0044CF48 File Offset: 0x0044B148
		private bool FadeOutActiveVoiceChannel()
		{
			switch (this._activeVoiceChannel)
			{
			case GropingModeController.GropingVoiceChannel.TouchReaction:
				if (!this._stimulationVoiceStopRequested && this._stimulationVoiceSource != null && this._stimulationVoiceSource.isPlaying)
				{
					CielVoiceController instance = CielVoiceController.Instance;
					if (instance != null)
					{
						instance.Stop();
					}
					this._stimulationVoiceStopRequested = true;
				}
				if (this._stimulationVoiceSource != null && this._stimulationVoiceSource.isPlaying)
				{
					return false;
				}
				this.ClearTouchReactionVoiceState();
				return true;
			case GropingModeController.GropingVoiceChannel.NipplePinch:
				this.TickActionVoiceLoop(this._nipplePinchVoiceLoop, false, this.nipplePinchVoiceLoopVolume, ref this._nipplePinchVoiceVolume);
				return this._nipplePinchVoiceLoop == null || (!this._nipplePinchVoiceLoop.IsPlaying && this._nipplePinchVoiceVolume <= 0.001f);
			case GropingModeController.GropingVoiceChannel.Vaginal:
				this.TickActionVoiceLoop(this._vaginalVoiceLoop, false, this.vaginalVoiceLoopVolume, ref this._vaginalVoiceVolume);
				return this._vaginalVoiceLoop == null || (!this._vaginalVoiceLoop.IsPlaying && this._vaginalVoiceVolume <= 0.001f);
			default:
				return true;
			}
		}

		// Token: 0x0600A4F9 RID: 42233 RVA: 0x0044D05B File Offset: 0x0044B25B
		private void CancelStimulationVoiceRequest()
		{
			this._queuedStimulationVoice = null;
			this._stimulationVoiceRequested = false;
		}

		// Token: 0x0600A4FA RID: 42234 RVA: 0x0044D06B File Offset: 0x0044B26B
		private void ClearTouchReactionVoiceState()
		{
			this._queuedStimulationVoice = null;
			this._ownedStimulationVoice = null;
			this._stimulationVoiceRequested = false;
			this._stimulationVoiceStopRequested = false;
			this._stimulationVoiceSource = null;
			this._stimulationVoiceTargetVolume = 0f;
		}

		// Token: 0x0600A4FB RID: 42235 RVA: 0x0044D09C File Offset: 0x0044B29C
		private static bool IsCentralVoiceSourcePlaying()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			return instance != null && instance.VoiceSource != null && instance.VoiceSource.isPlaying;
		}

		// Token: 0x0600A4FC RID: 42236 RVA: 0x0044D0D4 File Offset: 0x0044B2D4
		private int CountAudibleVoiceChannels()
		{
			int num = 0;
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance != null && instance.VoiceSource != null && instance.VoiceSource.isPlaying && instance.VoiceSource.volume > 0.001f)
			{
				num++;
			}
			if (this._nipplePinchVoiceLoop != null && this._nipplePinchVoiceLoop.IsPlaying && this._nipplePinchVoiceVolume > 0.001f)
			{
				num++;
			}
			if (this._vaginalVoiceLoop != null && this._vaginalVoiceLoop.IsPlaying && this._vaginalVoiceVolume > 0.001f)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600A4FD RID: 42237 RVA: 0x0044D170 File Offset: 0x0044B370
		private void RecordVoiceChannelDiagnostics()
		{
			this._maximumSimultaneousVoiceChannels = Mathf.Max(this._maximumSimultaneousVoiceChannels, this.CountAudibleVoiceChannels());
		}

		// Token: 0x0600A4FE RID: 42238 RVA: 0x0044D18C File Offset: 0x0044B38C
		private void TickActionVoiceLoop(CrossfadeLoopPlayer loop, bool requested, float maximumVolume, ref float currentVolume)
		{
			if (loop == null)
			{
				currentVolume = 0f;
				return;
			}
			maximumVolume = Mathf.Clamp01(maximumVolume);
			if (requested && !loop.IsPlaying)
			{
				loop.Resume();
			}
			float num = (requested ? maximumVolume : 0f);
			float num2 = ((num > currentVolume) ? this.actionVoiceFadeIn : this.actionVoiceFadeOut);
			currentVolume = ((num2 <= 0f) ? num : Mathf.MoveTowards(currentVolume, num, Mathf.Max(0.001f, maximumVolume) / num2 * Time.unscaledDeltaTime));
			loop.Tick(currentVolume);
			if (!requested && currentVolume <= 0.001f && loop.IsPlaying)
			{
				currentVolume = 0f;
				loop.Pause();
			}
		}

		// Token: 0x0600A4FF RID: 42239 RVA: 0x0044D238 File Offset: 0x0044B438
		private void StopActionVoiceLoops()
		{
			this._nipplePinchVoiceRequested = false;
			this._vaginalVoiceRequested = false;
			this.CancelStimulationVoiceRequest();
			if (this._activeVoiceChannel == GropingModeController.GropingVoiceChannel.TouchReaction && this._stimulationVoiceSource != null && this._stimulationVoiceSource.isPlaying)
			{
				CielVoiceController instance = CielVoiceController.Instance;
				if (instance != null)
				{
					instance.Stop();
				}
			}
			this._nipplePinchVoiceVolume = 0f;
			this._vaginalVoiceVolume = 0f;
			CrossfadeLoopPlayer nipplePinchVoiceLoop = this._nipplePinchVoiceLoop;
			if (nipplePinchVoiceLoop != null)
			{
				nipplePinchVoiceLoop.Stop();
			}
			CrossfadeLoopPlayer vaginalVoiceLoop = this._vaginalVoiceLoop;
			if (vaginalVoiceLoop != null)
			{
				vaginalVoiceLoop.Stop();
			}
			this.ClearTouchReactionVoiceState();
			this._activeVoiceChannel = GropingModeController.GropingVoiceChannel.None;
		}

		// Token: 0x0600A500 RID: 42240 RVA: 0x0044D2D4 File Offset: 0x0044B4D4
		private void HandleExternalBreastGrabSolved(int ownerId, int breastIndex, BoobTouchModeController.ExternalGrabKind kind, Vector3 anchorWorld, Vector3 outwardWorld)
		{
			if (!this._active || kind != BoobTouchModeController.ExternalGrabKind.Nipple)
			{
				return;
			}
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				if (runtimeHand.OwnerId == ownerId && runtimeHand.IsGrabLatched && runtimeHand.GrabTargetIndex == breastIndex)
				{
					Vector3 vector;
					Vector3 vector2;
					if (this._backend.IsFlatBreastPressMode && this._backend.TryGetExternalBreastBoneAnchor(breastIndex, out vector, out vector2))
					{
						anchorWorld = vector;
						outwardWorld = vector2;
					}
					this._lastContinuousPinchError = runtimeHand.LockPinchToSolvedNipple(anchorWorld, outwardWorld);
					this._lastContinuousPinchFrame = Time.frameCount;
					runtimeHand.SyncCollisionVolumes(this.colliderSurfaceBias);
					break;
				}
			}
		}

		// Token: 0x0600A501 RID: 42241 RVA: 0x0044D398 File Offset: 0x0044B598
		private void SynchronizeNippleHandToCurrentAnchor(GropingModeController.RuntimeHand hand)
		{
			Vector3 vector;
			Vector3 vector2;
			if (hand == null || !hand.IsGrabLatched || hand.BackendGrabKind != BoobTouchModeController.ExternalGrabKind.Nipple || !this.TryGetContinuousPinchFollowAnchor(hand.GrabTargetIndex, out vector, out vector2))
			{
				return;
			}
			this._lastContinuousPinchError = hand.LockPinchToSolvedNipple(vector, vector2);
			this._lastContinuousPinchFrame = Time.frameCount;
		}

		// Token: 0x0600A502 RID: 42242 RVA: 0x0044D3E8 File Offset: 0x0044B5E8
		private bool TryGetContinuousPinchFollowAnchor(int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			return !(this._backend == null) && ((this._backend.IsFlatBreastPressMode && this._backend.TryGetExternalBreastBoneAnchor(breastIndex, out anchorWorld, out outwardWorld)) || this._backend.TryGetExternalBreastAnchor(breastIndex, out anchorWorld, out outwardWorld));
		}

		// Token: 0x0600A503 RID: 42243 RVA: 0x0044D448 File Offset: 0x0044B648
		private void UpdateFreeHandOnSurface(Camera cam, Transform chest, Vector3 point, Vector3 normal)
		{
			bool flag = this._activeHand.PoseMode == GropingModeController.GrabPoseMode.NipplePinch;
			int num = -1;
			bool flag2 = flag && this.ResolveNippleMagnet(cam, ref point, ref normal, out num);
			if (!flag)
			{
				this._activeHand.ClearNippleMagnet();
			}
			Vector3 vector = Vector3.ProjectOnPlane(chest.up, normal);
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(cam.transform.up, normal);
			}
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.up;
			}
			Vector3 normalized = vector.normalized;
			Vector3 vector2 = Vector3.ProjectOnPlane(chest.right, normal);
			int num2;
			Vector3 vector3;
			Vector3 vector4;
			if (this._backend.TryGetNearestExternalBreastAnchor(point, out num2, out vector3, out vector4))
			{
				Vector3 vector5 = Vector3.ProjectOnPlane(point - vector3, normal);
				if (vector5.sqrMagnitude > 0.0001f && vector2.sqrMagnitude > 0.0001f)
				{
					vector = Quaternion.AngleAxis(Vector3.Dot(vector5.normalized, vector2.normalized) * 90f * Mathf.Clamp(this.breastRadialOrientation, 0f, 0.75f), normal) * normalized;
				}
			}
			float num3 = (this._activeHand.IsLeft ? (-this.handRollDegrees) : this.handRollDegrees);
			Quaternion quaternion = (flag2 ? this.ResolvePinchCalibrationRotation(cam, chest, normal) : this._activeHand.ResolveSurfaceRotation(normal, vector, num3, this.surfaceFrameSharpness, Time.unscaledDeltaTime));
			float num4 = (flag2 ? this.AdaptiveNipplePinchClearance : Mathf.Max(0.002f, this.AdaptivePalmClearance - this.gripPressDepth * this._activeHand.GripAmount));
			Vector3 vector6 = point + normal * num4;
			if (flag2)
			{
				this.ApplyPinchCalibration(num, quaternion, ref vector6, ref quaternion);
			}
			this._activeHand.SetSurfaceTarget(vector6, quaternion, point, normal, flag2);
			if (flag2)
			{
				this._activeHand.SnapToTargetPose();
			}
			float num5 = (flag ? this.nipplePinchLatchThreshold : this.grabLatchThreshold);
			if (!this._activeGripRequested || this._activeHand.GripAmount < num5)
			{
				return;
			}
			int num6;
			BoobTouchModeController.ExternalGrabKind externalGrabKind;
			Vector3 vector7;
			if (flag)
			{
				if (!flag2 || num < 0)
				{
					return;
				}
				num6 = num;
				externalGrabKind = BoobTouchModeController.ExternalGrabKind.Nipple;
			}
			else
			{
				externalGrabKind = BoobTouchModeController.ExternalGrabKind.Breast;
				if (!this._backend.TryAcquireExternalBreastGrab(point, externalGrabKind, out num6, out vector4, out vector7))
				{
					return;
				}
			}
			Vector3 vector8 = point;
			Vector3 vector9;
			if (externalGrabKind == BoobTouchModeController.ExternalGrabKind.Nipple && this.TryGetContinuousPinchFollowAnchor(num6, out vector9, out vector7))
			{
				vector8 = vector9;
			}
			this._activeHand.BeginGrab(num6, externalGrabKind, cam, this._interactionScreenPoint, this.GetGrabMovementLimit(this._activeHand.PoseMode), vector8);
		}

		// Token: 0x0600A504 RID: 42244 RVA: 0x0044D6BC File Offset: 0x0044B8BC
		private void UpdateFreeVaginalHand(Camera cam, Vector3 point, Vector3 normal)
		{
			if (this._activeHand == null)
			{
				return;
			}
			Transform transform = ((this._vaginalHips != null) ? this._vaginalHips : this._backend.InteractionChestAnchor);
			Vector3 vector = ((transform != null) ? Vector3.ProjectOnPlane(transform.up, normal) : Vector3.ProjectOnPlane(cam.transform.up, normal));
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(cam.transform.up, normal);
			}
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.up;
			}
			Quaternion quaternion = this._activeHand.ResolveSurfaceRotation(normal, vector, 0f, this.surfaceFrameSharpness, Time.unscaledDeltaTime);
			Vector3 vector2 = point + normal * Mathf.Max(0.002f, this.AdaptivePalmClearance);
			this._activeHand.SetSurfaceTarget(vector2, quaternion, point, normal, false);
			Vector3 vector3;
			Vector3 vector4;
			if (!this._activeGripRequested || !this.TryGetVaginalFrame(out vector3, out vector4) || Vector3.Distance(point, vector3) > Mathf.Max(0.01f, this.vaginalAcquireRadius))
			{
				return;
			}
			this._activeHand.BeginVaginalInsertion();
			this.UpdateLatchedVaginalHand(cam, vector3, vector4);
		}

		// Token: 0x0600A505 RID: 42245 RVA: 0x0044D7E4 File Offset: 0x0044B9E4
		private void UpdateLatchedVaginalHand(Camera cam)
		{
			Vector3 vector;
			Vector3 vector2;
			if (this.TryGetVaginalFrame(out vector, out vector2))
			{
				this.UpdateLatchedVaginalHand(cam, vector, vector2);
				return;
			}
			GropingModeController.RuntimeHand activeHand = this._activeHand;
			if (activeHand == null)
			{
				return;
			}
			activeHand.ReleaseVaginalInsertion();
		}

		// Token: 0x0600A506 RID: 42246 RVA: 0x0044D818 File Offset: 0x0044BA18
		private void UpdateLatchedVaginalHand(Camera cam, Vector3 vaginalAnchor, Vector3 vaginalOutward)
		{
			if (this._activeHand == null || !this._activeHand.IsVaginalLatched)
			{
				return;
			}
			Vector3 vector = ((this._vaginalHips != null) ? Vector3.ProjectOnPlane(this._vaginalHips.up, vaginalOutward) : Vector3.ProjectOnPlane(cam.transform.up, vaginalOutward));
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.ProjectOnPlane(cam.transform.up, vaginalOutward);
			}
			if (vector.sqrMagnitude < 0.0001f)
			{
				vector = Vector3.up;
			}
			Quaternion quaternion = Quaternion.LookRotation(-vaginalOutward, vector.normalized);
			float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this._activeHand.VaginalInsertionAmount));
			Vector3 vector2 = GropingModeController.LerpEulerAngles(this._runtimeVaginalEndpointEulers[0], this._runtimeVaginalEndpointEulers[1], num);
			Quaternion quaternion2 = quaternion * Quaternion.Euler(this._runtimeVaginalHandEuler + vector2);
			quaternion2 = this._activeHand.EnsureVaginalFingersPointUp(quaternion2, (this._vaginalHips != null) ? this._vaginalHips.up : vector);
			Vector3 vector3 = ((this._vaginalHips != null) ? this._vaginalHips.up.normalized : vector.normalized);
			float num2 = Mathf.Lerp(-Mathf.Max(0.005f, this.vaginalEntryBelowOffset), Mathf.Max(0.005f, this.vaginalInsertionDepth), num);
			float num3 = Mathf.Max(0f, this.vaginalFingerClearance) - Mathf.Max(0f, this.vaginalInsertionInwardDepth) * num;
			Vector3 vector4 = vaginalAnchor + vector3 * num2 + vaginalOutward * num3;
			Vector3 vector5 = Vector3.Lerp(this._runtimeVaginalEndpointPositions[0], this._runtimeVaginalEndpointPositions[1], num);
			vector4 += ((this._vaginalHips != null) ? this._vaginalHips.TransformDirection(vector5) : vector5);
			this._activeHand.SetVaginalTarget(vector4, quaternion2, vaginalAnchor, vaginalOutward);
		}

		// Token: 0x0600A507 RID: 42247 RVA: 0x002CB942 File Offset: 0x002C9B42
		private static Vector3 LerpEulerAngles(Vector3 from, Vector3 to, float amount)
		{
			return new Vector3(Mathf.LerpAngle(from.x, to.x, amount), Mathf.LerpAngle(from.y, to.y, amount), Mathf.LerpAngle(from.z, to.z, amount));
		}

		// Token: 0x0600A508 RID: 42248 RVA: 0x0044DA24 File Offset: 0x0044BC24
		private void ReadModeCommands()
		{
			if (!this._pinchCalibrationActive && !this._vaginalCalibrationActive)
			{
				if (this.fullGrabKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.DpadLeft) : GameplayInput.GetKeyDown(this.fullGrabKey)))
				{
					this.SelectGrabMode(GropingModeController.GrabPoseMode.FullGrab);
				}
				else if (this.nipplePinchKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.DpadUp) : GameplayInput.GetKeyDown(this.nipplePinchKey)))
				{
					this.SelectGrabMode(GropingModeController.GrabPoseMode.NipplePinch);
				}
				else if (this.vaginalFingersKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.DpadRight) : GameplayInput.GetKeyDown(this.vaginalFingersKey)))
				{
					this.SelectGrabMode(GropingModeController.GrabPoseMode.VaginalFingers);
				}
				if (this.placeHandKey != KeyCode.None && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.South) : GameplayInput.GetKeyDown(this.placeHandKey)))
				{
					this.HandlePlaceHandPressed();
				}
			}
		}

		// Token: 0x0600A509 RID: 42249 RVA: 0x0044DAF9 File Offset: 0x0044BCF9
		private bool ExitPressed()
		{
			if (this.exitKey == KeyCode.None)
			{
				return false;
			}
			if (!ControllerInput.IsActive)
			{
				return EscapeInputCoordinator.TryConsume(this.exitKey, this);
			}
			if (!ControllerWorldPointer.Down(ControllerButton.East))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x0600A50A RID: 42250 RVA: 0x0044DB2C File Offset: 0x0044BD2C
		private float ReadInteractionDelta(float deltaTime)
		{
			if (!this._activeGripRequested)
			{
				return 0f;
			}
			if (!ControllerInput.IsActive)
			{
				return GameplayInput.GetAxisRaw("Mouse Y");
			}
			if (!ControllerInput.Modifier && ControllerWorldPointer.Held(ControllerButton.RightTrigger))
			{
				return ControllerInput.Look.y * deltaTime * 110f;
			}
			return 0f;
		}

		// Token: 0x0600A50B RID: 42251 RVA: 0x0044DB80 File Offset: 0x0044BD80
		private void UpdateVaginalInsertionInput()
		{
			if (this._activeHand == null || this._activeHand.PoseMode != GropingModeController.GrabPoseMode.VaginalFingers || !this._activeHand.IsVaginalLatched)
			{
				this._vaginalPushGestureActive = false;
				this._vaginalPushSfxPending = false;
				return;
			}
			float num = this.ReadInteractionDelta(Time.unscaledDeltaTime);
			num = Mathf.Clamp(num, -Mathf.Max(0.1f, this.vaginalMouseDeltaClamp), Mathf.Max(0.1f, this.vaginalMouseDeltaClamp));
			this._activeHand.TickVaginalInsertion(num, this._activeGripRequested, this.vaginalMouseInsertionSensitivity, this.vaginalMouseDeadZone, this.vaginalInsertionSharpness, Time.unscaledDeltaTime);
			float num2 = Mathf.Max(0f, this.vaginalMouseDeadZone);
			if (!this._activeGripRequested)
			{
				this._vaginalPushGestureActive = false;
				this._vaginalPushSfxPending = false;
				return;
			}
			if (num > num2)
			{
				if (!this._vaginalPushGestureActive)
				{
					this._vaginalPushGestureActive = true;
					if (!this._vaginalPushSfxPending)
					{
						this._vaginalPushSfxPending = true;
						this._vaginalPushGestureStartPassage = this._lastActiveVaginalPassage;
					}
				}
				this._vaginalPushGestureLastInputTime = Time.unscaledTime;
				return;
			}
			if (num < -num2)
			{
				this._vaginalPushGestureActive = false;
				this._vaginalPushSfxPending = false;
				return;
			}
			if (Time.unscaledTime - this._vaginalPushGestureLastInputTime > 0.1f)
			{
				this._vaginalPushGestureActive = false;
			}
		}

		// Token: 0x0600A50C RID: 42252 RVA: 0x0044DCAC File Offset: 0x0044BEAC
		private bool TryGetVaginalFrame(out Vector3 anchorWorld, out Vector3 outwardWorld)
		{
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			if (this._vaginalHips == null)
			{
				return false;
			}
			anchorWorld = this._vaginalHips.TransformPoint(this._runtimeVaginalAnchorLocalOffset);
			outwardWorld = Vector3.ProjectOnPlane(anchorWorld - this._vaginalHips.position, this._vaginalHips.up);
			if (outwardWorld.sqrMagnitude < 1E-06f)
			{
				outwardWorld = this._vaginalHips.forward;
			}
			outwardWorld.Normalize();
			return true;
		}

		// Token: 0x0600A50D RID: 42253 RVA: 0x0044DD48 File Offset: 0x0044BF48
		private bool HasVaginalFingerExited(GropingModeController.RuntimeHand hand)
		{
			Vector3 vector;
			Vector3 vector2;
			if (hand == null || !this.TryGetVaginalFrame(out vector, out vector2))
			{
				return true;
			}
			float num = Vector3.Dot(hand.VaginalFingerMidpoint - vector, vector2);
			Vector3 vector3 = ((this._vaginalHips != null) ? this._vaginalHips.up.normalized : Vector3.up);
			return Vector3.Dot(hand.VaginalFingerMidpoint - vector, vector3) <= -Mathf.Max(0.005f, this.vaginalEntryBelowOffset) * 0.75f && num >= -Mathf.Max(0.002f, this.vaginalInsertionInwardDepth);
		}

		// Token: 0x0600A50E RID: 42254 RVA: 0x0044DDE8 File Offset: 0x0044BFE8
		private void ResolveVaginalInteraction()
		{
			this._vaginalHips = null;
			this._vaginalBodyRenderer = null;
			this._vaginalBlendShapeIndex = -1;
			this._blendShapeManager = BlendShapeManager.ResolveForRig(this._backend);
			this._vaginalNextSfxClip = 0;
			this._vaginalSfxPlayCount = 0;
			this._vaginalLastSfxTime = -999f;
			this._lastActiveVaginalPassage = 0f;
			this._vaginalPushGestureActive = false;
			this._vaginalPushSfxPending = false;
			if (this.vaginalInsertionClip1 != null || this.vaginalInsertionClip2 != null)
			{
				GameObject gameObject = new GameObject("GropingVaginalAudio");
				gameObject.transform.SetParent(base.transform, false);
				this._vaginalAudioSource = gameObject.AddComponent<AudioSource>();
				this._vaginalAudioSource.playOnAwake = false;
				this._vaginalAudioSource.loop = false;
				this._vaginalAudioSource.spatialBlend = 0f;
			}
			Animator animator = null;
			if (this._backend != null)
			{
				animator = this._backend.GetComponentInParent<Animator>(true);
				if (animator == null)
				{
					animator = this._backend.GetComponentInChildren<Animator>(true);
				}
			}
			if (animator != null && animator.isHuman)
			{
				this._vaginalHips = animator.GetBoneTransform(HumanBodyBones.Hips);
			}
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			if (this._backend != null && !string.IsNullOrEmpty(this.vaginalBlendShapeName))
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in ((animator != null) ? animator.transform : this._backend.transform.root).GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (!(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.sharedMesh == null))
					{
						int blendShapeIndex = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(this.vaginalBlendShapeName);
						if (blendShapeIndex >= 0)
						{
							if (skinnedMeshRenderer2.name == "Body_sotai_NSFW")
							{
								this._vaginalBodyRenderer = skinnedMeshRenderer2;
								this._vaginalBlendShapeIndex = blendShapeIndex;
								break;
							}
							if (skinnedMeshRenderer == null)
							{
								skinnedMeshRenderer = skinnedMeshRenderer2;
							}
						}
					}
				}
			}
			if (this._vaginalBodyRenderer == null && skinnedMeshRenderer != null)
			{
				this._vaginalBodyRenderer = skinnedMeshRenderer;
				this._vaginalBlendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.vaginalBlendShapeName);
			}
			if (this._vaginalBodyRenderer != null && this._vaginalBlendShapeIndex >= 0)
			{
				this._vaginalBlendShapeBaseline = this._vaginalBodyRenderer.GetBlendShapeWeight(this._vaginalBlendShapeIndex);
				this._vaginalBlendShapeWeight = this._vaginalBlendShapeBaseline;
			}
			else
			{
				Debug.LogWarning("[GropingModeController] Blendshape '" + this.vaginalBlendShapeName + "' non trovato sul renderer del corpo.", this);
			}
			if (this._vaginalHips == null)
			{
				Debug.LogWarning("[GropingModeController] Bone Hips non disponibile: interazione preset 3 disabilitata.", this);
			}
		}

		// Token: 0x0600A50F RID: 42255 RVA: 0x0044E078 File Offset: 0x0044C278
		private void UpdateVaginalBlendShape()
		{
			Vector3 vector;
			Vector3 vector2;
			if (!this.TryGetVaginalFrame(out vector, out vector2))
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			Vector3 vector3 = ((this._vaginalHips != null) ? this._vaginalHips.up.normalized : Vector3.up);
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				if (runtimeHand != null && runtimeHand.IsVaginalLatched)
				{
					float num3 = this.CalculateVaginalPassage(runtimeHand, vector, vector2, vector3);
					num = Mathf.Max(num, num3);
					if (runtimeHand == this._activeHand)
					{
						num2 = num3;
					}
				}
			}
			this.UpdateVaginalInsertionSfx(num2);
			if (this._vaginalBodyRenderer == null || this._vaginalBlendShapeIndex < 0)
			{
				return;
			}
			float num4 = Mathf.Lerp(this._vaginalBlendShapeBaseline, Mathf.Max(this._vaginalBlendShapeBaseline, this.vaginalBlendShapeMaxWeight), num);
			float num5 = ((num4 > this._vaginalBlendShapeWeight) ? this.vaginalOpenSharpness : this.vaginalCloseSharpness);
			this._vaginalBlendShapeWeight = Mathf.Lerp(this._vaginalBlendShapeWeight, num4, GropingModeController.ExpBlendValue(num5, Time.unscaledDeltaTime));
			if (this._blendShapeManager != null)
			{
				this._blendShapeManager.SetOverride(this.vaginalBlendShapeName, this._vaginalBlendShapeWeight, "GropingModeController.VaginalFingers", BlendShapePriority.Pose, false);
			}
			this._vaginalBodyRenderer.SetBlendShapeWeight(this._vaginalBlendShapeIndex, this._vaginalBlendShapeWeight);
		}

		// Token: 0x0600A510 RID: 42256 RVA: 0x0044E1F8 File Offset: 0x0044C3F8
		private float CalculateVaginalPassage(GropingModeController.RuntimeHand hand, Vector3 anchorWorld, Vector3 outwardWorld, Vector3 anatomicalUp)
		{
			if (hand == null || !hand.IsVaginalLatched)
			{
				return 0f;
			}
			Vector3 vector = hand.VaginalFingerMidpoint - anchorWorld;
			if (anatomicalUp.sqrMagnitude < 1E-06f)
			{
				anatomicalUp = Vector3.up;
			}
			anatomicalUp.Normalize();
			float num = Vector3.Dot(vector, anatomicalUp);
			Vector3 vector2 = vector - anatomicalUp * num;
			float num2 = Mathf.Max(0.03f, this.vaginalAcquireRadius * 1.25f);
			if (vector2.magnitude > num2)
			{
				return 0f;
			}
			return Mathf.Clamp01((num + Mathf.Max(0.005f, this.vaginalEntryBelowOffset)) / Mathf.Max(0.005f, this.vaginalEntryBelowOffset + this.vaginalInsertionDepth));
		}

		// Token: 0x0600A511 RID: 42257 RVA: 0x0044E2AB File Offset: 0x0044C4AB
		private void UpdateVaginalInsertionSfx(float activePassage)
		{
			if (this._vaginalPushSfxPending && activePassage > this._vaginalPushGestureStartPassage + Mathf.Max(0.005f, this.vaginalInsertionSfxMotionThreshold))
			{
				this.PlayVaginalInsertionSfx();
				this._vaginalPushSfxPending = false;
			}
			this._lastActiveVaginalPassage = activePassage;
		}

		// Token: 0x0600A512 RID: 42258 RVA: 0x0044E2E4 File Offset: 0x0044C4E4
		private void PlayVaginalInsertionSfx()
		{
			if (this._vaginalAudioSource == null || Time.unscaledTime - this._vaginalLastSfxTime < Mathf.Max(0f, this.vaginalInsertionSfxMinInterval))
			{
				return;
			}
			AudioClip audioClip = (((this._vaginalNextSfxClip & 1) == 0) ? this.vaginalInsertionClip1 : this.vaginalInsertionClip2);
			if (audioClip == null)
			{
				audioClip = ((this.vaginalInsertionClip1 != null) ? this.vaginalInsertionClip1 : this.vaginalInsertionClip2);
			}
			if (audioClip == null)
			{
				return;
			}
			this._vaginalNextSfxClip++;
			this._vaginalLastSfxTime = Time.unscaledTime;
			float num = Mathf.Min(this.vaginalInsertionSfxPitchRange.x, this.vaginalInsertionSfxPitchRange.y);
			float num2 = Mathf.Max(this.vaginalInsertionSfxPitchRange.x, this.vaginalInsertionSfxPitchRange.y);
			this._vaginalAudioSource.pitch = Random.Range(num, num2);
			float num3 = Mathf.Clamp01(this.vaginalInsertionSfxVolume - Random.Range(0f, this.vaginalInsertionSfxVolumeJitter));
			this._vaginalAudioSource.PlayOneShot(audioClip, num3);
			this._vaginalSfxPlayCount++;
		}

		// Token: 0x0600A513 RID: 42259 RVA: 0x0044E404 File Offset: 0x0044C604
		private void ResetVaginalBlendShape()
		{
			if (this._blendShapeManager != null)
			{
				this._blendShapeManager.RemoveOverride(this.vaginalBlendShapeName, "GropingModeController.VaginalFingers", false, false, 0f);
			}
			if (this._vaginalBodyRenderer != null && this._vaginalBlendShapeIndex >= 0)
			{
				this._vaginalBodyRenderer.SetBlendShapeWeight(this._vaginalBlendShapeIndex, this._vaginalBlendShapeBaseline);
			}
			this._vaginalBlendShapeWeight = this._vaginalBlendShapeBaseline;
			this._vaginalBlendShapeIndex = -1;
			this._vaginalBodyRenderer = null;
			this._vaginalHips = null;
			this._blendShapeManager = null;
			if (this._vaginalAudioSource != null)
			{
				Object.Destroy(this._vaginalAudioSource.gameObject);
			}
			this._vaginalAudioSource = null;
			this._vaginalPushGestureActive = false;
			this._vaginalPushSfxPending = false;
			this._lastActiveVaginalPassage = 0f;
		}

		// Token: 0x0600A514 RID: 42260 RVA: 0x00442A83 File Offset: 0x00440C83
		private static float ExpBlendValue(float sharpness, float dt)
		{
			if (sharpness > 0f)
			{
				return 1f - Mathf.Exp(-sharpness * Mathf.Max(0f, dt));
			}
			return 1f;
		}

		// Token: 0x0600A515 RID: 42261 RVA: 0x0044E4D0 File Offset: 0x0044C6D0
		private void RefreshActiveBreastSizeIfChanged()
		{
			if (this._backend == null)
			{
				return;
			}
			int currentBoobsTypeValue = this._backend.CurrentBoobsTypeValue;
			if (currentBoobsTypeValue == this._activeBoobsType)
			{
				return;
			}
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				if (runtimeHand != null)
				{
					runtimeHand.ReleaseGrab(this._backend);
				}
			}
			this._backend.ReleaseAllExternalBreastGrabs();
			this._activeBoobsType = currentBoobsTypeValue;
			this.LoadPinchCalibration();
			this.SetLoadedSizeCalibrationStatus(this._backend.CurrentBreastSizeLabel);
		}

		// Token: 0x0600A516 RID: 42262 RVA: 0x0044E57C File Offset: 0x0044C77C
		private float GetGrabMovementLimit(GropingModeController.GrabPoseMode mode)
		{
			float num = 1f;
			if (this._backend != null)
			{
				num = ((mode == GropingModeController.GrabPoseMode.NipplePinch) ? this._backend.CurrentBreastNippleMotionScale : this._backend.CurrentBreastFullGrabMotionScale);
			}
			return Mathf.Max(0.01f, ((mode == GropingModeController.GrabPoseMode.NipplePinch) ? this.nipplePinchMovementLimit : this.fullGrabMovementLimit) * num);
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600A517 RID: 42263 RVA: 0x0044E5D8 File Offset: 0x0044C7D8
		private float AdaptivePalmClearance
		{
			get
			{
				return this.palmClearance * ((this._backend != null) ? this._backend.CurrentBreastClearanceScale : 1f);
			}
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600A518 RID: 42264 RVA: 0x0044E601 File Offset: 0x0044C801
		private float AdaptiveNipplePinchClearance
		{
			get
			{
				return this.nipplePinchClearance * ((this._backend != null) ? this._backend.CurrentBreastClearanceScale : 1f);
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x0600A519 RID: 42265 RVA: 0x0044E62A File Offset: 0x0044C82A
		private float AdaptiveNippleMagnetRadius
		{
			get
			{
				return this.nippleMagnetRadius * ((this._backend != null) ? this._backend.CurrentBreastMagnetScale : 1f);
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x0600A51A RID: 42266 RVA: 0x0044E653 File Offset: 0x0044C853
		private float AdaptiveNippleMagnetReleaseRadius
		{
			get
			{
				return this.nippleMagnetReleaseRadius * ((this._backend != null) ? this._backend.CurrentBreastMagnetScale : 1f);
			}
		}

		// Token: 0x0600A51B RID: 42267 RVA: 0x0044E67C File Offset: 0x0044C87C
		private bool ResolveNippleMagnet(Camera cam, ref Vector3 point, ref Vector3 normal, out int resolvedBreastIndex)
		{
			resolvedBreastIndex = -1;
			if (!this._activeGripRequested || this._activeHand == null || this._activeHand.PoseMode != GropingModeController.GrabPoseMode.NipplePinch || this._backend == null || !this._backend.IsExternalInteractionReady)
			{
				GropingModeController.RuntimeHand activeHand = this._activeHand;
				if (activeHand != null)
				{
					activeHand.ClearNippleMagnet();
				}
				return false;
			}
			int nippleMagnetTargetIndex = this._activeHand.NippleMagnetTargetIndex;
			Vector3 zero = Vector3.zero;
			Vector3 vector = Vector3.forward;
			Vector3 vector2 = Vector3.zero;
			bool flag = nippleMagnetTargetIndex >= 0 && this._backend.TryGetExternalBreastAnchor(nippleMagnetTargetIndex, out zero, out vector);
			if (flag)
			{
				vector2 = this.GetNippleMagnetSelectionPoint(cam, nippleMagnetTargetIndex, zero);
			}
			else if (!this.TryGetNearestNippleMagnetAnchor(cam, point, out nippleMagnetTargetIndex, out zero, out vector, out vector2))
			{
				this._activeHand.ClearNippleMagnet();
				return false;
			}
			float num = (flag ? Mathf.Max(this.AdaptiveNippleMagnetRadius, this.AdaptiveNippleMagnetReleaseRadius) : this.AdaptiveNippleMagnetRadius);
			if (Vector3.Dot(vector, cam.transform.position - zero) < 0f)
			{
				vector = -vector;
			}
			if (Vector3.Distance(point, vector2) > Mathf.Max(0.01f, num))
			{
				this._activeHand.ClearNippleMagnet();
				return false;
			}
			this._activeHand.SetNippleMagnet(nippleMagnetTargetIndex);
			resolvedBreastIndex = nippleMagnetTargetIndex;
			point = zero;
			normal = vector.normalized;
			return true;
		}

		// Token: 0x0600A51C RID: 42268 RVA: 0x0044E7D4 File Offset: 0x0044C9D4
		private bool TryGetNearestNippleMagnetAnchor(Camera cam, Vector3 queryPoint, out int breastIndex, out Vector3 anchorWorld, out Vector3 outwardWorld, out Vector3 acquisitionPoint)
		{
			breastIndex = -1;
			anchorWorld = Vector3.zero;
			outwardWorld = Vector3.forward;
			acquisitionPoint = Vector3.zero;
			if (this._backend == null)
			{
				return false;
			}
			if (!this._backend.IsFlatBreastPressMode)
			{
				if (!this._backend.TryGetNearestExternalBreastAnchor(queryPoint, out breastIndex, out anchorWorld, out outwardWorld))
				{
					return false;
				}
				acquisitionPoint = anchorWorld;
				return true;
			}
			else
			{
				float num = float.PositiveInfinity;
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this._backend.ExternalBreastAnchorCount; i++)
				{
					Vector3 vector2;
					Vector3 vector3;
					Vector3 vector4;
					if (this._backend.TryGetExternalBreastBoneAnchor(i, out vector2, out vector3) && this._backend.TryGetExternalBreastAnchor(i, out vector4, out vector3))
					{
						Vector3 nippleMagnetSelectionPoint = this.GetNippleMagnetSelectionPoint(cam, i, vector4);
						vector3 = queryPoint - nippleMagnetSelectionPoint;
						float sqrMagnitude = vector3.sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							breastIndex = i;
							vector = nippleMagnetSelectionPoint;
						}
					}
				}
				if (breastIndex < 0 || !this._backend.TryGetExternalBreastAnchor(breastIndex, out anchorWorld, out outwardWorld))
				{
					return false;
				}
				acquisitionPoint = vector;
				return true;
			}
		}

		// Token: 0x0600A51D RID: 42269 RVA: 0x0044E8E0 File Offset: 0x0044CAE0
		private Vector3 GetNippleMagnetSelectionPoint(Camera cam, int breastIndex, Vector3 fallbackAnchorWorld)
		{
			Vector3 vector;
			Vector3 vector2;
			if (this._backend != null && this._backend.IsFlatBreastPressMode && this._backend.TryGetExternalBreastBoneAnchor(breastIndex, out vector, out vector2))
			{
				Vector3 vector3 = ((cam != null) ? cam.WorldToScreenPoint(vector) : Vector3.back);
				Vector3 vector4;
				if (cam != null && vector3.z > 0f && this._backend.InteractionChestAnchor != null && this.TryGetChestSurface(cam, this._backend.InteractionChestAnchor, new Vector2(vector3.x, vector3.y), out vector4, out vector2))
				{
					return vector4;
				}
			}
			return fallbackAnchorWorld;
		}

		// Token: 0x0600A51E RID: 42270 RVA: 0x0044E98A File Offset: 0x0044CB8A
		private static int CalibrationProfileIndex(int breastIndex)
		{
			if (breastIndex > 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600A51F RID: 42271 RVA: 0x0044E993 File Offset: 0x0044CB93
		private Vector3 GetRuntimePinchCalibrationPosition(int breastIndex)
		{
			return this._runtimePinchCalibrationPositions[GropingModeController.CalibrationProfileIndex(breastIndex)];
		}

		// Token: 0x0600A520 RID: 42272 RVA: 0x0044E9A6 File Offset: 0x0044CBA6
		private Vector3 GetRuntimePinchCalibrationEuler(int breastIndex)
		{
			return this._runtimePinchCalibrationEulers[GropingModeController.CalibrationProfileIndex(breastIndex)];
		}

		// Token: 0x0600A521 RID: 42273 RVA: 0x0044E9B9 File Offset: 0x0044CBB9
		private void ApplyPinchCalibration(int breastIndex, Quaternion baseRotation, ref Vector3 contactWorld, ref Quaternion handRotation)
		{
			contactWorld += baseRotation * this.GetRuntimePinchCalibrationPosition(breastIndex);
			handRotation = baseRotation * Quaternion.Euler(this.GetRuntimePinchCalibrationEuler(breastIndex));
		}

		// Token: 0x0600A522 RID: 42274 RVA: 0x0044E9F4 File Offset: 0x0044CBF4
		private Quaternion ResolvePinchCalibrationRotation(Camera cam, Transform chest, Vector3 outwardWorld)
		{
			outwardWorld = ((outwardWorld.sqrMagnitude > 1E-06f) ? outwardWorld.normalized : Vector3.forward);
			Vector3 vector = ((chest != null) ? Vector3.ProjectOnPlane(chest.up, outwardWorld) : Vector3.zero);
			if (vector.sqrMagnitude < 1E-06f && cam != null)
			{
				vector = Vector3.ProjectOnPlane(cam.transform.up, outwardWorld);
			}
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = Vector3.ProjectOnPlane(Vector3.up, outwardWorld);
			}
			if (vector.sqrMagnitude < 1E-06f)
			{
				vector = Vector3.right;
			}
			vector.Normalize();
			float num = ((this._activeHand != null && this._activeHand.IsLeft) ? (-this.handRollDegrees) : this.handRollDegrees);
			return Quaternion.LookRotation(-outwardWorld, vector) * Quaternion.Euler(0f, 0f, num);
		}

		// Token: 0x0600A523 RID: 42275 RVA: 0x0044EAE0 File Offset: 0x0044CCE0
		private void SetPinchCalibrationActive(bool active)
		{
			if (this._pinchCalibrationActive == active)
			{
				return;
			}
			if (active && (this._activeHand == null || this._backend == null))
			{
				return;
			}
			if (active && this._vaginalCalibrationActive)
			{
				this.SetVaginalCalibrationActive(false);
			}
			this._pinchCalibrationActive = active;
			if (active)
			{
				this._activeHand.ReleaseGrab(this._backend);
				this._selectedGrabMode = GropingModeController.GrabPoseMode.NipplePinch;
				this._activeHand.SetPoseMode(GropingModeController.GrabPoseMode.NipplePinch);
				this._activeHand.SetCalibrationPreview(true);
				this._activeHand.ClearNippleMagnet();
				this._pinchCalibrationTargetIndex = this.FindNearestCalibrationTarget();
				this.SetCalibrationStatus("PREVIEW ACTIVE - colliders disabled", null);
				return;
			}
			if (this._activeHand != null)
			{
				this._activeHand.SetCalibrationPreview(false);
				this._activeHand.ClearNippleMagnet();
			}
			this.SetCalibrationStatus("preview closed", null);
		}

		// Token: 0x0600A524 RID: 42276 RVA: 0x0044EBAC File Offset: 0x0044CDAC
		private int FindNearestCalibrationTarget()
		{
			if (this._backend == null)
			{
				return -1;
			}
			Vector3 vector = ((this._activeHand != null) ? this._activeHand.SurfacePoint : Vector3.zero);
			int num;
			Vector3 vector2;
			Vector3 vector3;
			if (this._backend.TryGetNearestExternalBreastAnchor(vector, out num, out vector2, out vector3))
			{
				return num;
			}
			if (this._backend.ExternalBreastAnchorCount <= 0)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600A525 RID: 42277 RVA: 0x0044EC0C File Offset: 0x0044CE0C
		private void CyclePinchCalibrationTarget()
		{
			int num = ((this._backend != null) ? this._backend.ExternalBreastAnchorCount : 0);
			if (num <= 0)
			{
				this._pinchCalibrationTargetIndex = -1;
				return;
			}
			this._pinchCalibrationTargetIndex = (this._pinchCalibrationTargetIndex + 1 + num) % num;
			this.SetTargetCalibrationStatus(this._pinchCalibrationTargetIndex + 1, num);
		}

		// Token: 0x0600A526 RID: 42278 RVA: 0x0044EC64 File Offset: 0x0044CE64
		private void UpdatePinchCalibrationInput()
		{
			int num = GropingModeController.CalibrationProfileIndex(this._pinchCalibrationTargetIndex);
			float num2 = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
			float num3 = ((GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift)) ? Mathf.Clamp(this.pinchCalibrationFineMultiplier, 0.05f, 1f) : 1f);
			float num4 = Mathf.Max(0.001f, this.pinchCalibrationPositionSpeed) * num3 * num2;
			float num5 = Mathf.Max(1f, this.pinchCalibrationRotationSpeed) * num3 * num2;
			Vector3 zero = Vector3.zero;
			if (GameplayInput.GetKey(KeyCode.LeftArrow))
			{
				zero.x -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.RightArrow))
			{
				zero.x += num4;
			}
			if (GameplayInput.GetKey(KeyCode.DownArrow))
			{
				zero.y -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.UpArrow))
			{
				zero.y += num4;
			}
			if (GameplayInput.GetKey(KeyCode.PageDown))
			{
				zero.z -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.PageUp))
			{
				zero.z += num4;
			}
			Vector3 vector = this._runtimePinchCalibrationPositions[num] + zero;
			this._runtimePinchCalibrationPositions[num] = new Vector3(Mathf.Clamp(vector.x, -0.12f, 0.12f), Mathf.Clamp(vector.y, -0.12f, 0.12f), Mathf.Clamp(vector.z, -0.12f, 0.12f));
			Vector3 zero2 = Vector3.zero;
			if (GameplayInput.GetKey(KeyCode.S))
			{
				zero2.x -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.W))
			{
				zero2.x += num5;
			}
			if (GameplayInput.GetKey(KeyCode.A))
			{
				zero2.y -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.D))
			{
				zero2.y += num5;
			}
			if (GameplayInput.GetKey(KeyCode.Q))
			{
				zero2.z -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.E))
			{
				zero2.z += num5;
			}
			Vector3 vector2 = this._runtimePinchCalibrationEulers[num] + zero2;
			this._runtimePinchCalibrationEulers[num] = new Vector3(Mathf.Clamp(vector2.x, -180f, 180f), Mathf.Clamp(vector2.y, -180f, 180f), Mathf.Clamp(vector2.z, -180f, 180f));
			if (GameplayInput.GetKeyDown(KeyCode.Tab))
			{
				this.CyclePinchCalibrationTarget();
			}
			if (GameplayInput.GetKeyDown(KeyCode.R))
			{
				this._runtimePinchCalibrationPositions[num] = Vector3.zero;
				this._runtimePinchCalibrationEulers[num] = Vector3.zero;
				this.SetResetCalibrationStatus(num + 1);
			}
			if (GameplayInput.GetKeyDown(KeyCode.Backspace))
			{
				this.LoadPinchCalibration();
				this.SetCalibrationStatus("saved values reloaded", null);
			}
			if (GameplayInput.GetKeyDown(KeyCode.Return) || GameplayInput.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.SavePinchCalibration();
			}
		}

		// Token: 0x0600A527 RID: 42279 RVA: 0x0044EF5C File Offset: 0x0044D15C
		private bool UpdatePinchCalibrationPreview(Camera cam, Transform chest)
		{
			if (this._activeHand == null || this._backend == null || cam == null || chest == null)
			{
				return false;
			}
			Vector3 vector;
			Vector3 vector2;
			if (!this._backend.TryGetExternalBreastAnchor(this._pinchCalibrationTargetIndex, out vector, out vector2))
			{
				this._pinchCalibrationTargetIndex = this.FindNearestCalibrationTarget();
				if (!this._backend.TryGetExternalBreastAnchor(this._pinchCalibrationTargetIndex, out vector, out vector2))
				{
					return false;
				}
			}
			if (Vector3.Dot(vector2, cam.transform.position - vector) < 0f)
			{
				vector2 = -vector2;
			}
			vector2.Normalize();
			Quaternion quaternion = this.ResolvePinchCalibrationRotation(cam, chest, vector2);
			Vector3 vector3 = vector + vector2 * this.AdaptiveNipplePinchClearance;
			this.ApplyPinchCalibration(this._pinchCalibrationTargetIndex, quaternion, ref vector3, ref quaternion);
			this._activeHand.SetSurfaceTarget(vector3, quaternion, vector, vector2, true);
			return true;
		}

		// Token: 0x0600A528 RID: 42280 RVA: 0x0044F03C File Offset: 0x0044D23C
		private void LoadPinchCalibration()
		{
			Vector3 vector = new Vector3(PlayerPrefs.GetFloat("Groping.PinchCalibration.Position.X", this.nipplePinchCalibrationPosition.x), PlayerPrefs.GetFloat("Groping.PinchCalibration.Position.Y", this.nipplePinchCalibrationPosition.y), PlayerPrefs.GetFloat("Groping.PinchCalibration.Position.Z", this.nipplePinchCalibrationPosition.z));
			Vector3 vector2 = new Vector3(PlayerPrefs.GetFloat("Groping.PinchCalibration.Euler.X", this.nipplePinchCalibrationEuler.x), PlayerPrefs.GetFloat("Groping.PinchCalibration.Euler.Y", this.nipplePinchCalibrationEuler.y), PlayerPrefs.GetFloat("Groping.PinchCalibration.Euler.Z", this.nipplePinchCalibrationEuler.z));
			for (int i = 0; i < 2; i++)
			{
				Vector3 vector3 = (this.hasPerNipplePinchCalibration ? ((i == 0) ? this.nipplePinchCalibrationTarget0Position : this.nipplePinchCalibrationTarget1Position) : vector);
				Vector3 vector4 = (this.hasPerNipplePinchCalibration ? ((i == 0) ? this.nipplePinchCalibrationTarget0Euler : this.nipplePinchCalibrationTarget1Euler) : vector2);
				this.GetSerializedPinchSizeCalibration(this._activeBoobsType, i, ref vector3, ref vector4);
				bool flag = this.nipplePinchCalibrationsBySize != null && this._activeBoobsType >= 0 && this._activeBoobsType < this.nipplePinchCalibrationsBySize.Length && this.nipplePinchCalibrationsBySize[this._activeBoobsType].hasValues;
				Vector3 vector5 = ((!flag && this._activeBoobsType == 3) ? GropingModeController.ReadLegacyPerNippleCalibrationVector(i, true, vector3) : vector3);
				Vector3 vector6 = ((!flag && this._activeBoobsType == 3) ? GropingModeController.ReadLegacyPerNippleCalibrationVector(i, false, vector4) : vector4);
				this._runtimePinchCalibrationPositions[i] = (flag ? vector5 : GropingModeController.ReadSavedCalibrationVector(this._activeBoobsType, i, true, vector5));
				this._runtimePinchCalibrationEulers[i] = (flag ? vector6 : GropingModeController.ReadSavedCalibrationVector(this._activeBoobsType, i, false, vector6));
			}
		}

		// Token: 0x0600A529 RID: 42281 RVA: 0x0044F1EC File Offset: 0x0044D3EC
		private void GetSerializedPinchSizeCalibration(int boobsType, int targetIndex, ref Vector3 position, ref Vector3 euler)
		{
			if (this.nipplePinchCalibrationsBySize == null || boobsType < 0 || boobsType >= this.nipplePinchCalibrationsBySize.Length)
			{
				return;
			}
			GropingModeController.PinchSizeCalibrationProfile pinchSizeCalibrationProfile = this.nipplePinchCalibrationsBySize[boobsType];
			if (!pinchSizeCalibrationProfile.hasValues)
			{
				return;
			}
			if (GropingModeController.CalibrationProfileIndex(targetIndex) == 0)
			{
				position = pinchSizeCalibrationProfile.target0Position;
				euler = pinchSizeCalibrationProfile.target0Euler;
				return;
			}
			position = pinchSizeCalibrationProfile.target1Position;
			euler = pinchSizeCalibrationProfile.target1Euler;
		}

		// Token: 0x0600A52A RID: 42282 RVA: 0x0044F264 File Offset: 0x0044D464
		private void SavePinchCalibration()
		{
			if (this.nipplePinchCalibrationsBySize == null || this.nipplePinchCalibrationsBySize.Length != 6)
			{
				Array.Resize<GropingModeController.PinchSizeCalibrationProfile>(ref this.nipplePinchCalibrationsBySize, 6);
			}
			this.nipplePinchCalibrationsBySize[this._activeBoobsType] = new GropingModeController.PinchSizeCalibrationProfile
			{
				hasValues = true,
				target0Position = this._runtimePinchCalibrationPositions[0],
				target0Euler = this._runtimePinchCalibrationEulers[0],
				target1Position = this._runtimePinchCalibrationPositions[1],
				target1Euler = this._runtimePinchCalibrationEulers[1]
			};
			for (int i = 0; i < 2; i++)
			{
				GropingModeController.WriteSavedCalibrationVector(this._activeBoobsType, i, true, this._runtimePinchCalibrationPositions[i]);
				GropingModeController.WriteSavedCalibrationVector(this._activeBoobsType, i, false, this._runtimePinchCalibrationEulers[i]);
			}
			if (this._activeBoobsType == 3)
			{
				for (int j = 0; j < 2; j++)
				{
					GropingModeController.WriteLegacyPerNippleCalibrationVector(j, true, this._runtimePinchCalibrationPositions[j]);
					GropingModeController.WriteLegacyPerNippleCalibrationVector(j, false, this._runtimePinchCalibrationEulers[j]);
				}
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Position.X", this._runtimePinchCalibrationPositions[0].x);
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Position.Y", this._runtimePinchCalibrationPositions[0].y);
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Position.Z", this._runtimePinchCalibrationPositions[0].z);
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Euler.X", this._runtimePinchCalibrationEulers[0].x);
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Euler.Y", this._runtimePinchCalibrationEulers[0].y);
				PlayerPrefs.SetFloat("Groping.PinchCalibration.Euler.Z", this._runtimePinchCalibrationEulers[0].z);
			}
			PlayerPrefs.Save();
			BoobTouchModeController backend = this._backend;
			this.SetSavedCalibrationStatus(((backend != null) ? backend.CurrentBreastSizeLabel : null) ?? "size");
		}

		// Token: 0x0600A52B RID: 42283 RVA: 0x0044F444 File Offset: 0x0044D644
		private void SetVaginalCalibrationActive(bool active)
		{
			if (active && !this.IsGrabModeAvailable(GropingModeController.GrabPoseMode.VaginalFingers))
			{
				return;
			}
			if (this._vaginalCalibrationActive == active)
			{
				return;
			}
			if (active && (this._activeHand == null || this._backend == null))
			{
				return;
			}
			if (active && this._pinchCalibrationActive)
			{
				this.SetPinchCalibrationActive(false);
			}
			this._vaginalCalibrationActive = active;
			if (active)
			{
				this.LoadVaginalCalibration(true);
				this._activeHand.ReleaseGrab(this._backend);
				this._activeHand.ReleaseVaginalInsertion();
				this._selectedGrabMode = GropingModeController.GrabPoseMode.VaginalFingers;
				this._activeHand.SetPoseMode(GropingModeController.GrabPoseMode.VaginalFingers);
				this._activeHand.SetCalibrationPreview(true);
				this._activeHand.BeginVaginalInsertion();
				this._vaginalCalibrationInsertedPreview = false;
				this._activeHand.SetVaginalInsertionImmediate(0f);
				this.SetVaginalCalibrationStatus("ENTRY PREVIEW - colliders disabled", null);
				return;
			}
			if (this._activeHand != null)
			{
				this._activeHand.SetCalibrationPreview(false);
				this._activeHand.ReleaseVaginalInsertion();
			}
			this.SetVaginalCalibrationStatus("preview closed", null);
		}

		// Token: 0x0600A52C RID: 42284 RVA: 0x0044F53C File Offset: 0x0044D73C
		private void UpdateVaginalCalibrationInput()
		{
			if (this._activeHand == null)
			{
				return;
			}
			int num = (this._vaginalCalibrationInsertedPreview ? 1 : 0);
			float num2 = Mathf.Max(0.0001f, Time.unscaledDeltaTime);
			float num3 = ((GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift)) ? Mathf.Clamp(this.vaginalCalibrationFineMultiplier, 0.05f, 1f) : 1f);
			float num4 = Mathf.Max(0.001f, this.vaginalCalibrationPositionSpeed) * num3 * num2;
			float num5 = Mathf.Max(1f, this.vaginalCalibrationRotationSpeed) * num3 * num2;
			Vector3 zero = Vector3.zero;
			if (GameplayInput.GetKey(KeyCode.LeftArrow))
			{
				zero.x -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.RightArrow))
			{
				zero.x += num4;
			}
			if (GameplayInput.GetKey(KeyCode.DownArrow))
			{
				zero.y -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.UpArrow))
			{
				zero.y += num4;
			}
			if (GameplayInput.GetKey(KeyCode.PageDown))
			{
				zero.z -= num4;
			}
			if (GameplayInput.GetKey(KeyCode.PageUp))
			{
				zero.z += num4;
			}
			Vector3 vector = this._runtimeVaginalEndpointPositions[num] + zero;
			this._runtimeVaginalEndpointPositions[num] = new Vector3(Mathf.Clamp(vector.x, -0.2f, 0.2f), Mathf.Clamp(vector.y, -0.2f, 0.2f), Mathf.Clamp(vector.z, -0.2f, 0.2f));
			Vector3 zero2 = Vector3.zero;
			if (GameplayInput.GetKey(KeyCode.S))
			{
				zero2.x -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.W))
			{
				zero2.x += num5;
			}
			if (GameplayInput.GetKey(KeyCode.A))
			{
				zero2.y -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.D))
			{
				zero2.y += num5;
			}
			if (GameplayInput.GetKey(KeyCode.Q))
			{
				zero2.z -= num5;
			}
			if (GameplayInput.GetKey(KeyCode.E))
			{
				zero2.z += num5;
			}
			Vector3 vector2 = this._runtimeVaginalEndpointEulers[num] + zero2;
			this._runtimeVaginalEndpointEulers[num] = new Vector3(Mathf.Clamp(vector2.x, -180f, 180f), Mathf.Clamp(vector2.y, -180f, 180f), Mathf.Clamp(vector2.z, -180f, 180f));
			if (GameplayInput.GetKeyDown(KeyCode.Tab))
			{
				this._vaginalCalibrationInsertedPreview = !this._vaginalCalibrationInsertedPreview;
				this._activeHand.SetVaginalInsertionImmediate(this._vaginalCalibrationInsertedPreview ? 1f : 0f);
				this.SetVaginalCalibrationStatus(this._vaginalCalibrationInsertedPreview ? "FULL INSERTION PROFILE" : "FINGER START PROFILE", null);
			}
			if (GameplayInput.GetKeyDown(KeyCode.R))
			{
				this._runtimeVaginalEndpointPositions[num] = ((num == 0) ? this.vaginalStartLocalOffset : this.vaginalInsertedLocalOffset);
				this._runtimeVaginalEndpointEulers[num] = ((num == 0) ? this.vaginalStartEulerOffset : this.vaginalInsertedEulerOffset);
				this.SetVaginalCalibrationStatus("selected profile reset", null);
			}
			if (GameplayInput.GetKeyDown(KeyCode.Backspace))
			{
				this.LoadVaginalCalibration(true);
				this.SetVaginalCalibrationStatus("saved values reloaded", null);
			}
			if (GameplayInput.GetKeyDown(KeyCode.Return) || GameplayInput.GetKeyDown(KeyCode.KeypadEnter))
			{
				this.SaveVaginalCalibration();
			}
		}

		// Token: 0x0600A52D RID: 42285 RVA: 0x0044F89C File Offset: 0x0044DA9C
		private void LoadVaginalCalibration(bool includeLocalEditorOverrides = false)
		{
			if (Application.isEditor && includeLocalEditorOverrides)
			{
				this._runtimeVaginalAnchorLocalOffset = new Vector3(PlayerPrefs.GetFloat("Groping.VaginalCalibration.Anchor.X", this.vaginalAnchorLocalOffset.x), PlayerPrefs.GetFloat("Groping.VaginalCalibration.Anchor.Y", this.vaginalAnchorLocalOffset.y), PlayerPrefs.GetFloat("Groping.VaginalCalibration.Anchor.Z", this.vaginalAnchorLocalOffset.z));
				this._runtimeVaginalHandEuler = new Vector3(PlayerPrefs.GetFloat("Groping.VaginalCalibration.Euler.X", this.vaginalHandEulerOffset.x), PlayerPrefs.GetFloat("Groping.VaginalCalibration.Euler.Y", this.vaginalHandEulerOffset.y), PlayerPrefs.GetFloat("Groping.VaginalCalibration.Euler.Z", this.vaginalHandEulerOffset.z));
				this._runtimeVaginalEndpointPositions[0] = GropingModeController.ReadSavedVaginalEndpointVector(0, true, this.vaginalStartLocalOffset);
				this._runtimeVaginalEndpointEulers[0] = GropingModeController.ReadSavedVaginalEndpointVector(0, false, this.vaginalStartEulerOffset);
				this._runtimeVaginalEndpointPositions[1] = GropingModeController.ReadSavedVaginalEndpointVector(1, true, this.vaginalInsertedLocalOffset);
				this._runtimeVaginalEndpointEulers[1] = GropingModeController.ReadSavedVaginalEndpointVector(1, false, this.vaginalInsertedEulerOffset);
				return;
			}
			this._runtimeVaginalAnchorLocalOffset = this.vaginalAnchorLocalOffset;
			this._runtimeVaginalHandEuler = this.vaginalHandEulerOffset;
			this._runtimeVaginalEndpointPositions[0] = this.vaginalStartLocalOffset;
			this._runtimeVaginalEndpointEulers[0] = this.vaginalStartEulerOffset;
			this._runtimeVaginalEndpointPositions[1] = this.vaginalInsertedLocalOffset;
			this._runtimeVaginalEndpointEulers[1] = this.vaginalInsertedEulerOffset;
		}

		// Token: 0x0600A52E RID: 42286 RVA: 0x0044FA10 File Offset: 0x0044DC10
		private void SaveVaginalCalibration()
		{
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Anchor.X", this._runtimeVaginalAnchorLocalOffset.x);
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Anchor.Y", this._runtimeVaginalAnchorLocalOffset.y);
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Anchor.Z", this._runtimeVaginalAnchorLocalOffset.z);
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Euler.X", this._runtimeVaginalHandEuler.x);
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Euler.Y", this._runtimeVaginalHandEuler.y);
			PlayerPrefs.SetFloat("Groping.VaginalCalibration.Euler.Z", this._runtimeVaginalHandEuler.z);
			for (int i = 0; i < 2; i++)
			{
				GropingModeController.WriteVaginalEndpointVector(i, true, this._runtimeVaginalEndpointPositions[i]);
				GropingModeController.WriteVaginalEndpointVector(i, false, this._runtimeVaginalEndpointEulers[i]);
			}
			PlayerPrefs.Save();
			bool flag = GropingModeController.SavedVaginalEndpointMatches(0, true, this._runtimeVaginalEndpointPositions[0]) && GropingModeController.SavedVaginalEndpointMatches(0, false, this._runtimeVaginalEndpointEulers[0]) && GropingModeController.SavedVaginalEndpointMatches(1, true, this._runtimeVaginalEndpointPositions[1]) && GropingModeController.SavedVaginalEndpointMatches(1, false, this._runtimeVaginalEndpointEulers[1]);
			string text = (flag ? "SAVED AND VERIFIED - start + insertion" : "ERROR: save verification failed");
			this.SetVaginalCalibrationStatus(text, null);
			if (flag)
			{
				Debug.Log("[GropingVaginalCalibration] " + text, this);
				return;
			}
			Debug.LogError("[GropingVaginalCalibration] " + text, this);
		}

		// Token: 0x0600A52F RID: 42287 RVA: 0x0044FB64 File Offset: 0x0044DD64
		private static Vector3 ReadSavedVaginalEndpointVector(int profileIndex, bool position, Vector3 fallback)
		{
			string text = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "X") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "X"));
			string text2 = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "Y") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "Y"));
			string text3 = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "Z") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "Z"));
			return new Vector3(PlayerPrefs.GetFloat(text, fallback.x), PlayerPrefs.GetFloat(text2, fallback.y), PlayerPrefs.GetFloat(text3, fallback.z));
		}

		// Token: 0x0600A530 RID: 42288 RVA: 0x0044FBEC File Offset: 0x0044DDEC
		private static void WriteVaginalEndpointVector(int profileIndex, bool position, Vector3 value)
		{
			string text = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "X") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "X"));
			string text2 = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "Y") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "Y"));
			string text3 = (position ? GropingModeController.GetVaginalEndpointPositionKey(profileIndex, "Z") : GropingModeController.GetVaginalEndpointEulerKey(profileIndex, "Z"));
			PlayerPrefs.SetFloat(text, value.x);
			PlayerPrefs.SetFloat(text2, value.y);
			PlayerPrefs.SetFloat(text3, value.z);
		}

		// Token: 0x0600A531 RID: 42289 RVA: 0x0044FC70 File Offset: 0x0044DE70
		private static bool SavedVaginalEndpointMatches(int profileIndex, bool position, Vector3 expected)
		{
			Vector3 vector = GropingModeController.ReadSavedVaginalEndpointVector(profileIndex, position, new Vector3(float.NaN, float.NaN, float.NaN));
			return Mathf.Abs(vector.x - expected.x) <= 1E-05f && Mathf.Abs(vector.y - expected.y) <= 1E-05f && Mathf.Abs(vector.z - expected.z) <= 1E-05f;
		}

		// Token: 0x0600A532 RID: 42290 RVA: 0x0044FCE9 File Offset: 0x0044DEE9
		private void SetVaginalCalibrationStatus(string status, Func<string> presentation = null)
		{
			this._vaginalCalibrationStatus = status;
			this._vaginalCalibrationStatusPresentation = presentation;
			this._vaginalCalibrationStatusUntil = Time.unscaledTime + 3f;
		}

		// Token: 0x0600A533 RID: 42291 RVA: 0x0044FD0C File Offset: 0x0044DF0C
		private static Vector3 ReadSavedCalibrationVector(int boobsType, int targetIndex, bool position, Vector3 fallback)
		{
			string text = (position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "X") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "X"));
			string text2 = (position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "Y") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "Y"));
			string text3 = (position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "Z") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "Z"));
			return new Vector3(PlayerPrefs.GetFloat(text, fallback.x), PlayerPrefs.GetFloat(text2, fallback.y), PlayerPrefs.GetFloat(text3, fallback.z));
		}

		// Token: 0x0600A534 RID: 42292 RVA: 0x0044FD9C File Offset: 0x0044DF9C
		private static void WriteSavedCalibrationVector(int boobsType, int targetIndex, bool position, Vector3 value)
		{
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "X") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "X"), value.x);
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "Y") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "Y"), value.y);
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(boobsType, targetIndex, "Z") : GropingModeController.GetPinchCalibrationEulerKey(boobsType, targetIndex, "Z"), value.z);
		}

		// Token: 0x0600A535 RID: 42293 RVA: 0x0044FE24 File Offset: 0x0044E024
		private static Vector3 ReadLegacyPerNippleCalibrationVector(int targetIndex, bool position, Vector3 fallback)
		{
			string text = (position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "X") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "X"));
			string text2 = (position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "Y") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "Y"));
			string text3 = (position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "Z") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "Z"));
			return new Vector3(PlayerPrefs.GetFloat(text, fallback.x), PlayerPrefs.GetFloat(text2, fallback.y), PlayerPrefs.GetFloat(text3, fallback.z));
		}

		// Token: 0x0600A536 RID: 42294 RVA: 0x0044FEAC File Offset: 0x0044E0AC
		private static void WriteLegacyPerNippleCalibrationVector(int targetIndex, bool position, Vector3 value)
		{
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "X") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "X"), value.x);
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "Y") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "Y"), value.y);
			PlayerPrefs.SetFloat(position ? GropingModeController.GetPinchCalibrationPositionKey(targetIndex, "Z") : GropingModeController.GetPinchCalibrationEulerKey(targetIndex, "Z"), value.z);
		}

		// Token: 0x0600A537 RID: 42295 RVA: 0x0044FF2B File Offset: 0x0044E12B
		private void SetCalibrationStatus(string status, Func<string> presentation = null)
		{
			this._pinchCalibrationStatus = status;
			this._pinchCalibrationStatusPresentation = presentation;
			this._pinchCalibrationStatusUntil = Time.unscaledTime + 3f;
		}

		// Token: 0x0600A538 RID: 42296 RVA: 0x0044FF4C File Offset: 0x0044E14C
		private void FixedUpdate()
		{
			if (!this._active)
			{
				return;
			}
			float fixedUnscaledDeltaTime = Time.fixedUnscaledDeltaTime;
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				runtimeHand.MovePhysics(this.positionSharpness, this.rotationSharpness, fixedUnscaledDeltaTime);
			}
		}

		// Token: 0x0600A539 RID: 42297 RVA: 0x0044FFB8 File Offset: 0x0044E1B8
		private static string CalibrationText(string source)
		{
			return GameLocalization.GetSource("groping_ui", source);
		}

		// Token: 0x0600A53A RID: 42298 RVA: 0x0044FFC5 File Offset: 0x0044E1C5
		private static string CalibrationFormat(string source, params object[] arguments)
		{
			return GameLocalization.FormatSource("groping_ui", source, arguments);
		}

		// Token: 0x0600A53B RID: 42299 RVA: 0x0044FFD4 File Offset: 0x0044E1D4
		private void SetLoadedSizeCalibrationStatus(string size)
		{
			this.SetCalibrationStatus("profile " + size + " loaded", () => GropingModeController.CalibrationFormat("profile {0} loaded", new object[] { GropingModeController.CalibrationText(size) }));
		}

		// Token: 0x0600A53C RID: 42300 RVA: 0x00450018 File Offset: 0x0044E218
		private void SetTargetCalibrationStatus(int target, int count)
		{
			this.SetCalibrationStatus(string.Format("nipple {0}/{1}", target, count), () => GropingModeController.CalibrationFormat("nipple {0}/{1}", new object[] { target, count }));
		}

		// Token: 0x0600A53D RID: 42301 RVA: 0x0045006C File Offset: 0x0044E26C
		private void SetResetCalibrationStatus(int target)
		{
			this.SetCalibrationStatus(string.Format("nipple {0} offsets reset", target), () => GropingModeController.CalibrationFormat("nipple {0} offsets reset", new object[] { target }));
		}

		// Token: 0x0600A53E RID: 42302 RVA: 0x004500B0 File Offset: 0x0044E2B0
		private void SetSavedCalibrationStatus(string size)
		{
			this.SetCalibrationStatus("SAVED " + size + ": both nipples", () => GropingModeController.CalibrationFormat("SAVED {0}: both nipples", new object[] { GropingModeController.CalibrationText(size) }));
		}

		// Token: 0x0600A53F RID: 42303 RVA: 0x004500F4 File Offset: 0x0044E2F4
		private string CalibrationStatusPresentation(bool vaginal)
		{
			string text = (vaginal ? this._vaginalCalibrationStatus : this._pinchCalibrationStatus);
			float num = (vaginal ? this._vaginalCalibrationStatusUntil : this._pinchCalibrationStatusUntil);
			if (Time.unscaledTime > num)
			{
				return GropingModeController.CalibrationText(vaginal ? "nonphysical preview: hand colliders disabled" : "nonphysical preview: hand colliders are disabled");
			}
			if (GameLocalization.CurrentCode == "en")
			{
				return text;
			}
			Func<string> func = (vaginal ? this._vaginalCalibrationStatusPresentation : this._pinchCalibrationStatusPresentation);
			if (func == null)
			{
				return GropingModeController.CalibrationText(text);
			}
			return func();
		}

		// Token: 0x0600A540 RID: 42304 RVA: 0x00450178 File Offset: 0x0044E378
		private string[] PinchCalibrationLines()
		{
			return new string[]
			{
				GropingModeController.CalibrationText("LIVE NIPPLE PINCH CALIBRATION"),
				GropingModeController.CalibrationFormat("Size: {0}   |   Nipple: {1}/{2}   Position offset (cm): {3:F2}", new object[]
				{
					(this._backend != null) ? GropingModeController.CalibrationText(this._backend.CurrentBreastSizeLabel) : this._activeBoobsType.ToString(),
					this._pinchCalibrationTargetIndex + 1,
					Mathf.Max(1, (this._backend != null) ? this._backend.ExternalBreastAnchorCount : 0),
					string.Format("{0:F2}", this.RuntimePinchCalibrationPosition * 100f)
				}),
				GropingModeController.CalibrationFormat("Local rotation (degrees): {0:F1}", new object[] { string.Format("{0:F1}", this.RuntimePinchCalibrationEuler) }),
				GropingModeController.CalibrationText("Arrows: left/right + up/down   |   PageUp/PageDown: depth"),
				GropingModeController.CalibrationText("W/S: pitch   A/D: yaw   Q/E: roll   |   SHIFT: fine adjustment"),
				GropingModeController.CalibrationText("TAB: switch nipple   R: reset   BACKSPACE: reload saved"),
				GropingModeController.CalibrationFormat("ENTER: save   {0}: close preview   ESC: close preview", new object[] { this.pinchCalibrationKey }),
				this.CalibrationStatusPresentation(false)
			};
		}

		// Token: 0x0600A541 RID: 42305 RVA: 0x004502B4 File Offset: 0x0044E4B4
		private string[] VaginalCalibrationLines()
		{
			return new string[]
			{
				GropingModeController.CalibrationText("LIVE HAND CALIBRATION - PRESET 3"),
				GropingModeController.CalibrationFormat("SELECTED PROFILE: {0}", new object[] { GropingModeController.CalibrationText(this._vaginalCalibrationInsertedPreview ? "FULL INSERTION" : "FINGER START") }),
				GropingModeController.CalibrationFormat("Profile offset in local hip space (cm): {0:F2}", new object[] { string.Format("{0:F2}", this.RuntimeVaginalEndpointPosition * 100f) }),
				GropingModeController.CalibrationFormat("Profile rotation (degrees): {0:F1}", new object[] { string.Format("{0:F1}", this.RuntimeVaginalEndpointEuler) }),
				GropingModeController.CalibrationText("Arrows: X/Y   PageUp/PageDown: Z depth"),
				GropingModeController.CalibrationText("W/S: pitch   A/D: yaw   Q/E: roll   SHIFT: fine adjustment"),
				GropingModeController.CalibrationText("TAB: start/inserted   R: reset profile   BACKSPACE: reload saved"),
				GropingModeController.CalibrationFormat("ENTER: save   {0}: close preview   ESC: close preview", new object[] { this.vaginalCalibrationKey }),
				GropingModeController.CalibrationFormat("Base path: below {0:F1} cm, above {1:F1} cm, inward {2:F1} cm", new object[]
				{
					this.vaginalEntryBelowOffset * 100f,
					this.vaginalInsertionDepth * 100f,
					this.vaginalInsertionInwardDepth * 100f
				}),
				this.CalibrationStatusPresentation(true)
			};
		}

		// Token: 0x0600A542 RID: 42306 RVA: 0x00450408 File Offset: 0x0044E608
		private void CreateCalibrationGuiStyles(GUISkin skin)
		{
			GUIStyle guistyle = new GUIStyle(skin.label)
			{
				fontSize = 19,
				fontStyle = FontStyle.Bold
			};
			guistyle.normal.textColor = Color.white;
			GUIStyle guistyle2 = new GUIStyle(skin.label)
			{
				fontSize = 14
			};
			guistyle2.normal.textColor = new Color(0.92f, 0.92f, 0.92f);
			GUIStyle guistyle3 = new GUIStyle(guistyle2)
			{
				fontStyle = FontStyle.Bold
			};
			guistyle3.normal.textColor = new Color(0.45f, 0.9f, 1f);
			this._calibrationGuiStyles = new GUIStyle[] { guistyle, guistyle2, guistyle3 };
			this._calibrationOriginalFonts = new Font[] { guistyle.font, guistyle2.font, guistyle3.font };
			this._calibrationOriginalWrap = new bool[] { guistyle.wordWrap, guistyle2.wordWrap, guistyle3.wordWrap };
		}

		// Token: 0x0600A543 RID: 42307 RVA: 0x00450508 File Offset: 0x0044E708
		private void RefreshCalibrationGuiStyles()
		{
			bool flag = GameLocalization.CurrentCode != "en";
			TMP_FontAsset font = GameLocalization.GetFont(null);
			Font font2 = ((font != null) ? font.sourceFontFile : null);
			for (int i = 0; i < this._calibrationGuiStyles.Length; i++)
			{
				this._calibrationGuiStyles[i].font = ((font2 != null) ? font2 : this._calibrationOriginalFonts[i]);
				this._calibrationGuiStyles[i].wordWrap = flag || this._calibrationOriginalWrap[i];
			}
		}

		// Token: 0x0600A544 RID: 42308 RVA: 0x00450587 File Offset: 0x0044E787
		private GUIStyle CalibrationRowStyle(bool vaginal, int index, int count)
		{
			return this._calibrationGuiStyles[(index == 0) ? 0 : ((index == count - 1 || index == 1 || index == 2 || (vaginal && index == 3)) ? 2 : 1)];
		}

		// Token: 0x0600A545 RID: 42309 RVA: 0x004505B0 File Offset: 0x0044E7B0
		private Rect[] CalibrationRowRects(string[] lines, bool vaginal, float width)
		{
			Rect[] array = new Rect[lines.Length];
			if (GameLocalization.CurrentCode == "en")
			{
				float[] array2;
				if (!vaginal)
				{
					RuntimeHelpers.InitializeArray(array2 = new float[8], fieldof(<PrivateImplementationDetails>.90DE549B555D983741A4C7C56A1268B669E864BDB7CB9F952A9A0B7CD5056BC6).FieldHandle);
				}
				else
				{
					RuntimeHelpers.InitializeArray(array2 = new float[10], fieldof(<PrivateImplementationDetails>.186A0BAADF9D9F3E04F94387B08DECCEADF3264A14CDE5DEA9AC27EE40C90BEE).FieldHandle);
				}
				float[] array3 = array2;
				for (int i = 0; i < lines.Length; i++)
				{
					array[i] = new Rect(16f, array3[i], width - 32f, (i == 0) ? 28f : 24f);
				}
				return array;
			}
			float num = 10f;
			for (int j = 0; j < lines.Length; j++)
			{
				float num2 = Mathf.Max((j == 0) ? 28f : 24f, this.CalibrationRowStyle(vaginal, j, lines.Length).CalcHeight(new GUIContent(lines[j]), width - 32f));
				array[j] = new Rect(16f, num, width - 32f, num2);
				num += num2 + ((j == 0) ? 4f : 2f);
			}
			return array;
		}

		// Token: 0x0600A546 RID: 42310 RVA: 0x004506C0 File Offset: 0x0044E8C0
		private void OnGUI()
		{
			if (!this._active)
			{
				return;
			}
			if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
			{
				return;
			}
			if (this._vaginalCalibrationActive)
			{
				this.DrawVaginalCalibrationGui();
				return;
			}
			if (this._pinchCalibrationActive)
			{
				this.DrawCalibrationPanel(false);
			}
		}

		// Token: 0x0600A547 RID: 42311 RVA: 0x00450715 File Offset: 0x0044E915
		private void DrawVaginalCalibrationGui()
		{
			this.DrawCalibrationPanel(true);
		}

		// Token: 0x0600A548 RID: 42312 RVA: 0x00450720 File Offset: 0x0044E920
		private void DrawCalibrationPanel(bool vaginal)
		{
			if (this._calibrationGuiStyles == null)
			{
				this.CreateCalibrationGuiStyles(GUI.skin);
			}
			this.RefreshCalibrationGuiStyles();
			bool flag = GameLocalization.CurrentCode != "en";
			float num = (vaginal ? 660f : 610f);
			if (flag)
			{
				num = Mathf.Min(num, Mathf.Max(120f, (float)Screen.width - 36f));
			}
			string[] array = (vaginal ? this.VaginalCalibrationLines() : this.PinchCalibrationLines());
			Rect[] array2 = this.CalibrationRowRects(array, vaginal, num - (flag ? 16f : 0f));
			float num2 = (flag ? (array2[array2.Length - 1].yMax + 8f) : (vaginal ? 285f : 245f));
			float num3 = (flag ? Mathf.Min(num2, Mathf.Max(80f, (float)Screen.height - 36f)) : num2);
			Rect rect = new Rect(18f, 18f, num, num3);
			Color color = GUI.color;
			GUI.color = new Color(0.04f, 0.04f, 0.04f, 0.92f);
			GUI.Box(rect, GUIContent.none);
			GUI.color = color;
			if (flag)
			{
				this._calibrationGuiScroll = GUI.BeginScrollView(rect, this._calibrationGuiScroll, new Rect(0f, 0f, num - 16f, num2));
				for (int i = 0; i < array.Length; i++)
				{
					GUI.Label(array2[i], array[i], this.CalibrationRowStyle(vaginal, i, array.Length));
				}
				GUI.EndScrollView();
				return;
			}
			for (int j = 0; j < array.Length; j++)
			{
				Rect rect2 = array2[j];
				rect2.position += rect.position;
				GUI.Label(rect2, array[j], this.CalibrationRowStyle(vaginal, j, array.Length));
			}
		}

		// Token: 0x0600A549 RID: 42313 RVA: 0x004508FC File Offset: 0x0044EAFC
		public bool EnterMode()
		{
			if (this._active || this._backend == null)
			{
				return false;
			}
			if (this.leftHandPrefab == null || this.rightHandPrefab == null)
			{
				Debug.LogWarning("[GropingModeController] Prefab mani non assegnati: esegui Tools > Groping > Setup Player Prefab.", this);
				return false;
			}
			if (!this._backend.TryEnterGropingMode())
			{
				return false;
			}
			this._active = true;
			ControllerWorldPointer.Position(this);
			this._maximumSimultaneousVoiceChannels = 0;
			this._activeGripRequested = false;
			this._initialGrabInputArmed = !(ControllerInput.IsActive ? ControllerWorldPointer.Held(ControllerButton.RightTrigger) : GameplayInput.GetMouseButton(0));
			this._selectedGrabMode = GropingModeController.GrabPoseMode.FullGrab;
			this._pinchCalibrationActive = false;
			this._pinchCalibrationTargetIndex = -1;
			this._vaginalCalibrationActive = false;
			this._vaginalCalibrationInsertedPreview = false;
			this._lastContinuousPinchError = -1f;
			this._lastContinuousPinchFrame = -10;
			this._activeBoobsType = this._backend.CurrentBoobsTypeValue;
			this.LoadPinchCalibration();
			this.LoadVaginalCalibration(false);
			this.ResolveVaginalInteraction();
			if (this.faceCameraOverlay != null)
			{
				this.faceCameraOverlay.BeginSession(this._backend.InteractionCamera, this._backend.InteractionAnimator);
			}
			this._backend.ReportExternalBreastTouch(0f);
			this.SpawnHand(false);
			if (this._activeHand == null)
			{
				this.ExitMode();
				return false;
			}
			this._currentReactionFaceIndex = this.hornyFaceIndex;
			this._orgasmReactionActive = false;
			this._backend.SetModeFaceOverride(this.hornyFaceIndex);
			this._backend.SetModeEyesOpenOverride(false);
			if (this.debugLogs)
			{
				Debug.Log("[GropingModeController] Modalita' attiva.", this);
			}
			return true;
		}

		// Token: 0x0600A54A RID: 42314 RVA: 0x00450A87 File Offset: 0x0044EC87
		public void RequestEnterMode(float retrySeconds = 6f)
		{
			if (this._active)
			{
				return;
			}
			this._manualEntryRequestUntil = Mathf.Max(this._manualEntryRequestUntil, Time.unscaledTime + Mathf.Max(0.25f, retrySeconds));
		}

		// Token: 0x0600A54B RID: 42315 RVA: 0x00450AB4 File Offset: 0x0044ECB4
		public void ExitMode()
		{
			if (!this._active)
			{
				return;
			}
			this.FinishExit(true);
		}

		// Token: 0x0600A54C RID: 42316 RVA: 0x00450AC8 File Offset: 0x0044ECC8
		private void FinishExit(bool exitBackend)
		{
			ControllerWorldPointer.Release(this);
			this.ResetInteractionPresentation();
			if (this._backend != null)
			{
				this._backend.ReportExternalBreastTouch(0f);
			}
			this._active = false;
			this._pinchCalibrationActive = false;
			this._pinchCalibrationTargetIndex = -1;
			this._vaginalCalibrationActive = false;
			this._vaginalCalibrationInsertedPreview = false;
			this._activeHand = null;
			this._activeHandAwaitingPointerMove = false;
			this._activeGripRequested = false;
			this._initialGrabInputArmed = false;
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				runtimeHand.Dispose(this._backend);
			}
			this._hands.Clear();
			this.ResetVaginalBlendShape();
			if (exitBackend && this._backend != null)
			{
				this._backend.ExitGropingMode();
			}
			if (this.debugLogs)
			{
				Debug.Log("[GropingModeController] Modalita' chiusa.", this);
			}
		}

		// Token: 0x0600A54D RID: 42317 RVA: 0x00450BC8 File Offset: 0x0044EDC8
		private void OnDisable()
		{
			if (this._active)
			{
				this.FinishExit(true);
			}
			this.StopActionVoiceLoops();
		}

		// Token: 0x0600A54E RID: 42318 RVA: 0x00450BDF File Offset: 0x0044EDDF
		private bool IsGrabModeAvailable(GropingModeController.GrabPoseMode mode)
		{
			return mode != GropingModeController.GrabPoseMode.VaginalFingers || (this._backend != null && this._backend.IsThirdHandPoseAvailable);
		}

		// Token: 0x0600A54F RID: 42319 RVA: 0x00450C04 File Offset: 0x0044EE04
		private void EnforceGrabModeAvailability()
		{
			if (this.IsGrabModeAvailable(GropingModeController.GrabPoseMode.VaginalFingers))
			{
				return;
			}
			if (this._vaginalCalibrationActive)
			{
				this.SetVaginalCalibrationActive(false);
			}
			if (!this.IsGrabModeAvailable(this._selectedGrabMode))
			{
				this.SelectGrabMode(GropingModeController.GrabPoseMode.FullGrab);
			}
			bool flag = false;
			for (int i = this._hands.Count - 1; i >= 0; i--)
			{
				GropingModeController.RuntimeHand runtimeHand = this._hands[i];
				if (!this.IsGrabModeAvailable(runtimeHand.PoseMode))
				{
					if (runtimeHand == this._activeHand)
					{
						this._activeHand = null;
					}
					runtimeHand.Dispose(this._backend);
					this._hands.RemoveAt(i);
					flag = true;
				}
			}
			if (flag && this._activeHand == null)
			{
				this.SpawnHand(false);
			}
		}

		// Token: 0x0600A550 RID: 42320 RVA: 0x00450CB0 File Offset: 0x0044EEB0
		private void SelectGrabMode(GropingModeController.GrabPoseMode mode)
		{
			if (!this.IsGrabModeAvailable(mode))
			{
				return;
			}
			if (this._pinchCalibrationActive)
			{
				this.SetPinchCalibrationActive(false);
			}
			if (this._vaginalCalibrationActive)
			{
				this.SetVaginalCalibrationActive(false);
			}
			this._selectedGrabMode = mode;
			if (this._activeHand == null)
			{
				return;
			}
			this._activeHand.ReleaseGrab(this._backend);
			this._activeHand.SetPoseMode(mode);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GropingModeController] Presa selezionata: {0}.", mode), this);
			}
		}

		// Token: 0x0600A551 RID: 42321 RVA: 0x00450D30 File Offset: 0x0044EF30
		private void PlaceActiveHand()
		{
			if (this._activeHand == null || !this._activeHand.HasSurfaceContact)
			{
				return;
			}
			Transform interactionChestAnchor = this._backend.InteractionChestAnchor;
			if (interactionChestAnchor == null)
			{
				return;
			}
			this._activeHand.Place(interactionChestAnchor);
			this._activeHand = null;
			if (this._hands.Count < 2)
			{
				this.SpawnHand(true);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GropingModeController] Mano piazzata; mani={0}.", this._hands.Count), this);
			}
		}

		// Token: 0x0600A552 RID: 42322 RVA: 0x00450DBC File Offset: 0x0044EFBC
		private void HandlePlaceHandPressed()
		{
			if (this._activeHand != null)
			{
				this.PlaceActiveHand();
				return;
			}
			if (this._hands.Count < 2)
			{
				return;
			}
			foreach (GropingModeController.RuntimeHand runtimeHand in this._hands)
			{
				runtimeHand.Dispose(this._backend);
			}
			this._hands.Clear();
			this._activeGripRequested = false;
			if (this._backend != null)
			{
				this._backend.ReportExternalBreastTouch(0f);
			}
			this._activeHandAwaitingPointerMove = false;
			this.SpawnHand(false);
			if (this.debugLogs)
			{
				Debug.Log("[GropingModeController] Terzo SPACE: entrambe le mani sbloccate.", this);
			}
		}

		// Token: 0x0600A553 RID: 42323 RVA: 0x00450E84 File Offset: 0x0044F084
		private void SpawnHand(bool isLeft)
		{
			if (this._hands.Count >= 2)
			{
				return;
			}
			Transform interactionChestAnchor = this._backend.InteractionChestAnchor;
			Camera interactionCamera = this._backend.InteractionCamera;
			if (interactionChestAnchor == null || interactionCamera == null)
			{
				return;
			}
			GameObject gameObject = (isLeft ? this.leftHandPrefab : this.rightHandPrefab);
			Vector3 vector = (interactionCamera.transform.position - interactionChestAnchor.position).normalized;
			if (vector.sqrMagnitude < 0.5f)
			{
				vector = -interactionChestAnchor.forward;
			}
			float num = (isLeft ? (-0.1f) : 0.1f);
			Vector3 vector2 = new Vector3(num, this.initialHandLocalHeight, 0.12f);
			Vector3 vector3 = interactionChestAnchor.TransformPoint(vector2) + vector * 0.08f;
			Quaternion quaternion = Quaternion.LookRotation(-vector, interactionChestAnchor.up);
			GropingModeController.RuntimeHand runtimeHand = new GropingModeController.RuntimeHand(gameObject, isLeft, vector3, quaternion, this.handLength, this.palmColliderRadius, this.fingerColliderRadius, this.thumbColliderRadius, this._backend, this.ResolveCurrentSkinColor(), this.handOpacity);
			runtimeHand.SetPoseMode(this._selectedGrabMode);
			this._hands.Add(runtimeHand);
			this._activeHand = runtimeHand;
			this._pointerPositionAtHandSpawn = this.ClampInteractionScreenPoint(interactionCamera, ControllerWorldPointer.Position(this));
			this._activeHandNeutralLocalPoint = vector2;
			Vector3 vector4 = interactionCamera.WorldToScreenPoint(interactionChestAnchor.TransformPoint(vector2));
			this._neutralHandScreenPoint = this.ClampInteractionScreenPoint(interactionCamera, new Vector2(vector4.x, vector4.y));
			this._interactionScreenPoint = this._neutralHandScreenPoint;
			this._pointerIntentDistance = 0f;
			this._pointerSettlesAt = Time.unscaledTime + Mathf.Max(0f, this.initialPointerSettleSeconds);
			this._activeHandAwaitingPointerMove = true;
		}

		// Token: 0x0600A554 RID: 42324 RVA: 0x00451044 File Offset: 0x0044F244
		private Color ResolveCurrentSkinColor()
		{
			Color color = this.defaultHandSkinColor;
			SkinnedMeshRenderer skinnedMeshRenderer = this._vaginalBodyRenderer;
			if (skinnedMeshRenderer == null && this._backend != null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in this._backend.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer2 != null && skinnedMeshRenderer2.name == "Body_sotai_NSFW")
					{
						skinnedMeshRenderer = skinnedMeshRenderer2;
						break;
					}
				}
			}
			if (skinnedMeshRenderer == null)
			{
				color.a = 1f;
				return color;
			}
			foreach (Material material in skinnedMeshRenderer.sharedMaterials)
			{
				if (!(material == null))
				{
					if (material.HasProperty("_UseMain2ndTex") && material.GetFloat("_UseMain2ndTex") > 0.5f && material.HasProperty("_Color2nd"))
					{
						color = material.GetColor("_Color2nd");
						break;
					}
					if (material.HasProperty("_Color"))
					{
						Color color2 = material.GetColor("_Color");
						if (!GropingModeController.ApproximatelyWhite(color2))
						{
							color = color2;
							break;
						}
					}
				}
			}
			color.a = 1f;
			return color;
		}

		// Token: 0x0600A555 RID: 42325 RVA: 0x00451180 File Offset: 0x0044F380
		private static bool ApproximatelyWhite(Color color)
		{
			return Mathf.Abs(color.r - 1f) < 0.01f && Mathf.Abs(color.g - 1f) < 0.01f && Mathf.Abs(color.b - 1f) < 0.01f;
		}

		// Token: 0x0600A556 RID: 42326 RVA: 0x004511D7 File Offset: 0x0044F3D7
		private Vector2 GetBoundedInteractionScreenPoint(Camera cam)
		{
			return this.ClampInteractionScreenPoint(cam, ControllerWorldPointer.Position(this));
		}

		// Token: 0x0600A557 RID: 42327 RVA: 0x004511E8 File Offset: 0x0044F3E8
		private Vector2 ClampInteractionScreenPoint(Camera cam, Vector2 raw)
		{
			Rect pixelRect = cam.pixelRect;
			float num = Mathf.Max(0f, this.screenEdgePadding);
			float num2 = pixelRect.xMin + num;
			float num3 = Mathf.Max(num2, pixelRect.xMax - num);
			float num4 = pixelRect.yMin + num;
			float num5 = Mathf.Max(num4, pixelRect.yMax - num);
			return new Vector2(Mathf.Clamp(raw.x, num2, num3), Mathf.Clamp(raw.y, num4, num5));
		}

		// Token: 0x0600A558 RID: 42328 RVA: 0x00451268 File Offset: 0x0044F468
		private bool TryGetChestSurface(Camera cam, Transform chest, Vector2 screenPoint, out Vector3 point, out Vector3 normal)
		{
			Ray ray = cam.ScreenPointToRay(screenPoint);
			if (this.TryGetChestSurfaceAlongRay(cam, chest, ray, out point, out normal))
			{
				return true;
			}
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (this._activeHand != null && this._activeHand.PoseMode == GropingModeController.GrabPoseMode.VaginalFingers && this.TryGetVaginalFrame(out vector, out vector2))
			{
				vector3 = vector;
			}
			else
			{
				Vector3 vector4 = cam.WorldToScreenPoint(chest.TransformPoint(new Vector3(0f, 0.01f, 0.12f)));
				float num = ((screenPoint.x < vector4.x) ? (-0.1f) : 0.1f);
				vector3 = chest.TransformPoint(new Vector3(num, 0f, 0.14f));
				int num2;
				Vector3 vector5;
				if (this._backend.TryGetNearestExternalBreastAnchor(vector3, out num2, out vector5, out vector2))
				{
					vector3 = vector5;
				}
			}
			Vector3 vector6 = cam.WorldToScreenPoint(vector3);
			Vector2 vector7 = new Vector2(vector6.x, vector6.y);
			int num3 = Mathf.Clamp(this.surfaceEdgeSearchSteps, 4, 32);
			for (int i = 1; i <= num3; i++)
			{
				Vector2 vector8 = Vector2.Lerp(screenPoint, vector7, (float)i / (float)num3);
				if (this.TryGetChestSurfaceAlongRay(cam, chest, cam.ScreenPointToRay(vector8), out point, out normal))
				{
					return true;
				}
			}
			point = Vector3.zero;
			normal = Vector3.up;
			return false;
		}

		// Token: 0x0600A559 RID: 42329 RVA: 0x004513AC File Offset: 0x0044F5AC
		private bool TryGetChestSurfaceAlongRay(Camera cam, Transform chest, Ray screenRay, out Vector3 point, out Vector3 normal)
		{
			point = Vector3.zero;
			normal = Vector3.up;
			Ray ray = new Ray(screenRay.origin - screenRay.direction * 2f, screenRay.direction);
			return this._backend.TryRaycastInteractionSurface(ray, out point, out normal) && Vector3.Dot(point - cam.transform.position, screenRay.direction) >= 0f && this.ValidateChestSurface(cam, chest, point, ref normal);
		}

		// Token: 0x0600A55A RID: 42330 RVA: 0x00451450 File Offset: 0x0044F650
		private bool TryGetDefaultBreastSurface(Camera cam, Transform chest, float localX, out Vector3 point, out Vector3 normal)
		{
			Vector3 vector = chest.TransformPoint(new Vector3(localX, 0f, 0.6f));
			Vector3 normalized = chest.TransformDirection(Vector3.back).normalized;
			return this._backend.TryRaycastInteractionSurface(new Ray(vector, normalized), out point, out normal) && this.ValidateChestSurface(cam, chest, point, ref normal);
		}

		// Token: 0x0600A55B RID: 42331 RVA: 0x004514B4 File Offset: 0x0044F6B4
		private bool ValidateChestSurface(Camera cam, Transform chest, Vector3 point, ref Vector3 normal)
		{
			Vector3 vector = chest.InverseTransformPoint(point);
			float num = ((this._backend != null) ? this._backend.CurrentBreastChestWidthScale : 1f);
			float num2 = ((this._backend != null) ? this._backend.CurrentBreastChestDepthScale : 1f);
			float num3 = this.chestHorizontalRange.x * num;
			float num4 = this.chestHorizontalRange.y * num;
			float x = this.chestDepthRange.x;
			float num5 = this.chestDepthRange.y * num2;
			if (vector.x < num3 || vector.x > num4 || vector.y < this.chestVerticalRange.x || vector.y > this.chestVerticalRange.y || vector.z < x || vector.z > num5)
			{
				return false;
			}
			Vector3 vector2 = cam.transform.position - point;
			if (Vector3.Dot(normal, vector2) < 0f)
			{
				normal = -normal;
			}
			normal.Normalize();
			return true;
		}

		// Token: 0x0400A912 RID: 43282
		[Header("Input")]
		[SerializeField]
		private KeyCode placeHandKey = KeyCode.Space;

		// Token: 0x0400A913 RID: 43283
		[SerializeField]
		private KeyCode fullGrabKey = KeyCode.Alpha1;

		// Token: 0x0400A914 RID: 43284
		[SerializeField]
		private KeyCode nipplePinchKey = KeyCode.Alpha2;

		// Token: 0x0400A915 RID: 43285
		[SerializeField]
		private KeyCode vaginalFingersKey = KeyCode.Alpha3;

		// Token: 0x0400A916 RID: 43286
		[SerializeField]
		private KeyCode exitKey = KeyCode.X;

		// Token: 0x0400A917 RID: 43287
		[SerializeField]
		private KeyCode pinchCalibrationKey = KeyCode.F11;

		// Token: 0x0400A918 RID: 43288
		[SerializeField]
		private KeyCode vaginalCalibrationKey = KeyCode.F12;

		// Token: 0x0400A919 RID: 43289
		[Header("Prefab mani")]
		[SerializeField]
		private GameObject leftHandPrefab;

		// Token: 0x0400A91A RID: 43290
		[SerializeField]
		private GameObject rightHandPrefab;

		// Token: 0x0400A91B RID: 43291
		[Tooltip("Lunghezza massima del visual in metri, normalizzata dai bounds del prefab.")]
		[SerializeField]
		[Min(0.05f)]
		private float handLength = 0.225f;

		// Token: 0x0400A91C RID: 43292
		[Header("Aspetto mani")]
		[Tooltip("Opacita' delle sole copie runtime delle mani. I prefab e i materiali originali non vengono modificati.")]
		[SerializeField]
		[Range(0.15f, 1f)]
		private float handOpacity = 0.68f;

		// Token: 0x0400A91D RID: 43293
		[Tooltip("Colore pelle usato quando il body non espone ancora la tinta selezionata.")]
		[SerializeField]
		private Color defaultHandSkinColor = new Color(0.996f, 0.858f, 0.792f, 1f);

		// Token: 0x0400A91E RID: 43294
		[Header("Reazioni facciali")]
		[Tooltip("Faccia base mantenuta per tutta la modalita' (3 = HornySupreme).")]
		[SerializeField]
		private int hornyFaceIndex = 3;

		// Token: 0x0400A91F RID: 43295
		[Tooltip("OrgasmFace1 usata durante pinch e inserimento vaginale.")]
		[SerializeField]
		private int orgasmFaceIndex = 5;

		// Token: 0x0400A920 RID: 43296
		[SerializeField]
		private GropingFaceCameraOverlay faceCameraOverlay;

		// Token: 0x0400A921 RID: 43297
		[Header("Area petto (spazio del chest anchor)")]
		[SerializeField]
		private Vector2 chestHorizontalRange = new Vector2(-0.24f, 0.24f);

		// Token: 0x0400A922 RID: 43298
		[SerializeField]
		private Vector2 chestVerticalRange = new Vector2(-0.7f, 0.18f);

		// Token: 0x0400A923 RID: 43299
		[SerializeField]
		private Vector2 chestDepthRange = new Vector2(-0.04f, 0.34f);

		// Token: 0x0400A924 RID: 43300
		[Tooltip("Altezza locale neutra in cui nasce una nuova mano, sotto i seni.")]
		[SerializeField]
		private float initialHandLocalHeight = -0.24f;

		// Token: 0x0400A925 RID: 43301
		[Tooltip("Movimento minimo del mouse prima che una mano appena nata inizi a seguire il puntatore.")]
		[SerializeField]
		[Min(0f)]
		private float initialPointerWakeDistance = 10f;

		// Token: 0x0400A926 RID: 43302
		[Tooltip("Breve finestra che assorbe il ricentraggio del cursore proveniente dalla emote wheel.")]
		[SerializeField]
		[Min(0f)]
		private float initialPointerSettleSeconds = 0.25f;

		// Token: 0x0400A927 RID: 43303
		[Header("Contatto")]
		[Tooltip("Distanza del centro mano dalla superficie a riposo.")]
		[SerializeField]
		[Min(0f)]
		private float palmClearance = 0.026f;

		// Token: 0x0400A928 RID: 43304
		[Tooltip("Piccolo avanzamento aggiuntivo quando la presa e' chiusa.")]
		[SerializeField]
		[Min(0f)]
		private float gripPressDepth = 0.012f;

		// Token: 0x0400A929 RID: 43305
		[SerializeField]
		[Min(0.005f)]
		private float palmColliderRadius = 0.05f;

		// Token: 0x0400A92A RID: 43306
		[SerializeField]
		[Min(0.005f)]
		private float fingerColliderRadius = 0.022f;

		// Token: 0x0400A92B RID: 43307
		[SerializeField]
		[Min(0.005f)]
		private float thumbColliderRadius = 0.025f;

		// Token: 0x0400A92C RID: 43308
		[Tooltip("Affonda appena i proxy rispetto al visual per mantenere un contatto morbido.")]
		[SerializeField]
		[Min(0f)]
		private float colliderSurfaceBias = 0.006f;

		// Token: 0x0400A92D RID: 43309
		[Header("Movimento fisico")]
		[SerializeField]
		[Min(0f)]
		private float positionSharpness = 24f;

		// Token: 0x0400A92E RID: 43310
		[SerializeField]
		[Min(0f)]
		private float rotationSharpness = 20f;

		// Token: 0x0400A92F RID: 43311
		[SerializeField]
		[Min(0f)]
		private float gripInSpeed = 1.8f;

		// Token: 0x0400A930 RID: 43312
		[SerializeField]
		[Min(0f)]
		private float gripOutSpeed = 2.4f;

		// Token: 0x0400A931 RID: 43313
		[SerializeField]
		[Range(0f, 35f)]
		private float handRollDegrees = 10f;

		// Token: 0x0400A932 RID: 43314
		[SerializeField]
		[Min(0f)]
		private float surfaceFrameSharpness = 18f;

		// Token: 0x0400A933 RID: 43315
		[Tooltip("Quanto la posizione laterale attorno al seno ruota la mano sul proprio asse. Anche al massimo resta sotto i 90 gradi: il palmo non puo' ribaltarsi.")]
		[SerializeField]
		[Range(0f, 0.75f)]
		private float breastRadialOrientation = 0.42f;

		// Token: 0x0400A934 RID: 43316
		[SerializeField]
		[Range(0.05f, 0.95f)]
		private float grabLatchThreshold = 0.32f;

		// Token: 0x0400A935 RID: 43317
		[Tooltip("Escursione WORLD massima della mano dal punto in cui aggancia il seno.")]
		[SerializeField]
		[Min(0.01f)]
		private float fullGrabMovementLimit = 0.075f;

		// Token: 0x0400A936 RID: 43318
		[Tooltip("Escursione WORLD massima della mano dal punto in cui aggancia il capezzolo.")]
		[SerializeField]
		[Min(0.01f)]
		private float nipplePinchMovementLimit = 0.055f;

		// Token: 0x0400A937 RID: 43319
		[Tooltip("Il pinch aggancia solo quando indice e pollice sono gia' abbastanza chiusi.")]
		[SerializeField]
		[Range(0.05f, 0.95f)]
		private float nipplePinchLatchThreshold = 0.62f;

		// Token: 0x0400A938 RID: 43320
		[Header("Puntatore confinato")]
		[Tooltip("Margine interno della Game View usato dal raycast delle mani.")]
		[SerializeField]
		[Min(0f)]
		private float screenEdgePadding = 2f;

		// Token: 0x0400A939 RID: 43321
		[Tooltip("Campioni dal cursore verso il seno piu' vicino quando il raycast esce dalla silhouette. Il primo contatto utile diventa il bordo.")]
		[SerializeField]
		[Range(4f, 32f)]
		private int surfaceEdgeSearchSteps = 18;

		// Token: 0x0400A93A RID: 43322
		[Header("Presa dita")]
		[SerializeField]
		[Range(0f, 70f)]
		private float proximalCurlDegrees = 28f;

		// Token: 0x0400A93B RID: 43323
		[SerializeField]
		[Range(0f, 90f)]
		private float middleCurlDegrees = 38f;

		// Token: 0x0400A93C RID: 43324
		[SerializeField]
		[Range(0f, 70f)]
		private float distalCurlDegrees = 24f;

		// Token: 0x0400A93D RID: 43325
		[SerializeField]
		[Range(0f, 70f)]
		private float thumbCurlDegrees = 24f;

		// Token: 0x0400A93E RID: 43326
		[Tooltip("Nel pinch soltanto indice e pollice ricevono curl e collisione.")]
		[SerializeField]
		[Range(0f, 90f)]
		private float pinchIndexProximalDegrees = 34f;

		// Token: 0x0400A93F RID: 43327
		[SerializeField]
		[Range(0f, 110f)]
		private float pinchIndexMiddleDegrees = 54f;

		// Token: 0x0400A940 RID: 43328
		[SerializeField]
		[Range(0f, 90f)]
		private float pinchIndexDistalDegrees = 32f;

		// Token: 0x0400A941 RID: 43329
		[SerializeField]
		[Range(0f, 90f)]
		private float pinchThumbDegrees = 38f;

		// Token: 0x0400A942 RID: 43330
		[Tooltip("Chiusura naturale di medio, anulare e mignolo durante il pinch.")]
		[SerializeField]
		[Range(0f, 90f)]
		private float pinchFoldedProximalDegrees = 55f;

		// Token: 0x0400A943 RID: 43331
		[SerializeField]
		[Range(0f, 110f)]
		private float pinchFoldedMiddleDegrees = 72f;

		// Token: 0x0400A944 RID: 43332
		[SerializeField]
		[Range(0f, 90f)]
		private float pinchFoldedDistalDegrees = 44f;

		// Token: 0x0400A945 RID: 43333
		[Tooltip("Distanza desiderata fra pollice e indice a pinch chiuso.")]
		[SerializeField]
		[Min(0.01f)]
		private float pinchTargetSpan = 0.018f;

		// Token: 0x0400A946 RID: 43334
		[SerializeField]
		[Range(0f, 1f)]
		private float pinchConvergence = 0.9f;

		// Token: 0x0400A947 RID: 43335
		[SerializeField]
		[Min(0f)]
		private float nipplePinchClearance = 0.002f;

		// Token: 0x0400A948 RID: 43336
		[Tooltip("Distanza dalla punta entro cui LMB puo' attivare il magnete. Il tasto 2 da solo non lo attiva.")]
		[SerializeField]
		[Min(0.01f)]
		private float nippleMagnetRadius = 0.055f;

		// Token: 0x0400A949 RID: 43337
		[Tooltip("Isteresi corta usata soltanto mentre LMB resta premuto.")]
		[SerializeField]
		[Min(0.01f)]
		private float nippleMagnetReleaseRadius = 0.075f;

		// Token: 0x0400A94A RID: 43338
		[Header("Preset 3 - indice e medio")]
		[Tooltip("Indice e medio restano in bind pose; queste curve piegano anulare e mignolo.")]
		[SerializeField]
		[Range(0f, 90f)]
		private float vaginalFoldedProximalDegrees = 58f;

		// Token: 0x0400A94B RID: 43339
		[SerializeField]
		[Range(0f, 110f)]
		private float vaginalFoldedMiddleDegrees = 78f;

		// Token: 0x0400A94C RID: 43340
		[SerializeField]
		[Range(0f, 90f)]
		private float vaginalFoldedDistalDegrees = 48f;

		// Token: 0x0400A94D RID: 43341
		[SerializeField]
		[Range(0f, 90f)]
		private float vaginalFoldedThumbDegrees = 52f;

		// Token: 0x0400A94E RID: 43342
		[Header("Interazione vaginale")]
		[SerializeField]
		private string vaginalBlendShapeName = "Kupa_V_LR";

		// Token: 0x0400A94F RID: 43343
		[Tooltip("Centro della vulva nello spazio locale del bone Hips.")]
		[SerializeField]
		private Vector3 vaginalAnchorLocalOffset = new Vector3(-0.0061f, 0.0057f, 0.0761f);

		// Token: 0x0400A950 RID: 43344
		[Tooltip("Il click aggancia solo se il midpoint indice/medio e' vicino all'ancora.")]
		[SerializeField]
		[Min(0.01f)]
		private float vaginalAcquireRadius = 0.075f;

		// Token: 0x0400A951 RID: 43345
		[Tooltip("Distanza iniziale delle punte davanti al piano della vulva.")]
		[SerializeField]
		[Min(0f)]
		private float vaginalFingerClearance = 0.008f;

		// Token: 0x0400A952 RID: 43346
		[Tooltip("Quanto sotto l'ingresso partono le punte prima della spinta.")]
		[SerializeField]
		[Min(0.005f)]
		private float vaginalEntryBelowOffset = 0.03f;

		// Token: 0x0400A953 RID: 43347
		[Tooltip("Corsa verticale massima dentro, dall'ingresso verso l'alto.")]
		[SerializeField]
		[Min(0.005f)]
		private float vaginalInsertionDepth = 0.045f;

		// Token: 0x0400A954 RID: 43348
		[Tooltip("Piccola componente entrante verso il corpo durante la corsa verticale.")]
		[SerializeField]
		[Min(0f)]
		private float vaginalInsertionInwardDepth = 0.06f;

		// Token: 0x0400A955 RID: 43349
		[Tooltip("Quanto il delta verticale del mouse cambia la profondita' mentre LMB resta premuto.")]
		[SerializeField]
		[Min(0.001f)]
		private float vaginalMouseInsertionSensitivity = 0.065f;

		// Token: 0x0400A956 RID: 43350
		[Tooltip("Limita i picchi del mouse che altrimenti causerebbero salti di profondita'.")]
		[SerializeField]
		[Min(0.1f)]
		private float vaginalMouseDeltaClamp = 1.5f;

		// Token: 0x0400A957 RID: 43351
		[SerializeField]
		[Min(0f)]
		private float vaginalMouseDeadZone = 0.025f;

		// Token: 0x0400A958 RID: 43352
		[Tooltip("Smorzamento indipendente dell'ingresso/uscita delle dita.")]
		[SerializeField]
		[Min(0f)]
		private float vaginalInsertionSharpness = 18f;

		// Token: 0x0400A959 RID: 43353
		[SerializeField]
		[Range(0f, 100f)]
		private float vaginalBlendShapeMaxWeight = 100f;

		// Token: 0x0400A95A RID: 43354
		[SerializeField]
		[Min(0f)]
		private float vaginalOpenSharpness = 20f;

		// Token: 0x0400A95B RID: 43355
		[SerializeField]
		[Min(0f)]
		private float vaginalCloseSharpness = 14f;

		// Token: 0x0400A95C RID: 43356
		[Tooltip("Taratura locale della sola orientazione del preset 3.")]
		[SerializeField]
		private Vector3 vaginalHandEulerOffset = new Vector3(0f, 73.75f, 0f);

		// Token: 0x0400A95D RID: 43357
		[Tooltip("Offset Hips locale applicato soltanto al punto di partenza delle dita.")]
		[SerializeField]
		private Vector3 vaginalStartLocalOffset;

		// Token: 0x0400A95E RID: 43358
		[Tooltip("Rotazione locale aggiuntiva applicata soltanto alla partenza.")]
		[SerializeField]
		private Vector3 vaginalStartEulerOffset;

		// Token: 0x0400A95F RID: 43359
		[Tooltip("Offset Hips locale applicato soltanto a inserimento completo.")]
		[SerializeField]
		private Vector3 vaginalInsertedLocalOffset;

		// Token: 0x0400A960 RID: 43360
		[Tooltip("Rotazione locale aggiuntiva applicata soltanto a inserimento completo.")]
		[SerializeField]
		private Vector3 vaginalInsertedEulerOffset;

		// Token: 0x0400A961 RID: 43361
		[Header("Calibrazione preset 3 live")]
		[SerializeField]
		[Min(0.001f)]
		private float vaginalCalibrationPositionSpeed = 0.04f;

		// Token: 0x0400A962 RID: 43362
		[SerializeField]
		[Min(1f)]
		private float vaginalCalibrationRotationSpeed = 45f;

		// Token: 0x0400A963 RID: 43363
		[SerializeField]
		[Range(0.05f, 1f)]
		private float vaginalCalibrationFineMultiplier = 0.2f;

		// Token: 0x0400A964 RID: 43364
		[Header("Audio inserimento preset 3")]
		[SerializeField]
		private AudioClip vaginalInsertionClip1;

		// Token: 0x0400A965 RID: 43365
		[SerializeField]
		private AudioClip vaginalInsertionClip2;

		// Token: 0x0400A966 RID: 43366
		[SerializeField]
		[Range(0f, 1f)]
		private float vaginalInsertionSfxVolume = 0.8f;

		// Token: 0x0400A967 RID: 43367
		[SerializeField]
		private Vector2 vaginalInsertionSfxPitchRange = new Vector2(0.95f, 1.06f);

		// Token: 0x0400A968 RID: 43368
		[SerializeField]
		[Range(0f, 0.5f)]
		private float vaginalInsertionSfxVolumeJitter = 0.1f;

		// Token: 0x0400A969 RID: 43369
		[SerializeField]
		[Min(0f)]
		private float vaginalInsertionSfxMinInterval = 0.08f;

		// Token: 0x0400A96A RID: 43370
		[Tooltip("Movimento reale minimo, normalizzato sulla profondita', prima del suono.")]
		[SerializeField]
		[Range(0.005f, 0.5f)]
		private float vaginalInsertionSfxMotionThreshold = 0.035f;

		// Token: 0x0400A96B RID: 43371
		[Header("Loop vocali azioni")]
		[Tooltip("LewdBreathing: loop attivo soltanto durante un pinch capezzolo realmente agganciato.")]
		[SerializeField]
		private AudioClip nipplePinchVoiceLoopClip;

		// Token: 0x0400A96C RID: 43372
		[Tooltip("LewdEnjoyment1: loop attivo durante l'azione del preset 3 dentro la vagina.")]
		[SerializeField]
		private AudioClip vaginalVoiceLoopClip;

		// Token: 0x0400A96D RID: 43373
		[SerializeField]
		[Range(0f, 1f)]
		private float nipplePinchVoiceLoopVolume = 0.3f;

		// Token: 0x0400A96E RID: 43374
		[SerializeField]
		[Range(0f, 1f)]
		private float vaginalVoiceLoopVolume = 0.3f;

		// Token: 0x0400A96F RID: 43375
		[Tooltip("Sovrapposizione fra i due AudioSource al giro dell'MP3, per eliminare gap e click.")]
		[SerializeField]
		[Min(0.02f)]
		private float actionVoiceLoopCrossfade = 0.3f;

		// Token: 0x0400A970 RID: 43376
		[SerializeField]
		[Min(0f)]
		private float actionVoiceFadeIn = 0.35f;

		// Token: 0x0400A971 RID: 43377
		[SerializeField]
		[Min(0f)]
		private float actionVoiceFadeOut = 0.3f;

		// Token: 0x0400A972 RID: 43378
		[Header("Calibrazione pinch live")]
		[Tooltip("Fallback legacy usato per migrare i vecchi salvataggi globali.")]
		[SerializeField]
		private Vector3 nipplePinchCalibrationPosition;

		// Token: 0x0400A973 RID: 43379
		[Tooltip("Fallback legacy usato per migrare i vecchi salvataggi globali.")]
		[SerializeField]
		private Vector3 nipplePinchCalibrationEuler;

		// Token: 0x0400A974 RID: 43380
		[SerializeField]
		[HideInInspector]
		private bool hasPerNipplePinchCalibration;

		// Token: 0x0400A975 RID: 43381
		[Tooltip("Taratura indipendente dell'ancora capezzolo 1.")]
		[SerializeField]
		private Vector3 nipplePinchCalibrationTarget0Position;

		// Token: 0x0400A976 RID: 43382
		[SerializeField]
		private Vector3 nipplePinchCalibrationTarget0Euler;

		// Token: 0x0400A977 RID: 43383
		[Tooltip("Taratura indipendente dell'ancora capezzolo 2.")]
		[SerializeField]
		private Vector3 nipplePinchCalibrationTarget1Position;

		// Token: 0x0400A978 RID: 43384
		[SerializeField]
		private Vector3 nipplePinchCalibrationTarget1Euler;

		// Token: 0x0400A979 RID: 43385
		[SerializeField]
		[HideInInspector]
		private GropingModeController.PinchSizeCalibrationProfile[] nipplePinchCalibrationsBySize = new GropingModeController.PinchSizeCalibrationProfile[6];

		// Token: 0x0400A97A RID: 43386
		[SerializeField]
		[Range(0.1f, 1f)]
		private float pinchCalibrationGrip = 0.85f;

		// Token: 0x0400A97B RID: 43387
		[SerializeField]
		[Min(0.001f)]
		private float pinchCalibrationPositionSpeed = 0.04f;

		// Token: 0x0400A97C RID: 43388
		[SerializeField]
		[Min(1f)]
		private float pinchCalibrationRotationSpeed = 45f;

		// Token: 0x0400A97D RID: 43389
		[SerializeField]
		[Range(0.05f, 1f)]
		private float pinchCalibrationFineMultiplier = 0.2f;

		// Token: 0x0400A97E RID: 43390
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400A97F RID: 43391
		public const string PinchCalibrationPositionXKey = "Groping.PinchCalibration.Position.X";

		// Token: 0x0400A980 RID: 43392
		public const string PinchCalibrationPositionYKey = "Groping.PinchCalibration.Position.Y";

		// Token: 0x0400A981 RID: 43393
		public const string PinchCalibrationPositionZKey = "Groping.PinchCalibration.Position.Z";

		// Token: 0x0400A982 RID: 43394
		public const string PinchCalibrationEulerXKey = "Groping.PinchCalibration.Euler.X";

		// Token: 0x0400A983 RID: 43395
		public const string PinchCalibrationEulerYKey = "Groping.PinchCalibration.Euler.Y";

		// Token: 0x0400A984 RID: 43396
		public const string PinchCalibrationEulerZKey = "Groping.PinchCalibration.Euler.Z";

		// Token: 0x0400A985 RID: 43397
		public const string VaginalCalibrationAnchorXKey = "Groping.VaginalCalibration.Anchor.X";

		// Token: 0x0400A986 RID: 43398
		public const string VaginalCalibrationAnchorYKey = "Groping.VaginalCalibration.Anchor.Y";

		// Token: 0x0400A987 RID: 43399
		public const string VaginalCalibrationAnchorZKey = "Groping.VaginalCalibration.Anchor.Z";

		// Token: 0x0400A988 RID: 43400
		public const string VaginalCalibrationEulerXKey = "Groping.VaginalCalibration.Euler.X";

		// Token: 0x0400A989 RID: 43401
		public const string VaginalCalibrationEulerYKey = "Groping.VaginalCalibration.Euler.Y";

		// Token: 0x0400A98A RID: 43402
		public const string VaginalCalibrationEulerZKey = "Groping.VaginalCalibration.Euler.Z";

		// Token: 0x0400A98B RID: 43403
		private BoobTouchModeController _backend;

		// Token: 0x0400A98C RID: 43404
		private SoapNippleStimulation _stimulation;

		// Token: 0x0400A98D RID: 43405
		private readonly List<GropingModeController.RuntimeHand> _hands = new List<GropingModeController.RuntimeHand>(2);

		// Token: 0x0400A98E RID: 43406
		private GropingModeController.RuntimeHand _activeHand;

		// Token: 0x0400A98F RID: 43407
		private bool _active;

		// Token: 0x0400A990 RID: 43408
		private bool _activeGripRequested;

		// Token: 0x0400A991 RID: 43409
		private float _manualEntryRequestUntil = -1f;

		// Token: 0x0400A992 RID: 43410
		private GropingModeController.GrabPoseMode _selectedGrabMode = GropingModeController.GrabPoseMode.FullGrab;

		// Token: 0x0400A993 RID: 43411
		private Vector2 _interactionScreenPoint;

		// Token: 0x0400A994 RID: 43412
		private Vector2 _pointerPositionAtHandSpawn;

		// Token: 0x0400A995 RID: 43413
		private Vector2 _neutralHandScreenPoint;

		// Token: 0x0400A996 RID: 43414
		private Vector3 _activeHandNeutralLocalPoint;

		// Token: 0x0400A997 RID: 43415
		private float _pointerIntentDistance;

		// Token: 0x0400A998 RID: 43416
		private float _pointerSettlesAt;

		// Token: 0x0400A999 RID: 43417
		private bool _activeHandAwaitingPointerMove;

		// Token: 0x0400A99A RID: 43418
		private bool _initialGrabInputArmed;

		// Token: 0x0400A99B RID: 43419
		private bool _pinchCalibrationActive;

		// Token: 0x0400A99C RID: 43420
		private int _pinchCalibrationTargetIndex = -1;

		// Token: 0x0400A99D RID: 43421
		private readonly Vector3[] _runtimePinchCalibrationPositions = new Vector3[2];

		// Token: 0x0400A99E RID: 43422
		private readonly Vector3[] _runtimePinchCalibrationEulers = new Vector3[2];

		// Token: 0x0400A99F RID: 43423
		private int _activeBoobsType = 3;

		// Token: 0x0400A9A0 RID: 43424
		private string _pinchCalibrationStatus = "non salvata";

		// Token: 0x0400A9A1 RID: 43425
		private float _pinchCalibrationStatusUntil;

		// Token: 0x0400A9A2 RID: 43426
		private float _lastContinuousPinchError = -1f;

		// Token: 0x0400A9A3 RID: 43427
		private int _lastContinuousPinchFrame = -10;

		// Token: 0x0400A9A4 RID: 43428
		private bool _vaginalCalibrationActive;

		// Token: 0x0400A9A5 RID: 43429
		private bool _vaginalCalibrationInsertedPreview;

		// Token: 0x0400A9A6 RID: 43430
		private Vector3 _runtimeVaginalAnchorLocalOffset;

		// Token: 0x0400A9A7 RID: 43431
		private Vector3 _runtimeVaginalHandEuler;

		// Token: 0x0400A9A8 RID: 43432
		private readonly Vector3[] _runtimeVaginalEndpointPositions = new Vector3[2];

		// Token: 0x0400A9A9 RID: 43433
		private readonly Vector3[] _runtimeVaginalEndpointEulers = new Vector3[2];

		// Token: 0x0400A9AA RID: 43434
		private string _vaginalCalibrationStatus = "non salvata";

		// Token: 0x0400A9AB RID: 43435
		private float _vaginalCalibrationStatusUntil;

		// Token: 0x0400A9AC RID: 43436
		private Transform _vaginalHips;

		// Token: 0x0400A9AD RID: 43437
		private SkinnedMeshRenderer _vaginalBodyRenderer;

		// Token: 0x0400A9AE RID: 43438
		private int _vaginalBlendShapeIndex = -1;

		// Token: 0x0400A9AF RID: 43439
		private float _vaginalBlendShapeBaseline;

		// Token: 0x0400A9B0 RID: 43440
		private float _vaginalBlendShapeWeight;

		// Token: 0x0400A9B1 RID: 43441
		private BlendShapeManager _blendShapeManager;

		// Token: 0x0400A9B2 RID: 43442
		private AudioSource _vaginalAudioSource;

		// Token: 0x0400A9B3 RID: 43443
		private int _vaginalNextSfxClip;

		// Token: 0x0400A9B4 RID: 43444
		private int _vaginalSfxPlayCount;

		// Token: 0x0400A9B5 RID: 43445
		private float _vaginalLastSfxTime = -999f;

		// Token: 0x0400A9B6 RID: 43446
		private float _lastActiveVaginalPassage;

		// Token: 0x0400A9B7 RID: 43447
		private float _vaginalPushGestureStartPassage;

		// Token: 0x0400A9B8 RID: 43448
		private float _vaginalPushGestureLastInputTime = -999f;

		// Token: 0x0400A9B9 RID: 43449
		private bool _vaginalPushGestureActive;

		// Token: 0x0400A9BA RID: 43450
		private bool _vaginalPushSfxPending;

		// Token: 0x0400A9BB RID: 43451
		private AudioSource _nipplePinchVoiceSourceA;

		// Token: 0x0400A9BC RID: 43452
		private AudioSource _nipplePinchVoiceSourceB;

		// Token: 0x0400A9BD RID: 43453
		private AudioSource _vaginalVoiceSourceA;

		// Token: 0x0400A9BE RID: 43454
		private AudioSource _vaginalVoiceSourceB;

		// Token: 0x0400A9BF RID: 43455
		private CrossfadeLoopPlayer _nipplePinchVoiceLoop;

		// Token: 0x0400A9C0 RID: 43456
		private CrossfadeLoopPlayer _vaginalVoiceLoop;

		// Token: 0x0400A9C1 RID: 43457
		private bool _nipplePinchVoiceRequested;

		// Token: 0x0400A9C2 RID: 43458
		private bool _vaginalVoiceRequested;

		// Token: 0x0400A9C3 RID: 43459
		private float _nipplePinchVoiceVolume;

		// Token: 0x0400A9C4 RID: 43460
		private float _vaginalVoiceVolume;

		// Token: 0x0400A9C5 RID: 43461
		private GropingModeController.GropingVoiceChannel _activeVoiceChannel;

		// Token: 0x0400A9C6 RID: 43462
		private CielVoiceSO _queuedStimulationVoice;

		// Token: 0x0400A9C7 RID: 43463
		private CielVoiceSO _ownedStimulationVoice;

		// Token: 0x0400A9C8 RID: 43464
		private float _queuedStimulationSubtitleHold;

		// Token: 0x0400A9C9 RID: 43465
		private bool _stimulationVoiceRequested;

		// Token: 0x0400A9CA RID: 43466
		private bool _stimulationVoiceStopRequested;

		// Token: 0x0400A9CB RID: 43467
		private AudioSource _stimulationVoiceSource;

		// Token: 0x0400A9CC RID: 43468
		private float _stimulationVoiceTargetVolume;

		// Token: 0x0400A9CD RID: 43469
		private int _maximumSimultaneousVoiceChannels;

		// Token: 0x0400A9CE RID: 43470
		private int _currentReactionFaceIndex = -1;

		// Token: 0x0400A9CF RID: 43471
		private bool _orgasmReactionActive;

		// Token: 0x0400A9D0 RID: 43472
		private const string VaginalBlendShapeSource = "GropingModeController.VaginalFingers";

		// Token: 0x0400A9D1 RID: 43473
		private const string CalibrationLocalizationScope = "groping_ui";

		// Token: 0x0400A9D2 RID: 43474
		private Func<string> _pinchCalibrationStatusPresentation;

		// Token: 0x0400A9D3 RID: 43475
		private Func<string> _vaginalCalibrationStatusPresentation;

		// Token: 0x0400A9D4 RID: 43476
		private GUIStyle[] _calibrationGuiStyles;

		// Token: 0x0400A9D5 RID: 43477
		private Font[] _calibrationOriginalFonts;

		// Token: 0x0400A9D6 RID: 43478
		private bool[] _calibrationOriginalWrap;

		// Token: 0x0400A9D7 RID: 43479
		private Vector2 _calibrationGuiScroll;

		// Token: 0x020014E7 RID: 5351
		public enum GrabPoseMode
		{
			// Token: 0x0400A9D9 RID: 43481
			FullGrab = 1,
			// Token: 0x0400A9DA RID: 43482
			NipplePinch,
			// Token: 0x0400A9DB RID: 43483
			VaginalFingers
		}

		// Token: 0x020014E8 RID: 5352
		private enum GropingVoiceChannel
		{
			// Token: 0x0400A9DD RID: 43485
			None,
			// Token: 0x0400A9DE RID: 43486
			TouchReaction,
			// Token: 0x0400A9DF RID: 43487
			NipplePinch,
			// Token: 0x0400A9E0 RID: 43488
			Vaginal
		}

		// Token: 0x020014E9 RID: 5353
		[Serializable]
		private struct PinchSizeCalibrationProfile
		{
			// Token: 0x0400A9E1 RID: 43489
			public bool hasValues;

			// Token: 0x0400A9E2 RID: 43490
			public Vector3 target0Position;

			// Token: 0x0400A9E3 RID: 43491
			public Vector3 target0Euler;

			// Token: 0x0400A9E4 RID: 43492
			public Vector3 target1Position;

			// Token: 0x0400A9E5 RID: 43493
			public Vector3 target1Euler;
		}

		// Token: 0x020014EA RID: 5354
		private sealed class RuntimeHand
		{
			// Token: 0x1700197A RID: 6522
			// (get) Token: 0x0600A55D RID: 42333 RVA: 0x00451AB3 File Offset: 0x0044FCB3
			public bool IsLeft
			{
				get
				{
					return this._isLeft;
				}
			}

			// Token: 0x1700197B RID: 6523
			// (get) Token: 0x0600A55E RID: 42334 RVA: 0x00451ABB File Offset: 0x0044FCBB
			// (set) Token: 0x0600A55F RID: 42335 RVA: 0x00451AC3 File Offset: 0x0044FCC3
			public bool IsPlaced { get; private set; }

			// Token: 0x1700197C RID: 6524
			// (get) Token: 0x0600A560 RID: 42336 RVA: 0x00451ACC File Offset: 0x0044FCCC
			// (set) Token: 0x0600A561 RID: 42337 RVA: 0x00451AD4 File Offset: 0x0044FCD4
			public bool HasSurfaceContact { get; private set; }

			// Token: 0x1700197D RID: 6525
			// (get) Token: 0x0600A562 RID: 42338 RVA: 0x00451ADD File Offset: 0x0044FCDD
			public bool IsTouchingSurface
			{
				get
				{
					return this.HasSurfaceContact && (this.IsPlaced || Time.frameCount - this._surfaceUpdatedFrame <= 2);
				}
			}

			// Token: 0x1700197E RID: 6526
			// (get) Token: 0x0600A563 RID: 42339 RVA: 0x00451B05 File Offset: 0x0044FD05
			// (set) Token: 0x0600A564 RID: 42340 RVA: 0x00451B0D File Offset: 0x0044FD0D
			public float GripAmount { get; private set; }

			// Token: 0x1700197F RID: 6527
			// (get) Token: 0x0600A565 RID: 42341 RVA: 0x00451B16 File Offset: 0x0044FD16
			// (set) Token: 0x0600A566 RID: 42342 RVA: 0x00451B1E File Offset: 0x0044FD1E
			public float LockedGrip { get; private set; }

			// Token: 0x17001980 RID: 6528
			// (get) Token: 0x0600A567 RID: 42343 RVA: 0x00451B27 File Offset: 0x0044FD27
			public int ColliderCount
			{
				get
				{
					return this._colliders.Count;
				}
			}

			// Token: 0x17001981 RID: 6529
			// (get) Token: 0x0600A568 RID: 42344 RVA: 0x00451B34 File Offset: 0x0044FD34
			public int EnabledColliderCount
			{
				get
				{
					int num = 0;
					foreach (ColliderComponent colliderComponent in this._colliders)
					{
						if (colliderComponent != null && colliderComponent.enabled)
						{
							num++;
						}
					}
					return num;
				}
			}

			// Token: 0x17001982 RID: 6530
			// (get) Token: 0x0600A569 RID: 42345 RVA: 0x00451B98 File Offset: 0x0044FD98
			public int OwnerId
			{
				get
				{
					if (!(this._root != null))
					{
						return 0;
					}
					return this._root.GetInstanceID();
				}
			}

			// Token: 0x17001983 RID: 6531
			// (get) Token: 0x0600A56A RID: 42346 RVA: 0x00451BB5 File Offset: 0x0044FDB5
			// (set) Token: 0x0600A56B RID: 42347 RVA: 0x00451BBD File Offset: 0x0044FDBD
			public GropingModeController.GrabPoseMode PoseMode { get; private set; } = GropingModeController.GrabPoseMode.FullGrab;

			// Token: 0x17001984 RID: 6532
			// (get) Token: 0x0600A56C RID: 42348 RVA: 0x00451BC6 File Offset: 0x0044FDC6
			public bool IsGrabLatched
			{
				get
				{
					return this.GrabTargetIndex >= 0;
				}
			}

			// Token: 0x17001985 RID: 6533
			// (get) Token: 0x0600A56D RID: 42349 RVA: 0x00451BD4 File Offset: 0x0044FDD4
			// (set) Token: 0x0600A56E RID: 42350 RVA: 0x00451BDC File Offset: 0x0044FDDC
			public bool IsVaginalLatched { get; private set; }

			// Token: 0x17001986 RID: 6534
			// (get) Token: 0x0600A56F RID: 42351 RVA: 0x00451BE5 File Offset: 0x0044FDE5
			public float VaginalInsertionTarget
			{
				get
				{
					return this._vaginalInsertionTarget;
				}
			}

			// Token: 0x17001987 RID: 6535
			// (get) Token: 0x0600A570 RID: 42352 RVA: 0x00451BED File Offset: 0x0044FDED
			public float VaginalInsertionAmount
			{
				get
				{
					return this._vaginalInsertionAmount;
				}
			}

			// Token: 0x17001988 RID: 6536
			// (get) Token: 0x0600A571 RID: 42353 RVA: 0x00451BF5 File Offset: 0x0044FDF5
			// (set) Token: 0x0600A572 RID: 42354 RVA: 0x00451BFD File Offset: 0x0044FDFD
			public int GrabTargetIndex { get; private set; } = -1;

			// Token: 0x17001989 RID: 6537
			// (get) Token: 0x0600A573 RID: 42355 RVA: 0x00451C06 File Offset: 0x0044FE06
			// (set) Token: 0x0600A574 RID: 42356 RVA: 0x00451C0E File Offset: 0x0044FE0E
			public int NippleMagnetTargetIndex { get; private set; } = -1;

			// Token: 0x1700198A RID: 6538
			// (get) Token: 0x0600A575 RID: 42357 RVA: 0x00451C17 File Offset: 0x0044FE17
			// (set) Token: 0x0600A576 RID: 42358 RVA: 0x00451C1F File Offset: 0x0044FE1F
			public BoobTouchModeController.ExternalGrabKind BackendGrabKind { get; private set; } = BoobTouchModeController.ExternalGrabKind.Breast;

			// Token: 0x1700198B RID: 6539
			// (get) Token: 0x0600A577 RID: 42359 RVA: 0x00451C28 File Offset: 0x0044FE28
			public Vector3 GrabPointWorld
			{
				get
				{
					return this.ContactWorldPoint(this.PoseMode);
				}
			}

			// Token: 0x1700198C RID: 6540
			// (get) Token: 0x0600A578 RID: 42360 RVA: 0x00451C36 File Offset: 0x0044FE36
			public Vector3 GrabTargetPointWorld
			{
				get
				{
					return this.TargetContactWorld(this.PoseMode);
				}
			}

			// Token: 0x1700198D RID: 6541
			// (get) Token: 0x0600A579 RID: 42361 RVA: 0x00451C44 File Offset: 0x0044FE44
			public float GrabDragDistance
			{
				get
				{
					if (!this.IsGrabLatched)
					{
						return 0f;
					}
					return Vector3.Distance(this.TargetContactWorld(this.PoseMode), this._grabOriginContact);
				}
			}

			// Token: 0x1700198E RID: 6542
			// (get) Token: 0x0600A57A RID: 42362 RVA: 0x00451C6B File Offset: 0x0044FE6B
			public float GrabMovementLimit
			{
				get
				{
					return this._grabMovementLimit;
				}
			}

			// Token: 0x1700198F RID: 6543
			// (get) Token: 0x0600A57B RID: 42363 RVA: 0x00451C73 File Offset: 0x0044FE73
			public Vector3 SurfacePoint
			{
				get
				{
					return this._surfacePoint;
				}
			}

			// Token: 0x17001990 RID: 6544
			// (get) Token: 0x0600A57C RID: 42364 RVA: 0x00451C7B File Offset: 0x0044FE7B
			public float PinchSpan
			{
				get
				{
					if (!(this._indexTipTarget != null) || !(this._thumbTipTarget != null))
					{
						return 0f;
					}
					return Vector3.Distance(this._indexTipTarget.position, this._thumbTipTarget.position);
				}
			}

			// Token: 0x17001991 RID: 6545
			// (get) Token: 0x0600A57D RID: 42365 RVA: 0x00451CBC File Offset: 0x0044FEBC
			public Vector3 VaginalFingerMidpoint
			{
				get
				{
					if (!(this._indexTipTarget != null) || !(this._middleTipTarget != null))
					{
						return this._root.transform.position;
					}
					return Vector3.Lerp(this._indexTipTarget.position, this._middleTipTarget.position, 0.5f);
				}
			}

			// Token: 0x17001992 RID: 6546
			// (get) Token: 0x0600A57E RID: 42366 RVA: 0x00451D18 File Offset: 0x0044FF18
			public float ActualPalmToBodyDot
			{
				get
				{
					if (this._surfaceNormal.sqrMagnitude <= 1E-06f)
					{
						return 0f;
					}
					return Vector3.Dot(this._root.transform.TransformDirection(Vector3.forward), -this._surfaceNormal.normalized);
				}
			}

			// Token: 0x17001993 RID: 6547
			// (get) Token: 0x0600A57F RID: 42367 RVA: 0x00451D67 File Offset: 0x0044FF67
			public float TargetPalmToBodyDot
			{
				get
				{
					if (this._surfaceNormal.sqrMagnitude <= 1E-06f)
					{
						return 0f;
					}
					return Vector3.Dot(this._targetRotation * Vector3.forward, -this._surfaceNormal.normalized);
				}
			}

			// Token: 0x0600A580 RID: 42368 RVA: 0x00451DA8 File Offset: 0x0044FFA8
			public RuntimeHand(GameObject prefab, bool isLeft, Vector3 startPosition, Quaternion startRotation, float targetLength, float palmRadius, float fingerRadius, float thumbRadius, BoobTouchModeController backend, Color skinColor, float opacity)
			{
				this._isLeft = isLeft;
				this._root = new GameObject(isLeft ? "GropingHand.Left" : "GropingHand.Right");
				this._root.transform.SetPositionAndRotation(startPosition, startRotation);
				this._body = this._root.AddComponent<Rigidbody>();
				this._body.isKinematic = true;
				this._body.useGravity = false;
				this._body.interpolation = RigidbodyInterpolation.Interpolate;
				this._body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
				GameObject gameObject = Object.Instantiate<GameObject>(prefab, this._root.transform);
				gameObject.name = "Visual";
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				Collider[] componentsInChildren = gameObject.GetComponentsInChildren<Collider>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					Object.Destroy(componentsInChildren[i]);
				}
				Rigidbody[] componentsInChildren2 = gameObject.GetComponentsInChildren<Rigidbody>(true);
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					Object.Destroy(componentsInChildren2[i]);
				}
				this.ApplyVisualAppearance(gameObject, skinColor, opacity);
				this.NormalizeAndCenterVisual(gameObject.transform, targetLength);
				Transform[] componentsInChildren3 = gameObject.GetComponentsInChildren<Transform>(true);
				this._wristBone = GropingModeController.RuntimeHand.FindBone(componentsInChildren3, "StylizedHand." + this.SideSuffix);
				this._middleBaseBone = GropingModeController.RuntimeHand.FindBone(componentsInChildren3, "ProximalMiddle_Bone." + this.SideSuffix);
				this._indexProximalBone = GropingModeController.RuntimeHand.FindBone(componentsInChildren3, "ProximalIndex_Bone." + this.SideSuffix);
				this._thumbProximalBone = GropingModeController.RuntimeHand.FindBone(componentsInChildren3, "ProximalThumb_Bone." + this.SideSuffix);
				this.CacheFingerBones(componentsInChildren3);
				this._palmTarget = this.CreatePalmTarget();
				this.CreateCollider("Palm", GropingModeController.RuntimeHand.ColliderRole.Palm, this._palmTarget, palmRadius, backend);
				this._indexTipTarget = this.CreateFingerCollider(componentsInChildren3, "Index", GropingModeController.RuntimeHand.ColliderRole.Index, fingerRadius, backend);
				this._middleTipTarget = this.CreateFingerCollider(componentsInChildren3, "Middle", GropingModeController.RuntimeHand.ColliderRole.Middle, fingerRadius, backend);
				this.CreateFingerCollider(componentsInChildren3, "Ring", GropingModeController.RuntimeHand.ColliderRole.Ring, fingerRadius, backend);
				this.CreateFingerCollider(componentsInChildren3, "Pink", GropingModeController.RuntimeHand.ColliderRole.Pink, fingerRadius * 0.9f, backend);
				this._thumbTipTarget = this.CreateFingerCollider(componentsInChildren3, "Thumb", GropingModeController.RuntimeHand.ColliderRole.Thumb, thumbRadius, backend);
				this._targetPosition = startPosition;
				this._targetRotation = startRotation;
				this._hasTarget = true;
			}

			// Token: 0x17001994 RID: 6548
			// (get) Token: 0x0600A581 RID: 42369 RVA: 0x0045206F File Offset: 0x0045026F
			private string SideSuffix
			{
				get
				{
					if (!this._isLeft)
					{
						return "Right";
					}
					return "Left";
				}
			}

			// Token: 0x0600A582 RID: 42370 RVA: 0x00452084 File Offset: 0x00450284
			public void TickGrip(float target, float inSpeed, float outSpeed, float dt)
			{
				float num = ((target > this.GripAmount) ? inSpeed : outSpeed);
				this.GripAmount = Mathf.MoveTowards(this.GripAmount, Mathf.Clamp01(target), Mathf.Max(0f, num) * Mathf.Max(0f, dt));
			}

			// Token: 0x0600A583 RID: 42371 RVA: 0x004520CE File Offset: 0x004502CE
			public void SetGripImmediate(float amount)
			{
				this.GripAmount = Mathf.Clamp01(amount);
			}

			// Token: 0x0600A584 RID: 42372 RVA: 0x004520DC File Offset: 0x004502DC
			public void TickVaginalInsertion(float mouseDelta, bool held, float sensitivity, float deadZone, float sharpness, float dt)
			{
				if (!this.IsVaginalLatched)
				{
					this._vaginalInsertionTarget = 0f;
					this._vaginalInsertionAmount = 0f;
					return;
				}
				if (!held)
				{
					this._vaginalInsertionTarget = 0f;
				}
				else if (Mathf.Abs(mouseDelta) > Mathf.Max(0f, deadZone))
				{
					this._vaginalInsertionTarget = Mathf.Clamp01(this._vaginalInsertionTarget + mouseDelta * Mathf.Max(0.001f, sensitivity));
				}
				this._vaginalInsertionAmount = Mathf.Lerp(this._vaginalInsertionAmount, this._vaginalInsertionTarget, GropingModeController.RuntimeHand.ExpBlend(sharpness, dt));
				if (Mathf.Abs(this._vaginalInsertionAmount - this._vaginalInsertionTarget) < 0.0001f)
				{
					this._vaginalInsertionAmount = this._vaginalInsertionTarget;
				}
			}

			// Token: 0x0600A585 RID: 42373 RVA: 0x00452190 File Offset: 0x00450390
			public void SetVaginalInsertionTarget(float amount)
			{
				if (this.IsVaginalLatched)
				{
					this._vaginalInsertionTarget = Mathf.Clamp01(amount);
				}
			}

			// Token: 0x0600A586 RID: 42374 RVA: 0x004521A8 File Offset: 0x004503A8
			public void SetVaginalInsertionImmediate(float amount)
			{
				float num = Mathf.Clamp01(amount);
				this._vaginalInsertionTarget = num;
				this._vaginalInsertionAmount = num;
			}

			// Token: 0x0600A587 RID: 42375 RVA: 0x004521CA File Offset: 0x004503CA
			public void SetPoseMode(GropingModeController.GrabPoseMode mode)
			{
				if (this.IsPlaced)
				{
					return;
				}
				this.ReleaseVaginalInsertion();
				this.PoseMode = mode;
				this.ClearNippleMagnet();
				this._alignPinchCenter = false;
			}

			// Token: 0x0600A588 RID: 42376 RVA: 0x004521EF File Offset: 0x004503EF
			public void SetNippleMagnet(int breastIndex)
			{
				this.NippleMagnetTargetIndex = breastIndex;
			}

			// Token: 0x0600A589 RID: 42377 RVA: 0x004521F8 File Offset: 0x004503F8
			public void ClearNippleMagnet()
			{
				this.NippleMagnetTargetIndex = -1;
			}

			// Token: 0x0600A58A RID: 42378 RVA: 0x00452201 File Offset: 0x00450401
			public void SetCalibrationPreview(bool active)
			{
				this._calibrationPreview = active;
				this.ApplyColliderMode();
			}

			// Token: 0x0600A58B RID: 42379 RVA: 0x00452210 File Offset: 0x00450410
			public Quaternion ResolveSurfaceRotation(Vector3 normal, Vector3 preferredUp, float rollDegrees, float sharpness, float deltaTime)
			{
				normal = ((normal.sqrMagnitude > 1E-06f) ? normal.normalized : Vector3.forward);
				Vector3 vector = Vector3.ProjectOnPlane(preferredUp, normal);
				if (vector.sqrMagnitude < 1E-06f)
				{
					vector = Vector3.ProjectOnPlane(Vector3.up, normal);
				}
				if (vector.sqrMagnitude < 1E-06f)
				{
					vector = Vector3.right;
				}
				vector.Normalize();
				if (!this._hasStableSurfaceFrame)
				{
					this._stableSurfaceUp = vector;
					this._hasStableSurfaceFrame = true;
				}
				else
				{
					float num = GropingModeController.RuntimeHand.ExpBlend(sharpness, deltaTime);
					Vector3 vector2 = Vector3.ProjectOnPlane(this._stableSurfaceUp, normal);
					if (vector2.sqrMagnitude < 1E-06f)
					{
						vector2 = vector;
					}
					vector2.Normalize();
					vector = Vector3.ProjectOnPlane(vector, normal);
					if (vector.sqrMagnitude < 1E-06f)
					{
						vector = vector2;
					}
					else
					{
						vector.Normalize();
					}
					if (Vector3.Dot(vector, vector2) < 0f)
					{
						vector = -vector;
					}
					Vector3 vector3 = Vector3.Slerp(vector2, vector, 0.55f);
					this._stableSurfaceUp = Vector3.Slerp(vector2, vector3, num).normalized;
				}
				return Quaternion.LookRotation(-normal, this._stableSurfaceUp) * Quaternion.Euler(0f, 0f, rollDegrees);
			}

			// Token: 0x0600A58C RID: 42380 RVA: 0x00452344 File Offset: 0x00450544
			public Quaternion EnsureVaginalFingersPointUp(Quaternion candidate, Vector3 anatomicalUp)
			{
				Vector3 vaginalFingerDirectionLocal = this.GetVaginalFingerDirectionLocal();
				if (vaginalFingerDirectionLocal.sqrMagnitude < 1E-06f || anatomicalUp.sqrMagnitude < 1E-06f)
				{
					return candidate;
				}
				anatomicalUp.Normalize();
				if (Vector3.Dot(candidate * vaginalFingerDirectionLocal, anatomicalUp) < 0f)
				{
					candidate *= Quaternion.AngleAxis(180f, Vector3.forward);
				}
				return candidate;
			}

			// Token: 0x0600A58D RID: 42381 RVA: 0x004523AC File Offset: 0x004505AC
			public float GetVaginalFingerUpDot(Vector3 anatomicalUp)
			{
				Vector3 vaginalFingerDirectionWorld = this.GetVaginalFingerDirectionWorld();
				if (vaginalFingerDirectionWorld.sqrMagnitude <= 1E-06f || anatomicalUp.sqrMagnitude <= 1E-06f)
				{
					return 0f;
				}
				return Vector3.Dot(vaginalFingerDirectionWorld.normalized, anatomicalUp.normalized);
			}

			// Token: 0x0600A58E RID: 42382 RVA: 0x004523F8 File Offset: 0x004505F8
			public void SetSurfaceTarget(Vector3 contactWorld, Quaternion rotation, Vector3 surfacePoint, Vector3 surfaceNormal, bool alignPinchCenter)
			{
				if (this.IsPlaced)
				{
					return;
				}
				GropingModeController.GrabPoseMode grabPoseMode = (alignPinchCenter ? GropingModeController.GrabPoseMode.NipplePinch : GropingModeController.GrabPoseMode.FullGrab);
				Vector3 vector = this.ContactLocalPoint(grabPoseMode);
				this._targetPosition = contactWorld - rotation * vector;
				this._targetRotation = rotation;
				this._surfacePoint = surfacePoint;
				this._surfaceNormal = surfaceNormal;
				this._alignPinchCenter = alignPinchCenter;
				this._hasTarget = true;
				this.HasSurfaceContact = true;
				this._surfaceUpdatedFrame = Time.frameCount;
			}

			// Token: 0x0600A58F RID: 42383 RVA: 0x0045246C File Offset: 0x0045066C
			public void SnapToTargetPose()
			{
				if (this._body == null || this._root == null || !this._hasTarget || this.IsPlaced)
				{
					return;
				}
				this._body.position = this._targetPosition;
				this._body.rotation = this._targetRotation;
				this._root.transform.SetPositionAndRotation(this._targetPosition, this._targetRotation);
			}

			// Token: 0x0600A590 RID: 42384 RVA: 0x004524E4 File Offset: 0x004506E4
			public void SetVaginalTarget(Vector3 fingerMidpointWorld, Quaternion rotation, Vector3 surfacePoint, Vector3 surfaceNormal)
			{
				if (this.IsPlaced)
				{
					return;
				}
				Vector3 vector = this.ContactLocalPoint(GropingModeController.GrabPoseMode.VaginalFingers);
				this._targetPosition = fingerMidpointWorld - rotation * vector;
				this._targetRotation = rotation;
				this._surfacePoint = surfacePoint;
				this._surfaceNormal = surfaceNormal;
				this._alignPinchCenter = false;
				this._hasTarget = true;
				this.HasSurfaceContact = true;
				this._surfaceUpdatedFrame = Time.frameCount;
			}

			// Token: 0x0600A591 RID: 42385 RVA: 0x0045254B File Offset: 0x0045074B
			public void BeginVaginalInsertion()
			{
				if (this.IsPlaced || !this._hasTarget)
				{
					return;
				}
				if (!this.IsVaginalLatched)
				{
					this._vaginalInsertionTarget = 0f;
					this._vaginalInsertionAmount = 0f;
				}
				this.IsVaginalLatched = true;
				this.ClearNippleMagnet();
			}

			// Token: 0x0600A592 RID: 42386 RVA: 0x00452589 File Offset: 0x00450789
			public void ReleaseVaginalInsertion()
			{
				this.IsVaginalLatched = false;
				this._vaginalInsertionTarget = 0f;
				this._vaginalInsertionAmount = 0f;
			}

			// Token: 0x0600A593 RID: 42387 RVA: 0x004525A8 File Offset: 0x004507A8
			public void BeginGrab(int breastIndex, BoobTouchModeController.ExternalGrabKind kind, Camera cam, Vector2 screenPoint, float movementLimit, Vector3 continuousPinchAnchorWorld)
			{
				if (this.IsPlaced || breastIndex < 0 || cam == null || !this._hasTarget)
				{
					return;
				}
				this.GrabTargetIndex = breastIndex;
				this.BackendGrabKind = kind;
				Vector3 vector = this.TargetContactWorld(this.PoseMode);
				this._grabOriginContact = vector;
				this._grabMovementLimit = Mathf.Max(0.01f, movementLimit);
				this._grabContactClearance = Vector3.Dot(vector - this._surfacePoint, this._surfaceNormal);
				this._grabPlane = new Plane(cam.transform.forward, vector);
				this._hasContinuousPinchAttachment = kind == BoobTouchModeController.ExternalGrabKind.Nipple;
				if (this._hasContinuousPinchAttachment)
				{
					this._pinchContactFromAnchorLocal = Quaternion.Inverse(this._targetRotation) * (vector - continuousPinchAnchorWorld);
					this._body.interpolation = RigidbodyInterpolation.None;
				}
				Ray ray = cam.ScreenPointToRay(screenPoint);
				float num;
				if (this._grabPlane.Raycast(ray, out num))
				{
					this._grabPlaneCursorOffset = vector - ray.GetPoint(num);
					return;
				}
				this._grabPlaneCursorOffset = Vector3.zero;
			}

			// Token: 0x0600A594 RID: 42388 RVA: 0x004526B8 File Offset: 0x004508B8
			public void UpdateGrabPlaneTarget(Camera cam, Vector2 screenPoint, BoobTouchModeController backend)
			{
				if (!this.IsGrabLatched || this.IsPlaced || cam == null)
				{
					return;
				}
				Ray ray = cam.ScreenPointToRay(screenPoint);
				float num;
				if (!this._grabPlane.Raycast(ray, out num))
				{
					return;
				}
				Vector3 vector = ray.GetPoint(num) + this._grabPlaneCursorOffset;
				if (backend != null)
				{
					vector = backend.ConstrainExternalGrabMotion(this.BackendGrabKind, this._grabOriginContact, vector);
				}
				vector = this.ConstrainGrabContact(vector);
				Vector3 vector2 = vector - this._surfaceNormal * this._grabContactClearance;
				this.SetSurfaceTarget(vector, this._targetRotation, vector2, this._surfaceNormal, this._alignPinchCenter);
			}

			// Token: 0x0600A595 RID: 42389 RVA: 0x00452768 File Offset: 0x00450968
			public void ReleaseGrab(BoobTouchModeController backend)
			{
				if (this.IsGrabLatched && backend != null)
				{
					backend.ReleaseExternalBreastGrab(this.OwnerId);
				}
				this.GrabTargetIndex = -1;
				this._grabPlaneCursorOffset = Vector3.zero;
				this._grabMovementLimit = 0f;
				this._hasContinuousPinchAttachment = false;
				if (this._body != null)
				{
					this._body.interpolation = RigidbodyInterpolation.Interpolate;
				}
				this.ClearNippleMagnet();
			}

			// Token: 0x0600A596 RID: 42390 RVA: 0x004527D8 File Offset: 0x004509D8
			public void DebugNudgeGrab(Vector3 worldDelta, BoobTouchModeController backend)
			{
				if (!this.IsGrabLatched || this.IsPlaced)
				{
					return;
				}
				Vector3 vector = this.TargetContactWorld(this.PoseMode);
				Vector3 vector2 = vector + worldDelta;
				if (backend != null)
				{
					vector2 = backend.ConstrainExternalGrabMotion(this.BackendGrabKind, this._grabOriginContact, vector2);
				}
				vector2 = this.ConstrainGrabContact(vector2);
				Vector3 vector3 = vector2 - vector;
				this._grabPlaneCursorOffset += vector3;
				this._targetPosition += vector3;
				this._surfacePoint += vector3;
			}

			// Token: 0x0600A597 RID: 42391 RVA: 0x00452870 File Offset: 0x00450A70
			public float LockPinchToSolvedNipple(Vector3 anchorWorld, Vector3 outwardWorld)
			{
				if (!this._hasContinuousPinchAttachment || !this.IsGrabLatched || this.BackendGrabKind != BoobTouchModeController.ExternalGrabKind.Nipple || this._body == null)
				{
					return -1f;
				}
				if (outwardWorld.sqrMagnitude < 1E-06f)
				{
					outwardWorld = this._surfaceNormal;
				}
				else
				{
					outwardWorld.Normalize();
				}
				if (this._surfaceNormal.sqrMagnitude > 1E-06f && Vector3.Dot(outwardWorld, this._surfaceNormal) < 0f)
				{
					outwardWorld = -outwardWorld;
				}
				Vector3 vector = anchorWorld + this._targetRotation * this._pinchContactFromAnchorLocal;
				Vector3 vector2 = this.ContactLocalPoint(GropingModeController.GrabPoseMode.NipplePinch);
				Vector3 vector3 = vector - this._targetRotation * vector2;
				this._targetPosition = vector3;
				this._surfacePoint = anchorWorld;
				this._surfaceNormal = outwardWorld;
				this._hasTarget = true;
				this.HasSurfaceContact = true;
				this._surfaceUpdatedFrame = Time.frameCount;
				this._body.position = vector3;
				this._body.rotation = this._targetRotation;
				this._root.transform.SetPositionAndRotation(vector3, this._targetRotation);
				Vector3 vector4 = vector - this.ContactWorldPoint(GropingModeController.GrabPoseMode.NipplePinch);
				if (vector4.sqrMagnitude > 1E-10f)
				{
					this._targetPosition += vector4;
					this._body.position = this._targetPosition;
					this._root.transform.position = this._targetPosition;
				}
				return Vector3.Distance(vector, this.ContactWorldPoint(GropingModeController.GrabPoseMode.NipplePinch));
			}

			// Token: 0x0600A598 RID: 42392 RVA: 0x004529EB File Offset: 0x00450BEB
			private Vector3 ConstrainGrabContact(Vector3 desiredContact)
			{
				if (!this.IsGrabLatched || this._grabMovementLimit <= 0f)
				{
					return desiredContact;
				}
				return this._grabOriginContact + Vector3.ClampMagnitude(desiredContact - this._grabOriginContact, this._grabMovementLimit);
			}

			// Token: 0x0600A599 RID: 42393 RVA: 0x00452A28 File Offset: 0x00450C28
			public void Place(Transform chest)
			{
				if (this.IsPlaced || chest == null || !this._hasTarget)
				{
					return;
				}
				this.IsPlaced = true;
				this.LockedGrip = this.GripAmount;
				this._placedLocalPosition = chest.InverseTransformPoint(this._targetPosition);
				this._placedLocalRotation = Quaternion.Inverse(chest.rotation) * this._targetRotation;
				this._placedSurfaceLocalPoint = chest.InverseTransformPoint(this._surfacePoint);
				this._placedSurfaceLocalNormal = chest.InverseTransformDirection(this._surfaceNormal);
			}

			// Token: 0x0600A59A RID: 42394 RVA: 0x00452AB4 File Offset: 0x00450CB4
			public void UpdatePlacedTarget(Transform chest)
			{
				if (!this.IsPlaced || chest == null)
				{
					return;
				}
				this._targetPosition = chest.TransformPoint(this._placedLocalPosition);
				this._targetRotation = chest.rotation * this._placedLocalRotation;
				this._surfacePoint = chest.TransformPoint(this._placedSurfaceLocalPoint);
				this._surfaceNormal = chest.TransformDirection(this._placedSurfaceLocalNormal).normalized;
				this._hasTarget = true;
				this._surfaceUpdatedFrame = Time.frameCount;
			}

			// Token: 0x0600A59B RID: 42395 RVA: 0x00452B3C File Offset: 0x00450D3C
			public void MovePhysics(float positionSharpness, float rotationSharpness, float dt)
			{
				if (this._body == null || !this._hasTarget)
				{
					return;
				}
				if (this._hasContinuousPinchAttachment && this.IsGrabLatched)
				{
					return;
				}
				float num = GropingModeController.RuntimeHand.ExpBlend(positionSharpness, dt);
				float num2 = GropingModeController.RuntimeHand.ExpBlend(rotationSharpness, dt);
				this._body.MovePosition(Vector3.Lerp(this._body.position, this._targetPosition, num));
				this._body.MoveRotation(Quaternion.Slerp(this._body.rotation, this._targetRotation, num2));
			}

			// Token: 0x0600A59C RID: 42396 RVA: 0x00452BC8 File Offset: 0x00450DC8
			public void ApplyFingerPose(float proximal, float middle, float distal, float thumb, float pinchIndexProximal, float pinchIndexMiddle, float pinchIndexDistal, float pinchThumb, float pinchFoldedProximal, float pinchFoldedMiddle, float pinchFoldedDistal, float vaginalFoldedProximal, float vaginalFoldedMiddle, float vaginalFoldedDistal, float vaginalFoldedThumb, float pinchTargetDistance, float convergence)
			{
				foreach (GropingModeController.RuntimeHand.BonePose bonePose in this._bones)
				{
					if (!(bonePose.bone == null))
					{
						float num = this.GripAmount;
						float num2;
						if (this.PoseMode == GropingModeController.GrabPoseMode.NipplePinch)
						{
							if (bonePose.finger == GropingModeController.RuntimeHand.FingerKind.Index)
							{
								GropingModeController.RuntimeHand.CurlSegment curlSegment = bonePose.segment;
								if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Middle)
								{
									if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Distal)
									{
										num2 = pinchIndexProximal;
									}
									else
									{
										num2 = pinchIndexDistal;
									}
								}
								else
								{
									num2 = pinchIndexMiddle;
								}
							}
							else if (bonePose.finger == GropingModeController.RuntimeHand.FingerKind.Thumb)
							{
								num2 = pinchThumb;
							}
							else
							{
								GropingModeController.RuntimeHand.CurlSegment curlSegment = bonePose.segment;
								if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Middle)
								{
									if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Distal)
									{
										num2 = pinchFoldedProximal;
									}
									else
									{
										num2 = pinchFoldedDistal;
									}
								}
								else
								{
									num2 = pinchFoldedMiddle;
								}
							}
						}
						else if (this.PoseMode == GropingModeController.GrabPoseMode.VaginalFingers)
						{
							num = 1f;
							if (bonePose.finger == GropingModeController.RuntimeHand.FingerKind.Index || bonePose.finger == GropingModeController.RuntimeHand.FingerKind.Middle)
							{
								num2 = 0f;
							}
							else if (bonePose.finger == GropingModeController.RuntimeHand.FingerKind.Thumb)
							{
								num2 = vaginalFoldedThumb;
							}
							else
							{
								GropingModeController.RuntimeHand.CurlSegment curlSegment = bonePose.segment;
								if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Middle)
								{
									if (curlSegment != GropingModeController.RuntimeHand.CurlSegment.Distal)
									{
										num2 = vaginalFoldedProximal;
									}
									else
									{
										num2 = vaginalFoldedDistal;
									}
								}
								else
								{
									num2 = vaginalFoldedMiddle;
								}
							}
						}
						else
						{
							switch (bonePose.segment)
							{
							case GropingModeController.RuntimeHand.CurlSegment.Middle:
								num2 = middle;
								break;
							case GropingModeController.RuntimeHand.CurlSegment.Distal:
								num2 = distal;
								break;
							case GropingModeController.RuntimeHand.CurlSegment.Thumb:
								num2 = thumb;
								break;
							default:
								num2 = proximal;
								break;
							}
						}
						bonePose.bone.localRotation = bonePose.bindRotation * Quaternion.AngleAxis(num2 * num, Vector3.right);
					}
				}
				if (this.PoseMode == GropingModeController.GrabPoseMode.NipplePinch)
				{
					this.ApplyPinchConvergence(Mathf.Max(0.01f, pinchTargetDistance), Mathf.Clamp01(convergence) * this.GripAmount);
				}
			}

			// Token: 0x0600A59D RID: 42397 RVA: 0x00452D94 File Offset: 0x00450F94
			private void ApplyPinchConvergence(float targetDistance, float weight)
			{
				if (this._indexProximalBone == null || this._thumbProximalBone == null || this._indexTipTarget == null || this._thumbTipTarget == null || weight <= 0.0001f)
				{
					return;
				}
				for (int i = 0; i < 4; i++)
				{
					Vector3 position = this._indexTipTarget.position;
					Vector3 position2 = this._thumbTipTarget.position;
					if (Vector3.Distance(position, position2) <= targetDistance)
					{
						break;
					}
					Vector3 vector = Vector3.Lerp(position, position2, 0.5f);
					GropingModeController.RuntimeHand.RotateBoneTipToward(this._indexProximalBone, this._indexTipTarget, vector, 0.42f * weight);
					GropingModeController.RuntimeHand.RotateBoneTipToward(this._thumbProximalBone, this._thumbTipTarget, vector, 0.58f * weight);
				}
			}

			// Token: 0x0600A59E RID: 42398 RVA: 0x00452E50 File Offset: 0x00451050
			private static void RotateBoneTipToward(Transform bone, Transform tip, Vector3 target, float weight)
			{
				Vector3 vector = tip.position - bone.position;
				Vector3 vector2 = target - bone.position;
				if (vector.sqrMagnitude < 1E-06f || vector2.sqrMagnitude < 1E-06f)
				{
					return;
				}
				Quaternion quaternion = Quaternion.FromToRotation(vector, vector2);
				bone.rotation = Quaternion.Slerp(Quaternion.identity, quaternion, Mathf.Clamp01(weight)) * bone.rotation;
			}

			// Token: 0x0600A59F RID: 42399 RVA: 0x00452EC4 File Offset: 0x004510C4
			public void ApplyColliderMode()
			{
				bool flag = this.PoseMode == GropingModeController.GrabPoseMode.NipplePinch;
				bool flag2 = this.PoseMode == GropingModeController.GrabPoseMode.VaginalFingers;
				for (int i = 0; i < this._colliders.Count; i++)
				{
					ColliderComponent colliderComponent = this._colliders[i];
					if (!(colliderComponent == null))
					{
						GropingModeController.RuntimeHand.ColliderRole colliderRole = this._colliderRoles[i];
						bool flag3 = !this._calibrationPreview && ((!flag && !flag2) || (flag && (colliderRole == GropingModeController.RuntimeHand.ColliderRole.Index || colliderRole == GropingModeController.RuntimeHand.ColliderRole.Thumb)) || (flag2 && (colliderRole == GropingModeController.RuntimeHand.ColliderRole.Index || colliderRole == GropingModeController.RuntimeHand.ColliderRole.Middle)));
						if (colliderComponent.enabled != flag3)
						{
							colliderComponent.enabled = flag3;
						}
					}
				}
			}

			// Token: 0x0600A5A0 RID: 42400 RVA: 0x00452F68 File Offset: 0x00451168
			public void SyncCollisionVolumes(float surfaceBias)
			{
				Vector3 vector = (this.HasSurfaceContact ? (-this._surfaceNormal * Mathf.Max(0f, surfaceBias)) : Vector3.zero);
				for (int i = 0; i < this._colliderTransforms.Count; i++)
				{
					Transform transform = this._colliderTransforms[i];
					Transform transform2 = this._colliderTargets[i];
					if (!(transform == null) && !(transform2 == null))
					{
						transform.position = transform2.position + vector;
						transform.rotation = transform2.rotation;
					}
				}
			}

			// Token: 0x0600A5A1 RID: 42401 RVA: 0x00453000 File Offset: 0x00451200
			public void Dispose(BoobTouchModeController backend)
			{
				this.ReleaseGrab(backend);
				this.ReleaseVaginalInsertion();
				foreach (ColliderComponent colliderComponent in this._colliders)
				{
					if (backend != null && colliderComponent != null)
					{
						backend.UnregisterExternalCollider(colliderComponent);
					}
				}
				this._colliders.Clear();
				foreach (Material material in this._visualMaterials)
				{
					if (material != null)
					{
						Object.Destroy(material);
					}
				}
				this._visualMaterials.Clear();
				if (this._root != null)
				{
					Object.Destroy(this._root);
				}
			}

			// Token: 0x0600A5A2 RID: 42402 RVA: 0x004530EC File Offset: 0x004512EC
			private void ApplyVisualAppearance(GameObject visual, Color skinColor, float opacity)
			{
				Shader shader = Shader.Find("Hidden/lilToonTransparent");
				Color color = skinColor;
				color.a = Mathf.Clamp(opacity, 0.15f, 1f);
				foreach (Renderer renderer in visual.GetComponentsInChildren<Renderer>(true))
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					Material[] array = new Material[sharedMaterials.Length];
					for (int j = 0; j < sharedMaterials.Length; j++)
					{
						Material material = sharedMaterials[j];
						if (!(material == null))
						{
							Material material2 = new Material(material)
							{
								name = material.name + " (Groping Runtime)"
							};
							if (shader != null)
							{
								material2.shader = shader;
							}
							GropingModeController.RuntimeHand.SetTextureIfPresent(material2, "_MainTex", Texture2D.whiteTexture);
							GropingModeController.RuntimeHand.SetTextureIfPresent(material2, "_BaseMap", Texture2D.whiteTexture);
							GropingModeController.RuntimeHand.SetTextureIfPresent(material2, "_BaseColorMap", Texture2D.whiteTexture);
							GropingModeController.RuntimeHand.SetColorIfPresent(material2, "_Color", color);
							GropingModeController.RuntimeHand.SetColorIfPresent(material2, "_BaseColor", color);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_SrcBlend", 1f);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_DstBlend", 10f);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_SrcBlendAlpha", 1f);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_DstBlendAlpha", 10f);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_ZWrite", 0f);
							GropingModeController.RuntimeHand.SetFloatIfPresent(material2, "_AlphaToMask", 0f);
							material2.SetOverrideTag("RenderType", "Transparent");
							material2.renderQueue = 3000;
							array[j] = material2;
							this._visualMaterials.Add(material2);
						}
					}
					renderer.sharedMaterials = array;
					renderer.shadowCastingMode = ShadowCastingMode.Off;
					renderer.receiveShadows = false;
				}
			}

			// Token: 0x0600A5A3 RID: 42403 RVA: 0x004532AD File Offset: 0x004514AD
			private static void SetFloatIfPresent(Material material, string property, float value)
			{
				if (material.HasProperty(property))
				{
					material.SetFloat(property, value);
				}
			}

			// Token: 0x0600A5A4 RID: 42404 RVA: 0x00197469 File Offset: 0x00195669
			private static void SetColorIfPresent(Material material, string property, Color value)
			{
				if (material.HasProperty(property))
				{
					material.SetColor(property, value);
				}
			}

			// Token: 0x0600A5A5 RID: 42405 RVA: 0x001973A6 File Offset: 0x001955A6
			private static void SetTextureIfPresent(Material material, string property, Texture value)
			{
				if (material.HasProperty(property))
				{
					material.SetTexture(property, value);
				}
			}

			// Token: 0x0600A5A6 RID: 42406 RVA: 0x004532C0 File Offset: 0x004514C0
			private void NormalizeAndCenterVisual(Transform visual, float targetLength)
			{
				Renderer[] componentsInChildren = visual.GetComponentsInChildren<Renderer>(true);
				if (componentsInChildren.Length == 0)
				{
					return;
				}
				Bounds bounds = componentsInChildren[0].bounds;
				for (int i = 1; i < componentsInChildren.Length; i++)
				{
					bounds.Encapsulate(componentsInChildren[i].bounds);
				}
				float num = Mathf.Max(bounds.size.x, Mathf.Max(bounds.size.y, bounds.size.z));
				if (num > 0.0001f)
				{
					visual.localScale *= Mathf.Max(0.01f, targetLength) / num;
				}
				bounds = componentsInChildren[0].bounds;
				for (int j = 1; j < componentsInChildren.Length; j++)
				{
					bounds.Encapsulate(componentsInChildren[j].bounds);
				}
				visual.position -= bounds.center - this._root.transform.position;
			}

			// Token: 0x0600A5A7 RID: 42407 RVA: 0x004533B0 File Offset: 0x004515B0
			private void CacheFingerBones(Transform[] transforms)
			{
				string[] array = new string[] { "Index", "Middle", "Ring", "Pink" };
				GropingModeController.RuntimeHand.FingerKind[] array2 = new GropingModeController.RuntimeHand.FingerKind[]
				{
					GropingModeController.RuntimeHand.FingerKind.Index,
					GropingModeController.RuntimeHand.FingerKind.Middle,
					GropingModeController.RuntimeHand.FingerKind.Ring,
					GropingModeController.RuntimeHand.FingerKind.Pink
				};
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					GropingModeController.RuntimeHand.FingerKind fingerKind = array2[i];
					this.CacheBone(transforms, "Proximal" + text + "_Bone." + this.SideSuffix, GropingModeController.RuntimeHand.CurlSegment.Proximal, fingerKind);
					this.CacheBone(transforms, "Middle" + text + "_Bone." + this.SideSuffix, GropingModeController.RuntimeHand.CurlSegment.Middle, fingerKind);
					this.CacheBone(transforms, "Distal" + text + "_Bone." + this.SideSuffix, GropingModeController.RuntimeHand.CurlSegment.Distal, fingerKind);
				}
				this.CacheBone(transforms, "ProximalThumb_Bone." + this.SideSuffix, GropingModeController.RuntimeHand.CurlSegment.Thumb, GropingModeController.RuntimeHand.FingerKind.Thumb);
				this.CacheBone(transforms, "DistalThumb_Bone." + this.SideSuffix, GropingModeController.RuntimeHand.CurlSegment.Thumb, GropingModeController.RuntimeHand.FingerKind.Thumb);
			}

			// Token: 0x0600A5A8 RID: 42408 RVA: 0x004534A0 File Offset: 0x004516A0
			private void CacheBone(Transform[] transforms, string name, GropingModeController.RuntimeHand.CurlSegment segment, GropingModeController.RuntimeHand.FingerKind finger)
			{
				Transform transform = GropingModeController.RuntimeHand.FindBone(transforms, name);
				if (transform == null)
				{
					return;
				}
				this._bones.Add(new GropingModeController.RuntimeHand.BonePose
				{
					bone = transform,
					bindRotation = transform.localRotation,
					segment = segment,
					finger = finger
				});
			}

			// Token: 0x0600A5A9 RID: 42409 RVA: 0x004534F4 File Offset: 0x004516F4
			private Transform CreatePalmTarget()
			{
				Transform transform = new GameObject("PalmContactTarget").transform;
				transform.SetParent(this._root.transform, true);
				if (this._wristBone != null && this._middleBaseBone != null)
				{
					transform.position = Vector3.Lerp(this._wristBone.position, this._middleBaseBone.position, 0.58f);
					transform.rotation = this._root.transform.rotation;
				}
				else
				{
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
				}
				return transform;
			}

			// Token: 0x0600A5AA RID: 42410 RVA: 0x00453594 File Offset: 0x00451794
			private Transform CreateFingerCollider(Transform[] transforms, string finger, GropingModeController.RuntimeHand.ColliderRole role, float radius, BoobTouchModeController backend)
			{
				string text = ((finger == "Thumb") ? ("DistalThumb_Bone." + this.SideSuffix + "_end") : string.Concat(new string[] { "Distal", finger, "_Bone.", this.SideSuffix, "_end" }));
				Transform transform = GropingModeController.RuntimeHand.FindBone(transforms, text);
				if (transform == null)
				{
					text = ((finger == "Thumb") ? ("DistalThumb_Bone." + this.SideSuffix) : ("Distal" + finger + "_Bone." + this.SideSuffix));
					transform = GropingModeController.RuntimeHand.FindBone(transforms, text);
				}
				if (transform != null)
				{
					this.CreateCollider(finger, role, transform, radius, backend);
				}
				return transform;
			}

			// Token: 0x0600A5AB RID: 42411 RVA: 0x0045365C File Offset: 0x0045185C
			private void CreateCollider(string label, GropingModeController.RuntimeHand.ColliderRole role, Transform target, float radius, BoobTouchModeController backend)
			{
				GameObject gameObject = new GameObject("GropingCollider." + label);
				gameObject.transform.SetParent(this._root.transform, true);
				gameObject.transform.SetPositionAndRotation(target.position, target.rotation);
				MagicaSphereCollider magicaSphereCollider = gameObject.AddComponent<MagicaSphereCollider>();
				magicaSphereCollider.SetSize(new Vector3(Mathf.Max(0.005f, radius), 0f, 0f));
				this._colliders.Add(magicaSphereCollider);
				this._colliderTransforms.Add(gameObject.transform);
				this._colliderTargets.Add(target);
				this._colliderRoles.Add(role);
				backend.RegisterExternalCollider(magicaSphereCollider, false);
			}

			// Token: 0x0600A5AC RID: 42412 RVA: 0x00453710 File Offset: 0x00451910
			private Vector3 ContactWorldPoint(GropingModeController.GrabPoseMode mode)
			{
				if (mode == GropingModeController.GrabPoseMode.NipplePinch && this._indexTipTarget != null && this._thumbTipTarget != null)
				{
					return Vector3.Lerp(this._indexTipTarget.position, this._thumbTipTarget.position, 0.5f);
				}
				if (mode == GropingModeController.GrabPoseMode.VaginalFingers && this._indexTipTarget != null && this._middleTipTarget != null)
				{
					return Vector3.Lerp(this._indexTipTarget.position, this._middleTipTarget.position, 0.5f);
				}
				if (!(this._palmTarget != null))
				{
					return this._root.transform.position;
				}
				return this._palmTarget.position;
			}

			// Token: 0x0600A5AD RID: 42413 RVA: 0x004537CC File Offset: 0x004519CC
			private Vector3 GetVaginalFingerDirectionWorld()
			{
				if (this._indexTipTarget == null || this._middleTipTarget == null)
				{
					return Vector3.zero;
				}
				Vector3 vector;
				if (this._indexProximalBone != null && this._middleBaseBone != null)
				{
					vector = Vector3.Lerp(this._indexProximalBone.position, this._middleBaseBone.position, 0.5f);
				}
				else if (this._palmTarget != null)
				{
					vector = this._palmTarget.position;
				}
				else
				{
					vector = this._root.transform.position;
				}
				return this.VaginalFingerMidpoint - vector;
			}

			// Token: 0x0600A5AE RID: 42414 RVA: 0x00453874 File Offset: 0x00451A74
			private Vector3 GetVaginalFingerDirectionLocal()
			{
				Vector3 vaginalFingerDirectionWorld = this.GetVaginalFingerDirectionWorld();
				if (vaginalFingerDirectionWorld.sqrMagnitude <= 1E-06f)
				{
					return Vector3.zero;
				}
				return this._root.transform.InverseTransformDirection(vaginalFingerDirectionWorld).normalized;
			}

			// Token: 0x0600A5AF RID: 42415 RVA: 0x004538B5 File Offset: 0x00451AB5
			private Vector3 ContactLocalPoint(GropingModeController.GrabPoseMode mode)
			{
				return this._root.transform.InverseTransformPoint(this.ContactWorldPoint(mode));
			}

			// Token: 0x0600A5B0 RID: 42416 RVA: 0x004538CE File Offset: 0x00451ACE
			private Vector3 TargetContactWorld(GropingModeController.GrabPoseMode mode)
			{
				return this._targetPosition + this._targetRotation * this.ContactLocalPoint(mode);
			}

			// Token: 0x0600A5B1 RID: 42417 RVA: 0x004538F0 File Offset: 0x00451AF0
			private static Transform FindBone(Transform[] transforms, string exactName)
			{
				foreach (Transform transform in transforms)
				{
					if (transform != null && transform.name == exactName)
					{
						return transform;
					}
				}
				return null;
			}

			// Token: 0x0600A5B2 RID: 42418 RVA: 0x00442A83 File Offset: 0x00440C83
			private static float ExpBlend(float sharpness, float dt)
			{
				if (sharpness > 0f)
				{
					return 1f - Mathf.Exp(-sharpness * Mathf.Max(0f, dt));
				}
				return 1f;
			}

			// Token: 0x0400A9E6 RID: 43494
			private readonly GameObject _root;

			// Token: 0x0400A9E7 RID: 43495
			private readonly Rigidbody _body;

			// Token: 0x0400A9E8 RID: 43496
			private readonly bool _isLeft;

			// Token: 0x0400A9E9 RID: 43497
			private readonly List<GropingModeController.RuntimeHand.BonePose> _bones = new List<GropingModeController.RuntimeHand.BonePose>(14);

			// Token: 0x0400A9EA RID: 43498
			private readonly List<Material> _visualMaterials = new List<Material>(2);

			// Token: 0x0400A9EB RID: 43499
			private readonly List<ColliderComponent> _colliders = new List<ColliderComponent>(6);

			// Token: 0x0400A9EC RID: 43500
			private readonly List<Transform> _colliderTransforms = new List<Transform>(6);

			// Token: 0x0400A9ED RID: 43501
			private readonly List<Transform> _colliderTargets = new List<Transform>(6);

			// Token: 0x0400A9EE RID: 43502
			private readonly List<GropingModeController.RuntimeHand.ColliderRole> _colliderRoles = new List<GropingModeController.RuntimeHand.ColliderRole>(6);

			// Token: 0x0400A9EF RID: 43503
			private readonly Transform _wristBone;

			// Token: 0x0400A9F0 RID: 43504
			private readonly Transform _middleBaseBone;

			// Token: 0x0400A9F1 RID: 43505
			private readonly Transform _indexProximalBone;

			// Token: 0x0400A9F2 RID: 43506
			private readonly Transform _thumbProximalBone;

			// Token: 0x0400A9F3 RID: 43507
			private Transform _palmTarget;

			// Token: 0x0400A9F4 RID: 43508
			private Transform _indexTipTarget;

			// Token: 0x0400A9F5 RID: 43509
			private Transform _middleTipTarget;

			// Token: 0x0400A9F6 RID: 43510
			private Transform _thumbTipTarget;

			// Token: 0x0400A9F7 RID: 43511
			private Vector3 _targetPosition;

			// Token: 0x0400A9F8 RID: 43512
			private Quaternion _targetRotation;

			// Token: 0x0400A9F9 RID: 43513
			private Vector3 _surfacePoint;

			// Token: 0x0400A9FA RID: 43514
			private Vector3 _surfaceNormal;

			// Token: 0x0400A9FB RID: 43515
			private Vector3 _placedLocalPosition;

			// Token: 0x0400A9FC RID: 43516
			private Quaternion _placedLocalRotation;

			// Token: 0x0400A9FD RID: 43517
			private Vector3 _placedSurfaceLocalPoint;

			// Token: 0x0400A9FE RID: 43518
			private Vector3 _placedSurfaceLocalNormal;

			// Token: 0x0400A9FF RID: 43519
			private bool _hasTarget;

			// Token: 0x0400AA00 RID: 43520
			private Vector3 _stableSurfaceUp;

			// Token: 0x0400AA01 RID: 43521
			private bool _hasStableSurfaceFrame;

			// Token: 0x0400AA02 RID: 43522
			private Plane _grabPlane;

			// Token: 0x0400AA03 RID: 43523
			private Vector3 _grabPlaneCursorOffset;

			// Token: 0x0400AA04 RID: 43524
			private float _grabContactClearance;

			// Token: 0x0400AA05 RID: 43525
			private Vector3 _grabOriginContact;

			// Token: 0x0400AA06 RID: 43526
			private float _grabMovementLimit;

			// Token: 0x0400AA07 RID: 43527
			private bool _alignPinchCenter;

			// Token: 0x0400AA08 RID: 43528
			private bool _calibrationPreview;

			// Token: 0x0400AA09 RID: 43529
			private bool _hasContinuousPinchAttachment;

			// Token: 0x0400AA0A RID: 43530
			private Vector3 _pinchContactFromAnchorLocal;

			// Token: 0x0400AA0B RID: 43531
			private int _surfaceUpdatedFrame = -10;

			// Token: 0x0400AA0C RID: 43532
			private float _vaginalInsertionTarget;

			// Token: 0x0400AA0D RID: 43533
			private float _vaginalInsertionAmount;

			// Token: 0x020014EB RID: 5355
			private sealed class BonePose
			{
				// Token: 0x0400AA17 RID: 43543
				public Transform bone;

				// Token: 0x0400AA18 RID: 43544
				public Quaternion bindRotation;

				// Token: 0x0400AA19 RID: 43545
				public GropingModeController.RuntimeHand.CurlSegment segment;

				// Token: 0x0400AA1A RID: 43546
				public GropingModeController.RuntimeHand.FingerKind finger;
			}

			// Token: 0x020014EC RID: 5356
			private enum CurlSegment
			{
				// Token: 0x0400AA1C RID: 43548
				Proximal,
				// Token: 0x0400AA1D RID: 43549
				Middle,
				// Token: 0x0400AA1E RID: 43550
				Distal,
				// Token: 0x0400AA1F RID: 43551
				Thumb
			}

			// Token: 0x020014ED RID: 5357
			private enum FingerKind
			{
				// Token: 0x0400AA21 RID: 43553
				Index,
				// Token: 0x0400AA22 RID: 43554
				Middle,
				// Token: 0x0400AA23 RID: 43555
				Ring,
				// Token: 0x0400AA24 RID: 43556
				Pink,
				// Token: 0x0400AA25 RID: 43557
				Thumb
			}

			// Token: 0x020014EE RID: 5358
			private enum ColliderRole
			{
				// Token: 0x0400AA27 RID: 43559
				Palm,
				// Token: 0x0400AA28 RID: 43560
				Index,
				// Token: 0x0400AA29 RID: 43561
				Middle,
				// Token: 0x0400AA2A RID: 43562
				Ring,
				// Token: 0x0400AA2B RID: 43563
				Pink,
				// Token: 0x0400AA2C RID: 43564
				Thumb
			}
		}
	}
}
