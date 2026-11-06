using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vCharacterController
{
	// Token: 0x0200068E RID: 1678
	public class vHUDController : MonoBehaviour
	{
		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06002777 RID: 10103 RVA: 0x000E1372 File Offset: 0x000DF572
		public static vHUDController instance
		{
			get
			{
				if (vHUDController._instance == null)
				{
					vHUDController._instance = Object.FindObjectOfType<vHUDController>();
				}
				return vHUDController._instance;
			}
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000E1390 File Offset: 0x000DF590
		private void Start()
		{
			this.InitFadeText();
			if (this.debugPanel != null)
			{
				this.debugText = this.debugPanel.GetComponentInChildren<Text>();
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000E13B8 File Offset: 0x000DF5B8
		public void Init(vThirdPersonController cc)
		{
			cc.onDead.AddListener(new UnityAction<GameObject>(this.OnDead));
			cc.onReceiveDamage.AddListener(new UnityAction<vDamage>(this.EnableDamageSprite));
			this.damageImage.color = new Color(0f, 0f, 0f, 0f);
			if (this.healthSlider)
			{
				if ((float)cc.maxHealth != this.healthSlider.maxValue)
				{
					this.healthSlider.maxValue = (float)cc.maxHealth;
					this.healthSlider.onValueChanged.Invoke(this.healthSlider.value);
				}
				this.healthSlider.value = cc.currentHealth;
			}
			if (this.staminaSlider)
			{
				if (cc.maxStamina != this.staminaSlider.maxValue)
				{
					this.staminaSlider.maxValue = cc.maxStamina;
					this.staminaSlider.onValueChanged.Invoke(this.staminaSlider.value);
				}
				this.staminaSlider.value = cc.currentStamina;
			}
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000E14D3 File Offset: 0x000DF6D3
		private void OnDead(GameObject arg0)
		{
			this.ShowText("You are Dead!");
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000E14E0 File Offset: 0x000DF6E0
		public virtual void UpdateHUD(vThirdPersonController cc)
		{
			this.UpdateDebugWindow(cc);
			this.UpdateSliders(cc);
			this.ChangeInputDisplay();
			this.ShowDamageSprite();
			this.FadeEffect();
		}

		// Token: 0x0600277C RID: 10108 RVA: 0x000E1504 File Offset: 0x000DF704
		public void ShowText(string message, float textTime = 2f, float fadeTime = 0.5f)
		{
			if (this.fadeText != null && !this.fade)
			{
				this.fadeText.text = message;
				this.textDuration = textTime;
				this.fadeDuration = fadeTime;
				this.durationTimer = 0f;
				this.timer = 0f;
				this.fade = true;
				return;
			}
			if (this.fadeText != null)
			{
				Text text = this.fadeText;
				text.text = text.text + "\n" + message;
				this.textDuration = textTime;
				this.fadeDuration = fadeTime;
				this.durationTimer = 0f;
				this.timer = 0f;
			}
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000E15B0 File Offset: 0x000DF7B0
		public void ShowText(string message)
		{
			if (this.fadeText != null && !this.fade)
			{
				this.fadeText.text = message;
				this.textDuration = 2f;
				this.fadeDuration = 0.5f;
				this.durationTimer = 0f;
				this.timer = 0f;
				this.fade = true;
				return;
			}
			if (this.fadeText != null)
			{
				Text text = this.fadeText;
				text.text = text.text + "\n" + message;
				this.textDuration = 2f;
				this.fadeDuration = 0.5f;
				this.durationTimer = 0f;
				this.timer = 0f;
			}
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000E166C File Offset: 0x000DF86C
		private void UpdateSliders(vThirdPersonController cc)
		{
			if (this.healthSlider != null)
			{
				if ((float)cc.maxHealth != this.healthSlider.maxValue)
				{
					this.healthSlider.maxValue = Mathf.Lerp(this.healthSlider.maxValue, (float)cc.maxHealth, this.healthSliderMaxValueSmooth * Time.fixedDeltaTime);
					this.healthSlider.onValueChanged.Invoke(this.healthSlider.value);
				}
				this.healthSlider.value = Mathf.Lerp(this.healthSlider.value, cc.currentHealth, this.healthSliderValueSmooth * Time.fixedDeltaTime);
			}
			if (this.staminaSlider)
			{
				if (cc.maxStamina != this.staminaSlider.maxValue)
				{
					this.staminaSlider.maxValue = Mathf.Lerp(this.staminaSlider.maxValue, cc.maxStamina, this.staminaSliderMaxValueSmooth * Time.fixedDeltaTime);
					this.staminaSlider.onValueChanged.Invoke(this.staminaSlider.value);
				}
				this.staminaSlider.value = Mathf.Lerp(this.staminaSlider.value, cc.currentStamina, this.staminaSliderValueSmooth * Time.fixedDeltaTime);
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000E17AC File Offset: 0x000DF9AC
		public void ShowDamageSprite()
		{
			if (this.damaged)
			{
				this.damaged = false;
				if (this.damageImage != null)
				{
					this.damageImage.color = this.flashColour;
					return;
				}
			}
			else if (this.damageImage != null)
			{
				this.damageImage.color = Color.Lerp(this.damageImage.color, Color.clear, this.flashSpeed * Time.deltaTime);
			}
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000E1822 File Offset: 0x000DFA22
		public void EnableDamageSprite(vDamage damage)
		{
			if (this.damageImage != null)
			{
				this.damageImage.enabled = true;
			}
			this.damaged = true;
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000E1848 File Offset: 0x000DFA48
		private void UpdateDebugWindow(vThirdPersonController cc)
		{
			if (cc.debugWindow)
			{
				if (this.debugPanel != null && !this.debugPanel.activeSelf)
				{
					this.debugPanel.SetActive(true);
				}
				if (this.debugText)
				{
					this.debugText.text = cc.DebugInfo("");
					return;
				}
			}
			else if (this.debugPanel != null && this.debugPanel.activeSelf)
			{
				this.debugPanel.SetActive(false);
			}
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000E18D0 File Offset: 0x000DFAD0
		private void ChangeInputDisplay()
		{
			if (this.displayControls == null)
			{
				return;
			}
			if (this.controllerInput && vInput.instance.inputDevice == InputDevice.Joystick)
			{
				this.displayControls.sprite = this.joystickControls;
				return;
			}
			this.displayControls.sprite = this.keyboardControls;
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000E1924 File Offset: 0x000DFB24
		private void InitFadeText()
		{
			if (this.fadeText != null)
			{
				this.fadeText.verticalOverflow = VerticalWrapMode.Overflow;
				this.startColor = this.fadeText.color;
				this.endColor.a = 0f;
				this.fadeText.color = this.endColor;
				return;
			}
			Debug.Log("Please assign a Text object on the field Fade Text");
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000E1988 File Offset: 0x000DFB88
		private void FadeEffect()
		{
			if (this.fadeText != null)
			{
				if (this.fade)
				{
					this.fadeText.color = Color.Lerp(this.endColor, this.startColor, this.timer);
					if (this.timer < 1f)
					{
						this.timer += Time.deltaTime / this.fadeDuration;
					}
					if (this.fadeText.color.a >= 1f)
					{
						this.fade = false;
						this.timer = 0f;
						return;
					}
				}
				else
				{
					if (this.fadeText.color.a >= 1f)
					{
						this.durationTimer += Time.deltaTime;
					}
					if (this.durationTimer >= this.textDuration)
					{
						this.fadeText.color = Color.Lerp(this.startColor, this.endColor, this.timer);
						if (this.timer < 1f)
						{
							this.timer += Time.deltaTime / this.fadeDuration;
						}
					}
				}
			}
		}

		// Token: 0x0400280C RID: 10252
		[Header("Health/Stamina")]
		public Slider healthSlider;

		// Token: 0x0400280D RID: 10253
		public float healthSliderMaxValueSmooth = 10f;

		// Token: 0x0400280E RID: 10254
		public float healthSliderValueSmooth = 10f;

		// Token: 0x0400280F RID: 10255
		public Slider staminaSlider;

		// Token: 0x04002810 RID: 10256
		public float staminaSliderMaxValueSmooth = 10f;

		// Token: 0x04002811 RID: 10257
		public float staminaSliderValueSmooth = 100f;

		// Token: 0x04002812 RID: 10258
		[Header("DamageHUD")]
		public Image damageImage;

		// Token: 0x04002813 RID: 10259
		public float flashSpeed = 5f;

		// Token: 0x04002814 RID: 10260
		public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

		// Token: 0x04002815 RID: 10261
		[HideInInspector]
		public bool damaged;

		// Token: 0x04002816 RID: 10262
		[Header("Controls Display")]
		[HideInInspector]
		public bool controllerInput;

		// Token: 0x04002817 RID: 10263
		public Image displayControls;

		// Token: 0x04002818 RID: 10264
		public Sprite joystickControls;

		// Token: 0x04002819 RID: 10265
		public Sprite keyboardControls;

		// Token: 0x0400281A RID: 10266
		[Header("Debug Window")]
		public GameObject debugPanel;

		// Token: 0x0400281B RID: 10267
		[HideInInspector]
		public Text debugText;

		// Token: 0x0400281C RID: 10268
		[Header("Text with FadeIn/Out")]
		public Text fadeText;

		// Token: 0x0400281D RID: 10269
		private float textDuration;

		// Token: 0x0400281E RID: 10270
		private float fadeDuration;

		// Token: 0x0400281F RID: 10271
		private float durationTimer;

		// Token: 0x04002820 RID: 10272
		private float timer;

		// Token: 0x04002821 RID: 10273
		private Color startColor;

		// Token: 0x04002822 RID: 10274
		private Color endColor;

		// Token: 0x04002823 RID: 10275
		private bool fade;

		// Token: 0x04002824 RID: 10276
		private static vHUDController _instance;
	}
}
