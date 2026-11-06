using System;
using UnityEngine;

namespace Features.NippleRotor
{
	// Token: 0x02000F52 RID: 3922
	[DisallowMultipleComponent]
	public sealed class NippleRotorBuzz : MonoBehaviour
	{
		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06007590 RID: 30096 RVA: 0x002FA0A1 File Offset: 0x002F82A1
		public float Amplitude
		{
			get
			{
				return this._amplitude;
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06007591 RID: 30097 RVA: 0x002FA0A9 File Offset: 0x002F82A9
		public Vector3 RestLocalPosition
		{
			get
			{
				this.CaptureRestIfNeeded();
				return this._restPosition;
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06007592 RID: 30098 RVA: 0x002FA0B7 File Offset: 0x002F82B7
		public Quaternion RestLocalRotation
		{
			get
			{
				this.CaptureRestIfNeeded();
				return this._restRotation;
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06007593 RID: 30099 RVA: 0x002FA0C5 File Offset: 0x002F82C5
		public Vector3 CurrentOffset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06007594 RID: 30100 RVA: 0x002FA0CD File Offset: 0x002F82CD
		public bool IsAtRest
		{
			get
			{
				return !this._displaced;
			}
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x002FA0D8 File Offset: 0x002F82D8
		private void Awake()
		{
			this.CaptureRestIfNeeded();
			this.EnsureSeed();
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x002FA0E6 File Offset: 0x002F82E6
		private void OnDisable()
		{
			if (this._displaced)
			{
				this.SnapToRest();
			}
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x002FA0F6 File Offset: 0x002F82F6
		private void LateUpdate()
		{
			if (Time.timeScale <= 0f)
			{
				return;
			}
			this.Tick(Time.deltaTime);
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x002FA110 File Offset: 0x002F8310
		public void SetIntensity(float amplitude, float hz, float wobbleDegrees)
		{
			this.CaptureRestIfNeeded();
			if (!NippleRotorBuzz.IsFinite(amplitude) || amplitude <= 0f)
			{
				this._amplitude = 0f;
				return;
			}
			bool flag = this._amplitude <= 0f;
			this._amplitude = amplitude;
			this._hz = (NippleRotorBuzz.IsFinite(hz) ? Mathf.Clamp(hz, 0.5f, 120f) : 0.5f);
			this._wobbleDegrees = (NippleRotorBuzz.IsFinite(wobbleDegrees) ? Mathf.Max(0f, wobbleDegrees) : 0f);
			if (flag)
			{
				this._retargetTimer = 0f;
			}
		}

		// Token: 0x06007599 RID: 30105 RVA: 0x002FA1A8 File Offset: 0x002F83A8
		public void ResetToRest()
		{
			this.CaptureRestIfNeeded();
			this._amplitude = 0f;
			this.SnapToRest();
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x002FA1C4 File Offset: 0x002F83C4
		public void Tick(float deltaTime)
		{
			if (!NippleRotorBuzz.IsFinite(deltaTime) || deltaTime <= 0f)
			{
				return;
			}
			this.CaptureRestIfNeeded();
			if (this._amplitude > 0f)
			{
				this._returnElapsed = 0f;
				this._retargetTimer -= deltaTime;
				if (this._retargetTimer <= 0f)
				{
					this.PickTargets();
					float num = 1f / this._hz;
					this._retargetTimer = ((this._retargetTimer <= -num) ? num : (this._retargetTimer + num));
				}
				float num2 = 1f - Mathf.Exp(-this._hz * 3f * deltaTime);
				this._offset = Vector3.ClampMagnitude(Vector3.Lerp(this._offset, this._targetOffset, num2), this._amplitude);
				this._wobble = Vector3.ClampMagnitude(Vector3.Lerp(this._wobble, this._targetWobble, num2), this._wobbleDegrees);
				this.Apply();
				return;
			}
			if (!this._displaced)
			{
				return;
			}
			this._returnElapsed += deltaTime;
			if (this._returnElapsed >= 0.1f)
			{
				this.SnapToRest();
				return;
			}
			float num3 = 1f - Mathf.Exp(-60f * deltaTime);
			this._offset = Vector3.Lerp(this._offset, Vector3.zero, num3);
			this._wobble = Vector3.Lerp(this._wobble, Vector3.zero, num3);
			this.Apply();
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x002FA326 File Offset: 0x002F8526
		private void PickTargets()
		{
			this._targetOffset = this.InsideUnitSphere() * this._amplitude;
			this._targetWobble = this.InsideUnitSphere() * this._wobbleDegrees;
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x002FA358 File Offset: 0x002F8558
		private void Apply()
		{
			base.transform.localPosition = this._restPosition + this._offset;
			base.transform.localRotation = this._restRotation * Quaternion.Euler(this._wobble);
			this._displaced = true;
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x002FA3AC File Offset: 0x002F85AC
		private void SnapToRest()
		{
			this._offset = (this._targetOffset = Vector3.zero);
			this._wobble = (this._targetWobble = Vector3.zero);
			this._returnElapsed = 0f;
			this._retargetTimer = 0f;
			if (this._restCaptured)
			{
				base.transform.localPosition = this._restPosition;
				base.transform.localRotation = this._restRotation;
			}
			this._displaced = false;
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x002FA428 File Offset: 0x002F8628
		private void CaptureRestIfNeeded()
		{
			if (this._restCaptured)
			{
				return;
			}
			this._restPosition = base.transform.localPosition;
			this._restRotation = base.transform.localRotation;
			this._restCaptured = true;
		}

		// Token: 0x0600759F RID: 30111 RVA: 0x002FA45C File Offset: 0x002F865C
		private void EnsureSeed()
		{
			if (this._random != 0U)
			{
				return;
			}
			this._random = (uint)((base.GetInstanceID() * -1640531535) ^ -1640531527);
			if (this._random == 0U)
			{
				this._random = 1831565813U;
			}
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x002FA494 File Offset: 0x002F8694
		private float NextSigned()
		{
			this.EnsureSeed();
			this._random ^= this._random << 13;
			this._random ^= this._random >> 17;
			this._random ^= this._random << 5;
			return (this._random >> 8) * 1.1920929E-07f - 1f;
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x002FA500 File Offset: 0x002F8700
		private Vector3 InsideUnitSphere()
		{
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector = new Vector3(this.NextSigned(), this.NextSigned(), this.NextSigned());
				if (vector.sqrMagnitude <= 1f)
				{
					return vector;
				}
			}
			return new Vector3(this.NextSigned(), this.NextSigned(), this.NextSigned()) * 0.5773502f;
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool IsFinite(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x0400786A RID: 30826
		private const float ReturnSeconds = 0.1f;

		// Token: 0x0400786B RID: 30827
		private const float ReturnSharpness = 60f;

		// Token: 0x0400786C RID: 30828
		private const float FollowPerPeriod = 3f;

		// Token: 0x0400786D RID: 30829
		private const float MinimumHz = 0.5f;

		// Token: 0x0400786E RID: 30830
		private const float MaximumHz = 120f;

		// Token: 0x0400786F RID: 30831
		private Vector3 _restPosition;

		// Token: 0x04007870 RID: 30832
		private Quaternion _restRotation = Quaternion.identity;

		// Token: 0x04007871 RID: 30833
		private bool _restCaptured;

		// Token: 0x04007872 RID: 30834
		private float _amplitude;

		// Token: 0x04007873 RID: 30835
		private float _hz = 10f;

		// Token: 0x04007874 RID: 30836
		private float _wobbleDegrees;

		// Token: 0x04007875 RID: 30837
		private Vector3 _offset;

		// Token: 0x04007876 RID: 30838
		private Vector3 _targetOffset;

		// Token: 0x04007877 RID: 30839
		private Vector3 _wobble;

		// Token: 0x04007878 RID: 30840
		private Vector3 _targetWobble;

		// Token: 0x04007879 RID: 30841
		private float _retargetTimer;

		// Token: 0x0400787A RID: 30842
		private float _returnElapsed;

		// Token: 0x0400787B RID: 30843
		private bool _displaced;

		// Token: 0x0400787C RID: 30844
		private uint _random;
	}
}
