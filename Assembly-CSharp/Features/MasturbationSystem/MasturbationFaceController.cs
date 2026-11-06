using System;
using Features.Detection;
using Features.PlayerEmotes;
using UnityEngine;

namespace Features.MasturbationSystem
{
	// Token: 0x02000FF1 RID: 4081
	[DisallowMultipleComponent]
	public class MasturbationFaceController : MonoBehaviour
	{
		// Token: 0x06007BAB RID: 31659 RVA: 0x00328364 File Offset: 0x00326564
		private void Awake()
		{
			if (this.emoteController == null)
			{
				this.emoteController = base.GetComponent<GameplayEmoteController>();
			}
			if (this.emotionController == null)
			{
				this.emotionController = base.GetComponent<PlayerEmotionController>();
			}
			if (this.emotionController == null)
			{
				this.emotionController = base.GetComponentInChildren<PlayerEmotionController>();
			}
		}

		// Token: 0x06007BAC RID: 31660 RVA: 0x003283BF File Offset: 0x003265BF
		private void OnEnable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted += this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded += this.HandleEmoteEnded;
			}
		}

		// Token: 0x06007BAD RID: 31661 RVA: 0x00328400 File Offset: 0x00326600
		private void OnDisable()
		{
			if (this.emoteController != null)
			{
				this.emoteController.OnEmoteStarted -= this.HandleEmoteStarted;
				this.emoteController.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this.RestoreFaceIfOverridden();
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x00328450 File Offset: 0x00326650
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			if (this.emotionController == null)
			{
				return;
			}
			this._savedFaceIndex = this.ReadCurrentFaceIndex();
			this._faceWasOverridden = true;
			this.emotionController.DisableFacialExpressionForAction();
			this.emotionController.SetExpression(this.orgasmFaceIndex);
			if (this.debugMode)
			{
				Debug.Log(string.Format("[MasturbationFace] Saved face={0}, disabled emotion polling, set OrgasmFace={1}", this._savedFaceIndex, this.orgasmFaceIndex));
			}
		}

		// Token: 0x06007BAF RID: 31663 RVA: 0x003284DA File Offset: 0x003266DA
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (emote == null || !emote.IsMasturbationEmote)
			{
				return;
			}
			this.RestoreFaceIfOverridden();
		}

		// Token: 0x06007BB0 RID: 31664 RVA: 0x003284F4 File Offset: 0x003266F4
		private void RestoreFaceIfOverridden()
		{
			if (!this._faceWasOverridden)
			{
				return;
			}
			if (this.emotionController == null)
			{
				this._faceWasOverridden = false;
				return;
			}
			if (this.restorePreviousFaceOnEnd)
			{
				this.emotionController.SetExpression(this._savedFaceIndex);
				if (this.debugMode)
				{
					Debug.Log(string.Format("[MasturbationFace] Restored face={0}", this._savedFaceIndex));
				}
			}
			else
			{
				this.emotionController.ResetToNeutral();
				if (this.debugMode)
				{
					Debug.Log("[MasturbationFace] Reset to neutral");
				}
			}
			this.emotionController.RestoreFacialExpression();
			this._faceWasOverridden = false;
		}

		// Token: 0x06007BB1 RID: 31665 RVA: 0x0032858C File Offset: 0x0032678C
		private int ReadCurrentFaceIndex()
		{
			if (this.emotionController == null || this.emotionController.PlayerAnimator == null)
			{
				return 0;
			}
			string text = (string.IsNullOrEmpty(this.emotionController.ExpressionParamName) ? "FaceExpressionIndex" : this.emotionController.ExpressionParamName);
			return this.emotionController.PlayerAnimator.GetInteger(text);
		}

		// Token: 0x04007F3E RID: 32574
		[Header("References")]
		[SerializeField]
		private GameplayEmoteController emoteController;

		// Token: 0x04007F3F RID: 32575
		[SerializeField]
		private PlayerEmotionController emotionController;

		// Token: 0x04007F40 RID: 32576
		[Header("Face")]
		[Tooltip("Index della OrgasmFace nell'enum delle espressioni del PlayerEmotionController. Default 5.")]
		[SerializeField]
		private int orgasmFaceIndex = 5;

		// Token: 0x04007F41 RID: 32577
		[Tooltip("Se true, il valore della face salvato all'inizio viene ripristinato a fine emote. Se false, la face viene riportata al neutral index del PlayerEmotionController.")]
		[SerializeField]
		private bool restorePreviousFaceOnEnd = true;

		// Token: 0x04007F42 RID: 32578
		[Header("Debug")]
		[SerializeField]
		private bool debugMode;

		// Token: 0x04007F43 RID: 32579
		private int _savedFaceIndex;

		// Token: 0x04007F44 RID: 32580
		private bool _faceWasOverridden;
	}
}
