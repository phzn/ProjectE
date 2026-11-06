using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.BodyPhysics;
using Features.CielVoice;
using Features.Detection;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Loading;
using Features.MainMenu;
using Features.MasturbationSystem;
using Features.NPC;
using Features.Stadium;
using Features.WardrobeFeedback;
using GPUInstancerPro;
using Invector;
using Invector.vCamera;
using Invector.vCharacterController;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AzureSky;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.RunModes
{
	// Token: 0x02000D00 RID: 3328
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1050)]
	public sealed class LongWayHomeStadiumSequence : MonoBehaviour, ILongWayHomePoiProloguePresenter
	{
		// Token: 0x060060C7 RID: 24775 RVA: 0x00266AA0 File Offset: 0x00264CA0
		private void BuildCielGraph()
		{
			if (this._cielGraph.IsValid())
			{
				this._cielGraph.Destroy();
			}
			this._slots.Clear();
			this._cielGraph = PlayableGraph.Create("Long Way Home - Stadium Ciel");
			this._cielGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
			this._bodyMixer = AnimationMixerPlayable.Create(this._cielGraph, 0);
			this.AddSlot(LongWayHomeStadiumSequence.BodyPose.Wall, this._config.wallPose, null);
			this.AddSlot(LongWayHomeStadiumSequence.BodyPose.Run, null, this._config.fieldRun);
			this.AddSlot(LongWayHomeStadiumSequence.BodyPose.LookAtMe, this._config.lookAtMe, null);
			this.AddSlot(LongWayHomeStadiumSequence.BodyPose.Embarrassed, this._config.escapeIdle, this._config.escapeRun);
			this._layers = AnimationLayerMixerPlayable.Create(this._cielGraph, 2);
			this._cielGraph.Connect<AnimationMixerPlayable, AnimationLayerMixerPlayable>(this._bodyMixer, 0, this._layers, 0);
			this._layers.SetInputWeight(0, 1f);
			this._faceCover = AnimationClipPlayable.Create(this._cielGraph, this._config.faceCoverArms);
			this._faceCover.SetApplyFootIK(false);
			this._cielGraph.Connect<AnimationClipPlayable, AnimationLayerMixerPlayable>(this._faceCover, 0, this._layers, 1);
			this._layers.SetInputWeight(1, 0f);
			this._layers.SetLayerMaskFromAvatarMask(1U, this._config.armsMask);
			this._layers.SetLayerAdditive(1U, false);
			this._coverWeight = (this._coverTarget = 0f);
			this._lookAtJob = AnimationScriptPlayable.Create<LongWayHomeStadiumLookAt>(this._cielGraph, default(LongWayHomeStadiumLookAt), 1);
			this._cielGraph.Connect<AnimationLayerMixerPlayable, AnimationScriptPlayable>(this._layers, 0, this._lookAtJob, 0);
			this._lookAtJob.SetInputWeight(0, 1f);
			AnimationPlayableOutput.Create(this._cielGraph, "Ciel - Stadium", this._animator).SetSourcePlayable(this._lookAtJob);
			this._cielGraph.Play();
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x00266C90 File Offset: 0x00264E90
		private void AddSlot(LongWayHomeStadiumSequence.BodyPose pose, AnimationClip still, AnimationClip moving)
		{
			LongWayHomeStadiumSequence.PoseSlot poseSlot = new LongWayHomeStadiumSequence.PoseSlot();
			if (still != null)
			{
				poseSlot.Still = AnimationClipPlayable.Create(this._cielGraph, still);
				poseSlot.Still.SetApplyFootIK(false);
				poseSlot.StillInput = this._bodyMixer.AddInput(poseSlot.Still, 0, 0f);
			}
			if (moving != null)
			{
				poseSlot.Moving = AnimationClipPlayable.Create(this._cielGraph, moving);
				poseSlot.Moving.SetApplyFootIK(false);
				poseSlot.MovingInput = this._bodyMixer.AddInput(poseSlot.Moving, 0, 0f);
			}
			this._slots[pose] = poseSlot;
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x00266D38 File Offset: 0x00264F38
		private static LongWayHomeStadiumSequence.BodyPose BodySlot(LongWayHomeStadiumSequence.BodyPose pose)
		{
			if (pose != LongWayHomeStadiumSequence.BodyPose.HoldBag)
			{
				return pose;
			}
			return LongWayHomeStadiumSequence.BodyPose.Wall;
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x00266D44 File Offset: 0x00264F44
		private void UseBodyPose(LongWayHomeStadiumSequence.BodyPose pose, float blendSeconds)
		{
			if (!this._cielGraph.IsValid())
			{
				return;
			}
			LongWayHomeStadiumSequence.BodyPose bodyPose = LongWayHomeStadiumSequence.BodySlot(this._pose);
			this._pose = pose;
			LongWayHomeStadiumSequence.BodyPose bodyPose2 = LongWayHomeStadiumSequence.BodySlot(pose);
			if (blendSeconds <= 0f)
			{
				foreach (KeyValuePair<LongWayHomeStadiumSequence.BodyPose, LongWayHomeStadiumSequence.PoseSlot> keyValuePair in this._slots)
				{
					keyValuePair.Value.Weight = ((keyValuePair.Key == bodyPose2) ? 1f : 0f);
				}
				this._poseBlendSpeed = 1000f;
				LongWayHomeStadiumSequence.PoseSlot poseSlot;
				if (this._slots.TryGetValue(bodyPose2, out poseSlot))
				{
					if (poseSlot.Still.IsValid<AnimationClipPlayable>())
					{
						poseSlot.Still.SetTime(0.0);
					}
					if (poseSlot.Moving.IsValid<AnimationClipPlayable>())
					{
						poseSlot.Moving.SetTime(0.0);
					}
				}
			}
			else
			{
				this._poseBlendSpeed = 1f / blendSeconds;
				LongWayHomeStadiumSequence.PoseSlot poseSlot2;
				if (bodyPose2 != bodyPose && this._slots.TryGetValue(bodyPose2, out poseSlot2) && poseSlot2.Weight <= 0.001f)
				{
					if (poseSlot2.Still.IsValid<AnimationClipPlayable>())
					{
						poseSlot2.Still.SetTime(0.0);
					}
					if (poseSlot2.Moving.IsValid<AnimationClipPlayable>())
					{
						poseSlot2.Moving.SetTime(0.0);
					}
				}
			}
			this.ApplyBodyWeights();
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x00266EC8 File Offset: 0x002650C8
		private void TickBodyPose(float deltaTime)
		{
			if (!this._cielGraph.IsValid())
			{
				return;
			}
			LongWayHomeStadiumSequence.BodyPose bodyPose = LongWayHomeStadiumSequence.BodySlot(this._pose);
			foreach (KeyValuePair<LongWayHomeStadiumSequence.BodyPose, LongWayHomeStadiumSequence.PoseSlot> keyValuePair in this._slots)
			{
				keyValuePair.Value.Weight = Mathf.MoveTowards(keyValuePair.Value.Weight, (keyValuePair.Key == bodyPose) ? 1f : 0f, this._poseBlendSpeed * deltaTime);
			}
			this.ApplyBodyWeights();
			this._coverWeight = Mathf.MoveTowards(this._coverWeight, this._coverTarget, this._coverSpeed * deltaTime);
			this.ApplyFaceCover();
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x00266F94 File Offset: 0x00265194
		private void SetFaceCover(bool on, float seconds)
		{
			if (!this._layers.IsValid<AnimationLayerMixerPlayable>())
			{
				return;
			}
			if (on && this._coverWeight <= 0.001f && this._faceCover.IsValid<AnimationClipPlayable>())
			{
				this._faceCover.SetTime(0.0);
			}
			this._coverTarget = (on ? 1f : 0f);
			this._coverSpeed = ((seconds > 0f) ? (1f / seconds) : 1000f);
			if (seconds <= 0f)
			{
				this._coverWeight = this._coverTarget;
			}
			this.ApplyFaceCover();
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0026702B File Offset: 0x0026522B
		private void ApplyFaceCover()
		{
			if (this._layers.IsValid<AnimationLayerMixerPlayable>())
			{
				this._layers.SetInputWeight(1, Mathf.SmoothStep(0f, 1f, this._coverWeight));
			}
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0026705C File Offset: 0x0026525C
		private void ApplyBodyWeights()
		{
			if (!this._bodyMixer.IsValid<AnimationMixerPlayable>())
			{
				return;
			}
			float num = Mathf.Clamp01(this._runWeight);
			foreach (LongWayHomeStadiumSequence.PoseSlot poseSlot in this._slots.Values)
			{
				bool flag = poseSlot.StillInput >= 0 && poseSlot.MovingInput >= 0;
				if (poseSlot.StillInput >= 0)
				{
					this._bodyMixer.SetInputWeight(poseSlot.StillInput, poseSlot.Weight * (flag ? (1f - num) : 1f));
				}
				if (poseSlot.MovingInput >= 0)
				{
					this._bodyMixer.SetInputWeight(poseSlot.MovingInput, poseSlot.Weight * (flag ? num : 1f));
				}
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x060060CF RID: 24783 RVA: 0x00267144 File Offset: 0x00265344
		private float RunCycle
		{
			get
			{
				LongWayHomeStadiumSequence.PoseSlot poseSlot;
				if (!this._slots.TryGetValue(LongWayHomeStadiumSequence.BodyPose.Run, out poseSlot) || !poseSlot.Moving.IsValid<AnimationClipPlayable>())
				{
					return 0f;
				}
				double num = (double)this._config.fieldRun.length;
				if (num <= 0.0)
				{
					return 0f;
				}
				return (float)(poseSlot.Moving.GetTime<AnimationClipPlayable>() / num % 1.0);
			}
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x002671B0 File Offset: 0x002653B0
		private AnimationClipPlayable MovingClip()
		{
			LongWayHomeStadiumSequence.PoseSlot poseSlot;
			if (!this._slots.TryGetValue(LongWayHomeStadiumSequence.BodySlot(this._pose), out poseSlot))
			{
				return default(AnimationClipPlayable);
			}
			return poseSlot.Moving;
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x002671E8 File Offset: 0x002653E8
		private void SetLookAt(bool on, float seconds)
		{
			this._lookTarget = (on ? 1f : 0f);
			this._lookSpeed = ((seconds > 0f) ? (1f / seconds) : 1000f);
			if (seconds <= 0f)
			{
				this._lookWeight = this._lookTarget;
			}
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x0026723C File Offset: 0x0026543C
		private void TickLookAt(float deltaTime)
		{
			if (!this._lookAtJob.IsValid<AnimationScriptPlayable>())
			{
				return;
			}
			this._lookWeight = Mathf.MoveTowards(this._lookWeight, this._lookTarget, this._lookSpeed * deltaTime);
			LongWayHomeStadiumLookAt jobData = this._lookAtJob.GetJobData<LongWayHomeStadiumLookAt>();
			jobData.Weight = Mathf.SmoothStep(0f, 1f, this._lookWeight);
			if (jobData.Weight > 0f)
			{
				Vector3 vector = ((this._handBag != null && this._handBag.activeInHierarchy) ? this._handBag.transform.position : this.Bone(HumanBodyBones.RightHand).position);
				Vector3 vector2 = ((this._camera != null) ? this._camera.transform.position : vector);
				jobData.Target = Vector3.Lerp(vector, vector2, this._config.lookAtCameraShare);
				jobData.HeadWeight = this._config.lookAtHeadWeight;
				jobData.BodyWeight = this._config.lookAtBodyWeight;
				jobData.EyesWeight = 0f;
				jobData.ClampWeight = this._config.lookAtClampWeight;
			}
			this._lookAtJob.SetJobData<LongWayHomeStadiumLookAt>(jobData);
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x00267372 File Offset: 0x00265572
		private IEnumerator FieldLoop()
		{
			bool flag = true;
			while (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Run)
			{
				yield return this.MoveAlong(this._config.fieldPath, this._config.fieldMetersPerSecond, true, false, flag);
				flag = false;
			}
			yield break;
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x00267381 File Offset: 0x00265581
		private Vector3[] FieldStart()
		{
			return this._config.fieldPath;
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x0026738E File Offset: 0x0026558E
		private IEnumerator MoveAlong(Vector3[] waypoints, float metersPerSecond, bool alreadyMoving, bool stopAtEnd, bool restartCycle = true)
		{
			Vector3[] path = LongWayHomeCinematicPath.RoundCorners(waypoints, 4);
			float[] along = LongWayHomeCinematicPath.CumulativeLengths(path);
			float total = along[along.Length - 1];
			float travelled = 0f;
			float speed = (alreadyMoving ? metersPerSecond : 0f);
			float speedVelocity = 0f;
			float yaw = this._player.eulerAngles.y;
			float yawVelocity = 0f;
			AnimationClipPlayable cycle = this.MovingClip();
			float clipSpeed = (cycle.IsValid<AnimationClipPlayable>() ? ((float)cycle.GetSpeed<AnimationClipPlayable>()) : 1f);
			if (restartCycle && !alreadyMoving && cycle.IsValid<AnimationClipPlayable>())
			{
				cycle.SetTime(0.0);
			}
			this._moving = true;
			this.BeginFootsteps();
			while (travelled < total - 0.002f)
			{
				float deltaTime = Time.deltaTime;
				float num = LongWayHomeCinematicPath.BendDegrees(path, along, travelled, 1.6f);
				float num2 = metersPerSecond * Mathf.Lerp(1f, 0.82f, Mathf.InverseLerp(25f, 110f, num));
				float num3 = (stopAtEnd ? (metersPerSecond * Mathf.Sqrt(Mathf.Clamp01((total - travelled) / 1.2f))) : metersPerSecond);
				float num4 = Mathf.Max(Mathf.Min(num2, num3), 0.15f);
				speed = Mathf.SmoothDamp(speed, num4, ref speedVelocity, 0.3f);
				travelled = Mathf.Min(total, travelled + speed * deltaTime);
				Vector3 vector = LongWayHomeCinematicPath.Sample(path, along, travelled);
				Vector3 vector2 = LongWayHomeCinematicPath.Sample(path, along, Mathf.Min(total, travelled + 1.1f)) - vector;
				vector2.y = 0f;
				if (vector2.sqrMagnitude > 0.0004f)
				{
					yaw = Mathf.SmoothDampAngle(yaw, Quaternion.LookRotation(vector2).eulerAngles.y, ref yawVelocity, 0.2f);
				}
				Quaternion quaternion = Quaternion.Euler(0f, yaw, 0f);
				this._player.SetPositionAndRotation(vector, quaternion);
				this._body.position = vector;
				this._body.rotation = quaternion;
				float num5 = ((metersPerSecond > 0f) ? Mathf.Clamp01(speed / metersPerSecond) : 0f);
				this._runWeight = Mathf.Clamp01(num5 * 1.6f);
				this.ApplyBodyWeights();
				if (cycle.IsValid<AnimationClipPlayable>())
				{
					cycle.SetSpeed((double)(clipSpeed * Mathf.Lerp(0.7f, 1f, num5)));
				}
				this.TickFootsteps();
				yield return null;
			}
			this._moving = false;
			if (cycle.IsValid<AnimationClipPlayable>())
			{
				cycle.SetSpeed((double)clipSpeed);
			}
			if (!stopAtEnd)
			{
				yield break;
			}
			while (this._runWeight > 0f)
			{
				this._runWeight = Mathf.MoveTowards(this._runWeight, 0f, Time.deltaTime / 0.25f);
				this.ApplyBodyWeights();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x002673C4 File Offset: 0x002655C4
		private void CaptureNipples()
		{
			this._boobsType = 3f;
			foreach (AnimatorControllerParameter animatorControllerParameter in this._animator.parameters)
			{
				if (animatorControllerParameter.name == "BoobsType")
				{
					this._boobsType = ((animatorControllerParameter.type == AnimatorControllerParameterType.Int) ? ((float)this._animator.GetInteger(animatorControllerParameter.nameHash)) : this._animator.GetFloat(animatorControllerParameter.nameHash));
				}
			}
			this._nipples.Clear();
			if (this._coverPose == null || this._coverPose.ConfidentBlendShapesPerPose == null)
			{
				return;
			}
			foreach (CoverPoseBlendShapeSet coverPoseBlendShapeSet in this._coverPose.ConfidentBlendShapesPerPose)
			{
				if (coverPoseBlendShapeSet != null && string.Equals(coverPoseBlendShapeSet.Label, "Stimulation", StringComparison.OrdinalIgnoreCase) && coverPoseBlendShapeSet.BlendShapes != null)
				{
					using (List<CoverBlendShapeEntry>.Enumerator enumerator2 = coverPoseBlendShapeSet.BlendShapes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CoverBlendShapeEntry coverBlendShapeEntry = enumerator2.Current;
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
									this._nipples.Add(coverBlendShapeEntry2);
								}
							}
						}
						break;
					}
				}
			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x002675C0 File Offset: 0x002657C0
		private void HoldErectNipples()
		{
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._nipples)
			{
				coverBlendShapeEntry.Advance(coverBlendShapeEntry.ResolveTargetValue(this._boobsType), 1000f, 1f);
			}
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x00267628 File Offset: 0x00265828
		private void ReleaseNipples()
		{
			foreach (CoverBlendShapeEntry coverBlendShapeEntry in this._nipples)
			{
				coverBlendShapeEntry.RemoveOverride();
			}
			this._nipples.Clear();
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x00267684 File Offset: 0x00265884
		private void PrepareBreasts()
		{
			this._left.Spring = LongWayHomeStadiumSequence.FindChildIgnoreCase(this._player, "c_breast_01.l");
			this._right.Spring = LongWayHomeStadiumSequence.FindChildIgnoreCase(this._player, "c_breast_01.r");
			this._left.Base = LongWayHomeStadiumSequence.FindChildIgnoreCase(this._player, "c_breast_02.l");
			this._right.Base = LongWayHomeStadiumSequence.FindChildIgnoreCase(this._player, "c_breast_02.r");
			this._breastCloths.Clear();
			foreach (MagicaCloth magicaCloth in this._player.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && (magicaCloth.name == "BoobL" || magicaCloth.name == "BoobR"))
				{
					this._breastCloths.Add(magicaCloth.gameObject);
				}
			}
			this._bounceScale = this._config.BounceScaleFor(Mathf.RoundToInt(this._boobsType));
			this._bounceContacts = LongWayHomeUniversitySequence.FootContacts(this._config.fieldRun);
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x00267798 File Offset: 0x00265998
		private void CaptureBreastRest()
		{
			if (this._left.Spring == null || this._right.Spring == null)
			{
				return;
			}
			foreach (LongWayHomeStadiumSequence.BreastBones breastBones in new LongWayHomeStadiumSequence.BreastBones[] { this._left, this._right })
			{
				breastBones.SpringPosition = breastBones.Spring.localPosition;
				breastBones.SpringRotation = breastBones.Spring.localRotation;
				if (!(breastBones.Base == null))
				{
					breastBones.BasePosition = breastBones.Base.localPosition;
					breastBones.BaseRotation = breastBones.Base.localRotation;
					breastBones.Arm = Mathf.Max(0.03f, Vector3.Distance(breastBones.Spring.position, breastBones.Base.position));
				}
			}
			this._hasRest = true;
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x060060DB RID: 24795 RVA: 0x00267884 File Offset: 0x00265A84
		private bool CanBounce
		{
			get
			{
				return this._config.animeBounce && AnimeBodyPhysics.AnimeSelected && this._left.Spring != null && this._right.Spring != null && this._hasRest && this._bounceScale > 0.001f && this._config.bounceGain > 0.001f;
			}
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x002678F4 File Offset: 0x00265AF4
		private void StartBounce()
		{
			this._bounceWanted = true;
			this.BeginBounce();
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x00267903 File Offset: 0x00265B03
		private void StopBounce()
		{
			this._bounceWanted = false;
			this.EndBounce();
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x00267912 File Offset: 0x00265B12
		private void FollowBodyPhysicsSetting()
		{
			if (!this._bounceWanted)
			{
				return;
			}
			if (this._bouncing && !AnimeBodyPhysics.AnimeSelected)
			{
				this.EndBounce();
				return;
			}
			if (!this._bouncing)
			{
				this.BeginBounce();
			}
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x00267944 File Offset: 0x00265B44
		private void BeginBounce()
		{
			if (!this.CanBounce || this._bouncing)
			{
				return;
			}
			this._bouncing = true;
			this._bounceWeight = 0f;
			this._bounceClipTime = double.NaN;
			float num = this._config.bounceFrequency * this._config.bounceAsymmetry * 0.5f;
			this._left.Frequency = this._config.bounceFrequency - num;
			this._right.Frequency = this._config.bounceFrequency + num;
			this._left.Settled = (this._right.Settled = false);
			foreach (GameObject gameObject in this._breastCloths)
			{
				if (!(gameObject == null) && gameObject.activeSelf)
				{
					gameObject.SetActive(false);
					this._heldCloths.Add(gameObject);
				}
			}
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x00267A50 File Offset: 0x00265C50
		private void EndBounce()
		{
			this._bouncing = false;
			this._bounceWeight = 0f;
			if (this._bounceApplied && this._hasRest)
			{
				foreach (LongWayHomeStadiumSequence.BreastBones breastBones in new LongWayHomeStadiumSequence.BreastBones[] { this._left, this._right })
				{
					if (breastBones.Spring != null)
					{
						breastBones.Spring.SetLocalPositionAndRotation(breastBones.SpringPosition, breastBones.SpringRotation);
					}
					if (breastBones.Base != null)
					{
						breastBones.Base.SetLocalPositionAndRotation(breastBones.BasePosition, breastBones.BaseRotation);
					}
				}
			}
			this._bounceApplied = false;
			foreach (GameObject gameObject in this._heldCloths)
			{
				if (!(gameObject == null))
				{
					gameObject.SetActive(true);
					foreach (MagicaCloth magicaCloth in gameObject.GetComponents<MagicaCloth>())
					{
						if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
						{
							magicaCloth.ResetCloth(false);
						}
					}
				}
			}
			this._heldCloths.Clear();
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x00267B94 File Offset: 0x00265D94
		private void ResetBreasts()
		{
			if (!this._bouncing)
			{
				return;
			}
			this._bounceWeight = 0f;
			this._bounceClipTime = double.NaN;
			this._left.Settled = (this._right.Settled = false);
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x00267BDE File Offset: 0x00265DDE
		private void ReleaseBreasts()
		{
			this.StopBounce();
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x00267BE8 File Offset: 0x00265DE8
		private void TickBreasts(float deltaTime)
		{
			this.FollowBodyPhysicsSetting();
			if (!this._bouncing)
			{
				return;
			}
			this._bounceWeight = Mathf.MoveTowards(this._bounceWeight, this._moving ? 1f : 0f, deltaTime / Mathf.Max(0.05f, this._config.bounceBlendSeconds));
			int num = this.TookStep();
			if (num >= 0)
			{
				this.Kick((num == 0) ? this._left : this._right, 1f);
				this.Kick((num == 0) ? this._right : this._left, -this._config.bounceCounterKick);
			}
			this.SwingBreast(this._left, deltaTime);
			this.SwingBreast(this._right, deltaTime);
			Vector3 vector = this._left.Mass - this._left.Anchor;
			Vector3 vector2 = this._right.Mass - this._right.Anchor;
			Vector3 vector3 = (vector + vector2) * 0.5f * (1f - this._config.bounceUnison);
			float num2 = this._config.bounceGain * this._bounceScale * this._bounceWeight;
			this.ApplyBounce(this._left, (vector - vector3) * num2);
			this.ApplyBounce(this._right, (vector2 - vector3) * num2);
			this._bounceApplied = true;
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x00267D5C File Offset: 0x00265F5C
		private int TookStep()
		{
			LongWayHomeStadiumSequence.PoseSlot poseSlot;
			if (!this._slots.TryGetValue(LongWayHomeStadiumSequence.BodyPose.Run, out poseSlot) || !poseSlot.Moving.IsValid<AnimationClipPlayable>())
			{
				return -1;
			}
			double time = poseSlot.Moving.GetTime<AnimationClipPlayable>();
			double bounceClipTime = this._bounceClipTime;
			this._bounceClipTime = time;
			if (double.IsNaN(bounceClipTime) || !this._moving)
			{
				return -1;
			}
			for (int i = 0; i < this._bounceContacts.Length; i++)
			{
				if (LongWayHomeUniversitySequence.CrossedFootContact(bounceClipTime, time, this._config.fieldRun.length, this._bounceContacts[i]))
				{
					return i % 2;
				}
			}
			return -1;
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x00267DF0 File Offset: 0x00265FF0
		private void Kick(LongWayHomeStadiumSequence.BreastBones breast, float strength)
		{
			if (!breast.Moving)
			{
				return;
			}
			float num = 1f + this._config.bounceKickRandom * ((float)this._bounceRandom.NextDouble() * 2f - 1f);
			breast.Velocity += Vector3.down * (this._config.bounceKick * strength * num);
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x00267E5C File Offset: 0x0026605C
		private void SwingBreast(LongWayHomeStadiumSequence.BreastBones breast, float deltaTime)
		{
			Transform parent = breast.Spring.parent;
			Vector3 vector = ((parent != null) ? parent.TransformPoint(breast.SpringPosition) : breast.SpringPosition);
			if (deltaTime <= 0f)
			{
				return;
			}
			if (!breast.Settled)
			{
				breast.Mass = (breast.Anchor = vector);
				breast.Velocity = Vector3.zero;
				breast.Settled = true;
				breast.Moving = false;
				return;
			}
			if (!breast.Moving)
			{
				breast.Velocity = (vector - breast.Anchor) / deltaTime;
				breast.Mass += vector - breast.Anchor;
				breast.Anchor = vector;
				breast.Moving = true;
				return;
			}
			Vector3 mass = breast.Mass;
			Vector3 velocity = breast.Velocity;
			LongWayHomeStadiumSequence.SpringStep(ref mass, ref velocity, breast.Anchor, vector, deltaTime, 6.2831855f * breast.Frequency, this._config.bounceDamping);
			Vector3 vector2 = Vector3.ClampMagnitude(mass - vector, this._config.bounceMaxSwing);
			breast.Mass = vector + vector2;
			breast.Velocity = Vector3.ClampMagnitude(velocity, 6f);
			breast.Anchor = vector;
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x00267F94 File Offset: 0x00266194
		public static void SpringStep(ref Vector3 mass, ref Vector3 velocity, Vector3 from, Vector3 to, float deltaTime, float omega, float damping)
		{
			if (deltaTime <= 0f)
			{
				return;
			}
			int num = Mathf.Clamp(Mathf.CeilToInt(deltaTime / 0.004166667f), 1, 64);
			float num2 = deltaTime / (float)num;
			Vector3 vector = (to - from) / deltaTime;
			float num3 = omega * omega;
			float num4 = 2f * Mathf.Clamp(damping, 0.01f, 1f) * omega;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = Vector3.Lerp(from, to, (float)i / (float)num);
				Vector3 vector3 = num3 * (vector2 - mass) - num4 * (velocity - vector);
				velocity += vector3 * num2;
				mass += velocity * num2;
			}
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x0026807C File Offset: 0x0026627C
		private void ApplyBounce(LongWayHomeStadiumSequence.BreastBones breast, Vector3 swing)
		{
			Vector3 up = this._player.up;
			float num = Vector3.Dot(swing, up);
			float num2 = Vector3.Dot(swing, this._player.right) * this._config.bounceSway;
			float bounceMaxAngle = this._config.bounceMaxAngle;
			float num3 = Mathf.Clamp(num / breast.Arm * 57.29578f, -bounceMaxAngle, bounceMaxAngle);
			float num4 = Mathf.Clamp(num2 / breast.Arm * 57.29578f, -bounceMaxAngle * 0.5f, bounceMaxAngle * 0.5f);
			Transform parent = breast.Spring.parent;
			Vector3 vector = ((parent != null) ? parent.InverseTransformDirection(this._player.right) : Vector3.right);
			Vector3 vector2 = ((parent != null) ? parent.InverseTransformDirection(this._player.forward) : Vector3.forward);
			Quaternion quaternion = Quaternion.AngleAxis(-num3, vector) * Quaternion.AngleAxis(num4, vector2);
			Vector3 vector3 = ((breast.Base != null) ? breast.BasePosition : breast.SpringPosition);
			breast.Spring.SetLocalPositionAndRotation(vector3 + quaternion * (breast.SpringPosition - vector3), quaternion * breast.SpringRotation);
			if (breast.Base != null)
			{
				breast.Base.localRotation = Quaternion.Slerp(Quaternion.identity, quaternion, this._config.bounceBaseFollow) * breast.BaseRotation;
			}
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x002681F8 File Offset: 0x002663F8
		private static Transform FindChildIgnoreCase(Transform parent, string childName)
		{
			if (parent == null)
			{
				return null;
			}
			if (string.Equals(parent.name, childName, StringComparison.OrdinalIgnoreCase))
			{
				return parent;
			}
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform transform = LongWayHomeStadiumSequence.FindChildIgnoreCase(parent.GetChild(i), childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060060EA RID: 24810 RVA: 0x0026824C File Offset: 0x0026644C
		private void CreateCamera()
		{
			GameObject gameObject = new GameObject("Camera - Stadium Opening");
			gameObject.transform.SetParent(base.transform, false);
			this._camera = gameObject.AddComponent<Camera>();
			this._camera.enabled = false;
			this._camera.nearClipPlane = 0.03f;
			this._camera.farClipPlane = 600f;
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
			this._crowdCamera = gameObject.AddComponent<GPUICamera>();
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x002683BC File Offset: 0x002665BC
		private void DisableCamera()
		{
			if (this._crowdCamera != null)
			{
				this._crowdCamera.enabled = false;
			}
			if (this._camera == null)
			{
				return;
			}
			this._camera.enabled = false;
			AudioListener component = this._camera.GetComponent<AudioListener>();
			if (component != null)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x0026841A File Offset: 0x0026661A
		private void StartShots(LongWayHomeStadiumConfigSO.BodyShot[] shots)
		{
			this._shots = shots;
			this._single = null;
			this.SelectShot(0);
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x00268434 File Offset: 0x00266634
		private void SelectShot(int index)
		{
			if (this._shots == null || this._shots.Length == 0)
			{
				return;
			}
			this._single = null;
			this._orbiting = false;
			this._shotIndex = (index % this._shots.Length + this._shots.Length) % this._shots.Length;
			this._fieldOfView = this._shots[this._shotIndex].fieldOfView;
			this.UpdateCamera(true);
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x002684A2 File Offset: 0x002666A2
		private void NextShot()
		{
			this.SelectShot(this._shotIndex + 1);
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x002684B2 File Offset: 0x002666B2
		private void ActivateShot(LongWayHomeStadiumConfigSO.BodyShot shot)
		{
			if (shot == null)
			{
				return;
			}
			this._single = shot;
			this._orbiting = false;
			this._fieldOfView = shot.fieldOfView;
			this.UpdateCamera(true);
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x060060F0 RID: 24816 RVA: 0x002684D9 File Offset: 0x002666D9
		private LongWayHomeStadiumConfigSO.BodyShot CurrentShot
		{
			get
			{
				LongWayHomeStadiumConfigSO.BodyShot bodyShot;
				if ((bodyShot = this._single) == null)
				{
					if (this._shots == null || this._shots.Length == 0)
					{
						return null;
					}
					bodyShot = this._shots[this._shotIndex];
				}
				return bodyShot;
			}
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x00268508 File Offset: 0x00266708
		private void UpdateCamera(bool snap)
		{
			if (this._camera == null || this._animator == null)
			{
				return;
			}
			float frameYaw = this.FrameYaw;
			if (snap)
			{
				this._cameraYaw = frameYaw;
				this._cameraYawVelocity = 0f;
			}
			else
			{
				this._cameraYaw = Mathf.SmoothDampAngle(this._cameraYaw, frameYaw, ref this._cameraYawVelocity, 0.45f);
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
			LongWayHomeStadiumConfigSO.BodyShot currentShot = this.CurrentShot;
			if (currentShot == null)
			{
				return;
			}
			Transform transform2 = this.Bone(currentShot.anchor);
			Quaternion quaternion = Quaternion.Euler(0f, this._cameraYaw, 0f);
			Vector3 vector = transform2.position + quaternion * currentShot.lookOffset;
			Vector3 vector2 = this.KeepClear(vector, transform2.position + quaternion * currentShot.cameraOffset, snap);
			this.Place(transform, vector2, vector, snap, currentShot.damping);
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x00268664 File Offset: 0x00266864
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
			view.rotation = Quaternion.LookRotation(vector, Vector3.up);
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x00268710 File Offset: 0x00266910
		private Vector3 KeepClear(Vector3 focus, Vector3 desired, bool snap)
		{
			Vector3 vector = desired - focus;
			float magnitude = vector.magnitude;
			float num = 1f;
			if (magnitude > 0.01f)
			{
				float num2 = magnitude;
				int num3 = Physics.SphereCastNonAlloc(focus, 0.1f, vector / magnitude, this._cameraHits, magnitude, -1, QueryTriggerInteraction.Ignore);
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

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x060060F4 RID: 24820 RVA: 0x00268861 File Offset: 0x00266A61
		private float FrameYaw
		{
			get
			{
				return this._player.eulerAngles.y + ((LongWayHomeStadiumSequence.BodySlot(this._pose) == LongWayHomeStadiumSequence.BodyPose.Wall) ? this._wallYawOffset : 0f);
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x060060F5 RID: 24821 RVA: 0x0026888E File Offset: 0x00266A8E
		private Quaternion BodyFrame
		{
			get
			{
				return Quaternion.Euler(0f, this.FrameYaw, 0f);
			}
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x002688A8 File Offset: 0x00266AA8
		private float MeasuredBodyYaw()
		{
			Vector3 vector = this.Bone(HumanBodyBones.RightUpperArm).position - this.Bone(HumanBodyBones.LeftUpperArm).position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 1E-06f)
			{
				return this._player.eulerAngles.y;
			}
			Vector3 vector2 = Vector3.Cross(vector.normalized, Vector3.up);
			return Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x0026892C File Offset: 0x00266B2C
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

		// Token: 0x060060F8 RID: 24824 RVA: 0x002689B4 File Offset: 0x00266BB4
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
				this._runDayLength = timeSystem.dayLength;
				this._clockHeld = true;
			}
			this._heldHour = hour;
			timeSystem.dayLength = 0f;
			timeSystem.startTime = hour;
			timeSystem.timeline = hour;
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x00268A40 File Offset: 0x00266C40
		private void HoldClockTick()
		{
			if (!this._clockHeld || this._azure == null || this._azure.timeSystem == null)
			{
				return;
			}
			AzureTimeSystem timeSystem = this._azure.timeSystem;
			if (timeSystem.dayLength > 0f)
			{
				this._runDayLength = timeSystem.dayLength;
				timeSystem.dayLength = 0f;
			}
			if (!Mathf.Approximately(timeSystem.timeline, this._heldHour))
			{
				timeSystem.timeline = this._heldHour;
			}
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x00268AC0 File Offset: 0x00266CC0
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
			timeSystem.startTime = this._heldHour;
			timeSystem.timeline = this._heldHour;
			timeSystem.dayLength = ((this._runDayLength > 0f) ? this._runDayLength : 24f);
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x00268B3B File Offset: 0x00266D3B
		private IEnumerator Commentary()
		{
			SimpleDialogueManager dialogue = SimpleDialogueManager.Instance;
			if (dialogue == null || string.IsNullOrEmpty(this._config.commentatorLine))
			{
				Debug.LogWarning("[LongWayHomeStadium] No dialogue box for the commentator's line.", this);
				yield break;
			}
			this.ShowDialogueBox(dialogue, true);
			bool closed = false;
			DialogueLine dialogueLine = new DialogueLine
			{
				speakerName = this._config.commentatorSpeaker,
				text = this._config.commentatorLine,
				localizationScope = "stadium",
				speakerLocalizationScope = "dialogue_speaker"
			};
			dialogue.StartOwnedDialogue(this, new DialogueLine[] { dialogueLine }, delegate
			{
				closed = true;
			});
			while (!closed && dialogue != null && dialogue.IsDialogueOwnedBy(this))
			{
				yield return null;
			}
			this.ShowDialogueBox(dialogue, false);
			yield break;
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x00268B4C File Offset: 0x00266D4C
		private void EndCommentary()
		{
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance != null)
			{
				instance.TryEndDialogue(this, false);
			}
			this.ShowDialogueBox(instance, false);
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x00268B7C File Offset: 0x00266D7C
		private void ShowDialogueBox(SimpleDialogueManager dialogue, bool show)
		{
			if (show)
			{
				GameObject gameObject = ((dialogue != null) ? dialogue.DialoguePanelVisual : null);
				Canvas canvas = ((gameObject != null) ? gameObject.GetComponentInParent<Canvas>(true) : null);
				this._dialogueCanvas = ((canvas != null) ? canvas.rootCanvas : null);
			}
			if (this._dialogueCanvas == null)
			{
				return;
			}
			using (List<LongWayHomeStadiumSequence.BehaviourState>.Enumerator enumerator = this._presentation.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Component == this._dialogueCanvas)
					{
						this._dialogueCanvas.enabled = show;
						break;
					}
				}
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x060060FE RID: 24830 RVA: 0x00268C38 File Offset: 0x00266E38
		// (set) Token: 0x060060FF RID: 24831 RVA: 0x00268C40 File Offset: 0x00266E40
		public LongWayHomeStadiumSequence.StoryPhase Phase { get; private set; }

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06006100 RID: 24832 RVA: 0x00268C49 File Offset: 0x00266E49
		// (set) Token: 0x06006101 RID: 24833 RVA: 0x00268C51 File Offset: 0x00266E51
		public bool IsReady { get; private set; }

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06006102 RID: 24834 RVA: 0x00268C5A File Offset: 0x00266E5A
		// (set) Token: 0x06006103 RID: 24835 RVA: 0x00268C62 File Offset: 0x00266E62
		public bool IsPreparedForReveal { get; private set; }

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06006104 RID: 24836 RVA: 0x00268C6B File Offset: 0x00266E6B
		public bool OwnsDestinationReveal
		{
			get
			{
				return base.isActiveAndEnabled && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Failed && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Done;
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06006105 RID: 24837 RVA: 0x00268C8E File Offset: 0x00266E8E
		public Transform RuntimePlayer
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06006106 RID: 24838 RVA: 0x00268C98 File Offset: 0x00266E98
		public bool CanSkip
		{
			get
			{
				return this.IsReady && this._captured && !this._skipRequested && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Loading && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Handoff && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Done && this.Phase != LongWayHomeStadiumSequence.StoryPhase.Failed;
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00268CE8 File Offset: 0x00266EE8
		public static LongWayHomeStadiumSequence GetPresenterForScene(Scene scene)
		{
			if (!(LongWayHomeStadiumSequence._activePresenter != null) || !LongWayHomeStadiumSequence._activePresenter.isActiveAndEnabled || !(LongWayHomeStadiumSequence._activePresenter.gameObject.scene == scene))
			{
				return null;
			}
			return LongWayHomeStadiumSequence._activePresenter;
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x00268D24 File Offset: 0x00266F24
		public static LongWayHomeStadiumSequence Install(Scene scene)
		{
			LongWayHomeStadiumSequence presenterForScene = LongWayHomeStadiumSequence.GetPresenterForScene(scene);
			if (presenterForScene != null)
			{
				return presenterForScene;
			}
			GameObject gameObject = new GameObject("Long Way Home - Stadium Opening");
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			return gameObject.AddComponent<LongWayHomeStadiumSequence>();
		}

		// Token: 0x06006109 RID: 24841 RVA: 0x00268D59 File Offset: 0x00266F59
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

		// Token: 0x0600610A RID: 24842 RVA: 0x00268D91 File Offset: 0x00266F91
		private IEnumerator SkipToGameplay()
		{
			this.Phase = LongWayHomeStadiumSequence.StoryPhase.Handoff;
			this.EndCommentary();
			this.SetSlowMotion(false, true);
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			this.PrepareHandoffPose();
			yield return new WaitForSecondsRealtime(this._config.handoffHoldSeconds);
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x00268DA0 File Offset: 0x00266FA0
		public void FailPreparation(string reason)
		{
			if (this.IsPreparedForReveal || !this.OwnsDestinationReveal)
			{
				return;
			}
			base.StopAllCoroutines();
			this.Fail(reason);
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x00268DC0 File Offset: 0x00266FC0
		public void PrepareForSceneExit()
		{
			if (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Done)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (this.Phase != LongWayHomeStadiumSequence.StoryPhase.Failed)
			{
				this.Phase = LongWayHomeStadiumSequence.StoryPhase.Handoff;
			}
			base.enabled = false;
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x00268DF2 File Offset: 0x00266FF2
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			LongWayHomeStadiumSequence._activePresenter = this;
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetBlack();
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x00268E11 File Offset: 0x00267011
		private IEnumerator Start()
		{
			this._config = LongWayHomeStadiumConfigSO.Load();
			if (this._config == null)
			{
				this.Fail("Missing Resources/LongWayHomeStadiumConfig.");
				yield break;
			}
			string text;
			if (!this._config.HasRequiredAssets(out text))
			{
				this.Fail("The stadium opening config is missing '" + text + "'.");
				yield break;
			}
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
					this.Fail("The persistent player did not finish initialization in the Stadium.");
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
			while (LongWayHomeOpeningReadinessWait.HasUpstreamFailure || !PlayerActivityGate.TryAcquire("LongWayHomeStadium#" + base.GetInstanceID().ToString(), this, PlayerActivity.None, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._lease, false, null))
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
			this.CapturePlayerControl();
			this.CaptureNipples();
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
			this.CaptureStadium();
			base.StartCoroutine(this.SettleArrivalPopulation());
			string text2;
			if (!this.PrepareBag(out text2))
			{
				this.Fail(text2);
				yield break;
			}
			this.CreateCamera();
			this.BuildCielGraph();
			this.PrepareBreasts();
			this.StartMix();
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.Wall, 0f);
			this.TeleportPlayer(this._config.wallMark, Quaternion.Euler(0f, this._config.wallYaw, 0f));
			this.BeginFace(this._config.hornyFaceAnimation);
			yield return null;
			this._wallYawOffset = Mathf.DeltaAngle(this._player.eulerAngles.y, this.MeasuredBodyYaw());
			this.TeleportPlayer(this._config.wallMark, Quaternion.Euler(0f, this._config.wallYaw - this._wallYawOffset, 0f));
			this.StartShots(this._config.wallShots);
			this.HoldClock(this._config.dayHour);
			yield return LongWayHomeStadiumSequence.RefreshLighting();
			yield return this.WarmPresentation();
			if (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Failed)
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

		// Token: 0x0600610F RID: 24847 RVA: 0x00268E20 File Offset: 0x00267020
		private bool PlayerReady()
		{
			if (SceneManager.GetActiveScene() != base.gameObject.scene || !SceneBootLoader.CoreReady || !GameSystemsBootstrapper.GameplayReady || GameSystemsBootstrapper.Instance == null || GameSystemsBootstrapper.Instance.InitializedSceneName != base.gameObject.scene.name || PlayerRegistry.Player == null)
			{
				return false;
			}
			ClothInventorySyncer componentInChildren = PlayerRegistry.Player.GetComponentInChildren<ClothInventorySyncer>(true);
			return componentInChildren == null || componentInChildren.SyncCompleted;
		}

		// Token: 0x06006110 RID: 24848 RVA: 0x00268EAF File Offset: 0x002670AF
		private IEnumerator PlayStory()
		{
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Wall);
			base.StartCoroutine(this.FadeScreen(false, 0.8f));
			while (!this._continueRequested)
			{
				yield return null;
			}
			this.CaptureBreastRest();
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Bag);
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.HoldBag, this._config.poseBlendSeconds);
			this.HoldBagLow(true);
			this.ShowBagInHand();
			this.ActivateShot(this._config.bagShot);
			this.SetLookAt(true, this._config.lookAtBlendSeconds);
			yield return new WaitForSeconds(this._config.bagHoldSeconds);
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Wear);
			this.PutOnBag();
			this.SetLookAt(false, this._config.lookAtBlendSeconds);
			yield return new WaitForSeconds(this._config.wornHoldSeconds);
			yield return this.FadeScreen(true, this._config.transitionSeconds);
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Run);
			Vector3[] array = this.FieldStart();
			this.TeleportPlayer(array[0], LongWayHomeStadiumSequence.FacingRotation(array[0], array[1]));
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.Run, 0f);
			this._runWeight = 1f;
			this.ApplyBodyWeights();
			this.StartBounce();
			this.StartShots(this._config.runShots);
			Coroutine run = base.StartCoroutine(this.FieldLoop());
			this.CheerCrowd(true);
			this.FocusScreenOnHer();
			yield return null;
			yield return this.FadeScreen(false, this._config.transitionSeconds);
			while (!this._continueRequested)
			{
				yield return null;
			}
			this.SetSlowMotion(false, false);
			yield return this.FadeScreen(true, this._config.quickCutSeconds);
			if (run != null)
			{
				base.StopCoroutine(run);
			}
			this._moving = false;
			this.StopFootsteps();
			this.StopBounce();
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Center);
			this.TeleportPlayer(this._config.centerMark, Quaternion.Euler(0f, this._config.centerYaw, 0f));
			this._runWeight = 0f;
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.LookAtMe, 0f);
			this.StartShots(this._config.centerShots);
			yield return null;
			base.StartCoroutine(this.FadeScreen(false, this._config.quickCutSeconds));
			yield return new WaitForSeconds(this._config.giggleDelaySeconds);
			this.PlayGiggle();
			Coroutine underVoice = base.StartCoroutine(this.CrowdUnderHerVoice());
			while (!this._continueRequested)
			{
				yield return null;
			}
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.BagOff);
			if (underVoice != null)
			{
				base.StopCoroutine(underVoice);
			}
			this.StopGiggle();
			this.ScreenOnHerFace();
			this.CheerCrowd(false);
			this.ActivateShot(this._config.bagOffShot);
			yield return new WaitForSeconds(this._config.gustDelaySeconds);
			yield return this.GustTakesTheBag();
			this.DuckCrowd(1f, 0.3f);
			this.CheerCrowd(true);
			this._face.Show(this._config.embarrassedFaceAnimation);
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.Embarrassed, this._config.poseBlendSeconds * 0.6f);
			this.PlayGasp();
			yield return new WaitForSeconds(this._config.realizeSeconds);
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Exposed);
			yield return this.Commentary();
			this.BeginPhase(LongWayHomeStadiumSequence.StoryPhase.Escape);
			this.SetFaceCover(true, this._config.poseBlendSeconds * 0.6f);
			this.ActivateShot(this._config.escapeFaceShot);
			Coroutine escape = base.StartCoroutine(this.MoveAlong(this._config.escapePath, this._config.escapeMetersPerSecond, false, false, true));
			yield return new WaitForSeconds(this._config.escapeFaceSeconds);
			this.ActivateShot(this._config.escapeSideShot);
			yield return new WaitForSeconds(this._config.escapeSideSeconds);
			this.SetSlowMotion(false, false);
			this.CheerCrowd(false);
			yield return this.FadeScreen(true, this._config.handoffFadeSeconds);
			if (escape != null)
			{
				base.StopCoroutine(escape);
			}
			this._moving = false;
			this.StopFootsteps();
			this.Phase = LongWayHomeStadiumSequence.StoryPhase.Handoff;
			LongWayHomeCinematicSkip hints = this._hints;
			if (hints != null)
			{
				hints.Dispose();
			}
			this.PrepareHandoffPose();
			yield return new WaitForSecondsRealtime(this._config.handoffHoldSeconds);
			this.CompleteHandoff();
			yield break;
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x00268EBE File Offset: 0x002670BE
		private void BeginPhase(LongWayHomeStadiumSequence.StoryPhase phase)
		{
			this.Phase = phase;
			this._continueRequested = false;
			this._phaseStartedAt = Time.unscaledTime;
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x00268EDC File Offset: 0x002670DC
		private void PrepareHandoffPose()
		{
			this._moving = false;
			this.StopFootsteps();
			this.StopGiggle();
			this.SetSlowMotion(false, true);
			this.SetLookAt(false, 0f);
			this.SetFaceCover(false, 0f);
			this.StopWind(0f);
			this.HideBag();
			LongWayHomeGymShowerFaceDriver face = this._face;
			if (face != null)
			{
				face.Show(this._config.embarrassedFaceAnimation);
			}
			this._runWeight = 0f;
			this.UseBodyPose(LongWayHomeStadiumSequence.BodyPose.Embarrassed, 0f);
			this.TeleportPlayer(this._config.handoffMark, Quaternion.Euler(0f, this._config.handoffYaw, 0f));
			this.BeginRunPopulation();
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x00268F94 File Offset: 0x00267194
		private void CompleteHandoff()
		{
			this.ReleasePlayer();
			if (this._gameplayCamera != null && this._gameplayCamera.isInit)
			{
				this._gameplayCamera.ResetAngleWithoutSmooth();
			}
			LongWayHomeRuntimeController instance = LongWayHomeRuntimeController.Instance;
			if (instance == null || !instance.BeginRunFromPoiPrologue())
			{
				Debug.LogWarning("[LongWayHomeStadium] No prepared Long Way Home run took over the Stadium.", this);
			}
			this.Phase = LongWayHomeStadiumSequence.StoryPhase.Done;
			ScreenFadeController instance2 = ScreenFadeController.Instance;
			if (instance2 == null)
			{
				return;
			}
			instance2.FadeIn(this._config.handoffFadeSeconds, null);
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x00269014 File Offset: 0x00267214
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
			if (this._clockHeld)
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
			this.TickSlowMotion();
			this.TickCrowd();
			this.TickBodyPose(Time.deltaTime);
			this.TickLookAt(Time.deltaTime);
			this.TickBag(Time.deltaTime);
			this.TickWind(Time.deltaTime);
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x002690F4 File Offset: 0x002672F4
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
			this.HoldErectNipples();
			this.TickBreasts(Time.deltaTime);
			this.UpdateCamera(false);
			this.FollowBag(Time.deltaTime);
			this.TickFootballers(Time.deltaTime);
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x002691B4 File Offset: 0x002673B4
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

		// Token: 0x06006117 RID: 24855 RVA: 0x0026920C File Offset: 0x0026740C
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
			this.ResetBreasts();
			this.UpdateCamera(true);
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x002692B4 File Offset: 0x002674B4
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

		// Token: 0x06006119 RID: 24857 RVA: 0x002692C4 File Offset: 0x002674C4
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

		// Token: 0x0600611A RID: 24858 RVA: 0x00269534 File Offset: 0x00267734
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

		// Token: 0x0600611B RID: 24859 RVA: 0x00269684 File Offset: 0x00267884
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
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x0026977C File Offset: 0x0026797C
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

		// Token: 0x0600611D RID: 24861 RVA: 0x002698E8 File Offset: 0x00267AE8
		private void Suppress(Behaviour component)
		{
			this._presentation.Add(new LongWayHomeStadiumSequence.BehaviourState
			{
				Component = component,
				Enabled = component.enabled
			});
			component.enabled = false;
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x00269928 File Offset: 0x00267B28
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

		// Token: 0x0600611F RID: 24863 RVA: 0x002699E0 File Offset: 0x00267BE0
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

		// Token: 0x06006120 RID: 24864 RVA: 0x00269A90 File Offset: 0x00267C90
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

		// Token: 0x06006121 RID: 24865 RVA: 0x00269B28 File Offset: 0x00267D28
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

		// Token: 0x06006122 RID: 24866 RVA: 0x00269B3E File Offset: 0x00267D3E
		private static IEnumerator RefreshLighting()
		{
			EnvironmentLightingRefresh.ForceRequestForActiveScene();
			yield return EnvironmentLightingRefresh.WaitUntilReady(12f);
			yield break;
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x00269B48 File Offset: 0x00267D48
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

		// Token: 0x06006124 RID: 24868 RVA: 0x00269B84 File Offset: 0x00267D84
		private void Fail(string reason)
		{
			this.Phase = LongWayHomeStadiumSequence.StoryPhase.Failed;
			Debug.LogError("[LongWayHomeStadium] " + reason, this);
			this.ReleasePlayer();
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

		// Token: 0x06006125 RID: 24869 RVA: 0x00269BD0 File Offset: 0x00267DD0
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			base.StopAllCoroutines();
			this.ReleasePlayer();
			if (LongWayHomeStadiumSequence._activePresenter == this)
			{
				LongWayHomeStadiumSequence._activePresenter = null;
			}
		}

		// Token: 0x06006126 RID: 24870 RVA: 0x00269BF9 File Offset: 0x00267DF9
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.ReleasePlayer();
			this.DestroyBag();
			this.DestroyWind();
			if (LongWayHomeStadiumSequence._activePresenter == this)
			{
				LongWayHomeStadiumSequence._activePresenter = null;
			}
		}

		// Token: 0x06006127 RID: 24871 RVA: 0x00269C28 File Offset: 0x00267E28
		private void ReleasePlayer()
		{
			LongWayHomeCinematicSkip hints = this._hints;
			if (hints != null)
			{
				hints.Dispose();
			}
			this._hints = null;
			this.IsReady = false;
			this.EndCommentary();
			this.SetSlowMotion(false, true);
			this.ReleaseClock();
			this.ReleaseMix();
			this.StopFootsteps();
			this.StopGiggle();
			this.CheerCrowd(false);
			this.ReleaseBreasts();
			this.HideBag();
			this.ReleaseWind();
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
			this.ReleasePopulationFocus();
			this.ReleaseStadium();
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
				this.ReleaseNipples();
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
			foreach (LongWayHomeStadiumSequence.BehaviourState behaviourState in this._presentation)
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

		// Token: 0x06006128 RID: 24872 RVA: 0x00269FC8 File Offset: 0x002681C8
		private void CaptureFootballers()
		{
			this._watchers.Clear();
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (FieldPlayerLookAtCiel fieldPlayerLookAtCiel in rootGameObjects[i].GetComponentsInChildren<FieldPlayerLookAtCiel>(false))
				{
					NPCSimpleLookAt npcsimpleLookAt = ((fieldPlayerLookAtCiel.lookAt != null) ? fieldPlayerLookAtCiel.lookAt : fieldPlayerLookAtCiel.GetComponentInChildren<NPCSimpleLookAt>());
					if (!(npcsimpleLookAt == null))
					{
						Animator animator = ((fieldPlayerLookAtCiel.animator != null) ? fieldPlayerLookAtCiel.animator : fieldPlayerLookAtCiel.GetComponentInChildren<Animator>());
						Transform transform = ((npcsimpleLookAt.headBone != null) ? npcsimpleLookAt.headBone : ((animator != null && animator.isHuman) ? animator.GetBoneTransform(HumanBodyBones.Head) : null));
						if (!(transform == null))
						{
							Transform transform2 = ((npcsimpleLookAt.neckBone != null) ? npcsimpleLookAt.neckBone : ((animator != null && animator.isHuman) ? animator.GetBoneTransform(HumanBodyBones.Neck) : null));
							this._watchers.Add(new LongWayHomeStadiumSequence.Watcher
							{
								Field = fieldPlayerLookAtCiel,
								LookAt = npcsimpleLookAt,
								Head = transform,
								Neck = transform2,
								HeadPose = transform.localRotation,
								NeckPose = ((transform2 != null) ? transform2.localRotation : Quaternion.identity)
							});
						}
					}
				}
			}
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x0026A15C File Offset: 0x0026835C
		private void TickFootballers(float deltaTime)
		{
			if (this._watchers.Count == 0 || this._animator == null)
			{
				return;
			}
			Vector3 position = this.Bone(HumanBodyBones.Head).position;
			foreach (LongWayHomeStadiumSequence.Watcher watcher in this._watchers)
			{
				if (!(watcher.Head == null) && !(watcher.Field == null) && !(watcher.LookAt == null))
				{
					NPCSimpleLookAt lookAt = watcher.LookAt;
					float num = watcher.Field.lookRange + (watcher.Looking ? watcher.Field.exitHysteresis : 0f);
					watcher.Looking = (position - watcher.Field.transform.position).sqrMagnitude <= num * num;
					Vector3 vector = (watcher.Looking ? LongWayHomeStadiumSequence.LookAngles(lookAt, watcher.Head, position) : Vector3.zero);
					float num2 = 1f - Mathf.Exp(-lookAt.smoothing * (watcher.Looking ? 1f : 1.5f) * deltaTime);
					watcher.HeadOffset = Vector3.Lerp(watcher.HeadOffset, vector * lookAt.headWeight, num2);
					watcher.NeckOffset = Vector3.Lerp(watcher.NeckOffset, vector * lookAt.neckWeight, num2);
					if (watcher.Neck != null && lookAt.neckWeight > 0.01f)
					{
						watcher.Neck.localRotation = watcher.NeckPose * Quaternion.Euler(watcher.NeckOffset);
					}
					if (lookAt.headWeight > 0.01f)
					{
						watcher.Head.localRotation = watcher.HeadPose * Quaternion.Euler(watcher.HeadOffset);
					}
				}
			}
		}

		// Token: 0x0600612A RID: 24874 RVA: 0x0026A368 File Offset: 0x00268568
		private static Vector3 LookAngles(NPCSimpleLookAt settings, Transform head, Vector3 target)
		{
			Vector3 vector = settings.transform.InverseTransformDirection((target - head.position).normalized);
			float num = Mathf.Clamp(Mathf.Atan2(vector.x, vector.z) * 57.29578f, -settings.maxHorizontalAngle, settings.maxHorizontalAngle);
			return new Vector3(settings.horizontalOnly ? 0f : Mathf.Clamp(-Mathf.Asin(Mathf.Clamp(vector.y, -1f, 1f)) * 57.29578f, -settings.maxVerticalAngle, settings.maxVerticalAngle), num, 0f);
		}

		// Token: 0x0600612B RID: 24875 RVA: 0x0026A40C File Offset: 0x0026860C
		private void ReleaseFootballers()
		{
			foreach (LongWayHomeStadiumSequence.Watcher watcher in this._watchers)
			{
				if (watcher.Head != null)
				{
					watcher.Head.localRotation = watcher.HeadPose;
				}
				if (watcher.Neck != null)
				{
					watcher.Neck.localRotation = watcher.NeckPose;
				}
			}
			this._watchers.Clear();
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x0600612C RID: 24876 RVA: 0x0026A4A4 File Offset: 0x002686A4
		private string BagPart
		{
			get
			{
				return this._config.headBag.mcStylePartId;
			}
		}

		// Token: 0x0600612D RID: 24877 RVA: 0x0026A4B8 File Offset: 0x002686B8
		private bool PrepareBag(out string error)
		{
			error = null;
			this._bagBone = LongWayHomeStadiumSequence.FindChild(this._player, "head.x");
			if (this._bagBone == null)
			{
				this._bagBone = this.Bone(HumanBodyBones.Head);
			}
			this._bagShapes = BlendShapeManager.ResolveForRig(this._style);
			this._hadBagEquipped = this._style.IsSlotEquipped(this.BagPart);
			if (this._hadBagEquipped)
			{
				this._style.SetSlotEquipped(this.BagPart, false);
			}
			if (this.EquipWornBag(false))
			{
				GameObject spawnedRoot = this._style.GetSpawnedRoot(this.BagPart);
				if (spawnedRoot != null)
				{
					Transform transform = ((spawnedRoot.transform.parent != null) ? spawnedRoot.transform.parent : this._bagBone);
					this._bagBone = transform;
					this._wornLocalPosition = spawnedRoot.transform.localPosition;
					this._wornLocalRotation = spawnedRoot.transform.localRotation;
					this._wornLocalScale = spawnedRoot.transform.localScale;
					this.CopyBagMaterials(spawnedRoot);
				}
				this.UnequipWornBag();
			}
			else
			{
				Debug.LogWarning("[LongWayHomeStadium] The Bag's style part did not equip while preparing; its prefab pose on the head bone is used.", this);
			}
			this._handBag = this.CreateVisual(this._config.headBag.handPrefab, "Head Bag - In Hand", false);
			this._flyBag = this.CreateVisual(this._config.flyingBag, "Head Bag - Gust", true);
			if (this._handBag == null || this._flyBag == null)
			{
				error = "The Bag's hand prefab or worn mesh could not be prepared.";
				return false;
			}
			if (this._flyMaterials.Count > 0)
			{
				Renderer[] componentsInChildren = this._flyBag.GetComponentsInChildren<Renderer>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sharedMaterials = this._flyMaterials.ToArray();
				}
			}
			return true;
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x0026A680 File Offset: 0x00268880
		private GameObject CreateVisual(GameObject prefab, string name, bool removeCloth)
		{
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject(name + " (holder)");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(base.transform, false);
			GameObject gameObject2;
			try
			{
				gameObject2 = Object.Instantiate<GameObject>(prefab, gameObject.transform, false);
				gameObject2.SetActive(false);
				gameObject2.transform.SetParent(base.transform, false);
			}
			finally
			{
				Object.DestroyImmediate(gameObject);
			}
			gameObject2.name = name;
			foreach (Transform transform in gameObject2.GetComponentsInChildren<Transform>(true))
			{
				if (transform != null && transform != gameObject2.transform && transform is RectTransform && !(transform.parent is RectTransform))
				{
					Object.DestroyImmediate(transform.gameObject);
				}
			}
			foreach (MonoBehaviour monoBehaviour in gameObject2.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour != null && (removeCloth || !(monoBehaviour is MagicaCloth)))
				{
					Object.DestroyImmediate(monoBehaviour);
				}
			}
			foreach (Component component in gameObject2.GetComponentsInChildren<Component>(true))
			{
				if (!(component == null) && !(component is Transform) && !(component is MeshFilter) && !(component is Renderer) && !(component is MagicaCloth))
				{
					Object.DestroyImmediate(component);
				}
			}
			return gameObject2;
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x0026A7F0 File Offset: 0x002689F0
		private void CopyBagMaterials(GameObject worn)
		{
			this._flyMaterials.Clear();
			Renderer componentInChildren = worn.GetComponentInChildren<Renderer>(true);
			if (componentInChildren == null)
			{
				return;
			}
			foreach (Material material in componentInChildren.sharedMaterials)
			{
				if (material != null)
				{
					this._flyMaterials.Add(new Material(material));
				}
			}
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x0026A850 File Offset: 0x00268A50
		private bool EquipWornBag(bool withShape)
		{
			if (!this._style.SetSlotEquipped(this.BagPart, true) && !this._style.EquipRuntimePreset(this.BagPart, this._config.headBag.mcStylePreset))
			{
				return false;
			}
			if (this._clothing != null && this._clothing.ManagesSlot(this.BagPart))
			{
				this._clothing.SetSlotVisible(this.BagPart, true);
			}
			else
			{
				this._style.SetSlotPhysicallyHidden(this.BagPart, false);
			}
			this._bagEquipped = true;
			if (withShape)
			{
				this.SetBagShape(true);
			}
			return true;
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x0026A8F3 File Offset: 0x00268AF3
		private void TakeOffWornPart()
		{
			if (!this._bagEquipped)
			{
				return;
			}
			this._bagEquipped = false;
			if (this._style != null)
			{
				this._style.SetSlotEquipped(this.BagPart, false);
			}
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x0026A926 File Offset: 0x00268B26
		private void UnequipWornBag()
		{
			this.SetBagShape(false);
			this.TakeOffWornPart();
		}

		// Token: 0x06006133 RID: 24883 RVA: 0x0026A938 File Offset: 0x00268B38
		private void SetBagShape(bool on)
		{
			if (this._bagShapes == null || this._config.headBag.blendshapes == null)
			{
				return;
			}
			foreach (WearableBlendshape wearableBlendshape in this._config.headBag.blendshapes)
			{
				if (wearableBlendshape != null && !string.IsNullOrEmpty(wearableBlendshape.blendshapeName))
				{
					if (on)
					{
						this._bagShapes.SetOverride(wearableBlendshape.blendshapeName, wearableBlendshape.wornValue, "LongWayHomeStadium.HeadBag", BlendShapePriority.Vehicle, false);
					}
					else
					{
						this._bagShapes.RemoveOverride(wearableBlendshape.blendshapeName, "LongWayHomeStadium.HeadBag", true, true, 0f);
					}
				}
			}
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x0026AA00 File Offset: 0x00268C00
		private void ShowBagInHand()
		{
			if (this._handBag == null)
			{
				return;
			}
			Transform transform = this._animator.GetBoneTransform(this._config.headBag.attachBone);
			if (transform == null)
			{
				transform = this.Bone(HumanBodyBones.RightHand);
			}
			this._handBag.transform.SetParent(transform, false);
			this._handBag.transform.localPosition = this._config.headBag.handPositionOffset;
			this._handBag.transform.localRotation = Quaternion.Euler(this._config.headBag.handRotationOffset);
			this._handBag.SetActive(true);
			foreach (MagicaCloth magicaCloth in this._handBag.GetComponentsInChildren<MagicaCloth>(true))
			{
				if (magicaCloth != null && magicaCloth.isActiveAndEnabled)
				{
					magicaCloth.ResetCloth(false);
				}
			}
			this._bagState = LongWayHomeStadiumSequence.BagState.InHand;
		}

		// Token: 0x06006135 RID: 24885 RVA: 0x0026AAEB File Offset: 0x00268CEB
		private void PutOnBag()
		{
			if (this._handBag != null)
			{
				this._handBag.SetActive(false);
			}
			this.HoldBagLow(false);
			this.EquipWornBag(true);
			this._bagState = LongWayHomeStadiumSequence.BagState.Worn;
		}

		// Token: 0x06006136 RID: 24886 RVA: 0x0026AB1D File Offset: 0x00268D1D
		private IEnumerator GustTakesTheBag()
		{
			this.StartWind(this._config.gustTrembleSeconds);
			Pose pose = this.WornBagPose();
			GameObject gameObject = (this._bagEquipped ? this._style.GetSpawnedRoot(this.BagPart) : null);
			if (gameObject != null)
			{
				pose = new Pose(gameObject.transform.position, gameObject.transform.rotation);
			}
			this.TakeOffWornPart();
			this._flyBag.transform.SetPositionAndRotation(pose.position, pose.rotation);
			this._flyBag.transform.localScale = this.BagWorldScale();
			this._flyBag.SetActive(true);
			this._shiver = 0f;
			this._bagState = LongWayHomeStadiumSequence.BagState.Shivering;
			float elapsed = 0f;
			while (elapsed < this._config.gustTrembleSeconds)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
			this.SetBagShape(false);
			this._flyVelocity = this.BodyFrame * this._config.gustVelocity;
			this._flySpin = this._config.gustSpin;
			this._flyElapsed = 0f;
			this._bagState = LongWayHomeStadiumSequence.BagState.Flying;
			if (this._config.gustSound != null)
			{
				this.Audio().PlayOneShot(this._config.gustSound, this._config.gustVolume);
			}
			this.StopWind(this._config.gustFadeSeconds);
			yield break;
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x0026AB2C File Offset: 0x00268D2C
		private void HideBag()
		{
			this.HoldBagLow(false);
			this._handWeight = 0f;
			this.ReleaseHand();
			if (this._handBag != null)
			{
				this._handBag.SetActive(false);
			}
			if (this._flyBag != null)
			{
				this._flyBag.SetActive(false);
			}
			this.UnequipWornBag();
			this._bagState = LongWayHomeStadiumSequence.BagState.None;
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x0026AB94 File Offset: 0x00268D94
		private void DestroyBag()
		{
			if (this._handBag != null)
			{
				Object.Destroy(this._handBag);
			}
			if (this._flyBag != null)
			{
				Object.Destroy(this._flyBag);
			}
			this._handBag = (this._flyBag = null);
			foreach (Material material in this._flyMaterials)
			{
				if (material != null)
				{
					Object.Destroy(material);
				}
			}
			this._flyMaterials.Clear();
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x0026AC3C File Offset: 0x00268E3C
		private void TickBag(float deltaTime)
		{
			if (this._bagState == LongWayHomeStadiumSequence.BagState.Flying && this._flyBag != null)
			{
				Vector3 position = this._flyBag.transform.position;
				LongWayHomeStadiumSequence.BagFlightStep(ref position, ref this._flyVelocity, this.BodyFrame * this._config.gustWind, this._config.gustGravity, this._config.gustDrag, deltaTime);
				this._flyBag.transform.position = position;
				this._flyBag.transform.rotation *= Quaternion.Euler(this._flySpin * deltaTime);
				this._flyElapsed += deltaTime;
				if (this._flyElapsed >= this._config.bagFlightSeconds)
				{
					this._flyBag.SetActive(false);
					this._bagState = LongWayHomeStadiumSequence.BagState.None;
				}
			}
			this.TickHand(deltaTime);
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x0026AD2C File Offset: 0x00268F2C
		public static void BagFlightStep(ref Vector3 position, ref Vector3 velocity, Vector3 wind, float gravity, float drag, float deltaTime)
		{
			velocity += (wind + Vector3.down * gravity - velocity * drag) * deltaTime;
			position += velocity * deltaTime;
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x0026AD94 File Offset: 0x00268F94
		private void FollowBag(float deltaTime)
		{
			if (this._bagState != LongWayHomeStadiumSequence.BagState.Shivering || this._flyBag == null)
			{
				return;
			}
			this._shiver = Mathf.Min(1f, this._shiver + deltaTime / Mathf.Max(0.1f, this._config.gustTrembleSeconds));
			float num = this._shiver * this._shiver;
			float num2 = Time.time * Mathf.Lerp(9f, 23f, this._shiver);
			float num3 = (Mathf.PerlinNoise(num2, 0.17f) * 2f - 1f) * this._config.gustTrembleDegrees * num;
			float num4 = (Mathf.PerlinNoise(0.53f, num2) * 2f - 1f) * this._config.gustTrembleDegrees * num;
			float num5 = (Mathf.PerlinNoise(num2 * 0.7f, 0.91f) * 2f - 1f) * this._config.gustTrembleDegrees * 0.5f * num;
			Pose pose = this.WornBagPose();
			Vector3 vector = this.BodyFrame * new Vector3(0f, 1f, -0.35f) * (this._config.gustTrembleLift * num);
			Quaternion quaternion = Quaternion.AngleAxis(num3, this.BodyFrame * Vector3.right) * Quaternion.AngleAxis(num4, this.BodyFrame * Vector3.forward) * Quaternion.AngleAxis(num5, Vector3.up);
			this._flyBag.transform.SetPositionAndRotation(pose.position + vector, quaternion * pose.rotation);
			this._flyBag.transform.localScale = this.BagWorldScale();
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x0026AF50 File Offset: 0x00269150
		private Pose WornBagPose()
		{
			Transform transform = ((this._bagBone != null) ? this._bagBone : this.Bone(HumanBodyBones.Head));
			return new Pose(transform.TransformPoint(this._wornLocalPosition), transform.rotation * this._wornLocalRotation);
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x0026AF9E File Offset: 0x0026919E
		private Vector3 BagWorldScale()
		{
			return Vector3.Scale(((this._bagBone != null) ? this._bagBone : this.Bone(HumanBodyBones.Head)).lossyScale, this._wornLocalScale);
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x0026AFCE File Offset: 0x002691CE
		private void HoldBagLow(bool hold)
		{
			this._holdingLow = hold;
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x0026AFD7 File Offset: 0x002691D7
		private Vector3 HoldPoint()
		{
			return this.Bone(HumanBodyBones.Hips).position + this.BodyFrame * this._config.holdBagHand;
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x0026B000 File Offset: 0x00269200
		private void TickHand(float deltaTime)
		{
			float num = 1f / Mathf.Max(0.05f, this._config.poseBlendSeconds);
			this._handWeight = Mathf.MoveTowards(this._handWeight, this._holdingLow ? 1f : 0f, num * deltaTime);
			if (this._handWeight <= 0.001f)
			{
				if (this._rigOwned)
				{
					this.ReleaseHand();
				}
				return;
			}
			if (!this.AcquireHand())
			{
				return;
			}
			Vector3 vector = this.Bone(HumanBodyBones.RightUpperArm).position + this.BodyFrame * this._config.holdBagElbow;
			this._rig.SetCoverLimbPoseWeighted(LimbType.RightHand, this.HoldPoint(), this.Bone(HumanBodyBones.RightHand).rotation, this._handWeight, 1f, 0f, vector, 0.8f);
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x0026B0D4 File Offset: 0x002692D4
		private bool AcquireHand()
		{
			if (this._rigOwned)
			{
				return true;
			}
			if (!this._rigInitialized)
			{
				this._rigInitialized = true;
				this._rig = this._player.GetComponentInChildren<CharacterRigController>(true);
				if (this._rig != null && !this._rig.Initialize())
				{
					this._rig = null;
				}
			}
			if (this._rig == null)
			{
				return false;
			}
			this._rigSavedUpdateMode = this._animator.updateMode;
			this._animator.updateMode = AnimatorUpdateMode.Normal;
			if (!this._rig.IsCoverRigActive)
			{
				this._rig.ActivateCoverRig();
			}
			this._rigOwned = true;
			return true;
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x0026B17C File Offset: 0x0026937C
		private void ReleaseHand()
		{
			if (!this._rigOwned)
			{
				return;
			}
			this._rigOwned = false;
			if (this._rig != null)
			{
				this._rig.ZeroCoverLimb(LimbType.RightHand);
				this._rig.DeactivateCoverRig();
			}
			if (this._animator != null)
			{
				this._animator.updateMode = this._rigSavedUpdateMode;
			}
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x0026B1E0 File Offset: 0x002693E0
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
				Transform transform = LongWayHomeStadiumSequence.FindChild(parent.GetChild(i), childName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06006144 RID: 24900 RVA: 0x0026B234 File Offset: 0x00269434
		private bool CanContinue
		{
			get
			{
				return this.IsReady && !this._continueRequested && Time.unscaledTime >= this._phaseStartedAt + ((this.Phase == LongWayHomeStadiumSequence.StoryPhase.Run) ? 1.5f : 1f) && (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Wall || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Run || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Center);
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06006145 RID: 24901 RVA: 0x0026B294 File Offset: 0x00269494
		private bool CanSwitchCamera
		{
			get
			{
				return this._single == null && this._shots != null && this._shots.Length > 1 && !this._continueRequested && (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Wall || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Run || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Center);
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06006146 RID: 24902 RVA: 0x0026B2E3 File Offset: 0x002694E3
		private bool CanSlowMotion
		{
			get
			{
				return this.IsReady && (this.Phase == LongWayHomeStadiumSequence.StoryPhase.Run || this.Phase == LongWayHomeStadiumSequence.StoryPhase.BagOff || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Exposed || this.Phase == LongWayHomeStadiumSequence.StoryPhase.Escape);
			}
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x0026B318 File Offset: 0x00269518
		private IReadOnlyList<LongWayHomeCinematicSkip.Hint> CurrentHints()
		{
			this._hintList.Clear();
			if (this.CanContinue)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Space", "Continue", null));
			}
			if (this.IsReady && this.Phase == LongWayHomeStadiumSequence.StoryPhase.Exposed)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Space", "Continue", null));
			}
			if (this.CanOrbit)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Mouse", "Rotate camera", "RS"));
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("Wheel", "Zoom", "LT + RS"));
			}
			if (this.CanSwitchCamera)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("F", "Switch camera", null));
			}
			if (this.CanSlowMotion)
			{
				this._hintList.Add(new LongWayHomeCinematicSkip.Hint("C", this._slowMotion ? "Normal speed" : "Slow motion", ControllerInput.ForKey(KeyCode.C)));
			}
			return this._hintList;
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x0026B428 File Offset: 0x00269628
		private void ReadCinematicInput()
		{
			if (Time.timeScale <= 0f || TextInputFocus.HasFocusedField || GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			this.ReadLookInput();
			if (this.CanContinue && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || LongWayHomeStadiumSequence.Controller(KeyCode.Space)))
			{
				this._continueRequested = true;
			}
			if (this.CanSwitchCamera && (Input.GetKeyDown(KeyCode.F) || LongWayHomeStadiumSequence.Controller(KeyCode.F)))
			{
				this.NextShot();
			}
			if (this.CanSlowMotion && (Input.GetKeyDown(KeyCode.C) || LongWayHomeStadiumSequence.Controller(KeyCode.C)))
			{
				this.SetSlowMotion(!this._slowMotion, false);
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x0026B4CA File Offset: 0x002696CA
		private static bool Controller(KeyCode key)
		{
			if (!ControllerInput.IsActive || ControllerInput.UiInputConsumedThisFrame || !ControllerInput.GetKeyDown(key))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x0026B4EC File Offset: 0x002696EC
		private void SetSlowMotion(bool on, bool immediate)
		{
			if (on && !this._ownsTime)
			{
				if (Time.timeScale <= 0f)
				{
					return;
				}
				this._ownsTime = true;
				this._baseTimeScale = Time.timeScale;
				this._baseFixedDeltaTime = Time.fixedDeltaTime;
				this._nextSlowAudioScan = 0f;
			}
			this._slowMotion = on;
			if (!on && immediate)
			{
				this.RestoreTime();
			}
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x0026B550 File Offset: 0x00269750
		private void TickSlowMotion()
		{
			if (!this._ownsTime)
			{
				return;
			}
			float timeScale = Time.timeScale;
			if (timeScale <= 0f)
			{
				return;
			}
			float num = this._baseTimeScale * Mathf.Clamp(this._config.slowMotionScale, 0.05f, 1f);
			float num2 = (this._slowMotion ? num : this._baseTimeScale);
			float num3 = (this._baseTimeScale - num) * Time.unscaledDeltaTime / Mathf.Max(0.01f, this._config.slowMotionRampSeconds);
			float num4 = Mathf.MoveTowards(timeScale, num2, num3);
			if (!this._slowMotion && Mathf.Approximately(num4, this._baseTimeScale))
			{
				this.RestoreTime();
				return;
			}
			Time.timeScale = num4;
			Time.fixedDeltaTime = this._baseFixedDeltaTime * num4 / this._baseTimeScale;
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x0026B612 File Offset: 0x00269812
		private void RestoreTime()
		{
			this._slowMotion = false;
			if (this._ownsTime)
			{
				this._ownsTime = false;
				if (Time.timeScale > 0f)
				{
					Time.timeScale = this._baseTimeScale;
				}
				Time.fixedDeltaTime = this._baseFixedDeltaTime;
			}
			this.RestoreSlowAudio();
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x0026B652 File Offset: 0x00269852
		private void StartMix()
		{
			if (this._mixGuard == null)
			{
				this._mixGuard = base.gameObject.AddComponent<LongWayHomeStadiumMixGuard>();
			}
			this._mixGuard.Bind(this);
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x0026B67F File Offset: 0x0026987F
		private void DuckCrowd(float gain, float seconds)
		{
			this._crowdGainTarget = Mathf.Clamp01(gain);
			if (seconds <= 0f)
			{
				this._crowdGain = this._crowdGainTarget;
				return;
			}
			this._crowdGainSpeed = Mathf.Abs(this._crowdGainTarget - this._crowdGain) / seconds;
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x0026B6BC File Offset: 0x002698BC
		private IEnumerator CrowdUnderHerVoice()
		{
			this.DuckCrowd(this._config.voiceCrowdLevel, this._config.crowdDuckSeconds);
			CielVoiceController instance = CielVoiceController.Instance;
			AudioSource source = ((instance != null) ? instance.VoiceSource : null);
			float deadline = Time.unscaledTime + 0.5f;
			while (source != null && !source.isPlaying)
			{
				if (Time.unscaledTime >= deadline)
				{
					break;
				}
				yield return null;
			}
			while (source != null && source.isPlaying)
			{
				yield return null;
			}
			this.DuckCrowd(1f, this._config.crowdRestoreSeconds);
			yield break;
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x0026B6CB File Offset: 0x002698CB
		internal void LateMix()
		{
			this.MixCrowd();
			this.MixSlowMotion();
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x0026B6DC File Offset: 0x002698DC
		private void MixCrowd()
		{
			this._crowdGain = Mathf.MoveTowards(this._crowdGain, this._crowdGainTarget, this._crowdGainSpeed * Time.unscaledDeltaTime);
			if (this._crowd == null)
			{
				return;
			}
			if (this._crowdSources.Count == 0)
			{
				foreach (AudioSource audioSource in this._crowd.GetComponents<AudioSource>())
				{
					this._crowdSources.Add(new LongWayHomeStadiumSequence.MixedSource
					{
						Source = audioSource,
						Value = audioSource.volume,
						Applied = audioSource.volume
					});
				}
			}
			foreach (LongWayHomeStadiumSequence.MixedSource mixedSource in this._crowdSources)
			{
				AudioSource source = mixedSource.Source;
				if (!(source == null))
				{
					if (!Mathf.Approximately(source.volume, mixedSource.Applied))
					{
						mixedSource.Value = source.volume;
					}
					mixedSource.Applied = mixedSource.Value * this._crowdGain;
					source.volume = mixedSource.Applied;
				}
			}
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x0026B810 File Offset: 0x00269A10
		private float SlowMotionPitch()
		{
			if (!this._ownsTime || this._config == null || this._baseTimeScale <= 0f)
			{
				return 1f;
			}
			float num = this._baseTimeScale * Mathf.Clamp(this._config.slowMotionScale, 0.05f, 1f);
			float num2 = Mathf.InverseLerp(this._baseTimeScale, num, Time.timeScale);
			return Mathf.Lerp(1f, this._config.slowMotionAudioPitch, num2);
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x0026B890 File Offset: 0x00269A90
		private void MixSlowMotion()
		{
			float num = this.SlowMotionPitch();
			if (num >= 0.999f)
			{
				if (this._slowedSources.Count > 0)
				{
					this.RestoreSlowAudio();
				}
				return;
			}
			if (Time.unscaledTime >= this._nextSlowAudioScan)
			{
				this._nextSlowAudioScan = Time.unscaledTime + 0.25f;
				foreach (AudioSource audioSource in Object.FindObjectsByType<AudioSource>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
				{
					this.SlowAudio(audioSource);
				}
			}
			this._lostSources.Clear();
			foreach (KeyValuePair<int, LongWayHomeStadiumSequence.MixedSource> keyValuePair in this._slowedSources)
			{
				LongWayHomeStadiumSequence.MixedSource value = keyValuePair.Value;
				AudioSource source = value.Source;
				if (source == null)
				{
					this._lostSources.Add(keyValuePair.Key);
				}
				else
				{
					if (!Mathf.Approximately(source.pitch, value.Applied))
					{
						value.Value = source.pitch;
					}
					value.Applied = value.Value * num;
					source.pitch = value.Applied;
				}
			}
			foreach (int num2 in this._lostSources)
			{
				this._slowedSources.Remove(num2);
			}
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x0026BA0C File Offset: 0x00269C0C
		private void SlowAudio(AudioSource source)
		{
			if (source == null || !source.gameObject.scene.IsValid())
			{
				return;
			}
			int instanceID = source.GetInstanceID();
			if (this._slowedSources.ContainsKey(instanceID))
			{
				return;
			}
			CielVoiceController instance = CielVoiceController.Instance;
			Canvas componentInParent = source.GetComponentInParent<Canvas>(true);
			if (componentInParent != null && componentInParent.renderMode != RenderMode.WorldSpace && (instance == null || source != instance.VoiceSource))
			{
				return;
			}
			this._slowedSources.Add(instanceID, new LongWayHomeStadiumSequence.MixedSource
			{
				Source = source,
				Value = source.pitch,
				Applied = source.pitch
			});
		}

		// Token: 0x06006155 RID: 24917 RVA: 0x0026BAB8 File Offset: 0x00269CB8
		private void RestoreSlowAudio()
		{
			foreach (LongWayHomeStadiumSequence.MixedSource mixedSource in this._slowedSources.Values)
			{
				if (mixedSource.Source != null && Mathf.Approximately(mixedSource.Source.pitch, mixedSource.Applied))
				{
					mixedSource.Source.pitch = mixedSource.Value;
				}
			}
			this._slowedSources.Clear();
			this._nextSlowAudioScan = 0f;
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x0026BB58 File Offset: 0x00269D58
		private void ReleaseMix()
		{
			this.RestoreSlowAudio();
			this._crowdGain = (this._crowdGainTarget = 1f);
			foreach (LongWayHomeStadiumSequence.MixedSource mixedSource in this._crowdSources)
			{
				if (mixedSource.Source != null && Mathf.Approximately(mixedSource.Source.volume, mixedSource.Applied))
				{
					mixedSource.Source.volume = mixedSource.Value;
				}
			}
			this._crowdSources.Clear();
			if (this._mixGuard != null)
			{
				this._mixGuard.Bind(null);
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06006157 RID: 24919 RVA: 0x0026BC1C File Offset: 0x00269E1C
		private bool CanOrbit
		{
			get
			{
				return this.IsReady && this.Phase == LongWayHomeStadiumSequence.StoryPhase.Wall && !this._continueRequested;
			}
		}

		// Token: 0x06006158 RID: 24920 RVA: 0x0026BC3C File Offset: 0x00269E3C
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
			return LongWayHomeStadiumSequence.FallbackCameraState;
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06006159 RID: 24921 RVA: 0x0026BCDC File Offset: 0x00269EDC
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

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x0026BCFD File Offset: 0x00269EFD
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

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x0026BD1E File Offset: 0x00269F1E
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

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x0026BD3F File Offset: 0x00269F3F
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

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x0026BD60 File Offset: 0x00269F60
		private static float GameSettingsLook
		{
			get
			{
				return Mathf.Max(0.05f, GameSettingsService.LookSensitivity);
			}
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x0026BD74 File Offset: 0x00269F74
		private static float InvectorBlend(float rate, float deltaTime)
		{
			float num = Mathf.Max(0.001f, Time.fixedDeltaTime);
			return 1f - Mathf.Pow(1f - Mathf.Clamp(rate * num, 0f, 0.999f), deltaTime / num);
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x0026BDB8 File Offset: 0x00269FB8
		private void ReadLookInput()
		{
			if (!this.CanOrbit)
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
			if (vector2 == Vector3.zero)
			{
				return;
			}
			this._lookInput += vector2;
			this._lookFromPad = isActive;
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x0026BEC0 File Offset: 0x0026A0C0
		private void BeginOrbit()
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.GameplayCameraState();
			Transform transform = this._camera.transform;
			LongWayHomeStadiumConfigSO.BodyShot currentShot = this.CurrentShot;
			this._orbitAnchor = this.Bone((currentShot != null) ? currentShot.anchor : HumanBodyBones.Hips);
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
			this._orbitYaw = this.OpenSideYaw(Mathf.Atan2(-vector.x, -vector.z) * 57.29578f);
			this._orbitRotation = Quaternion.Euler(num, this._orbitYaw, 0f);
			this._orbitPitch = Mathf.Clamp(num, this.OrbitMinPitch(vThirdPersonCameraState), vThirdPersonCameraState.yMaxLimit);
			this._orbitDistance = magnitude;
			this._orbitZoom = Mathf.Clamp(magnitude, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
			this._orbitFieldOfView = this._camera.fieldOfView;
			this._orbitReach = 1f;
			this._orbiting = true;
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x0026C024 File Offset: 0x0026A224
		private float OrbitMinPitch(vThirdPersonCameraState state)
		{
			return Mathf.Max(state.yMinLimit, this._config.orbitMinPitch);
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x0026C03C File Offset: 0x0026A23C
		private float OpenSideYaw(float yaw)
		{
			float num = this.FrameYaw + 180f;
			float num2 = Mathf.Clamp(this._config.orbitYawLimit, 0f, 180f);
			return Mathf.Repeat(num + Mathf.Clamp(Mathf.DeltaAngle(num, yaw), -num2, num2), 360f);
		}

		// Token: 0x06006163 RID: 24931 RVA: 0x0026C08C File Offset: 0x0026A28C
		private void PlaceOrbitCamera(bool snap)
		{
			vThirdPersonCameraState vThirdPersonCameraState = this.GameplayCameraState();
			float deltaTime = Time.deltaTime;
			Vector3 lookInput = this._lookInput;
			this._lookInput = Vector3.zero;
			float num = (this._lookFromPad ? this.StickSensitivity : LongWayHomeStadiumSequence.GameSettingsLook);
			this._orbitYaw = this.OpenSideYaw(this._orbitYaw + lookInput.x * vThirdPersonCameraState.xMouseSensitivity * num);
			this._orbitPitch = Mathf.Clamp(this._orbitPitch - lookInput.y * vThirdPersonCameraState.yMouseSensitivity * num, this.OrbitMinPitch(vThirdPersonCameraState), vThirdPersonCameraState.yMaxLimit);
			this._orbitZoom = Mathf.Clamp(this._orbitZoom - lookInput.z * this.ScrollSpeed, vThirdPersonCameraState.minDistance, vThirdPersonCameraState.maxDistance);
			float num2 = ((this._orbitZoom < this._orbitDistance) ? Mathf.Max(vThirdPersonCameraState.smooth, this.SmoothBetweenStates) : Mathf.Min(vThirdPersonCameraState.smooth, this.SmoothBetweenStates));
			this._orbitDistance = (snap ? this._orbitZoom : Mathf.Lerp(this._orbitDistance, this._orbitZoom, LongWayHomeStadiumSequence.InvectorBlend(num2, deltaTime)));
			Quaternion quaternion = Quaternion.Euler(this._orbitPitch, this._orbitYaw, 0f);
			this._orbitRotation = (snap ? quaternion : Quaternion.Lerp(this._orbitRotation, quaternion, LongWayHomeStadiumSequence.InvectorBlend(this.RotationSharpness, deltaTime)));
			this._orbitFieldOfView = (snap ? vThirdPersonCameraState.fov : Mathf.Lerp(this._orbitFieldOfView, vThirdPersonCameraState.fov, LongWayHomeStadiumSequence.InvectorBlend(this.SmoothBetweenStates, deltaTime)));
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
			float num3 = this._config.wallMark.y + 0.08f;
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
			this._orbitReach = ((snap || num4 < this._orbitReach) ? num4 : Mathf.Lerp(this._orbitReach, num4, LongWayHomeStadiumSequence.InvectorBlend(Mathf.Min(vThirdPersonCameraState.smooth, this.SmoothBetweenStates), deltaTime)));
			Vector3 vector4 = this._orbitPivot + vector3 * this._orbitReach;
			Vector3 vector5 = this._orbitPivot - vector4;
			this._camera.transform.SetPositionAndRotation(vector4, (vector5.sqrMagnitude > 1E-06f) ? Quaternion.LookRotation(vector5) : this._orbitRotation);
			this._camera.nearClipPlane = 0.03f;
			this._camera.fieldOfView = this._orbitFieldOfView;
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x0026C4A0 File Offset: 0x0026A6A0
		private void FocusScreenOnHer()
		{
			if (this._screen == null || !this._screen.isActiveAndEnabled || this._player == null)
			{
				return;
			}
			if (!this._screenFocused)
			{
				this._screenWasFocused = this._screen.IsPlayerFocusActive;
			}
			this._screen.SwitchToPlayerFocus(this._player);
			this._screenFocused = true;
			if (this._screenCycle != null)
			{
				base.StopCoroutine(this._screenCycle);
			}
			this._screenCycle = base.StartCoroutine(this.CycleScreen());
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x0026C52E File Offset: 0x0026A72E
		private IEnumerator CycleScreen()
		{
			for (;;)
			{
				yield return new WaitForSeconds(this._config.screenShotSeconds);
				DetectionCameraController detectionCameraController = ((this._screen != null) ? this._screen.CycleController : null);
				if (detectionCameraController == null || !detectionCameraController.IsActive)
				{
					break;
				}
				detectionCameraController.CycleToNextPreset();
			}
			yield break;
			yield break;
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x0026C540 File Offset: 0x0026A740
		private void ScreenOnHerFace()
		{
			if (this._screenCycle != null)
			{
				base.StopCoroutine(this._screenCycle);
				this._screenCycle = null;
			}
			DetectionCameraController detectionCameraController = ((this._screen != null) ? this._screen.CycleController : null);
			if (detectionCameraController == null || !detectionCameraController.IsActive)
			{
				return;
			}
			int num = this.ScreenFacePreset();
			if (num >= 0 && num < detectionCameraController.ActivePresetCount)
			{
				detectionCameraController.ApplyPresetByIndex(num);
			}
		}

		// Token: 0x06006167 RID: 24935 RVA: 0x0026C5B4 File Offset: 0x0026A7B4
		private int ScreenFacePreset()
		{
			DetectionCameraPresetCollectionSO screenPresets = this._config.screenPresets;
			if (screenPresets == null)
			{
				return -1;
			}
			for (int i = 0; i < screenPresets.Count; i++)
			{
				DetectionCameraPresetEntry preset = screenPresets.GetPreset(i);
				if (((preset != null) ? preset.Label : null) == this._config.screenFacePreset)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x0026C614 File Offset: 0x0026A814
		private void ReleaseScreen()
		{
			if (this._screenCycle != null)
			{
				base.StopCoroutine(this._screenCycle);
				this._screenCycle = null;
			}
			if (!this._screenFocused)
			{
				return;
			}
			this._screenFocused = false;
			if (this._screen != null && !this._screenWasFocused)
			{
				this._screen.SwitchToAmbient();
			}
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x0026C66D File Offset: 0x0026A86D
		private AudioSource Audio()
		{
			if (this._audio != null)
			{
				return this._audio;
			}
			this._audio = this.CreateAudioSource("Audio - Stadium Opening");
			return this._audio;
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x0026C69C File Offset: 0x0026A89C
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
			if (this.SlowMotionPitch() < 0.999f)
			{
				this.SlowAudio(audioSource);
			}
			return audioSource;
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x0026C707 File Offset: 0x0026A907
		private void PlayGasp()
		{
			if (this._config.realizationGasp != null)
			{
				this.Audio().PlayOneShot(this._config.realizationGasp, this._config.gaspVolume);
			}
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x0026C740 File Offset: 0x0026A940
		private void PlayGiggle()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance != null && this._config.giggleVoice != null)
			{
				instance.Play(this._config.giggleVoice);
			}
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x0026C784 File Offset: 0x0026A984
		private void StopGiggle()
		{
			CielVoiceController instance = CielVoiceController.Instance;
			if (instance != null && this._config != null && instance.CurrentVoice == this._config.giggleVoice)
			{
				instance.Stop();
			}
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x0026C7CC File Offset: 0x0026A9CC
		private void BeginFootsteps()
		{
			this._stepsArmed = false;
			AnimationClipPlayable animationClipPlayable = this.MovingClip();
			AudioClip[] footstepClips = this._config.footstepClips;
			if (this._player == null || !animationClipPlayable.IsValid<AnimationClipPlayable>() || footstepClips == null || footstepClips.Length == 0)
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
			AnimationClip animationClip = animationClipPlayable.GetAnimationClip();
			if (animationClip != this._stepClip)
			{
				this._stepClip = animationClip;
				this._contacts = LongWayHomeUniversitySequence.FootContacts(animationClip);
			}
			this._previousStepTime = animationClipPlayable.GetTime<AnimationClipPlayable>();
			this._previousStepPosition = this._player.position;
			this._stepsArmed = true;
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x0026C8B4 File Offset: 0x0026AAB4
		private void TickFootsteps()
		{
			if (!this._stepsArmed)
			{
				return;
			}
			AnimationClipPlayable animationClipPlayable = this.MovingClip();
			if (!this._moving || !animationClipPlayable.IsValid<AnimationClipPlayable>() || this._stepClip == null)
			{
				this._stepsArmed = false;
				return;
			}
			double previousStepTime = this._previousStepTime;
			double time = animationClipPlayable.GetTime<AnimationClipPlayable>();
			Vector3 vector = this._player.position - this._previousStepPosition;
			vector.y = 0f;
			this._previousStepTime = time;
			this._previousStepPosition = this._player.position;
			if (Mathf.Clamp01(this._runWeight) <= 0.25f || vector.sqrMagnitude <= 1E-08f)
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
					this._steps.PlayOneShot(footstepClips[this._lastStep], this._config.runFootstepVolume);
					return;
				}
			}
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x0026CA28 File Offset: 0x0026AC28
		private void StopFootsteps()
		{
			this._stepsArmed = false;
			if (this._steps != null)
			{
				this._steps.Stop();
			}
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x0026CA4C File Offset: 0x0026AC4C
		private void CaptureStadium()
		{
			foreach (GameObject gameObject in base.gameObject.scene.GetRootGameObjects())
			{
				foreach (StadiumController stadiumController in gameObject.GetComponentsInChildren<StadiumController>(false))
				{
					if (stadiumController.enabled)
					{
						stadiumController.enabled = false;
						this._stadiumControllers.Add(stadiumController);
					}
				}
				if (this._crowd == null)
				{
					this._crowd = gameObject.GetComponentInChildren<StadiumAudioController>(false);
				}
				if (this._screen == null)
				{
					this._screen = gameObject.GetComponentInChildren<StadiumScreenController>(false);
				}
			}
			this.CaptureFootballers();
			Debug.Log(string.Concat(new string[]
			{
				string.Format("[LongWayHomeStadium] Stadium ready for the opening: {0} stadium ", this._stadiumControllers.Count),
				"presentation(s) held, crowd audio ",
				(this._crowd != null) ? "found" : "missing",
				", ",
				string.Format("big screen {0}, {1} footballers watching.", (this._screen != null) ? "found" : "missing", this._watchers.Count)
			}), this);
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x0026CB94 File Offset: 0x0026AD94
		private void ReleaseStadium()
		{
			this.CheerCrowd(false);
			this.ReleaseScreen();
			this.ReleaseFootballers();
			foreach (StadiumController stadiumController in this._stadiumControllers)
			{
				if (stadiumController != null)
				{
					stadiumController.enabled = true;
				}
			}
			this._stadiumControllers.Clear();
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x0026CC10 File Offset: 0x0026AE10
		private void CheerCrowd(bool on)
		{
			if (this._crowd == null)
			{
				this._cheering = false;
				return;
			}
			if (on)
			{
				this._crowd.StartHype();
				this._cheering = true;
				return;
			}
			if (this._cheering)
			{
				this._crowd.ReleaseHype();
				this._cheering = false;
			}
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x0026CC63 File Offset: 0x0026AE63
		private void TickCrowd()
		{
			if (this._cheering && this._crowd != null)
			{
				this._crowd.HoldHype();
			}
		}

		// Token: 0x06006175 RID: 24949 RVA: 0x0026CC86 File Offset: 0x0026AE86
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

		// Token: 0x06006176 RID: 24950 RVA: 0x0026CC98 File Offset: 0x0026AE98
		private void BeginRunPopulation()
		{
			NPCZoneManager instance = NPCZoneManager.Instance;
			if (instance != null && instance.TrySetPreparationFocus(this, this._config.handoffMark))
			{
				this._populationFocus = true;
			}
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x0026CCCF File Offset: 0x0026AECF
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

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06006178 RID: 24952 RVA: 0x0026CCF1 File Offset: 0x0026AEF1
		private float WindLevel
		{
			get
			{
				return Mathf.SmoothStep(0f, 1f, this._windRamp);
			}
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x0026CD08 File Offset: 0x0026AF08
		private void StartWind(float riseSeconds)
		{
			this._windTarget = 1f;
			this._windSpeed = 1f / Mathf.Max(0.05f, riseSeconds);
			this.EnsureWindOverlay();
			if (this._windOverlay != null)
			{
				this._windOverlay.SetActive(true);
			}
			if (this._windSource == null && this._config.gustWindSound != null)
			{
				this._windSource = this.CreateAudioSource("Audio - Gust");
				this._windSource.clip = this._config.gustWindSound;
				this._windSource.loop = true;
			}
			if (this._windSource != null && !this._windSource.isPlaying)
			{
				this._windSource.volume = 0f;
				this._windSource.time = Mathf.Min(1.5f, this._config.gustWindSound.length * 0.1f);
				this._windSource.Play();
			}
			this.EnsureWindZone();
			if (this._windZone != null)
			{
				this._windZone.gameObject.SetActive(true);
			}
			this.TickWind(0f);
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x0026CE3C File Offset: 0x0026B03C
		private void StopWind(float fadeSeconds)
		{
			this._windTarget = 0f;
			if (fadeSeconds > 0f)
			{
				this._windSpeed = 1f / fadeSeconds;
				return;
			}
			this._windRamp = 0f;
			this.TickWind(0f);
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x0026CE78 File Offset: 0x0026B078
		private void TickWind(float deltaTime)
		{
			if (this._windRamp <= 0f && this._windTarget <= 0f)
			{
				if (this._windSource != null && this._windSource.isPlaying)
				{
					this._windSource.Stop();
				}
				if (this._windOverlay != null && this._windOverlay.activeSelf)
				{
					this._windOverlay.SetActive(false);
				}
				if (this._windZone != null && this._windZone.gameObject.activeSelf)
				{
					this._windZone.gameObject.SetActive(false);
				}
				return;
			}
			this._windRamp = Mathf.MoveTowards(this._windRamp, this._windTarget, this._windSpeed * deltaTime);
			float windLevel = this.WindLevel;
			if (this._windSource != null)
			{
				this._windSource.volume = this._config.gustWindVolume * windLevel;
				this._windSource.pitch = Mathf.Lerp(0.85f, 1.05f, windLevel);
			}
			if (this._windZone != null && this._player != null)
			{
				this._windZone.transform.position = this._player.position + Vector3.up * 0.1f;
				this._windZone.SetWindDirection(this.BodyFrame * LongWayHomeStadiumSequence.GustDirection.normalized, false);
				this._windZone.main = this._config.hairWind * windLevel;
				this._windZone.turbulence = this._config.hairWindTurbulence;
			}
			this.TickWindLines(deltaTime, windLevel);
			if (this._windRamp <= 0f && this._windTarget <= 0f)
			{
				this.TickWind(0f);
			}
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x0026D05C File Offset: 0x0026B25C
		private void ReleaseWind()
		{
			this._windRamp = (this._windTarget = 0f);
			if (this._windSource != null)
			{
				this._windSource.Stop();
			}
			if (this._windOverlay != null)
			{
				this._windOverlay.SetActive(false);
			}
			if (this._windZone != null)
			{
				this._windZone.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x0026D0D0 File Offset: 0x0026B2D0
		private void DestroyWind()
		{
			this.ReleaseWind();
			if (this._windOverlay != null)
			{
				Object.Destroy(this._windOverlay);
			}
			if (this._windZone != null)
			{
				Object.Destroy(this._windZone.gameObject);
			}
			if (this._windSprite != null)
			{
				Object.Destroy(this._windSprite);
			}
			if (this._windTexture != null)
			{
				Object.Destroy(this._windTexture);
			}
			this._windOverlay = null;
			this._windZone = null;
			this._windSprite = null;
			this._windTexture = null;
			this._windLines.Clear();
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x0026D174 File Offset: 0x0026B374
		private void EnsureWindZone()
		{
			if (this._windZone != null || this._config.hairWind <= 0f)
			{
				return;
			}
			GameObject gameObject = new GameObject("Wind - Stadium Gust");
			gameObject.SetActive(false);
			gameObject.transform.SetParent(base.transform, false);
			this._windZone = gameObject.AddComponent<MagicaWindZone>();
			this._windZone.mode = MagicaWindZone.Mode.SphereDirection;
			this._windZone.radius = 1.5f;
			this._windZone.isAddition = true;
			this._windZone.main = 0f;
			this._windZone.turbulence = this._config.hairWindTurbulence;
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x0026D220 File Offset: 0x0026B420
		private void EnsureWindOverlay()
		{
			if (this._windOverlay != null || this._config.windLineCount <= 0)
			{
				return;
			}
			this._windOverlay = new GameObject("Wind Lines - Stadium Opening", new Type[] { typeof(RectTransform) });
			this._windOverlay.transform.SetParent(base.transform, false);
			Canvas canvas = this._windOverlay.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = -40;
			CanvasScaler canvasScaler = this._windOverlay.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			this._windArea = (RectTransform)this._windOverlay.transform;
			this._windTexture = LongWayHomeStadiumSequence.CreateStreakTexture();
			this._windSprite = Sprite.Create(this._windTexture, new Rect(0f, 0f, (float)this._windTexture.width, (float)this._windTexture.height), new Vector2(0.5f, 0.5f), 100f);
			this._windLines.Clear();
			for (int i = 0; i < this._config.windLineCount; i++)
			{
				GameObject gameObject = new GameObject("Wind Line", new Type[] { typeof(RectTransform) });
				gameObject.transform.SetParent(this._windArea, false);
				Image image = gameObject.AddComponent<Image>();
				image.sprite = this._windSprite;
				image.raycastTarget = false;
				image.color = new Color(1f, 1f, 1f, 0f);
				LongWayHomeStadiumSequence.WindLine windLine = new LongWayHomeStadiumSequence.WindLine
				{
					Rect = (RectTransform)gameObject.transform,
					Image = image
				};
				RectTransform rect = windLine.Rect;
				RectTransform rect2 = windLine.Rect;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rect2.anchorMax = vector;
				rect.anchorMin = vector;
				windLine.Threshold = Random.value * 0.7f;
				this.SpawnWindLine(windLine, true);
				this._windLines.Add(windLine);
			}
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x0026D43C File Offset: 0x0026B63C
		private Vector2 WindAreaSize()
		{
			Rect rect = ((this._windArea != null) ? this._windArea.rect : default(Rect));
			if (rect.width <= 1f || rect.height <= 1f)
			{
				return new Vector2(1920f, 1080f);
			}
			return rect.size;
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x0026D4A4 File Offset: 0x0026B6A4
		private void SpawnWindLine(LongWayHomeStadiumSequence.WindLine line, bool anywhere)
		{
			Vector2 vector = this.WindAreaSize();
			line.Length = Random.Range(160f, 480f);
			float num = Random.Range(2.5f, 6.5f);
			line.Rect.sizeDelta = new Vector2(line.Length, num * 4f);
			line.Y = Random.Range(-0.46f, 0.46f) * vector.y;
			line.Speed = this._config.windLineSpeed * vector.x * Random.Range(0.75f, 1.3f);
			line.Alpha = Random.Range(0.45f, 1f);
			line.Wobble = Random.Range(2f, 10f);
			line.Phase = Random.value * 3.1415927f * 2f;
			float num2 = vector.x * 0.5f + line.Length;
			line.X = (anywhere ? Random.Range(-num2, num2) : (num2 + Random.Range(0f, vector.x * 0.35f)));
			line.Rect.localEulerAngles = new Vector3(0f, 0f, -this._config.windLineTilt);
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x0026D5E4 File Offset: 0x0026B7E4
		private void TickWindLines(float deltaTime, float level)
		{
			if (this._windOverlay == null || !this._windOverlay.activeSelf)
			{
				return;
			}
			Vector2 vector = this.WindAreaSize();
			float num = Mathf.Tan(this._config.windLineTilt * 0.017453292f);
			float num2 = Mathf.Lerp(0.55f, 1f, level);
			float time = Time.time;
			foreach (LongWayHomeStadiumSequence.WindLine windLine in this._windLines)
			{
				windLine.X -= windLine.Speed * num2 * deltaTime;
				if (windLine.X < -(vector.x * 0.5f + windLine.Length))
				{
					this.SpawnWindLine(windLine, false);
				}
				float num3 = windLine.Y - windLine.X * num + Mathf.Sin(windLine.Phase + time * 3.1f) * windLine.Wobble;
				windLine.Rect.anchoredPosition = new Vector2(windLine.X, num3);
				float num4 = Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(windLine.Threshold, windLine.Threshold + 0.3f, level));
				windLine.Image.color = new Color(1f, 1f, 1f, this._config.windLineOpacity * windLine.Alpha * num4);
			}
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x0026D780 File Offset: 0x0026B980
		private static Texture2D CreateStreakTexture()
		{
			Texture2D texture2D = new Texture2D(256, 16, TextureFormat.RGBA32, false)
			{
				name = "Wind Streak",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear,
				hideFlags = HideFlags.DontSave
			};
			Color32[] array = new Color32[4096];
			for (int i = 0; i < 16; i++)
			{
				float num = ((float)i + 0.5f) / 16f * 2f - 1f;
				float num2 = Mathf.Exp(-num * num * 5f);
				for (int j = 0; j < 256; j++)
				{
					float num3 = ((float)j + 0.5f) / 256f;
					float num4 = num2 * Mathf.SmoothStep(0f, 1f, num3 / 0.08f) * Mathf.Pow(1f - num3, 1.6f);
					array[i * 256 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.RoundToInt(Mathf.Clamp01(num4) * 255f));
				}
			}
			texture2D.SetPixels32(array);
			texture2D.Apply(false, true);
			return texture2D;
		}

		// Token: 0x040062C5 RID: 25285
		private PlayableGraph _cielGraph;

		// Token: 0x040062C6 RID: 25286
		private AnimationMixerPlayable _bodyMixer;

		// Token: 0x040062C7 RID: 25287
		private AnimationLayerMixerPlayable _layers;

		// Token: 0x040062C8 RID: 25288
		private AnimationClipPlayable _faceCover;

		// Token: 0x040062C9 RID: 25289
		private AnimationScriptPlayable _lookAtJob;

		// Token: 0x040062CA RID: 25290
		private readonly Dictionary<LongWayHomeStadiumSequence.BodyPose, LongWayHomeStadiumSequence.PoseSlot> _slots = new Dictionary<LongWayHomeStadiumSequence.BodyPose, LongWayHomeStadiumSequence.PoseSlot>();

		// Token: 0x040062CB RID: 25291
		private LongWayHomeStadiumSequence.BodyPose _pose;

		// Token: 0x040062CC RID: 25292
		private float _poseBlendSpeed = 1000f;

		// Token: 0x040062CD RID: 25293
		private float _runWeight;

		// Token: 0x040062CE RID: 25294
		private float _wallYawOffset;

		// Token: 0x040062CF RID: 25295
		private bool _moving;

		// Token: 0x040062D0 RID: 25296
		private float _lookWeight;

		// Token: 0x040062D1 RID: 25297
		private float _lookTarget;

		// Token: 0x040062D2 RID: 25298
		private float _lookSpeed = 2f;

		// Token: 0x040062D3 RID: 25299
		private float _coverWeight;

		// Token: 0x040062D4 RID: 25300
		private float _coverTarget;

		// Token: 0x040062D5 RID: 25301
		private float _coverSpeed = 4f;

		// Token: 0x040062D6 RID: 25302
		private const string StimulationPoseLabel = "Stimulation";

		// Token: 0x040062D7 RID: 25303
		private readonly List<CoverBlendShapeEntry> _nipples = new List<CoverBlendShapeEntry>();

		// Token: 0x040062D8 RID: 25304
		private float _boobsType = 3f;

		// Token: 0x040062D9 RID: 25305
		private const float BounceStep = 0.004166667f;

		// Token: 0x040062DA RID: 25306
		private readonly LongWayHomeStadiumSequence.BreastBones _left = new LongWayHomeStadiumSequence.BreastBones();

		// Token: 0x040062DB RID: 25307
		private readonly LongWayHomeStadiumSequence.BreastBones _right = new LongWayHomeStadiumSequence.BreastBones();

		// Token: 0x040062DC RID: 25308
		private readonly Random _bounceRandom = new Random();

		// Token: 0x040062DD RID: 25309
		private bool _hasRest;

		// Token: 0x040062DE RID: 25310
		private bool _bouncing;

		// Token: 0x040062DF RID: 25311
		private bool _bounceApplied;

		// Token: 0x040062E0 RID: 25312
		private bool _bounceWanted;

		// Token: 0x040062E1 RID: 25313
		private float _bounceWeight;

		// Token: 0x040062E2 RID: 25314
		private float _bounceScale;

		// Token: 0x040062E3 RID: 25315
		private double _bounceClipTime;

		// Token: 0x040062E4 RID: 25316
		private float[] _bounceContacts = new float[] { 0.25f, 0.75f };

		// Token: 0x040062E5 RID: 25317
		private readonly List<GameObject> _breastCloths = new List<GameObject>();

		// Token: 0x040062E6 RID: 25318
		private readonly List<GameObject> _heldCloths = new List<GameObject>();

		// Token: 0x040062E7 RID: 25319
		private Camera _camera;

		// Token: 0x040062E8 RID: 25320
		private GPUICamera _crowdCamera;

		// Token: 0x040062E9 RID: 25321
		private LongWayHomeStadiumConfigSO.BodyShot[] _shots;

		// Token: 0x040062EA RID: 25322
		private LongWayHomeStadiumConfigSO.BodyShot _single;

		// Token: 0x040062EB RID: 25323
		private int _shotIndex;

		// Token: 0x040062EC RID: 25324
		private float _fieldOfView = 45f;

		// Token: 0x040062ED RID: 25325
		private Vector3 _cameraVelocity;

		// Token: 0x040062EE RID: 25326
		private Vector3 _focus;

		// Token: 0x040062EF RID: 25327
		private Vector3 _focusVelocity;

		// Token: 0x040062F0 RID: 25328
		private float _cameraYaw;

		// Token: 0x040062F1 RID: 25329
		private float _cameraYawVelocity;

		// Token: 0x040062F2 RID: 25330
		private float _reach = 1f;

		// Token: 0x040062F3 RID: 25331
		private float _reachVelocity;

		// Token: 0x040062F4 RID: 25332
		private readonly RaycastHit[] _cameraHits = new RaycastHit[16];

		// Token: 0x040062F5 RID: 25333
		private AzureCoreSystem _azure;

		// Token: 0x040062F6 RID: 25334
		private bool _clockHeld;

		// Token: 0x040062F7 RID: 25335
		private float _heldHour;

		// Token: 0x040062F8 RID: 25336
		private float _runDayLength;

		// Token: 0x040062F9 RID: 25337
		private const string CommentaryScope = "stadium";

		// Token: 0x040062FA RID: 25338
		private const string SpeakerScope = "dialogue_speaker";

		// Token: 0x040062FB RID: 25339
		private Canvas _dialogueCanvas;

		// Token: 0x040062FC RID: 25340
		private const string FaceRendererPath = "3D Model/Ciel_07_05_26_ass/Body";

		// Token: 0x040062FD RID: 25341
		private const float ReadinessTimeout = 60f;

		// Token: 0x040062FE RID: 25342
		private static LongWayHomeStadiumSequence _activePresenter;

		// Token: 0x040062FF RID: 25343
		private readonly List<LongWayHomeStadiumSequence.BehaviourState> _presentation = new List<LongWayHomeStadiumSequence.BehaviourState>();

		// Token: 0x04006300 RID: 25344
		private LongWayHomeStadiumConfigSO _config;

		// Token: 0x04006301 RID: 25345
		private Transform _player;

		// Token: 0x04006302 RID: 25346
		private Animator _animator;

		// Token: 0x04006303 RID: 25347
		private vThirdPersonInput _input;

		// Token: 0x04006304 RID: 25348
		private vThirdPersonCamera _gameplayCamera;

		// Token: 0x04006305 RID: 25349
		private Camera _gameplayView;

		// Token: 0x04006306 RID: 25350
		private Rigidbody _body;

		// Token: 0x04006307 RID: 25351
		private MCStyleCustomizationService _style;

		// Token: 0x04006308 RID: 25352
		private ClothingToggleController _clothing;

		// Token: 0x04006309 RID: 25353
		private CoverPoseController _coverPose;

		// Token: 0x0400630A RID: 25354
		private PlayerActivityLease _lease;

		// Token: 0x0400630B RID: 25355
		private bool _captured;

		// Token: 0x0400630C RID: 25356
		private bool _inputEnabled;

		// Token: 0x0400630D RID: 25357
		private bool _inputLock;

		// Token: 0x0400630E RID: 25358
		private bool _cameraLock;

		// Token: 0x0400630F RID: 25359
		private bool _cameraFrozen;

		// Token: 0x04006310 RID: 25360
		private bool _coverEnabled;

		// Token: 0x04006311 RID: 25361
		private bool _rootMotion;

		// Token: 0x04006312 RID: 25362
		private bool _kinematic;

		// Token: 0x04006313 RID: 25363
		private bool _gravity;

		// Token: 0x04006314 RID: 25364
		private RigidbodyConstraints _constraints;

		// Token: 0x04006315 RID: 25365
		private RigidbodyInterpolation _interpolation;

		// Token: 0x04006316 RID: 25366
		private float[] _layerWeights;

		// Token: 0x04006317 RID: 25367
		private AnimatorCullingMode _culling;

		// Token: 0x04006318 RID: 25368
		private AnimatorUpdateMode _updateMode;

		// Token: 0x04006319 RID: 25369
		private PlayerEmotionController _emotion;

		// Token: 0x0400631A RID: 25370
		private Animator _faceAnimator;

		// Token: 0x0400631B RID: 25371
		private string _faceParameter;

		// Token: 0x0400631C RID: 25372
		private int _faceLayer = -1;

		// Token: 0x0400631D RID: 25373
		private int _savedFaceIndex;

		// Token: 0x0400631E RID: 25374
		private int _neutralFaceIndex;

		// Token: 0x0400631F RID: 25375
		private bool _ownsFaceOverride;

		// Token: 0x04006320 RID: 25376
		private bool _ownsEmotionOverride;

		// Token: 0x04006321 RID: 25377
		private LongWayHomeGymShowerFaceDriver _face;

		// Token: 0x04006322 RID: 25378
		private SkinnedMeshRenderer _faceRenderer;

		// Token: 0x04006323 RID: 25379
		private MCStyleCustomizationService.WardrobeSnapshot _wardrobeSnapshot;

		// Token: 0x04006324 RID: 25380
		private WearableUseController _wearController;

		// Token: 0x04006325 RID: 25381
		private bool _wearWasEnabled;

		// Token: 0x04006326 RID: 25382
		private IDisposable _wardrobePresentation;

		// Token: 0x04006327 RID: 25383
		private bool _skipRequested;

		// Token: 0x04006328 RID: 25384
		private bool _continueRequested;

		// Token: 0x04006329 RID: 25385
		private float _phaseStartedAt;

		// Token: 0x0400632A RID: 25386
		private LongWayHomeCinematicSkip _hints;

		// Token: 0x0400632E RID: 25390
		private readonly List<LongWayHomeStadiumSequence.Watcher> _watchers = new List<LongWayHomeStadiumSequence.Watcher>();

		// Token: 0x0400632F RID: 25391
		private const string BagShapeSource = "LongWayHomeStadium.HeadBag";

		// Token: 0x04006330 RID: 25392
		private const string HeadAttachBone = "head.x";

		// Token: 0x04006331 RID: 25393
		private LongWayHomeStadiumSequence.BagState _bagState;

		// Token: 0x04006332 RID: 25394
		private GameObject _handBag;

		// Token: 0x04006333 RID: 25395
		private GameObject _flyBag;

		// Token: 0x04006334 RID: 25396
		private readonly List<Material> _flyMaterials = new List<Material>();

		// Token: 0x04006335 RID: 25397
		private bool _bagEquipped;

		// Token: 0x04006336 RID: 25398
		private bool _hadBagEquipped;

		// Token: 0x04006337 RID: 25399
		private Transform _bagBone;

		// Token: 0x04006338 RID: 25400
		private Vector3 _wornLocalPosition;

		// Token: 0x04006339 RID: 25401
		private Vector3 _wornLocalScale = Vector3.one;

		// Token: 0x0400633A RID: 25402
		private Quaternion _wornLocalRotation = Quaternion.identity;

		// Token: 0x0400633B RID: 25403
		private BlendShapeManager _bagShapes;

		// Token: 0x0400633C RID: 25404
		private Vector3 _flyVelocity;

		// Token: 0x0400633D RID: 25405
		private Vector3 _flySpin;

		// Token: 0x0400633E RID: 25406
		private float _flyElapsed;

		// Token: 0x0400633F RID: 25407
		private float _shiver;

		// Token: 0x04006340 RID: 25408
		private CharacterRigController _rig;

		// Token: 0x04006341 RID: 25409
		private bool _rigOwned;

		// Token: 0x04006342 RID: 25410
		private bool _rigInitialized;

		// Token: 0x04006343 RID: 25411
		private bool _holdingLow;

		// Token: 0x04006344 RID: 25412
		private AnimatorUpdateMode _rigSavedUpdateMode;

		// Token: 0x04006345 RID: 25413
		private float _handWeight;

		// Token: 0x04006346 RID: 25414
		internal const string ContinueAction = "Continue";

		// Token: 0x04006347 RID: 25415
		internal const string SwitchCameraAction = "Switch camera";

		// Token: 0x04006348 RID: 25416
		internal const string SlowMotionAction = "Slow motion";

		// Token: 0x04006349 RID: 25417
		internal const string NormalSpeedAction = "Normal speed";

		// Token: 0x0400634A RID: 25418
		internal const string RotateCameraAction = "Rotate camera";

		// Token: 0x0400634B RID: 25419
		internal const string ZoomAction = "Zoom";

		// Token: 0x0400634C RID: 25420
		private readonly List<LongWayHomeCinematicSkip.Hint> _hintList = new List<LongWayHomeCinematicSkip.Hint>();

		// Token: 0x0400634D RID: 25421
		private bool _slowMotion;

		// Token: 0x0400634E RID: 25422
		private bool _ownsTime;

		// Token: 0x0400634F RID: 25423
		private float _baseTimeScale = 1f;

		// Token: 0x04006350 RID: 25424
		private float _baseFixedDeltaTime = 0.02f;

		// Token: 0x04006351 RID: 25425
		private const float SlowAudioScanSeconds = 0.25f;

		// Token: 0x04006352 RID: 25426
		private readonly List<LongWayHomeStadiumSequence.MixedSource> _crowdSources = new List<LongWayHomeStadiumSequence.MixedSource>();

		// Token: 0x04006353 RID: 25427
		private readonly Dictionary<int, LongWayHomeStadiumSequence.MixedSource> _slowedSources = new Dictionary<int, LongWayHomeStadiumSequence.MixedSource>();

		// Token: 0x04006354 RID: 25428
		private readonly List<int> _lostSources = new List<int>();

		// Token: 0x04006355 RID: 25429
		private LongWayHomeStadiumMixGuard _mixGuard;

		// Token: 0x04006356 RID: 25430
		private float _crowdGain = 1f;

		// Token: 0x04006357 RID: 25431
		private float _crowdGainTarget = 1f;

		// Token: 0x04006358 RID: 25432
		private float _crowdGainSpeed;

		// Token: 0x04006359 RID: 25433
		private float _nextSlowAudioScan;

		// Token: 0x0400635A RID: 25434
		private bool _orbiting;

		// Token: 0x0400635B RID: 25435
		private Transform _orbitAnchor;

		// Token: 0x0400635C RID: 25436
		private Vector3 _orbitAnchorOffset;

		// Token: 0x0400635D RID: 25437
		private Vector3 _orbitPivot;

		// Token: 0x0400635E RID: 25438
		private Vector3 _orbitPivotVelocity;

		// Token: 0x0400635F RID: 25439
		private float _orbitYaw;

		// Token: 0x04006360 RID: 25440
		private float _orbitPitch;

		// Token: 0x04006361 RID: 25441
		private float _orbitZoom;

		// Token: 0x04006362 RID: 25442
		private float _orbitDistance;

		// Token: 0x04006363 RID: 25443
		private float _orbitFieldOfView;

		// Token: 0x04006364 RID: 25444
		private float _orbitReach = 1f;

		// Token: 0x04006365 RID: 25445
		private Quaternion _orbitRotation = Quaternion.identity;

		// Token: 0x04006366 RID: 25446
		private Vector3 _lookInput;

		// Token: 0x04006367 RID: 25447
		private bool _lookFromPad;

		// Token: 0x04006368 RID: 25448
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

		// Token: 0x04006369 RID: 25449
		private StadiumScreenController _screen;

		// Token: 0x0400636A RID: 25450
		private bool _screenFocused;

		// Token: 0x0400636B RID: 25451
		private bool _screenWasFocused;

		// Token: 0x0400636C RID: 25452
		private Coroutine _screenCycle;

		// Token: 0x0400636D RID: 25453
		private AudioSource _audio;

		// Token: 0x0400636E RID: 25454
		private AudioSource _steps;

		// Token: 0x0400636F RID: 25455
		private AnimationClip _stepClip;

		// Token: 0x04006370 RID: 25456
		private float[] _contacts = new float[] { 0.25f, 0.75f };

		// Token: 0x04006371 RID: 25457
		private double _previousStepTime;

		// Token: 0x04006372 RID: 25458
		private Vector3 _previousStepPosition;

		// Token: 0x04006373 RID: 25459
		private bool _stepsArmed;

		// Token: 0x04006374 RID: 25460
		private int _lastStep = -1;

		// Token: 0x04006375 RID: 25461
		private readonly List<StadiumController> _stadiumControllers = new List<StadiumController>();

		// Token: 0x04006376 RID: 25462
		private StadiumAudioController _crowd;

		// Token: 0x04006377 RID: 25463
		private bool _cheering;

		// Token: 0x04006378 RID: 25464
		private bool _populationFocus;

		// Token: 0x04006379 RID: 25465
		private const int WindSortingOrder = -40;

		// Token: 0x0400637A RID: 25466
		private static readonly Vector3 GustDirection = new Vector3(0.25f, 0.3f, -1f);

		// Token: 0x0400637B RID: 25467
		private GameObject _windOverlay;

		// Token: 0x0400637C RID: 25468
		private RectTransform _windArea;

		// Token: 0x0400637D RID: 25469
		private readonly List<LongWayHomeStadiumSequence.WindLine> _windLines = new List<LongWayHomeStadiumSequence.WindLine>();

		// Token: 0x0400637E RID: 25470
		private Texture2D _windTexture;

		// Token: 0x0400637F RID: 25471
		private Sprite _windSprite;

		// Token: 0x04006380 RID: 25472
		private AudioSource _windSource;

		// Token: 0x04006381 RID: 25473
		private MagicaWindZone _windZone;

		// Token: 0x04006382 RID: 25474
		private float _windRamp;

		// Token: 0x04006383 RID: 25475
		private float _windTarget;

		// Token: 0x04006384 RID: 25476
		private float _windSpeed = 1f;

		// Token: 0x02000D01 RID: 3329
		private enum BodyPose
		{
			// Token: 0x04006386 RID: 25478
			Wall,
			// Token: 0x04006387 RID: 25479
			HoldBag,
			// Token: 0x04006388 RID: 25480
			Run,
			// Token: 0x04006389 RID: 25481
			LookAtMe,
			// Token: 0x0400638A RID: 25482
			Embarrassed
		}

		// Token: 0x02000D02 RID: 3330
		private sealed class PoseSlot
		{
			// Token: 0x0400638B RID: 25483
			public AnimationClipPlayable Still;

			// Token: 0x0400638C RID: 25484
			public AnimationClipPlayable Moving;

			// Token: 0x0400638D RID: 25485
			public int StillInput = -1;

			// Token: 0x0400638E RID: 25486
			public int MovingInput = -1;

			// Token: 0x0400638F RID: 25487
			public float Weight;
		}

		// Token: 0x02000D03 RID: 3331
		private sealed class BreastBones
		{
			// Token: 0x04006390 RID: 25488
			public Transform Spring;

			// Token: 0x04006391 RID: 25489
			public Transform Base;

			// Token: 0x04006392 RID: 25490
			public Vector3 SpringPosition;

			// Token: 0x04006393 RID: 25491
			public Vector3 BasePosition;

			// Token: 0x04006394 RID: 25492
			public Quaternion SpringRotation = Quaternion.identity;

			// Token: 0x04006395 RID: 25493
			public Quaternion BaseRotation = Quaternion.identity;

			// Token: 0x04006396 RID: 25494
			public Vector3 Mass;

			// Token: 0x04006397 RID: 25495
			public Vector3 Velocity;

			// Token: 0x04006398 RID: 25496
			public Vector3 Anchor;

			// Token: 0x04006399 RID: 25497
			public bool Settled;

			// Token: 0x0400639A RID: 25498
			public bool Moving;

			// Token: 0x0400639B RID: 25499
			public float Frequency;

			// Token: 0x0400639C RID: 25500
			public float Arm = 0.1f;
		}

		// Token: 0x02000D04 RID: 3332
		public enum StoryPhase
		{
			// Token: 0x0400639E RID: 25502
			Loading,
			// Token: 0x0400639F RID: 25503
			Wall,
			// Token: 0x040063A0 RID: 25504
			Bag,
			// Token: 0x040063A1 RID: 25505
			Wear,
			// Token: 0x040063A2 RID: 25506
			Run,
			// Token: 0x040063A3 RID: 25507
			Center,
			// Token: 0x040063A4 RID: 25508
			BagOff,
			// Token: 0x040063A5 RID: 25509
			Exposed,
			// Token: 0x040063A6 RID: 25510
			Escape,
			// Token: 0x040063A7 RID: 25511
			Handoff,
			// Token: 0x040063A8 RID: 25512
			Done,
			// Token: 0x040063A9 RID: 25513
			Failed
		}

		// Token: 0x02000D05 RID: 3333
		private struct BehaviourState
		{
			// Token: 0x040063AA RID: 25514
			public Behaviour Component;

			// Token: 0x040063AB RID: 25515
			public bool Enabled;
		}

		// Token: 0x02000D06 RID: 3334
		private sealed class Watcher
		{
			// Token: 0x040063AC RID: 25516
			public FieldPlayerLookAtCiel Field;

			// Token: 0x040063AD RID: 25517
			public NPCSimpleLookAt LookAt;

			// Token: 0x040063AE RID: 25518
			public Transform Head;

			// Token: 0x040063AF RID: 25519
			public Transform Neck;

			// Token: 0x040063B0 RID: 25520
			public Quaternion HeadPose;

			// Token: 0x040063B1 RID: 25521
			public Quaternion NeckPose;

			// Token: 0x040063B2 RID: 25522
			public Vector3 HeadOffset;

			// Token: 0x040063B3 RID: 25523
			public Vector3 NeckOffset;

			// Token: 0x040063B4 RID: 25524
			public bool Looking;
		}

		// Token: 0x02000D07 RID: 3335
		private enum BagState
		{
			// Token: 0x040063B6 RID: 25526
			None,
			// Token: 0x040063B7 RID: 25527
			InHand,
			// Token: 0x040063B8 RID: 25528
			Worn,
			// Token: 0x040063B9 RID: 25529
			Shivering,
			// Token: 0x040063BA RID: 25530
			Flying
		}

		// Token: 0x02000D08 RID: 3336
		private sealed class MixedSource
		{
			// Token: 0x040063BB RID: 25531
			public AudioSource Source;

			// Token: 0x040063BC RID: 25532
			public float Value;

			// Token: 0x040063BD RID: 25533
			public float Applied;
		}

		// Token: 0x02000D09 RID: 3337
		private sealed class WindLine
		{
			// Token: 0x040063BE RID: 25534
			public RectTransform Rect;

			// Token: 0x040063BF RID: 25535
			public Image Image;

			// Token: 0x040063C0 RID: 25536
			public float X;

			// Token: 0x040063C1 RID: 25537
			public float Y;

			// Token: 0x040063C2 RID: 25538
			public float Speed;

			// Token: 0x040063C3 RID: 25539
			public float Alpha;

			// Token: 0x040063C4 RID: 25540
			public float Threshold;

			// Token: 0x040063C5 RID: 25541
			public float Wobble;

			// Token: 0x040063C6 RID: 25542
			public float Phase;

			// Token: 0x040063C7 RID: 25543
			public float Length;
		}
	}
}
