using System;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x0200021E RID: 542
	public class ListViewItem : MonoBehaviour
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x00064C68 File Offset: 0x00062E68
		public void PassReferences()
		{
			if (this.rowCount == ListView.RowCount.One)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(false);
				this.row2.gameObject.SetActive(false);
			}
			else if (this.rowCount == ListView.RowCount.Two)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(true);
				this.row2.gameObject.SetActive(false);
			}
			else if (this.rowCount == ListView.RowCount.Three)
			{
				this.row0.gameObject.SetActive(true);
				this.row1.gameObject.SetActive(true);
				this.row2.gameObject.SetActive(true);
			}
			if (this.row0Ref.rowType == ListView.RowType.Icon)
			{
				this.row0.iconImage.sprite = this.row0Ref.rowIcon;
				this.row0.iconImage.gameObject.SetActive(true);
				this.row0.textObject.gameObject.SetActive(false);
				this.row0.iconImage.transform.localScale = new Vector3(this.row0Ref.iconScale, this.row0Ref.iconScale, this.row0Ref.iconScale);
			}
			else if (this.row0Ref.rowType == ListView.RowType.Text)
			{
				this.row0.textObject.text = this.row0Ref.rowText;
				this.row0.iconImage.gameObject.SetActive(false);
				this.row0.textObject.gameObject.SetActive(true);
			}
			if (this.row0Ref.usePreferredWidth)
			{
				this.row0.layoutElement.preferredWidth = (float)this.row0Ref.preferredWidth;
			}
			else
			{
				this.row0.layoutElement.preferredWidth = -1f;
			}
			if (this.row1Ref == null)
			{
				return;
			}
			if (this.row1Ref.rowType == ListView.RowType.Icon)
			{
				this.row1.iconImage.sprite = this.row1Ref.rowIcon;
				this.row1.iconImage.gameObject.SetActive(true);
				this.row1.textObject.gameObject.SetActive(false);
				this.row1.iconImage.transform.localScale = new Vector3(this.row1Ref.iconScale, this.row1Ref.iconScale, this.row1Ref.iconScale);
			}
			else if (this.row1Ref.rowType == ListView.RowType.Text)
			{
				this.row1.textObject.text = this.row1Ref.rowText;
				this.row1.iconImage.gameObject.SetActive(false);
				this.row1.textObject.gameObject.SetActive(true);
			}
			if (this.row1Ref.usePreferredWidth)
			{
				this.row1.layoutElement.preferredWidth = (float)this.row1Ref.preferredWidth;
			}
			else
			{
				this.row1.layoutElement.preferredWidth = -1f;
			}
			if (this.row2Ref == null)
			{
				return;
			}
			if (this.row2Ref.rowType == ListView.RowType.Icon)
			{
				this.row2.iconImage.sprite = this.row2Ref.rowIcon;
				this.row2.iconImage.gameObject.SetActive(true);
				this.row2.textObject.gameObject.SetActive(false);
				this.row2.iconImage.transform.localScale = new Vector3(this.row2Ref.iconScale, this.row2Ref.iconScale, this.row2Ref.iconScale);
			}
			else if (this.row2Ref.rowType == ListView.RowType.Text)
			{
				this.row2.textObject.text = this.row2Ref.rowText;
				this.row2.iconImage.gameObject.SetActive(false);
				this.row2.textObject.gameObject.SetActive(true);
			}
			if (this.row2Ref.usePreferredWidth)
			{
				this.row2.layoutElement.preferredWidth = (float)this.row2Ref.preferredWidth;
				return;
			}
			this.row2.layoutElement.preferredWidth = -1f;
		}

		// Token: 0x04000FCE RID: 4046
		[Header("Settings")]
		public ListViewRow row0;

		// Token: 0x04000FCF RID: 4047
		public ListViewRow row1;

		// Token: 0x04000FD0 RID: 4048
		public ListViewRow row2;

		// Token: 0x04000FD1 RID: 4049
		[Header("References")]
		public ListView.RowCount rowCount;

		// Token: 0x04000FD2 RID: 4050
		public ListView.ListRow row0Ref;

		// Token: 0x04000FD3 RID: 4051
		public ListView.ListRow row1Ref;

		// Token: 0x04000FD4 RID: 4052
		public ListView.ListRow row2Ref;
	}
}
