using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.CielVoice;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Loading;
using Features.MainMenu;
using Features.MasturbationSystem;
using Features.NPC;
using Features.WardrobeFeedback;
using Invector;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AzureSky;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace Features.RunModes
{
	// Token: 0x02000D28 RID: 3368
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1050)]
	public sealed class LongWayHomeUniversitySequence : MonoBehaviour, ILongWayHomePoiProloguePresenter
	{
		// Token: 0x0600624F RID: 25167 RVA: 0x00273634 File Offset: 0x00271834
		private void BuildCielGraph()
		{
			if (this._cielGraph.IsValid())
			{
				this._cielGraph.Destroy();
			}
			this._cielGraph = PlayableGraph.Create("Long Way Home - University Ciel");
			this._cielGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			this._bodyMixer = AnimationMixerPlayable.Create(this._cielGraph, 4);
			this._floorA = this.CreateFloorChannel();
			this._floorB = this.CreateFloorChannel();
			this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._floorA, 0, this._bodyMixer, 2);
			this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._floorB, 0, this._bodyMixer, 3);
			this._locoIdleClip = (this._locoMoveClip = null);
			this._poseFix = AnimationScriptPlayable.Create<LongWayHomeFloorPoseFix>(this._cielGraph, this.CreatePoseFix(), 1);
			this._cielGraph.Connect<AnimationMixerPlayable, AnimationScriptPlayable>(this._bodyMixer, 0, this._poseFix, 0);
			this._poseFix.SetInputWeight(0, 1f);
			this._layers = AnimationLayerMixerPlayable.Create(this._cielGraph, 3);
			this._cielGraph.Connect<AnimationScriptPlayable, AnimationLayerMixerPlayable>(this._poseFix, 0, this._layers, 0);
			this._layers.SetInputWeight(0, 1f);
			this._armsMixer = AnimationMixerPlayable.Create(this._cielGraph, 0);
			this._cielGraph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this._armsMixer, 0, this._layers, 1);
			if (this._config.stimulationMask != null)
			{
				this._layers.SetLayerMaskFromAvatarMask(1U, this._config.stimulationMask);
			}
			this._layers.SetInputWeight(1, 0f);
			this._leftArmMixer = AnimationMixerPlayable.Create(this._cielGraph, 0);
			this._cielGraph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this._leftArmMixer, 0, this._layers, 2);
			if (this._config.leftArmMask != null)
			{
				this._layers.SetLayerMaskFromAvatarMask(2U, this._config.leftArmMask);
			}
			this._layers.SetInputWeight(2, 0f);
			AnimationPlayableOutput.Create(this._cielGraph, "Ciel - University", this._animator).SetSourcePlayable(this._layers);
			this._cielGraph.Play();
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x00273860 File Offset: 0x00271A60
		private AnimationClipPlayable CreateFloorChannel()
		{
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(this._cielGraph, this._config.floorRoutine);
			animationClipPlayable.SetApplyFootIK(false);
			animationClipPlayable.SetSpeed(0.0);
			return animationClipPlayable;
		}

		// Token: 0x06006251 RID: 25169 RVA: 0x0027389C File Offset: 0x00271A9C
		private AnimationClipPlayable CreateArmsClip(AnimationClip clip, AnimationMixerPlayable mixer, out int input)
		{
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(this._cielGraph, clip);
			animationClipPlayable.SetApplyFootIK(false);
			input = mixer.AddInput(animationClipPlayable, 0, 0f);
			return animationClipPlayable;
		}

		// Token: 0x06006252 RID: 25170 RVA: 0x002738D0 File Offset: 0x00271AD0
		private void UseLocomotion(AnimationClip idle, AnimationClip move, float moveSpeed)
		{
			if (!this._cielGraph.IsValid())
			{
				return;
			}
			if (idle != this._locoIdleClip)
			{
				if (this._locoIdle.IsValid<AnimationClipPlayable>())
				{
					this._cielGraph.Disconnect<AnimationMixerPlayable>(this._bodyMixer, 0);
					this._locoIdle.Destroy<AnimationClipPlayable>();
				}
				this._locoIdle = AnimationClipPlayable.Create(this._cielGraph, idle);
				this._locoIdle.SetApplyFootIK(false);
				this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._locoIdle, 0, this._bodyMixer, 0);
				this._locoIdleClip = idle;
			}
			if (move != this._locoMoveClip)
			{
				if (this._locoMove.IsValid<AnimationClipPlayable>())
				{
					this._cielGraph.Disconnect<AnimationMixerPlayable>(this._bodyMixer, 1);
					this._locoMove.Destroy<AnimationClipPlayable>();
				}
				this._locoMove = AnimationClipPlayable.Create(this._cielGraph, (move != null) ? move : idle);
				this._locoMove.SetApplyFootIK(false);
				this._cielGraph.Connect<AnimationClipPlayable, AnimationMixerPlayable>(this._locoMove, 0, this._bodyMixer, 1);
				this._locoMoveClip = move;
			}
			this._locoMove.SetSpeed((double)moveSpeed);
			this.ApplyBodyWeights();
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x002739F8 File Offset: 0x00271BF8
		private void ApplyBodyWeights()
		{
			if (!this._bodyMixer.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			float num = Mathf.Clamp01(this._floorWeight);
			float num2 = Mathf.Clamp01(this._walkWeight);
			float num3 = ((this._routine != null) ? this._routine.WeightB : 0f);
			this._bodyMixer.SetInputWeight(0, (1f - num) * (1f - num2));
			this._bodyMixer.SetInputWeight(1, (1f - num) * num2);
			this._bodyMixer.SetInputWeight(2, num * (1f - num3));
			this._bodyMixer.SetInputWeight(3, num * num3);
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x00273A9C File Offset: 0x00271C9C
		private void ApplyRoutine()
		{
			if (this._routine == null || !this._floorA.IsValid<AnimationClipPlayable>())
			{
				return;
			}
			this._floorA.SetTime(this._routine.TimeA);
			this._floorB.SetTime(this._routine.TimeB);
			this.ApplyBodyWeights();
			this.ApplyFloorFit();
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x00273AF8 File Offset: 0x00271CF8
		private LongWayHomeFloorPoseFix CreatePoseFix()
		{
			LongWayHomeFloorPoseFix longWayHomeFloorPoseFix = default(LongWayHomeFloorPoseFix);
			this._poseFixCount = 0;
			LongWayHomeUniversityConfigSO.MuscleOffset[] floorPoseFixes = this._config.floorPoseFixes;
			int num = ((floorPoseFixes != null) ? Mathf.Min(floorPoseFixes.Length, 8) : 0);
			this._poseFixA = new float[num];
			this._poseFixB = new float[num];
			for (int i = 0; i < num; i++)
			{
				MuscleHandle muscleHandle;
				if (floorPoseFixes[i] == null || !LongWayHomeFloorPoseFix.TryFindMuscle(floorPoseFixes[i].muscle, out muscleHandle))
				{
					string text = "[LongWayHomeUniversity] Unknown floor pose muscle '";
					LongWayHomeUniversityConfigSO.MuscleOffset muscleOffset = floorPoseFixes[i];
					Debug.LogWarning(text + ((muscleOffset != null) ? muscleOffset.muscle : null) + "'.", this);
				}
				else
				{
					longWayHomeFloorPoseFix.SetMuscle(i, muscleHandle);
					this._poseFixCount = i + 1;
				}
			}
			if (floorPoseFixes != null && floorPoseFixes.Length > 8)
			{
				Debug.LogWarning("[LongWayHomeUniversity] Only the first " + 8.ToString() + " floor pose fixes are applied.", this);
			}
			longWayHomeFloorPoseFix.Count = this._poseFixCount;
			return longWayHomeFloorPoseFix;
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x00273BDC File Offset: 0x00271DDC
		private void ApplyFloorFit()
		{
			if (this._routine == null)
			{
				return;
			}
			float num = Mathf.Clamp01(this._floorWeight);
			float weightB = this._routine.WeightB;
			float num2 = (float)this._routine.TimeA;
			float num3 = (float)this._routine.TimeB;
			if (this._poseFix.IsValid<AnimationScriptPlayable>())
			{
				LongWayHomeFloorPoseFix jobData = this._poseFix.GetJobData<LongWayHomeFloorPoseFix>();
				jobData.BodyPitch = num * Mathf.Lerp(this._config.EvaluateFloorBodyPitch(num2), this._config.EvaluateFloorBodyPitch(num3), weightB);
				if (this._poseFixCount > 0)
				{
					this._config.EvaluateFloorPoseFix(num2, this._poseFixA);
					this._config.EvaluateFloorPoseFix(num3, this._poseFixB);
					for (int i = 0; i < this._poseFixCount; i++)
					{
						jobData.SetOffset(i, num * Mathf.Lerp(this._poseFixA[i], this._poseFixB[i], weightB));
					}
				}
				this._poseFix.SetJobData<LongWayHomeFloorPoseFix>(jobData);
			}
			this._floorHeight = num * Mathf.Lerp(this._config.EvaluateFloorHeight(num2), this._config.EvaluateFloorHeight(num3), weightB);
			this._floorFitApplied = num > 0f;
			if (this.OnHallFloor)
			{
				Vector3 position = this._player.position;
				position.y = this._floorBaseY + this._floorHeight;
				this._player.position = position;
				this._body.position = position;
			}
		}

		// Token: 0x06006257 RID: 25175 RVA: 0x00273D54 File Offset: 0x00271F54
		private void ClearFloorFit()
		{
			this._floorFitApplied = false;
			this._floorHeight = 0f;
			if (!this._poseFix.IsValid<AnimationScriptPlayable>())
			{
				return;
			}
			LongWayHomeFloorPoseFix jobData = this._poseFix.GetJobData<LongWayHomeFloorPoseFix>();
			jobData.BodyPitch = 0f;
			for (int i = 0; i < 8; i++)
			{
				jobData.SetOffset(i, 0f);
			}
			this._poseFix.SetJobData<LongWayHomeFloorPoseFix>(jobData);
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06006258 RID: 25176 RVA: 0x00273DBE File Offset: 0x00271FBE
		private bool OnHallFloor
		{
			get
			{
				return this.Phase == LongWayHomeUniversitySequence.StoryPhase.Fall || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Floor || this.Phase == LongWayHomeUniversitySequence.StoryPhase.FallingAsleep || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Morning || this.Phase == LongWayHomeUniversitySequence.StoryPhase.GetUp || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Realize;
			}
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x00273DF8 File Offset: 0x00271FF8
		private IEnumerator CorridorLoop()
		{
			Vector3[] corridorPath = this._config.corridorPath;
			Vector3 from = corridorPath[0];
			Vector3 to = corridorPath[corridorPath.Length - 1];
			while (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Corridor)
			{
				Vector3[] array = LongWayHomeCinematicPath.BackAndForthLeg(from, to, this._config.corridorTurnRadius, 8);
				yield return this.MoveAlong(array, this._config.corridorMetersPerSecond, true, false, 0f);
				Vector3 vector = from;
				from = to;
				to = vector;
			}
			yield break;
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x00273E08 File Offset: 0x00272008
		private Vector3[] CorridorStart()
		{
			Vector3[] corridorPath = this._config.corridorPath;
			return LongWayHomeCinematicPath.BackAndForthLeg(corridorPath[0], corridorPath[corridorPath.Length - 1], this._config.corridorTurnRadius, 8);
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x00273E44 File Offset: 0x00272044
		private IEnumerator RunAlong(Vector3[] waypoints, bool fromStanding)
		{
			float num = this._config.escapeMetersPerSecond * this._config.escapePlaybackSpeed;
			bool flag = !fromStanding;
			yield return this.MoveAlong(waypoints, num, !fromStanding, flag, fromStanding ? 0.35f : 0f);
			yield break;
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x00273E61 File Offset: 0x00272061
		private IEnumerator MoveAlong(Vector3[] waypoints, float metersPerSecond, bool alreadyMoving, bool stopAtEnd, float floorBlendSeconds)
		{
			Vector3[] path = LongWayHomeCinematicPath.RoundCorners(waypoints, 4);
			float[] along = LongWayHomeCinematicPath.CumulativeLengths(path);
			float total = along[along.Length - 1];
			float travelled = 0f;
			float speed = (alreadyMoving ? metersPerSecond : 0f);
			float speedVelocity = 0f;
			float yaw = this._player.eulerAngles.y;
			float yawVelocity = 0f;
			float clipSpeed = (this._locoMove.IsValid<AnimationClipPlayable>() ? ((float)this._locoMove.GetSpeed<AnimationClipPlayable>()) : 1f);
			if (!alreadyMoving && this._locoMove.IsValid<AnimationClipPlayable>())
			{
				this._locoMove.SetTime(0.0);
			}
			this._moving = true;
			this.BeginFootsteps();
			bool running = this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape;
			while (travelled < total - 0.002f)
			{
				float deltaTime = Time.deltaTime;
				if (floorBlendSeconds > 0f && this._floorWeight > 0f)
				{
					this._floorWeight = Mathf.MoveTowards(this._floorWeight, 0f, deltaTime / floorBlendSeconds);
				}
				float num = LongWayHomeCinematicPath.BendDegrees(path, along, travelled, 1.2f);
				float num2 = metersPerSecond * Mathf.Lerp(1f, 0.8f, Mathf.InverseLerp(25f, 100f, num));
				float num3 = (stopAtEnd ? (metersPerSecond * Mathf.Sqrt(Mathf.Clamp01((total - travelled) / 0.9f))) : metersPerSecond);
				float num4 = Mathf.Max(Mathf.Min(num2, num3), 0.15f);
				speed = Mathf.SmoothDamp(speed, num4, ref speedVelocity, 0.28f);
				travelled = Mathf.Min(total, travelled + speed * deltaTime);
				Vector3 vector = LongWayHomeCinematicPath.Sample(path, along, travelled);
				Vector3 vector2 = LongWayHomeCinematicPath.Sample(path, along, Mathf.Min(total, travelled + 0.75f)) - vector;
				vector2.y = 0f;
				if (vector2.sqrMagnitude > 0.0004f)
				{
					yaw = Mathf.SmoothDampAngle(yaw, Quaternion.LookRotation(vector2).eulerAngles.y, ref yawVelocity, 0.22f);
				}
				Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
				this._player.SetPositionAndRotation(vector, quaternion);
				this._body.position = vector;
				this._body.rotation = quaternion;
				float num5 = ((metersPerSecond > 0f) ? Mathf.Clamp01(speed / metersPerSecond) : 0f);
				this._walkWeight = Mathf.Clamp01(num5 * 1.6f);
				this.ApplyBodyWeights();
				if (this._locoMove.IsValid<AnimationClipPlayable>())
				{
					this._locoMove.SetSpeed((double)(clipSpeed * Mathf.Lerp(0.7f, 1f, num5)));
				}
				this.TickFootsteps(running);
				yield return null;
			}
			this._moving = false;
			if (this._locoMove.IsValid<AnimationClipPlayable>())
			{
				this._locoMove.SetSpeed((double)clipSpeed);
			}
			this._floorWeight = 0f;
			if (!stopAtEnd)
			{
				yield break;
			}
			while (this._walkWeight > 0f)
			{
				this._walkWeight = Mathf.MoveTowards(this._walkWeight, 0f, Time.deltaTime / 0.25f);
				this.ApplyBodyWeights();
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x00273E95 File Offset: 0x00272095
		private IEnumerator TurnTo(float targetYaw, float seconds)
		{
			float yaw = this._player.eulerAngles.y;
			float velocity = 0f;
			float deadline = Time.time + Mathf.Max(0.2f, seconds * 4f);
			while (Mathf.Abs(Mathf.DeltaAngle(yaw, targetYaw)) > 0.5f && Time.time < deadline)
			{
				yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref velocity, seconds * 0.5f);
				Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
				this._player.rotation = quaternion;
				this._body.rotation = quaternion;
				yield return null;
			}
			yield break;
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x0600625E RID: 25182 RVA: 0x00273EB2 File Offset: 0x002720B2
		private bool CanPickCorridorPose
		{
			get
			{
				return this.IsReady && this._armsMoment == LongWayHomeUniversitySequence.ArmsMoment.Corridor && this.Phase == LongWayHomeUniversitySequence.StoryPhase.Corridor && !this._continueRequested;
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x0600625F RID: 25183 RVA: 0x00273ED9 File Offset: 0x002720D9
		private bool CanToggleCover
		{
			get
			{
				return this.IsReady && this._armsMoment == LongWayHomeUniversitySequence.ArmsMoment.Escape && this.HasArmsSlot(LongWayHomeUniversitySequence.ArmsPose.Cover) && (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Realize || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape);
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06006260 RID: 25184 RVA: 0x00273F0B File Offset: 0x0027210B
		private bool CanCoverLowerBody
		{
			get
			{
				return this._armsOn && (this.CanPickCorridorPose || this.CanToggleCover) && this.CurrentArmsSlot != null && this.CurrentArmsSlot.HasLower;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06006261 RID: 25185 RVA: 0x00273F3A File Offset: 0x0027213A
		private LongWayHomeUniversitySequence.ArmsSlot CurrentArmsSlot
		{
			get
			{
				return this.FindArmsSlot(this._armsPose);
			}
		}

		// Token: 0x06006262 RID: 25186 RVA: 0x00273F48 File Offset: 0x00272148
		private void CaptureArmsData()
		{
			this._boobsType = 3f;
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.name == "BoobsType")
				{
					this._boobsType = ((animatorControllerParameter.type == AnimatorControllerParameterType.Int) ? ((float)this._animator.GetInteger(animatorControllerParameter.nameHash)) : this._animator.GetFloat(animatorControllerParameter.nameHash));
				}
			}
			this._stimulationShapes.Clear();
			this._stimulationPins.Clear();
			this._coverPins.Clear();
			this._coverCloth = null;
			this.CaptureClipDeformations();
			if (this._coverPose == null)
			{
				return;
			}
			LongWayHomeUniversitySequence.CopyShapes(LongWayHomeUniversitySequence.FindPoseSet(this._coverPose.ConfidentBlendShapesPerPose, "Stimulation"), this._stimulationShapes);
			this._coverCloth = LongWayHomeUniversitySequence.ReadField<GameObject[]>(this._coverPose, "magicaClothObjects");
			Transform transform = LongWayHomeUniversitySequence.ReadField<Transform>(this._coverPose, "ikReferenceBone");
			if (transform == null)
			{
				transform = this._animator.GetBoneTransform(HumanBodyBones.Hips);
			}
			LongWayHomeUniversitySequence.ReadPins(LongWayHomeUniversitySequence.ReadField<List<CoverIKOverride>>(this._coverPose, "fullCoverExhibitIK"), transform, this._stimulationPins);
			LongWayHomeUniversitySequence.ReadPins(LongWayHomeUniversitySequence.ReadField<List<CoverIKOverride>>(this._coverPose, "fullCoverEmbarrassedIK"), transform, this._coverPins);
			this._rig = this._coverPose.RigController;
			if (this._rig != null && !this._rig.Initialize())
			{
				this._rig = null;
			}
		}

		// Token: 0x06006263 RID: 25187 RVA: 0x002740CC File Offset: 0x002722CC
		private static CoverPoseBlendShapeSet FindPoseSet(List<CoverPoseBlendShapeSet> sets, string label)
		{
			if (sets == null)
			{
				return null;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in sets)
			{
				if (coverPoseBlendShapeSet != null && string.Equals(coverPoseBlendShapeSet.Label, label, StringComparison.OrdinalIgnoreCase))
				{
					return coverPoseBlendShapeSet;
				}
			}
			return null;
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x00274134 File Offset: 0x00272334
		private static void CopyShapes(CoverPoseBlendShapeSet set, List<CoverBlendShapeEntry> into)
		{
			if (set == null || set.BlendShapes == null)
			{
				return;
			}
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in set.BlendShapes)
			{
				if (coverBlendShapeEntry != null)
				{
					CoverBlendShapeEntry coverBlendShapeEntry2 = new CoverBlendShapeEntry
					{
						TargetRenderer = coverBlendShapeEntry.TargetRenderer,
						BlendShapeName = coverBlendShapeEntry.BlendShapeName,
						TargetValue = coverBlendShapeEntry.TargetValue,
						TargetsByBoobsType = coverBlendShapeEntry.TargetsByBoobsType,
						BlendSpeedOverride = coverBlendShapeEntry.BlendSpeedOverride,
						PreserveExternalValue = coverBlendShapeEntry.PreserveExternalValue,
						RequiresChestContact = coverBlendShapeEntry.RequiresChestContact
					};
					coverBlendShapeEntry2.CacheIndex();
					if (coverBlendShapeEntry2.CachedIndex >= 0)
					{
						into.Add(coverBlendShapeEntry2);
					}
				}
			}
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x00274204 File Offset: 0x00272404
		private static T ReadField<T>(object owner, string name) where T : class
		{
			FieldInfo fieldInfo = ((owner != null) ? owner.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null);
			return ((fieldInfo != null) ? fieldInfo.GetValue(owner) : null) as T;
		}

		// Token: 0x06006266 RID: 25190 RVA: 0x00274234 File Offset: 0x00272434
		private static void ReadPins(List<CoverIKOverride> source, Transform reference, List<LongWayHomeUniversitySequence.LowerPin> into)
		{
			if (source == null || reference == null)
			{
				return;
			}
			foreach (CoverIKOverride coverIKOverride in source)
			{
				if (coverIKOverride != null)
				{
					into.Add(new LongWayHomeUniversitySequence.LowerPin
					{
						Limb = coverIKOverride.limb,
						Reference = ((coverIKOverride.referenceMode == IKReferenceMode.CustomBone && coverIKOverride.customReferenceBone != null) ? coverIKOverride.customReferenceBone : reference),
						Position = coverIKOverride.localPosition,
						Rotation = coverIKOverride.rotationOffset,
						Hint = coverIKOverride.hintLocalPosition,
						PositionWeight = coverIKOverride.targetPositionWeight,
						RotationWeight = coverIKOverride.targetRotationWeight,
						HintWeight = coverIKOverride.hintWeight
					});
				}
			}
		}

		// Token: 0x06006267 RID: 25191 RVA: 0x00274328 File Offset: 0x00272528
		private void BuildArms(int boobsType)
		{
			this._armsSlots.Clear();
			this._armsPose = LongWayHomeUniversitySequence.ArmsPose.None;
			this._armsOn = (this._lowerOn = false);
			this._lowerWeight = (this._pinWeight = 0f);
			this._pinSlot = null;
			if (!this._cielGraph.IsValid() || !this._armsMixer.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			List<CoverPoseBlendShapeSet> list = ((this._coverPose != null) ? this._coverPose.ConfidentBlendShapesPerPose : null);
			List<CoverPoseBlendShapeSet> list2 = ((this._coverPose != null) ? this._coverPose.EmbarrassedBlendShapesPerPose : null);
			this.AddArmsSlot(LongWayHomeUniversitySequence.ArmsPose.Stimulation, LongWayHomeUniversitySequence.FindPoseSet(list, "Stimulation"), this._config.StimulationArmsFor(boobsType), this._config.AdvancedStimulationArmsFor(boobsType), this._stimulationPins);
			this.AddArmsSlot(LongWayHomeUniversitySequence.ArmsPose.HoldBoobs, LongWayHomeUniversitySequence.FindPoseSet(list2, "Hold Boobs"), this._config.HoldBoobsArmsFor(boobsType), null, null);
			this.AddArmsSlot(LongWayHomeUniversitySequence.ArmsPose.ShyPose, LongWayHomeUniversitySequence.FindPoseSet(list2, "Shy Pose"), this._config.ShyPoseArmsFor(boobsType), null, null);
			this.AddArmsSlot(LongWayHomeUniversitySequence.ArmsPose.Cover, LongWayHomeUniversitySequence.FindPoseSet(list2, "Cover"), this._config.CoverArmsFor(boobsType), this._config.CoverLowerArmsFor(boobsType), this._coverPins);
			this.ApplyArmsWeights();
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00274470 File Offset: 0x00272670
		private void AddArmsSlot(LongWayHomeUniversitySequence.ArmsPose pose, CoverPoseBlendShapeSet set, AnimationClip chest, AnimationClip lower, List<LongWayHomeUniversitySequence.LowerPin> pins)
		{
			if (chest == null)
			{
				return;
			}
			bool flag = ((set != null) ? (set.ArmsMode == CoverArmsMode.LeftArmOnly) : (pose == LongWayHomeUniversitySequence.ArmsPose.ShyPose));
			if (flag && (this._config.leftArmMask == null || !this._leftArmMixer.IsValid<AnimationMixerPlayable>()))
			{
				return;
			}
			AnimationMixerPlayable animationMixerPlayable = (flag ? this._leftArmMixer : this._armsMixer);
			LongWayHomeUniversitySequence.ArmsSlot armsSlot = new LongWayHomeUniversitySequence.ArmsSlot
			{
				Pose = pose,
				LeftArmOnly = flag,
				HidesCloth = (set != null && set.DisableMagicaCloth),
				Pins = pins
			};
			armsSlot.Chest = this.CreateArmsClip(chest, animationMixerPlayable, out armsSlot.ChestInput);
			if (lower != null)
			{
				armsSlot.Lower = this.CreateArmsClip(lower, animationMixerPlayable, out armsSlot.LowerInput);
			}
			if (pose != LongWayHomeUniversitySequence.ArmsPose.Stimulation)
			{
				LongWayHomeUniversitySequence.CopyShapes(set, armsSlot.Shapes);
			}
			this._armsSlots.Add(armsSlot);
		}

		// Token: 0x06006269 RID: 25193 RVA: 0x00274550 File Offset: 0x00272750
		private LongWayHomeUniversitySequence.ArmsSlot FindArmsSlot(LongWayHomeUniversitySequence.ArmsPose pose)
		{
			if (pose == LongWayHomeUniversitySequence.ArmsPose.None)
			{
				return null;
			}
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				if (armsSlot.Pose == pose)
				{
					return armsSlot;
				}
			}
			return null;
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x002745B4 File Offset: 0x002727B4
		private bool HasArmsSlot(LongWayHomeUniversitySequence.ArmsPose pose)
		{
			return this.FindArmsSlot(pose) != null;
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x002745C0 File Offset: 0x002727C0
		private void UseArmsMoment(LongWayHomeUniversitySequence.ArmsMoment moment)
		{
			this.StopArms(true);
			this._armsMoment = moment;
			this._armsPose = ((moment == LongWayHomeUniversitySequence.ArmsMoment.Escape) ? LongWayHomeUniversitySequence.ArmsPose.Cover : LongWayHomeUniversitySequence.ArmsPose.None);
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x002745E0 File Offset: 0x002727E0
		private void SelectCorridorPose(LongWayHomeUniversitySequence.ArmsPose pose)
		{
			if (!this.CanPickCorridorPose || !this.HasArmsSlot(pose))
			{
				return;
			}
			if (this._armsOn && this._armsPose == pose)
			{
				this.StopArms(false);
				return;
			}
			if (this._armsOn)
			{
				this.EndPose(this._armsPose, false);
			}
			this._armsPose = pose;
			this.StartArms();
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x0027463A File Offset: 0x0027283A
		private void ToggleCover()
		{
			if (!this.CanToggleCover)
			{
				return;
			}
			this._armsPose = LongWayHomeUniversitySequence.ArmsPose.Cover;
			if (this._armsOn)
			{
				this.StopArms(false);
				return;
			}
			this.StartArms();
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x00274662 File Offset: 0x00272862
		private void ToggleLowerBody()
		{
			if (!this.CanCoverLowerBody)
			{
				return;
			}
			this._lowerOn = !this._lowerOn;
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x0027467C File Offset: 0x0027287C
		private void StartArms()
		{
			LongWayHomeUniversitySequence.ArmsSlot currentArmsSlot = this.CurrentArmsSlot;
			if (currentArmsSlot == null)
			{
				return;
			}
			this._armsOn = true;
			this._lowerOn = false;
			if (currentArmsSlot.Chest.IsValid<AnimationClipPlayable>())
			{
				currentArmsSlot.Chest.SetTime(0.0);
			}
			if (currentArmsSlot.Lower.IsValid<AnimationClipPlayable>())
			{
				currentArmsSlot.Lower.SetTime(0.0);
			}
			if (this._armsPose == LongWayHomeUniversitySequence.ArmsPose.Stimulation)
			{
				LongWayHomeGymShowerFaceDriver face = this._face;
				if (face != null)
				{
					face.BlendTo(this._config.pleasureFaceAnimation, this._config.faceBlendSeconds);
				}
				CielVoiceController instance = CielVoiceController.Instance;
				if (instance != null && this._config.stimulationVoice != null)
				{
					instance.Play(this._config.stimulationVoice);
				}
			}
			this.HideCoverCloth(currentArmsSlot.HidesCloth);
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x00274754 File Offset: 0x00272954
		private void StopArms(bool immediate)
		{
			bool armsOn = this._armsOn;
			this._armsOn = false;
			this._lowerOn = false;
			this._armsPressed = false;
			if (armsOn)
			{
				this.EndPose(this._armsPose, immediate);
			}
			this.HideCoverCloth(false);
			if (!immediate)
			{
				return;
			}
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				armsSlot.Weight = 0f;
				foreach (CoverBlendShapeEntry coverBlendShapeEntry in armsSlot.Shapes)
				{
					coverBlendShapeEntry.RemoveOverride();
				}
			}
			this._lowerWeight = 0f;
			this._pinWeight = 0f;
			this.ApplyArmsWeights();
			this.ReleaseLowerPin();
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x00274840 File Offset: 0x00272A40
		private void EndPose(LongWayHomeUniversitySequence.ArmsPose pose, bool immediate)
		{
			if (pose != LongWayHomeUniversitySequence.ArmsPose.Stimulation)
			{
				return;
			}
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance != null)
			{
				Object currentVoice = instance.CurrentVoice;
				LongWayHomeUniversityConfigSO config = this._config;
				if (currentVoice == ((config != null) ? config.stimulationVoice : null))
				{
					instance.Stop();
				}
			}
			if (!immediate && this.Phase == LongWayHomeUniversitySequence.StoryPhase.Corridor)
			{
				LongWayHomeGymShowerFaceDriver face = this._face;
				if (face == null)
				{
					return;
				}
				face.BlendTo(this._config.corridorFaceAnimation, this._config.faceBlendSeconds * 1.5f);
			}
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x002748C0 File Offset: 0x00272AC0
		private void TickArms(float deltaTime)
		{
			if (!this._layers.IsValid<AnimationLayerMixerPlayable>() || !this._armsMixer.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			float num = deltaTime / Mathf.Max(0.05f, this._config.armsBlendSeconds);
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				armsSlot.Weight = Mathf.MoveTowards(armsSlot.Weight, (this._armsOn && armsSlot.Pose == this._armsPose) ? 1f : 0f, num);
			}
			LongWayHomeUniversitySequence.ArmsSlot currentArmsSlot = this.CurrentArmsSlot;
			bool flag = this._armsOn && this._lowerOn && currentArmsSlot != null && currentArmsSlot.HasLower;
			this._lowerWeight = Mathf.MoveTowards(this._lowerWeight, flag ? 1f : 0f, 4f * deltaTime);
			this.ApplyArmsWeights();
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot2 in this._armsSlots)
			{
				LongWayHomeUniversitySequence.KeepPlaying(armsSlot2.Chest);
				LongWayHomeUniversitySequence.KeepPlaying(armsSlot2.Lower);
			}
			this.TickPoseShapes(deltaTime);
			this.HoldErectNipples();
			this.TickLowerPin(deltaTime);
		}

		// Token: 0x06006273 RID: 25203 RVA: 0x00274A2C File Offset: 0x00272C2C
		private void ApplyArmsWeights()
		{
			if (!this._layers.IsValid<AnimationLayerMixerPlayable>() || !this._armsMixer.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				if (armsSlot.LeftArmOnly)
				{
					num2 += armsSlot.Weight;
				}
				else
				{
					num += armsSlot.Weight;
				}
			}
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot2 in this._armsSlots)
			{
				float num3 = (armsSlot2.LeftArmOnly ? num2 : num);
				float num4 = ((num3 > 0.0001f) ? (armsSlot2.Weight / num3) : 0f);
				float num5 = (armsSlot2.HasLower ? this._lowerWeight : 0f);
				AnimationMixerPlayable animationMixerPlayable = (armsSlot2.LeftArmOnly ? this._leftArmMixer : this._armsMixer);
				if (armsSlot2.ChestInput >= 0)
				{
					animationMixerPlayable.SetInputWeight(armsSlot2.ChestInput, num4 * (1f - num5));
				}
				if (armsSlot2.LowerInput >= 0)
				{
					animationMixerPlayable.SetInputWeight(armsSlot2.LowerInput, num4 * num5);
				}
			}
			this._layers.SetInputWeight(1, Mathf.Clamp01(num));
			if (this._layers.GetInputCount<AnimationLayerMixerPlayable>() > 2)
			{
				this._layers.SetInputWeight(2, Mathf.Clamp01(num2));
			}
		}

		// Token: 0x06006274 RID: 25204 RVA: 0x00274BCC File Offset: 0x00272DCC
		private static void KeepPlaying(AnimationClipPlayable playable)
		{
			if (!playable.IsValid<AnimationClipPlayable>())
			{
				return;
			}
			AnimationClip animationClip = playable.GetAnimationClip();
			double num = ((animationClip != null) ? ((double)animationClip.length) : 0.0);
			if (num <= 0.01 || playable.GetTime<AnimationClipPlayable>() <= num)
			{
				return;
			}
			playable.SetTime(animationClip.isLooping ? (playable.GetTime<AnimationClipPlayable>() % num) : num);
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00274C38 File Offset: 0x00272E38
		private void TickPoseShapes(float deltaTime)
		{
			float num = ((this._coverPose != null) ? this._coverPose.BlendShapeBlendSpeed : 0.005f);
			float num2 = Mathf.Max(num, (this._coverPose != null) ? this._coverPose.LayerBlendSpeed : 2f);
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				bool flag = this._armsOn && armsSlot.Pose == this._armsPose;
				foreach (CoverBlendShapeEntry coverBlendShapeEntry in armsSlot.Shapes)
				{
					if (flag)
					{
						float num3 = ((coverBlendShapeEntry.BlendSpeedOverride > 0f) ? coverBlendShapeEntry.BlendSpeedOverride : num);
						coverBlendShapeEntry.Advance(coverBlendShapeEntry.ResolveTargetValue(this._boobsType), num3 * deltaTime * 100f, 1f);
					}
					else if (coverBlendShapeEntry.HasOverride)
					{
						coverBlendShapeEntry.Release(num2 * deltaTime * 100f);
					}
				}
			}
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x00274D88 File Offset: 0x00272F88
		private void HoldErectNipples()
		{
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._stimulationShapes)
			{
				coverBlendShapeEntry.Advance(coverBlendShapeEntry.ResolveTargetValue(this._boobsType), 1000f, 1f);
			}
		}

		// Token: 0x06006277 RID: 25207 RVA: 0x00274DF0 File Offset: 0x00272FF0
		private void TickLowerPin(float deltaTime)
		{
			if (this._rig == null)
			{
				return;
			}
			LongWayHomeUniversitySequence.ArmsSlot currentArmsSlot = this.CurrentArmsSlot;
			if (this._pinWeight <= 0.001f)
			{
				this._pinSlot = currentArmsSlot;
			}
			float num = ((this._pinSlot != null && this._pinSlot == currentArmsSlot) ? this._lowerWeight : 0f);
			this._pinWeight = Mathf.MoveTowards(this._pinWeight, num, 4f * deltaTime);
			this.ZeroLowerPins();
			LongWayHomeUniversitySequence.ArmsSlot pinSlot = this._pinSlot;
			List<LongWayHomeUniversitySequence.LowerPin> list = ((pinSlot != null) ? pinSlot.Pins : null);
			if (this._pinWeight <= 0.001f || list == null || list.Count == 0)
			{
				this._pinWeight = 0f;
				this.ReleaseLowerPin();
				return;
			}
			foreach (LongWayHomeUniversitySequence.LowerPin lowerPin in list)
			{
				if (!(lowerPin.Reference == null))
				{
					this._rig.SetCoverLimbPoseWeighted(lowerPin.Limb, lowerPin.Reference.TransformPoint(lowerPin.Position), lowerPin.Reference.rotation * Quaternion.Euler(lowerPin.Rotation), this._pinWeight, lowerPin.PositionWeight, lowerPin.RotationWeight, lowerPin.Reference.TransformPoint(lowerPin.Hint), lowerPin.HintWeight);
				}
			}
			if (this._rigOwned || this._rig.IsCoverRigActive)
			{
				return;
			}
			this._rigSavedUpdateMode = this._animator.updateMode;
			this._animator.updateMode = AnimatorUpdateMode.Normal;
			this._rig.ActivateCoverRig();
			this._rigOwned = true;
		}

		// Token: 0x06006278 RID: 25208 RVA: 0x00274FA8 File Offset: 0x002731A8
		private void ZeroLowerPins()
		{
			if (this._rig == null)
			{
				return;
			}
			foreach (LongWayHomeUniversitySequence.LowerPin lowerPin in this._stimulationPins)
			{
				this._rig.ZeroCoverLimb(lowerPin.Limb);
			}
			foreach (LongWayHomeUniversitySequence.LowerPin lowerPin2 in this._coverPins)
			{
				this._rig.ZeroCoverLimb(lowerPin2.Limb);
			}
		}

		// Token: 0x06006279 RID: 25209 RVA: 0x00275060 File Offset: 0x00273260
		private void ReleaseLowerPin()
		{
			this.ZeroLowerPins();
			if (!this._rigOwned)
			{
				return;
			}
			this._rigOwned = false;
			if (this._rig != null)
			{
				this._rig.DeactivateCoverRig();
			}
			if (this._animator != null)
			{
				this._animator.updateMode = this._rigSavedUpdateMode;
			}
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x002750BC File Offset: 0x002732BC
		private void HideCoverCloth(bool hide)
		{
			if (!hide)
			{
				foreach (GameObject gameObject in this._hiddenCloth)
				{
					if (gameObject != null)
					{
						gameObject.SetActive(true);
					}
				}
				this._hiddenCloth.Clear();
				return;
			}
			if (this._coverCloth == null)
			{
				return;
			}
			foreach (GameObject gameObject2 in this._coverCloth)
			{
				if (!(gameObject2 == null) && gameObject2.activeSelf)
				{
					gameObject2.SetActive(false);
					this._hiddenCloth.Add(gameObject2);
				}
			}
		}

		// Token: 0x0600627B RID: 25211 RVA: 0x00275174 File Offset: 0x00273374
		private void ReleaseArms()
		{
			if (this._armsPose != LongWayHomeUniversitySequence.ArmsPose.None || this._armsOn)
			{
				this.StopArms(true);
			}
			this.ReleaseLowerPin();
			this.HideCoverCloth(false);
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._stimulationShapes)
			{
				coverBlendShapeEntry.RemoveOverride();
			}
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				foreach (CoverBlendShapeEntry coverBlendShapeEntry2 in armsSlot.Shapes)
				{
					coverBlendShapeEntry2.RemoveOverride();
				}
			}
			this._armsMoment = LongWayHomeUniversitySequence.ArmsMoment.None;
			this._armsPose = LongWayHomeUniversitySequence.ArmsPose.None;
			this._rig = null;
		}

		// Token: 0x0600627C RID: 25212 RVA: 0x00275278 File Offset: 0x00273478
		private void CaptureClipDeformations()
		{
			this._clipDeformations.Clear();
			this._clipDeformationsShown = false;
			FieldInfo field = typeof(CoverPoseController).GetField("CoverClipDeformationShapes", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			string[] array = ((field != null) ? field.GetValue(null) : null) as string[];
			if (array == null || this._player == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig(skinnedMeshRenderer);
					foreach (string text in array)
					{
						int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(text);
						if (blendShapeIndex >= 0)
						{
							this._clipDeformations.Add(new LongWayHomeUniversitySequence.ClipDeformation
							{
								Renderer = skinnedMeshRenderer,
								Manager = blendShapeManager,
								Shape = text,
								Index = blendShapeIndex,
								Baseline = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex)
							});
						}
					}
				}
			}
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x00275388 File Offset: 0x00273588
		private void KeepClipDeformationsAtRest()
		{
			bool flag = false;
			foreach (LongWayHomeUniversitySequence.ArmsSlot armsSlot in this._armsSlots)
			{
				flag |= armsSlot.Weight > 0f;
			}
			if (flag)
			{
				this._clipDeformationsShown = true;
				return;
			}
			if (this._clipDeformationsShown)
			{
				this.RestoreClipDeformations();
			}
		}

		// Token: 0x0600627E RID: 25214 RVA: 0x00275400 File Offset: 0x00273600
		private void RestoreClipDeformations()
		{
			foreach (LongWayHomeUniversitySequence.ClipDeformation clipDeformation in this._clipDeformations)
			{
				if (!(clipDeformation.Renderer == null))
				{
					float num = ((clipDeformation.Manager != null) ? clipDeformation.Manager.GetEffectiveBlendShapeValue(clipDeformation.Shape, null, false) : clipDeformation.Baseline);
					clipDeformation.Renderer.SetBlendShapeWeight(clipDeformation.Index, num);
				}
			}
		}

		// Token: 0x0600627F RID: 25215 RVA: 0x00275498 File Offset: 0x00273698
		private void CreateCamera()
		{
			GameObject gameObject = new GameObject("Camera - University Opening");
			gameObject.transform.SetParent(base.transform, false);
			this._camera = gameObject.AddComponent<Camera>();
			this._camera.enabled = false;
			this._camera.nearClipPlane = 0.03f;
			this._camera.farClipPlane = 400f;
			UniversalAdditionalCameraData universalAdditionalCameraData = gameObject.AddComponent<UniversalAdditionalCameraData>();
			if (this._gameplayView != null)
			{
				this._camera.cullingMask = this._gameplayView.cullingMask;
				this._camera.clearFlags = this._gameplayView.clearFlags;
				this._camera.backgroundColor = this._gameplayView.backgroundColor;
				this._camera.allowHDR = this._gameplayView.allowHDR;
				this._camera.allowMSAA = this._gameplayView.allowMSAA;
				UniversalAdditionalCameraData component = this._gameplayView.GetComponent<UniversalAdditionalCameraData>();
				if (component != null)
				{
					universalAdditionalCameraData.renderPostProcessing = component.renderPostProcessing;
					universalAdditionalCameraData.antialiasing = component.antialiasing;
					universalAdditionalCameraData.antialiasingQuality = component.antialiasingQuality;
					universalAdditionalCameraData.renderShadows = component.renderShadows;
					universalAdditionalCameraData.volumeLayerMask = component.volumeLayerMask;
					universalAdditionalCameraData.stopNaN = component.stopNaN;
					universalAdditionalCameraData.dithering = component.dithering;
				}
			}
			gameObject.AddComponent<AudioListener>();
			this._camera.enabled = true;
			this._eyelids = base.gameObject.AddComponent<LongWayHomeEyelidOverlay>();
		}

		// Token: 0x06006280 RID: 25216 RVA: 0x00275610 File Offset: 0x00273810
		private void DisableCamera()
		{
			if (this._camera != null)
			{
				this._camera.enabled = false;
				AudioListener component = this._camera.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			if (this._eyelids != null)
			{
				this._eyelids.Visible = false;
			}
			this.SetRenderCallbacks(false);
			this.RestoreHeadRenderers();
			this._headRenderers.Clear();
		}

		// Token: 0x06006281 RID: 25217 RVA: 0x00275685 File Offset: 0x00273885
		private void StartShots(LongWayHomeUniversityConfigSO.BodyShot[] shots)
		{
			this._shots = shots;
			this.SelectShot(0, true);
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x00275698 File Offset: 0x00273898
		private void SelectShot(int index, bool snap)
		{
			if (this._shots == null || this._shots.Length == 0)
			{
				return;
			}
			this._orbiting = false;
			this._shotMode = LongWayHomeUniversitySequence.ShotMode.Body;
			this._shotIndex = (index % this._shots.Length + this._shots.Length) % this._shots.Length;
			this._fieldOfView = this._shots[this._shotIndex].fieldOfView;
			this.UpdateCamera(true);
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x00275706 File Offset: 0x00273906
		private void NextShot()
		{
			this.SelectShot(this._shotIndex + 1, true);
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00275717 File Offset: 0x00273917
		private void ShowFaceShot()
		{
			this._orbiting = false;
			this._shotMode = LongWayHomeUniversitySequence.ShotMode.Face;
			this._fieldOfView = this._config.faceShotFieldOfView;
			this.UpdateCamera(true);
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x0027573F File Offset: 0x0027393F
		private void ActivateFixedShot(Vector3 position, float fieldOfView)
		{
			this._orbiting = false;
			this._shotMode = LongWayHomeUniversitySequence.ShotMode.Fixed;
			this._fixedPosition = position;
			this._fieldOfView = fieldOfView;
			this.UpdateCamera(true);
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x00275764 File Offset: 0x00273964
		private vThirdPersonCameraState GameplayCameraState()
		{
			List<vThirdPersonCameraState> list = ((this._gameplayCamera != null && this._gameplayCamera.CameraStateList != null) ? this._gameplayCamera.CameraStateList.tpCameraStates : null);
			if (list != null)
			{
				foreach (vThirdPersonCameraState vThirdPersonCameraState in list)
				{
					if (vThirdPersonCameraState != null && vThirdPersonCameraState.Name == "Default")
					{
						return vThirdPersonCameraState;
					}
				}
			}
			return LongWayHomeUniversitySequence.FallbackCameraState;
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06006287 RID: 25223 RVA: 0x00275804 File Offset: 0x00273A04
		private float ScrollSpeed
		{
			get
			{
				if (!(this._gameplayCamera != null))
				{
					return 2f;
				}
				return this._gameplayCamera.scrollSpeed;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06006288 RID: 25224 RVA: 0x00275825 File Offset: 0x00273A25
		private float SmoothBetweenStates
		{
			get
			{
				if (!(this._gameplayCamera != null))
				{
					return 6f;
				}
				return this._gameplayCamera.smoothBetweenState;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06006289 RID: 25225 RVA: 0x00275846 File Offset: 0x00273A46
		private float RotationSharpness
		{
			get
			{
				if (!(this._gameplayCamera != null))
				{
					return 12f;
				}
				return this._gameplayCamera.smoothCameraRotation;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600628A RID: 25226 RVA: 0x00275867 File Offset: 0x00273A67
		private float StickSensitivity
		{
			get
			{
				if (!(this._gameplayCamera != null))
				{
					return 1f;
				}
				return this._gameplayCamera.joystickSensitivity;
			}
		}

		// Token: 0x0600628B RID: 25227 RVA: 0x00275888 File Offset: 0x00273A88
		private static float InvectorBlend(float rate, float deltaTime)
		{
			float num = Mathf.Max(0.001f, Time.fixedDeltaTime);
			return 1f - Mathf.Pow(1f - Mathf.Clamp(rate * num, 0f, 0.999f), deltaTime / num);
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x0600628C RID: 25228 RVA: 0x002758CB File Offset: 0x00273ACB
		private bool CanOrbit
		{
			get
			{
				return !this._pov && this._shotMode != LongWayHomeUniversitySequence.ShotMode.Fixed && this.OnHallFloor;
			}
		}

		// Token: 0x0600628D RID: 25229 RVA: 0x002758E6 File Offset: 0x00273AE6
		private void AddLookInput(Vector3 input, bool fromPad)
		{
			this._lookInput += input;
			this._lookFromPad = fromPad;
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x00275904 File Offset: 0x00273B04
		private void BeginOrbit()
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.GameplayCameraState();
			Transform transform = this._camera.transform;
			this._orbitAnchor = ((this._shotMode == LongWayHomeUniversitySequence.ShotMode.Face) ? this.Bone(HumanBodyBones.Head) : ((this._shots != null && this._shots.Length != 0) ? this.Bone(this._shots[this._shotIndex].anchor) : this.Bone(HumanBodyBones.Hips)));
			Vector3 focus = this._focus;
			Vector3 vector = transform.position - focus;
			if (vector.sqrMagnitude < 0.01f)
			{
				vector = -transform.forward;
			}
			float magnitude = vector.magnitude;
			float num = Mathf.Asin(Mathf.Clamp(vector.y / magnitude, -1f, 1f)) * 57.29578f;
			this._orbitAnchorOffset = focus - this._orbitAnchor.position;
			this._orbitPivot = focus;
			this._orbitPivotVelocity = Vector3.zero;
			this._orbitYaw = Mathf.Atan2(-vector.x, -vector.z) * 57.29578f;
			this._orbitRotation = Quaternion.Euler(num, this._orbitYaw, 0f);
			this._orbitPitch = Mathf.Clamp(num, this.OrbitMinPitch(vThirdPersonCameraState), vThirdPersonCameraState.yMaxLimit);
			this._orbitDistance = magnitude;
			this._orbitZoom = Mathf.Clamp(magnitude, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
			this._orbitFieldOfView = this._camera.fieldOfView;
			this._orbitReach = 1f;
			this._orbiting = true;
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x00275A89 File Offset: 0x00273C89
		private float OrbitMinPitch(vThirdPersonCameraState state)
		{
			return Mathf.Max(state.yMinLimit, this._config.orbitMinPitch);
		}

		// Token: 0x06006290 RID: 25232 RVA: 0x00275AA4 File Offset: 0x00273CA4
		private void PlaceOrbitCamera(bool snap)
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.GameplayCameraState();
			float deltaTime = Time.deltaTime;
			Vector3 lookInput = this._lookInput;
			this._lookInput = Vector3.zero;
			float num = (this._lookFromPad ? this.StickSensitivity : LongWayHomeUniversitySequence.GameSettingsLook);
			this._orbitYaw = Mathf.Repeat(this._orbitYaw + lookInput.x * vThirdPersonCameraState.xMouseSensitivity * num, 360f);
			this._orbitPitch = Mathf.Clamp(this._orbitPitch - lookInput.y * vThirdPersonCameraState.yMouseSensitivity * num, this.OrbitMinPitch(vThirdPersonCameraState), vThirdPersonCameraState.yMaxLimit);
			this._orbitZoom = Mathf.Clamp(this._orbitZoom - lookInput.z * this.ScrollSpeed, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
			float num2 = ((this._orbitZoom < this._orbitDistance) ? Mathf.Max(vThirdPersonCameraState.smooth, this.SmoothBetweenStates) : Mathf.Min(vThirdPersonCameraState.smooth, this.SmoothBetweenStates));
			this._orbitDistance = (snap ? this._orbitZoom : Mathf.Lerp(this._orbitDistance, this._orbitZoom, LongWayHomeUniversitySequence.InvectorBlend(num2, deltaTime)));
			Quaternion quaternion = Quaternion.Euler(this._orbitPitch, this._orbitYaw, 0f);
			this._orbitRotation = (snap ? quaternion : Quaternion.Lerp(this._orbitRotation, quaternion, LongWayHomeUniversitySequence.InvectorBlend(this.RotationSharpness, deltaTime)));
			this._orbitFieldOfView = (snap ? vThirdPersonCameraState.fov : Mathf.Lerp(this._orbitFieldOfView, vThirdPersonCameraState.fov, LongWayHomeUniversitySequence.InvectorBlend(this.SmoothBetweenStates, deltaTime)));
			Vector3 vector = ((this._orbitAnchor != null) ? this._orbitAnchor.position : this._player.position) + this._orbitAnchorOffset;
			if (snap)
			{
				this._orbitPivot = vector;
				this._orbitPivotVelocity = Vector3.zero;
			}
			else
			{
				this._orbitPivot = Vector3.SmoothDamp(this._orbitPivot, vector, ref this._orbitPivotVelocity, Mathf.Max(0.01f, vThirdPersonCameraState.smoothDamp * Time.fixedDeltaTime));
			}
			Vector3 vector2 = this._orbitPivot + this._orbitRotation * Vector3.back * this._orbitDistance;
			float num3 = this._floorBaseY + 0.08f;
			if (vector2.y < num3)
			{
				vector2.y = num3;
			}
			Vector3 vector3 = vector2 - this._orbitPivot;
			float magnitude = vector3.magnitude;
			float num4 = 1f;
			if (magnitude > 0.01f)
			{
				float num5 = magnitude;
				int num6 = Physics.SphereCastNonAlloc(this._orbitPivot, 0.1f, vector3 / magnitude, this._cameraHits, magnitude, -1, QueryTriggerInteraction.Ignore);
				for (int i = 0; i < num6; i++)
				{
					if (this._cameraHits[i].collider != null && !this._cameraHits[i].collider.transform.IsChildOf(this._player) && this._cameraHits[i].distance > 0f && this._cameraHits[i].distance < num5)
					{
						num5 = this._cameraHits[i].distance;
					}
				}
				if (num5 < magnitude)
				{
					num4 = Mathf.Max(0.12f, num5 - 0.05f) / magnitude;
				}
			}
			this._orbitReach = ((snap || num4 < this._orbitReach) ? num4 : Mathf.Lerp(this._orbitReach, num4, LongWayHomeUniversitySequence.InvectorBlend(Mathf.Min(vThirdPersonCameraState.smooth, this.SmoothBetweenStates), deltaTime)));
			Vector3 vector4 = this._orbitPivot + vector3 * this._orbitReach;
			Vector3 vector5 = this._orbitPivot - vector4;
			this._camera.transform.SetPositionAndRotation(vector4, (vector5.sqrMagnitude > 1E-06f) ? Quaternion.LookRotation(vector5) : this._orbitRotation);
			this._camera.nearClipPlane = 0.03f;
			this._camera.fieldOfView = this._orbitFieldOfView;
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06006291 RID: 25233 RVA: 0x0026BD60 File Offset: 0x00269F60
		private static float GameSettingsLook
		{
			get
			{
				return Mathf.Max(0.05f, GameSettingsService.LookSensitivity);
			}
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x00275EB0 File Offset: 0x002740B0
		private void ToggleFirstPerson()
		{
			this.SetFirstPerson(!this._pov);
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x00275EC4 File Offset: 0x002740C4
		private void SetFirstPerson(bool enabled)
		{
			if (this._pov == enabled)
			{
				return;
			}
			this._pov = enabled;
			this._povPitch = ((this._config != null) ? this._config.povStartPitch : 0f);
			this._povYaw = 0f;
			this._povYInitialized = false;
			this._povRotationInitialized = false;
			this._lookInput = Vector3.zero;
			if (this._eyelids != null)
			{
				this._eyelids.Visible = enabled;
			}
			this.SetRenderCallbacks(enabled);
			if (enabled)
			{
				this.RefreshHeadRenderers();
			}
			else
			{
				this.RestoreHeadRenderers();
			}
			this.UpdateCamera(true);
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x00275F68 File Offset: 0x00274168
		private void ResetLook()
		{
			this._povPitch = ((this._config != null) ? this._config.povStartPitch : 0f);
			this._povYaw = 0f;
			this._povYInitialized = false;
			this._lookInput = Vector3.zero;
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06006295 RID: 25237 RVA: 0x00275FB8 File Offset: 0x002741B8
		private bool PovLying
		{
			get
			{
				return this.Phase == LongWayHomeUniversitySequence.StoryPhase.Fall || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Floor || this.Phase == LongWayHomeUniversitySequence.StoryPhase.FallingAsleep || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Morning || this.Phase == LongWayHomeUniversitySequence.StoryPhase.GetUp;
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06006296 RID: 25238 RVA: 0x00275FE9 File Offset: 0x002741E9
		private bool PovEmbarrassedStance
		{
			get
			{
				return this.Phase == LongWayHomeUniversitySequence.StoryPhase.Realize || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Handoff;
			}
		}

		// Token: 0x06006297 RID: 25239 RVA: 0x0027600C File Offset: 0x0027420C
		private void PlaceEyeCamera(bool snap)
		{
			Transform transform = this.Bone(HumanBodyBones.Head);
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.LeftEye);
			Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.RightEye);
			Vector3 vector = ((boneTransform != null && boneTransform2 != null) ? ((boneTransform.position + boneTransform2.position) * 0.5f) : transform.position);
			Vector3 lookInput = this._lookInput;
			this._lookInput = Vector3.zero;
			float num = 3f * LongWayHomeUniversitySequence.GameSettingsLook;
			this._povYaw += lookInput.x * num;
			this._povPitch -= lookInput.y * num;
			this._povEmbarrassed = (snap ? (this.PovEmbarrassedStance ? 1f : 0f) : Mathf.MoveTowards(this._povEmbarrassed, this.PovEmbarrassedStance ? 1f : 0f, 8f * Time.deltaTime));
			bool flag = this.PovLying && (this.Phase != LongWayHomeUniversitySequence.StoryPhase.Fall || this._routine == null || this._routine.ActiveTime >= 0.25);
			Vector3 vector2;
			Quaternion quaternion;
			if (!flag)
			{
				this._povYaw = Mathf.Clamp(this._povYaw, -80f, 80f);
				this._povPitch = Mathf.Clamp(this._povPitch, -80f, 80f);
				vector2 = this.StandingEyePosition(transform, boneTransform != null && boneTransform2 != null, vector);
				float y = this._player.eulerAngles.y;
				quaternion = Quaternion.Euler(Mathf.Clamp(this._povPitch + 14.03f * this._povEmbarrassed, -89f, 89f), y + this._povYaw, 0f);
				this._povBasis = Quaternion.Inverse(transform.rotation) * Quaternion.Euler(0f, y, 0f);
			}
			else
			{
				this._povYaw = Mathf.Clamp(this._povYaw, -80f, 80f);
				this._povPitch = Mathf.Clamp(this._povPitch, -70f, 60f);
				vector2 = this.LyingEyePosition(transform, vector);
				quaternion = transform.rotation * this._povBasis * Quaternion.Euler(this._povPitch, this._povYaw, 0f);
				this._povYInitialized = false;
			}
			if (snap || !this._povRotationInitialized)
			{
				this._povCorrection = Quaternion.identity;
				this._povPositionCorrection = Vector3.zero;
				this._povCorrectionElapsed = 1f;
				this._povWasLying = flag;
				this._povRotationInitialized = true;
			}
			else if (flag != this._povWasLying)
			{
				this._povCorrection = this._povLastRotation * Quaternion.Inverse(quaternion);
				this._povPositionCorrection = this._povLastPosition - vector2;
				this._povCorrectionElapsed = 0f;
				this._povWasLying = flag;
			}
			float num2 = (flag ? 0.12f : 0.25f);
			float num3 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(this._povCorrectionElapsed / num2));
			Quaternion quaternion2 = Quaternion.Slerp(this._povCorrection, Quaternion.identity, num3) * quaternion;
			vector2 += this._povPositionCorrection * (1f - num3);
			this._povCorrectionElapsed = Mathf.Min(num2, this._povCorrectionElapsed + Time.unscaledDeltaTime);
			this._povLastRotation = quaternion2;
			this._povLastPosition = vector2;
			this._camera.transform.SetPositionAndRotation(vector2, quaternion2);
			this._camera.nearClipPlane = 0.01f;
			this._camera.fieldOfView = this.GameplayCameraState().fov;
			if (Time.unscaledTime >= this._povHeadRefreshAt)
			{
				this.RefreshHeadRenderers();
			}
		}

		// Token: 0x06006298 RID: 25240 RVA: 0x002763EC File Offset: 0x002745EC
		private Vector3 StandingEyePosition(Transform head, bool hasEyes, Vector3 eyeMid)
		{
			Vector3 up = this._player.up;
			Vector3 normalized = Vector3.ProjectOnPlane(this._player.forward, up).normalized;
			Vector3 vector2;
			if (hasEyes)
			{
				Vector3 vector = Vector3.ProjectOnPlane(eyeMid - head.position, up);
				if (vector.sqrMagnitude < 1E-06f)
				{
					vector = normalized;
				}
				vector2 = eyeMid + up * -0.08f + vector.normalized * 0.045f;
			}
			else
			{
				vector2 = head.position + up * -0.02f + normalized * 0.125f;
			}
			if (this._povEmbarrassed > 0.0001f)
			{
				vector2 = this.TorsoAnchored(vector2, up, normalized, this._povEmbarrassed, Mathf.Lerp(0.15f, 0.17f, this._povEmbarrassed), 0.776f * this._povEmbarrassed);
			}
			float y = this._player.InverseTransformPoint(vector2).y;
			if (!this._povYInitialized)
			{
				this._povSmoothedY = y;
				this._povYInitialized = true;
			}
			else
			{
				float num = Mathf.Clamp01(5f * Time.deltaTime);
				this._povSmoothedY = Mathf.Lerp(this._povSmoothedY, y, num);
			}
			float num2 = Mathf.Lerp(y, this._povSmoothedY, 0.5f);
			Vector3 vector3 = this._player.TransformVector(Vector3.up * (num2 - y));
			return vector2 + Vector3.ClampMagnitude(vector3, 0.025f);
		}

		// Token: 0x06006299 RID: 25241 RVA: 0x00276568 File Offset: 0x00274768
		private Vector3 LyingEyePosition(Transform head, Vector3 eyeMid)
		{
			Transform transform = this.Bone(HumanBodyBones.Chest);
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Neck);
			Vector3 vector = ((boneTransform != null) ? boneTransform.position : head.position) - transform.position;
			vector = ((vector.sqrMagnitude > 1E-06f) ? vector.normalized : Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(eyeMid - head.position, vector);
			vector2 = ((vector2.sqrMagnitude > 1E-06f) ? vector2.normalized : Vector3.ProjectOnPlane(this._player.forward, vector).normalized);
			Vector3 vector3 = eyeMid + vector * -0.08f + vector2 * 0.045f;
			return this.TorsoAnchored(vector3, vector, vector2, 1f, 0.17f, 0.776f);
		}

		// Token: 0x0600629A RID: 25242 RVA: 0x0027664C File Offset: 0x0027484C
		private Vector3 TorsoAnchored(Vector3 eyes, Vector3 up, Vector3 fallbackForward, float anchor, float forwardOffset, float verticalAnchor)
		{
			Transform transform = this.Bone(HumanBodyBones.Chest);
			if (this._leftBreast == null)
			{
				this._leftBreast = LongWayHomeUniversitySequence.FindChild(this._player, "c_breast_01.l");
			}
			if (this._rightBreast == null)
			{
				this._rightBreast = LongWayHomeUniversitySequence.FindChild(this._player, "c_breast_01.r");
			}
			Vector3 vector = fallbackForward;
			if (this._leftBreast != null && this._rightBreast != null)
			{
				Vector3 vector2 = Vector3.ProjectOnPlane((this._leftBreast.position + this._rightBreast.position) * 0.5f - transform.position, up);
				if (vector2.sqrMagnitude > 1E-06f)
				{
					vector = vector2.normalized;
				}
			}
			Vector3 vector3 = transform.position + vector * forwardOffset;
			Vector3 vector4 = vector3 + Vector3.Project(eyes - vector3, up);
			Vector3 vector5 = Vector3.Lerp(eyes, vector4, anchor);
			if (verticalAnchor > 0.0001f)
			{
				float num = Vector3.Dot(vector5 - transform.position, up);
				vector5 += up * ((0.16f - num) * verticalAnchor);
			}
			return vector5;
		}

		// Token: 0x0600629B RID: 25243 RVA: 0x00276784 File Offset: 0x00274984
		private static Transform FindChild(Transform parent, string childName)
		{
			if (parent == null)
			{
				return null;
			}
			if (parent.name == childName)
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = LongWayHomeUniversitySequence.FindChild(parent.GetChild(i), childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x0600629C RID: 25244 RVA: 0x002767D8 File Offset: 0x002749D8
		private void SetRenderCallbacks(bool enabled)
		{
			if (this._renderCallbacks == enabled)
			{
				return;
			}
			this._renderCallbacks = enabled;
			if (enabled)
			{
				RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
				RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
				return;
			}
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
		}

		// Token: 0x0600629D RID: 25245 RVA: 0x00276840 File Offset: 0x00274A40
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera view)
		{
			if (view != this._camera || !this._pov || this._headHidden)
			{
				return;
			}
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this._headRenderers)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
				}
			}
			this._headHidden = true;
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x002768D0 File Offset: 0x00274AD0
		private void OnEndCameraRendering(ScriptableRenderContext context, Camera view)
		{
			if (view != this._camera)
			{
				return;
			}
			this.RestoreHeadRenderers();
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x002768E8 File Offset: 0x00274AE8
		private void RestoreHeadRenderers()
		{
			if (!this._headHidden)
			{
				return;
			}
			foreach (KeyValuePair<Renderer, ShadowCastingMode> keyValuePair in this._headRenderers)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.shadowCastingMode = keyValuePair.Value;
				}
			}
			this._headHidden = false;
		}

		// Token: 0x060062A0 RID: 25248 RVA: 0x00276968 File Offset: 0x00274B68
		private void RefreshHeadRenderers()
		{
			this._povHeadRefreshAt = Time.unscaledTime + 0.25f;
			if (this._player == null)
			{
				return;
			}
			this.RestoreHeadRenderers();
			this._headRenderers.Clear();
			foreach (Renderer renderer in this._player.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer != null && this.ShouldHideInFirstPerson(renderer))
				{
					this._headRenderers[renderer] = renderer.shadowCastingMode;
				}
			}
		}

		// Token: 0x060062A1 RID: 25249 RVA: 0x002769EC File Offset: 0x00274BEC
		private bool ShouldHideInFirstPerson(Renderer renderer)
		{
			if (renderer.GetComponentInParent<FirstPersonVisibleProp>(true) != null)
			{
				return false;
			}
			if (renderer.gameObject.layer == 21)
			{
				return true;
			}
			Transform transform = renderer.transform;
			int num = 0;
			while (transform != null && transform != this._player && num < 4)
			{
				if (LongWayHomeUniversitySequence.IsHeadSemantic(transform.name))
				{
					return true;
				}
				num++;
				transform = transform.parent;
			}
			foreach (Material material in renderer.sharedMaterials)
			{
				if (material != null && LongWayHomeUniversitySequence.IsHeadSemantic(material.name))
				{
					return true;
				}
			}
			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			return skinnedMeshRenderer != null && skinnedMeshRenderer.sharedMesh != null && LongWayHomeUniversitySequence.IsHeadSemantic(skinnedMeshRenderer.sharedMesh.name);
		}

		// Token: 0x060062A2 RID: 25250 RVA: 0x00276AC0 File Offset: 0x00274CC0
		private static bool IsHeadSemantic(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			foreach (string text in LongWayHomeUniversitySequence.HeadSemantics)
			{
				if (value.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062A3 RID: 25251 RVA: 0x00276B00 File Offset: 0x00274D00
		private void UpdateCamera(bool snap)
		{
			if (this._camera == null || this._animator == null)
			{
				return;
			}
			if (this._shotMode == LongWayHomeUniversitySequence.ShotMode.Body && (this._shots == null || this._shots.Length == 0) && !this._pov)
			{
				return;
			}
			float y = this._player.eulerAngles.y;
			if (snap)
			{
				this._cameraYaw = y;
				this._cameraYawVelocity = 0f;
			}
			else
			{
				this._cameraYaw = Mathf.SmoothDampAngle(this._cameraYaw, y, ref this._cameraYawVelocity, 0.45f);
			}
			if (this._pov)
			{
				this.PlaceEyeCamera(snap);
				return;
			}
			if (this.CanOrbit && (this._orbiting || this._lookInput != Vector3.zero))
			{
				if (!this._orbiting)
				{
					this.BeginOrbit();
				}
				this.PlaceOrbitCamera(snap);
				return;
			}
			this._lookInput = Vector3.zero;
			this._orbiting = false;
			this._camera.nearClipPlane = 0.03f;
			this._camera.fieldOfView = this._fieldOfView;
			Transform transform = this._camera.transform;
			LongWayHomeUniversitySequence.ShotMode shotMode = this._shotMode;
			if (shotMode == LongWayHomeUniversitySequence.ShotMode.Fixed)
			{
				Vector3 vector = this.Bone(HumanBodyBones.Hips).position + Vector3.up * 0.15f;
				transform.position = this._fixedPosition;
				if (snap)
				{
					this._focus = vector;
					this._focusVelocity = Vector3.zero;
				}
				else
				{
					this._focus = Vector3.SmoothDamp(this._focus, vector, ref this._focusVelocity, 0.22f);
				}
				transform.rotation = Quaternion.LookRotation(this._focus - transform.position);
				return;
			}
			if (shotMode != LongWayHomeUniversitySequence.ShotMode.Face)
			{
				LongWayHomeUniversityConfigSO.BodyShot bodyShot = this._shots[this._shotIndex];
				Transform transform2 = this.Bone(bodyShot.anchor);
				Quaternion quaternion = Quaternion.Euler(0f, this._cameraYaw, 0f);
				Vector3 vector2 = transform2.position + quaternion * bodyShot.lookOffset;
				Vector3 vector3 = this.KeepClear(vector2, transform2.position + quaternion * bodyShot.cameraOffset, snap);
				this.Place(transform, vector3, vector2, snap, bodyShot.damping);
				return;
			}
			Transform transform3 = this.Bone(HumanBodyBones.Head);
			Vector3 vector4 = this.FaceForward(transform3);
			Vector3 vector5 = transform3.position + vector4 * this._config.faceShotDistance + Vector3.up * this._config.faceShotHeight;
			Vector3 vector6 = transform3.position + vector4 * 0.05f;
			this.Place(transform, vector5, vector6, snap, 0.25f);
		}

		// Token: 0x060062A4 RID: 25252 RVA: 0x00276DB0 File Offset: 0x00274FB0
		private void Place(Transform view, Vector3 position, Vector3 focus, bool snap, float damping)
		{
			if (snap || damping <= 0.001f)
			{
				view.position = position;
				this._cameraVelocity = Vector3.zero;
				this._focus = focus;
				this._focusVelocity = Vector3.zero;
			}
			else
			{
				view.position = Vector3.SmoothDamp(view.position, position, ref this._cameraVelocity, damping);
				this._focus = Vector3.SmoothDamp(this._focus, focus, ref this._focusVelocity, 0.08f);
			}
			Vector3 vector = this._focus - view.position;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return;
			}
			Vector3 vector2 = ((Mathf.Abs(vector.normalized.y) > 0.92f) ? this.BodyAxis() : Vector3.up);
			view.rotation = Quaternion.LookRotation(vector, vector2);
		}

		// Token: 0x060062A5 RID: 25253 RVA: 0x00276E7C File Offset: 0x0027507C
		private Vector3 KeepClear(Vector3 focus, Vector3 desired, bool snap)
		{
			Vector3 vector = desired - focus;
			float magnitude = vector.magnitude;
			float num = 1f;
			if (magnitude > 0.01f)
			{
				float num2 = magnitude;
				int num3 = Physics.SphereCastNonAlloc(focus, 0.12f, vector / magnitude, this._cameraHits, magnitude, -1, QueryTriggerInteraction.Ignore);
				for (int i = 0; i < num3; i++)
				{
					if (this._cameraHits[i].collider != null && !this._cameraHits[i].collider.transform.IsChildOf(this._player) && this._cameraHits[i].distance > 0f && this._cameraHits[i].distance < num2)
					{
						num2 = this._cameraHits[i].distance;
					}
				}
				if (num2 < magnitude)
				{
					num = Mathf.Max(0.25f, num2 - 0.05f) / magnitude;
				}
			}
			if (snap)
			{
				this._reach = num;
				this._reachVelocity = 0f;
			}
			else
			{
				this._reach = Mathf.SmoothDamp(this._reach, num, ref this._reachVelocity, (num < this._reach) ? 0.08f : 0.6f);
			}
			return focus + vector * this._reach;
		}

		// Token: 0x060062A6 RID: 25254 RVA: 0x00276FD0 File Offset: 0x002751D0
		private Vector3 FaceForward(Transform head)
		{
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.LeftEye);
			Transform boneTransform2 = this._animator.GetBoneTransform(HumanBodyBones.RightEye);
			if (boneTransform != null && boneTransform2 != null)
			{
				Vector3 vector = Vector3.ProjectOnPlane((boneTransform.position + boneTransform2.position) * 0.5f - head.position, this.BodyAxisAt(head));
				if (vector.sqrMagnitude > 1E-06f)
				{
					return vector.normalized;
				}
			}
			return head.forward;
		}

		// Token: 0x060062A7 RID: 25255 RVA: 0x0027705C File Offset: 0x0027525C
		private Vector3 BodyAxisAt(Transform head)
		{
			Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Neck);
			Vector3 vector = ((boneTransform != null) ? (head.position - boneTransform.position) : Vector3.up);
			if (vector.sqrMagnitude <= 1E-06f)
			{
				return Vector3.up;
			}
			return vector.normalized;
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x002770B4 File Offset: 0x002752B4
		private Vector3 BodyAxis()
		{
			Vector3 vector = this.Bone(HumanBodyBones.Head).position - this.Bone(HumanBodyBones.Hips).position;
			vector.y = 0f;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return this._player.forward;
			}
			return vector.normalized;
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x00277110 File Offset: 0x00275310
		private Transform Bone(HumanBodyBones bone)
		{
			Transform transform = ((this._animator != null) ? this._animator.GetBoneTransform(bone) : null);
			if (transform != null)
			{
				return transform;
			}
			if (bone == HumanBodyBones.Chest || bone == HumanBodyBones.UpperChest)
			{
				transform = this._animator.GetBoneTransform(HumanBodyBones.Spine);
				if (transform != null)
				{
					return transform;
				}
			}
			Transform transform2 = ((this._animator != null) ? this._animator.GetBoneTransform(HumanBodyBones.Hips) : null);
			if (!(transform2 != null))
			{
				return this._player;
			}
			return transform2;
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x060062AA RID: 25258 RVA: 0x00277196 File Offset: 0x00275396
		// (set) Token: 0x060062AB RID: 25259 RVA: 0x0027719E File Offset: 0x0027539E
		public LongWayHomeUniversitySequence.StoryPhase Phase { get; private set; }

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x060062AC RID: 25260 RVA: 0x002771A7 File Offset: 0x002753A7
		// (set) Token: 0x060062AD RID: 25261 RVA: 0x002771AF File Offset: 0x002753AF
		public bool IsReady { get; private set; }

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x060062AE RID: 25262 RVA: 0x002771B8 File Offset: 0x002753B8
		// (set) Token: 0x060062AF RID: 25263 RVA: 0x002771C0 File Offset: 0x002753C0
		public bool IsPreparedForReveal { get; private set; }

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x060062B0 RID: 25264 RVA: 0x002771C9 File Offset: 0x002753C9
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Failed && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Done;
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x060062B1 RID: 25265 RVA: 0x002771EC File Offset: 0x002753EC
		public Transform RuntimePlayer
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x002771F4 File Offset: 0x002753F4
		public LongWayHomeFloorRoutine Routine
		{
			get
			{
				return this._routine;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x002771FC File Offset: 0x002753FC
		public bool CanSkip
		{
			get
			{
				return this.IsReady && this._captured && !this._skipRequested && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Loading && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Handoff && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Done && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Failed;
			}
		}

		// Token: 0x060062B4 RID: 25268 RVA: 0x0027724C File Offset: 0x0027544C
		public static LongWayHomeUniversitySequence GetPresenterForScene(Scene scene)
		{
			if (!(LongWayHomeUniversitySequence._activePresenter != null) || !LongWayHomeUniversitySequence._activePresenter.isActiveAndEnabled || !(LongWayHomeUniversitySequence._activePresenter.gameObject.scene == scene))
			{
				return null;
			}
			return LongWayHomeUniversitySequence._activePresenter;
		}

		// Token: 0x060062B5 RID: 25269 RVA: 0x00277288 File Offset: 0x00275488
		public static LongWayHomeUniversitySequence Install(Scene scene)
		{
			LongWayHomeUniversitySequence presenterForScene = LongWayHomeUniversitySequence.GetPresenterForScene(scene);
			if (presenterForScene != null)
			{
				return presenterForScene;
			}
			GameObject gameObject = new GameObject("Long Way Home - University Opening");
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			return gameObject.AddComponent<LongWayHomeUniversitySequence>();
		}

		// Token: 0x060062B6 RID: 25270 RVA: 0x002772BD File Offset: 0x002754BD
		public bool TrySkip()
		{
			if (!this.CanSkip)
			{
				return false;
			}
			this._skipRequested = true;
			base.StopAllCoroutines();
			LongWayHomeCinematicSkip hints = this._hints;
			if (hints != null)
			{
				hints.Dispose();
			}
			base.StartCoroutine(this.SkipToGameplay());
			return true;
		}

		// Token: 0x060062B7 RID: 25271 RVA: 0x002772F5 File Offset: 0x002754F5
		private IEnumerator SkipToGameplay()
		{
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Handoff;
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			this._moving = false;
			this.StopArms(true);
			this.StopFootsteps();
			this.StopNightAmbience();
			this.SetFirstPerson(false);
			this.SetHairOnFloor(false);
			this.HideCrowd();
			if (!this._morningApplied)
			{
				this.SetClock(this._config.morningHour);
				yield return LongWayHomeUniversitySequence.RefreshLighting();
			}
			this.CloseRestroomDoor();
			this.HideStudents();
			this.UseLocomotion(this._config.escapeIdle, this._config.escapeRun, this._config.escapePlaybackSpeed);
			this._floorWeight = 0f;
			this._walkWeight = 0f;
			this.ClearFloorFit();
			this.ApplyBodyWeights();
			LongWayHomeGymShowerFaceDriver face = this._face;
			if (face != null)
			{
				face.Show(this._config.embarrassedFaceAnimation);
			}
			this.TeleportPlayer(this._config.handoffMark, Quaternion.Euler(0f, this._config.handoffYaw, 0f));
			this.BeginRunPopulation();
			yield return new WaitForSeconds(1.2f);
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x060062B8 RID: 25272 RVA: 0x00277304 File Offset: 0x00275504
		public void FailPreparation(string reason)
		{
			if (this.IsPreparedForReveal || !this.OwnsDestinationReveal)
			{
				return;
			}
			base.StopAllCoroutines();
			this.Fail(reason);
		}

		// Token: 0x060062B9 RID: 25273 RVA: 0x00277324 File Offset: 0x00275524
		public void PrepareForSceneExit()
		{
			if (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Done)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (this.Phase != LongWayHomeUniversitySequence.StoryPhase.Failed)
			{
				this.Phase = LongWayHomeUniversitySequence.StoryPhase.Handoff;
			}
			base.enabled = false;
		}

		// Token: 0x060062BA RID: 25274 RVA: 0x00277356 File Offset: 0x00275556
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			LongWayHomeUniversitySequence._activePresenter = this;
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBlack();
		}

		// Token: 0x060062BB RID: 25275 RVA: 0x00277375 File Offset: 0x00275575
		private IEnumerator Start()
		{
			this._config = LongWayHomeUniversityConfigSO.Load();
			if (this._config == null)
			{
				this.Fail("Missing Resources/LongWayHomeUniversityConfig.");
				yield break;
			}
			string text;
			if (!this._config.HasRequiredAssets(out text))
			{
				this.Fail("The university opening config is missing '" + text + "'.");
				yield break;
			}
			this._routine = new LongWayHomeFloorRoutine(this._config);
			LongWayHomeOpeningReadinessWait playerReadiness = new LongWayHomeOpeningReadinessWait(60f);
			while (!this.PlayerReady())
			{
				LongWayHomeOpeningReadinessWait.Result result = playerReadiness.ObservePlayer(base.gameObject.scene);
				if (result == LongWayHomeOpeningReadinessWait.Result.UpstreamFailed)
				{
					yield break;
				}
				if (result == LongWayHomeOpeningReadinessWait.Result.TimedOut)
				{
					this.Fail("The persistent player did not finish initialization in the University.");
					yield break;
				}
				yield return null;
			}
			yield return null;
			if (LongWayHomeOpeningReadinessWait.HasUpstreamFailure)
			{
				yield break;
			}
			LoadingReadinessWatchdog activityReadiness = new LoadingReadinessWatchdog(60f);
			while (LongWayHomeOpeningReadinessWait.HasUpstreamFailure || !PlayerActivityGate.TryAcquire("LongWayHomeUniversity#" + base.GetInstanceID().ToString(), this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._lease, false, null))
			{
				if (LongWayHomeOpeningReadinessWait.HasUpstreamFailure)
				{
					yield break;
				}
				if (activityReadiness.Tick(Time.realtimeSinceStartupAsDouble, 0f, 0, 0))
				{
					this.Fail("Another player activity still owns cinematic control.");
					yield break;
				}
				yield return null;
			}
			this._player = PlayerRegistry.Player;
			this._animator = this._player.GetComponent<Animator>();
			this._input = this._player.GetComponent<vThirdPersonInput>();
			this._body = this._player.GetComponent<Rigidbody>();
			this._style = this._player.GetComponentInChildren<MCStyleCustomizationService>(true);
			this._clothing = this._player.GetComponentInChildren<ClothingToggleController>(true);
			if (this._animator == null || !this._animator.isHuman || this._input == null || this._body == null || this._style == null)
			{
				this.Fail("The registered player is missing its production rig components.");
				yield break;
			}
			this._gameplayCamera = this._input.tpCamera;
			this._gameplayView = ((this._gameplayCamera != null) ? this._gameplayCamera.GetComponentInChildren<Camera>(true) : Camera.main);
			int boobsType = this.ReadBoobsType();
			this.CapturePlayerControl();
			this.CaptureArmsData();
			this.CaptureFace();
			this.SuppressCorePresentation();
			WearAdmireAnimation.SetPresentationSuppressed(this, true);
			if (!this.CaptureWardrobe())
			{
				this.Fail("The player's wardrobe could not be isolated for the cinematic.");
				yield break;
			}
			this.RemoveGarments();
			yield return null;
			this.HideScenePopulation();
			base.StartCoroutine(this.SettleArrivalPopulation());
			this.HoldClock(this._config.nightHour);
			string text2;
			if (!this.PrepareCrowd(out text2))
			{
				this.Fail(text2);
				yield break;
			}
			this.PrepareStudents();
			if (!this.ResolveRestroomDoor())
			{
				string text3 = "[LongWayHomeUniversity] No restroom door near ";
				Vector3 restroomDoor = this._config.restroomDoor;
				Debug.LogWarning(text3 + restroomDoor.ToString() + "; the escape continues without it.", this);
			}
			this.CreateCamera();
			this.BuildCielGraph();
			this.BuildArms(boobsType);
			this.UseArmsMoment(LongWayHomeUniversitySequence.ArmsMoment.Corridor);
			this.PrepareNightAmbience();
			Vector3[] array = this.CorridorStart();
			this.TeleportPlayer(array[0], LongWayHomeUniversitySequence.FacingRotation(array[0], array[1]));
			this.UseLocomotion(this._config.corridorIdle, this._config.corridorWalk, 1f);
			this._walkWeight = 1f;
			this._floorWeight = 0f;
			this.ApplyBodyWeights();
			this.BeginFace(this._config.corridorFaceAnimation);
			this.StartShots(this._config.corridorShots);
			yield return LongWayHomeUniversitySequence.RefreshLighting();
			yield return this.WarmPresentation();
			if (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Failed)
			{
				yield break;
			}
			this.IsPreparedForReveal = true;
			while ((LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning))
			{
				yield return null;
			}
			this._input.SetLockAllInput(true);
			this._input.SetLockCameraInput(true);
			this.IsReady = true;
			yield return this.PlayStory();
			yield break;
		}

		// Token: 0x060062BC RID: 25276 RVA: 0x00277384 File Offset: 0x00275584
		private bool PlayerReady()
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene || !SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.InitializedSceneName != base.gameObject.scene.name || PlayerRegistry.Player == null)
			{
				return false;
			}
			ClothInventorySyncer componentInChildren = PlayerRegistry.Player.GetComponentInChildren<ClothInventorySyncer>(true);
			return componentInChildren == null || componentInChildren.SyncCompleted;
		}

		// Token: 0x060062BD RID: 25277 RVA: 0x00277413 File Offset: 0x00275613
		private IEnumerator PlayStory()
		{
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Corridor;
			this._continueRequested = false;
			this._corridorStartedAt = Time.time;
			this.SetNightAmbience(true);
			base.StartCoroutine(this.FadeScreen(false, 0.8f));
			Coroutine walk = base.StartCoroutine(this.CorridorLoop());
			while (!this._continueRequested)
			{
				yield return null;
			}
			yield return this.FadeScreen(true, this._config.transitionSeconds);
			if (walk != null)
			{
				base.StopCoroutine(walk);
			}
			this._moving = false;
			this.StopFootsteps();
			this.UseArmsMoment(LongWayHomeUniversitySequence.ArmsMoment.None);
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Fall;
			this._floorBaseY = this._config.hallMark.y;
			this.TeleportPlayer(this._config.hallMark, Quaternion.Euler(0f, this._config.hallYaw, 0f));
			this._routine.StartFall();
			this._walkWeight = 0f;
			this._floorWeight = 1f;
			this.ApplyRoutine();
			this.ApplyBodyWeights();
			this.SetHairOnFloor(true);
			this._face.Show(this._config.floorFaceAnimation);
			this.StartShots(this._config.hallShots);
			yield return null;
			yield return null;
			yield return this.FadeScreen(false, this._config.transitionSeconds);
			while (this._routine.Current == LongWayHomeFloorRoutine.Segment.Fall)
			{
				yield return null;
			}
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Floor;
			this._continueRequested = false;
			while (!this._continueRequested)
			{
				yield return null;
			}
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.FallingAsleep;
			while (this._routine.Current != LongWayHomeFloorRoutine.Segment.Settle && this._routine.Current != LongWayHomeFloorRoutine.Segment.Asleep)
			{
				if (this._routine.CanSleep)
				{
					this._routine.RequestSleep();
				}
				yield return null;
			}
			this._face.BlendTo(this._config.sleepFaceAnimation, 0.6f);
			this.ShowFaceShot();
			yield return new WaitForSeconds(Mathf.Max(1.5f, this._config.sleepFaceAnimation.duration * 0.85f));
			yield return this.FadeScreen(true, 1.6f);
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Morning;
			this.SetNightAmbience(false);
			this.ResetLook();
			this._routine.StartAsleep();
			this.ApplyRoutine();
			this.HoldClock(this._config.morningHour);
			this._morningApplied = true;
			yield return LongWayHomeUniversitySequence.RefreshLighting();
			this.ShowCrowd();
			float blackUntil = Time.realtimeSinceStartup + this._config.nightPassesSeconds;
			while (Time.realtimeSinceStartup < blackUntil)
			{
				yield return null;
			}
			this.PlayBell();
			yield return new WaitForSecondsRealtime(1.1f);
			this._face.Show(this._config.wakeFaceAnimation);
			this.ShowFaceShot();
			yield return this.FadeScreen(false, 1.2f);
			yield return new WaitForSeconds(this._config.wakeBeforeGetUpSeconds);
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.GetUp;
			this._routine.StartGetUp();
			if (!this._orbiting)
			{
				this.SelectShot(0, false);
			}
			while (!this._routine.HasStood)
			{
				yield return null;
			}
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Realize;
			this.UseArmsMoment(LongWayHomeUniversitySequence.ArmsMoment.Escape);
			this._face.BlendTo(this._config.embarrassedFaceAnimation, 0.25f);
			this.PlayGasp();
			if (!this._orbiting)
			{
				this.ShowFaceShot();
			}
			yield return new WaitForSeconds(this._config.realizeSeconds);
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Escape;
			this.ShowStudents();
			this.SetHairOnFloor(false);
			this.UseLocomotion(this._config.escapeIdle, this._config.escapeRun, this._config.escapePlaybackSpeed);
			this.ActivateFixedShot(this._config.hallEscapeCamera, 50f);
			base.StartCoroutine(this.CutOutsideAtDoorway());
			Coroutine hallRun = base.StartCoroutine(this.RunAlong(this._config.hallEscapePath, true));
			Vector3 hallExit = this._config.hallEscapePath[this._config.hallEscapePath.Length - 1];
			float cutDistance = this._config.escapeMetersPerSecond * this._config.escapePlaybackSpeed * this._config.quickCutSeconds;
			while (this._moving && LongWayHomeUniversitySequence.PlanarDistance(this._player.position, hallExit) > cutDistance)
			{
				yield return null;
			}
			yield return this.FadeScreen(true, this._config.quickCutSeconds);
			if (hallRun != null)
			{
				base.StopCoroutine(hallRun);
			}
			this._moving = false;
			this.HideCrowd();
			Vector3[] restroomPath = this._config.restroomPath;
			this.TeleportPlayer(restroomPath[0], LongWayHomeUniversitySequence.FacingRotation(restroomPath[0], restroomPath[1]));
			this._floorWeight = 0f;
			this._walkWeight = 1f;
			this.ApplyBodyWeights();
			this.ActivateFixedShot(this._config.restroomCorridorCamera, 50f);
			base.StartCoroutine(this.FadeScreen(false, this._config.quickCutSeconds));
			yield return this.RunAlong(restroomPath, false);
			this.ActivateFixedShot(this._config.restroomInsideCamera, 52f);
			this.OpenRestroomDoor();
			yield return new WaitForSeconds(this._config.restroomDoorOpenSeconds);
			yield return this.MoveAlong(this._config.restroomEntryPath, this._config.escapeMetersPerSecond * this._config.escapePlaybackSpeed, false, true, 0f);
			yield return this.TurnTo(this._config.handoffYaw, 0.45f);
			this.CloseRestroomDoor();
			float doorShut = Mathf.Min(1.3f, this._config.restroomHoldSeconds);
			yield return new WaitForSeconds(doorShut);
			this.BeginRunPopulation();
			yield return new WaitForSeconds(this._config.restroomHoldSeconds - doorShut);
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Handoff;
			LongWayHomeCinematicSkip hints = this._hints;
			if (hints != null)
			{
				hints.Dispose();
			}
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			this.StopArms(true);
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x060062BE RID: 25278 RVA: 0x00277424 File Offset: 0x00275624
		private void CompleteHandoff()
		{
			this.ReleasePlayer();
			this.DestroyCrowd();
			this.DestroyStudents();
			if (this._gameplayCamera != null && this._gameplayCamera.isInit)
			{
				this._gameplayCamera.ResetAngleWithoutSmooth();
			}
			LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
			if (instance == null || !instance.BeginRunFromPoiPrologue())
			{
				Debug.LogWarning("[LongWayHomeUniversity] No prepared Long Way Home run took over the University.", this);
			}
			this.RestoreScenePopulation();
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Done;
			ScreenFadeController instance2 = ScreenFadeController.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.FadeIn(this._config.handoffFadeSeconds, null);
		}

		// Token: 0x060062BF RID: 25279 RVA: 0x002774B4 File Offset: 0x002756B4
		private void Update()
		{
			if (!this.IsPreparedForReveal && this.OwnsDestinationReveal && SceneManager.GetActiveScene() == base.gameObject.scene)
			{
				ScreenFadeController instance = ScreenFadeController.Instance;
				if (instance != null)
				{
					instance.SetBlack();
				}
			}
			if (this._captured)
			{
				this.HoldClockTick();
			}
			if (this.CanSkip)
			{
				if (this._hints == null)
				{
					this._hints = new LongWayHomeCinematicSkip(this);
				}
				this._hints.ShowWith(this.CurrentHints(), true);
				if (LongWayHomeCinematicSkip.TryConsume(this))
				{
					this.TrySkip();
					return;
				}
				this.ReadCinematicInput();
			}
			else if (this._captured)
			{
				EscapeInputCoordinator.TryConsume(this);
			}
			this.TickArms(Time.deltaTime);
			this.TickNightAmbience();
			this.TickCrowd(Time.deltaTime);
			this.TickStudents(Time.deltaTime);
			if (this._captured && this._routine != null && this._floorWeight > 0f)
			{
				this._routine.Tick(Time.deltaTime);
				this.ApplyRoutine();
				return;
			}
			if (this._floorFitApplied)
			{
				this.ClearFloorFit();
			}
		}

		// Token: 0x060062C0 RID: 25280 RVA: 0x002775C4 File Offset: 0x002757C4
		private void LateUpdate()
		{
			if (!this._captured || this._animator == null)
			{
				return;
			}
			for (int i = 1; i < this._animator.layerCount; i++)
			{
				this._animator.SetLayerWeight(i, (float)((i == this._faceLayer) ? 1 : 0));
			}
			if (this._faceAnimator != null && this._faceAnimator.GetInteger(this._faceParameter) != this._neutralFaceIndex)
			{
				this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
			}
			this.KeepClipDeformationsAtRest();
			this.UpdateCamera(false);
		}

		// Token: 0x060062C1 RID: 25281 RVA: 0x00277664 File Offset: 0x00275864
		private int ReadBoobsType()
		{
			AnimatorControllerParameter[] parameters = this._animator.parameters;
			int i = 0;
			while (i < parameters.Length)
			{
				AnimatorControllerParameter animatorControllerParameter = parameters[i];
				if (animatorControllerParameter.name == "BoobsType")
				{
					if (animatorControllerParameter.type != AnimatorControllerParameterType.Int)
					{
						return Mathf.RoundToInt(this._animator.GetFloat(animatorControllerParameter.nameHash));
					}
					return this._animator.GetInteger(animatorControllerParameter.nameHash);
				}
				else
				{
					i++;
				}
			}
			return 3;
		}

		// Token: 0x060062C2 RID: 25282 RVA: 0x002776D4 File Offset: 0x002758D4
		private void SetAnimatorFloat(string name, float value)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.name == name && animatorControllerParameter.type == AnimatorControllerParameterType.Float)
				{
					this._animator.SetFloat(animatorControllerParameter.nameHash, value);
					return;
				}
			}
		}

		// Token: 0x060062C3 RID: 25283 RVA: 0x0027772C File Offset: 0x0027592C
		private void TeleportPlayer(Vector3 position, Quaternion rotation)
		{
			this._player.SetPositionAndRotation(position, rotation);
			this._body.position = position;
			this._body.rotation = rotation;
			if (!this._body.isKinematic)
			{
				this._body.velocity = Vector3.zero;
				this._body.angularVelocity = Vector3.zero;
			}
			Physics.SyncTransforms();
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
			this.ResetLook();
			this.UpdateCamera(true);
		}

		// Token: 0x060062C4 RID: 25284 RVA: 0x002777D4 File Offset: 0x002759D4
		private IEnumerator WarmPresentation()
		{
			MagicaCloth[] cloths = this._player.GetComponentsInChildren<MagicaCloth>(true);
			float deadline = Time.realtimeSinceStartup + 12f;
			int settledFrames = 0;
			for (;;)
			{
				yield return null;
				bool flag = true;
				foreach (MagicaCloth magicaCloth in cloths)
				{
					if (magicaCloth != null && magicaCloth.isActiveAndEnabled && !magicaCloth.IsValid())
					{
						flag = false;
						break;
					}
				}
				settledFrames = (flag ? (settledFrames + 1) : 0);
				if (Time.realtimeSinceStartup >= deadline)
				{
					break;
				}
				if (settledFrames >= 6)
				{
					goto Block_6;
				}
			}
			this.Fail("The cinematic character physics did not finish preparing.");
			yield break;
			Block_6:
			float renderDeadline = Time.realtimeSinceStartup + 4f;
			int smoothFrames = 0;
			do
			{
				yield return null;
				smoothFrames = ((Time.unscaledDeltaTime <= 0.05f) ? (smoothFrames + 1) : 0);
			}
			while (smoothFrames < 6 && Time.realtimeSinceStartup < renderDeadline);
			yield break;
		}

		// Token: 0x060062C5 RID: 25285 RVA: 0x002777E4 File Offset: 0x002759E4
		private void CapturePlayerControl()
		{
			this._captured = true;
			this._inputEnabled = this._input.enabled;
			this._inputLock = this._input.lockInput;
			this._cameraLock = this._input.lockCameraInput;
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			bool flag;
			bool flag2;
			if (existingInstance != null && existingInstance.TryGetPreLoadingInputLocks(this._input, out flag, out flag2))
			{
				this._inputLock = flag;
				this._cameraLock = flag2;
			}
			if (this._gameplayCamera != null)
			{
				this._cameraFrozen = this._gameplayCamera.isFreezed;
			}
			this._input.SetLockAllInput(true);
			this._input.SetLockCameraInput(true);
			if (this._input.cc != null)
			{
				this._input.cc.input = Vector2.zero;
				this._input.cc.isSprinting = false;
			}
			this.SetAnimatorFloat("InputMagnitude", 0f);
			this.SetAnimatorFloat("InputVertical", 0f);
			this.SetAnimatorFloat("InputHorizontal", 0f);
			this._input.enabled = false;
			this._coverPose = this._player.GetComponentInChildren<CoverPoseController>(true);
			if (this._coverPose != null)
			{
				this._coverEnabled = this._coverPose.enabled;
				this._coverPose.enabled = false;
			}
			this._rootMotion = this._animator.applyRootMotion;
			this._culling = this._animator.cullingMode;
			this._animator.applyRootMotion = false;
			this._animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this._updateMode = this._animator.updateMode;
			this._animator.updateMode = AnimatorUpdateMode.Normal;
			this._layerWeights = new float[this._animator.layerCount];
			for (int i = 0; i < this._layerWeights.Length; i++)
			{
				this._layerWeights[i] = this._animator.GetLayerWeight(i);
			}
			this._kinematic = this._body.isKinematic;
			this._gravity = this._body.useGravity;
			this._constraints = this._body.constraints;
			this._interpolation = this._body.interpolation;
			this._body.interpolation = RigidbodyInterpolation.None;
			this._body.isKinematic = true;
			this._body.useGravity = false;
			this._body.constraints = RigidbodyConstraints.FreezeAll;
		}

		// Token: 0x060062C6 RID: 25286 RVA: 0x00277A54 File Offset: 0x00275C54
		private void CaptureFace()
		{
			this._emotion = this._player.GetComponentInChildren<PlayerEmotionController>(true);
			this._faceAnimator = ((this._emotion != null && this._emotion.PlayerAnimator != null) ? this._emotion.PlayerAnimator : this._animator);
			this._faceParameter = ((this._emotion != null && !string.IsNullOrEmpty(this._emotion.ExpressionParamName)) ? this._emotion.ExpressionParamName : "FaceExpressionIndex");
			this._neutralFaceIndex = ((this._emotion != null) ? this._emotion.NeutralExpressionIndex : 0);
			this._savedFaceIndex = this._faceAnimator.GetInteger(this._faceParameter);
			this._faceLayer = this._animator.GetLayerIndex("Face");
			if (this._emotion != null)
			{
				this._emotion.DisableEmotionLayersForAction();
				this._ownsEmotionOverride = true;
				this._ownsFaceOverride = !this._emotion.IsFacialExpressionOverriddenForAction;
				if (this._ownsFaceOverride)
				{
					this._emotion.DisableFacialExpressionForAction();
				}
				this._emotion.SetExpression(this._neutralFaceIndex);
				return;
			}
			this._faceAnimator.SetInteger(this._faceParameter, this._neutralFaceIndex);
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x00277BA4 File Offset: 0x00275DA4
		private void BeginFace(LongWayHomeFaceAnimationSO face)
		{
			Transform transform = this._player.Find("3D Model/Ciel_07_05_26_ass/Body");
			this._faceRenderer = ((transform != null) ? transform.GetComponent<SkinnedMeshRenderer>() : null);
			if (this._faceRenderer == null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
				{
					if (skinnedMeshRenderer.name == "Body")
					{
						this._faceRenderer = skinnedMeshRenderer;
						break;
					}
				}
			}
			if (this._face == null)
			{
				this._face = base.gameObject.AddComponent<LongWayHomeGymShowerFaceDriver>();
			}
			this._face.Begin((this._faceRenderer != null && this._faceRenderer.transform.parent != null) ? this._faceRenderer.transform.parent.gameObject : null, this._faceRenderer);
			this._face.Show(face);
			if (this._eyelids != null)
			{
				this._eyelids.Bind(this._faceRenderer);
			}
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x00277CBC File Offset: 0x00275EBC
		private void SuppressCorePresentation()
		{
			Scene sceneByName = SceneManager.GetSceneByName("_Core");
			if (!sceneByName.IsValid() || !sceneByName.isLoaded)
			{
				return;
			}
			foreach (GameObject gameObject in sceneByName.GetRootGameObjects())
			{
				foreach (Camera camera in gameObject.GetComponentsInChildren<Camera>(true))
				{
					this.Suppress(camera);
				}
				foreach (AudioListener audioListener in gameObject.GetComponentsInChildren<AudioListener>(true))
				{
					this.Suppress(audioListener);
				}
				foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>(true))
				{
					if (!GameplayHudVisibility.IsProtected(canvas.transform) && !(canvas.GetComponentInChildren<CielVoiceSubtitleUI>(true) != null))
					{
						this.Suppress(canvas);
					}
				}
				foreach (AdaptiveHeartbeatDriver adaptiveHeartbeatDriver in gameObject.GetComponentsInChildren<AdaptiveHeartbeatDriver>(true))
				{
					this.Suppress(adaptiveHeartbeatDriver);
				}
				foreach (HeartbeatController heartbeatController in gameObject.GetComponentsInChildren<HeartbeatController>(true))
				{
					this.Suppress(heartbeatController);
				}
				foreach (BreathController breathController in gameObject.GetComponentsInChildren<BreathController>(true))
				{
					this.Suppress(breathController);
				}
			}
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x00277E28 File Offset: 0x00276028
		private void Suppress(Behaviour component)
		{
			this._presentation.Add(new LongWayHomeUniversitySequence.BehaviourState
			{
				Component = component,
				Enabled = component.enabled
			});
			component.enabled = false;
		}

		// Token: 0x060062CA RID: 25290 RVA: 0x00277E68 File Offset: 0x00276068
		private bool CaptureWardrobe()
		{
			if (this._wardrobeSnapshot != null)
			{
				return true;
			}
			this._wardrobePresentation = WardrobeGameplayContext.BeginPresentationOverride(this);
			this._wearController = this._player.GetComponentInChildren<WearableUseController>(true);
			this._wearWasEnabled = this._wearController != null && this._wearController.enabled;
			if (this._wearController != null)
			{
				this._wearController.enabled = false;
			}
			bool flag;
			try
			{
				this._wardrobeSnapshot = this._style.CaptureWardrobeState(this._clothing);
				flag = this._wardrobeSnapshot != null;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
				this.RestoreWardrobe();
				flag = false;
			}
			return flag;
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x00277F20 File Offset: 0x00276120
		private void RemoveGarments()
		{
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this._style.PartDefinitions)
			{
				if (!(mcstylePartDefinitionSO == null) && !mcstylePartDefinitionSO.isBodyAppearance && mcstylePartDefinitionSO.partId.StartsWith("Outfit.", StringComparison.Ordinal) && (this._style.IsSlotEquipped(mcstylePartDefinitionSO.partId) || this._style.GetSpawnedRoot(mcstylePartDefinitionSO.partId) != null))
				{
					this._style.SetSlotEquipped(mcstylePartDefinitionSO.partId, false);
				}
			}
		}

		// Token: 0x060062CC RID: 25292 RVA: 0x00277FD0 File Offset: 0x002761D0
		private void RestoreWardrobe()
		{
			MCStyleCustomizationService.WardrobeSnapshot wardrobeSnapshot = this._wardrobeSnapshot;
			this._wardrobeSnapshot = null;
			WearableUseController wearController = this._wearController;
			this._wearController = null;
			try
			{
				if (this._style != null && wardrobeSnapshot != null)
				{
					this._style.RestoreWardrobeState(wardrobeSnapshot);
				}
			}
			finally
			{
				try
				{
					if (wearController != null)
					{
						wearController.enabled = this._wearWasEnabled;
					}
				}
				finally
				{
					IDisposable wardrobePresentation = this._wardrobePresentation;
					if (wardrobePresentation != null)
					{
						wardrobePresentation.Dispose();
					}
					this._wardrobePresentation = null;
				}
			}
		}

		// Token: 0x060062CD RID: 25293 RVA: 0x00278068 File Offset: 0x00276268
		private IEnumerator FadeScreen(bool toBlack, float seconds)
		{
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				yield return new WaitForSecondsRealtime(seconds);
				yield break;
			}
			bool done = false;
			if (toBlack)
			{
				instance.FadeOut(seconds, delegate
				{
					done = true;
				});
			}
			else
			{
				instance.FadeIn(seconds, delegate
				{
					done = true;
				});
			}
			float deadline = Time.realtimeSinceStartup + seconds + 2f;
			while (!done && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060062CE RID: 25294 RVA: 0x00278080 File Offset: 0x00276280
		private static Quaternion FacingRotation(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			vector.y = 0f;
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(vector);
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x002780BC File Offset: 0x002762BC
		private void Fail(string reason)
		{
			this.Phase = LongWayHomeUniversitySequence.StoryPhase.Failed;
			Debug.LogError("[LongWayHomeUniversity] " + reason, this);
			this.ReleasePlayer();
			this.RestoreScenePopulation();
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.SetClear();
			}
			LongWayHomeRuntimeController instance2 = LongWayHomeRuntimeController.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.FailPoiPrologue(reason);
		}

		// Token: 0x060062D0 RID: 25296 RVA: 0x0027810E File Offset: 0x0027630E
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			this.RestoreScenePopulation();
			if (LongWayHomeUniversitySequence._activePresenter == this)
			{
				LongWayHomeUniversitySequence._activePresenter = null;
			}
		}

		// Token: 0x060062D1 RID: 25297 RVA: 0x0027813D File Offset: 0x0027633D
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.ReleasePlayer();
			this.RestoreScenePopulation();
			this.DestroyCrowd();
			this.DestroyStudents();
			if (LongWayHomeUniversitySequence._activePresenter == this)
			{
				LongWayHomeUniversitySequence._activePresenter = null;
			}
		}

		// Token: 0x060062D2 RID: 25298 RVA: 0x00278174 File Offset: 0x00276374
		private void ReleasePlayer()
		{
			LongWayHomeCinematicSkip hints = this._hints;
			if (hints != null)
			{
				hints.Dispose();
			}
			this._hints = null;
			this.IsReady = false;
			this.ReleaseArms();
			this.StopFootsteps();
			this.StopNightAmbience();
			this.SetFirstPerson(false);
			this.SetHairOnFloor(false);
			LongWayHomeGymShowerFaceDriver face = this._face;
			if (face != null)
			{
				face.Release();
			}
			if (this._cielGraph.IsValid())
			{
				this._cielGraph.Destroy();
			}
			this.DisableCamera();
			this.HideCrowd();
			this.HideStudents();
			this.ReleasePopulationFocus();
			this.ReleaseRestroomDoorMotion();
			this.ReleaseClock();
			this.RestoreWardrobe();
			WearAdmireAnimation.SetPresentationSuppressed(this, false);
			if (this._faceAnimator != null)
			{
				if (this._ownsFaceOverride && this._emotion != null)
				{
					this._emotion.RestoreFacialExpression();
				}
				else
				{
					this._faceAnimator.SetInteger(this._faceParameter, this._savedFaceIndex);
				}
			}
			this._ownsFaceOverride = false;
			this._faceAnimator = null;
			if (this._captured)
			{
				if (this._animator != null)
				{
					this._animator.applyRootMotion = this._rootMotion;
					this._animator.cullingMode = this._culling;
					this._animator.updateMode = this._updateMode;
					int num = 1;
					while (num < this._animator.layerCount && num < this._layerWeights.Length)
					{
						this._animator.SetLayerWeight(num, this._layerWeights[num]);
						num++;
					}
					if (this._animator.isActiveAndEnabled && this._animator.isInitialized)
					{
						this._animator.Update(0f);
					}
				}
				this.RestoreClipDeformations();
				if (this._body != null)
				{
					this._body.constraints = this._constraints;
					this._body.useGravity = this._gravity;
					this._body.isKinematic = this._kinematic;
					this._body.interpolation = this._interpolation;
					if (!this._body.isKinematic)
					{
						this._body.velocity = Vector3.zero;
						this._body.angularVelocity = Vector3.zero;
					}
				}
				if (this._coverPose != null)
				{
					this._coverPose.enabled = this._coverEnabled;
				}
				if (this._input != null)
				{
					LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
					bool flag2;
					bool flag3;
					bool flag = existingInstance != null && existingInstance.TryGetPreLoadingInputLocks(this._input, out flag2, out flag3);
					this._input.SetLockAllInput(flag || this._inputLock);
					this._input.SetLockCameraInput(flag || this._cameraLock);
					this._input.enabled = this._inputEnabled;
				}
				if (this._gameplayCamera != null)
				{
					this._gameplayCamera.isFreezed = this._cameraFrozen;
				}
				this._captured = false;
			}
			if (this._ownsEmotionOverride && this._emotion != null)
			{
				this._emotion.RestoreEmotionLayers();
			}
			this._ownsEmotionOverride = false;
			foreach (LongWayHomeUniversitySequence.BehaviourState behaviourState in this._presentation)
			{
				if (behaviourState.Component != null)
				{
					behaviourState.Component.enabled = behaviourState.Enabled;
				}
			}
			this._presentation.Clear();
			PlayerActivityLease lease = this._lease;
			if (lease != null)
			{
				lease.Dispose();
			}
			this._lease = null;
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x060062D3 RID: 25299 RVA: 0x00278508 File Offset: 0x00276708
		private bool CanToggleView
		{
			get
			{
				return this.Phase != LongWayHomeUniversitySequence.StoryPhase.Loading && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Handoff && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Done && this.Phase != LongWayHomeUniversitySequence.StoryPhase.Failed;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x060062D4 RID: 25300 RVA: 0x00278535 File Offset: 0x00276735
		private bool CanSwitchCamera
		{
			get
			{
				return !this._pov && this._shots != null && this._shots.Length > 1 && this._shotMode == LongWayHomeUniversitySequence.ShotMode.Body && ((this.Phase == LongWayHomeUniversitySequence.StoryPhase.Corridor && !this._continueRequested) || this.OnHallFloor);
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x060062D5 RID: 25301 RVA: 0x00278575 File Offset: 0x00276775
		private bool CanChangePosition
		{
			get
			{
				return this.Phase == LongWayHomeUniversitySequence.StoryPhase.Floor && this._routine != null && this._routine.CanSwitch;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x060062D6 RID: 25302 RVA: 0x00278598 File Offset: 0x00276798
		private bool CanContinue
		{
			get
			{
				return !this._continueRequested && ((this.Phase == LongWayHomeUniversitySequence.StoryPhase.Floor && this._routine != null) || (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Corridor && this.IsReady && Time.time >= this._corridorStartedAt + 1f));
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x060062D7 RID: 25303 RVA: 0x002785EB File Offset: 0x002767EB
		private bool CanLookAround
		{
			get
			{
				return this._pov && this.CanToggleView;
			}
		}

		// Token: 0x060062D8 RID: 25304 RVA: 0x00278600 File Offset: 0x00276800
		private IReadOnlyList<LongWayHomeCinematicSkip.Hint> CurrentHints()
		{
			this._hintList.Clear();
			if (this.CanPickCorridorPose)
			{
				for (int i = 0; i < LongWayHomeUniversitySequence.CorridorPoses.Length; i++)
				{
					if (this.HasArmsSlot(LongWayHomeUniversitySequence.CorridorPoses[i]))
					{
						this._hintList.Add(new LongWayHomeCinematicSkip.Hint(LongWayHomeUniversitySequence.CorridorPoseKeyLabels[i], LongWayHomeUniversitySequence.PoseAction(LongWayHomeUniversitySequence.CorridorPoses[i]), ControllerBindings.Label(LongWayHomeUniversitySequence.CorridorPoseButtons[i])));
					}
				}
			}
			if (this.CanToggleCover)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("RMB", "Cover", ControllerInput.ForKey(KeyCode.Mouse1)));
			}
			if (this.CanCoverLowerBody)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("LMB", "Cover lower body", ControllerInput.ForKey(KeyCode.Mouse0)));
			}
			if (this.CanChangePosition)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("A / D", "Change position", "LB / RB"));
			}
			if (this.CanContinue)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Space", "Continue", null));
			}
			if (this.CanOrbit)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Mouse", "Rotate camera", "RS"));
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Wheel", "Zoom", "LT + RS"));
			}
			if (this.CanLookAround)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Mouse", "Look around", "RS"));
			}
			if (this.CanSwitchCamera)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("F", "Switch camera", null));
			}
			if (this.CanToggleView)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("V", this._pov ? "Third person" : "First person", null));
			}
			return this._hintList;
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x002787DC File Offset: 0x002769DC
		private void ReadCinematicInput()
		{
			if (Time.timeScale <= 0f || TextInputFocus.HasFocusedField || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			this.ReadLookInput();
			this.ReadArmsInput();
			if (this.CanChangePosition && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || LongWayHomeUniversitySequence.Controller(KeyCode.Mouse0) || LongWayHomeUniversitySequence.Controller(KeyCode.Mouse1)))
			{
				this._routine.RequestSwitch();
			}
			if (this.CanContinue && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || LongWayHomeUniversitySequence.Controller(KeyCode.Space)))
			{
				if (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Floor)
				{
					this._routine.RequestSleep();
				}
				this._continueRequested = true;
			}
			if (this.CanSwitchCamera && (Input.GetKeyDown(KeyCode.F) || LongWayHomeUniversitySequence.Controller(KeyCode.F)))
			{
				this.NextShot();
			}
			if (this.CanToggleView && (Input.GetKeyDown(KeyCode.V) || LongWayHomeUniversitySequence.Controller(KeyCode.V)))
			{
				this.ToggleFirstPerson();
			}
		}

		// Token: 0x060062DA RID: 25306 RVA: 0x002788E5 File Offset: 0x00276AE5
		private static string PoseAction(LongWayHomeUniversitySequence.ArmsPose pose)
		{
			if (pose == LongWayHomeUniversitySequence.ArmsPose.HoldBoobs)
			{
				return "Hold Boobs";
			}
			if (pose == LongWayHomeUniversitySequence.ArmsPose.ShyPose)
			{
				return "Shy Pose";
			}
			if (pose != LongWayHomeUniversitySequence.ArmsPose.Cover)
			{
				return "Stimulation";
			}
			return "Cover";
		}

		// Token: 0x060062DB RID: 25307 RVA: 0x0027890C File Offset: 0x00276B0C
		private void ReadArmsInput()
		{
			bool isActive = ControllerInput.IsActive;
			if (this.CanPickCorridorPose)
			{
				for (int i = 0; i < LongWayHomeUniversitySequence.CorridorPoses.Length; i++)
				{
					if (Input.GetKeyDown(LongWayHomeUniversitySequence.CorridorPoseKeys[i]) || LongWayHomeUniversitySequence.PadButton(LongWayHomeUniversitySequence.CorridorPoseButtons[i]))
					{
						this.SelectCorridorPose(LongWayHomeUniversitySequence.CorridorPoses[i]);
						break;
					}
				}
			}
			if (!this.CanToggleCover)
			{
				this._armsPressed = false;
			}
			else
			{
				if (isActive ? (ControllerInput.GetKeyDown(KeyCode.Mouse1) && !ControllerInput.UiInputConsumedThisFrame) : Input.GetMouseButtonDown(1))
				{
					this._armsPressed = true;
					if (isActive)
					{
						ControllerInput.ConsumeUIInput();
					}
				}
				if (this._armsPressed && (isActive ? ControllerInput.GetKeyUp(KeyCode.Mouse1) : Input.GetMouseButtonUp(1)))
				{
					this._armsPressed = false;
					this.ToggleCover();
				}
			}
			if (this.CanCoverLowerBody && (isActive ? LongWayHomeUniversitySequence.Controller(KeyCode.Mouse0) : Input.GetMouseButtonDown(0)))
			{
				this.ToggleLowerBody();
			}
		}

		// Token: 0x060062DC RID: 25308 RVA: 0x002789FC File Offset: 0x00276BFC
		private void ReadLookInput()
		{
			if (!this.CanOrbit && !this.CanLookAround)
			{
				return;
			}
			bool isActive = ControllerInput.IsActive;
			float num;
			float num2;
			float num3;
			if (this._input != null)
			{
				num = this._input.rotateCameraXInput.GetAxis();
				num2 = this._input.rotateCameraYInput.GetAxis();
				num3 = (isActive ? (ControllerGameplayBridge.AllowsAxes ? ControllerInput.Scroll : 0f) : this._input.cameraZoomInput.GetAxis());
				if (this._input.invertCameraInputHorizontal)
				{
					num = -num;
				}
				if (this._input.invertCameraInputVertical)
				{
					num2 = -num2;
				}
			}
			else
			{
				num = Input.GetAxis("Mouse X");
				num2 = Input.GetAxis("Mouse Y");
				num3 = Input.GetAxis("Mouse ScrollWheel");
			}
			if (!isActive)
			{
				Vector2 vector = GameSettingsService.ApplyMouseLookInversion(new Vector2(num, num2));
				num = vector.x;
				num2 = vector.y;
			}
			Vector3 vector2 = new Vector3(num, num2, num3);
			if (vector2 != Vector3.zero)
			{
				this.AddLookInput(vector2, isActive);
			}
		}

		// Token: 0x060062DD RID: 25309 RVA: 0x0026B4CA File Offset: 0x002696CA
		private static bool Controller(KeyCode key)
		{
			if (!ControllerInput.IsActive || ControllerInput.UiInputConsumedThisFrame || !ControllerInput.GetKeyDown(key))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x00278AF9 File Offset: 0x00276CF9
		private static bool PadButton(ControllerButton button)
		{
			if (!ControllerInput.IsActive || ControllerInput.Modifier || ControllerInput.UiInputConsumedThisFrame || !ControllerInput.GetButtonDown(button))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x00278B20 File Offset: 0x00276D20
		private void PrepareNightAmbience()
		{
			if (this._config.nightAmbience == null || this._night != null)
			{
				return;
			}
			this._nightA = this.CreateAudioSource("Audio - Night Ambience A");
			this._nightB = this.CreateAudioSource("Audio - Night Ambience B");
			this._night = new CrossfadeLoopPlayer(this._nightA, this._nightB, this._config.nightAmbience, 0.3f, 0f, 0f);
		}

		// Token: 0x060062E0 RID: 25312 RVA: 0x00278B9C File Offset: 0x00276D9C
		private void SetNightAmbience(bool on)
		{
			this._nightTarget = (on ? 1f : 0f);
			if (on && this._night != null && !this._night.IsPlaying)
			{
				this._night.Play();
			}
		}

		// Token: 0x060062E1 RID: 25313 RVA: 0x00278BD8 File Offset: 0x00276DD8
		private void TickNightAmbience()
		{
			if (this._night == null)
			{
				return;
			}
			this._nightLevel = Mathf.MoveTowards(this._nightLevel, this._nightTarget, Time.unscaledDeltaTime / 1.5f);
			this._night.Tick(this._nightLevel * this._config.nightAmbienceVolume);
			if (this._nightLevel <= 0f && this._nightTarget <= 0f && this._night.IsPlaying)
			{
				this._night.Stop();
			}
		}

		// Token: 0x060062E2 RID: 25314 RVA: 0x00278C60 File Offset: 0x00276E60
		private void StopNightAmbience()
		{
			this._nightTarget = (this._nightLevel = 0f);
			CrossfadeLoopPlayer night = this._night;
			if (night == null)
			{
				return;
			}
			night.Stop();
		}

		// Token: 0x060062E3 RID: 25315 RVA: 0x00278C94 File Offset: 0x00276E94
		private AudioSource CreateAudioSource(string name)
		{
			GameObject gameObject = new GameObject(name);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.dopplerLevel = 0f;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Sfx);
			return audioSource;
		}

		// Token: 0x060062E4 RID: 25316 RVA: 0x00278CEC File Offset: 0x00276EEC
		private void BeginFootsteps()
		{
			this._stepsArmed = false;
			AudioClip[] footstepClips = this._config.footstepClips;
			if (this._player == null || !this._locoMove.IsValid<AnimationClipPlayable>() || footstepClips == null || footstepClips.Length == 0)
			{
				return;
			}
			if (this._steps == null)
			{
				this._steps = this.CreateAudioSource("Audio - Ciel Footsteps");
			}
			foreach (AudioClip audioClip in footstepClips)
			{
				if (audioClip != null && audioClip.loadState == AudioDataLoadState.Unloaded)
				{
					audioClip.LoadAudioData();
				}
			}
			AnimationClip animationClip = this._locoMove.GetAnimationClip();
			if (animationClip != this._stepClip)
			{
				this._stepClip = animationClip;
				this._contacts = LongWayHomeUniversitySequence.FootContacts(animationClip);
			}
			this._previousStepTime = this._locoMove.GetTime<AnimationClipPlayable>();
			this._previousStepPosition = this._player.position;
			this._stepsArmed = true;
		}

		// Token: 0x060062E5 RID: 25317 RVA: 0x00278DD4 File Offset: 0x00276FD4
		private void TickFootsteps(bool running)
		{
			if (!this._stepsArmed)
			{
				return;
			}
			if (!this._moving || !this._locoMove.IsValid<AnimationClipPlayable>() || this._stepClip == null)
			{
				this._stepsArmed = false;
				return;
			}
			double previousStepTime = this._previousStepTime;
			double time = this._locoMove.GetTime<AnimationClipPlayable>();
			Vector3 vector = this._player.position - this._previousStepPosition;
			vector.y = 0f;
			this._previousStepTime = time;
			this._previousStepPosition = this._player.position;
			if ((1f - Mathf.Clamp01(this._floorWeight)) * Mathf.Clamp01(this._walkWeight) <= 0.25f || vector.sqrMagnitude <= 1E-08f)
			{
				return;
			}
			bool flag = false;
			foreach (float num in this._contacts)
			{
				flag |= LongWayHomeUniversitySequence.CrossedFootContact(previousStepTime, time, this._stepClip.length, num);
			}
			if (!flag)
			{
				return;
			}
			AudioClip[] footstepClips = this._config.footstepClips;
			for (int j = 0; j < footstepClips.Length; j++)
			{
				this._lastStep = (this._lastStep + 1) % footstepClips.Length;
				if (!(footstepClips[this._lastStep] == null))
				{
					this._steps.pitch = 1f + Random.Range(-0.08f, 0.08f);
					this._steps.PlayOneShot(footstepClips[this._lastStep], running ? this._config.runFootstepVolume : this._config.walkFootstepVolume);
					return;
				}
			}
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x00278F6C File Offset: 0x0027716C
		private void StopFootsteps()
		{
			this._stepsArmed = false;
			if (this._steps != null)
			{
				this._steps.Stop();
			}
		}

		// Token: 0x060062E7 RID: 25319 RVA: 0x00278F90 File Offset: 0x00277190
		public static float[] FootContacts(AnimationClip clip)
		{
			if (clip == null || clip.length <= 0f)
			{
				return LongWayHomeUniversitySequence.FallbackContacts;
			}
			AnimationEvent[] events = clip.events;
			int num = 0;
			AnimationEvent[] array = events;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].functionName == "PlayFootstep")
				{
					num++;
				}
			}
			if (num == 0)
			{
				return LongWayHomeUniversitySequence.FallbackContacts;
			}
			float[] array2 = new float[num];
			int num2 = 0;
			foreach (AnimationEvent animationEvent in events)
			{
				if (animationEvent.functionName == "PlayFootstep")
				{
					array2[num2++] = Mathf.Repeat(animationEvent.time / clip.length, 1f);
				}
			}
			return array2;
		}

		// Token: 0x060062E8 RID: 25320 RVA: 0x00279054 File Offset: 0x00277254
		public static bool CrossedFootContact(double previous, double current, float duration, float phase)
		{
			if (duration <= 0f || double.IsNaN(previous) || double.IsNaN(current) || double.IsInfinity(previous) || double.IsInfinity(current))
			{
				return false;
			}
			if (current <= previous || current - previous > (double)duration)
			{
				return false;
			}
			double num = (double)phase - Math.Floor((double)phase);
			return Math.Floor(current / (double)duration - num) > Math.Floor(previous / (double)duration - num);
		}

		// Token: 0x060062E9 RID: 25321 RVA: 0x002790BC File Offset: 0x002772BC
		private void PrepareStudents()
		{
			LongWayHomeUniversityBystander.Clips clips = new LongWayHomeUniversityBystander.Clips
			{
				idleMale = this._config.bystanderIdleMale,
				idleFemale = this._config.bystanderIdleFemale,
				walkMale = this._config.bystanderWalkMale,
				walkFemale = this._config.bystanderWalkFemale,
				phoneWalk = this._config.bystanderTextingWalk,
				photo = this._config.bystanderPhoto,
				video = this._config.bystanderVideo,
				surprisedFace = this._config.bystanderSurprisedFace,
				shutter = this._config.npcShutter,
				voices = this._config.bystanderVoices
			};
			if (this._config.escapeBystanders == null)
			{
				return;
			}
			foreach (LongWayHomeUniversityConfigSO.BystanderMark bystanderMark in this._config.escapeBystanders)
			{
				if (bystanderMark != null)
				{
					string text;
					LongWayHomeUniversityBystander longWayHomeUniversityBystander = LongWayHomeUniversityBystander.Create(base.gameObject.scene, clips, bystanderMark.prefer, this._usedCivilians, bystanderMark.reaction, bystanderMark.texting, bystanderMark.voice, out text);
					if (longWayHomeUniversityBystander == null)
					{
						Debug.LogWarning("[LongWayHomeUniversity] " + text, this);
						return;
					}
					this._usedCivilians.Add(longWayHomeUniversityBystander.ResourceName);
					this._students.Add(longWayHomeUniversityBystander);
					this._studentMarks.Add(bystanderMark);
				}
			}
		}

		// Token: 0x060062EA RID: 25322 RVA: 0x00279224 File Offset: 0x00277424
		private void ShowStudents()
		{
			for (int i = 0; i < this._students.Count; i++)
			{
				LongWayHomeUniversityConfigSO.BystanderMark bystanderMark = this._studentMarks[i];
				this._students[i].Show(bystanderMark.from, bystanderMark.to, bystanderMark.yaw, this._config.bystanderWalkMetersPerSecond);
			}
			this._studentsVisible = this._students.Count > 0;
		}

		// Token: 0x060062EB RID: 25323 RVA: 0x00279298 File Offset: 0x00277498
		private void TickStudents(float deltaTime)
		{
			if (!this._studentsVisible || this._player == null)
			{
				return;
			}
			foreach (LongWayHomeUniversityBystander longWayHomeUniversityBystander in this._students)
			{
				if (longWayHomeUniversityBystander != null && longWayHomeUniversityBystander.IsValid)
				{
					longWayHomeUniversityBystander.Tick(deltaTime, this._player, this._config.bystanderReactDistance);
				}
			}
		}

		// Token: 0x060062EC RID: 25324 RVA: 0x00279320 File Offset: 0x00277520
		private void HideStudents()
		{
			foreach (LongWayHomeUniversityBystander longWayHomeUniversityBystander in this._students)
			{
				if (longWayHomeUniversityBystander != null)
				{
					longWayHomeUniversityBystander.Hide();
				}
			}
			this._studentsVisible = false;
		}

		// Token: 0x060062ED RID: 25325 RVA: 0x00279380 File Offset: 0x00277580
		private void DestroyStudents()
		{
			foreach (LongWayHomeUniversityBystander longWayHomeUniversityBystander in this._students)
			{
				if (longWayHomeUniversityBystander != null)
				{
					longWayHomeUniversityBystander.Destroy();
				}
			}
			this._students.Clear();
			this._studentMarks.Clear();
			this._studentsVisible = false;
		}

		// Token: 0x060062EE RID: 25326 RVA: 0x002793F4 File Offset: 0x002775F4
		private IEnumerator SettleArrivalPopulation()
		{
			NPCZoneManager zones = NPCZoneManager.Instance;
			if (zones == null || zones.IsInitialPopulationComplete || !zones.TrySetPreparationFocus(this, this._config.handoffMark))
			{
				yield break;
			}
			this._populationFocus = true;
			float deadline = Time.realtimeSinceStartup + 30f;
			while (zones != null && !zones.IsInitialPopulationComplete && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			this.ReleasePopulationFocus();
			yield break;
		}

		// Token: 0x060062EF RID: 25327 RVA: 0x00279404 File Offset: 0x00277604
		private void BeginRunPopulation()
		{
			this.RestoreScenePopulation();
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance != null && instance.TrySetPreparationFocus(this, this._config.handoffMark))
			{
				this._populationFocus = true;
			}
		}

		// Token: 0x060062F0 RID: 25328 RVA: 0x00279441 File Offset: 0x00277641
		private void ReleasePopulationFocus()
		{
			if (!this._populationFocus)
			{
				return;
			}
			this._populationFocus = false;
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.ReleasePreparationFocus(this);
		}

		// Token: 0x060062F1 RID: 25329 RVA: 0x00279464 File Offset: 0x00277664
		private void HideScenePopulation()
		{
			if (this._populationHidden)
			{
				return;
			}
			this._populationHidden = true;
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance != null)
			{
				this._savedDensity = instance.DensityMultiplier;
				instance.SetDensityMultiplier(0f);
				instance.DespawnAllNPCs();
			}
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (GameObject gameObject in base.gameObject.scene.GetRootGameObjects())
			{
				foreach (SeatedRandomReaction seatedRandomReaction in gameObject.GetComponentsInChildren<SeatedRandomReaction>(false))
				{
					this.HideSeated(LongWayHomeUniversitySequence.SeatedRoot(seatedRandomReaction.transform), hashSet);
				}
				foreach (Transform transform in gameObject.GetComponentsInChildren<Transform>(false))
				{
					if (transform.name.StartsWith("NPC_StaticSeated_", StringComparison.Ordinal))
					{
						this.HideSeated(LongWayHomeUniversitySequence.SeatedRoot(transform), hashSet);
					}
				}
			}
			Debug.Log(string.Format("[LongWayHomeUniversity] Campus emptied for the night: {0} seated students hidden, ", this._hiddenSeated.Count) + string.Format("walkers despawned (density {0:0.##} -> 0).", this._savedDensity), this);
		}

		// Token: 0x060062F2 RID: 25330 RVA: 0x00279598 File Offset: 0x00277798
		private static Transform SeatedRoot(Transform transform)
		{
			Transform transform2 = transform;
			Transform transform3 = transform;
			while (transform3 != null)
			{
				if (transform3.name.StartsWith("NPC_StaticSeated_", StringComparison.Ordinal))
				{
					transform2 = transform3;
				}
				transform3 = transform3.parent;
			}
			return transform2;
		}

		// Token: 0x060062F3 RID: 25331 RVA: 0x002795D4 File Offset: 0x002777D4
		private void HideSeated(Transform root, HashSet<GameObject> seen)
		{
			if (root == null || !root.gameObject.activeSelf || !seen.Add(root.gameObject))
			{
				return;
			}
			root.gameObject.SetActive(false);
			this._hiddenSeated.Add(root.gameObject);
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x00279624 File Offset: 0x00277824
		private void RestoreScenePopulation()
		{
			if (!this._populationHidden)
			{
				return;
			}
			this._populationHidden = false;
			foreach (GameObject gameObject in this._hiddenSeated)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			this._hiddenSeated.Clear();
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance != null)
			{
				instance.SetDensityMultiplier(this._savedDensity);
			}
		}

		// Token: 0x060062F5 RID: 25333 RVA: 0x002796B8 File Offset: 0x002778B8
		private void HoldClock(float hour)
		{
			if (this._azure == null)
			{
				this._azure = Object.FindObjectOfType<AzureCoreSystem>();
			}
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			AzureTimeSystem timeSystem = this._azure.timeSystem;
			if (!this._clockHeld)
			{
				this._savedDayLength = timeSystem.dayLength;
				this._savedStartTime = timeSystem.startTime;
				this._clockHeld = true;
			}
			this._heldHour = hour;
			timeSystem.dayLength = 0f;
			timeSystem.startTime = hour;
			timeSystem.timeline = hour;
		}

		// Token: 0x060062F6 RID: 25334 RVA: 0x00279750 File Offset: 0x00277950
		private void HoldClockTick()
		{
			if (!this._clockHeld || this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			if (!Mathf.Approximately(this._azure.timeSystem.timeline, this._heldHour))
			{
				this._azure.timeSystem.timeline = this._heldHour;
			}
		}

		// Token: 0x060062F7 RID: 25335 RVA: 0x002797B4 File Offset: 0x002779B4
		private void ReleaseClock()
		{
			if (!this._clockHeld)
			{
				return;
			}
			this._clockHeld = false;
			if (this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			AzureTimeSystem timeSystem = this._azure.timeSystem;
			timeSystem.startTime = (this._morningApplied ? this._heldHour : this._savedStartTime);
			timeSystem.dayLength = ((this._savedDayLength > 0f) ? this._savedDayLength : 24f);
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x00279833 File Offset: 0x00277A33
		private void SetClock(float hour)
		{
			this.HoldClock(hour);
			if (hour >= this._config.morningHour - 0.01f)
			{
				this._morningApplied = true;
			}
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x00279857 File Offset: 0x00277A57
		private static IEnumerator RefreshLighting()
		{
			EnvironmentLightingRefresh.ForceRequestForActiveScene();
			yield return EnvironmentLightingRefresh.WaitUntilReady(12f);
			yield break;
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x00279860 File Offset: 0x00277A60
		private bool PrepareCrowd(out string error)
		{
			error = null;
			LongWayHomeGymShowerPhotographer.Clips clips = new LongWayHomeGymShowerPhotographer.Clips
			{
				idle = this._config.npcIdle,
				walk = this._config.npcWalk,
				run = this._config.npcRun,
				photo = this._config.npcPhoto,
				shutter = this._config.npcShutter
			};
			Vector3[] crowdMarks = this._config.crowdMarks;
			int i = 0;
			while (i < crowdMarks.Length)
			{
				string text;
				LongWayHomeGymShowerPhotographer longWayHomeGymShowerPhotographer = LongWayHomeGymShowerPhotographer.Create(base.gameObject.scene, clips, false, this._usedCivilians, "Lecture Hall Student", out text);
				if (longWayHomeGymShowerPhotographer == null)
				{
					if (this._crowd.Count <= 0)
					{
						error = text;
						return false;
					}
					break;
				}
				else
				{
					this._usedCivilians.Add(longWayHomeGymShowerPhotographer.ResourceName);
					this._crowd.Add(longWayHomeGymShowerPhotographer);
					i++;
				}
			}
			return true;
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x00279944 File Offset: 0x00277B44
		private void ShowCrowd()
		{
			Vector3 vector = this.BodyCenter();
			int num = 0;
			while (num < this._crowd.Count && num < this._config.crowdMarks.Length)
			{
				Vector3 vector2 = this._config.crowdMarks[num];
				this._crowd[num].Show(vector2, LongWayHomeUniversitySequence.FacingRotation(vector2, vector));
				this._crowd[num].SetPose(LongWayHomeGymShowerPhotographer.Pose.Photo, 0.01f);
				this._crowd[num].SetPoseTime(LongWayHomeGymShowerPhotographer.Pose.Photo, Random.Range(0f, 1.6f));
				num++;
			}
			this._crowdVisible = true;
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x002799EC File Offset: 0x00277BEC
		private void TickCrowd(float deltaTime)
		{
			if (!this._crowdVisible)
			{
				return;
			}
			Vector3 vector = ((this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape) ? this._player.position : this.BodyCenter());
			foreach (LongWayHomeGymShowerPhotographer longWayHomeGymShowerPhotographer in this._crowd)
			{
				if (longWayHomeGymShowerPhotographer != null && longWayHomeGymShowerPhotographer.IsValid)
				{
					if (this.Phase == LongWayHomeUniversitySequence.StoryPhase.GetUp || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Realize || this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape)
					{
						longWayHomeGymShowerPhotographer.TurnTowards(vector, 120f, deltaTime);
					}
					longWayHomeGymShowerPhotographer.Tick(deltaTime);
				}
			}
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x00279A9C File Offset: 0x00277C9C
		private void HideCrowd()
		{
			foreach (LongWayHomeGymShowerPhotographer longWayHomeGymShowerPhotographer in this._crowd)
			{
				if (longWayHomeGymShowerPhotographer != null)
				{
					longWayHomeGymShowerPhotographer.Hide();
				}
			}
			this._crowdVisible = false;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x00279AFC File Offset: 0x00277CFC
		private void DestroyCrowd()
		{
			foreach (LongWayHomeGymShowerPhotographer longWayHomeGymShowerPhotographer in this._crowd)
			{
				if (longWayHomeGymShowerPhotographer != null)
				{
					longWayHomeGymShowerPhotographer.Destroy();
				}
			}
			this._crowd.Clear();
			this._crowdVisible = false;
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x00279B64 File Offset: 0x00277D64
		private Vector3 BodyCenter()
		{
			if (this._animator == null)
			{
				return this._config.hallMark;
			}
			Vector3 position = this.Bone(HumanBodyBones.Hips).position;
			position.y = this._floorBaseY;
			return position;
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x00279BA8 File Offset: 0x00277DA8
		private void SetHairOnFloor(bool onFloor)
		{
			if (!onFloor)
			{
				foreach (MagicaCloth magicaCloth in this._hairCloths)
				{
					if (!(magicaCloth == null))
					{
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList.Remove(this._hairFloor);
						magicaCloth.SetParameterChange();
					}
				}
				this._hairCloths.Clear();
				if (this._hairFloor != null)
				{
					Object.Destroy(this._hairFloor.gameObject);
				}
				this._hairFloor = null;
				return;
			}
			if (this._config == null || !this._config.hairRestsOnFloor || this._hairFloor != null || this._player == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Lecture Hall Floor (hair)");
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.SetPositionAndRotation(new Vector3(this._config.hallMark.x, this._floorBaseY, this._config.hallMark.z), Quaternion.identity);
			this._hairFloor = gameObject.AddComponent<MagicaPlaneCollider>();
			foreach (MagicaCloth magicaCloth2 in this._player.GetComponentsInChildren<MagicaCloth>(false))
			{
				if (!(magicaCloth2 == null) && magicaCloth2.isActiveAndEnabled && magicaCloth2.SerializeData.clothType == ClothProcess.ClothType.BoneCloth && magicaCloth2.name.IndexOf("Hair", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					List<ColliderComponent> colliderList = magicaCloth2.SerializeData.colliderCollisionConstraint.colliderList;
					if (!colliderList.Contains(this._hairFloor))
					{
						colliderList.Add(this._hairFloor);
						magicaCloth2.SetParameterChange();
						this._hairCloths.Add(magicaCloth2);
					}
				}
			}
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x00279D8C File Offset: 0x00277F8C
		private bool ResolveRestroomDoor()
		{
			float num = 4f;
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (DoorInteraction doorInteraction in rootGameObjects[i].GetComponentsInChildren<DoorInteraction>(true))
				{
					float num2 = Vector3.Distance(doorInteraction.transform.position, this._config.restroomDoor);
					if (num2 < num)
					{
						num = num2;
						this._restroomDoor = doorInteraction;
					}
				}
			}
			return this._restroomDoor != null;
		}

		// Token: 0x06006302 RID: 25346 RVA: 0x00279E20 File Offset: 0x00278020
		private IEnumerator CutOutsideAtDoorway()
		{
			while (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape && LongWayHomeUniversitySequence.PlanarDistance(this._player.position, this._config.hallDoorway) > this._config.hallDoorCameraAtDistance)
			{
				yield return null;
			}
			if (this.Phase == LongWayHomeUniversitySequence.StoryPhase.Escape)
			{
				this.ActivateFixedShot(this._config.hallDoorCamera, 52f);
			}
			yield break;
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x00279E2F File Offset: 0x0027802F
		private void OpenRestroomDoor()
		{
			if (this._restroomDoor == null)
			{
				return;
			}
			this.BeginRestroomDoorMotion();
			this._restroomDoor.ForceOpen();
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x00279E51 File Offset: 0x00278051
		private void CloseRestroomDoor()
		{
			if (this._restroomDoor == null)
			{
				return;
			}
			this.BeginRestroomDoorMotion();
			this._restroomDoor.ForceClose();
			base.StartCoroutine(this.RestoreDoorPause());
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x00279E80 File Offset: 0x00278080
		private void BeginRestroomDoorMotion()
		{
			if (this._restroomDoor == null)
			{
				return;
			}
			if (!this._ownsRestroomDoorPause)
			{
				this._restroomDoorPaused = this._restroomDoor.SimulationPaused;
				this._ownsRestroomDoorPause = true;
			}
			this._restroomDoor.SimulationPaused = false;
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x00279EBD File Offset: 0x002780BD
		private void ReleaseRestroomDoorMotion()
		{
			if (!this._ownsRestroomDoorPause)
			{
				return;
			}
			this._ownsRestroomDoorPause = false;
			if (this._restroomDoor != null)
			{
				this._restroomDoor.SimulationPaused = this._restroomDoorPaused;
			}
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x00279EEE File Offset: 0x002780EE
		private IEnumerator RestoreDoorPause()
		{
			yield return new WaitForSeconds(1.2f);
			this.ReleaseRestroomDoorMotion();
			yield break;
		}

		// Token: 0x06006308 RID: 25352 RVA: 0x00279F00 File Offset: 0x00278100
		private static float PlanarDistance(Vector3 a, Vector3 b)
		{
			a.y = (b.y = 0f);
			return Vector3.Distance(a, b);
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x00279F2C File Offset: 0x0027812C
		private AudioSource Audio()
		{
			if (this._audio != null)
			{
				return this._audio;
			}
			this._audio = base.gameObject.AddComponent<AudioSource>();
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0f;
			GameAudioRouter.Route(this._audio, GameAudioRouter.Channel.Sfx);
			return this._audio;
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x00279F90 File Offset: 0x00278190
		private void PlayBell()
		{
			AudioClip audioClip = ((this._config.morningBell != null) ? this._config.morningBell : LongWayHomeSchoolChime.Clip);
			if (audioClip != null)
			{
				this.Audio().PlayOneShot(audioClip, this._config.bellVolume);
			}
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x00279FE3 File Offset: 0x002781E3
		private void PlayGasp()
		{
			if (this._config.realizationGasp != null)
			{
				this.Audio().PlayOneShot(this._config.realizationGasp, this._config.gaspVolume);
			}
		}

		// Token: 0x040064F9 RID: 25849
		private PlayableGraph _cielGraph;

		// Token: 0x040064FA RID: 25850
		private AnimationLayerMixerPlayable _layers;

		// Token: 0x040064FB RID: 25851
		private AnimationMixerPlayable _bodyMixer;

		// Token: 0x040064FC RID: 25852
		private AnimationMixerPlayable _armsMixer;

		// Token: 0x040064FD RID: 25853
		private AnimationMixerPlayable _leftArmMixer;

		// Token: 0x040064FE RID: 25854
		private AnimationScriptPlayable _poseFix;

		// Token: 0x040064FF RID: 25855
		private AnimationClipPlayable _locoIdle;

		// Token: 0x04006500 RID: 25856
		private AnimationClipPlayable _locoMove;

		// Token: 0x04006501 RID: 25857
		private AnimationClipPlayable _floorA;

		// Token: 0x04006502 RID: 25858
		private AnimationClipPlayable _floorB;

		// Token: 0x04006503 RID: 25859
		private AnimationClip _locoIdleClip;

		// Token: 0x04006504 RID: 25860
		private AnimationClip _locoMoveClip;

		// Token: 0x04006505 RID: 25861
		private float _walkWeight;

		// Token: 0x04006506 RID: 25862
		private float _floorWeight;

		// Token: 0x04006507 RID: 25863
		private bool _moving;

		// Token: 0x04006508 RID: 25864
		private bool _floorFitApplied;

		// Token: 0x04006509 RID: 25865
		private int _poseFixCount;

		// Token: 0x0400650A RID: 25866
		private float[] _poseFixA;

		// Token: 0x0400650B RID: 25867
		private float[] _poseFixB;

		// Token: 0x0400650C RID: 25868
		private float _floorBaseY;

		// Token: 0x0400650D RID: 25869
		private float _floorHeight;

		// Token: 0x0400650E RID: 25870
		private const string StimulationPoseLabel = "Stimulation";

		// Token: 0x0400650F RID: 25871
		private const string HoldBoobsPoseLabel = "Hold Boobs";

		// Token: 0x04006510 RID: 25872
		private const string ShyPoseLabel = "Shy Pose";

		// Token: 0x04006511 RID: 25873
		private const string CoverPoseLabel = "Cover";

		// Token: 0x04006512 RID: 25874
		private const float LowerBlendSpeed = 4f;

		// Token: 0x04006513 RID: 25875
		private const float LowerPinBlendSpeed = 4f;

		// Token: 0x04006514 RID: 25876
		private const float DefaultShapeSpeed = 0.005f;

		// Token: 0x04006515 RID: 25877
		private const float DefaultLayerSpeed = 2f;

		// Token: 0x04006516 RID: 25878
		public const string ClipDeformationShapesField = "CoverClipDeformationShapes";

		// Token: 0x04006517 RID: 25879
		private static readonly LongWayHomeUniversitySequence.ArmsPose[] CorridorPoses = new LongWayHomeUniversitySequence.ArmsPose[]
		{
			LongWayHomeUniversitySequence.ArmsPose.Stimulation,
			LongWayHomeUniversitySequence.ArmsPose.HoldBoobs,
			LongWayHomeUniversitySequence.ArmsPose.ShyPose
		};

		// Token: 0x04006518 RID: 25880
		private static readonly KeyCode[] CorridorPoseKeys = new KeyCode[]
		{
			KeyCode.Alpha1,
			KeyCode.Alpha2,
			KeyCode.Alpha3
		};

		// Token: 0x04006519 RID: 25881
		private static readonly string[] CorridorPoseKeyLabels = new string[] { "1", "2", "3" };

		// Token: 0x0400651A RID: 25882
		private static readonly ControllerButton[] CorridorPoseButtons = new ControllerButton[]
		{
			ControllerButton.DpadLeft,
			ControllerButton.DpadUp,
			ControllerButton.DpadRight
		};

		// Token: 0x0400651B RID: 25883
		private readonly List<LongWayHomeUniversitySequence.ArmsSlot> _armsSlots = new List<LongWayHomeUniversitySequence.ArmsSlot>();

		// Token: 0x0400651C RID: 25884
		private LongWayHomeUniversitySequence.ArmsMoment _armsMoment;

		// Token: 0x0400651D RID: 25885
		private LongWayHomeUniversitySequence.ArmsPose _armsPose;

		// Token: 0x0400651E RID: 25886
		private LongWayHomeUniversitySequence.ArmsSlot _pinSlot;

		// Token: 0x0400651F RID: 25887
		private bool _armsOn;

		// Token: 0x04006520 RID: 25888
		private bool _lowerOn;

		// Token: 0x04006521 RID: 25889
		private bool _armsPressed;

		// Token: 0x04006522 RID: 25890
		private float _lowerWeight;

		// Token: 0x04006523 RID: 25891
		private float _pinWeight;

		// Token: 0x04006524 RID: 25892
		private float _boobsType = 3f;

		// Token: 0x04006525 RID: 25893
		private readonly List<CoverBlendShapeEntry> _stimulationShapes = new List<CoverBlendShapeEntry>();

		// Token: 0x04006526 RID: 25894
		private readonly List<LongWayHomeUniversitySequence.LowerPin> _stimulationPins = new List<LongWayHomeUniversitySequence.LowerPin>();

		// Token: 0x04006527 RID: 25895
		private readonly List<LongWayHomeUniversitySequence.LowerPin> _coverPins = new List<LongWayHomeUniversitySequence.LowerPin>();

		// Token: 0x04006528 RID: 25896
		private CharacterRigController _rig;

		// Token: 0x04006529 RID: 25897
		private bool _rigOwned;

		// Token: 0x0400652A RID: 25898
		private AnimatorUpdateMode _rigSavedUpdateMode;

		// Token: 0x0400652B RID: 25899
		private GameObject[] _coverCloth;

		// Token: 0x0400652C RID: 25900
		private readonly List<GameObject> _hiddenCloth = new List<GameObject>();

		// Token: 0x0400652D RID: 25901
		private readonly List<LongWayHomeUniversitySequence.ClipDeformation> _clipDeformations = new List<LongWayHomeUniversitySequence.ClipDeformation>();

		// Token: 0x0400652E RID: 25902
		private bool _clipDeformationsShown;

		// Token: 0x0400652F RID: 25903
		private Camera _camera;

		// Token: 0x04006530 RID: 25904
		private LongWayHomeUniversitySequence.ShotMode _shotMode;

		// Token: 0x04006531 RID: 25905
		private LongWayHomeUniversityConfigSO.BodyShot[] _shots;

		// Token: 0x04006532 RID: 25906
		private int _shotIndex;

		// Token: 0x04006533 RID: 25907
		private Vector3 _fixedPosition;

		// Token: 0x04006534 RID: 25908
		private float _fieldOfView = 45f;

		// Token: 0x04006535 RID: 25909
		private Vector3 _cameraVelocity;

		// Token: 0x04006536 RID: 25910
		private Vector3 _focus;

		// Token: 0x04006537 RID: 25911
		private Vector3 _focusVelocity;

		// Token: 0x04006538 RID: 25912
		private float _cameraYaw;

		// Token: 0x04006539 RID: 25913
		private float _cameraYawVelocity;

		// Token: 0x0400653A RID: 25914
		private float _reach = 1f;

		// Token: 0x0400653B RID: 25915
		private float _reachVelocity;

		// Token: 0x0400653C RID: 25916
		private readonly RaycastHit[] _cameraHits = new RaycastHit[16];

		// Token: 0x0400653D RID: 25917
		private bool _orbiting;

		// Token: 0x0400653E RID: 25918
		private Transform _orbitAnchor;

		// Token: 0x0400653F RID: 25919
		private Vector3 _orbitAnchorOffset;

		// Token: 0x04006540 RID: 25920
		private Vector3 _orbitPivot;

		// Token: 0x04006541 RID: 25921
		private Vector3 _orbitPivotVelocity;

		// Token: 0x04006542 RID: 25922
		private float _orbitYaw;

		// Token: 0x04006543 RID: 25923
		private float _orbitPitch;

		// Token: 0x04006544 RID: 25924
		private float _orbitZoom;

		// Token: 0x04006545 RID: 25925
		private float _orbitDistance;

		// Token: 0x04006546 RID: 25926
		private float _orbitFieldOfView;

		// Token: 0x04006547 RID: 25927
		private float _orbitReach = 1f;

		// Token: 0x04006548 RID: 25928
		private Quaternion _orbitRotation = Quaternion.identity;

		// Token: 0x04006549 RID: 25929
		private Vector3 _lookInput;

		// Token: 0x0400654A RID: 25930
		private bool _lookFromPad;

		// Token: 0x0400654B RID: 25931
		private const float PovEyeHeight = -0.08f;

		// Token: 0x0400654C RID: 25932
		private const float PovEyeForward = 0.045f;

		// Token: 0x0400654D RID: 25933
		private const float PovConfidentTorsoForward = 0.15f;

		// Token: 0x0400654E RID: 25934
		private const float PovTorsoForward = 0.17f;

		// Token: 0x0400654F RID: 25935
		private const float PovTorsoHeight = 0.16f;

		// Token: 0x04006550 RID: 25936
		private const float PovTorsoVertical = 0.776f;

		// Token: 0x04006551 RID: 25937
		private const float PovEmbarrassedPitch = 14.03f;

		// Token: 0x04006552 RID: 25938
		private const float PovPresetBlendSpeed = 8f;

		// Token: 0x04006553 RID: 25939
		private const float PovVerticalDamping = 0.5f;

		// Token: 0x04006554 RID: 25940
		private const float PovMaximumVerticalLag = 0.025f;

		// Token: 0x04006555 RID: 25941
		private const float PovNearClip = 0.01f;

		// Token: 0x04006556 RID: 25942
		private const float PovMouseSensitivity = 3f;

		// Token: 0x04006557 RID: 25943
		private const float PovHeadRefreshSeconds = 0.25f;

		// Token: 0x04006558 RID: 25944
		private const int PlayerHeadLayer = 21;

		// Token: 0x04006559 RID: 25945
		private bool _pov;

		// Token: 0x0400655A RID: 25946
		private float _povPitch;

		// Token: 0x0400655B RID: 25947
		private float _povYaw;

		// Token: 0x0400655C RID: 25948
		private float _povSmoothedY;

		// Token: 0x0400655D RID: 25949
		private float _povCorrectionElapsed = 1f;

		// Token: 0x0400655E RID: 25950
		private float _povHeadRefreshAt;

		// Token: 0x0400655F RID: 25951
		private bool _povYInitialized;

		// Token: 0x04006560 RID: 25952
		private bool _povWasLying;

		// Token: 0x04006561 RID: 25953
		private bool _povRotationInitialized;

		// Token: 0x04006562 RID: 25954
		private Quaternion _povBasis = Quaternion.identity;

		// Token: 0x04006563 RID: 25955
		private Quaternion _povLastRotation = Quaternion.identity;

		// Token: 0x04006564 RID: 25956
		private Quaternion _povCorrection = Quaternion.identity;

		// Token: 0x04006565 RID: 25957
		private Vector3 _povLastPosition;

		// Token: 0x04006566 RID: 25958
		private Vector3 _povPositionCorrection;

		// Token: 0x04006567 RID: 25959
		private float _povEmbarrassed;

		// Token: 0x04006568 RID: 25960
		private Transform _leftBreast;

		// Token: 0x04006569 RID: 25961
		private Transform _rightBreast;

		// Token: 0x0400656A RID: 25962
		private readonly Dictionary<Renderer, ShadowCastingMode> _headRenderers = new Dictionary<Renderer, ShadowCastingMode>();

		// Token: 0x0400656B RID: 25963
		private bool _headHidden;

		// Token: 0x0400656C RID: 25964
		private bool _renderCallbacks;

		// Token: 0x0400656D RID: 25965
		private LongWayHomeEyelidOverlay _eyelids;

		// Token: 0x0400656E RID: 25966
		private static readonly vThirdPersonCameraState FallbackCameraState = new vThirdPersonCameraState("Default")
		{
			defaultDistance = 1.5f,
			minDistance = 0.5f,
			maxDistance = 3f,
			height = 1.4f,
			smooth = 4f,
			smoothDamp = 4f,
			xMouseSensitivity = 3f,
			yMouseSensitivity = 3f,
			yMinLimit = -40f,
			yMaxLimit = 80f,
			fov = 60f,
			useZoom = true
		};

		// Token: 0x0400656F RID: 25967
		private static readonly string[] HeadSemantics = new string[]
		{
			"head", "face", "hair", "eye", "brow", "lash", "glasses", "spectacle", "headpin", "earring",
			"mask"
		};

		// Token: 0x04006570 RID: 25968
		private const string FaceRendererPath = "3D Model/Ciel_07_05_26_ass/Body";

		// Token: 0x04006571 RID: 25969
		private const float ReadinessTimeout = 60f;

		// Token: 0x04006572 RID: 25970
		private static LongWayHomeUniversitySequence _activePresenter;

		// Token: 0x04006573 RID: 25971
		private readonly List<LongWayHomeUniversitySequence.BehaviourState> _presentation = new List<LongWayHomeUniversitySequence.BehaviourState>();

		// Token: 0x04006574 RID: 25972
		private LongWayHomeUniversityConfigSO _config;

		// Token: 0x04006575 RID: 25973
		private Transform _player;

		// Token: 0x04006576 RID: 25974
		private Animator _animator;

		// Token: 0x04006577 RID: 25975
		private vThirdPersonInput _input;

		// Token: 0x04006578 RID: 25976
		private vThirdPersonCamera _gameplayCamera;

		// Token: 0x04006579 RID: 25977
		private Camera _gameplayView;

		// Token: 0x0400657A RID: 25978
		private Rigidbody _body;

		// Token: 0x0400657B RID: 25979
		private MCStyleCustomizationService _style;

		// Token: 0x0400657C RID: 25980
		private ClothingToggleController _clothing;

		// Token: 0x0400657D RID: 25981
		private CoverPoseController _coverPose;

		// Token: 0x0400657E RID: 25982
		private PlayerActivityLease _lease;

		// Token: 0x0400657F RID: 25983
		private bool _captured;

		// Token: 0x04006580 RID: 25984
		private bool _inputEnabled;

		// Token: 0x04006581 RID: 25985
		private bool _inputLock;

		// Token: 0x04006582 RID: 25986
		private bool _cameraLock;

		// Token: 0x04006583 RID: 25987
		private bool _cameraFrozen;

		// Token: 0x04006584 RID: 25988
		private bool _coverEnabled;

		// Token: 0x04006585 RID: 25989
		private bool _rootMotion;

		// Token: 0x04006586 RID: 25990
		private bool _kinematic;

		// Token: 0x04006587 RID: 25991
		private bool _gravity;

		// Token: 0x04006588 RID: 25992
		private RigidbodyConstraints _constraints;

		// Token: 0x04006589 RID: 25993
		private RigidbodyInterpolation _interpolation;

		// Token: 0x0400658A RID: 25994
		private float[] _layerWeights;

		// Token: 0x0400658B RID: 25995
		private AnimatorCullingMode _culling;

		// Token: 0x0400658C RID: 25996
		private AnimatorUpdateMode _updateMode;

		// Token: 0x0400658D RID: 25997
		private PlayerEmotionController _emotion;

		// Token: 0x0400658E RID: 25998
		private Animator _faceAnimator;

		// Token: 0x0400658F RID: 25999
		private string _faceParameter;

		// Token: 0x04006590 RID: 26000
		private int _faceLayer = -1;

		// Token: 0x04006591 RID: 26001
		private int _savedFaceIndex;

		// Token: 0x04006592 RID: 26002
		private int _neutralFaceIndex;

		// Token: 0x04006593 RID: 26003
		private bool _ownsFaceOverride;

		// Token: 0x04006594 RID: 26004
		private bool _ownsEmotionOverride;

		// Token: 0x04006595 RID: 26005
		private LongWayHomeGymShowerFaceDriver _face;

		// Token: 0x04006596 RID: 26006
		private SkinnedMeshRenderer _faceRenderer;

		// Token: 0x04006597 RID: 26007
		private MCStyleCustomizationService.WardrobeSnapshot _wardrobeSnapshot;

		// Token: 0x04006598 RID: 26008
		private WearableUseController _wearController;

		// Token: 0x04006599 RID: 26009
		private bool _wearWasEnabled;

		// Token: 0x0400659A RID: 26010
		private IDisposable _wardrobePresentation;

		// Token: 0x0400659B RID: 26011
		private LongWayHomeFloorRoutine _routine;

		// Token: 0x0400659C RID: 26012
		private bool _skipRequested;

		// Token: 0x0400659D RID: 26013
		private bool _continueRequested;

		// Token: 0x0400659E RID: 26014
		private const float DoorShutSeconds = 1.3f;

		// Token: 0x0400659F RID: 26015
		private const float SkipPopulationSeconds = 1.2f;

		// Token: 0x040065A0 RID: 26016
		private LongWayHomeCinematicSkip _hints;

		// Token: 0x040065A4 RID: 26020
		internal const string StimulationAction = "Stimulation";

		// Token: 0x040065A5 RID: 26021
		internal const string HoldBoobsAction = "Hold Boobs";

		// Token: 0x040065A6 RID: 26022
		internal const string ShyPoseAction = "Shy Pose";

		// Token: 0x040065A7 RID: 26023
		internal const string CoverAction = "Cover";

		// Token: 0x040065A8 RID: 26024
		internal const string CoverLowerBodyAction = "Cover lower body";

		// Token: 0x040065A9 RID: 26025
		internal const string ChangePositionAction = "Change position";

		// Token: 0x040065AA RID: 26026
		internal const string ContinueAction = "Continue";

		// Token: 0x040065AB RID: 26027
		internal const string SwitchCameraAction = "Switch camera";

		// Token: 0x040065AC RID: 26028
		internal const string RotateCameraAction = "Rotate camera";

		// Token: 0x040065AD RID: 26029
		internal const string ZoomAction = "Zoom";

		// Token: 0x040065AE RID: 26030
		internal const string LookAroundAction = "Look around";

		// Token: 0x040065AF RID: 26031
		internal const string FirstPersonAction = "First person";

		// Token: 0x040065B0 RID: 26032
		internal const string ThirdPersonAction = "Third person";

		// Token: 0x040065B1 RID: 26033
		private readonly List<LongWayHomeCinematicSkip.Hint> _hintList = new List<LongWayHomeCinematicSkip.Hint>();

		// Token: 0x040065B2 RID: 26034
		private float _corridorStartedAt;

		// Token: 0x040065B3 RID: 26035
		private const float NightFadeSeconds = 1.5f;

		// Token: 0x040065B4 RID: 26036
		private AudioSource _nightA;

		// Token: 0x040065B5 RID: 26037
		private AudioSource _nightB;

		// Token: 0x040065B6 RID: 26038
		private CrossfadeLoopPlayer _night;

		// Token: 0x040065B7 RID: 26039
		private float _nightLevel;

		// Token: 0x040065B8 RID: 26040
		private float _nightTarget;

		// Token: 0x040065B9 RID: 26041
		private static readonly float[] FallbackContacts = new float[] { 0.25f, 0.75f };

		// Token: 0x040065BA RID: 26042
		private AudioSource _steps;

		// Token: 0x040065BB RID: 26043
		private AnimationClip _stepClip;

		// Token: 0x040065BC RID: 26044
		private float[] _contacts = LongWayHomeUniversitySequence.FallbackContacts;

		// Token: 0x040065BD RID: 26045
		private double _previousStepTime;

		// Token: 0x040065BE RID: 26046
		private Vector3 _previousStepPosition;

		// Token: 0x040065BF RID: 26047
		private bool _stepsArmed;

		// Token: 0x040065C0 RID: 26048
		private int _lastStep = -1;

		// Token: 0x040065C1 RID: 26049
		private readonly List<LongWayHomeUniversityBystander> _students = new List<LongWayHomeUniversityBystander>();

		// Token: 0x040065C2 RID: 26050
		private readonly List<LongWayHomeUniversityConfigSO.BystanderMark> _studentMarks = new List<LongWayHomeUniversityConfigSO.BystanderMark>();

		// Token: 0x040065C3 RID: 26051
		private readonly HashSet<string> _usedCivilians = new HashSet<string>();

		// Token: 0x040065C4 RID: 26052
		private bool _studentsVisible;

		// Token: 0x040065C5 RID: 26053
		private bool _populationFocus;

		// Token: 0x040065C6 RID: 26054
		private const string SeatedPrefix = "NPC_StaticSeated_";

		// Token: 0x040065C7 RID: 26055
		private readonly List<GameObject> _hiddenSeated = new List<GameObject>();

		// Token: 0x040065C8 RID: 26056
		private bool _populationHidden;

		// Token: 0x040065C9 RID: 26057
		private float _savedDensity = 1f;

		// Token: 0x040065CA RID: 26058
		private AzureCoreSystem _azure;

		// Token: 0x040065CB RID: 26059
		private bool _clockHeld;

		// Token: 0x040065CC RID: 26060
		private bool _morningApplied;

		// Token: 0x040065CD RID: 26061
		private float _heldHour;

		// Token: 0x040065CE RID: 26062
		private float _savedDayLength;

		// Token: 0x040065CF RID: 26063
		private float _savedStartTime;

		// Token: 0x040065D0 RID: 26064
		private readonly List<LongWayHomeGymShowerPhotographer> _crowd = new List<LongWayHomeGymShowerPhotographer>();

		// Token: 0x040065D1 RID: 26065
		private bool _crowdVisible;

		// Token: 0x040065D2 RID: 26066
		private DoorInteraction _restroomDoor;

		// Token: 0x040065D3 RID: 26067
		private bool _restroomDoorPaused;

		// Token: 0x040065D4 RID: 26068
		private bool _ownsRestroomDoorPause;

		// Token: 0x040065D5 RID: 26069
		private AudioSource _audio;

		// Token: 0x040065D6 RID: 26070
		private MagicaPlaneCollider _hairFloor;

		// Token: 0x040065D7 RID: 26071
		private readonly List<MagicaCloth> _hairCloths = new List<MagicaCloth>();

		// Token: 0x02000D29 RID: 3369
		private enum ArmsPose
		{
			// Token: 0x040065D9 RID: 26073
			None,
			// Token: 0x040065DA RID: 26074
			Stimulation,
			// Token: 0x040065DB RID: 26075
			HoldBoobs,
			// Token: 0x040065DC RID: 26076
			ShyPose,
			// Token: 0x040065DD RID: 26077
			Cover
		}

		// Token: 0x02000D2A RID: 3370
		private enum ArmsMoment
		{
			// Token: 0x040065DF RID: 26079
			None,
			// Token: 0x040065E0 RID: 26080
			Corridor,
			// Token: 0x040065E1 RID: 26081
			Escape
		}

		// Token: 0x02000D2B RID: 3371
		private struct LowerPin
		{
			// Token: 0x040065E2 RID: 26082
			public LimbType Limb;

			// Token: 0x040065E3 RID: 26083
			public Transform Reference;

			// Token: 0x040065E4 RID: 26084
			public Vector3 Position;

			// Token: 0x040065E5 RID: 26085
			public Vector3 Rotation;

			// Token: 0x040065E6 RID: 26086
			public Vector3 Hint;

			// Token: 0x040065E7 RID: 26087
			public float PositionWeight;

			// Token: 0x040065E8 RID: 26088
			public float RotationWeight;

			// Token: 0x040065E9 RID: 26089
			public float HintWeight;
		}

		// Token: 0x02000D2C RID: 3372
		private struct ClipDeformation
		{
			// Token: 0x040065EA RID: 26090
			public SkinnedMeshRenderer Renderer;

			// Token: 0x040065EB RID: 26091
			public BlendShapeManager Manager;

			// Token: 0x040065EC RID: 26092
			public string Shape;

			// Token: 0x040065ED RID: 26093
			public int Index;

			// Token: 0x040065EE RID: 26094
			public float Baseline;
		}

		// Token: 0x02000D2D RID: 3373
		private sealed class ArmsSlot
		{
			// Token: 0x17001004 RID: 4100
			// (get) Token: 0x0600630E RID: 25358 RVA: 0x0027A2EA File Offset: 0x002784EA
			public bool HasLower
			{
				get
				{
					return this.LowerInput >= 0;
				}
			}

			// Token: 0x040065EF RID: 26095
			public LongWayHomeUniversitySequence.ArmsPose Pose;

			// Token: 0x040065F0 RID: 26096
			public bool LeftArmOnly;

			// Token: 0x040065F1 RID: 26097
			public bool HidesCloth;

			// Token: 0x040065F2 RID: 26098
			public AnimationClipPlayable Chest;

			// Token: 0x040065F3 RID: 26099
			public AnimationClipPlayable Lower;

			// Token: 0x040065F4 RID: 26100
			public int ChestInput = -1;

			// Token: 0x040065F5 RID: 26101
			public int LowerInput = -1;

			// Token: 0x040065F6 RID: 26102
			public float Weight;

			// Token: 0x040065F7 RID: 26103
			public readonly List<CoverBlendShapeEntry> Shapes = new List<CoverBlendShapeEntry>();

			// Token: 0x040065F8 RID: 26104
			public List<LongWayHomeUniversitySequence.LowerPin> Pins;
		}

		// Token: 0x02000D2E RID: 3374
		private enum ShotMode
		{
			// Token: 0x040065FA RID: 26106
			Body,
			// Token: 0x040065FB RID: 26107
			Fixed,
			// Token: 0x040065FC RID: 26108
			Face
		}

		// Token: 0x02000D2F RID: 3375
		public enum StoryPhase
		{
			// Token: 0x040065FE RID: 26110
			Loading,
			// Token: 0x040065FF RID: 26111
			Corridor,
			// Token: 0x04006600 RID: 26112
			Fall,
			// Token: 0x04006601 RID: 26113
			Floor,
			// Token: 0x04006602 RID: 26114
			FallingAsleep,
			// Token: 0x04006603 RID: 26115
			Morning,
			// Token: 0x04006604 RID: 26116
			GetUp,
			// Token: 0x04006605 RID: 26117
			Realize,
			// Token: 0x04006606 RID: 26118
			Escape,
			// Token: 0x04006607 RID: 26119
			Handoff,
			// Token: 0x04006608 RID: 26120
			Done,
			// Token: 0x04006609 RID: 26121
			Failed
		}

		// Token: 0x02000D30 RID: 3376
		private struct BehaviourState
		{
			// Token: 0x0400660A RID: 26122
			public Behaviour Component;

			// Token: 0x0400660B RID: 26123
			public bool Enabled;
		}
	}
}
