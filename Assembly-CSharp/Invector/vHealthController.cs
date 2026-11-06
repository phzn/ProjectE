using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200047A RID: 1146
	[vClassHeader("HealthController", true, "icon_v2", false, "", iconName = "HealthControllerIcon")]
	public class vHealthController : vMonoBehaviour, vIHealthController, vIDamageReceiver
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001C23 RID: 7203 RVA: 0x000B0187 File Offset: 0x000AE387
		// (set) Token: 0x06001C24 RID: 7204 RVA: 0x000B018F File Offset: 0x000AE38F
		public virtual int maxHealth
		{
			get
			{
				return this._maxHealth;
			}
			set
			{
				this._maxHealth = value;
			}
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x000B0198 File Offset: 0x000AE398
		protected virtual void OnValidate()
		{
			if (this.currentHealth <= 0f && !this.isDead)
			{
				this.isDead = true;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x000B01B6 File Offset: 0x000AE3B6
		// (set) Token: 0x06001C27 RID: 7207 RVA: 0x000B01BE File Offset: 0x000AE3BE
		public virtual int MaxHealth
		{
			get
			{
				return this.maxHealth;
			}
			protected set
			{
				this.maxHealth = value;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x000B01C7 File Offset: 0x000AE3C7
		// (set) Token: 0x06001C29 RID: 7209 RVA: 0x000B01D0 File Offset: 0x000AE3D0
		public virtual float currentHealth
		{
			get
			{
				return this._currentHealth;
			}
			protected set
			{
				if (this._currentHealth != value)
				{
					this._currentHealth = value;
					this._currentHealth = Mathf.Clamp(this._currentHealth, 0f, (float)this.MaxHealth);
					this.onChangeHealth.Invoke(this._currentHealth);
					this.HandleCheckHealthEvents();
				}
				bool flag = this._currentHealth <= 0f;
				if (flag != this.isDead)
				{
					this.isDead = flag;
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x000B0242 File Offset: 0x000AE442
		// (set) Token: 0x06001C2B RID: 7211 RVA: 0x000B024A File Offset: 0x000AE44A
		public virtual bool isDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._isDead = value;
					if (value)
					{
						this.onDead.Invoke(base.gameObject);
					}
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000B0270 File Offset: 0x000AE470
		// (set) Token: 0x06001C2D RID: 7213 RVA: 0x000B0278 File Offset: 0x000AE478
		public virtual float healthRecovery
		{
			get
			{
				return this._healthRecovery;
			}
			set
			{
				this._healthRecovery = value;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x000B0281 File Offset: 0x000AE481
		// (set) Token: 0x06001C2F RID: 7215 RVA: 0x000B0289 File Offset: 0x000AE489
		public virtual float healthRecoveryDelay
		{
			get
			{
				return this._healthRecoveryDelay;
			}
			set
			{
				this._healthRecoveryDelay = value;
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x000B0292 File Offset: 0x000AE492
		// (set) Token: 0x06001C31 RID: 7217 RVA: 0x000B029A File Offset: 0x000AE49A
		public virtual float currentHealthRecoveryDelay
		{
			get
			{
				return this._currentHealthRecoveryDelay;
			}
			set
			{
				this._currentHealthRecoveryDelay = value;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x000B02A3 File Offset: 0x000AE4A3
		// (set) Token: 0x06001C33 RID: 7219 RVA: 0x000B02AB File Offset: 0x000AE4AB
		public OnReceiveDamage onStartReceiveDamage
		{
			get
			{
				return this._onStartReceiveDamage;
			}
			protected set
			{
				this._onStartReceiveDamage = value;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000B02B4 File Offset: 0x000AE4B4
		// (set) Token: 0x06001C35 RID: 7221 RVA: 0x000B02BC File Offset: 0x000AE4BC
		public OnReceiveDamage onReceiveDamage
		{
			get
			{
				return this._onReceiveDamage;
			}
			protected set
			{
				this._onReceiveDamage = value;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x000B02C5 File Offset: 0x000AE4C5
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x000B02CD File Offset: 0x000AE4CD
		public OnDead onDead
		{
			get
			{
				return this._onDead;
			}
			protected set
			{
				this._onDead = value;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x000B02D6 File Offset: 0x000AE4D6
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x000B02DE File Offset: 0x000AE4DE
		public virtual bool inHealthRecovery { get; set; }

		// Token: 0x06001C3A RID: 7226 RVA: 0x000B02E7 File Offset: 0x000AE4E7
		protected virtual void Start()
		{
			if (this.fillHealthOnStart)
			{
				this.currentHealth = (float)this.maxHealth;
			}
			this.currentHealthRecoveryDelay = this.healthRecoveryDelay;
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x000B030A File Offset: 0x000AE50A
		protected virtual bool canRecoverHealth
		{
			get
			{
				return this.currentHealth >= 0f && this.healthRecovery > 0f && this.currentHealth < (float)this.maxHealth;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x000B0337 File Offset: 0x000AE537
		protected virtual IEnumerator RecoverHealth()
		{
			this.inHealthRecovery = true;
			while (this.canRecoverHealth && !this.isDead)
			{
				this.HealthRecovery();
				yield return null;
			}
			this.inHealthRecovery = false;
			yield break;
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000B0348 File Offset: 0x000AE548
		protected virtual void HealthRecovery()
		{
			if (!this.canRecoverHealth || this.isDead)
			{
				return;
			}
			if (this.currentHealthRecoveryDelay > 0f)
			{
				this.currentHealthRecoveryDelay -= Time.deltaTime;
				return;
			}
			if (this.currentHealth > (float)this.maxHealth)
			{
				this.currentHealth = (float)this.maxHealth;
			}
			if (this.currentHealth < (float)this.maxHealth)
			{
				this.currentHealth += this.healthRecovery * Time.deltaTime;
			}
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000B03CA File Offset: 0x000AE5CA
		public virtual void AddHealth(int value)
		{
			this.currentHealth += (float)value;
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000B03DB File Offset: 0x000AE5DB
		public virtual void ChangeHealth(int value)
		{
			this.currentHealth = (float)value;
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x000B03E5 File Offset: 0x000AE5E5
		public virtual void ResetHealth(float health)
		{
			this.currentHealth = health;
			this.onResetHealth.Invoke();
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x000B03F9 File Offset: 0x000AE5F9
		public virtual void ResetHealth()
		{
			this.currentHealth = (float)this.maxHealth;
			this.onResetHealth.Invoke();
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000B0413 File Offset: 0x000AE613
		public virtual void ChangeMaxHealth(int value)
		{
			this.maxHealth += value;
			if (this.maxHealth < 0)
			{
				this.maxHealth = 0;
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000B0433 File Offset: 0x000AE633
		public virtual void SetHealthRecovery(float value)
		{
			this.healthRecovery = value;
			base.StartCoroutine(this.RecoverHealth());
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000B044C File Offset: 0x000AE64C
		public virtual void TakeDamage(vDamage damage)
		{
			if (damage != null)
			{
				this.onStartReceiveDamage.Invoke(damage);
				this.currentHealthRecoveryDelay = ((this.currentHealth <= 0f) ? 0f : this.healthRecoveryDelay);
				if (this.currentHealth > 0f && !this.isImmortal)
				{
					this.currentHealth -= damage.damageValue;
				}
				if (damage.damageValue > 0f)
				{
					this.onReceiveDamage.Invoke(damage);
				}
			}
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000B04CC File Offset: 0x000AE6CC
		protected virtual void HandleCheckHealthEvents()
		{
			List<vHealthController.CheckHealthEvent> list = this.checkHealthEvents.FindAll((vHealthController.CheckHealthEvent e) => (e.healthCompare == vHealthController.CheckHealthEvent.HealthCompare.Equals && this.currentHealth.Equals((float)e.healthToCheck)) || (e.healthCompare == vHealthController.CheckHealthEvent.HealthCompare.HigherThan && this.currentHealth > (float)e.healthToCheck) || (e.healthCompare == vHealthController.CheckHealthEvent.HealthCompare.LessThan && this.currentHealth < (float)e.healthToCheck));
			for (int i = 0; i < list.Count; i++)
			{
				list[i].OnCheckHealth.Invoke();
			}
			if (this.currentHealth < (float)this.maxHealth && base.gameObject.activeInHierarchy && !this.inHealthRecovery)
			{
				base.StartCoroutine(this.RecoverHealth());
			}
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIDamageReceiver.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIDamageReceiver.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04001D8B RID: 7563
		[vEditorToolbar("Health", false, "", false, false, order = 0)]
		[SerializeField]
		[vReadOnly(true)]
		protected bool _isDead;

		// Token: 0x04001D8C RID: 7564
		[vBarDisplay("_maxHealth", false)]
		[SerializeField]
		protected float _currentHealth;

		// Token: 0x04001D8D RID: 7565
		public bool isImmortal;

		// Token: 0x04001D8E RID: 7566
		[vHelpBox("If you want to start with different value, uncheck this and make sure that the current health has a value greater zero", vHelpBoxAttribute.MessageType.None)]
		public bool fillHealthOnStart = true;

		// Token: 0x04001D8F RID: 7567
		[SerializeField]
		protected int _maxHealth = 100;

		// Token: 0x04001D90 RID: 7568
		[SerializeField]
		protected float _healthRecovery;

		// Token: 0x04001D91 RID: 7569
		[SerializeField]
		protected float _healthRecoveryDelay;

		// Token: 0x04001D92 RID: 7570
		protected float _currentHealthRecoveryDelay;

		// Token: 0x04001D93 RID: 7571
		[vEditorToolbar("Events", false, "", false, false, order = 100)]
		public List<vHealthController.CheckHealthEvent> checkHealthEvents = new List<vHealthController.CheckHealthEvent>();

		// Token: 0x04001D94 RID: 7572
		[SerializeField]
		protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();

		// Token: 0x04001D95 RID: 7573
		[SerializeField]
		protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();

		// Token: 0x04001D96 RID: 7574
		[SerializeField]
		protected OnDead _onDead = new OnDead();

		// Token: 0x04001D97 RID: 7575
		public vHealthController.ValueChangedEvent onChangeHealth;

		// Token: 0x04001D98 RID: 7576
		public UnityEvent onResetHealth;

		// Token: 0x0200047B RID: 1147
		[Serializable]
		public class CheckHealthEvent
		{
			// Token: 0x04001D9A RID: 7578
			public int healthToCheck;

			// Token: 0x04001D9B RID: 7579
			public bool disableEventOnCheck;

			// Token: 0x04001D9C RID: 7580
			public vHealthController.CheckHealthEvent.HealthCompare healthCompare;

			// Token: 0x04001D9D RID: 7581
			public UnityEvent OnCheckHealth;

			// Token: 0x0200047C RID: 1148
			public enum HealthCompare
			{
				// Token: 0x04001D9F RID: 7583
				Equals,
				// Token: 0x04001DA0 RID: 7584
				HigherThan,
				// Token: 0x04001DA1 RID: 7585
				LessThan
			}
		}

		// Token: 0x0200047D RID: 1149
		[Serializable]
		public class ValueChangedEvent : UnityEvent<float>
		{
		}
	}
}
