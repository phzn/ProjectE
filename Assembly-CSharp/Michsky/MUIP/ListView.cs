using System;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x02000219 RID: 537
	public class ListView : MonoBehaviour
	{
		// Token: 0x06000F0E RID: 3854 RVA: 0x00064A43 File Offset: 0x00062C43
		private void Awake()
		{
			if (this.itemParent == null)
			{
				Debug.LogError("<b>[List View]</b> 'Item Parent' is missing.");
				return;
			}
			if (this.initializeOnAwake)
			{
				this.InitializeItems();
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00064A6C File Offset: 0x00062C6C
		public void InitializeItems()
		{
			foreach (object obj in this.itemParent)
			{
				Object.Destroy(((Transform)obj).gameObject);
			}
			for (int i = 0; i < this.listItems.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemPreset, new Vector3(0f, 0f, 0f), Quaternion.identity);
				gameObject.transform.SetParent(this.itemParent, false);
				gameObject.name = this.listItems[i].itemTitle;
				ListViewItem component = gameObject.GetComponent<ListViewItem>();
				component.rowCount = this.rowCount;
				component.row0Ref = this.listItems[i].row0;
				component.row1Ref = this.listItems[i].row1;
				component.row2Ref = this.listItems[i].row2;
				component.PassReferences();
			}
			if (!this.showScrollbar && this.scrollbar != null)
			{
				this.scrollbar.transform.localScale = new Vector3(0f, 0f, 0f);
				return;
			}
			if (this.showScrollbar && this.scrollbar != null)
			{
				this.scrollbar.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}

		// Token: 0x04000FB6 RID: 4022
		public Transform itemParent;

		// Token: 0x04000FB7 RID: 4023
		public GameObject itemPreset;

		// Token: 0x04000FB8 RID: 4024
		public GameObject scrollbar;

		// Token: 0x04000FB9 RID: 4025
		public bool initializeOnAwake = true;

		// Token: 0x04000FBA RID: 4026
		public bool showScrollbar = true;

		// Token: 0x04000FBB RID: 4027
		public ListView.RowCount rowCount = ListView.RowCount.Two;

		// Token: 0x04000FBC RID: 4028
		[SerializeField]
		public List<ListView.ListItem> listItems = new List<ListView.ListItem>();

		// Token: 0x0200021A RID: 538
		[Serializable]
		public class ListItem
		{
			// Token: 0x04000FBD RID: 4029
			public string itemTitle = "List Item";

			// Token: 0x04000FBE RID: 4030
			[HideInInspector]
			public ListView.ListRow row0;

			// Token: 0x04000FBF RID: 4031
			[HideInInspector]
			public ListView.ListRow row1;

			// Token: 0x04000FC0 RID: 4032
			[HideInInspector]
			public ListView.ListRow row2;
		}

		// Token: 0x0200021B RID: 539
		[Serializable]
		public class ListRow
		{
			// Token: 0x04000FC1 RID: 4033
			public ListView.RowType rowType = ListView.RowType.Text;

			// Token: 0x04000FC2 RID: 4034
			public Sprite rowIcon;

			// Token: 0x04000FC3 RID: 4035
			public string rowText = "Row text";

			// Token: 0x04000FC4 RID: 4036
			public bool usePreferredWidth;

			// Token: 0x04000FC5 RID: 4037
			public int preferredWidth = 50;

			// Token: 0x04000FC6 RID: 4038
			[Range(0.1f, 1f)]
			public float iconScale = 1f;
		}

		// Token: 0x0200021C RID: 540
		public enum RowType
		{
			// Token: 0x04000FC8 RID: 4040
			Icon,
			// Token: 0x04000FC9 RID: 4041
			Text
		}

		// Token: 0x0200021D RID: 541
		public enum RowCount
		{
			// Token: 0x04000FCB RID: 4043
			One,
			// Token: 0x04000FCC RID: 4044
			Two,
			// Token: 0x04000FCD RID: 4045
			Three
		}
	}
}
