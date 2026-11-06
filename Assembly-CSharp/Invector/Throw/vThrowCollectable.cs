using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.Throw
{
	// Token: 0x020004EF RID: 1263
	[vClassHeader("THROW COLLECTABLE", false, "icon_v2", false, "")]
	public class vThrowCollectable : vMonoBehaviour
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000B79B9 File Offset: 0x000B5BB9
		private bool isInventory
		{
			get
			{
				return this.throwManager != null && this.throwManager is vThrowManagerInventory;
			}
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000B79DC File Offset: 0x000B5BDC
		private void OnTriggerStay(Collider other)
		{
			if (this.throwManager != null)
			{
				this.UpdateThrowInfo(false);
				return;
			}
			if (other.gameObject.CompareTag("Player"))
			{
				this.throwManager = other.GetComponentInChildren<vThrowManagerBase>();
			}
			if (this.throwManager != null)
			{
				this._throwManagerCollider = other;
				this.onEnterTrigger.Invoke();
			}
			this.UpdateThrowInfo(true);
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000B7A44 File Offset: 0x000B5C44
		private void OnTriggerExit(Collider other)
		{
			if (this._throwManagerCollider != null && other.gameObject == this._throwManagerCollider.gameObject)
			{
				this._throwManagerCollider = null;
				this.throwManager = null;
				this.onExitTrigger.Invoke();
			}
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000B7A90 File Offset: 0x000B5C90
		public void UpdateThrowInfo(bool firstEnter)
		{
			if (this.throwManager != null)
			{
				if (this.isInventory != this._isInventory || firstEnter)
				{
					if (this.isInventory)
					{
						this.onIsInventoryManager.Invoke();
					}
					else
					{
						this.onIsStandAloneManager.Invoke();
					}
					this._isInventory = this.isInventory;
				}
				int num;
				bool flag = this.throwManager.CanCollectThrowable(this.throwableName, out num);
				if (this.canCollect != flag || firstEnter)
				{
					this.canCollect = flag;
					if (flag)
					{
						if (this.isInventory)
						{
							this.onCanCollectFromInventory.Invoke();
							return;
						}
						this.onCanCollect.Invoke();
						return;
					}
					else
					{
						this.onReachMaxObjects.Invoke();
					}
				}
			}
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B7B48 File Offset: 0x000B5D48
		public void UpdateThrowObj()
		{
			int num;
			if (this.throwManager.CanCollectThrowable(this.throwableName, out num))
			{
				this.throwManager.OnCollectThrowable(this.throwableName, this.amount);
				if (this.amount <= num)
				{
					if (this.destroyAfter)
					{
						Object.Destroy(base.gameObject);
					}
				}
				else
				{
					this.amount -= num;
					if (this.amount <= 0 && this.destroyAfter)
					{
						Object.Destroy(base.gameObject);
					}
				}
				this.onCollectObject.Invoke();
				return;
			}
			this.onReachMaxObjects.Invoke();
		}

		// Token: 0x04001F50 RID: 8016
		public string throwableName;

		// Token: 0x04001F51 RID: 8017
		public int amount = 1;

		// Token: 0x04001F52 RID: 8018
		public bool destroyAfter = true;

		// Token: 0x04001F53 RID: 8019
		public UnityEvent onCanCollect;

		// Token: 0x04001F54 RID: 8020
		public UnityEvent onCanCollectFromInventory;

		// Token: 0x04001F55 RID: 8021
		public UnityEvent onIsStandAloneManager;

		// Token: 0x04001F56 RID: 8022
		public UnityEvent onIsInventoryManager;

		// Token: 0x04001F57 RID: 8023
		public UnityEvent onCollectObject;

		// Token: 0x04001F58 RID: 8024
		public UnityEvent onReachMaxObjects;

		// Token: 0x04001F59 RID: 8025
		public UnityEvent onEnterTrigger;

		// Token: 0x04001F5A RID: 8026
		public UnityEvent onExitTrigger;

		// Token: 0x04001F5B RID: 8027
		private vThrowManagerBase throwManager;

		// Token: 0x04001F5C RID: 8028
		private Collider _throwManagerCollider;

		// Token: 0x04001F5D RID: 8029
		protected bool canCollect;

		// Token: 0x04001F5E RID: 8030
		private bool _isInventory;
	}
}
