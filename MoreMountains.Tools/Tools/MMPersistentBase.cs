using System;
using System.Linq;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200013B RID: 315
	[AddComponentMenu("")]
	public class MMPersistentBase : MonoBehaviour, IMMPersistent
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x000228C7 File Offset: 0x00020AC7
		protected virtual void OnValidate()
		{
			this.ValidateGuid();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000228CF File Offset: 0x00020ACF
		public virtual string GetGuid()
		{
			return this._guid;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x000228D7 File Offset: 0x00020AD7
		public virtual void SetGuid(string newGUID)
		{
			this._guid = newGUID;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x000228E0 File Offset: 0x00020AE0
		public virtual string OnSave()
		{
			return string.Empty;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000228E7 File Offset: 0x00020AE7
		public virtual void OnLoad(string data)
		{
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000228E9 File Offset: 0x00020AE9
		public virtual bool ShouldBeSaved()
		{
			return this.SaveActive;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x000228F4 File Offset: 0x00020AF4
		public virtual string GenerateGuid()
		{
			string text = Guid.NewGuid().ToString();
			string text2 = string.Concat(new string[]
			{
				base.gameObject.scene.name,
				"-",
				base.gameObject.name,
				"-",
				text
			});
			if (!string.IsNullOrEmpty(this.UniqueIDSuffix))
			{
				text2 = text2 + "-" + this.UniqueIDSuffix;
			}
			this.SetGuid(text2);
			return text2;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00022980 File Offset: 0x00020B80
		public virtual bool GuidIsUnique(string guid)
		{
			return Resources.FindObjectsOfTypeAll<MMPersistentBase>().Count<MMPersistentBase>((MMPersistentBase x) => x.GetGuid() == guid) == 1;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000229B4 File Offset: 0x00020BB4
		public virtual void ValidateGuid()
		{
			if (!base.gameObject.scene.IsValid())
			{
				this._guid = string.Empty;
				return;
			}
			int num = 1000;
			int num2 = 0;
			while ((string.IsNullOrEmpty(this._guid) || !this.GuidIsUnique(this._guid)) && num2 < num)
			{
				this.GenerateGuid();
				num2++;
			}
			if (num2 == num)
			{
				Debug.LogWarning(base.gameObject.name + " couldn't generate a unique GUID after " + num.ToString() + " tries, you should probably change its UniqueIDSuffix");
			}
		}

		// Token: 0x040008AB RID: 2219
		[Header("Save")]
		[Tooltip("whether or not this object should be saved")]
		public bool SaveActive = true;

		// Token: 0x040008AC RID: 2220
		[Header("ID")]
		[Tooltip("an optional suffix to add to the GUID, to make it more readable")]
		public string UniqueIDSuffix;

		// Token: 0x040008AD RID: 2221
		[Tooltip("the object's unique ID")]
		[SerializeField]
		[MMReadOnly]
		protected string _guid;

		// Token: 0x040008AE RID: 2222
		[MMInspectorButton("GenerateGuid")]
		public bool GenerateGuidButton;
	}
}
