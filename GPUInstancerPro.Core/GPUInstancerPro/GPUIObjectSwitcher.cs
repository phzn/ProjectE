using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GPUInstancerPro
{
	// Token: 0x0200004C RID: 76
	public class GPUIObjectSwitcher : GPUIInputHandler
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000E99C File Offset: 0x0000CB9C
		private void OnEnable()
		{
			if (this.gameObjects == null)
			{
				this.gameObjects = new List<GameObject>();
			}
			if (this.gameObjects.Count > 0)
			{
				this._currentActiveGO = this.gameObjects[0];
				if (this._currentActiveGO != null)
				{
					this._currentActiveGO.SetActive(true);
					if (this.activeGONameText != null)
					{
						this.activeGONameText.text = this._currentActiveGO.name;
					}
				}
			}
			for (int i = 1; i < this.gameObjects.Count; i++)
			{
				GameObject gameObject = this.gameObjects[i];
				if (!(gameObject == null))
				{
					gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000EA50 File Offset: 0x0000CC50
		private void Update()
		{
			int count = this.gameObjects.Count;
			int num = -1;
			if (count > 0 && base.GetKeyDown(KeyCode.Alpha1))
			{
				num = 0;
			}
			else if (count > 1 && base.GetKeyDown(KeyCode.Alpha2))
			{
				num = 1;
			}
			else if (count > 2 && base.GetKeyDown(KeyCode.Alpha3))
			{
				num = 2;
			}
			else if (count > 3 && base.GetKeyDown(KeyCode.Alpha4))
			{
				num = 3;
			}
			else if (count > 4 && base.GetKeyDown(KeyCode.Alpha5))
			{
				num = 4;
			}
			else if (count > 5 && base.GetKeyDown(KeyCode.Alpha6))
			{
				num = 5;
			}
			else if (count > 6 && base.GetKeyDown(KeyCode.Alpha7))
			{
				num = 6;
			}
			else if (count > 7 && base.GetKeyDown(KeyCode.Alpha8))
			{
				num = 7;
			}
			else if (count > 8 && base.GetKeyDown(KeyCode.Alpha9))
			{
				num = 8;
			}
			if (num >= 0)
			{
				if (this._currentActiveGO != null)
				{
					this._currentActiveGO.SetActive(false);
				}
				this._currentActiveGO = this.gameObjects[num];
				if (this._currentActiveGO != null)
				{
					this._currentActiveGO.SetActive(true);
					if (this.activeGONameText != null)
					{
						this.activeGONameText.text = this._currentActiveGO.name;
					}
				}
			}
		}

		// Token: 0x040001F9 RID: 505
		public List<GameObject> gameObjects;

		// Token: 0x040001FA RID: 506
		public Text activeGONameText;

		// Token: 0x040001FB RID: 507
		private GameObject _currentActiveGO;
	}
}
