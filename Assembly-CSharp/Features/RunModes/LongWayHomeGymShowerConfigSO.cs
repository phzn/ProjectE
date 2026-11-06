using System;
using Features.Inventory.Wearable;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CBA RID: 3258
	[CreateAssetMenu(fileName = "LongWayHomeGymShowerConfig", menuName = "Project Exhibitionist/Long Way Home/Gym Shower Config")]
	public sealed class LongWayHomeGymShowerConfigSO : ScriptableObject
	{
		// Token: 0x06005E8C RID: 24204 RVA: 0x00256401 File Offset: 0x00254601
		public static LongWayHomeGymShowerConfigSO Load()
		{
			return Resources.Load<LongWayHomeGymShowerConfigSO>("LongWayHomeGymShowerConfig");
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x00256410 File Offset: 0x00254610
		public bool HasRequiredAssets(out string missing)
		{
			missing = ((this.gymOutfit == null) ? "gymOutfit" : ((this.clothesPieces == null || this.clothesPieces.Length == 0 || this.clothesPieces[0] == null) ? "clothesPieces" : ((this.walkInIdle == null) ? "walkInIdle" : ((this.walkInWalk == null) ? "walkInWalk" : ((this.shower == null) ? "shower" : ((this.exitIdle == null) ? "exitIdle" : ((this.exitWalk == null) ? "exitWalk" : ((this.walkInFace == null) ? "walkInFace" : ((this.showerFace == null) ? "showerFace" : ((this.embarrassedClosedMouthFace == null) ? "embarrassedClosedMouthFace" : ((this.embarrassedFace == null) ? "embarrassedFace" : ((!LongWayHomeGymShowerConfigSO.HasRuntimeFace(this.walkInFaceAnimation, this.walkInFace)) ? "walkInFaceAnimation" : ((!LongWayHomeGymShowerConfigSO.HasRuntimeFace(this.showerFaceAnimation, this.showerFace)) ? "showerFaceAnimation" : ((!LongWayHomeGymShowerConfigSO.HasRuntimeFace(this.embarrassedClosedMouthFaceAnimation, this.embarrassedClosedMouthFace)) ? "embarrassedClosedMouthFaceAnimation" : ((!LongWayHomeGymShowerConfigSO.HasRuntimeFace(this.embarrassedFaceAnimation, this.embarrassedFace)) ? "embarrassedFaceAnimation" : ((this.npcIdle == null) ? "npcIdle" : ((this.npcWalk == null) ? "npcWalk" : ((this.npcRun == null) ? "npcRun" : ((this.npcPhoto == null) ? "npcPhoto" : ((this.npcPickUp == null) ? "npcPickUp" : ((this.walkInPath == null || this.walkInPath.Length < 2) ? "walkInPath" : ((this.exitPath == null || this.exitPath.Length < 2) ? "exitPath" : ((this.npcEntryPath == null || this.npcEntryPath.Length < 2) ? "npcEntryPath" : ((this.npcEscapePath == null || this.npcEscapePath.Length < 1) ? "npcEscapePath" : null))))))))))))))))))))))));
			return missing == null;
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x002566AC File Offset: 0x002548AC
		private static bool HasRuntimeFace(LongWayHomeFaceAnimationSO face, AnimationClip source)
		{
			if (face == null || face.sourceClip != source || face.duration <= 0f || face.curves == null || face.curves.Length == 0)
			{
				return false;
			}
			foreach (LongWayHomeFaceAnimationSO.CurveBinding curveBinding in face.curves)
			{
				if (curveBinding == null || string.IsNullOrEmpty(curveBinding.blendShapeName) || curveBinding.curve == null || curveBinding.curve.length == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005FA7 RID: 24487
		public const string ResourceName = "LongWayHomeGymShowerConfig";

		// Token: 0x04005FA8 RID: 24488
		[Header("Wardrobe")]
		[Tooltip("Complete gym outfit worn while walking to the shower (tank top, leggings, socks, shoes, accessories).")]
		public WearableItemSO gymOutfit;

		// Token: 0x04005FA9 RID: 24489
		[Tooltip("Floor items stacked on the bench while Ciel showers; the photographer takes them away.")]
		public GameObject[] clothesPieces;

		// Token: 0x04005FAA RID: 24490
		[Tooltip("Local offset of each piece from the pile position (same order as Clothes Pieces).")]
		public Vector3[] clothesPieceOffsets = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0.02f, 0.035f, -0.03f),
			new Vector3(-0.04f, 0.06f, 0.05f),
			new Vector3(0f, 0.05f, -0.48f)
		};

		// Token: 0x04005FAB RID: 24491
		[Header("Ciel animation")]
		public AnimationClip walkInIdle;

		// Token: 0x04005FAC RID: 24492
		public AnimationClip walkInWalk;

		// Token: 0x04005FAD RID: 24493
		public AnimationClip shower;

		// Token: 0x04005FAE RID: 24494
		public AnimationClip exitIdle;

		// Token: 0x04005FAF RID: 24495
		public AnimationClip exitWalk;

		// Token: 0x04005FB0 RID: 24496
		[Tooltip("Faces rebased on the player's renderer, without customization shapes (Tools > Long Way Home).")]
		public AnimationClip walkInFace;

		// Token: 0x04005FB1 RID: 24497
		public AnimationClip showerFace;

		// Token: 0x04005FB2 RID: 24498
		public AnimationClip embarrassedClosedMouthFace;

		// Token: 0x04005FB3 RID: 24499
		public AnimationClip embarrassedFace;

		// Token: 0x04005FB4 RID: 24500
		[Header("Runtime facial curves")]
		[Tooltip("Generated from the face clips above. Direct blend-shape curves also work in standalone players.")]
		public LongWayHomeFaceAnimationSO walkInFaceAnimation;

		// Token: 0x04005FB5 RID: 24501
		public LongWayHomeFaceAnimationSO showerFaceAnimation;

		// Token: 0x04005FB6 RID: 24502
		public LongWayHomeFaceAnimationSO embarrassedClosedMouthFaceAnimation;

		// Token: 0x04005FB7 RID: 24503
		public LongWayHomeFaceAnimationSO embarrassedFaceAnimation;

		// Token: 0x04005FB8 RID: 24504
		[Header("Photographer animation (Humanoid)")]
		public AnimationClip npcIdle;

		// Token: 0x04005FB9 RID: 24505
		public AnimationClip npcWalk;

		// Token: 0x04005FBA RID: 24506
		public AnimationClip npcRun;

		// Token: 0x04005FBB RID: 24507
		public AnimationClip npcPhoto;

		// Token: 0x04005FBC RID: 24508
		public AnimationClip npcPickUp;

		// Token: 0x04005FBD RID: 24509
		[Tooltip("Fallback shutter when the baked phone has no SmartphoneFlash setup.")]
		public AudioClip npcShutter;

		// Token: 0x04005FBE RID: 24510
		[Tooltip("Played on the PlayFootstep events of the photographer's walk clip.")]
		public AudioClip[] npcFootsteps;

		// Token: 0x04005FBF RID: 24511
		[Range(0f, 1f)]
		public float npcFootstepVolume = 0.3f;

		// Token: 0x04005FC0 RID: 24512
		[Header("Shower stall")]
		[Tooltip("Cubicle that contains the MallGymShowerStation used by the scene.")]
		public string stallName = "HMG_shower_divider_01 (7)";

		// Token: 0x04005FC1 RID: 24513
		public Vector3 stallFallbackPosition = new Vector3(-39.73f, 5.34f, -37.09f);

		// Token: 0x04005FC2 RID: 24514
		[Header("Ciel marks")]
		[Tooltip("Through the west entrance (open for x > -41.25 at z -31), west along the vestibule (the divider closes z -33.5 east of x -41.75), then south to the showers. Corners are rounded.")]
		public Vector3[] walkInPath = new Vector3[]
		{
			new Vector3(-39.3f, 5.34f, -31f),
			new Vector3(-39.4f, 5.34f, -32.1f),
			new Vector3(-42.7f, 5.34f, -32.4f),
			new Vector3(-42.7f, 5.34f, -35.2f),
			new Vector3(-42.5f, 5.34f, -35.8f)
		};

		// Token: 0x04005FC3 RID: 24515
		[Tooltip("Run from the shower stall back around the divider to the locker room doorway, where the player takes control.")]
		public Vector3[] exitPath = new Vector3[]
		{
			new Vector3(-42.75f, 5.34f, -33.65f),
			new Vector3(-42.6f, 5.34f, -32.4f),
			new Vector3(-39.6f, 5.34f, -32f),
			new Vector3(-39.35f, 5.34f, -31f)
		};

		// Token: 0x04005FC4 RID: 24516
		[Min(0.1f)]
		public float walkInMetersPerSecond = 1.3f;

		// Token: 0x04005FC5 RID: 24517
		[Range(0.5f, 2f)]
		public float walkInPlaybackSpeed = 1f;

		// Token: 0x04005FC6 RID: 24518
		[Min(0.1f)]
		public float exitMetersPerSecond = 3.2f;

		// Token: 0x04005FC7 RID: 24519
		[Range(0.5f, 2f)]
		public float exitPlaybackSpeed = 1f;

		// Token: 0x04005FC8 RID: 24520
		[Tooltip("Floor height inside the stall: the shower pose is kept with its feet on it.")]
		public float showerFloorY = 5.34f;

		// Token: 0x04005FC9 RID: 24521
		[Header("Photographer marks")]
		public Vector3[] npcEntryPath = new Vector3[]
		{
			new Vector3(-39.2f, 5.34f, -27.8f),
			new Vector3(-39.35f, 5.34f, -32.1f),
			new Vector3(-42.7f, 5.34f, -32.4f),
			new Vector3(-42.7f, 5.34f, -35.3f),
			new Vector3(-41.95f, 5.34f, -36.6f)
		};

		// Token: 0x04005FCA RID: 24522
		public Vector3 npcPickUpMark = new Vector3(-44.25f, 5.34f, -36.85f);

		// Token: 0x04005FCB RID: 24523
		[Tooltip("Ciel stops showering and turns towards the clothes when the photographer is this close to them.")]
		[Min(0f)]
		public float cielReactsAtNpcDistance = 1f;

		// Token: 0x04005FCC RID: 24524
		[Tooltip("Seconds at the bench before the clothes vanish (no grab animation).")]
		[Min(0f)]
		public float npcGrabSeconds = 0.2f;

		// Token: 0x04005FCD RID: 24525
		public Vector3[] npcEscapePath = new Vector3[]
		{
			new Vector3(-43f, 5.34f, -34.5f),
			new Vector3(-42.7f, 5.34f, -32.4f),
			new Vector3(-39.4f, 5.34f, -32.1f),
			new Vector3(-39.25f, 5.34f, -28f),
			new Vector3(-39.2f, 5.34f, -26.4f)
		};

		// Token: 0x04005FCE RID: 24526
		public Vector3 clothesPilePosition = new Vector3(-45.13f, 5.93f, -36.95f);

		// Token: 0x04005FCF RID: 24527
		public float clothesPileYaw = 10f;

		// Token: 0x04005FD0 RID: 24528
		[Min(0.1f)]
		public float npcWalkMetersPerSecond = 1.2f;

		// Token: 0x04005FD1 RID: 24529
		[Tooltip("Brisk walk from the stall to the clothes after the dialogue.")]
		[Min(0.1f)]
		public float npcToBenchMetersPerSecond = 2f;

		// Token: 0x04005FD2 RID: 24530
		[Min(0.1f)]
		public float npcRunMetersPerSecond = 3.6f;

		// Token: 0x04005FD3 RID: 24531
		[Header("Cameras")]
		[Tooltip("Fixed camera in the vestibule that pans with Ciel: she walks in towards it, then on to the showers.")]
		public Vector3 walkInCameraPosition = new Vector3(-43.9f, 6.95f, -32.3f);

		// Token: 0x04005FD4 RID: 24532
		[Tooltip("Fixed camera near the entrance that pans with Ciel as she runs towards it and stops in the doorway.")]
		public Vector3 exitCameraPosition = new Vector3(-38.3f, 6.9f, -32.55f);

		// Token: 0x04005FD5 RID: 24533
		[Min(0.1f)]
		public float followCameraLookHeight = 1.35f;

		// Token: 0x04005FD6 RID: 24534
		[Tooltip("Offset from the head in the stall frame (Z = towards the water Ciel faces, X = side, Y = up).")]
		public Vector3 showerCameraLocalOffset = new Vector3(0.4f, 0.32f, 0.45f);

		// Token: 0x04005FD7 RID: 24535
		[Tooltip("Maximum distance in front of the standing point, keeping the camera inside the stall.")]
		public float showerCameraMaxForward = 0.6f;

		// Token: 0x04005FD8 RID: 24536
		[Tooltip("In front of the lockers: bench, photographer and the open stall behind them.")]
		public Vector3 theftCameraPosition = new Vector3(-47.4f, 6.95f, -38.8f);

		// Token: 0x04005FD9 RID: 24537
		[Range(20f, 80f)]
		public float trackingFieldOfView = 42f;

		// Token: 0x04005FDA RID: 24538
		[Range(20f, 80f)]
		public float followFieldOfView = 48f;

		// Token: 0x04005FDB RID: 24539
		[Range(20f, 80f)]
		public float showerFieldOfView = 50f;

		// Token: 0x04005FDC RID: 24540
		[Header("Timing")]
		[Min(0f)]
		public float bubblesAfterSeconds = 1.5f;

		// Token: 0x04005FDD RID: 24541
		[Min(0f)]
		public float photographerAfterSeconds = 4f;

		// Token: 0x04005FDE RID: 24542
		[Min(0f)]
		public float dialogueAfterFirstPhotoSeconds = 1.2f;

		// Token: 0x04005FDF RID: 24543
		[Min(0f)]
		public float faceBlendSeconds = 0.25f;

		// Token: 0x04005FE0 RID: 24544
		[Min(0.1f)]
		public float transitionSeconds = 0.7f;

		// Token: 0x04005FE1 RID: 24545
		[Min(0.1f)]
		public float handoffFadeSeconds = 0.45f;
	}
}
