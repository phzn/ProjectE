using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000088 RID: 136
	[RequireComponent(typeof(SpriteRenderer))]
	[AddComponentMenu("More Mountains/Tools/Sprites/MMAutoOrderInLayer")]
	public class MMAutoOrderInLayer : MonoBehaviour
	{
		// Token: 0x06000303 RID: 771 RVA: 0x0000F7DB File Offset: 0x0000D9DB
		protected virtual void Start()
		{
			this.Initialization();
			this.AutomateLayerOrder();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000F7E9 File Offset: 0x0000D9E9
		protected virtual void Initialization()
		{
			this._spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		protected virtual void AutomateLayerOrder()
		{
			int num = 0;
			if (this._spriteRenderer == null)
			{
				return;
			}
			if (this.BasedOnParentOrder)
			{
				int num2 = 0;
				Component[] componentsInParent = base.GetComponentsInParent(typeof(SpriteRenderer));
				if (componentsInParent != null)
				{
					foreach (SpriteRenderer spriteRenderer in componentsInParent)
					{
						if (spriteRenderer.sortingLayerID == this._spriteRenderer.sortingLayerID && spriteRenderer.sortingOrder > num2)
						{
							num2 = spriteRenderer.sortingOrder;
						}
					}
					num = num2 + this.ParentIncrement;
				}
			}
			else
			{
				num = MMAutoOrderInLayer.CurrentMaxCharacterOrderInLayer + this.GlobalCounterIncrement;
				MMAutoOrderInLayer.CurrentMaxCharacterOrderInLayer += this.GlobalCounterIncrement;
			}
			this._spriteRenderer.sortingOrder = num;
			if (this.ApplyNewOrderToChildren)
			{
				Component[] componentsInChildren = base.GetComponentsInChildren(typeof(SpriteRenderer));
				if (componentsInChildren != null)
				{
					foreach (SpriteRenderer spriteRenderer2 in componentsInChildren)
					{
						if (spriteRenderer2.sortingLayerID == this._spriteRenderer.sortingLayerID)
						{
							spriteRenderer2.sortingOrder = num + this.ChildrenIncrement;
						}
					}
				}
			}
		}

		// Token: 0x04000438 RID: 1080
		private static int CurrentMaxCharacterOrderInLayer;

		// Token: 0x04000439 RID: 1081
		[Header("Global Counter")]
		[MMInformation("Add this component to an object with a sprite renderer, and it'll give it a new order in layer based on the settings defined here. First is the global counter increment, or how much you'd like to increment the layer order between two objects on that same layer.", MMInformationAttribute.InformationType.Info, false)]
		public int GlobalCounterIncrement = 5;

		// Token: 0x0400043A RID: 1082
		[Header("Parent")]
		[MMInformation("You can also decide to determine the new layer order based on the parent sprite's order (it'll have to be on the same layer).", MMInformationAttribute.InformationType.Info, false)]
		public bool BasedOnParentOrder;

		// Token: 0x0400043B RID: 1083
		public int ParentIncrement = 1;

		// Token: 0x0400043C RID: 1084
		[Header("Children")]
		[MMInformation("And here you can decide to apply your new layer order to all children.", MMInformationAttribute.InformationType.Info, false)]
		public bool ApplyNewOrderToChildren;

		// Token: 0x0400043D RID: 1085
		public int ChildrenIncrement;

		// Token: 0x0400043E RID: 1086
		protected SpriteRenderer _spriteRenderer;
	}
}
