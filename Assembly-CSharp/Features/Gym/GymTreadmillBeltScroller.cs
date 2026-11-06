using System;
using UnityEngine;

namespace Features.Gym
{
	// Token: 0x020011B5 RID: 4533
	[DisallowMultipleComponent]
	public sealed class GymTreadmillBeltScroller : MonoBehaviour
	{
		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06008B11 RID: 35601 RVA: 0x0038D5CA File Offset: 0x0038B7CA
		public bool IsRunning
		{
			get
			{
				return this._running;
			}
		}

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x06008B12 RID: 35602 RVA: 0x0038D5D2 File Offset: 0x0038B7D2
		public float CurrentSpeedMultiplier
		{
			get
			{
				return this._speedMultiplier;
			}
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x06008B13 RID: 35603 RVA: 0x0038D5DA File Offset: 0x0038B7DA
		private Transform BeltSpace
		{
			get
			{
				if (!(this.beltSurface != null) || !(this.beltSurface.parent != null))
				{
					return base.transform;
				}
				return this.beltSurface.parent;
			}
		}

		// Token: 0x06008B14 RID: 35604 RVA: 0x0038D60F File Offset: 0x0038B80F
		private void Awake()
		{
			this._station = base.GetComponent<GymExerciseStation>();
			this.CacheAuthoredPositions();
			this.CacheBeltRenderer();
			this.SetRunning(false);
		}

		// Token: 0x06008B15 RID: 35605 RVA: 0x0038D630 File Offset: 0x0038B830
		private void Start()
		{
			GymTreadmillAudio.Register(this);
		}

		// Token: 0x06008B16 RID: 35606 RVA: 0x0038D638 File Offset: 0x0038B838
		private void OnEnable()
		{
			if (this._station == null)
			{
				this._station = base.GetComponent<GymExerciseStation>();
			}
			if (this._ambientRunning)
			{
				this.SetRunning(true);
			}
		}

		// Token: 0x06008B17 RID: 35607 RVA: 0x0038D663 File Offset: 0x0038B863
		private void OnDisable()
		{
			this.StopAndReset();
		}

		// Token: 0x06008B18 RID: 35608 RVA: 0x0038D66C File Offset: 0x0038B86C
		private void Update()
		{
			if (this._station == null)
			{
				this._station = base.GetComponent<GymExerciseStation>();
			}
			bool flag = this._ambientRunning || (this._station != null && this._station.IsExerciseActive);
			if (this._running != flag)
			{
				this.SetRunning(flag);
			}
			this.AdvanceBelt(Time.deltaTime);
		}

		// Token: 0x06008B19 RID: 35609 RVA: 0x0038D6D8 File Offset: 0x0038B8D8
		private void AdvanceBelt(float deltaTime)
		{
			if (!this._running || this.stripes == null || this.stripes.Length == 0 || deltaTime <= 0f)
			{
				return;
			}
			Transform beltSpace = this.BeltSpace;
			float num = Mathf.Max(0.01f, this.travelLength);
			float num2 = num * 0.5f;
			float num3 = this.speed * this._speedMultiplier * deltaTime;
			for (int i = 0; i < this.stripes.Length; i++)
			{
				Transform transform = this.stripes[i];
				if (!(transform == null))
				{
					Vector3 beltLocalPosition = GymTreadmillBeltScroller.GetBeltLocalPosition(transform, beltSpace);
					float num4 = beltLocalPosition.z - this._loopCenterZ - num3;
					beltLocalPosition.z = Mathf.Repeat(num4 + num2, num) - num2 + this._loopCenterZ;
					GymTreadmillBeltScroller.SetBeltLocalPosition(transform, beltSpace, beltLocalPosition);
				}
			}
			if (this.textureScrollScale > 0f)
			{
				this._textureOffset = Mathf.Repeat(this._textureOffset - this.textureScrollScale * this._speedMultiplier * deltaTime, 1f);
				this.ApplyTextureOffset(this._textureOffset);
			}
		}

		// Token: 0x06008B1A RID: 35610 RVA: 0x0038D7E2 File Offset: 0x0038B9E2
		public void SetRunning(bool running)
		{
			this.EnsureCache();
			if (!running)
			{
				this.StopAndReset();
				return;
			}
			this.RestoreAuthoredPositions();
			this.SetStripesActive(true);
			this._running = true;
		}

		// Token: 0x06008B1B RID: 35611 RVA: 0x0038D808 File Offset: 0x0038BA08
		public void SetSpeedMultiplier(float multiplier)
		{
			this._speedMultiplier = Mathf.Clamp(multiplier, 0.25f, 2.5f);
		}

		// Token: 0x06008B1C RID: 35612 RVA: 0x0038D820 File Offset: 0x0038BA20
		public void SetAmbientRunning(bool running, float speedMultiplier = 1f)
		{
			this._ambientRunning = running;
			if (running)
			{
				this.SetSpeedMultiplier(speedMultiplier);
				this.SetRunning(true);
				return;
			}
			bool flag = this._station != null && this._station.IsExerciseActive;
			this.SetRunning(flag);
		}

		// Token: 0x06008B1D RID: 35613 RVA: 0x0038D86A File Offset: 0x0038BA6A
		private void EnsureCache()
		{
			if (!this._cacheValid || this._authoredBeltLocalPositions == null || this._authoredBeltLocalPositions.Length != ((this.stripes != null) ? this.stripes.Length : 0))
			{
				this.CacheAuthoredPositions();
			}
		}

		// Token: 0x06008B1E RID: 35614 RVA: 0x0038D8A0 File Offset: 0x0038BAA0
		private void CacheAuthoredPositions()
		{
			int num = ((this.stripes != null) ? this.stripes.Length : 0);
			this._authoredBeltLocalPositions = new Vector3[num];
			Transform beltSpace = this.BeltSpace;
			bool flag = false;
			float num2 = float.PositiveInfinity;
			float num3 = float.NegativeInfinity;
			for (int i = 0; i < num; i++)
			{
				Transform transform = this.stripes[i];
				if (!(transform == null))
				{
					Vector3 beltLocalPosition = GymTreadmillBeltScroller.GetBeltLocalPosition(transform, beltSpace);
					this._authoredBeltLocalPositions[i] = beltLocalPosition;
					num2 = Mathf.Min(num2, beltLocalPosition.z);
					num3 = Mathf.Max(num3, beltLocalPosition.z);
					flag = true;
				}
			}
			this._loopCenterZ = (flag ? ((num2 + num3) * 0.5f) : 0f);
			this._cacheValid = true;
		}

		// Token: 0x06008B1F RID: 35615 RVA: 0x0038D962 File Offset: 0x0038BB62
		private void CacheBeltRenderer()
		{
			this._beltRenderer = ((this.beltSurface != null) ? this.beltSurface.GetComponent<MeshRenderer>() : null);
			if (this._propertyBlock == null)
			{
				this._propertyBlock = new MaterialPropertyBlock();
			}
		}

		// Token: 0x06008B20 RID: 35616 RVA: 0x0038D999 File Offset: 0x0038BB99
		private void StopAndReset()
		{
			this._running = false;
			this.EnsureCache();
			this.RestoreAuthoredPositions();
			this.SetStripesActive(true);
			this._textureOffset = 0f;
			this.ApplyTextureOffset(0f);
		}

		// Token: 0x06008B21 RID: 35617 RVA: 0x0038D9CC File Offset: 0x0038BBCC
		private void RestoreAuthoredPositions()
		{
			if (!this._cacheValid || this.stripes == null || this._authoredBeltLocalPositions == null)
			{
				return;
			}
			Transform beltSpace = this.BeltSpace;
			int num = Mathf.Min(this.stripes.Length, this._authoredBeltLocalPositions.Length);
			for (int i = 0; i < num; i++)
			{
				Transform transform = this.stripes[i];
				if (transform != null)
				{
					GymTreadmillBeltScroller.SetBeltLocalPosition(transform, beltSpace, this._authoredBeltLocalPositions[i]);
				}
			}
		}

		// Token: 0x06008B22 RID: 35618 RVA: 0x0038DA40 File Offset: 0x0038BC40
		private static Vector3 GetBeltLocalPosition(Transform stripe, Transform beltSpace)
		{
			if (!(stripe.parent == beltSpace))
			{
				return beltSpace.InverseTransformPoint(stripe.position);
			}
			return stripe.localPosition;
		}

		// Token: 0x06008B23 RID: 35619 RVA: 0x0038DA63 File Offset: 0x0038BC63
		private static void SetBeltLocalPosition(Transform stripe, Transform beltSpace, Vector3 beltLocalPosition)
		{
			if (stripe.parent == beltSpace)
			{
				stripe.localPosition = beltLocalPosition;
				return;
			}
			stripe.position = beltSpace.TransformPoint(beltLocalPosition);
		}

		// Token: 0x06008B24 RID: 35620 RVA: 0x0038DA88 File Offset: 0x0038BC88
		private void SetStripesActive(bool active)
		{
			if (this.stripes == null)
			{
				return;
			}
			for (int i = 0; i < this.stripes.Length; i++)
			{
				Transform transform = this.stripes[i];
				if (transform != null && transform.gameObject.activeSelf != active)
				{
					transform.gameObject.SetActive(active);
				}
			}
		}

		// Token: 0x06008B25 RID: 35621 RVA: 0x0038DAE0 File Offset: 0x0038BCE0
		private void ApplyTextureOffset(float offset)
		{
			if (this._beltRenderer == null)
			{
				this.CacheBeltRenderer();
			}
			if (this._beltRenderer == null)
			{
				return;
			}
			this._beltRenderer.GetPropertyBlock(this._propertyBlock);
			Vector4 vector = new Vector4(1f, 4f, 0f, offset);
			this._propertyBlock.SetVector("_BaseMap_ST", vector);
			this._propertyBlock.SetVector("_MainTex_ST", vector);
			this._beltRenderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x04008DF5 RID: 36341
		[SerializeField]
		private Transform beltSurface;

		// Token: 0x04008DF6 RID: 36342
		[SerializeField]
		private Transform[] stripes;

		// Token: 0x04008DF7 RID: 36343
		[SerializeField]
		private float speed = 1.8f;

		// Token: 0x04008DF8 RID: 36344
		[SerializeField]
		[Min(0.01f)]
		private float travelLength = 1.6f;

		// Token: 0x04008DF9 RID: 36345
		private Vector3[] _authoredBeltLocalPositions;

		// Token: 0x04008DFA RID: 36346
		private bool _cacheValid;

		// Token: 0x04008DFB RID: 36347
		private bool _running;

		// Token: 0x04008DFC RID: 36348
		private bool _ambientRunning;

		// Token: 0x04008DFD RID: 36349
		private float _loopCenterZ;

		// Token: 0x04008DFE RID: 36350
		private float _speedMultiplier = 1f;

		// Token: 0x04008DFF RID: 36351
		private float _textureOffset;

		// Token: 0x04008E00 RID: 36352
		private MeshRenderer _beltRenderer;

		// Token: 0x04008E01 RID: 36353
		private MaterialPropertyBlock _propertyBlock;

		// Token: 0x04008E02 RID: 36354
		private GymExerciseStation _station;

		// Token: 0x04008E03 RID: 36355
		[SerializeField]
		[Min(0f)]
		private float textureScrollScale = 0.55f;
	}
}
