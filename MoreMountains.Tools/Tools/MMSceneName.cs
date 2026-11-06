using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000055 RID: 85
	public class MMSceneName : MonoBehaviour
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x000093A1 File Offset: 0x000075A1
		protected virtual void Awake()
		{
			this._text = base.gameObject.GetComponent<Text>();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000093B4 File Offset: 0x000075B4
		protected virtual void Start()
		{
			this.SetLevelNameText();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000093BC File Offset: 0x000075BC
		public virtual void SetLevelNameText()
		{
			if (this._text != null)
			{
				this._text.text = SceneManager.GetActiveScene().name;
			}
		}

		// Token: 0x04000285 RID: 645
		protected Text _text;
	}
}
