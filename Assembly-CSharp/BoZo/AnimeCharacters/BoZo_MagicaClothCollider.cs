using System;
using MagicaCloth2;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008C8 RID: 2248
	public class BoZo_MagicaClothCollider : MonoBehaviour
	{
		// Token: 0x06003C8E RID: 15502 RVA: 0x00168548 File Offset: 0x00166748
		private void Awake()
		{
			MagicaCapsuleCollider magicaCapsuleCollider = base.gameObject.AddComponent<MagicaCapsuleCollider>();
			switch (this.direction)
			{
			case BoZo_MagicaClothCollider.Direction.X:
				magicaCapsuleCollider.direction = MagicaCapsuleCollider.Direction.X;
				break;
			case BoZo_MagicaClothCollider.Direction.Y:
				magicaCapsuleCollider.direction = MagicaCapsuleCollider.Direction.Y;
				break;
			case BoZo_MagicaClothCollider.Direction.Z:
				magicaCapsuleCollider.direction = MagicaCapsuleCollider.Direction.Z;
				break;
			}
			magicaCapsuleCollider.reverseDirection = this.reverseDirection;
			magicaCapsuleCollider.SetSize(this.startRadius, this.endRadius, this.length);
			magicaCapsuleCollider.center = this.center;
			magicaCapsuleCollider.UpdateParameters();
		}

		// Token: 0x04003C80 RID: 15488
		[SerializeField]
		private BoZo_MagicaClothCollider.Direction direction;

		// Token: 0x04003C81 RID: 15489
		[SerializeField]
		private bool reverseDirection;

		// Token: 0x04003C82 RID: 15490
		[SerializeField]
		private float length;

		// Token: 0x04003C83 RID: 15491
		[SerializeField]
		private float startRadius;

		// Token: 0x04003C84 RID: 15492
		[SerializeField]
		private float endRadius;

		// Token: 0x04003C85 RID: 15493
		[SerializeField]
		private Vector3 center;

		// Token: 0x020008C9 RID: 2249
		private enum Direction
		{
			// Token: 0x04003C87 RID: 15495
			X,
			// Token: 0x04003C88 RID: 15496
			Y,
			// Token: 0x04003C89 RID: 15497
			Z
		}
	}
}
