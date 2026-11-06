using System;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x0200110C RID: 4364
	public sealed class WorldPickupHoldInput
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06008472 RID: 33906 RVA: 0x0035FDAD File Offset: 0x0035DFAD
		// (set) Token: 0x06008473 RID: 33907 RVA: 0x0035FDB5 File Offset: 0x0035DFB5
		public int CapturedSlot { get; private set; } = -1;

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06008474 RID: 33908 RVA: 0x0035FDBE File Offset: 0x0035DFBE
		// (set) Token: 0x06008475 RID: 33909 RVA: 0x0035FDC6 File Offset: 0x0035DFC6
		public InventoryItemSO CapturedItem { get; private set; }

		// Token: 0x06008476 RID: 33910 RVA: 0x0035FDD0 File Offset: 0x0035DFD0
		public WorldPickupInputAction Step(bool pressed, bool released, float deltaTime, float holdDuration, bool canHold, int selectedSlot, InventoryItemSO selectedItem)
		{
			if (pressed)
			{
				this._pressed = true;
				this._consumed = false;
				this._elapsed = 0f;
				this._canHold = canHold;
				this.CapturedSlot = selectedSlot;
				this.CapturedItem = selectedItem;
			}
			if (!this._pressed)
			{
				return WorldPickupInputAction.None;
			}
			if (!this._consumed)
			{
				this._elapsed += Mathf.Max(0f, deltaTime);
				if (this._canHold && this._elapsed >= Mathf.Max(0.05f, holdDuration))
				{
					this._consumed = true;
					if (released)
					{
						this._pressed = false;
					}
					return WorldPickupInputAction.Hold;
				}
			}
			if (!released)
			{
				return WorldPickupInputAction.None;
			}
			this._pressed = false;
			if (!this._consumed)
			{
				return WorldPickupInputAction.Tap;
			}
			return WorldPickupInputAction.None;
		}

		// Token: 0x06008477 RID: 33911 RVA: 0x0035FE84 File Offset: 0x0035E084
		public void Reset()
		{
			this._pressed = (this._consumed = (this._canHold = false));
			this._elapsed = 0f;
			this.CapturedSlot = -1;
			this.CapturedItem = null;
		}

		// Token: 0x0400873F RID: 34623
		private bool _pressed;

		// Token: 0x04008740 RID: 34624
		private bool _consumed;

		// Token: 0x04008741 RID: 34625
		private bool _canHold;

		// Token: 0x04008742 RID: 34626
		private float _elapsed;
	}
}
