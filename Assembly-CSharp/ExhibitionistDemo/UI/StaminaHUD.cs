using System;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.UI;

namespace ExhibitionistDemo.UI
{
	// Token: 0x02000791 RID: 1937
	public class StaminaHUD : MonoBehaviour
	{
		// Token: 0x060032F2 RID: 13042 RVA: 0x00121380 File Offset: 0x0011F580
		private void Start()
		{
			if (this.playerController == null)
			{
				this.playerController = Object.FindObjectOfType<vThirdPersonController>();
				if (this.playerController == null)
				{
					Debug.LogError("StaminaHUD: No vThirdPersonController found in scene!");
					return;
				}
			}
			if (this.staminaSlider == null)
			{
				this.staminaSlider = base.GetComponent<Slider>();
				if (this.staminaSlider == null)
				{
					Debug.LogError("StaminaHUD: No Slider component found!");
					return;
				}
			}
			if (this.useColorGradient && this.sliderFillImage == null)
			{
				RectTransform fillRect = this.staminaSlider.fillRect;
				this.sliderFillImage = ((fillRect != null) ? fillRect.GetComponent<Image>() : null);
				if (this.sliderFillImage == null)
				{
					Debug.LogWarning("StaminaHUD: Could not find slider fill image for color gradient!");
				}
			}
			this.InitializeSlider();
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x00121444 File Offset: 0x0011F644
		private void Update()
		{
			if (this.updateEveryFrame)
			{
				this.UpdateStaminaDisplay();
				return;
			}
			this.updateTimer += Time.deltaTime;
			if (this.updateTimer >= this.updateInterval)
			{
				this.updateTimer = 0f;
				this.UpdateStaminaDisplay();
			}
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x00121494 File Offset: 0x0011F694
		private void InitializeSlider()
		{
			if (this.playerController == null || this.staminaSlider == null)
			{
				Debug.LogError(string.Format("StaminaHUD: Cannot initialize - PlayerController: {0}, Slider: {1}", this.playerController != null, this.staminaSlider != null));
				return;
			}
			this.staminaSlider.maxValue = this.playerController.maxStamina;
			this.staminaSlider.value = this.playerController.currentStamina;
			Debug.Log(string.Format("StaminaHUD: Initialized - Max: {0}, Current: {1}, Slider Max: {2}, Slider Value: {3}", new object[]
			{
				this.playerController.maxStamina,
				this.playerController.currentStamina,
				this.staminaSlider.maxValue,
				this.staminaSlider.value
			}));
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0012157C File Offset: 0x0011F77C
		private void UpdateStaminaDisplay()
		{
			if (this.playerController == null || this.staminaSlider == null)
			{
				return;
			}
			if (this.staminaSlider.maxValue != this.playerController.maxStamina)
			{
				this.staminaSlider.maxValue = this.playerController.maxStamina;
			}
			float currentStamina = this.playerController.currentStamina;
			if (this.smoothTime > 0f)
			{
				this.staminaSlider.value = Mathf.SmoothDamp(this.staminaSlider.value, currentStamina, ref this.currentVelocity, this.smoothTime);
			}
			else
			{
				this.staminaSlider.value = currentStamina;
			}
			if (this.useColorGradient && this.sliderFillImage != null)
			{
				float num = this.playerController.currentStamina / this.playerController.maxStamina;
				this.sliderFillImage.color = Color.Lerp(this.emptyStaminaColor, this.fullStaminaColor, num);
			}
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x0012166E File Offset: 0x0011F86E
		public void ForceUpdate()
		{
			this.UpdateStaminaDisplay();
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x00121676 File Offset: 0x0011F876
		public void SetPlayerController(vThirdPersonController controller)
		{
			this.playerController = controller;
			this.InitializeSlider();
		}

		// Token: 0x04003395 RID: 13205
		[Header("References")]
		[Tooltip("The Invector Third Person Controller component")]
		public vThirdPersonController playerController;

		// Token: 0x04003396 RID: 13206
		[Tooltip("The UI Slider component for stamina bar")]
		public Slider staminaSlider;

		// Token: 0x04003397 RID: 13207
		[Header("Settings")]
		[Tooltip("Smooth time for slider value changes (lower = faster, 0 = instant)")]
		public float smoothTime = 0.15f;

		// Token: 0x04003398 RID: 13208
		[Tooltip("Update every frame or use fixed interval")]
		public bool updateEveryFrame = true;

		// Token: 0x04003399 RID: 13209
		[Tooltip("Update interval in seconds (if not updating every frame)")]
		public float updateInterval = 0.1f;

		// Token: 0x0400339A RID: 13210
		[Header("Optional: Color Based on Stamina")]
		[Tooltip("Enable color change based on stamina level")]
		public bool useColorGradient;

		// Token: 0x0400339B RID: 13211
		[Tooltip("Color when stamina is full")]
		public Color fullStaminaColor = Color.green;

		// Token: 0x0400339C RID: 13212
		[Tooltip("Color when stamina is empty")]
		public Color emptyStaminaColor = Color.red;

		// Token: 0x0400339D RID: 13213
		[Tooltip("Apply color to slider fill (if null, tries to find it automatically)")]
		public Image sliderFillImage;

		// Token: 0x0400339E RID: 13214
		private float updateTimer;

		// Token: 0x0400339F RID: 13215
		private float currentVelocity;
	}
}
