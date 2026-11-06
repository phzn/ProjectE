using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200005B RID: 91
	[AddComponentMenu("More Mountains/Tools/Gyroscope/MMGyroscope")]
	public class MMGyroscope : MonoBehaviour
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00009764 File Offset: 0x00007964
		public static Quaternion GyroscopeAttitude
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_GyroscopeAttitude;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00009770 File Offset: 0x00007970
		public static Vector3 GyroscopeRotationRate
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_GyroscopeRotationRate;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000977C File Offset: 0x0000797C
		public static Vector3 GyroscopeAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_GyroscopeAcceleration;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00009788 File Offset: 0x00007988
		public static Vector3 InputAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InputAcceleration;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00009794 File Offset: 0x00007994
		public static Vector3 GyroscopeGravity
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_GyroscopeGravity;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x000097A0 File Offset: 0x000079A0
		public static Quaternion InitialGyroscopeAttitude
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InitialGyroscopeAttitude;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x000097AC File Offset: 0x000079AC
		public static Vector3 InitialGyroscopeRotationRate
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InitialGyroscopeRotationRate;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x000097B8 File Offset: 0x000079B8
		public static Vector3 InitialGyroscopeAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InitialGyroscopeAcceleration;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x000097C4 File Offset: 0x000079C4
		public static Vector3 InitialInputAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InitialInputAcceleration;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x000097D0 File Offset: 0x000079D0
		public static Vector3 InitialGyroscopeGravity
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_InitialGyroscopeGravity;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x000097DC File Offset: 0x000079DC
		public static Vector3 CalibratedInputAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_CalibratedInputAcceleration;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x000097E8 File Offset: 0x000079E8
		public static Vector3 CalibratedGyroscopeGravity
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_CalibratedGyroscopeGravity;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000097F4 File Offset: 0x000079F4
		public static Vector3 LerpedCalibratedInputAcceleration
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_LerpedCalibratedInputAcceleration;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00009800 File Offset: 0x00007A00
		public static Vector3 LerpedCalibratedGyroscopeGravity
		{
			get
			{
				MMGyroscope.GetValues();
				return MMGyroscope.m_LerpedCalibratedGyroscopeGravity;
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000980C File Offset: 0x00007A0C
		protected virtual void Start()
		{
			MMGyroscope.TimeScale = this._TimeScale;
			MMGyroscope.Clamps = this._Clamps;
			MMGyroscope.LerpSpeed = this._LerpSpeed;
			MMGyroscope.TestMode = this._TestMode;
			MMGyroscope.GyroscopeInitialization();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000983F File Offset: 0x00007A3F
		public static void GyroscopeInitialization()
		{
			MMGyroscope._gyroscope = Input.gyro;
			MMGyroscope._gyroscope.enabled = true;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009858 File Offset: 0x00007A58
		protected virtual void Update()
		{
			if (!MMGyroscope.GyroscopeActive)
			{
				return;
			}
			this.HandleTestMode();
			MMGyroscope.GetValues();
			this._GyroscopeAttitude = MMGyroscope.m_GyroscopeAttitude;
			this._GyroscopeRotationRate = MMGyroscope.m_GyroscopeRotationRate;
			this._GyroscopeAcceleration = MMGyroscope.m_GyroscopeAcceleration;
			this._InputAcceleration = MMGyroscope.m_InputAcceleration;
			this._GyroscopeGravity = MMGyroscope.m_GyroscopeGravity;
			this._InitialGyroscopeAttitude = MMGyroscope.m_InitialGyroscopeAttitude;
			this._InitialGyroscopeRotationRate = MMGyroscope.m_InitialGyroscopeRotationRate;
			this._InitialGyroscopeAcceleration = MMGyroscope.m_InitialGyroscopeAcceleration;
			this._InitialInputAcceleration = MMGyroscope.m_InitialInputAcceleration;
			this._InitialGyroscopeGravity = MMGyroscope.m_InitialGyroscopeGravity;
			this._CalibratedInputAcceleration = MMGyroscope.m_CalibratedInputAcceleration;
			this._CalibratedGyroscopeGravity = MMGyroscope.m_CalibratedGyroscopeGravity;
			this._LerpedCalibratedInputAcceleration = MMGyroscope.m_LerpedCalibratedInputAcceleration;
			this._LerpedCalibratedGyroscopeGravity = MMGyroscope.m_LerpedCalibratedGyroscopeGravity;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00009912 File Offset: 0x00007B12
		public static void GetValues()
		{
			if ((float)Time.frameCount == MMGyroscope._lastGetValuesAt)
			{
				return;
			}
			MMGyroscope.AutoCalibration();
			MMGyroscope.GetGyroValues();
			MMGyroscope._lastGetValuesAt = (float)Time.frameCount;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00009938 File Offset: 0x00007B38
		private static void GetGyroValues()
		{
			float num = ((MMGyroscope.TimeScale == MMGyroscope.TimeScales.Scaled) ? Time.deltaTime : Time.unscaledDeltaTime);
			MMGyroscope.m_GyroscopeAttitude = MMGyroscope.GyroscopeToUnity(Input.gyro.attitude);
			MMGyroscope.m_GyroscopeRotationRate = Input.gyro.rotationRateUnbiased;
			MMGyroscope.m_GyroscopeAcceleration = Input.gyro.userAcceleration;
			MMGyroscope.GetAccelerationAndGravity();
			MMGyroscope.ClampAcceleration();
			MMGyroscope.m_CalibratedInputAcceleration = MMGyroscope.CalibratedAcceleration(MMGyroscope.m_InputAcceleration, MMGyroscope._accelerationMatrix);
			MMGyroscope.m_CalibratedGyroscopeGravity = MMGyroscope.CalibratedAcceleration(MMGyroscope.m_GyroscopeGravity, MMGyroscope._gravityMatrix);
			MMGyroscope.m_LerpedCalibratedInputAcceleration = Vector3.Lerp(MMGyroscope.m_LerpedCalibratedInputAcceleration, MMGyroscope.m_CalibratedInputAcceleration, num * MMGyroscope.LerpSpeed);
			MMGyroscope.m_LerpedCalibratedGyroscopeGravity = Vector3.Lerp(MMGyroscope.m_LerpedCalibratedGyroscopeGravity, MMGyroscope.m_CalibratedGyroscopeGravity, num * MMGyroscope.LerpSpeed);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000099F4 File Offset: 0x00007BF4
		private static void AutoCalibration()
		{
			if (!MMGyroscope._initialized && Time.time > 0.5f)
			{
				MMGyroscope.m_InitialGyroscopeAttitude = MMGyroscope.GyroscopeToUnity(Input.gyro.attitude);
				MMGyroscope.m_InitialGyroscopeRotationRate = Input.gyro.rotationRateUnbiased;
				MMGyroscope.m_InitialGyroscopeAcceleration = Input.gyro.userAcceleration;
				MMGyroscope.m_InitialInputAcceleration = Input.acceleration;
				MMGyroscope.m_InitialGyroscopeGravity = Input.gyro.gravity;
				MMGyroscope.Calibrate();
				MMGyroscope._initialized = true;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009A6A File Offset: 0x00007C6A
		protected static Quaternion GyroscopeToUnity(Quaternion q)
		{
			return new Quaternion(q.x, q.y, -q.z, -q.w);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009A8C File Offset: 0x00007C8C
		private static void ClampAcceleration()
		{
			MMGyroscope.m_InputAcceleration.x = Mathf.Clamp(MMGyroscope.m_InputAcceleration.x, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
			MMGyroscope.m_InputAcceleration.y = Mathf.Clamp(MMGyroscope.m_InputAcceleration.y, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
			MMGyroscope.m_InputAcceleration.z = Mathf.Clamp(MMGyroscope.m_InputAcceleration.z, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
			MMGyroscope.m_GyroscopeGravity.x = Mathf.Clamp(MMGyroscope.m_GyroscopeGravity.x, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
			MMGyroscope.m_GyroscopeGravity.y = Mathf.Clamp(MMGyroscope.m_GyroscopeGravity.y, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
			MMGyroscope.m_GyroscopeGravity.z = Mathf.Clamp(MMGyroscope.m_GyroscopeGravity.z, MMGyroscope.Clamps.x, MMGyroscope.Clamps.y);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00009BA8 File Offset: 0x00007DA8
		protected virtual void HandleTestMode()
		{
			if (MMGyroscope.TestMode)
			{
				MMGyroscope._testVector.x = this.TestXAcceleration;
				MMGyroscope._testVector.y = this.TestYAcceleration;
				MMGyroscope._testVector.z = this.TestZAcceleration;
				MMGyroscope.m_InputAcceleration = MMGyroscope._testVector;
				MMGyroscope.m_GyroscopeGravity = MMGyroscope._testVector;
				return;
			}
			MMGyroscope.GetAccelerationAndGravity();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009C06 File Offset: 0x00007E06
		private static void GetAccelerationAndGravity()
		{
			if (!MMGyroscope.TestMode)
			{
				MMGyroscope.m_InputAcceleration = Input.acceleration;
				MMGyroscope.m_GyroscopeGravity = Input.gyro.gravity;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009C28 File Offset: 0x00007E28
		private static void Calibrate()
		{
			MMGyroscope._accelerationMatrix = MMGyroscope.CalibrateAcceleration(MMGyroscope.m_InputAcceleration);
			MMGyroscope._gravityMatrix = MMGyroscope.CalibrateAcceleration(Input.gyro.gravity);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00009C50 File Offset: 0x00007E50
		private static Matrix4x4 CalibrateAcceleration(Vector3 initialAcceleration)
		{
			Quaternion quaternion = Quaternion.FromToRotation(-Vector3.forward, initialAcceleration);
			return Matrix4x4.TRS(Vector3.zero, quaternion, Vector3.one).inverse;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009C86 File Offset: 0x00007E86
		private static Vector3 CalibratedAcceleration(Vector3 accelerator, Matrix4x4 matrix)
		{
			return matrix.MultiplyVector(accelerator);
		}

		// Token: 0x04000297 RID: 663
		public static bool GyroscopeActive = true;

		// Token: 0x04000298 RID: 664
		public static MMGyroscope.TimeScales TimeScale = MMGyroscope.TimeScales.Scaled;

		// Token: 0x04000299 RID: 665
		public static Vector2 Clamps = new Vector2(-1f, 1f);

		// Token: 0x0400029A RID: 666
		public static float LerpSpeed = 1f;

		// Token: 0x0400029B RID: 667
		public static bool TestMode;

		// Token: 0x0400029C RID: 668
		[Header("Debug")]
		public bool _TestMode;

		// Token: 0x0400029D RID: 669
		[Range(-1f, 1f)]
		public float TestXAcceleration;

		// Token: 0x0400029E RID: 670
		[Range(-1f, 1f)]
		public float TestYAcceleration;

		// Token: 0x0400029F RID: 671
		[Range(-1f, 1f)]
		public float TestZAcceleration;

		// Token: 0x040002A0 RID: 672
		private static Quaternion m_GyroscopeAttitude;

		// Token: 0x040002A1 RID: 673
		private static Vector3 m_GyroscopeRotationRate;

		// Token: 0x040002A2 RID: 674
		private static Vector3 m_GyroscopeAcceleration;

		// Token: 0x040002A3 RID: 675
		private static Vector3 m_InputAcceleration;

		// Token: 0x040002A4 RID: 676
		private static Vector3 m_GyroscopeGravity;

		// Token: 0x040002A5 RID: 677
		private static Quaternion m_InitialGyroscopeAttitude;

		// Token: 0x040002A6 RID: 678
		private static Vector3 m_InitialGyroscopeRotationRate;

		// Token: 0x040002A7 RID: 679
		private static Vector3 m_InitialGyroscopeAcceleration;

		// Token: 0x040002A8 RID: 680
		private static Vector3 m_InitialInputAcceleration;

		// Token: 0x040002A9 RID: 681
		private static Vector3 m_InitialGyroscopeGravity;

		// Token: 0x040002AA RID: 682
		private static Vector3 m_CalibratedInputAcceleration;

		// Token: 0x040002AB RID: 683
		private static Vector3 m_CalibratedGyroscopeGravity;

		// Token: 0x040002AC RID: 684
		private static Vector3 m_LerpedCalibratedInputAcceleration;

		// Token: 0x040002AD RID: 685
		private static Vector3 m_LerpedCalibratedGyroscopeGravity;

		// Token: 0x040002AE RID: 686
		[Header("Settings")]
		[SerializeField]
		private MMGyroscope.TimeScales _TimeScale;

		// Token: 0x040002AF RID: 687
		[SerializeField]
		private Vector2 _Clamps = new Vector2(-1f, 1f);

		// Token: 0x040002B0 RID: 688
		[SerializeField]
		private float _LerpSpeed = 1f;

		// Token: 0x040002B1 RID: 689
		[Header("Raw Values")]
		[MMReadOnly]
		[SerializeField]
		private Quaternion _GyroscopeAttitude;

		// Token: 0x040002B2 RID: 690
		[MMReadOnly]
		[SerializeField]
		private Vector3 _GyroscopeRotationRate;

		// Token: 0x040002B3 RID: 691
		[MMReadOnly]
		[SerializeField]
		private Vector3 _GyroscopeAcceleration;

		// Token: 0x040002B4 RID: 692
		[MMReadOnly]
		[SerializeField]
		private Vector3 _InputAcceleration;

		// Token: 0x040002B5 RID: 693
		[MMReadOnly]
		[SerializeField]
		private Vector3 _GyroscopeGravity;

		// Token: 0x040002B6 RID: 694
		[Header("AutoCalibration Values")]
		[MMReadOnly]
		[SerializeField]
		private Quaternion _InitialGyroscopeAttitude;

		// Token: 0x040002B7 RID: 695
		[MMReadOnly]
		[SerializeField]
		private Vector3 _InitialGyroscopeRotationRate;

		// Token: 0x040002B8 RID: 696
		[MMReadOnly]
		[SerializeField]
		private Vector3 _InitialGyroscopeAcceleration;

		// Token: 0x040002B9 RID: 697
		[MMReadOnly]
		[SerializeField]
		private Vector3 _InitialInputAcceleration;

		// Token: 0x040002BA RID: 698
		[MMReadOnly]
		[SerializeField]
		private Vector3 _InitialGyroscopeGravity;

		// Token: 0x040002BB RID: 699
		[Header("Relative Values")]
		[MMReadOnly]
		[SerializeField]
		private Vector3 _CalibratedInputAcceleration;

		// Token: 0x040002BC RID: 700
		[MMReadOnly]
		[SerializeField]
		private Vector3 _CalibratedGyroscopeGravity;

		// Token: 0x040002BD RID: 701
		[Header("Lerped Values")]
		[MMReadOnly]
		[SerializeField]
		private Vector3 _LerpedCalibratedInputAcceleration;

		// Token: 0x040002BE RID: 702
		[MMReadOnly]
		[SerializeField]
		private Vector3 _LerpedCalibratedGyroscopeGravity;

		// Token: 0x040002BF RID: 703
		[MMInspectorButton("Calibrate")]
		public bool CalibrateButton;

		// Token: 0x040002C0 RID: 704
		private static Gyroscope _gyroscope;

		// Token: 0x040002C1 RID: 705
		protected static Vector3 _testVector = Vector3.zero;

		// Token: 0x040002C2 RID: 706
		private static bool _initialized = false;

		// Token: 0x040002C3 RID: 707
		private static Matrix4x4 _accelerationMatrix;

		// Token: 0x040002C4 RID: 708
		private static Matrix4x4 _gravityMatrix;

		// Token: 0x040002C5 RID: 709
		private static float _lastGetValuesAt = 0f;

		// Token: 0x020003C9 RID: 969
		public enum TimeScales
		{
			// Token: 0x04001DEA RID: 7658
			Scaled,
			// Token: 0x04001DEB RID: 7659
			Unscaled
		}
	}
}
