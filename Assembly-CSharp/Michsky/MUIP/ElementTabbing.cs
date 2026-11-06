using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.MUIP
{
	// Token: 0x02000244 RID: 580
	[AddComponentMenu("Modern UI Pack/Tools/Element Tabbing")]
	public class ElementTabbing : MonoBehaviour
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x00068654 File Offset: 0x00066854
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				if (this.currentIndex > base.transform.childCount - 2)
				{
					this.SelectElement(0);
					return;
				}
				if (this.catchedObject && this.type == ElementTabbing.ObjectType.Button)
				{
					base.transform.GetChild(this.currentIndex).GetComponent<ButtonManager>().OnDeselect(null);
				}
				else if (this.catchedObject && this.type == ElementTabbing.ObjectType.InputField)
				{
					base.transform.GetChild(this.currentIndex).GetComponent<CustomInputField>().inputText.DeactivateInputField(false);
				}
				this.currentIndex++;
				for (int i = 0; i < base.transform.childCount; i++)
				{
					if (i >= this.currentIndex)
					{
						if (base.transform.GetChild(i).GetComponent<ButtonManager>() != null)
						{
							base.transform.GetChild(i).GetComponent<ButtonManager>().OnSelect(null);
							EventSystem.current.SetSelectedGameObject(base.transform.GetChild(i).gameObject);
							this.type = ElementTabbing.ObjectType.Button;
							return;
						}
						if (base.transform.GetChild(i).GetComponent<CustomInputField>() != null)
						{
							base.transform.GetChild(i).GetComponent<CustomInputField>().inputText.ActivateInputField();
							EventSystem.current.SetSelectedGameObject(base.transform.GetChild(i).gameObject);
							this.type = ElementTabbing.ObjectType.InputField;
							return;
						}
						this.catchedObject = false;
					}
				}
			}
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x000687D4 File Offset: 0x000669D4
		private void SelectElement(int index)
		{
			this.currentIndex = index;
			if (base.transform.GetChild(index).GetComponent<ButtonManager>() != null)
			{
				base.transform.GetChild(index).GetComponent<ButtonManager>().OnSelect(null);
				EventSystem.current.SetSelectedGameObject(base.transform.GetChild(index).gameObject);
				this.type = ElementTabbing.ObjectType.Button;
				return;
			}
			if (base.transform.GetChild(index).GetComponent<CustomInputField>() != null)
			{
				base.transform.GetChild(index).GetComponent<CustomInputField>().inputText.ActivateInputField();
				EventSystem.current.SetSelectedGameObject(base.transform.GetChild(index).gameObject);
				this.type = ElementTabbing.ObjectType.InputField;
				return;
			}
			this.catchedObject = false;
		}

		// Token: 0x040010BA RID: 4282
		private int currentIndex = -1;

		// Token: 0x040010BB RID: 4283
		private bool catchedObject;

		// Token: 0x040010BC RID: 4284
		private ElementTabbing.ObjectType type;

		// Token: 0x02000245 RID: 581
		public enum ObjectType
		{
			// Token: 0x040010BE RID: 4286
			Button,
			// Token: 0x040010BF RID: 4287
			InputField
		}
	}
}
