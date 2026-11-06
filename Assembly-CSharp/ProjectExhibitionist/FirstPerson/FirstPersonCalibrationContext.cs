using System;
using System.Collections.Generic;
using System.Globalization;
using Features.Detection;
using Features.Inventory;
using Features.PlayerEmotions;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using RayznGames.VehicleSystem;
using UnityEngine;
using VehicleSpawnPlugin;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009E8 RID: 2536
	[Serializable]
	public sealed class FirstPersonCalibrationContext
	{
		// Token: 0x060047C3 RID: 18371 RVA: 0x001B3BE0 File Offset: 0x001B1DE0
		public static FirstPersonCalibrationContext CaptureContext(FirstPersonController controller)
		{
			FirstPersonCalibrationContext firstPersonCalibrationContext = new FirstPersonCalibrationContext();
			if (controller == null)
			{
				return firstPersonCalibrationContext;
			}
			firstPersonCalibrationContext.firstPersonActive = controller.IsFirstPerson;
			firstPersonCalibrationContext.hideSpotCamera = controller.YawClampOverrideCenter != null;
			firstPersonCalibrationContext.externalCameraAnchor = ((controller.CameraAnchorOverride != null) ? controller.CameraAnchorOverride.name : string.Empty);
			firstPersonCalibrationContext.activePlayerResources = PlayerActivityGate.ActiveResources.ToString();
			firstPersonCalibrationContext.blockedPlayerActivities = PlayerActivityGate.BlockedMask.ToString();
			firstPersonCalibrationContext.stance = ((EmotionManager.Instance != null) ? EmotionManager.Instance.CurrentStance.ToString() : "Unknown");
			InventoryItemSO carriedCameraItem = controller.CarriedCameraItem;
			firstPersonCalibrationContext.carriedItemCamera = carriedCameraItem != null;
			firstPersonCalibrationContext.carriedItemName = ((carriedCameraItem != null) ? carriedCameraItem.name : string.Empty);
			firstPersonCalibrationContext.cameraPreset = ((carriedCameraItem != null) ? "CarriedItem" : ("Context / " + firstPersonCalibrationContext.stance));
			vThirdPersonController component = controller.GetComponent<vThirdPersonController>();
			if (component != null)
			{
				firstPersonCalibrationContext.movementLocked = component.lockMovement;
				firstPersonCalibrationContext.crouching = component.isCrouching;
				firstPersonCalibrationContext.sprinting = component.isSprinting;
				firstPersonCalibrationContext.grounded = component.isGrounded;
				firstPersonCalibrationContext.ragdolled = component.ragdolled;
				FirstPersonCalibrationContext.CaptureVehicle(component, firstPersonCalibrationContext);
			}
			CoverPoseController component2 = controller.GetComponent<CoverPoseController>();
			if (component2 != null)
			{
				firstPersonCalibrationContext.covering = component2.IsCovering;
				firstPersonCalibrationContext.coverPose = component2.CurrentCoverType.ToString() + ": " + component2.GetCurrentEmotionPoseLabel();
			}
			Animator component3 = controller.GetComponent<Animator>();
			if (component3 != null && component3.isActiveAndEnabled && component3.runtimeAnimatorController != null)
			{
				FirstPersonCalibrationContext.CaptureAnimator(component3, firstPersonCalibrationContext);
			}
			firstPersonCalibrationContext.Sanitize();
			return firstPersonCalibrationContext;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x001B3DD8 File Offset: 0x001B1FD8
		private static void CaptureVehicle(vThirdPersonController rider, FirstPersonCalibrationContext result)
		{
			foreach (vVehicleController vVehicleController in Object.FindObjectsByType<vVehicleController>(FindObjectsSortMode.None))
			{
				if (!(vVehicleController.CurrentRider != rider))
				{
					result.vehicleType = vVehicleController.Type.ToString();
					result.vehicleName = vVehicleController.name;
					result.vehicleTransitioning = vVehicleController.IsTransitioning;
					FirstPersonCalibrationContext.CaptureVehicleMotion(vVehicleController.Vehicle, vVehicleController.GetComponent<VehiclePoseDriver>(), result);
					return;
				}
			}
			foreach (vBikeController vBikeController in Object.FindObjectsByType<vBikeController>(FindObjectsSortMode.None))
			{
				if (!(vBikeController.CurrentRider != rider))
				{
					result.vehicleType = "LegacyBicycle";
					result.vehicleName = vBikeController.name;
					FirstPersonCalibrationContext.CaptureVehicleMotion(vBikeController.Bicycle, vBikeController.GetComponent<VehiclePoseDriver>(), result);
					return;
				}
			}
			foreach (SkateboardInteractable skateboardInteractable in Object.FindObjectsByType<SkateboardInteractable>(FindObjectsSortMode.None))
			{
				if (!(skateboardInteractable.CurrentRider != rider))
				{
					result.vehicleType = "Skateboard";
					result.vehicleName = skateboardInteractable.name;
					result.vehicleSpeed = skateboardInteractable.CurrentSpeed;
					result.skateboardTrick = skateboardInteractable.TrickActive;
					result.skateboardBalance = skateboardInteractable.InBalance;
					result.poseState = (skateboardInteractable.TrickActive ? "Trick" : (skateboardInteractable.InBalance ? "Balance" : "AnimatorDriven"));
					return;
				}
			}
			foreach (SimpleRollerSkatesInteractable simpleRollerSkatesInteractable in Object.FindObjectsByType<SimpleRollerSkatesInteractable>(FindObjectsSortMode.None))
			{
				if (!(simpleRollerSkatesInteractable.CurrentRider != rider))
				{
					result.vehicleType = "SimpleRollerSkates";
					result.vehicleName = simpleRollerSkatesInteractable.name;
					result.vehicleSpeed = simpleRollerSkatesInteractable.CurrentSpeed;
					result.poseState = "AnimatorDriven";
					return;
				}
			}
			if (!CarRoofCarrier.IsPlayerMounted)
			{
				return;
			}
			foreach (CarRoofCarrier carRoofCarrier in Object.FindObjectsByType<CarRoofCarrier>(FindObjectsSortMode.None))
			{
				if (carRoofCarrier.HasRider)
				{
					VehicleAI componentInParent = carRoofCarrier.GetComponentInParent<VehicleAI>();
					if (!(componentInParent == null) && !(componentInParent.PassengerRoot == null) && (!(componentInParent.PassengerRoot != rider.transform) || rider.transform.IsChildOf(componentInParent.PassengerRoot)))
					{
						result.vehicleType = "TrafficCarRoof";
						result.vehicleName = componentInParent.name;
						result.vehicleSpeed = componentInParent.CurrentSpeed;
						result.poseState = "RoofRiding";
						result.rooftopRiding = true;
						result.trafficBlocked = componentInParent.IsBlocked;
						return;
					}
				}
			}
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x001B406C File Offset: 0x001B226C
		private static void CaptureVehicleMotion(IVehicle vehicle, VehiclePoseDriver driver, FirstPersonCalibrationContext result)
		{
			if (vehicle != null)
			{
				result.vehicleSpeed = vehicle.currentSpeed;
				result.vehicleInputAvailable = true;
				result.vehicleThrottle = vehicle.verticalInput;
				result.vehicleSteering = vehicle.horizontalInput;
				result.vehicleBraking = vehicle.braking;
			}
			if (driver != null && driver.IsActive)
			{
				result.poseState = driver.CurrentState.ToString();
				result.targetPoseState = driver.TargetState.ToString();
				result.poseBlendProgress = driver.BlendProgress;
			}
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x001B4104 File Offset: 0x001B2304
		private static void CaptureAnimator(Animator animator, FirstPersonCalibrationContext result)
		{
			result.animatorStates = new string[animator.layerCount];
			for (int i = 0; i < animator.layerCount; i++)
			{
				AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(i);
				string text = string.Format(CultureInfo.InvariantCulture, "{0}: hash={1}; time={2:R}; weight={3:R}", new object[]
				{
					animator.GetLayerName(i),
					currentAnimatorStateInfo.fullPathHash,
					FirstPersonCalibrationValues.Finite(currentAnimatorStateInfo.normalizedTime, 0f),
					FirstPersonCalibrationValues.Finite(animator.GetLayerWeight(i), 0f)
				});
				if (animator.IsInTransition(i))
				{
					AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(i);
					text += string.Format(CultureInfo.InvariantCulture, "; nextHash={0}; nextTime={1:R}", nextAnimatorStateInfo.fullPathHash, FirstPersonCalibrationValues.Finite(nextAnimatorStateInfo.normalizedTime, 0f));
				}
				result.animatorStates[i] = text;
			}
			AnimatorControllerParameter[] parameters = animator.parameters;
			List<string> list = new List<string>(parameters.Length);
			foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
			{
				switch (animatorControllerParameter.type)
				{
				case AnimatorControllerParameterType.Float:
					list.Add(animatorControllerParameter.name + "=" + FirstPersonCalibrationValues.Finite(animator.GetFloat(animatorControllerParameter.nameHash), 0f).ToString("R", CultureInfo.InvariantCulture));
					break;
				case AnimatorControllerParameterType.Int:
					list.Add(animatorControllerParameter.name + "=" + animator.GetInteger(animatorControllerParameter.nameHash).ToString(CultureInfo.InvariantCulture));
					break;
				case AnimatorControllerParameterType.Bool:
					list.Add(animatorControllerParameter.name + "=" + animator.GetBool(animatorControllerParameter.nameHash).ToString());
					break;
				}
			}
			result.animatorParameters = list.ToArray();
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x001B4300 File Offset: 0x001B2500
		public void Sanitize()
		{
			this.vehicleSpeed = FirstPersonCalibrationValues.Finite(this.vehicleSpeed, 0f);
			this.vehicleThrottle = Mathf.Clamp(FirstPersonCalibrationValues.Finite(this.vehicleThrottle, 0f), -1f, 1f);
			this.vehicleSteering = Mathf.Clamp(FirstPersonCalibrationValues.Finite(this.vehicleSteering, 0f), -1f, 1f);
			this.poseBlendProgress = Mathf.Clamp01(FirstPersonCalibrationValues.Finite(this.poseBlendProgress, 1f));
			if (this.animatorStates == null)
			{
				this.animatorStates = Array.Empty<string>();
			}
			if (this.animatorParameters == null)
			{
				this.animatorParameters = Array.Empty<string>();
			}
		}

		// Token: 0x0400477E RID: 18302
		public string vehicleType = "None";

		// Token: 0x0400477F RID: 18303
		public string vehicleName;

		// Token: 0x04004780 RID: 18304
		public string poseState;

		// Token: 0x04004781 RID: 18305
		public float vehicleSpeed;

		// Token: 0x04004782 RID: 18306
		public bool movementLocked;

		// Token: 0x04004783 RID: 18307
		public string stance;

		// Token: 0x04004784 RID: 18308
		public bool carriedItemCamera;

		// Token: 0x04004785 RID: 18309
		public string carriedItemName;

		// Token: 0x04004786 RID: 18310
		public string cameraPreset;

		// Token: 0x04004787 RID: 18311
		public string[] animatorStates = Array.Empty<string>();

		// Token: 0x04004788 RID: 18312
		public string[] animatorParameters = Array.Empty<string>();

		// Token: 0x04004789 RID: 18313
		public bool firstPersonActive;

		// Token: 0x0400478A RID: 18314
		public bool hideSpotCamera;

		// Token: 0x0400478B RID: 18315
		public string externalCameraAnchor;

		// Token: 0x0400478C RID: 18316
		public string activePlayerResources;

		// Token: 0x0400478D RID: 18317
		public string blockedPlayerActivities;

		// Token: 0x0400478E RID: 18318
		public bool crouching;

		// Token: 0x0400478F RID: 18319
		public bool sprinting;

		// Token: 0x04004790 RID: 18320
		public bool grounded;

		// Token: 0x04004791 RID: 18321
		public bool ragdolled;

		// Token: 0x04004792 RID: 18322
		public bool covering;

		// Token: 0x04004793 RID: 18323
		public string coverPose;

		// Token: 0x04004794 RID: 18324
		public string targetPoseState;

		// Token: 0x04004795 RID: 18325
		public float poseBlendProgress = 1f;

		// Token: 0x04004796 RID: 18326
		public bool vehicleTransitioning;

		// Token: 0x04004797 RID: 18327
		public bool vehicleInputAvailable;

		// Token: 0x04004798 RID: 18328
		public float vehicleThrottle;

		// Token: 0x04004799 RID: 18329
		public float vehicleSteering;

		// Token: 0x0400479A RID: 18330
		public bool vehicleBraking;

		// Token: 0x0400479B RID: 18331
		public bool skateboardTrick;

		// Token: 0x0400479C RID: 18332
		public bool skateboardBalance;

		// Token: 0x0400479D RID: 18333
		public bool rooftopRiding;

		// Token: 0x0400479E RID: 18334
		public bool trafficBlocked;
	}
}
