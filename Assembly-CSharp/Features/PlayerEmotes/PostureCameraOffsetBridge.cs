using System;
using Features.Detection;
using Invector.vCamera;
using Invector.vCharacterController;
using UnityEngine;

namespace Features.PlayerEmotes
{
	// Token: 0x02000E2B RID: 3627
	[DisallowMultipleComponent]
	public sealed class PostureCameraOffsetBridge : MonoBehaviour
	{
		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06006C33 RID: 27699 RVA: 0x002B49C1 File Offset: 0x002B2BC1
		public float AppliedOffset
		{
			get
			{
				return this._appliedOffset;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x06006C34 RID: 27700 RVA: 0x002B49C9 File Offset: 0x002B2BC9
		public float TargetOffset
		{
			get
			{
				return this._targetOffset;
			}
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x002B49D1 File Offset: 0x002B2BD1
		private void OnEnable()
		{
			this.ResolveReferences();
			if (this.emoteController == null)
			{
				base.enabled = false;
				return;
			}
			this.Subscribe();
			this.ReconcilePlayingEmote();
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x002B49FC File Offset: 0x002B2BFC
		private void LateUpdate()
		{
			if (this.emoteController == null)
			{
				base.enabled = false;
				return;
			}
			if (this.playerCamera == null)
			{
				this.ResolveReferences();
				this.Subscribe();
			}
			this.ReconcilePlayingEmote();
			this.AdvanceOffset(Time.unscaledDeltaTime);
		}

		// Token: 0x06006C37 RID: 27703 RVA: 0x002B4A4C File Offset: 0x002B2C4C
		private void ResolveReferences()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
				if (this.emoteController == null)
				{
					this.emoteController = base.GetComponentInParent<GameplayEmoteController>();
				}
			}
			if (this.emoteController == null)
			{
				return;
			}
			if (this.playerCamera != null)
			{
				return;
			}
			vThirdPersonInput vThirdPersonInput = base.GetComponent<vThirdPersonInput>();
			if (vThirdPersonInput == null)
			{
				vThirdPersonInput = base.GetComponentInParent<vThirdPersonInput>();
			}
			if (vThirdPersonInput != null)
			{
				this.playerCamera = vThirdPersonInput.tpCamera;
			}
			if (this.playerCamera == null)
			{
				this.playerCamera = base.GetComponentInChildren<vThirdPersonCamera>(true);
			}
		}

		// Token: 0x06006C38 RID: 27704 RVA: 0x002B4AF4 File Offset: 0x002B2CF4
		private void Subscribe()
		{
			if (this._subscribedController == this.emoteController)
			{
				return;
			}
			this.Unsubscribe();
			this._subscribedController = this.emoteController;
			if (this._subscribedController == null)
			{
				return;
			}
			this._subscribedController.OnEmoteStarted += this.HandleEmoteStarted;
			this._subscribedController.OnEmoteEnded += this.HandleEmoteEnded;
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x002B4B64 File Offset: 0x002B2D64
		private void Unsubscribe()
		{
			if (this._subscribedController == null)
			{
				return;
			}
			this._subscribedController.OnEmoteStarted -= this.HandleEmoteStarted;
			this._subscribedController.OnEmoteEnded -= this.HandleEmoteEnded;
			this._subscribedController = null;
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x002B4BB8 File Offset: 0x002B2DB8
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			float num;
			if (!this.TryGetPostureOffset(emote, out num))
			{
				return;
			}
			this.SetPostureTarget(emote, num);
		}

		// Token: 0x06006C3B RID: 27707 RVA: 0x002B4BD9 File Offset: 0x002B2DD9
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (this._activePosture == null || !PostureCameraOffsetBridge.MatchesEmote(emote, this._activePosture, null))
			{
				return;
			}
			this.BeginExit();
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x002B4C00 File Offset: 0x002B2E00
		private void ReconcilePlayingEmote()
		{
			EmoteDefinitionSO emoteDefinitionSO = ((this.emoteController != null && this.emoteController.IsPlaying) ? this.emoteController.PlayingEmote : null);
			float num;
			if (this.TryGetPostureOffset(emoteDefinitionSO, out num))
			{
				if (!PostureCameraOffsetBridge.MatchesEmote(emoteDefinitionSO, this._activePosture, null) || !Mathf.Approximately(this._targetOffset, num))
				{
					this.SetPostureTarget(emoteDefinitionSO, num);
				}
				return;
			}
			if (this._activePosture != null)
			{
				this.BeginExit();
			}
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x002B4C7C File Offset: 0x002B2E7C
		private void BeginExit()
		{
			this._activePosture = null;
			if (!Mathf.Approximately(this._targetOffset, 0f))
			{
				this._offsetVelocity = 0f;
			}
			this._targetOffset = 0f;
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x002B4CAD File Offset: 0x002B2EAD
		private void SetPostureTarget(EmoteDefinitionSO emote, float offset)
		{
			this._activePosture = emote;
			if (!Mathf.Approximately(this._targetOffset, offset))
			{
				this._offsetVelocity = 0f;
			}
			this._targetOffset = offset;
		}

		// Token: 0x06006C3F RID: 27711 RVA: 0x002B4CD8 File Offset: 0x002B2ED8
		private bool TryGetPostureOffset(EmoteDefinitionSO emote, out float offset)
		{
			if (PostureCameraOffsetBridge.MatchesEmote(emote, this.crouchEmote, "crouch_walk"))
			{
				offset = this.crouchOffset;
				return true;
			}
			if (PostureCameraOffsetBridge.MatchesEmote(emote, this.crawlEmote, "crawl_walk"))
			{
				offset = this.crawlOffset;
				return true;
			}
			if (PostureCameraOffsetBridge.MatchesEmote(emote, this.proneEmote, "prone_walk"))
			{
				offset = this.proneOffset;
				return true;
			}
			offset = 0f;
			return false;
		}

		// Token: 0x06006C40 RID: 27712 RVA: 0x002B4D44 File Offset: 0x002B2F44
		private static bool MatchesEmote(EmoteDefinitionSO candidate, EmoteDefinitionSO configured, string fallbackId)
		{
			if (candidate == null)
			{
				return false;
			}
			if (candidate == configured)
			{
				return true;
			}
			string text = ((configured != null && !string.IsNullOrWhiteSpace(configured.EmoteId)) ? configured.EmoteId : fallbackId);
			return !string.IsNullOrWhiteSpace(text) && string.Equals(candidate.EmoteId, text, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x002B4DA0 File Offset: 0x002B2FA0
		private void AdvanceOffset(float deltaTime)
		{
			this.EnsureOffsetCamera();
			if (this._offsetCamera == null)
			{
				return;
			}
			float num = PostureCameraOffsetBridge.SanitizeDeltaTime(deltaTime);
			if (Mathf.Approximately(this._targetOffset, 0f) && Mathf.Approximately(this._appliedOffset, 0f))
			{
				this._offsetVelocity = 0f;
				return;
			}
			float num2 = this._offsetCamera.offSetPlayerPivot - this._appliedOffset;
			float num3 = (Mathf.Approximately(this._targetOffset, 0f) ? this.exitSmoothTime : this.enterSmoothTime);
			if (num3 <= 0f || num <= 0f)
			{
				if (num3 <= 0f)
				{
					this._appliedOffset = this._targetOffset;
					this._offsetVelocity = 0f;
				}
			}
			else
			{
				this._appliedOffset = Mathf.SmoothDamp(this._appliedOffset, this._targetOffset, ref this._offsetVelocity, num3, float.PositiveInfinity, num);
			}
			if (Mathf.Abs(this._appliedOffset - this._targetOffset) <= 0.0001f)
			{
				this._appliedOffset = this._targetOffset;
				this._offsetVelocity = 0f;
			}
			this._offsetCamera.offSetPlayerPivot = num2 + this._appliedOffset;
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x002B4EC5 File Offset: 0x002B30C5
		private void EnsureOffsetCamera()
		{
			if (this._offsetCamera == this.playerCamera)
			{
				return;
			}
			this.RemoveOwnedOffset();
			this._offsetCamera = this.playerCamera;
		}

		// Token: 0x06006C43 RID: 27715 RVA: 0x002B4EF0 File Offset: 0x002B30F0
		private void RemoveOwnedOffset()
		{
			if (this._offsetCamera != null && !Mathf.Approximately(this._appliedOffset, 0f))
			{
				this._offsetCamera.offSetPlayerPivot -= this._appliedOffset;
			}
			this._offsetCamera = null;
			this._appliedOffset = 0f;
			this._offsetVelocity = 0f;
		}

		// Token: 0x06006C44 RID: 27716 RVA: 0x002B4F52 File Offset: 0x002B3152
		private static float SanitizeDeltaTime(float deltaTime)
		{
			if (!float.IsNaN(deltaTime) && !float.IsInfinity(deltaTime) && deltaTime >= 0f)
			{
				return deltaTime;
			}
			return 0f;
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x002B4F73 File Offset: 0x002B3173
		private void OnValidate()
		{
			this.enterSmoothTime = Mathf.Max(0f, this.enterSmoothTime);
			this.exitSmoothTime = Mathf.Max(0f, this.exitSmoothTime);
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x002B4FA1 File Offset: 0x002B31A1
		private void OnDisable()
		{
			this.Unsubscribe();
			this.BeginExit();
			this.RemoveOwnedOffset();
		}

		// Token: 0x06006C47 RID: 27719 RVA: 0x002B4FB5 File Offset: 0x002B31B5
		private void OnDestroy()
		{
			this.Unsubscribe();
			this.RemoveOwnedOffset();
		}

		// Token: 0x04006EAB RID: 28331
		private const string CrouchId = "crouch_walk";

		// Token: 0x04006EAC RID: 28332
		private const string CrawlId = "crawl_walk";

		// Token: 0x04006EAD RID: 28333
		private const string ProneId = "prone_walk";

		// Token: 0x04006EAE RID: 28334
		[Header("References")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04006EAF RID: 28335
		[SerializeField]
		private vThirdPersonCamera playerCamera;

		// Token: 0x04006EB0 RID: 28336
		[Header("Posture Emotes")]
		[SerializeField]
		private EmoteDefinitionSO crouchEmote;

		// Token: 0x04006EB1 RID: 28337
		[SerializeField]
		private EmoteDefinitionSO crawlEmote;

		// Token: 0x04006EB2 RID: 28338
		[SerializeField]
		private EmoteDefinitionSO proneEmote;

		// Token: 0x04006EB3 RID: 28339
		[Header("Additive Pivot Height")]
		[Tooltip("Metres added to the active camera pivot while crouching.")]
		[SerializeField]
		private float crouchOffset = -0.6f;

		// Token: 0x04006EB4 RID: 28340
		[Tooltip("Metres added to the active camera pivot while crawling.")]
		[SerializeField]
		private float crawlOffset = -0.9f;

		// Token: 0x04006EB5 RID: 28341
		[Tooltip("Metres added to the active camera pivot while prone.")]
		[SerializeField]
		private float proneOffset = -1f;

		// Token: 0x04006EB6 RID: 28342
		[Tooltip("Seconds used to settle into a posture camera height.")]
		[Min(0f)]
		[SerializeField]
		private float enterSmoothTime = 0.16f;

		// Token: 0x04006EB7 RID: 28343
		[Tooltip("Seconds used to return to the previous camera height.")]
		[Min(0f)]
		[SerializeField]
		private float exitSmoothTime = 0.2f;

		// Token: 0x04006EB8 RID: 28344
		private GameplayEmoteController _subscribedController;

		// Token: 0x04006EB9 RID: 28345
		private EmoteDefinitionSO _activePosture;

		// Token: 0x04006EBA RID: 28346
		private vThirdPersonCamera _offsetCamera;

		// Token: 0x04006EBB RID: 28347
		private float _targetOffset;

		// Token: 0x04006EBC RID: 28348
		private float _appliedOffset;

		// Token: 0x04006EBD RID: 28349
		private float _offsetVelocity;
	}
}
