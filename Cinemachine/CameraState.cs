using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200002D RID: 45
	public struct CameraState
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00010727 File Offset: 0x0000E927
		public bool HasLookAt
		{
			get
			{
				return this.ReferenceLookAt == this.ReferenceLookAt;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0001073A File Offset: 0x0000E93A
		public Vector3 CorrectedPosition
		{
			get
			{
				return this.RawPosition + this.PositionCorrection;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0001074D File Offset: 0x0000E94D
		public Quaternion CorrectedOrientation
		{
			get
			{
				return this.RawOrientation * this.OrientationCorrection;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00010760 File Offset: 0x0000E960
		public Vector3 FinalPosition
		{
			get
			{
				return this.RawPosition + this.PositionCorrection;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00010773 File Offset: 0x0000E973
		public Quaternion FinalOrientation
		{
			get
			{
				if (Mathf.Abs(this.Lens.Dutch) > 0.0001f)
				{
					return this.CorrectedOrientation * Quaternion.AngleAxis(this.Lens.Dutch, Vector3.forward);
				}
				return this.CorrectedOrientation;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600021C RID: 540 RVA: 0x000107B4 File Offset: 0x0000E9B4
		public static CameraState Default
		{
			get
			{
				return new CameraState
				{
					Lens = LensSettings.Default,
					ReferenceUp = Vector3.up,
					ReferenceLookAt = CameraState.kNoPoint,
					RawPosition = Vector3.zero,
					RawOrientation = Quaternion.identity,
					ShotQuality = 1f,
					PositionCorrection = Vector3.zero,
					OrientationCorrection = Quaternion.identity,
					PositionDampingBypass = Vector3.zero,
					BlendHint = CameraState.BlendHintValue.Nothing
				};
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0001083E File Offset: 0x0000EA3E
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00010846 File Offset: 0x0000EA46
		public int NumCustomBlendables { readonly get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x00010850 File Offset: 0x0000EA50
		public CameraState.CustomBlendable GetCustomBlendable(int index)
		{
			switch (index)
			{
			case 0:
				return this.mCustom0;
			case 1:
				return this.mCustom1;
			case 2:
				return this.mCustom2;
			case 3:
				return this.mCustom3;
			default:
				index -= 4;
				if (this.m_CustomOverflow != null && index < this.m_CustomOverflow.Count)
				{
					return this.m_CustomOverflow[index];
				}
				return new CameraState.CustomBlendable(null, 0f);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000108C4 File Offset: 0x0000EAC4
		private int FindCustomBlendable(Object custom)
		{
			if (this.mCustom0.m_Custom == custom)
			{
				return 0;
			}
			if (this.mCustom1.m_Custom == custom)
			{
				return 1;
			}
			if (this.mCustom2.m_Custom == custom)
			{
				return 2;
			}
			if (this.mCustom3.m_Custom == custom)
			{
				return 3;
			}
			if (this.m_CustomOverflow != null)
			{
				for (int i = 0; i < this.m_CustomOverflow.Count; i++)
				{
					if (this.m_CustomOverflow[i].m_Custom == custom)
					{
						return i + 4;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00010964 File Offset: 0x0000EB64
		public void AddCustomBlendable(CameraState.CustomBlendable b)
		{
			int num = this.FindCustomBlendable(b.m_Custom);
			if (num >= 0)
			{
				b.m_Weight += this.GetCustomBlendable(num).m_Weight;
			}
			else
			{
				int numCustomBlendables = this.NumCustomBlendables;
				this.NumCustomBlendables = numCustomBlendables + 1;
				num = numCustomBlendables;
			}
			switch (num)
			{
			case 0:
				this.mCustom0 = b;
				return;
			case 1:
				this.mCustom1 = b;
				return;
			case 2:
				this.mCustom2 = b;
				return;
			case 3:
				this.mCustom3 = b;
				return;
			default:
				num -= 4;
				if (this.m_CustomOverflow == null)
				{
					this.m_CustomOverflow = new List<CameraState.CustomBlendable>();
				}
				if (num < this.m_CustomOverflow.Count)
				{
					this.m_CustomOverflow[num] = b;
					return;
				}
				this.m_CustomOverflow.Add(b);
				return;
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00010A24 File Offset: 0x0000EC24
		public static CameraState Lerp(CameraState stateA, CameraState stateB, float t)
		{
			t = Mathf.Clamp01(t);
			float num = t;
			CameraState cameraState = default(CameraState);
			if ((stateA.BlendHint & stateB.BlendHint & CameraState.BlendHintValue.NoPosition) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.NoPosition;
			}
			if ((stateA.BlendHint & stateB.BlendHint & CameraState.BlendHintValue.NoOrientation) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.NoOrientation;
			}
			if ((stateA.BlendHint & stateB.BlendHint & CameraState.BlendHintValue.NoLens) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.NoLens;
			}
			if (((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.SphericalPositionBlend) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.SphericalPositionBlend;
			}
			if (((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.CylindricalPositionBlend) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.BlendHint |= CameraState.BlendHintValue.CylindricalPositionBlend;
			}
			if (((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.NoLens) == CameraState.BlendHintValue.Nothing)
			{
				cameraState.Lens = LensSettings.Lerp(stateA.Lens, stateB.Lens, t);
			}
			else if ((stateA.BlendHint & stateB.BlendHint & CameraState.BlendHintValue.NoLens) == CameraState.BlendHintValue.Nothing)
			{
				if ((stateA.BlendHint & CameraState.BlendHintValue.NoLens) != CameraState.BlendHintValue.Nothing)
				{
					cameraState.Lens = stateB.Lens;
				}
				else
				{
					cameraState.Lens = stateA.Lens;
				}
			}
			cameraState.ReferenceUp = Vector3.Slerp(stateA.ReferenceUp, stateB.ReferenceUp, t);
			cameraState.ShotQuality = Mathf.Lerp(stateA.ShotQuality, stateB.ShotQuality, t);
			cameraState.PositionCorrection = CameraState.ApplyPosBlendHint(stateA.PositionCorrection, stateA.BlendHint, stateB.PositionCorrection, stateB.BlendHint, cameraState.PositionCorrection, Vector3.Lerp(stateA.PositionCorrection, stateB.PositionCorrection, t));
			cameraState.OrientationCorrection = CameraState.ApplyRotBlendHint(stateA.OrientationCorrection, stateA.BlendHint, stateB.OrientationCorrection, stateB.BlendHint, cameraState.OrientationCorrection, Quaternion.Slerp(stateA.OrientationCorrection, stateB.OrientationCorrection, t));
			if (!stateA.HasLookAt || !stateB.HasLookAt)
			{
				cameraState.ReferenceLookAt = CameraState.kNoPoint;
			}
			else
			{
				float fieldOfView = stateA.Lens.FieldOfView;
				float fieldOfView2 = stateB.Lens.FieldOfView;
				if (((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.NoLens) == CameraState.BlendHintValue.Nothing && !cameraState.Lens.Orthographic && !Mathf.Approximately(fieldOfView, fieldOfView2))
				{
					LensSettings lens = cameraState.Lens;
					lens.FieldOfView = CameraState.InterpolateFOV(fieldOfView, fieldOfView2, Mathf.Max((stateA.ReferenceLookAt - stateA.CorrectedPosition).magnitude, stateA.Lens.NearClipPlane), Mathf.Max((stateB.ReferenceLookAt - stateB.CorrectedPosition).magnitude, stateB.Lens.NearClipPlane), t);
					cameraState.Lens = lens;
					num = Mathf.Abs((lens.FieldOfView - fieldOfView) / (fieldOfView2 - fieldOfView));
				}
				cameraState.ReferenceLookAt = Vector3.Lerp(stateA.ReferenceLookAt, stateB.ReferenceLookAt, num);
			}
			cameraState.RawPosition = CameraState.ApplyPosBlendHint(stateA.RawPosition, stateA.BlendHint, stateB.RawPosition, stateB.BlendHint, cameraState.RawPosition, cameraState.InterpolatePosition(stateA.RawPosition, stateA.ReferenceLookAt, stateB.RawPosition, stateB.ReferenceLookAt, t));
			if (cameraState.HasLookAt && ((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.RadialAimBlend) != CameraState.BlendHintValue.Nothing)
			{
				cameraState.ReferenceLookAt = cameraState.RawPosition + Vector3.Slerp(stateA.ReferenceLookAt - cameraState.RawPosition, stateB.ReferenceLookAt - cameraState.RawPosition, num);
			}
			Quaternion quaternion = cameraState.RawOrientation;
			if (((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.NoOrientation) == CameraState.BlendHintValue.Nothing)
			{
				Vector3 vector = Vector3.zero;
				if (cameraState.HasLookAt && Quaternion.Angle(stateA.RawOrientation, stateB.RawOrientation) > 0.0001f)
				{
					vector = cameraState.ReferenceLookAt - cameraState.CorrectedPosition;
				}
				if (vector.AlmostZero() || ((stateA.BlendHint | stateB.BlendHint) & CameraState.BlendHintValue.IgnoreLookAtTarget) != CameraState.BlendHintValue.Nothing)
				{
					quaternion = Quaternion.Slerp(stateA.RawOrientation, stateB.RawOrientation, t);
				}
				else
				{
					Vector3 vector2 = cameraState.ReferenceUp;
					vector.Normalize();
					if (Vector3.Cross(vector, vector2).AlmostZero())
					{
						quaternion = Quaternion.Slerp(stateA.RawOrientation, stateB.RawOrientation, t);
						vector2 = quaternion * Vector3.up;
					}
					quaternion = Quaternion.LookRotation(vector, vector2);
					Vector2 vector3 = -stateA.RawOrientation.GetCameraRotationToTarget(stateA.ReferenceLookAt - stateA.CorrectedPosition, vector2);
					Vector2 vector4 = -stateB.RawOrientation.GetCameraRotationToTarget(stateB.ReferenceLookAt - stateB.CorrectedPosition, vector2);
					quaternion = quaternion.ApplyCameraRotation(Vector2.Lerp(vector3, vector4, num), vector2);
				}
			}
			cameraState.RawOrientation = CameraState.ApplyRotBlendHint(stateA.RawOrientation, stateA.BlendHint, stateB.RawOrientation, stateB.BlendHint, cameraState.RawOrientation, quaternion);
			for (int i = 0; i < stateA.NumCustomBlendables; i++)
			{
				CameraState.CustomBlendable customBlendable = stateA.GetCustomBlendable(i);
				customBlendable.m_Weight *= 1f - t;
				if (customBlendable.m_Weight > 0f)
				{
					cameraState.AddCustomBlendable(customBlendable);
				}
			}
			for (int j = 0; j < stateB.NumCustomBlendables; j++)
			{
				CameraState.CustomBlendable customBlendable2 = stateB.GetCustomBlendable(j);
				customBlendable2.m_Weight *= t;
				if (customBlendable2.m_Weight > 0f)
				{
					cameraState.AddCustomBlendable(customBlendable2);
				}
			}
			return cameraState;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00010F84 File Offset: 0x0000F184
		private static float InterpolateFOV(float fovA, float fovB, float dA, float dB, float t)
		{
			float num = dA * 2f * Mathf.Tan(fovA * 0.017453292f / 2f);
			float num2 = dB * 2f * Mathf.Tan(fovB * 0.017453292f / 2f);
			float num3 = Mathf.Lerp(num, num2, t);
			float num4 = 179f;
			float num5 = Mathf.Lerp(dA, dB, t);
			if (num5 > 0.0001f)
			{
				num4 = 2f * Mathf.Atan(num3 / (2f * num5)) * 57.29578f;
			}
			return Mathf.Clamp(num4, Mathf.Min(fovA, fovB), Mathf.Max(fovA, fovB));
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00011016 File Offset: 0x0000F216
		private static Vector3 ApplyPosBlendHint(Vector3 posA, CameraState.BlendHintValue hintA, Vector3 posB, CameraState.BlendHintValue hintB, Vector3 original, Vector3 blended)
		{
			if (((hintA | hintB) & CameraState.BlendHintValue.NoPosition) == CameraState.BlendHintValue.Nothing)
			{
				return blended;
			}
			if ((hintA & hintB & CameraState.BlendHintValue.NoPosition) != CameraState.BlendHintValue.Nothing)
			{
				return original;
			}
			if ((hintA & CameraState.BlendHintValue.NoPosition) != CameraState.BlendHintValue.Nothing)
			{
				return posB;
			}
			return posA;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00011034 File Offset: 0x0000F234
		private static Quaternion ApplyRotBlendHint(Quaternion rotA, CameraState.BlendHintValue hintA, Quaternion rotB, CameraState.BlendHintValue hintB, Quaternion original, Quaternion blended)
		{
			if (((hintA | hintB) & CameraState.BlendHintValue.NoOrientation) == CameraState.BlendHintValue.Nothing)
			{
				return blended;
			}
			if ((hintA & hintB & CameraState.BlendHintValue.NoOrientation) != CameraState.BlendHintValue.Nothing)
			{
				return original;
			}
			if ((hintA & CameraState.BlendHintValue.NoOrientation) != CameraState.BlendHintValue.Nothing)
			{
				return rotB;
			}
			return rotA;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00011054 File Offset: 0x0000F254
		private Vector3 InterpolatePosition(Vector3 posA, Vector3 pivotA, Vector3 posB, Vector3 pivotB, float t)
		{
			if (pivotA == pivotA && pivotB == pivotB)
			{
				if ((this.BlendHint & CameraState.BlendHintValue.CylindricalPositionBlend) != CameraState.BlendHintValue.Nothing)
				{
					Vector3 vector = Vector3.ProjectOnPlane(posA - pivotA, this.ReferenceUp);
					Vector3 vector2 = Vector3.ProjectOnPlane(posB - pivotB, this.ReferenceUp);
					Vector3 vector3 = Vector3.Slerp(vector, vector2, t);
					posA = posA - vector + vector3;
					posB = posB - vector2 + vector3;
				}
				else if ((this.BlendHint & CameraState.BlendHintValue.SphericalPositionBlend) != CameraState.BlendHintValue.Nothing)
				{
					Vector3 vector4 = Vector3.Slerp(posA - pivotA, posB - pivotB, t);
					posA = pivotA + vector4;
					posB = pivotB + vector4;
				}
			}
			return Vector3.Lerp(posA, posB, t);
		}

		// Token: 0x040001A1 RID: 417
		public LensSettings Lens;

		// Token: 0x040001A2 RID: 418
		public Vector3 ReferenceUp;

		// Token: 0x040001A3 RID: 419
		public Vector3 ReferenceLookAt;

		// Token: 0x040001A4 RID: 420
		public static Vector3 kNoPoint = new Vector3(float.NaN, float.NaN, float.NaN);

		// Token: 0x040001A5 RID: 421
		public Vector3 RawPosition;

		// Token: 0x040001A6 RID: 422
		public Quaternion RawOrientation;

		// Token: 0x040001A7 RID: 423
		public Vector3 PositionDampingBypass;

		// Token: 0x040001A8 RID: 424
		public float ShotQuality;

		// Token: 0x040001A9 RID: 425
		public Vector3 PositionCorrection;

		// Token: 0x040001AA RID: 426
		public Quaternion OrientationCorrection;

		// Token: 0x040001AB RID: 427
		public CameraState.BlendHintValue BlendHint;

		// Token: 0x040001AC RID: 428
		private CameraState.CustomBlendable mCustom0;

		// Token: 0x040001AD RID: 429
		private CameraState.CustomBlendable mCustom1;

		// Token: 0x040001AE RID: 430
		private CameraState.CustomBlendable mCustom2;

		// Token: 0x040001AF RID: 431
		private CameraState.CustomBlendable mCustom3;

		// Token: 0x040001B0 RID: 432
		private List<CameraState.CustomBlendable> m_CustomOverflow;

		// Token: 0x020000A3 RID: 163
		public enum BlendHintValue
		{
			// Token: 0x0400035A RID: 858
			Nothing,
			// Token: 0x0400035B RID: 859
			NoPosition,
			// Token: 0x0400035C RID: 860
			NoOrientation,
			// Token: 0x0400035D RID: 861
			NoTransform,
			// Token: 0x0400035E RID: 862
			SphericalPositionBlend,
			// Token: 0x0400035F RID: 863
			CylindricalPositionBlend = 8,
			// Token: 0x04000360 RID: 864
			RadialAimBlend = 16,
			// Token: 0x04000361 RID: 865
			IgnoreLookAtTarget = 32,
			// Token: 0x04000362 RID: 866
			NoLens = 64
		}

		// Token: 0x020000A4 RID: 164
		public struct CustomBlendable
		{
			// Token: 0x06000450 RID: 1104 RVA: 0x0001937A File Offset: 0x0001757A
			public CustomBlendable(Object custom, float weight)
			{
				this.m_Custom = custom;
				this.m_Weight = weight;
			}

			// Token: 0x04000363 RID: 867
			public Object m_Custom;

			// Token: 0x04000364 RID: 868
			public float m_Weight;
		}
	}
}
