using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E68 RID: 3688
	[DisallowMultipleComponent]
	public class SelfieHeadAdjuster : MonoBehaviour
	{
		// Token: 0x06006E24 RID: 28196 RVA: 0x002C4B8D File Offset: 0x002C2D8D
		public void Initialize(Animator animator)
		{
			this._animator = animator;
		}

		// Token: 0x06006E25 RID: 28197 RVA: 0x002C4B96 File Offset: 0x002C2D96
		public void SetOffset(float yaw, float pitch, float smoothSpeed)
		{
			this._active = true;
			this._targetYaw = yaw;
			this._targetPitch = pitch;
			this._smoothSpeed = smoothSpeed;
		}

		// Token: 0x06006E26 RID: 28198 RVA: 0x002C4BB4 File Offset: 0x002C2DB4
		public void Deactivate()
		{
			this._targetYaw = 0f;
			this._targetPitch = 0f;
		}

		// Token: 0x06006E27 RID: 28199 RVA: 0x002C4BCC File Offset: 0x002C2DCC
		private void Update()
		{
			if (!this._active)
			{
				return;
			}
			float num = this._smoothSpeed * 90f * Time.deltaTime;
			this._yaw = Mathf.MoveTowards(this._yaw, this._targetYaw, num);
			this._pitch = Mathf.MoveTowards(this._pitch, this._targetPitch, num);
			if (Mathf.Abs(this._yaw) < 0.001f && Mathf.Abs(this._pitch) < 0.001f && Mathf.Approximately(this._targetYaw, 0f) && Mathf.Approximately(this._targetPitch, 0f))
			{
				this._active = false;
			}
		}

		// Token: 0x06006E28 RID: 28200 RVA: 0x002C4C74 File Offset: 0x002C2E74
		private void OnAnimatorIK(int layerIndex)
		{
			if (!this._active || this._animator == null)
			{
				return;
			}
			if (Mathf.Abs(this._yaw) < 0.001f && Mathf.Abs(this._pitch) < 0.001f)
			{
				return;
			}
			Quaternion localRotation = this._animator.GetBoneTransform(HumanBodyBones.Head).localRotation;
			this._animator.SetBoneLocalRotation(HumanBodyBones.Head, localRotation * Quaternion.Euler(this._pitch, this._yaw, 0f));
		}

		// Token: 0x040070CA RID: 28874
		private Animator _animator;

		// Token: 0x040070CB RID: 28875
		private bool _active;

		// Token: 0x040070CC RID: 28876
		private float _targetYaw;

		// Token: 0x040070CD RID: 28877
		private float _targetPitch;

		// Token: 0x040070CE RID: 28878
		private float _yaw;

		// Token: 0x040070CF RID: 28879
		private float _pitch;

		// Token: 0x040070D0 RID: 28880
		private float _smoothSpeed = 6f;
	}
}
