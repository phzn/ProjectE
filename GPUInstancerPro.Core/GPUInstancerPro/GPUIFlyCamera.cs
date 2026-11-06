using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000040 RID: 64
	public class GPUIFlyCamera : GPUIInputHandler
	{
		// Token: 0x0600025E RID: 606 RVA: 0x0000CF48 File Offset: 0x0000B148
		protected override void Start()
		{
			base.Start();
			this._inputVector = Vector3.zero;
			this._rotationEuler = base.transform.rotation.eulerAngles;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000CF80 File Offset: 0x0000B180
		private void Update()
		{
			if (base.GetMouseButton(1))
			{
				float axis = base.GetAxis("Mouse Y");
				float axis2 = base.GetAxis("Mouse X");
				if (Mathf.Abs(axis) < 5f && Mathf.Abs(axis2) < 5f)
				{
					this._rotationEuler.x = this._rotationEuler.x - axis * this.rotationSpeed;
					this._rotationEuler.y = this._rotationEuler.y + axis2 * this.rotationSpeed;
					base.transform.eulerAngles = this._rotationEuler;
				}
			}
			this.CalculateInputVector();
			base.transform.Translate(this._inputVector);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D020 File Offset: 0x0000B220
		private void CalculateInputVector()
		{
			this._inputVector.x = 0f;
			this._inputVector.y = 0f;
			this._inputVector.z = 0f;
			if (base.GetKey(KeyCode.W))
			{
				this._inputVector.z = this._inputVector.z + 1f;
			}
			if (base.GetKey(KeyCode.S))
			{
				this._inputVector.z = this._inputVector.z - 1f;
			}
			if (base.GetKey(KeyCode.A))
			{
				this._inputVector.x = this._inputVector.x - 1f;
			}
			if (base.GetKey(KeyCode.D))
			{
				this._inputVector.x = this._inputVector.x + 1f;
			}
			if (base.GetKey(KeyCode.Q))
			{
				this._inputVector.y = this._inputVector.y - 1f;
			}
			if (base.GetKey(KeyCode.E))
			{
				this._inputVector.y = this._inputVector.y + 1f;
			}
			if (base.GetKey(KeyCode.LeftShift))
			{
				this._inputVector *= Time.deltaTime * this.shiftSpeed;
				return;
			}
			this._inputVector *= Time.deltaTime * this.mainSpeed;
		}

		// Token: 0x0400017F RID: 383
		public float mainSpeed = 10f;

		// Token: 0x04000180 RID: 384
		public float shiftSpeed = 30f;

		// Token: 0x04000181 RID: 385
		public float rotationSpeed = 5f;

		// Token: 0x04000182 RID: 386
		private Vector3 _inputVector;

		// Token: 0x04000183 RID: 387
		private Vector3 _rotationEuler;
	}
}
