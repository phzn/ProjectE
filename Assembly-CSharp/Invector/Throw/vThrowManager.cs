using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Throw
{
	// Token: 0x020004F1 RID: 1265
	[vClassHeader("THROW MANAGER STANDALONE", true, "icon_v2", false, "")]
	public class vThrowManager : vThrowManagerBase
	{
		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x000B7D4A File Offset: 0x000B5F4A
		public virtual List<vThrowManager.Throwable> Throwables
		{
			get
			{
				return this.throwables;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000B7D52 File Offset: 0x000B5F52
		public virtual vThrowUI ui
		{
			get
			{
				if (!this._ui)
				{
					this._ui = base.GetComponentInChildren<vThrowUI>();
					if (this._ui)
					{
						this._ui.UpdateCount(this, true);
					}
				}
				return this._ui;
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000B7D8D File Offset: 0x000B5F8D
		protected override IEnumerator Start()
		{
			yield return base.Start();
			if (this.ui != null)
			{
				this.ui.UpdateCount(this, true);
			}
			this.onThrowObject.AddListener(new UnityAction(this.UpdateUI));
			for (int i = 0; i < this.Throwables.Count; i++)
			{
				this.Throwables[i].throwableInHandler.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000B7D9C File Offset: 0x000B5F9C
		protected override void StartThrow()
		{
			base.StartThrow();
			this.CurrentThrowable.amount--;
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000B7DB7 File Offset: 0x000B5FB7
		protected override void Throw()
		{
			base.Throw();
			this.CurrentThrowable.ResetThrowable();
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x000B7DCA File Offset: 0x000B5FCA
		protected override void DisableAimMode()
		{
			base.DisableAimMode();
			if (this.CurrentThrowable != null && !this.isThrowing)
			{
				this.CurrentThrowable.SetActive(false);
			}
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000B7DEE File Offset: 0x000B5FEE
		protected override void EquipThrowObject()
		{
			base.EquipThrowObject();
			if (this.CurrentThrowable != null)
			{
				this.CurrentThrowable.SetActive(true);
			}
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000B7E0A File Offset: 0x000B600A
		protected override vThrowableObject GetInstanceOfThrowable()
		{
			return this.ObjectToThrow;
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000B7E12 File Offset: 0x000B6012
		public override int MaxThrowObjects
		{
			get
			{
				if (this.CurrentThrowable == null)
				{
					return 0;
				}
				return this.CurrentThrowable.maxAmount;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000B7E29 File Offset: 0x000B6029
		public override Sprite CurrentThrowableSprite
		{
			get
			{
				if (this.CurrentThrowable == null)
				{
					return null;
				}
				return this.CurrentThrowable.sprite;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x000B7E40 File Offset: 0x000B6040
		public override int CurrentThrowAmount
		{
			get
			{
				if (this.CurrentThrowable == null)
				{
					return 0;
				}
				return this.CurrentThrowable.amount;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001E8C RID: 7820 RVA: 0x000B7E57 File Offset: 0x000B6057
		public override vThrowableObject ObjectToThrow
		{
			get
			{
				if (this.CurrentThrowable == null)
				{
					return null;
				}
				return this.CurrentThrowable.throwableInHandler;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x000B7E6E File Offset: 0x000B606E
		public vThrowManager.Throwable CurrentThrowable
		{
			get
			{
				if (this.indexOfCurrentThrowable < this.throwables.Count)
				{
					return this.throwables[this.indexOfCurrentThrowable];
				}
				this.indexOfCurrentThrowable = 0;
				return null;
			}
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000B7EA0 File Offset: 0x000B60A0
		public void NextThrowable()
		{
			int num = 0;
			if (this.indexOfCurrentThrowable + 1 < this.throwables.Count)
			{
				num = this.indexOfCurrentThrowable + 1;
			}
			if (this.throwables[num].amount > 0 || !this.isAiming)
			{
				this.SelectThrowable(num);
			}
			else if (this.isAiming)
			{
				if (this.throwables.Exists((vThrowManager.Throwable t) => t.amount > 0))
				{
					for (int i = 0; i < this.throwables.Count; i++)
					{
						if (num + 1 < this.throwables.Count)
						{
							num++;
						}
						else
						{
							num = 0;
						}
						if (this.throwables[num].amount > 0)
						{
							this.SelectThrowable(num);
							break;
						}
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000B7F78 File Offset: 0x000B6178
		public void PreviousThrowable()
		{
			int num = this.throwables.Count - 1;
			if (this.indexOfCurrentThrowable - 1 >= 0)
			{
				num = this.indexOfCurrentThrowable - 1;
			}
			if (this.throwables[num].amount > 0 || !this.isAiming)
			{
				this.SelectThrowable(num);
			}
			else if (this.isAiming)
			{
				if (this.throwables.Exists((vThrowManager.Throwable t) => t.amount > 0))
				{
					for (int i = 0; i < this.throwables.Count; i++)
					{
						if (num - 1 >= 0)
						{
							num--;
						}
						else
						{
							num = this.throwables.Count - 1;
						}
						if (this.throwables[num].amount > 0)
						{
							this.SelectThrowable(num);
							break;
						}
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000B8058 File Offset: 0x000B6258
		public void SelectThrowable(int indexOfThrowable)
		{
			if (this.inEnterThrowMode || this.isThrowing)
			{
				return;
			}
			bool isAiming = this.isAiming;
			this.DisableAimMode();
			this.ExitThrowMode();
			if (indexOfThrowable >= 0 && indexOfThrowable < this.throwables.Count)
			{
				this.indexOfCurrentThrowable = indexOfThrowable;
			}
			if (isAiming && this.CurrentThrowAmount > 0)
			{
				this.EnterThrowMode();
			}
			this.UpdateUI();
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000B80B8 File Offset: 0x000B62B8
		public override bool CanCollectThrowable(string throwableName, out int remainingAmount)
		{
			vThrowManager.Throwable throwable = this.throwables.Find((vThrowManager.Throwable t) => t.name.Equals(throwableName));
			if (throwable != null)
			{
				remainingAmount = throwable.maxAmount - throwable.amount;
				return remainingAmount > 0;
			}
			remainingAmount = 0;
			return false;
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000B8108 File Offset: 0x000B6308
		public override void OnCollectThrowable(string throwableName, int amount = 1)
		{
			vThrowManager.Throwable throwable = this.throwables.Find((vThrowManager.Throwable t) => t.name.Equals(throwableName));
			if (throwable != null)
			{
				int num = throwable.maxAmount - throwable.amount;
				if (num > 0)
				{
					int num2;
					if (num >= amount)
					{
						num2 = amount;
					}
					else
					{
						num2 = num;
					}
					throwable.amount += num2;
					this.UpdateUI();
				}
			}
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000B816F File Offset: 0x000B636F
		protected virtual void UpdateUI()
		{
			if (this.ui != null)
			{
				this.ui.UpdateCount(this, true);
			}
		}

		// Token: 0x04001F63 RID: 8035
		[vEditorToolbar("Throwables", false, "", false, false)]
		[SerializeField]
		protected List<vThrowManager.Throwable> throwables;

		// Token: 0x04001F64 RID: 8036
		[SerializeField]
		protected int defaultMaxAmount = 6;

		// Token: 0x04001F65 RID: 8037
		public int indexOfCurrentThrowable;

		// Token: 0x04001F66 RID: 8038
		protected vThrowManager.Throwable _currentThrowable;

		// Token: 0x04001F67 RID: 8039
		protected vThrowUI _ui;

		// Token: 0x020004F2 RID: 1266
		[Serializable]
		public class Throwable
		{
			// Token: 0x1700058F RID: 1423
			// (get) Token: 0x06001E96 RID: 7830 RVA: 0x000B81A4 File Offset: 0x000B63A4
			internal vThrowableObject throwableInHandler
			{
				get
				{
					if (this._throwableInHandler == null)
					{
						this._throwableInHandler = Object.Instantiate<vThrowableObject>(this.throwable, this.handler);
						this._throwableInHandler.transform.localPosition = Vector3.zero;
						this._throwableInHandler.transform.localEulerAngles = Vector3.zero;
						this._throwableInHandler.gameObject.SetActive(false);
					}
					return this._throwableInHandler;
				}
			}

			// Token: 0x06001E97 RID: 7831 RVA: 0x000B8217 File Offset: 0x000B6417
			public void ResetThrowable()
			{
				this._throwableInHandler = null;
			}

			// Token: 0x06001E98 RID: 7832 RVA: 0x000B8220 File Offset: 0x000B6420
			internal void SetActive(bool value)
			{
				if (this._throwableInHandler)
				{
					this._throwableInHandler.gameObject.SetActive(value);
				}
			}

			// Token: 0x04001F68 RID: 8040
			public string name = "ThrowableName";

			// Token: 0x04001F69 RID: 8041
			public Transform handler;

			// Token: 0x04001F6A RID: 8042
			public vThrowableObject throwable;

			// Token: 0x04001F6B RID: 8043
			public Sprite sprite;

			// Token: 0x04001F6C RID: 8044
			public int amount;

			// Token: 0x04001F6D RID: 8045
			public int maxAmount;

			// Token: 0x04001F6E RID: 8046
			private vThrowableObject _throwableInHandler;
		}
	}
}
