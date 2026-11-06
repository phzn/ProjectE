using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001BB RID: 443
	public class PegasusPoi : MonoBehaviour
	{
		// Token: 0x06000D1E RID: 3358 RVA: 0x0005A85D File Offset: 0x00058A5D
		private void OnDrawGizmosSelected()
		{
			this.DrawGizmos(true);
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0005A866 File Offset: 0x00058A66
		private void OnDrawGizmos()
		{
			this.DrawGizmos(false);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0005A870 File Offset: 0x00058A70
		private void DrawGizmos(bool isSelected)
		{
			if (!isSelected && !this.m_alwaysShowGizmos)
			{
				return;
			}
			if (base.transform.parent.childCount != this.m_manager.m_poiList.Count)
			{
				this.m_manager.InitialiseFlythrough();
			}
			float num = 249.99f;
			if (this.m_nextPoi != null)
			{
				bool flag = true;
				if (this.m_isLastPOI && this.m_manager.m_flythroughType == PegasusConstants.FlythroughType.SingleShot)
				{
					flag = false;
				}
				if (flag)
				{
					float num2 = 0.05f;
					Vector3 vector = this.CalculatePositionSpline(0f);
					float num3 = this.CalculateVelocity(0f);
					for (float num4 = num2; num4 <= 1.02f; num4 += num2)
					{
						Vector3 vector2 = this.CalculatePositionLinear(num4);
						float num5 = this.CalculateVelocity(num4);
						if (this.m_isSelected)
						{
							Gizmos.color = Color.magenta * Color.Lerp(Color.cyan, Color.red, (num3 + num5 / 2f - 0.01f) / num);
						}
						else
						{
							Gizmos.color = Color.Lerp(Color.cyan, Color.red, (num3 + num5 / 2f - 0.01f) / num);
						}
						Gizmos.DrawLine(vector, vector2);
						num3 = num5;
						vector = vector2;
					}
				}
			}
			if (this.m_lookatType == PegasusConstants.LookatType.Target)
			{
				Gizmos.color = Color.Lerp(Color.cyan, Color.red, (this.m_startSpeed - 0.01f) / num);
				Gizmos.DrawLine(base.transform.position, this.m_lookatLocation);
				Gizmos.color = Color.cyan;
				Gizmos.DrawSphere(this.m_lookatLocation, 0.25f);
			}
			Gizmos.color = Color.yellow;
			if (this.m_isLastPOI && this.m_manager.m_flythroughType == PegasusConstants.FlythroughType.SingleShot && this.m_manager.m_flythroughEndAction == PegasusConstants.FlythroughEndAction.PlayNextPegasus && this.m_manager.m_nextPegasus != null)
			{
				PegasusPoi firstPOI = this.m_manager.m_nextPegasus.GetFirstPOI();
				if (firstPOI != null)
				{
					Gizmos.DrawLine(base.transform.position, firstPOI.transform.position);
				}
			}
			Gizmos.DrawSphere(base.transform.position, this.m_manager.m_poiGizmoSize);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005AA97 File Offset: 0x00058C97
		public bool IsSameObject(PegasusPoi poi)
		{
			return !(poi == null) && base.GetInstanceID() == poi.GetInstanceID();
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0005AAB8 File Offset: 0x00058CB8
		public void Initialise(bool updateSegments)
		{
			if (this.m_prevPoi == null || this.m_nextPoi == null)
			{
				return;
			}
			base.transform.position = this.m_manager.GetValidatedPoiPosition(base.transform.position, this.m_heightCheckType);
			this.m_lastRotation = base.transform.rotation.eulerAngles;
			switch (this.m_velocityEasingType)
			{
			case PegasusConstants.EasingType.Linear:
				this.m_velocityEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);
				break;
			case PegasusConstants.EasingType.EaseIn:
				this.m_velocityEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseIn);
				break;
			case PegasusConstants.EasingType.EaseOut:
				this.m_velocityEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseOut);
				break;
			case PegasusConstants.EasingType.EaseInOut:
				this.m_velocityEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseInOut);
				break;
			}
			switch (this.m_rotationEasingType)
			{
			case PegasusConstants.EasingType.Linear:
				this.m_rotationEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);
				break;
			case PegasusConstants.EasingType.EaseIn:
				this.m_rotationEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseIn);
				break;
			case PegasusConstants.EasingType.EaseOut:
				this.m_rotationEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseOut);
				break;
			case PegasusConstants.EasingType.EaseInOut:
				this.m_rotationEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseInOut);
				break;
			}
			switch (this.m_positionEasingType)
			{
			case PegasusConstants.EasingType.Linear:
				this.m_positionEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);
				break;
			case PegasusConstants.EasingType.EaseIn:
				this.m_positionEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseIn);
				break;
			case PegasusConstants.EasingType.EaseOut:
				this.m_positionEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseOut);
				break;
			case PegasusConstants.EasingType.EaseInOut:
				this.m_positionEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseInOut);
				break;
			}
			PegasusConstants.LookatType lookatType = this.m_lookatType;
			if (lookatType != PegasusConstants.LookatType.Path)
			{
				if (lookatType == PegasusConstants.LookatType.Target)
				{
					this.GetRelativeOffsets(base.transform.position, this.m_lookatLocation, out this.m_lookAtDistance, out this.m_lookAtHeight, out this.m_lookAtAngle);
				}
			}
			else
			{
				this.m_lookatLocation = this.CalculatePositionSpline(0.005f);
				this.GetRelativeOffsets(base.transform.position, this.m_lookatLocation, out this.m_lookAtDistance, out this.m_lookAtHeight, out this.m_lookAtAngle);
			}
			if (this.m_autoRollOn && (this.m_manager.m_flythroughType == PegasusConstants.FlythroughType.Looped || (!this.m_isFirstPOI && !this.m_isLastPOI)))
			{
				Vector3 position = this.m_prevPoi.transform.position;
				Vector3 position2 = base.transform.position;
				Vector3 position3 = this.m_nextPoi.transform.position;
				Vector3 vector = position2 - position;
				Quaternion quaternion = Quaternion.identity;
				if (vector != Vector3.zero)
				{
					quaternion = Quaternion.LookRotation(vector);
				}
				Vector3 vector2 = position3 - position2;
				Quaternion quaternion2 = Quaternion.identity;
				if (vector2 != Vector3.zero)
				{
					quaternion2 = Quaternion.LookRotation(vector2);
				}
				float num = quaternion2.eulerAngles.y - quaternion.eulerAngles.y;
				float num2 = Mathf.Clamp(this.m_startSpeed, 0.01f, this.m_manager.m_autoRollMaxSpeed) / this.m_manager.m_autoRollMaxSpeed;
				float num3;
				if (num < 0f)
				{
					num3 = Mathf.Clamp(num, -90f, 0f) / 90f;
				}
				else
				{
					num3 = Mathf.Clamp(num, 0f, 90f) / 90f;
				}
				float num4 = num3 * num2 * this.m_manager.m_autoRollMaxAngle * -1f;
				base.transform.localRotation = Quaternion.Euler(0f, 0f, num4);
				this.m_lastRotation = base.transform.localEulerAngles;
			}
			Vector3 vector3 = this.m_lookatLocation - base.transform.position;
			if (vector3 != Vector3.zero)
			{
				this.m_rotationStart = Quaternion.LookRotation(this.m_lookatLocation - base.transform.position) * base.transform.localRotation;
			}
			else
			{
				this.m_rotationStart = base.transform.localRotation;
			}
			vector3 = this.m_nextPoi.m_lookatLocation - this.m_nextPoi.transform.position;
			if (vector3 != Vector3.zero)
			{
				this.m_rotationEnd = Quaternion.LookRotation(vector3) * this.m_nextPoi.transform.localRotation;
			}
			else
			{
				this.m_rotationEnd = this.m_nextPoi.transform.localRotation;
			}
			switch (this.m_startSpeedType)
			{
			case PegasusConstants.SpeedType.ReallySlow:
				this.m_startSpeed = 0.01f;
				break;
			case PegasusConstants.SpeedType.Slow:
				this.m_startSpeed = 1.4f;
				break;
			case PegasusConstants.SpeedType.Medium:
				this.m_startSpeed = 8f;
				break;
			case PegasusConstants.SpeedType.Fast:
				this.m_startSpeed = 25f;
				break;
			case PegasusConstants.SpeedType.ReallyFast:
				this.m_startSpeed = 70f;
				break;
			case PegasusConstants.SpeedType.Stratospheric:
				this.m_startSpeed = 250f;
				break;
			}
			if (updateSegments)
			{
				this.UpdateSegment();
			}
			this.m_poiTriggers.Clear();
			this.m_poiTriggers.AddRange(base.gameObject.GetComponentsInChildren<TriggerBase>());
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0005AFC8 File Offset: 0x000591C8
		public void OnStartTriggers()
		{
			for (int i = 0; i < this.m_poiTriggers.Count; i++)
			{
				this.m_poiTriggers[i].OnStart(this);
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0005B000 File Offset: 0x00059200
		public void OnUpdateTriggers(float progress)
		{
			for (int i = 0; i < this.m_poiTriggers.Count; i++)
			{
				this.m_poiTriggers[i].OnUpdate(this, progress);
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0005B038 File Offset: 0x00059238
		public void OnEndTriggers()
		{
			for (int i = 0; i < this.m_poiTriggers.Count; i++)
			{
				this.m_poiTriggers[i].OnEnd(this);
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0005B070 File Offset: 0x00059270
		public float GetStartSpeed(PegasusConstants.SpeedType speedType)
		{
			switch (speedType)
			{
			case PegasusConstants.SpeedType.ReallySlow:
				return 0.01f;
			case PegasusConstants.SpeedType.Slow:
				return 1.4f;
			case PegasusConstants.SpeedType.Medium:
				return 8f;
			case PegasusConstants.SpeedType.Fast:
				return 25f;
			case PegasusConstants.SpeedType.ReallyFast:
				return 70f;
			case PegasusConstants.SpeedType.Stratospheric:
				return 250f;
			default:
				return this.m_startSpeed;
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0005B0C8 File Offset: 0x000592C8
		public void UpdateSegment()
		{
			this.m_segmentDistance = 0f;
			this.m_segmentDuration = TimeSpan.Zero;
			this.m_segmentStartTime = TimeSpan.Zero;
			if (!this.m_isFirstPOI)
			{
				this.m_segmentStartTime = this.m_prevPoi.m_segmentStartTime + this.m_prevPoi.m_segmentDuration;
			}
			this.m_poiSteps.Clear();
			if (this.m_manager.m_flythroughType == PegasusConstants.FlythroughType.SingleShot && this.m_manager.GetNextPOI(this, false) == null)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			if (this.m_nextPoi != null)
			{
				int num = 3;
				float num2 = (float)(num * 20);
				float num3 = Vector3.Distance(base.transform.position, this.m_nextPoi.transform.position);
				int num4 = (int)Mathf.Ceil(num2 * num3);
				float num5 = 1f / (float)num4;
				float num6 = 0f;
				float num7 = 0f;
				vector = base.transform.position;
				int i = 1;
				float num8 = 0f;
				float num9 = 0f;
				float num10 = 0f;
				while (i <= num4)
				{
					num8 += num5;
					vector2 = this.CalculatePositionSpline(num8);
					vector2 = this.m_manager.GetValidatedPoiPosition(vector2, this.m_heightCheckType);
					float num11 = Vector3.Distance(vector, vector2);
					this.m_segmentDistance += num11;
					vector = vector2;
					if (PegasusPoi.ApproximatelyEqual(num9, 0f) || num11 < num9)
					{
						num9 = num11;
					}
					if (PegasusPoi.ApproximatelyEqual(num10, 0f) || num11 > num10)
					{
						num10 = num11;
					}
					i++;
				}
				if (this.m_segmentDistance < 2f)
				{
					num *= 3;
				}
				float num12 = 1f / (float)num;
				num12 = this.m_segmentDistance / Mathf.Floor(this.m_segmentDistance / num12);
				vector = base.transform.position;
				this.m_poiSteps.Add(vector);
				i = 1;
				num8 = 0f;
				float num13 = 0f;
				float num14 = 0f;
				while (i <= num4)
				{
					num8 += num5;
					vector2 = this.CalculatePositionSpline(num8);
					vector2 = this.m_manager.GetValidatedPoiPosition(vector2, this.m_heightCheckType);
					float num11 = Vector3.Distance(vector, vector2);
					num6 += num11;
					if (num6 >= num12)
					{
						if (PegasusPoi.ApproximatelyEqual(num13, 0f) || num6 < num13)
						{
							num13 = num6;
						}
						if (PegasusPoi.ApproximatelyEqual(num14, 0f) || num6 > num14)
						{
							num14 = num6;
						}
						while (num6 >= num12)
						{
							this.m_poiSteps.Add(Vector3.Lerp(this.m_poiSteps[this.m_poiSteps.Count - 1], vector2, num12 / num6));
							num6 -= num12;
							num7 += num12;
						}
					}
					vector = vector2;
					i++;
				}
				if ((num7 - this.m_segmentDistance) / num12 < -0.5f)
				{
					this.m_poiSteps.Add(this.m_nextPoi.transform.position);
				}
				else
				{
					this.m_poiSteps[this.m_poiSteps.Count - 1] = this.m_nextPoi.transform.position;
				}
			}
			this.UpdateSegmentDuration();
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0005B3FC File Offset: 0x000595FC
		public void UpdateSegmentDuration()
		{
			this.m_segmentDuration = TimeSpan.Zero;
			Vector3 vector = this.CalculatePositionLinear(0f);
			Vector3 vector2 = Vector3.zero;
			float num = 0f;
			for (float num2 = 0f; num2 < 1f; num2 += 0.05f)
			{
				vector2 = this.CalculatePositionLinear(num2);
				num += Vector3.Distance(vector, vector2) / this.CalculateVelocity(num2);
				vector = vector2;
			}
			this.m_segmentDuration = TimeSpan.FromSeconds((double)num);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x0005B473 File Offset: 0x00059673
		public void CalculateProgress(float percent, out float velocity, out Vector3 position, out Quaternion rotation)
		{
			velocity = this.CalculateVelocity(percent);
			rotation = this.CalculateRotation(percent);
			position = this.CalculatePositionLinear(percent);
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x0005B49C File Offset: 0x0005969C
		public Vector3 CalculatePositionSpline(float percent)
		{
			return PegasusPoi.CatmullRom(this.m_prevPoi.transform.position, base.transform.position, this.m_nextPoi.transform.position, this.m_nextPoi.m_nextPoi.transform.position, percent);
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x0005B4F0 File Offset: 0x000596F0
		public Vector3 CalculatePositionLinear(float percent)
		{
			percent = this.m_positionEasingCalculator(percent, 1f);
			if (this.m_poiSteps.Count == 0)
			{
				return Vector3.zero;
			}
			if (this.m_poiSteps.Count == 1)
			{
				return this.m_poiSteps[0];
			}
			int num = this.m_poiSteps.Count - 1;
			int num2 = (int)(percent * (float)num);
			if (num2 == num)
			{
				return this.m_poiSteps[num2];
			}
			float num3 = percent * (float)num - (float)num2;
			return Vector3.Lerp(this.m_poiSteps[num2], this.m_poiSteps[num2 + 1], num3);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x0005B58B File Offset: 0x0005978B
		public float CalculateVelocity(float percent)
		{
			return Mathf.Lerp(this.m_startSpeed, this.m_nextPoi.m_startSpeed, this.m_velocityEasingCalculator(percent, 1f));
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005B5B4 File Offset: 0x000597B4
		public Quaternion CalculateRotation(float percent)
		{
			return Quaternion.Lerp(this.m_rotationStart, this.m_rotationEnd, this.m_rotationEasingCalculator(percent, 1f));
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x0005B5D8 File Offset: 0x000597D8
		public void GetRelativeOffsets(Vector3 source, Vector3 target, out float targetDistance, out float targetHeight, out float targetAngle)
		{
			targetHeight = this.m_manager.GetValidatedLookatHeightRelativeToMinimum(target, this.m_heightCheckType);
			Vector3 vector = new Vector3(target.x, source.y, target.z);
			targetDistance = Vector3.Distance(source, vector);
			Vector3 vector2 = source - target;
			if (vector2 != Vector3.zero)
			{
				targetAngle = Quaternion.LookRotation(vector2, Vector3.up).eulerAngles.y;
				return;
			}
			targetAngle = 0f;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0005B655 File Offset: 0x00059855
		public static bool ApproximatelyEqual(float a, float b)
		{
			return a == b || Mathf.Abs(a - b) < float.Epsilon;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0005B670 File Offset: 0x00059870
		public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angle)
		{
			Vector3 vector = point - pivot;
			vector = Quaternion.Euler(angle) * vector;
			point = vector + pivot;
			return point;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x0005B69C File Offset: 0x0005989C
		private static float EaseLinear(float time, float duration = 1f)
		{
			return time / duration;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x0005B6A1 File Offset: 0x000598A1
		private static float EaseIn(float time, float duration = 1f)
		{
			return (time /= duration) * time;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0005B6AB File Offset: 0x000598AB
		private static float EaseOut(float time, float duration = 1f)
		{
			return -1f * (time /= duration) * (time - 2f);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0005B6C1 File Offset: 0x000598C1
		private static float EaseInOut(float time, float duration = 1f)
		{
			if ((time /= duration / 2f) < 1f)
			{
				return 0.5f * time * time;
			}
			return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0005B700 File Offset: 0x00059900
		public static Vector3 CatmullRom(Vector3 value1, Vector3 value2, Vector3 value3, Vector3 value4, float amount)
		{
			return new Vector3(PegasusPoi.CalcCatmullRom(value1.x, value2.x, value3.x, value4.x, amount), PegasusPoi.CalcCatmullRom(value1.y, value2.y, value3.y, value4.y, amount), PegasusPoi.CalcCatmullRom(value1.z, value2.z, value3.z, value4.z, amount));
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0005B770 File Offset: 0x00059970
		public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
		{
			result.x = PegasusPoi.CalcCatmullRom(value1.x, value2.x, value3.x, value4.x, amount);
			result.y = PegasusPoi.CalcCatmullRom(value1.y, value2.y, value3.y, value4.y, amount);
			result.z = PegasusPoi.CalcCatmullRom(value1.z, value2.z, value3.z, value4.z, amount);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0005B7F0 File Offset: 0x000599F0
		public static float CalcCatmullRom(float value1, float value2, float value3, float value4, float amount)
		{
			double num = (double)(amount * amount);
			double num2 = num * (double)amount;
			return (float)(0.5 * (2.0 * (double)value2 + (double)((value3 - value1) * amount) + (2.0 * (double)value1 - 5.0 * (double)value2 + 4.0 * (double)value3 - (double)value4) * num + (3.0 * (double)value2 - (double)value1 - 3.0 * (double)value3 + (double)value4) * num2));
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0005B878 File Offset: 0x00059A78
		public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount)
		{
			return new Vector3(PegasusPoi.CalcHermite(value1.x, tangent1.x, value2.x, tangent2.x, amount), PegasusPoi.CalcHermite(value1.y, tangent1.y, value2.y, tangent2.y, amount), PegasusPoi.CalcHermite(value1.z, tangent1.z, value2.z, tangent2.z, amount));
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0005B8E8 File Offset: 0x00059AE8
		public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
		{
			result.x = PegasusPoi.CalcHermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
			result.y = PegasusPoi.CalcHermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
			result.z = PegasusPoi.CalcHermite(value1.z, tangent1.z, value2.z, tangent2.z, amount);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0005B968 File Offset: 0x00059B68
		public static float CalcHermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			double num = (double)value1;
			double num2 = (double)value2;
			double num3 = (double)tangent1;
			double num4 = (double)tangent2;
			double num5 = (double)amount;
			double num6 = num5 * num5 * num5;
			double num7 = num5 * num5;
			double num8;
			if (amount == 0f)
			{
				num8 = (double)value1;
			}
			else if (amount == 1f)
			{
				num8 = (double)value2;
			}
			else
			{
				num8 = (2.0 * num - 2.0 * num2 + num4 + num3) * num6 + (3.0 * num2 - 3.0 * num - 2.0 * num3 - num4) * num7 + num3 * num5 + num;
			}
			return (float)num8;
		}

		// Token: 0x04000D77 RID: 3447
		[Tooltip("The type of POI. Auto generated POI's are subject to automatic deletion. Manual POI's will always be kept.")]
		public PegasusConstants.PoiType m_poiType;

		// Token: 0x04000D78 RID: 3448
		[Tooltip("The mechanism used to check height for the POI.")]
		public PegasusConstants.PoiHeightCheckType m_heightCheckType;

		// Token: 0x04000D79 RID: 3449
		[Tooltip("The lookat type for this POI segement. Changing this will re-generate the lookat location.")]
		public PegasusConstants.LookatType m_lookatType;

		// Token: 0x04000D7A RID: 3450
		[Tooltip("The lookat angle from the POI.")]
		public float m_lookAtAngle;

		// Token: 0x04000D7B RID: 3451
		[Tooltip("The lookat distance from the POI.")]
		public float m_lookAtDistance;

		// Token: 0x04000D7C RID: 3452
		[Tooltip("The lookat height above the ground from the POI.")]
		public float m_lookAtHeight;

		// Token: 0x04000D7D RID: 3453
		[Tooltip("The actual lookat location for the POI segment.")]
		public Vector3 m_lookatLocation = Vector3.zero;

		// Token: 0x04000D7E RID: 3454
		[Tooltip("The start speed type for this segment. The segement will always start at this speed and ease to the next segments start speed.")]
		public PegasusConstants.SpeedType m_startSpeedType = PegasusConstants.SpeedType.Medium;

		// Token: 0x04000D7F RID: 3455
		[Tooltip("The actual start speed for this segment. Speed varies between stat speed for this segment and start speed for next segment.")]
		public float m_startSpeed = 1f;

		// Token: 0x04000D80 RID: 3456
		[HideInInspector]
		public PegasusConstants.EasingType m_rotationEasingType;

		// Token: 0x04000D81 RID: 3457
		[HideInInspector]
		public PegasusConstants.EasingType m_velocityEasingType = PegasusConstants.EasingType.EaseInOut;

		// Token: 0x04000D82 RID: 3458
		[HideInInspector]
		public PegasusConstants.EasingType m_positionEasingType;

		// Token: 0x04000D83 RID: 3459
		[HideInInspector]
		public PegasusManager m_manager;

		// Token: 0x04000D84 RID: 3460
		[HideInInspector]
		public bool m_alwaysShowGizmos = true;

		// Token: 0x04000D85 RID: 3461
		[HideInInspector]
		public float m_segmentDistance;

		// Token: 0x04000D86 RID: 3462
		[HideInInspector]
		public TimeSpan m_segmentStartTime = TimeSpan.Zero;

		// Token: 0x04000D87 RID: 3463
		[HideInInspector]
		public TimeSpan m_segmentDuration = TimeSpan.Zero;

		// Token: 0x04000D88 RID: 3464
		[HideInInspector]
		public int m_segmentIndex;

		// Token: 0x04000D89 RID: 3465
		[HideInInspector]
		public bool m_isFirstPOI = true;

		// Token: 0x04000D8A RID: 3466
		[HideInInspector]
		public bool m_isLastPOI = true;

		// Token: 0x04000D8B RID: 3467
		[HideInInspector]
		public PegasusPoi m_prevPoi;

		// Token: 0x04000D8C RID: 3468
		[HideInInspector]
		public PegasusPoi m_nextPoi;

		// Token: 0x04000D8D RID: 3469
		[HideInInspector]
		public List<Vector3> m_poiSteps = new List<Vector3>();

		// Token: 0x04000D8E RID: 3470
		[HideInInspector]
		public bool m_isSelected;

		// Token: 0x04000D8F RID: 3471
		[HideInInspector]
		public List<TriggerBase> m_poiTriggers = new List<TriggerBase>();

		// Token: 0x04000D90 RID: 3472
		[HideInInspector]
		public bool m_autoRollOn;

		// Token: 0x04000D91 RID: 3473
		[HideInInspector]
		public Vector3 m_lastRotation = Vector3.zero;

		// Token: 0x04000D92 RID: 3474
		private PegasusPoi.Easing m_velocityEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);

		// Token: 0x04000D93 RID: 3475
		private PegasusPoi.Easing m_positionEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);

		// Token: 0x04000D94 RID: 3476
		private PegasusPoi.Easing m_rotationEasingCalculator = new PegasusPoi.Easing(PegasusPoi.EaseLinear);

		// Token: 0x04000D95 RID: 3477
		public Quaternion m_rotationStart = Quaternion.identity;

		// Token: 0x04000D96 RID: 3478
		public Quaternion m_rotationEnd = Quaternion.identity;

		// Token: 0x020001BC RID: 444
		// (Invoke) Token: 0x06000D3D RID: 3389
		private delegate float Easing(float time, float duration = 1f);
	}
}
