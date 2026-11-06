using System;
using Unity.Mathematics;

namespace UnityEngine.Splines
{
	// Token: 0x0200001D RID: 29
	[AddComponentMenu("Splines/Spline Animate")]
	[ExecuteInEditMode]
	public class SplineAnimate : SplineComponent
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005B0D File Offset: 0x00003D0D
		[Obsolete("Use Container instead.", false)]
		public SplineContainer splineContainer
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005B15 File Offset: 0x00003D15
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005B20 File Offset: 0x00003D20
		public SplineContainer Container
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				this.m_Target = value;
				if (base.enabled && this.m_Target != null && this.m_Target.Splines != null)
				{
					for (int i = 0; i < this.m_Target.Splines.Count; i++)
					{
						this.OnSplineChange(this.m_Target.Splines[i], -1, SplineModification.Default);
					}
				}
				this.UpdateStartOffsetT();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00005B91 File Offset: 0x00003D91
		[Obsolete("Use PlayOnAwake instead.", false)]
		public bool playOnAwake
		{
			get
			{
				return this.PlayOnAwake;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005B99 File Offset: 0x00003D99
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005BA1 File Offset: 0x00003DA1
		public bool PlayOnAwake
		{
			get
			{
				return this.m_PlayOnAwake;
			}
			set
			{
				this.m_PlayOnAwake = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005BAA File Offset: 0x00003DAA
		[Obsolete("Use Loop instead.", false)]
		public SplineAnimate.LoopMode loopMode
		{
			get
			{
				return this.Loop;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005BB2 File Offset: 0x00003DB2
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005BBA File Offset: 0x00003DBA
		public SplineAnimate.LoopMode Loop
		{
			get
			{
				return this.m_LoopMode;
			}
			set
			{
				this.m_LoopMode = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005BC3 File Offset: 0x00003DC3
		[Obsolete("Use AnimationMethod instead.", false)]
		public SplineAnimate.Method method
		{
			get
			{
				return this.AnimationMethod;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005BCB File Offset: 0x00003DCB
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00005BD3 File Offset: 0x00003DD3
		public SplineAnimate.Method AnimationMethod
		{
			get
			{
				return this.m_Method;
			}
			set
			{
				this.m_Method = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005BDC File Offset: 0x00003DDC
		[Obsolete("Use Duration instead.", false)]
		public float duration
		{
			get
			{
				return this.Duration;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005BE4 File Offset: 0x00003DE4
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00005BEC File Offset: 0x00003DEC
		public float Duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				if (this.m_Method == SplineAnimate.Method.Time)
				{
					this.m_Duration = Mathf.Max(0f, value);
					this.CalculateMaxSpeed();
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005C0D File Offset: 0x00003E0D
		[Obsolete("Use MaxSpeed instead.", false)]
		public float maxSpeed
		{
			get
			{
				return this.MaxSpeed;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005C15 File Offset: 0x00003E15
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00005C1D File Offset: 0x00003E1D
		public float MaxSpeed
		{
			get
			{
				return this.m_MaxSpeed;
			}
			set
			{
				if (this.m_Method == SplineAnimate.Method.Speed)
				{
					this.m_MaxSpeed = Mathf.Max(0f, value);
					this.CalculateDuration();
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00005C3F File Offset: 0x00003E3F
		[Obsolete("Use Easing instead.", false)]
		public SplineAnimate.EasingMode easingMode
		{
			get
			{
				return this.Easing;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00005C47 File Offset: 0x00003E47
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00005C4F File Offset: 0x00003E4F
		public SplineAnimate.EasingMode Easing
		{
			get
			{
				return this.m_EasingMode;
			}
			set
			{
				this.m_EasingMode = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00005C58 File Offset: 0x00003E58
		[Obsolete("Use Alignment instead.", false)]
		public SplineAnimate.AlignmentMode alignmentMode
		{
			get
			{
				return this.Alignment;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005C60 File Offset: 0x00003E60
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005C68 File Offset: 0x00003E68
		public SplineAnimate.AlignmentMode Alignment
		{
			get
			{
				return this.m_AlignmentMode;
			}
			set
			{
				this.m_AlignmentMode = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005C71 File Offset: 0x00003E71
		[Obsolete("Use ObjectForwardAxis instead.", false)]
		public SplineComponent.AlignAxis objectForwardAxis
		{
			get
			{
				return this.ObjectForwardAxis;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005C79 File Offset: 0x00003E79
		// (set) Token: 0x06000124 RID: 292 RVA: 0x00005C81 File Offset: 0x00003E81
		public SplineComponent.AlignAxis ObjectForwardAxis
		{
			get
			{
				return this.m_ObjectForwardAxis;
			}
			set
			{
				this.m_ObjectUpAxis = this.SetObjectAlignAxis(value, ref this.m_ObjectForwardAxis, this.m_ObjectUpAxis);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00005C9C File Offset: 0x00003E9C
		[Obsolete("Use ObjectUpAxis instead.", false)]
		public SplineComponent.AlignAxis objectUpAxis
		{
			get
			{
				return this.ObjectUpAxis;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005CA4 File Offset: 0x00003EA4
		// (set) Token: 0x06000127 RID: 295 RVA: 0x00005CAC File Offset: 0x00003EAC
		public SplineComponent.AlignAxis ObjectUpAxis
		{
			get
			{
				return this.m_ObjectUpAxis;
			}
			set
			{
				this.m_ObjectForwardAxis = this.SetObjectAlignAxis(value, ref this.m_ObjectUpAxis, this.m_ObjectForwardAxis);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005CC7 File Offset: 0x00003EC7
		[Obsolete("Use NormalizedTime instead.", false)]
		public float normalizedTime
		{
			get
			{
				return this.NormalizedTime;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005CCF File Offset: 0x00003ECF
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public float NormalizedTime
		{
			get
			{
				return this.m_NormalizedTime;
			}
			set
			{
				this.m_NormalizedTime = value;
				if (this.m_LoopMode == SplineAnimate.LoopMode.PingPong)
				{
					int num = (int)(this.m_ElapsedTime / this.m_Duration);
					this.m_ElapsedTime = this.m_Duration * this.m_NormalizedTime + ((num % 2 == 1) ? this.m_Duration : 0f);
				}
				else
				{
					this.m_ElapsedTime = this.m_Duration * this.m_NormalizedTime;
				}
				this.UpdateTransform();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00005D46 File Offset: 0x00003F46
		[Obsolete("Use ElapsedTime instead.", false)]
		public float elapsedTime
		{
			get
			{
				return this.ElapsedTime;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00005D4E File Offset: 0x00003F4E
		// (set) Token: 0x0600012D RID: 301 RVA: 0x00005D56 File Offset: 0x00003F56
		public float ElapsedTime
		{
			get
			{
				return this.m_ElapsedTime;
			}
			set
			{
				this.m_ElapsedTime = value;
				this.CalculateNormalizedTime(0f);
				this.UpdateTransform();
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00005D70 File Offset: 0x00003F70
		// (set) Token: 0x0600012F RID: 303 RVA: 0x00005D78 File Offset: 0x00003F78
		public float StartOffset
		{
			get
			{
				return this.m_StartOffset;
			}
			set
			{
				if (this.m_SplineLength < 0f)
				{
					this.RebuildSplinePath();
				}
				this.m_StartOffset = Mathf.Clamp01(value);
				this.UpdateStartOffsetT();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00005D9F File Offset: 0x00003F9F
		internal float StartOffsetT
		{
			get
			{
				return this.m_StartOffsetT;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005DA7 File Offset: 0x00003FA7
		[Obsolete("Use IsPlaying instead.", false)]
		public bool isPlaying
		{
			get
			{
				return this.IsPlaying;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00005DAF File Offset: 0x00003FAF
		public bool IsPlaying
		{
			get
			{
				return this.m_Playing;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000133 RID: 307 RVA: 0x00005DB8 File Offset: 0x00003FB8
		// (remove) Token: 0x06000134 RID: 308 RVA: 0x00005DF0 File Offset: 0x00003FF0
		[Obsolete("Use Updated instead.", false)]
		public event Action<Vector3, Quaternion> onUpdated;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000135 RID: 309 RVA: 0x00005E28 File Offset: 0x00004028
		// (remove) Token: 0x06000136 RID: 310 RVA: 0x00005E60 File Offset: 0x00004060
		public event Action<Vector3, Quaternion> Updated;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000137 RID: 311 RVA: 0x00005E98 File Offset: 0x00004098
		// (remove) Token: 0x06000138 RID: 312 RVA: 0x00005ED0 File Offset: 0x000040D0
		public event Action Completed;

		// Token: 0x06000139 RID: 313 RVA: 0x00005F05 File Offset: 0x00004105
		private void Awake()
		{
			this.RecalculateAnimationParameters();
			this.Restart(this.m_PlayOnAwake);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005F19 File Offset: 0x00004119
		private void OnEnable()
		{
			this.RecalculateAnimationParameters();
			Spline.Changed += this.OnSplineChange;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005F32 File Offset: 0x00004132
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChange;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005F45 File Offset: 0x00004145
		private void OnValidate()
		{
			this.m_Duration = Mathf.Max(0f, this.m_Duration);
			this.m_MaxSpeed = Mathf.Max(0f, this.m_MaxSpeed);
			this.RecalculateAnimationParameters();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005F7C File Offset: 0x0000417C
		internal void RecalculateAnimationParameters()
		{
			this.RebuildSplinePath();
			SplineAnimate.Method method = this.m_Method;
			if (method == SplineAnimate.Method.Time)
			{
				this.CalculateMaxSpeed();
				return;
			}
			if (method != SplineAnimate.Method.Speed)
			{
				Debug.Log(string.Format("{0} animation method is not supported!", this.m_Method), this);
				return;
			}
			this.CalculateDuration();
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005FC8 File Offset: 0x000041C8
		private bool IsNullOrEmptyContainer()
		{
			if (this.m_Target == null || this.m_Target.Splines.Count == 0)
			{
				if (Application.isPlaying)
				{
					Debug.LogError(SplineAnimate.k_EmptyContainerError, this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005FFF File Offset: 0x000041FF
		public void Play()
		{
			if (this.IsNullOrEmptyContainer())
			{
				return;
			}
			this.m_Playing = true;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006011 File Offset: 0x00004211
		public void Pause()
		{
			this.m_Playing = false;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000601C File Offset: 0x0000421C
		public void Restart(bool autoplay)
		{
			if (this.Container == null)
			{
				return;
			}
			if (this.IsNullOrEmptyContainer())
			{
				return;
			}
			this.m_Playing = false;
			this.m_ElapsedTime = 0f;
			this.NormalizedTime = 0f;
			SplineAnimate.Method method = this.m_Method;
			if (method != SplineAnimate.Method.Time)
			{
				if (method != SplineAnimate.Method.Speed)
				{
					Debug.Log(string.Format("{0} animation method is not supported!", this.m_Method), this);
				}
				else
				{
					this.CalculateDuration();
				}
			}
			else
			{
				this.CalculateMaxSpeed();
			}
			this.UpdateTransform();
			this.UpdateStartOffsetT();
			if (autoplay)
			{
				this.Play();
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000060B0 File Offset: 0x000042B0
		public void Update()
		{
			if (!this.m_Playing || (this.m_LoopMode == SplineAnimate.LoopMode.Once && this.m_NormalizedTime >= 1f))
			{
				return;
			}
			float deltaTime = Time.deltaTime;
			this.CalculateNormalizedTime(deltaTime);
			this.UpdateTransform();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000060F0 File Offset: 0x000042F0
		private void CalculateNormalizedTime(float deltaTime)
		{
			float elapsedTime = this.m_ElapsedTime;
			this.m_ElapsedTime += deltaTime;
			float num = this.m_Duration;
			float num2 = 0f;
			switch (this.m_LoopMode)
			{
			case SplineAnimate.LoopMode.Once:
				num2 = Mathf.Min(this.m_ElapsedTime, num);
				break;
			case SplineAnimate.LoopMode.Loop:
				num2 = this.m_ElapsedTime % num;
				this.UpdateEndReached(elapsedTime, num);
				break;
			case SplineAnimate.LoopMode.LoopEaseInOnce:
				if ((this.m_EasingMode == SplineAnimate.EasingMode.EaseIn || this.m_EasingMode == SplineAnimate.EasingMode.EaseInOut) && this.m_ElapsedTime >= num)
				{
					num *= 0.5f;
				}
				num2 = this.m_ElapsedTime % num;
				this.UpdateEndReached(elapsedTime, num);
				break;
			case SplineAnimate.LoopMode.PingPong:
				num2 = Mathf.PingPong(this.m_ElapsedTime, num);
				this.UpdateEndReached(elapsedTime, num);
				break;
			default:
				Debug.Log(string.Format("{0} animation loop mode is not supported!", this.m_LoopMode), this);
				break;
			}
			num2 /= num;
			if (this.m_LoopMode == SplineAnimate.LoopMode.LoopEaseInOnce)
			{
				if ((this.m_EasingMode == SplineAnimate.EasingMode.EaseIn || this.m_EasingMode == SplineAnimate.EasingMode.EaseInOut) && this.m_ElapsedTime < num)
				{
					num2 = this.EaseInQuadratic(num2);
				}
			}
			else
			{
				switch (this.m_EasingMode)
				{
				case SplineAnimate.EasingMode.EaseIn:
					num2 = this.EaseInQuadratic(num2);
					break;
				case SplineAnimate.EasingMode.EaseOut:
					num2 = this.EaseOutQuadratic(num2);
					break;
				case SplineAnimate.EasingMode.EaseInOut:
					num2 = this.EaseInOutQuadratic(num2);
					break;
				}
			}
			this.m_NormalizedTime = ((num2 == 0f) ? 0f : (Mathf.Floor(this.m_NormalizedTime) + num2));
			if (this.m_NormalizedTime >= 1f && this.m_LoopMode == SplineAnimate.LoopMode.Once)
			{
				this.m_EndReached = true;
				this.m_Playing = false;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000627A File Offset: 0x0000447A
		private void UpdateEndReached(float previousTime, float currentDuration)
		{
			this.m_EndReached = Mathf.FloorToInt(previousTime / currentDuration) < Mathf.FloorToInt(this.m_ElapsedTime / currentDuration);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006299 File Offset: 0x00004499
		private void UpdateStartOffsetT()
		{
			if (this.m_SplinePath != null)
			{
				this.m_StartOffsetT = this.m_SplinePath.ConvertIndexUnit(this.m_StartOffset * this.m_SplineLength, PathIndexUnit.Distance, PathIndexUnit.Normalized);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000062C4 File Offset: 0x000044C4
		private void UpdateTransform()
		{
			if (this.m_Target == null)
			{
				return;
			}
			Vector3 vector;
			Quaternion quaternion;
			this.EvaluatePositionAndRotation(out vector, out quaternion);
			base.transform.position = vector;
			if (this.m_AlignmentMode != SplineAnimate.AlignmentMode.None)
			{
				base.transform.rotation = quaternion;
			}
			Action<Vector3, Quaternion> action = this.onUpdated;
			if (action != null)
			{
				action(vector, quaternion);
			}
			Action<Vector3, Quaternion> updated = this.Updated;
			if (updated != null)
			{
				updated(vector, quaternion);
			}
			if (this.m_EndReached)
			{
				this.m_EndReached = false;
				Action completed = this.Completed;
				if (completed == null)
				{
					return;
				}
				completed();
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006350 File Offset: 0x00004550
		private void EvaluatePositionAndRotation(out Vector3 position, out Quaternion rotation)
		{
			float loopInterpolation = this.GetLoopInterpolation(true);
			position = this.m_Target.EvaluatePosition<SplinePath<Spline>>(this.m_SplinePath, loopInterpolation);
			rotation = Quaternion.identity;
			float3 axis = base.GetAxis(this.m_ObjectForwardAxis);
			float3 axis2 = base.GetAxis(this.m_ObjectUpAxis);
			Quaternion quaternion = Quaternion.Inverse(Quaternion.LookRotation(axis, axis2));
			if (this.m_AlignmentMode == SplineAnimate.AlignmentMode.None)
			{
				rotation = base.transform.rotation;
				return;
			}
			Vector3 vector = Vector3.forward;
			Vector3 vector2 = Vector3.up;
			SplineAnimate.AlignmentMode alignmentMode = this.m_AlignmentMode;
			if (alignmentMode != SplineAnimate.AlignmentMode.SplineElement)
			{
				if (alignmentMode != SplineAnimate.AlignmentMode.SplineObject)
				{
					Debug.Log(string.Format("{0} animation alignment mode is not supported!", this.m_AlignmentMode), this);
				}
				else
				{
					Quaternion rotation2 = this.m_Target.transform.rotation;
					vector = rotation2 * vector;
					vector2 = rotation2 * vector2;
				}
			}
			else
			{
				vector = this.m_Target.EvaluateTangent<SplinePath<Spline>>(this.m_SplinePath, loopInterpolation);
				if (Vector3.Magnitude(vector) <= Mathf.Epsilon)
				{
					if (loopInterpolation < 1f)
					{
						vector = this.m_Target.EvaluateTangent<SplinePath<Spline>>(this.m_SplinePath, Mathf.Min(1f, loopInterpolation + 0.01f));
					}
					else
					{
						vector = this.m_Target.EvaluateTangent<SplinePath<Spline>>(this.m_SplinePath, loopInterpolation - 0.01f);
					}
				}
				vector.Normalize();
				vector2 = this.m_Target.EvaluateUpVector<SplinePath<Spline>>(this.m_SplinePath, loopInterpolation);
			}
			if (math.all(math.isfinite(vector) & math.isfinite(vector2)))
			{
				rotation = Quaternion.LookRotation(vector, vector2) * quaternion;
				return;
			}
			Debug.LogError("Trying to EvaluatePositionAndRotation with invalid parameters. Please check the SplineAnimate component.", this);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006518 File Offset: 0x00004718
		private void CalculateDuration()
		{
			if (this.m_SplineLength < 0f)
			{
				this.RebuildSplinePath();
			}
			if (this.m_SplineLength >= 0f)
			{
				SplineAnimate.EasingMode easingMode = this.m_EasingMode;
				if (easingMode == SplineAnimate.EasingMode.None)
				{
					this.m_Duration = this.m_SplineLength / this.m_MaxSpeed;
					return;
				}
				if (easingMode - SplineAnimate.EasingMode.EaseIn <= 2)
				{
					this.m_Duration = 2f * this.m_SplineLength / this.m_MaxSpeed;
					return;
				}
				Debug.Log(string.Format("{0} animation easing mode is not supported!", this.m_EasingMode), this);
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000065A0 File Offset: 0x000047A0
		private void CalculateMaxSpeed()
		{
			if (this.m_SplineLength < 0f)
			{
				this.RebuildSplinePath();
			}
			if (this.m_SplineLength >= 0f)
			{
				SplineAnimate.EasingMode easingMode = this.m_EasingMode;
				if (easingMode == SplineAnimate.EasingMode.None)
				{
					this.m_MaxSpeed = this.m_SplineLength / this.m_Duration;
					return;
				}
				if (easingMode - SplineAnimate.EasingMode.EaseIn <= 2)
				{
					this.m_MaxSpeed = 2f * this.m_SplineLength / this.m_Duration;
					return;
				}
				Debug.Log(string.Format("{0} animation easing mode is not supported!", this.m_EasingMode), this);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006628 File Offset: 0x00004828
		private void RebuildSplinePath()
		{
			if (this.m_Target != null)
			{
				this.m_SplinePath = new SplinePath<Spline>(this.m_Target.Splines);
				this.m_SplineLength = ((this.m_SplinePath != null) ? this.m_SplinePath.GetLength() : 0f);
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00006679 File Offset: 0x00004879
		private SplineComponent.AlignAxis SetObjectAlignAxis(SplineComponent.AlignAxis newValue, ref SplineComponent.AlignAxis targetAxis, SplineComponent.AlignAxis otherAxis)
		{
			if (newValue == otherAxis)
			{
				otherAxis = targetAxis;
				targetAxis = newValue;
			}
			else if (newValue % SplineComponent.AlignAxis.NegativeXAxis != otherAxis % SplineComponent.AlignAxis.NegativeXAxis)
			{
				targetAxis = newValue;
			}
			return otherAxis;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006694 File Offset: 0x00004894
		private void OnSplineChange(Spline spline, int knotIndex, SplineModification modificationType)
		{
			this.RecalculateAnimationParameters();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000669C File Offset: 0x0000489C
		internal float GetLoopInterpolation(bool offset)
		{
			float num = this.NormalizedTime + (offset ? this.m_StartOffsetT : 0f);
			float num2;
			if (Mathf.Floor(num) == num)
			{
				num2 = Mathf.Clamp01(num);
			}
			else
			{
				num2 = num % 1f;
			}
			return num2;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000066E2 File Offset: 0x000048E2
		private float EaseInQuadratic(float t)
		{
			return t * t;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000066E7 File Offset: 0x000048E7
		private float EaseOutQuadratic(float t)
		{
			return t * (2f - t);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000066F4 File Offset: 0x000048F4
		private float EaseInOutQuadratic(float t)
		{
			float num = 2f * t * t;
			if (t > 0.5f)
			{
				num = 4f * t - num - 1f;
			}
			return num;
		}

		// Token: 0x04000054 RID: 84
		[SerializeField]
		[Tooltip("The target spline to follow.")]
		private SplineContainer m_Target;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		[Tooltip("Enable to have the animation start when the GameObject first loads.")]
		private bool m_PlayOnAwake = true;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		[Tooltip("The loop mode that the animation uses. Loop modes cause the animation to repeat after it finishes. The following loop modes are available:.\nOnce - Traverse the spline once and stop at the end.\nLoop Continuous - Traverse the spline continuously without stopping.\nEase In Then Continuous - Traverse the spline repeatedly without stopping. If Ease In easing is enabled, apply easing to the first loop only.\nPing Pong - Traverse the spline continuously without stopping and reverse direction after an end of the spline is reached.\n")]
		private SplineAnimate.LoopMode m_LoopMode = SplineAnimate.LoopMode.Loop;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		[Tooltip("The method used to animate the GameObject along the spline.\nTime - The spline is traversed in a given amount of seconds.\nSpeed - The spline is traversed at a given maximum speed.")]
		private SplineAnimate.Method m_Method;

		// Token: 0x04000058 RID: 88
		[SerializeField]
		[Tooltip("The period of time that it takes for the GameObject to complete its animation along the spline.")]
		private float m_Duration = 1f;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		[Tooltip("The speed in meters/second that the GameObject animates along the spline at.")]
		private float m_MaxSpeed = 10f;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		[Tooltip("The easing mode used when the GameObject animates along the spline.\nNone - Apply no easing to the animation. The animation speed is linear.\nEase In Only - Apply easing to the beginning of animation.\nEase Out Only - Apply easing to the end of animation.\nEase In-Out - Apply easing to the beginning and end of animation.\n")]
		private SplineAnimate.EasingMode m_EasingMode;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		[Tooltip("The coordinate space that the GameObject's up and forward axes align to.")]
		private SplineAnimate.AlignmentMode m_AlignmentMode = SplineAnimate.AlignmentMode.SplineElement;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		[Tooltip("Which axis of the GameObject is treated as the forward axis.")]
		private SplineComponent.AlignAxis m_ObjectForwardAxis = SplineComponent.AlignAxis.ZAxis;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		[Tooltip("Which axis of the GameObject is treated as the up axis.")]
		private SplineComponent.AlignAxis m_ObjectUpAxis = SplineComponent.AlignAxis.YAxis;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		[Tooltip("Normalized distance [0;1] offset along the spline at which the GameObject should be placed when the animation begins.")]
		private float m_StartOffset;

		// Token: 0x0400005F RID: 95
		[NonSerialized]
		private float m_StartOffsetT;

		// Token: 0x04000060 RID: 96
		private float m_SplineLength = -1f;

		// Token: 0x04000061 RID: 97
		private bool m_Playing;

		// Token: 0x04000062 RID: 98
		private float m_NormalizedTime;

		// Token: 0x04000063 RID: 99
		private float m_ElapsedTime;

		// Token: 0x04000064 RID: 100
		private SplinePath<Spline> m_SplinePath;

		// Token: 0x04000067 RID: 103
		private bool m_EndReached;

		// Token: 0x04000069 RID: 105
		internal static readonly string k_EmptyContainerError = "SplineAnimate does not have a valid SplineContainer set.";

		// Token: 0x02000059 RID: 89
		public enum Method
		{
			// Token: 0x04000121 RID: 289
			Time,
			// Token: 0x04000122 RID: 290
			Speed
		}

		// Token: 0x0200005A RID: 90
		public enum LoopMode
		{
			// Token: 0x04000124 RID: 292
			[InspectorName("Once")]
			Once,
			// Token: 0x04000125 RID: 293
			[InspectorName("Loop Continuous")]
			Loop,
			// Token: 0x04000126 RID: 294
			[InspectorName("Ease In Then Continuous")]
			LoopEaseInOnce,
			// Token: 0x04000127 RID: 295
			[InspectorName("Ping Pong")]
			PingPong
		}

		// Token: 0x0200005B RID: 91
		public enum EasingMode
		{
			// Token: 0x04000129 RID: 297
			[InspectorName("None")]
			None,
			// Token: 0x0400012A RID: 298
			[InspectorName("Ease In Only")]
			EaseIn,
			// Token: 0x0400012B RID: 299
			[InspectorName("Ease Out Only")]
			EaseOut,
			// Token: 0x0400012C RID: 300
			[InspectorName("Ease In-Out")]
			EaseInOut
		}

		// Token: 0x0200005C RID: 92
		public enum AlignmentMode
		{
			// Token: 0x0400012E RID: 302
			[InspectorName("None")]
			None,
			// Token: 0x0400012F RID: 303
			[InspectorName("Spline Element")]
			SplineElement,
			// Token: 0x04000130 RID: 304
			[InspectorName("Spline Object")]
			SplineObject,
			// Token: 0x04000131 RID: 305
			[InspectorName("World Space")]
			World
		}
	}
}
