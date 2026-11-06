using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x0200055D RID: 1373
	[Serializable]
	public class vItem : ScriptableObject
	{
		// Token: 0x060020E9 RID: 8425 RVA: 0x000C0D6D File Offset: 0x000BEF6D
		public void OnDestroy()
		{
			this.onDestroy.Invoke(this);
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x060020EA RID: 8426 RVA: 0x000C0D7C File Offset: 0x000BEF7C
		public Texture2D iconTexture
		{
			get
			{
				if (!this.icon)
				{
					return null;
				}
				Texture2D texture2D2;
				try
				{
					if (this.icon.rect.width != (float)this.icon.texture.width || this.icon.rect.height != (float)this.icon.texture.height)
					{
						Texture2D texture2D = new Texture2D((int)this.icon.textureRect.width, (int)this.icon.textureRect.height);
						texture2D.name = this.icon.name;
						Color[] pixels = this.icon.texture.GetPixels((int)this.icon.textureRect.x, (int)this.icon.textureRect.y, (int)this.icon.textureRect.width, (int)this.icon.textureRect.height);
						texture2D.SetPixels(pixels);
						texture2D.Apply();
						texture2D2 = texture2D;
					}
					else
					{
						texture2D2 = this.icon.texture;
					}
				}
				catch
				{
					Debug.LogWarning("Icon texture of the " + base.name + " is not Readable", this.icon.texture);
					texture2D2 = this.icon.texture;
				}
				return texture2D2;
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000C0EF8 File Offset: 0x000BF0F8
		public vItemAttribute GetItemAttribute(vItemAttributes attribute)
		{
			if (this.attributes != null)
			{
				return this.attributes.Find((vItemAttribute _attribute) => _attribute.name == attribute);
			}
			return null;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000C0F34 File Offset: 0x000BF134
		public vItemAttribute GetItemAttribute(string name)
		{
			if (this.attributes != null)
			{
				return this.attributes.Find((vItemAttribute attribute) => attribute.name.ToString().Equals(name));
			}
			return null;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000C0F70 File Offset: 0x000BF170
		public string GetItemAttributesText(List<vItemAttributes> ignore = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.attributes.Count; i++)
			{
				if (ignore == null || !ignore.Contains(this.attributes[i].name))
				{
					stringBuilder.AppendLine(this.GetItemAttributeText(i));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000C0FCC File Offset: 0x000BF1CC
		protected string GetItemAttributeText(int i)
		{
			if (this.attributes.Count > 0 && i < this.attributes.Count && this.attributes.Count > 0 && i < this.attributes.Count)
			{
				return this.attributes[i].GetDisplayText(null);
			}
			return string.Empty;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000C1029 File Offset: 0x000BF229
		protected string GetItemAttributeText(int i, string customFormat)
		{
			if (this.attributes.Count > 0 && i < this.attributes.Count)
			{
				return this.attributes[i].GetDisplayText(customFormat);
			}
			return string.Empty;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000C105F File Offset: 0x000BF25F
		public string ItemTypeText()
		{
			return this.ItemTypeText(this.type.DisplayFormat());
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000C1074 File Offset: 0x000BF274
		public string ItemTypeText(string format)
		{
			string text = this.type.ToString().InsertSpaceBeforeUpperCase().RemoveUnderline();
			if (string.IsNullOrEmpty(format))
			{
				return text;
			}
			if (format.Contains("(NAME)"))
			{
				format.Replace("(NAME)", text);
			}
			return format;
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000C10C4 File Offset: 0x000BF2C4
		public string GetFullItemDescription(string format = null, List<vItemAttributes> ignoreAttributes = null)
		{
			string text = "";
			if (string.IsNullOrEmpty(format))
			{
				text += base.name;
				text = text + "\n" + this.ItemTypeText();
				text = text + "\n" + this.description;
				text = text + "\n" + this.GetItemAttributesText(null);
			}
			else
			{
				text = format;
				if (text.Contains("(NAME)"))
				{
					text = text.Replace("(NAME)", base.name);
				}
				if (text.Contains("(TYPE)"))
				{
					text = text.Replace("(TYPE)", this.ItemTypeText());
				}
				if (text.Contains("(DESC)"))
				{
					text = text.Replace("(DESC)", this.description);
				}
				if (text.Contains("(ATTR)"))
				{
					text = text.Replace("(ATTR)", this.GetItemAttributesText(ignoreAttributes));
				}
			}
			return text;
		}

		// Token: 0x04002182 RID: 8578
		[HideInInspector]
		public int id;

		// Token: 0x04002183 RID: 8579
		[HideInInspector]
		public string description = "Item Description";

		// Token: 0x04002184 RID: 8580
		[HideInInspector]
		public vItemType type;

		// Token: 0x04002185 RID: 8581
		[HideInInspector]
		public Sprite icon;

		// Token: 0x04002186 RID: 8582
		[HideInInspector]
		public bool stackable = true;

		// Token: 0x04002187 RID: 8583
		[HideInInspector]
		public bool createNewItem = true;

		// Token: 0x04002188 RID: 8584
		[HideInInspector]
		public int maxStack;

		// Token: 0x04002189 RID: 8585
		[HideInInspector]
		public int amount;

		// Token: 0x0400218A RID: 8586
		[HideInInspector]
		public GameObject originalObject;

		// Token: 0x0400218B RID: 8587
		[HideInInspector]
		public GameObject dropObject;

		// Token: 0x0400218C RID: 8588
		[HideInInspector]
		public List<vItemAttribute> attributes = new List<vItemAttribute>();

		// Token: 0x0400218D RID: 8589
		[HideInInspector]
		public bool isInEquipArea;

		// Token: 0x0400218E RID: 8590
		[HideInInspector]
		public bool isEquiped;

		// Token: 0x0400218F RID: 8591
		public bool destroyAfterUse = true;

		// Token: 0x04002190 RID: 8592
		public bool canBeEquipped = true;

		// Token: 0x04002191 RID: 8593
		public bool canBeUsed = true;

		// Token: 0x04002192 RID: 8594
		public bool canBeDroped = true;

		// Token: 0x04002193 RID: 8595
		public bool canBeDestroyed = true;

		// Token: 0x04002194 RID: 8596
		[Header("Animation Settings")]
		[vHelpBox("Triggers a animation when Equipping a Weapon or enabling item.\nYou can also trigger an animation if the ItemType is a Consumable", vHelpBoxAttribute.MessageType.None)]
		public string EnableAnim = "LowBack";

		// Token: 0x04002195 RID: 8597
		[vHelpBox("Triggers a animation when Unequipping a Weapon or disable item", vHelpBoxAttribute.MessageType.None)]
		public string DisableAnim = "LowBack";

		// Token: 0x04002196 RID: 8598
		[vHelpBox("Delay to enable the Weapon/Item object when Equipping\n If ItemType is a Consumable use this to delay the item usage.", vHelpBoxAttribute.MessageType.None)]
		public float enableDelayTime = 0.5f;

		// Token: 0x04002197 RID: 8599
		[vHelpBox("Delay to hide the Weapon/Item object when Unequipping", vHelpBoxAttribute.MessageType.None)]
		public float disableDelayTime = 0.5f;

		// Token: 0x04002198 RID: 8600
		[vHelpBox("If the item is equippable use this to set a custom handler to instantiate the SpawnObject", vHelpBoxAttribute.MessageType.None)]
		public string customHandler;

		// Token: 0x04002199 RID: 8601
		[vHelpBox("If the item is equippable and need to use two hand\n<color=yellow><b>This option makes it impossible to equip two items</b></color>", vHelpBoxAttribute.MessageType.None)]
		public bool twoHandWeapon;

		// Token: 0x0400219A RID: 8602
		[HideInInspector]
		public OnHandleItemEvent onDestroy;
	}
}
