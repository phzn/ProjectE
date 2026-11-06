using System;
using System.Collections.Generic;
using Features.BoobTouch;
using Features.Detection;
using Features.InsertableToys;
using Features.Inventory;
using Features.Inventory.Actions;
using Features.PlayerEmotes;
using Features.Police;
using ProjectExhibitionist.PhotoBooth;
using RayznGames.VehicleSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Features.NippleRotor
{
	// Token: 0x02000F5A RID: 3930
	[DefaultExecutionOrder(1001)]
	[DisallowMultipleComponent]
	public sealed class NippleRotorFaceDriver : MonoBehaviour
	{
		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x0600764E RID: 30286 RVA: 0x0030103F File Offset: 0x002FF23F
		public bool IsWanted
		{
			get
			{
				return this._wanted;
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x0600764F RID: 30287 RVA: 0x00301047 File Offset: 0x002FF247
		public bool IsHoldingFace
		{
			get
			{
				return this._holding;
			}
		}

		// Token: 0x06007650 RID: 30288 RVA: 0x00301050 File Offset: 0x002FF250
		public void Configure(Animator playerAnimator)
		{
			if (this._animator != playerAnimator)
			{
				this.ReleaseFace(true);
				this.UnhookEmotes();
				this.UnhookEmotion();
				this._animator = playerAnimator;
				this._emotion = null;
				this._toy = null;
				this._emotes = null;
				this._ragdoll = null;
				this._boobTouch = null;
				this._dildo = null;
				this._lockOwned = false;
				this._releasePending = false;
				this._fallbackWritten = false;
				this._fallbackBaselineValid = false;
				this._parameterAnimator = null;
			}
			this._nextResolveTime = float.NegativeInfinity;
			this.ResolveDependencies();
		}

		// Token: 0x06007651 RID: 30289 RVA: 0x003010E3 File Offset: 0x002FF2E3
		public void SetWanted(bool wanted)
		{
			if (this._wanted == wanted)
			{
				return;
			}
			this._wanted = wanted;
			if (wanted)
			{
				this._releasePending = false;
				return;
			}
			this.ReleaseFace(false);
		}

		// Token: 0x06007652 RID: 30290 RVA: 0x00301108 File Offset: 0x002FF308
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			NippleRotorFaceDriver.Drivers.Clear();
		}

		// Token: 0x06007653 RID: 30291 RVA: 0x00301114 File Offset: 0x002FF314
		private void LateUpdate()
		{
			this.Tick();
		}

		// Token: 0x06007654 RID: 30292 RVA: 0x0030111C File Offset: 0x002FF31C
		private void OnEnable()
		{
			if (!NippleRotorFaceDriver.Drivers.Contains(this))
			{
				NippleRotorFaceDriver.Drivers.Add(this);
			}
		}

		// Token: 0x06007655 RID: 30293 RVA: 0x00301136 File Offset: 0x002FF336
		private void OnDisable()
		{
			NippleRotorFaceDriver.Drivers.Remove(this);
			this.ReleaseFace(false);
		}

		// Token: 0x06007656 RID: 30294 RVA: 0x0030114B File Offset: 0x002FF34B
		private void OnDestroy()
		{
			this.ReleaseFace(true);
			this.UnhookEmotes();
			this.UnhookEmotion();
		}

		// Token: 0x06007657 RID: 30295 RVA: 0x00301160 File Offset: 0x002FF360
		public void Tick()
		{
			if (!this._wanted && !this._releasePending)
			{
				this._holding = false;
				return;
			}
			this.ResolveDependencies();
			if (!this._wanted)
			{
				this.ReleaseFace(false);
				return;
			}
			this._releasePending = false;
			if (this._emotion != null)
			{
				this.TickEmotionLock();
				return;
			}
			this.TickFallback();
		}

		// Token: 0x06007658 RID: 30296 RVA: 0x003011C0 File Offset: 0x002FF3C0
		private void TickEmotionLock()
		{
			this._holding = false;
			this._fallbackWritten = false;
			if (this._emotion.PlayerAnimator == null || string.IsNullOrEmpty(this._emotion.ExpressionParamName))
			{
				return;
			}
			bool isFacialExpressionOverriddenForAction = this._emotion.IsFacialExpressionOverriddenForAction;
			if (!isFacialExpressionOverriddenForAction)
			{
				this._lockOwned = false;
				this._coveredFace = -1;
			}
			NippleRotorFaceDriver.Yield yield = this.GetYield();
			if (yield == NippleRotorFaceDriver.Yield.Release)
			{
				this.ReleaseFace(false);
				return;
			}
			if (yield == NippleRotorFaceDriver.Yield.Pause)
			{
				return;
			}
			int num;
			if (!this.TryReadFace(out num))
			{
				if (this._lockOwned)
				{
					this.ReleaseFace(false);
				}
				return;
			}
			if (isFacialExpressionOverriddenForAction && num != 5 && !this.MayCover(num))
			{
				return;
			}
			if (!isFacialExpressionOverriddenForAction)
			{
				this._emotion.DisableFacialExpressionForAction();
				this._lockOwned = true;
			}
			else if (!this._lockOwned && num >= 1 && num <= 4)
			{
				this._coveredFace = num;
			}
			if (num != 5)
			{
				this.WriteFace(5);
			}
			this._holding = true;
		}

		// Token: 0x06007659 RID: 30297 RVA: 0x003012A1 File Offset: 0x002FF4A1
		private bool MayCover(int face)
		{
			return (face == this._parameterDefault && this._lockOwned && !this._foreignWrite) || (face >= 1 && face <= 4 && !PhotoBoothController.AnySessionActive);
		}

		// Token: 0x0600765A RID: 30298 RVA: 0x003012D0 File Offset: 0x002FF4D0
		private void WriteFace(int face)
		{
			this._writing = true;
			try
			{
				this._emotion.SetExpression(face);
			}
			finally
			{
				this._writing = false;
			}
			this._foreignWrite = false;
		}

		// Token: 0x0600765B RID: 30299 RVA: 0x00301314 File Offset: 0x002FF514
		private void TickFallback()
		{
			this._holding = false;
			int num;
			if (!this.TryReadFace(out num))
			{
				return;
			}
			NippleRotorFaceDriver.Yield yield = this.GetYield();
			if (yield != NippleRotorFaceDriver.Yield.None)
			{
				if (yield == NippleRotorFaceDriver.Yield.Release)
				{
					this.ReleaseFallback();
				}
				return;
			}
			if (!this._fallbackWritten)
			{
				this._fallbackBaseline = num;
				this._fallbackBaselineValid = true;
				this._fallbackWritten = true;
			}
			if (num != 5)
			{
				this._animator.SetInteger(this._parameterHash, 5);
			}
			this._holding = true;
		}

		// Token: 0x0600765C RID: 30300 RVA: 0x00301384 File Offset: 0x002FF584
		private void ReleaseFace(bool final)
		{
			this._holding = false;
			if (this._emotion == null)
			{
				this.ReleaseFallback();
				this._lockOwned = false;
				this._releasePending = false;
				return;
			}
			if (!this._lockOwned || !this._emotion.IsFacialExpressionOverriddenForAction)
			{
				int num;
				if (this._coveredFace >= 0 && this._emotion.IsFacialExpressionOverriddenForAction && !this.OtherFaceOwnerActive() && this.TryReadFace(out num) && num == 5)
				{
					this.WriteFace(this._coveredFace);
				}
				this._coveredFace = -1;
				this._lockOwned = false;
				this._releasePending = false;
				return;
			}
			int num2;
			bool flag = !this.TryReadFace(out num2) || num2 == 5 || (num2 == this._parameterDefault && !this._foreignWrite);
			bool flag2 = this.OtherFaceOwnerActive() || this.GetYield() == NippleRotorFaceDriver.Yield.Pause;
			if (flag && !flag2)
			{
				this._lockOwned = false;
				this._releasePending = false;
				this._coveredFace = -1;
				this._emotion.RestoreFacialExpression();
				this.RestoreClothedFace();
				return;
			}
			this._releasePending = !final;
			if (final)
			{
				this._lockOwned = false;
			}
		}

		// Token: 0x0600765D RID: 30301 RVA: 0x0030149C File Offset: 0x002FF69C
		private void RestoreClothedFace()
		{
			if (this._emotion.IsFacialExpressionOverriddenForAction || this._emotion.HomeBasePresentationOverrideActive)
			{
				return;
			}
			if (this._emotion.ClothingController != null && this._emotion.ClothingController.IsFullyClothed)
			{
				this.WriteFace(this._emotion.NeutralExpressionIndex);
			}
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x003014FC File Offset: 0x002FF6FC
		private void ReleaseFallback()
		{
			if (!this._fallbackWritten)
			{
				return;
			}
			this._fallbackWritten = false;
			int num;
			if (this._fallbackBaselineValid && this._fallbackBaseline != 5 && !this.OtherFaceOwnerActive() && this.TryReadFace(out num) && num == 5)
			{
				this._animator.SetInteger(this._parameterHash, this._fallbackBaseline);
			}
			this._fallbackBaselineValid = false;
		}

		// Token: 0x0600765F RID: 30303 RVA: 0x00301560 File Offset: 0x002FF760
		private NippleRotorFaceDriver.Yield GetYield()
		{
			PlayerStunExecutor instance = PlayerStunExecutor.Instance;
			if (instance != null && instance.IsStunned)
			{
				return NippleRotorFaceDriver.Yield.Pause;
			}
			if (this._boobTouch != null && this._boobTouch.IsActive)
			{
				return NippleRotorFaceDriver.Yield.Pause;
			}
			if (this._ragdoll != null && this._ragdoll.IsRecovering)
			{
				return NippleRotorFaceDriver.Yield.Release;
			}
			if (this._emotion != null && this._emotion.HomeBasePresentationOverrideActive)
			{
				return NippleRotorFaceDriver.Yield.Release;
			}
			return NippleRotorFaceDriver.Yield.None;
		}

		// Token: 0x06007660 RID: 30304 RVA: 0x003015E0 File Offset: 0x002FF7E0
		private bool OtherFaceOwnerActive()
		{
			if (this._toy != null && this._toy.IsInserted)
			{
				return true;
			}
			for (int i = 0; i < NippleRotorFaceDriver.Drivers.Count; i++)
			{
				NippleRotorFaceDriver nippleRotorFaceDriver = NippleRotorFaceDriver.Drivers[i];
				if (nippleRotorFaceDriver != null && nippleRotorFaceDriver != this && nippleRotorFaceDriver._wanted && nippleRotorFaceDriver._animator == this._animator)
				{
					return true;
				}
			}
			return (this._dildo != null && this._dildo.IsExecuting) || (this._masturbationFaceActive && this._subscribedEmotes != null) || (this._emotes != null && this._emotes.IsPlaying && this._emotes.PlayingEmote != null && this._emotes.PlayingEmote.IsMasturbationEmote);
		}

		// Token: 0x06007661 RID: 30305 RVA: 0x003016D0 File Offset: 0x002FF8D0
		private bool TryReadFace(out int face)
		{
			bool flag = this._emotion != null && this._emotion.PlayerAnimator != null;
			Animator animator = (flag ? this._emotion.PlayerAnimator : this._animator);
			string text = ((flag && !string.IsNullOrEmpty(this._emotion.ExpressionParamName)) ? this._emotion.ExpressionParamName : "FaceExpressionIndex");
			return this.TryGetIntParameter(animator, text, out face);
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x00301748 File Offset: 0x002FF948
		private bool TryGetIntParameter(Animator animator, string name, out int value)
		{
			value = 0;
			if (animator == null || string.IsNullOrEmpty(name))
			{
				return false;
			}
			RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
			if (runtimeAnimatorController == null || !animator.isInitialized)
			{
				return false;
			}
			if (this._parameterAnimator != animator || this._parameterController != runtimeAnimatorController || this._parameterName != name || (!this._parameterExists && Time.unscaledTime >= this._parameterRetryTime))
			{
				this._parameterAnimator = animator;
				this._parameterController = runtimeAnimatorController;
				this._parameterName = name;
				this._parameterExists = false;
				this._parameterRetryTime = Time.unscaledTime + 2f;
				foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && !(animatorControllerParameter.name != name))
					{
						this._parameterHash = animatorControllerParameter.nameHash;
						this._parameterDefault = animatorControllerParameter.defaultInt;
						this._parameterExists = true;
						break;
					}
				}
			}
			if (!this._parameterExists)
			{
				return false;
			}
			value = animator.GetInteger(this._parameterHash);
			return true;
		}

		// Token: 0x06007663 RID: 30307 RVA: 0x00301864 File Offset: 0x002FFA64
		private void ResolveDependencies()
		{
			if (this._animator == null || Time.unscaledTime < this._nextResolveTime)
			{
				return;
			}
			if (this._emotion != null && this._toy != null && this._emotes != null && this._ragdoll != null && this._boobTouch != null && this._dildo != null && this._subscribedEmotion == this._emotion)
			{
				return;
			}
			this._nextResolveTime = Time.unscaledTime + 2f;
			if (this._emotion == null)
			{
				PlayerEmotionController playerEmotionController = this.ResolveEmotionController();
				if (playerEmotionController != null)
				{
					this._emotion = playerEmotionController;
					this._lockOwned = false;
					this._releasePending = false;
				}
			}
			if (this._toy == null)
			{
				PlayerInventoryManager instance = PlayerInventoryManager.Instance;
				if (instance != null && instance.PlayerAnimator == this._animator)
				{
					this._toy = instance.GetComponent<InsertableToyController>();
				}
				if (this._toy == null)
				{
					this._toy = this._animator.GetComponentInParent<InsertableToyController>(true);
				}
			}
			if (this._emotes == null)
			{
				this._emotes = this._animator.GetComponentInParent<GameplayEmoteController>(true);
				if (this._emotes == null)
				{
					this._emotes = this._animator.GetComponentInChildren<GameplayEmoteController>(true);
				}
			}
			if (this._ragdoll == null)
			{
				this._ragdoll = this._animator.GetComponentInParent<CielRagdollRecovery>(true);
			}
			if (this._boobTouch == null)
			{
				this._boobTouch = this._animator.GetComponentInParent<BoobTouchModeController>(true);
				if (this._boobTouch == null)
				{
					this._boobTouch = this._animator.GetComponentInChildren<BoobTouchModeController>(true);
				}
			}
			if (this._dildo == null)
			{
				this._dildo = this._animator.GetComponentInChildren<DildoActionExecutor>(true);
				if (this._dildo == null)
				{
					this._dildo = this._animator.GetComponentInParent<DildoActionExecutor>(true);
				}
			}
			this.HookEmotes();
			this.HookEmotion();
		}

		// Token: 0x06007664 RID: 30308 RVA: 0x00301A84 File Offset: 0x002FFC84
		private void HookEmotion()
		{
			if (this._emotion == this._subscribedEmotion)
			{
				return;
			}
			this.UnhookEmotion();
			if (this._emotion == null)
			{
				return;
			}
			if (this._emotion.OnExpressionTriggered == null)
			{
				this._emotion.OnExpressionTriggered = new UnityEvent<int>();
			}
			this._emotion.OnExpressionTriggered.AddListener(new UnityAction<int>(this.HandleExpressionTriggered));
			this._subscribedEmotion = this._emotion;
			this._foreignWrite = false;
		}

		// Token: 0x06007665 RID: 30309 RVA: 0x00301B06 File Offset: 0x002FFD06
		private void UnhookEmotion()
		{
			if (this._subscribedEmotion != null && this._subscribedEmotion.OnExpressionTriggered != null)
			{
				this._subscribedEmotion.OnExpressionTriggered.RemoveListener(new UnityAction<int>(this.HandleExpressionTriggered));
			}
			this._subscribedEmotion = null;
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x00301B46 File Offset: 0x002FFD46
		private void HandleExpressionTriggered(int face)
		{
			if (this._writing)
			{
				return;
			}
			this._foreignWrite = face != 5;
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x00301B60 File Offset: 0x002FFD60
		private PlayerEmotionController ResolveEmotionController()
		{
			PlayerEmotionController playerEmotionController = this._animator.GetComponentInParent<PlayerEmotionController>(true);
			if (playerEmotionController == null)
			{
				playerEmotionController = this._animator.GetComponentInChildren<PlayerEmotionController>(true);
			}
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (playerEmotionController == null && instance != null && instance.PlayerAnimator == this._animator)
			{
				playerEmotionController = instance;
			}
			return playerEmotionController;
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x00301BC0 File Offset: 0x002FFDC0
		private void HookEmotes()
		{
			if (this._emotes == this._subscribedEmotes)
			{
				return;
			}
			this.UnhookEmotes();
			if (this._emotes == null)
			{
				return;
			}
			this._emotes.OnEmoteStarted += this.HandleEmoteStarted;
			this._emotes.OnEmoteEnded += this.HandleEmoteEnded;
			this._subscribedEmotes = this._emotes;
		}

		// Token: 0x06007669 RID: 30313 RVA: 0x00301C30 File Offset: 0x002FFE30
		private void UnhookEmotes()
		{
			if (this._subscribedEmotes != null)
			{
				this._subscribedEmotes.OnEmoteStarted -= this.HandleEmoteStarted;
				this._subscribedEmotes.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._subscribedEmotes = null;
			this._masturbationFaceActive = false;
		}

		// Token: 0x0600766A RID: 30314 RVA: 0x00301C81 File Offset: 0x002FFE81
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote != null && emote.IsMasturbationEmote)
			{
				this._masturbationFaceActive = true;
			}
		}

		// Token: 0x0600766B RID: 30315 RVA: 0x00301C9B File Offset: 0x002FFE9B
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (emote != null && emote.IsMasturbationEmote)
			{
				this._masturbationFaceActive = false;
			}
		}

		// Token: 0x04007973 RID: 31091
		public const int OrgasmFaceIndex = 5;

		// Token: 0x04007974 RID: 31092
		private const int FirstEmotionFace = 1;

		// Token: 0x04007975 RID: 31093
		private const int LastEmotionFace = 4;

		// Token: 0x04007976 RID: 31094
		private const string DefaultFaceParameter = "FaceExpressionIndex";

		// Token: 0x04007977 RID: 31095
		private const float DependencyRetrySeconds = 2f;

		// Token: 0x04007978 RID: 31096
		private Animator _animator;

		// Token: 0x04007979 RID: 31097
		private PlayerEmotionController _emotion;

		// Token: 0x0400797A RID: 31098
		private InsertableToyController _toy;

		// Token: 0x0400797B RID: 31099
		private GameplayEmoteController _emotes;

		// Token: 0x0400797C RID: 31100
		private GameplayEmoteController _subscribedEmotes;

		// Token: 0x0400797D RID: 31101
		private CielRagdollRecovery _ragdoll;

		// Token: 0x0400797E RID: 31102
		private BoobTouchModeController _boobTouch;

		// Token: 0x0400797F RID: 31103
		private DildoActionExecutor _dildo;

		// Token: 0x04007980 RID: 31104
		private PlayerEmotionController _subscribedEmotion;

		// Token: 0x04007981 RID: 31105
		private float _nextResolveTime = float.NegativeInfinity;

		// Token: 0x04007982 RID: 31106
		private bool _wanted;

		// Token: 0x04007983 RID: 31107
		private bool _holding;

		// Token: 0x04007984 RID: 31108
		private bool _lockOwned;

		// Token: 0x04007985 RID: 31109
		private bool _releasePending;

		// Token: 0x04007986 RID: 31110
		private bool _masturbationFaceActive;

		// Token: 0x04007987 RID: 31111
		private bool _fallbackWritten;

		// Token: 0x04007988 RID: 31112
		private bool _writing;

		// Token: 0x04007989 RID: 31113
		private bool _foreignWrite;

		// Token: 0x0400798A RID: 31114
		private int _coveredFace = -1;

		// Token: 0x0400798B RID: 31115
		private bool _fallbackBaselineValid;

		// Token: 0x0400798C RID: 31116
		private int _fallbackBaseline;

		// Token: 0x0400798D RID: 31117
		private Animator _parameterAnimator;

		// Token: 0x0400798E RID: 31118
		private RuntimeAnimatorController _parameterController;

		// Token: 0x0400798F RID: 31119
		private string _parameterName;

		// Token: 0x04007990 RID: 31120
		private int _parameterHash;

		// Token: 0x04007991 RID: 31121
		private int _parameterDefault;

		// Token: 0x04007992 RID: 31122
		private bool _parameterExists;

		// Token: 0x04007993 RID: 31123
		private float _parameterRetryTime;

		// Token: 0x04007994 RID: 31124
		private static readonly List<NippleRotorFaceDriver> Drivers = new List<NippleRotorFaceDriver>(2);

		// Token: 0x02000F5B RID: 3931
		private enum Yield
		{
			// Token: 0x04007996 RID: 31126
			None,
			// Token: 0x04007997 RID: 31127
			Pause,
			// Token: 0x04007998 RID: 31128
			Release
		}
	}
}
