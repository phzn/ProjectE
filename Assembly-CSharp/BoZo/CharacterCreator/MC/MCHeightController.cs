using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007CB RID: 1995
	public class MCHeightController : MonoBehaviour
	{
		// Token: 0x06003518 RID: 13592 RVA: 0x0012E900 File Offset: 0x0012CB00
		private void Start()
		{
			this.currentScale = this.defaultScale;
			if (this.heightSlider != null)
			{
				this.heightSlider.minValue = this.minScale;
				this.heightSlider.maxValue = this.maxScale;
				this.heightSlider.value = this.defaultScale;
				this.heightSlider.onValueChanged.RemoveAllListeners();
				this.heightSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnHeightChanged));
			}
			if (this.resetButton != null)
			{
				this.resetButton.onClick.RemoveAllListeners();
				this.resetButton.onClick.AddListener(new UnityAction(this.ResetToDefault));
			}
			this.ApplyScale(this.currentScale);
		}

		// Token: 0x06003519 RID: 13593 RVA: 0x0012E9CC File Offset: 0x0012CBCC
		private void OnHeightChanged(float value)
		{
			this.currentScale = value;
			this.ApplyScale(this.currentScale);
			this.UpdateLabel();
		}

		// Token: 0x0600351A RID: 13594 RVA: 0x0012E9E7 File Offset: 0x0012CBE7
		private void ApplyScale(float scale)
		{
			if (this.modelTransform == null)
			{
				return;
			}
			this.modelTransform.localScale = new Vector3(scale, scale, scale);
		}

		// Token: 0x0600351B RID: 13595 RVA: 0x0012EA0B File Offset: 0x0012CC0B
		private void UpdateLabel()
		{
			if (this.valueLabel != null)
			{
				LocalizedGameText.BindFormat(this.valueLabel, "cielos_apps", "{0:F2}x", new object[] { this.currentScale });
			}
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x0012EA45 File Offset: 0x0012CC45
		public void ResetToDefault()
		{
			this.currentScale = this.defaultScale;
			if (this.heightSlider != null)
			{
				this.heightSlider.value = this.defaultScale;
			}
			this.ApplyScale(this.currentScale);
			this.UpdateLabel();
		}

		// Token: 0x0600351D RID: 13597 RVA: 0x0012EA84 File Offset: 0x0012CC84
		public float GetCurrentScale()
		{
			return this.currentScale;
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x0012EA8C File Offset: 0x0012CC8C
		public void SetScale(float scale)
		{
			this.currentScale = Mathf.Clamp(scale, this.minScale, this.maxScale);
			if (this.heightSlider != null)
			{
				this.heightSlider.value = this.currentScale;
			}
			this.ApplyScale(this.currentScale);
			this.UpdateLabel();
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x0012EAE2 File Offset: 0x0012CCE2
		public void SetModelTransform(Transform target)
		{
			this.modelTransform = target;
			this.ApplyScale(this.currentScale);
		}

		// Token: 0x0400354D RID: 13645
		[Header("Target")]
		[Tooltip("The transform to scale (usually the '3D Model' child of the character root).")]
		[SerializeField]
		private Transform modelTransform;

		// Token: 0x0400354E RID: 13646
		[Header("Height Range")]
		[SerializeField]
		private float minScale = 0.9f;

		// Token: 0x0400354F RID: 13647
		[SerializeField]
		private float maxScale = 1.1f;

		// Token: 0x04003550 RID: 13648
		[SerializeField]
		private float defaultScale = 1f;

		// Token: 0x04003551 RID: 13649
		[Header("UI")]
		[Tooltip("Slider that controls height. Min/Max will be overwritten at runtime.")]
		[SerializeField]
		private Slider heightSlider;

		// Token: 0x04003552 RID: 13650
		[Tooltip("Optional label showing the current value.")]
		[SerializeField]
		private TextMeshProUGUI valueLabel;

		// Token: 0x04003553 RID: 13651
		[Tooltip("Optional reset button to restore default height.")]
		[SerializeField]
		private Button resetButton;

		// Token: 0x04003554 RID: 13652
		private float currentScale;
	}
}
