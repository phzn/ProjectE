using System;
using ProjectExhibitionist.Core;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class SceneViewFlyCam : MonoBehaviour
{
	// Token: 0x060000EA RID: 234 RVA: 0x00007F60 File Offset: 0x00006160
	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		this.rotationX = eulerAngles.y;
		this.rotationY = eulerAngles.x;
		this.targetPosition = base.transform.position;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00007FA2 File Offset: 0x000061A2
	private void Update()
	{
		this.HandleSpeedChange();
		this.HandleFlyMode();
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00007FB0 File Offset: 0x000061B0
	private void HandleSpeedChange()
	{
		float axis = GameplayInput.GetAxis("Mouse ScrollWheel");
		if (axis != 0f && this.isFlying)
		{
			if (axis > 0f)
			{
				this.moveSpeed *= this.scrollSpeedMultiplier;
			}
			else
			{
				this.moveSpeed /= this.scrollSpeedMultiplier;
			}
			this.moveSpeed = Mathf.Clamp(this.moveSpeed, this.minSpeed, this.maxSpeed);
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00008028 File Offset: 0x00006228
	private void HandleFlyMode()
	{
		if (GameplayInput.GetMouseButtonDown(1))
		{
			this.isFlying = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		if (GameplayInput.GetMouseButtonUp(1))
		{
			this.isFlying = false;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (!this.isFlying)
		{
			return;
		}
		float num = GameplayInput.GetAxis("Mouse X") * this.lookSensitivity;
		float num2 = GameplayInput.GetAxis("Mouse Y") * this.lookSensitivity;
		this.rotationX += num;
		this.rotationY -= num2;
		this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
		base.transform.rotation = Quaternion.Euler(this.rotationY, this.rotationX, 0f);
		float num3 = this.moveSpeed;
		if (GameplayInput.GetKey(KeyCode.LeftShift) || GameplayInput.GetKey(KeyCode.RightShift))
		{
			num3 *= this.fastMoveMultiplier;
		}
		Vector3 vector = Vector3.zero;
		if (GameplayInput.GetKey(KeyCode.W))
		{
			vector += base.transform.forward;
		}
		if (GameplayInput.GetKey(KeyCode.S))
		{
			vector -= base.transform.forward;
		}
		if (GameplayInput.GetKey(KeyCode.A))
		{
			vector -= base.transform.right;
		}
		if (GameplayInput.GetKey(KeyCode.D))
		{
			vector += base.transform.right;
		}
		if (GameplayInput.GetKey(KeyCode.E))
		{
			vector += Vector3.up;
		}
		if (GameplayInput.GetKey(KeyCode.Q))
		{
			vector -= Vector3.up;
		}
		if (vector.sqrMagnitude > 0f)
		{
			vector.Normalize();
			base.transform.position += vector * num3 * Time.unscaledDeltaTime;
		}
	}

	// Token: 0x040000AF RID: 175
	[Header("Movement Settings")]
	[SerializeField]
	private float moveSpeed = 10f;

	// Token: 0x040000B0 RID: 176
	[SerializeField]
	private float fastMoveMultiplier = 3f;

	// Token: 0x040000B1 RID: 177
	[SerializeField]
	private float scrollSpeedMultiplier = 1.2f;

	// Token: 0x040000B2 RID: 178
	[SerializeField]
	private float minSpeed = 0.1f;

	// Token: 0x040000B3 RID: 179
	[SerializeField]
	private float maxSpeed = 100f;

	// Token: 0x040000B4 RID: 180
	[Header("Look Settings")]
	[SerializeField]
	private float lookSensitivity = 2f;

	// Token: 0x040000B5 RID: 181
	[SerializeField]
	private float smoothTime = 0.1f;

	// Token: 0x040000B6 RID: 182
	private float rotationX;

	// Token: 0x040000B7 RID: 183
	private float rotationY;

	// Token: 0x040000B8 RID: 184
	private Vector3 currentVelocity;

	// Token: 0x040000B9 RID: 185
	private Vector3 targetPosition;

	// Token: 0x040000BA RID: 186
	private bool isFlying;
}
