using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UltimateRadialMenuExample.CharacterInventory2D
{
	// Token: 0x0200017E RID: 382
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour
	{
		// Token: 0x06000BEE RID: 3054 RVA: 0x00052D8B File Offset: 0x00050F8B
		private void Start()
		{
			this.myRigidbody = base.GetComponent<Rigidbody2D>();
			this.mySpriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00052DA8 File Offset: 0x00050FA8
		private void FixedUpdate()
		{
			Vector2 zero = Vector2.zero;
			Keyboard device = InputSystem.GetDevice<Keyboard>();
			zero.x = (float)(device.aKey.isPressed ? (-1) : (device.dKey.isPressed ? 1 : 0));
			zero.y = (float)(device.sKey.isPressed ? (-1) : (device.wKey.isPressed ? 1 : 0));
			if (Mathf.Abs(zero.x) > 0f)
			{
				this.mySpriteRenderer.flipX = Mathf.Sign(zero.x) == -1f;
			}
			Vector3 vector = Camera.main.WorldToViewportPoint(this.myRigidbody.position + zero * this.speed);
			vector.x = Mathf.Clamp(vector.x, 0.05f, 0.95f);
			vector.y = Mathf.Clamp(vector.y, 0.1f, 0.9f);
			this.myRigidbody.MovePosition(Camera.main.ViewportToWorldPoint(vector));
		}

		// Token: 0x04000BFA RID: 3066
		public float speed = 10f;

		// Token: 0x04000BFB RID: 3067
		private Rigidbody2D myRigidbody;

		// Token: 0x04000BFC RID: 3068
		private SpriteRenderer mySpriteRenderer;
	}
}
