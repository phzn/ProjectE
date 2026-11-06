using System;
using Features.Detection;
using Features.Flash;
using UnityEngine;
using UnityEngine.Events;

namespace Features.UniversityUniform
{
	// Token: 0x02000BBD RID: 3005
	public sealed class UniversityUniformFlashFaceController : MonoBehaviour
	{
		// Token: 0x06005629 RID: 22057 RVA: 0x00217B3C File Offset: 0x00215D3C
		private void Awake()
		{
			if (this.flashController == null)
			{
				this.flashController = base.GetComponent<ArmsFlashController>();
			}
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x00217B58 File Offset: 0x00215D58
		private void OnEnable()
		{
			if (this.flashController != null)
			{
				this.flashController.OnFlashStateChanged.AddListener(new UnityAction<bool>(this.HandleFlashStateChanged));
			}
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x00217B84 File Offset: 0x00215D84
		private void OnDisable()
		{
			if (this.flashController != null)
			{
				this.flashController.OnFlashStateChanged.RemoveListener(new UnityAction<bool>(this.HandleFlashStateChanged));
			}
			this.Release();
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x00217BB8 File Offset: 0x00215DB8
		private void Update()
		{
			if (!(this.flashController != null) || (!this.flashController.IsOpen && !this.flashController.IsBusy))
			{
				this.Release();
				return;
			}
			if (Time.time < this._nextReassert)
			{
				return;
			}
			this._nextReassert = Time.time + Mathf.Max(0.1f, this.reassertInterval);
			this.Apply();
		}

		// Token: 0x0600562D RID: 22061 RVA: 0x00217C2A File Offset: 0x00215E2A
		private void HandleFlashStateChanged(bool open)
		{
			if (open)
			{
				this.Apply();
			}
		}

		// Token: 0x0600562E RID: 22062 RVA: 0x00217C38 File Offset: 0x00215E38
		private void Apply()
		{
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			if (!this._holding || !instance.IsFacialExpressionOverriddenForAction)
			{
				instance.DisableFacialExpressionForAction();
				this._holding = true;
			}
			instance.SetExpression(this.expressionIndex);
		}

		// Token: 0x0600562F RID: 22063 RVA: 0x00217C7E File Offset: 0x00215E7E
		private void Release()
		{
			if (!this._holding)
			{
				return;
			}
			this._holding = false;
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.RestoreFacialExpression();
		}

		// Token: 0x040056A9 RID: 22185
		[SerializeField]
		private ArmsFlashController flashController;

		// Token: 0x040056AA RID: 22186
		[Tooltip("3 = HornySupreme in Ciel's FaceExpressionIndex controller.")]
		[SerializeField]
		private int expressionIndex = 3;

		// Token: 0x040056AB RID: 22187
		[SerializeField]
		private float reassertInterval = 0.25f;

		// Token: 0x040056AC RID: 22188
		private bool _holding;

		// Token: 0x040056AD RID: 22189
		private float _nextReassert;
	}
}
