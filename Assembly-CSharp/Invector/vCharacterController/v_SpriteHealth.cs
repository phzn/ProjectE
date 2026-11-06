using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vCharacterController
{
	// Token: 0x020006A8 RID: 1704
	[vClassHeader("SpriteHealth", "Assign your canvas object in the 'healthBar' field to hide and only display when receive damage - leave it empty if you want to display the healthbar all the time.", openClose = false)]
	public class v_SpriteHealth : vMonoBehaviour
	{
		// Token: 0x06002979 RID: 10617 RVA: 0x000E9720 File Offset: 0x000E7920
		private void Start()
		{
			this.cameraMain = (Camera.main ? Camera.main.transform : null);
			this.healthControl = base.transform.GetComponentInParent<vHealthController>();
			if (this.healthControl == null)
			{
				Debug.LogWarning("The character must have a ICharacter Interface");
				Object.Destroy(base.gameObject);
			}
			this.healthControl.onReceiveDamage.AddListener(new UnityAction<vDamage>(this.Damage));
			this._healthSlider.maxValue = (float)this.healthControl.maxHealth;
			this._healthSlider.value = this._healthSlider.maxValue;
			this._damageDelay.maxValue = (float)this.healthControl.maxHealth;
			this._damageDelay.value = this._healthSlider.maxValue;
			this._damageCounter.text = string.Empty;
			if (this.healthBar)
			{
				this.healthBar.SetActive(false);
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x000E9820 File Offset: 0x000E7A20
		private void SpriteBehaviour()
		{
			if (this.lookToCamera && this.cameraMain != null)
			{
				base.transform.LookAt(this.cameraMain.position, Vector3.up);
			}
			if (this.healthControl == null || this.healthControl.currentHealth <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
			this._healthSlider.value = this.healthControl.currentHealth;
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000E989F File Offset: 0x000E7A9F
		private void Update()
		{
			if (!this.healthBar)
			{
				this.SpriteBehaviour();
			}
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000E98B4 File Offset: 0x000E7AB4
		public void Damage(vDamage damage)
		{
			try
			{
				this.damage += damage.damageValue;
				this._damageCounter.text = this.damage.ToString("00") + ((this._showDamageType && !string.IsNullOrEmpty(damage.damageType)) ? (" : by " + damage.damageType) : "");
				this._healthSlider.value -= damage.damageValue;
				if (!this.inDelay && this.healthControl && this.healthControl.gameObject.activeInHierarchy)
				{
					base.StartCoroutine(this.DamageDelay());
				}
			}
			catch
			{
				Object.Destroy(this);
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000E9988 File Offset: 0x000E7B88
		private IEnumerator DamageDelay()
		{
			this.inDelay = true;
			if (this.healthBar)
			{
				this.SpriteBehaviour();
			}
			if (this.healthBar)
			{
				this.healthBar.SetActive(true);
			}
			while (this._damageDelay.value > this._healthSlider.value)
			{
				if (this.healthBar)
				{
					this.SpriteBehaviour();
				}
				this._damageDelay.value -= this._smoothDamageDelay;
				yield return null;
			}
			this.inDelay = false;
			yield return new WaitForSeconds(this._damageCounterTimer);
			this.damage = 0f;
			this._damageCounter.text = string.Empty;
			if (this.healthBar)
			{
				this.healthBar.SetActive(false);
			}
			yield break;
		}

		// Token: 0x04002997 RID: 10647
		[Tooltip("UI to show on receive damage")]
		[SerializeField]
		protected GameObject healthBar;

		// Token: 0x04002998 RID: 10648
		public bool lookToCamera = true;

		// Token: 0x04002999 RID: 10649
		[Header("UI properties")]
		[SerializeField]
		protected Slider _healthSlider;

		// Token: 0x0400299A RID: 10650
		[SerializeField]
		protected Slider _damageDelay;

		// Token: 0x0400299B RID: 10651
		[SerializeField]
		protected float _smoothDamageDelay;

		// Token: 0x0400299C RID: 10652
		[SerializeField]
		protected Text _damageCounter;

		// Token: 0x0400299D RID: 10653
		[SerializeField]
		protected float _damageCounterTimer = 1.5f;

		// Token: 0x0400299E RID: 10654
		[SerializeField]
		protected bool _showDamageType = true;

		// Token: 0x0400299F RID: 10655
		private vHealthController healthControl;

		// Token: 0x040029A0 RID: 10656
		private bool inDelay;

		// Token: 0x040029A1 RID: 10657
		private float damage;

		// Token: 0x040029A2 RID: 10658
		private float currentSmoothDamage;

		// Token: 0x040029A3 RID: 10659
		internal Transform cameraMain;
	}
}
