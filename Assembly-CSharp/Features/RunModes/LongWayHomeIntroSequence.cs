using System;
using System.Collections;
using Cinemachine;
using Features.Detection;
using Features.PhoneCompanion;
using Features.PrisonIntro;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000CD1 RID: 3281
	[DefaultExecutionOrder(-1000)]
	public sealed class LongWayHomeIntroSequence : MonoBehaviour
	{
		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06005F60 RID: 24416 RVA: 0x0025CA3D File Offset: 0x0025AC3D
		// (set) Token: 0x06005F61 RID: 24417 RVA: 0x0025CA45 File Offset: 0x0025AC45
		public bool IsPrepared { get; private set; }

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06005F62 RID: 24418 RVA: 0x0025CA4E File Offset: 0x0025AC4E
		// (set) Token: 0x06005F63 RID: 24419 RVA: 0x0025CA56 File Offset: 0x0025AC56
		public bool CompletedSuccessfully { get; private set; }

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06005F64 RID: 24420 RVA: 0x0025CA5F File Offset: 0x0025AC5F
		public bool CanSkip
		{
			get
			{
				return this.IsPrepared && this._playbackStarted && !this._skipRequested && this._sequence != null && !this._sequence.HasFinished;
			}
		}

		// Token: 0x06005F65 RID: 24421 RVA: 0x0025CA97 File Offset: 0x0025AC97
		public bool TrySkip()
		{
			if (!this.CanSkip)
			{
				return false;
			}
			this._skipRequested = true;
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint != null)
			{
				skipHint.Dispose();
			}
			return true;
		}

		// Token: 0x06005F66 RID: 24422 RVA: 0x0025CABC File Offset: 0x0025ACBC
		private void Update()
		{
			if (this.CanSkip)
			{
				if (this._skipHint == null)
				{
					this._skipHint = new LongWayHomeCinematicSkip(this);
				}
				this._skipHint.Show();
				if (LongWayHomeCinematicSkip.TryConsume(this))
				{
					this.TrySkip();
				}
				return;
			}
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint == null)
			{
				return;
			}
			skipHint.Dispose();
		}

		// Token: 0x06005F67 RID: 24423 RVA: 0x0025CB10 File Offset: 0x0025AD10
		public bool Prepare(Transform player)
		{
			this.Cleanup();
			this.CompletedSuccessfully = false;
			bool flag;
			try
			{
				vThirdPersonInput vThirdPersonInput = ((player != null) ? player.GetComponent<vThirdPersonInput>() : null);
				PlayerActionAnimator playerActionAnimator = ((player != null) ? player.GetComponent<PlayerActionAnimator>() : null);
				GameObject gameObject = Resources.Load<GameObject>("LongWayHomeIntroRig");
				LongWayHomeIntroRig longWayHomeIntroRig = ((gameObject != null) ? gameObject.GetComponent<LongWayHomeIntroRig>() : null);
				if (vThirdPersonInput == null || vThirdPersonInput.cc == null || vThirdPersonInput.cc.animator == null || playerActionAnimator == null || playerActionAnimator.GenericAction == null || Camera.main == null || Camera.main.GetComponent<CinemachineBrain>() == null || longWayHomeIntroRig == null || longWayHomeIntroRig.Sequence == null || longWayHomeIntroRig.VoiceClip == null)
				{
					Debug.LogWarning("[LongWayHome] Missing player or authored wake-up rig dependencies.", this);
					flag = false;
				}
				else
				{
					this._rigRoot = new GameObject("[LongWayHome WakeUp]");
					this._rigRoot.SetActive(false);
					this._rigRoot.transform.SetParent(base.transform, false);
					Quaternion quaternion = Quaternion.Euler(0f, player.eulerAngles.y, 0f) * Quaternion.Inverse(longWayHomeIntroRig.SourcePlayerRotation);
					this._rigRoot.transform.SetPositionAndRotation(player.position - quaternion * longWayHomeIntroRig.SourcePlayerPosition, quaternion);
					LongWayHomeIntroRig longWayHomeIntroRig2 = Object.Instantiate<LongWayHomeIntroRig>(longWayHomeIntroRig, this._rigRoot.transform, false);
					this._sequence = longWayHomeIntroRig2.Sequence;
					Transform transform = LongWayHomeIntroSequence.FindNose(vThirdPersonInput.cc.animator.transform);
					if (transform == null || longWayHomeIntroRig2.VoiceOutput == null)
					{
						Debug.LogWarning("[LongWayHome] Wake-up nose target or audio output is missing.", this);
						flag = false;
					}
					else
					{
						this._sequence.ConfigureForGameplay(vThirdPersonInput, transform);
						PlayerActivityGate.Push("LongWayHome.WakeUp", PlayerActivity.AllPlayerActions);
						this._activityHeld = true;
						SmartphoneController componentInChildren = player.GetComponentInChildren<SmartphoneController>(true);
						this._phone = LongWayHomeWakePhone.CreateInactive(componentInChildren, player, base.transform, longWayHomeIntroRig2.PhoneGroundOffset);
						GameObject gameObject2 = vThirdPersonInput.cc.animator.gameObject;
						this._audioRelay = gameObject2.GetComponent<AnimationEventAudioRelay>();
						this._createdAudioRelay = this._audioRelay == null;
						if (this._createdAudioRelay)
						{
							this._audioRelay = gameObject2.AddComponent<AnimationEventAudioRelay>();
						}
						this._previousClip = this._audioRelay.DefaultClip;
						this._previousOutput = this._audioRelay.Output;
						this._previousVolume = this._audioRelay.Volume;
						this._audioRelay.Configure(longWayHomeIntroRig2.VoiceClip, longWayHomeIntroRig2.VoiceOutput, 1f);
						this._input = vThirdPersonInput;
						this._action = playerActionAnimator;
						this.IsPrepared = true;
						flag = true;
					}
				}
			}
			finally
			{
				if (!this.IsPrepared)
				{
					this.Cleanup();
				}
			}
			return flag;
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x0025CE1C File Offset: 0x0025B01C
		public IEnumerator Play(Transform player)
		{
			if (!this.Prepare(player))
			{
				yield break;
			}
			yield return this.PlayPrepared();
			yield break;
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x0025CE32 File Offset: 0x0025B032
		public IEnumerator PlayPrepared()
		{
			if (this._playbackStarted)
			{
				yield break;
			}
			if (!this.IsPrepared || this._rigRoot == null || this._sequence == null || this._input == null || this._input.cc == null || this._input.cc.animator == null || this._action == null || this._action.GenericAction == null)
			{
				this.Cleanup();
				yield break;
			}
			vThirdPersonInput input = this._input;
			PlayerActionAnimator action = this._action;
			this._playbackStarted = true;
			try
			{
				this._rigRoot.SetActive(true);
				this._sequence.PlaySequence();
				if (this._phone != null)
				{
					yield return null;
					if (!this.IsPrepared)
					{
						yield break;
					}
					LongWayHomeWakePhone phone = this._phone;
					if (phone != null)
					{
						phone.Reveal();
					}
				}
				float deadline = Time.time + 35f;
				while (!this._skipRequested && this._sequence != null && !this._sequence.HasFinished && Time.time < deadline)
				{
					yield return null;
				}
				if (this._skipRequested)
				{
					yield return this.SkipPrepared();
					yield break;
				}
				if (this._sequence != null && this._sequence.CompletedSuccessfully && input != null && input.cc != null && input.cc.animator != null)
				{
					LongWayHomeIntroSequence.BeginStandingHandoff(action, input.cc.animator);
					float handoffDeadline = Time.time + 1f;
					while (input != null && input.cc != null && action != null && action.GenericAction != null && (input.cc.customAction || action.GenericAction.doingAction) && Time.time < handoffDeadline)
					{
						yield return null;
					}
					this.CompletedSuccessfully = input != null && input.cc != null && action != null && action.GenericAction != null && !input.cc.customAction && !action.GenericAction.doingAction;
					if (!this.CompletedSuccessfully && action != null && action.GenericAction != null)
					{
						vTriggerGenericAction triggerAction = action.GenericAction.triggerAction;
						if (Time.time >= handoffDeadline && input != null && input.cc != null && input.cc.animator != null)
						{
							AnimatorStateInfo currentAnimatorStateInfo = input.cc.animator.GetCurrentAnimatorStateInfo(0);
							Debug.LogWarning(string.Concat(new string[]
							{
								"[LongWayHome] Wake-up locomotion handoff timed out after 1s: ",
								string.Format("customAction={0}, doingAction={1}, ", input.cc.customAction, action.GenericAction.doingAction),
								string.Format("stateHash={0}, normalizedTime={1:F3}, ", currentAnimatorStateInfo.shortNameHash, currentAnimatorStateInfo.normalizedTime),
								string.Format("inTransition={0}, ", input.cc.animator.IsInTransition(0)),
								"trigger=",
								(triggerAction != null) ? triggerAction.playAnimation : "<none>",
								"."
							}), this);
						}
						if (triggerAction == null || triggerAction.playAnimation == "PrisonGetUp")
						{
							action.CancelCurrentAction();
						}
					}
				}
			}
			finally
			{
				this.Cleanup();
			}
			yield break;
			yield break;
		}

		// Token: 0x06005F6A RID: 24426 RVA: 0x0025CE41 File Offset: 0x0025B041
		private IEnumerator SkipPrepared()
		{
			ScreenFadeController fade = ScreenFadeController.Instance;
			if (fade != null)
			{
				LongWayHomeIntroSequence.<>c__DisplayClass31_0 CS$<>8__locals1 = new LongWayHomeIntroSequence.<>c__DisplayClass31_0();
				CS$<>8__locals1.covered = false;
				fade.FadeOut(0.2f, delegate
				{
					CS$<>8__locals1.covered = true;
				});
				float fadeDeadline = Time.realtimeSinceStartup + 2f;
				while (!CS$<>8__locals1.covered && Time.realtimeSinceStartup < fadeDeadline)
				{
					yield return null;
				}
				fade.SetBlack();
				CS$<>8__locals1 = null;
			}
			PlayerActionAnimator action = this._action;
			vThirdPersonInput input = this._input;
			if (action == null || input == null || input.cc == null)
			{
				yield break;
			}
			bool smoothFinishAction = action.SmoothFinishAction;
			float smoothFinishDuration = action.SmoothFinishDuration;
			try
			{
				action.SmoothFinishAction = true;
				action.SmoothFinishDuration = 0.05f;
				PrisonIntroSequence sequence = this._sequence;
				if (sequence != null)
				{
					sequence.CancelSequence();
				}
				vTriggerGenericAction vTriggerGenericAction = ((action.GenericAction != null) ? action.GenericAction.triggerAction : null);
				if (action.GenericAction != null && (vTriggerGenericAction == null || vTriggerGenericAction.playAnimation == "PrisonGetUp") && (action.GenericAction.doingAction || input.cc.customAction))
				{
					action.CancelCurrentAction();
				}
			}
			finally
			{
				action.SmoothFinishAction = smoothFinishAction;
				action.SmoothFinishDuration = smoothFinishDuration;
			}
			float deadline = Time.realtimeSinceStartup + 1f;
			while (input != null && input.cc != null && action != null && action.GenericAction != null && (input.cc.customAction || action.GenericAction.doingAction) && Time.realtimeSinceStartup < deadline)
			{
				yield return null;
			}
			this.CompletedSuccessfully = input != null && input.cc != null && action != null && action.GenericAction != null && !input.cc.customAction && !action.GenericAction.doingAction;
			if (this.CompletedSuccessfully)
			{
				if (input.tpCamera != null && input.tpCamera.isInit)
				{
					input.tpCamera.ResetAngleWithoutSmooth();
					input.tpCamera.CameraMovement(true);
				}
				ScreenFadeController screenFadeController = fade;
				if (screenFadeController != null)
				{
					screenFadeController.FadeIn(0.2f, null);
				}
			}
			yield break;
		}

		// Token: 0x06005F6B RID: 24427 RVA: 0x0025CE50 File Offset: 0x0025B050
		private static bool BeginStandingHandoff(PlayerActionAnimator action, Animator animator)
		{
			if (action == null || action.GenericAction == null || animator == null || !animator.isInitialized)
			{
				return false;
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.shortNameHash != Animator.StringToHash("PrisonGetUp") || currentAnimatorStateInfo.normalizedTime < 0.9f)
			{
				return false;
			}
			vTriggerGenericAction triggerAction = action.GenericAction.triggerAction;
			if (triggerAction != null && triggerAction.playAnimation != "PrisonGetUp")
			{
				return false;
			}
			bool smoothFinishAction = action.SmoothFinishAction;
			float smoothFinishDuration = action.SmoothFinishDuration;
			try
			{
				action.SmoothFinishAction = true;
				action.SmoothFinishDuration = 0.15f;
				action.FinishCurrentAction();
			}
			finally
			{
				action.SmoothFinishAction = smoothFinishAction;
				action.SmoothFinishDuration = smoothFinishDuration;
			}
			return true;
		}

		// Token: 0x06005F6C RID: 24428 RVA: 0x0025CF24 File Offset: 0x0025B124
		private static Transform FindNose(Transform animatorRoot)
		{
			foreach (Transform transform in animatorRoot.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == "c_nose_03.x")
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x0025CF60 File Offset: 0x0025B160
		private void OnDisable()
		{
			this.Cleanup();
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x0025CF68 File Offset: 0x0025B168
		private void Cleanup()
		{
			LongWayHomeCinematicSkip skipHint = this._skipHint;
			if (skipHint != null)
			{
				skipHint.Dispose();
			}
			this._skipRequested = false;
			this.IsPrepared = false;
			if (this._phone != null)
			{
				this._phone.gameObject.SetActive(false);
				LongWayHomeIntroSequence.DestroyOwned(this._phone.gameObject);
			}
			this._phone = null;
			if (this._playbackStarted && this._sequence != null && !this._sequence.HasFinished)
			{
				this._sequence.CancelSequence();
			}
			this._sequence = null;
			this._playbackStarted = false;
			this._input = null;
			this._action = null;
			if (this._audioRelay != null)
			{
				this._audioRelay.Configure(this._previousClip, this._previousOutput, this._previousVolume);
				if (this._createdAudioRelay)
				{
					LongWayHomeIntroSequence.DestroyOwned(this._audioRelay);
				}
			}
			this._audioRelay = null;
			this._createdAudioRelay = false;
			this._previousClip = null;
			this._previousOutput = null;
			if (this._rigRoot != null)
			{
				this._rigRoot.SetActive(false);
				LongWayHomeIntroSequence.DestroyOwned(this._rigRoot);
			}
			this._rigRoot = null;
			if (this._activityHeld)
			{
				PlayerActivityGate.Pop("LongWayHome.WakeUp");
			}
			this._activityHeld = false;
		}

		// Token: 0x06005F6F RID: 24431 RVA: 0x00027EE2 File Offset: 0x000260E2
		private static void DestroyOwned(Object value)
		{
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x040060D2 RID: 24786
		public const string RigResourcePath = "LongWayHomeIntroRig";

		// Token: 0x040060D3 RID: 24787
		private const string ActivityReason = "LongWayHome.WakeUp";

		// Token: 0x040060D4 RID: 24788
		private GameObject _rigRoot;

		// Token: 0x040060D5 RID: 24789
		private PrisonIntroSequence _sequence;

		// Token: 0x040060D6 RID: 24790
		private AnimationEventAudioRelay _audioRelay;

		// Token: 0x040060D7 RID: 24791
		private bool _createdAudioRelay;

		// Token: 0x040060D8 RID: 24792
		private AudioClip _previousClip;

		// Token: 0x040060D9 RID: 24793
		private AudioSource _previousOutput;

		// Token: 0x040060DA RID: 24794
		private float _previousVolume;

		// Token: 0x040060DB RID: 24795
		private bool _activityHeld;

		// Token: 0x040060DC RID: 24796
		private LongWayHomeWakePhone _phone;

		// Token: 0x040060DD RID: 24797
		private vThirdPersonInput _input;

		// Token: 0x040060DE RID: 24798
		private PlayerActionAnimator _action;

		// Token: 0x040060DF RID: 24799
		private bool _playbackStarted;

		// Token: 0x040060E0 RID: 24800
		private bool _skipRequested;

		// Token: 0x040060E1 RID: 24801
		private LongWayHomeCinematicSkip _skipHint;
	}
}
