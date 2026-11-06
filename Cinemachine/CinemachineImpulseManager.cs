using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000059 RID: 89
	[DocumentationSorting(DocumentationSortingAttribute.Level.API)]
	public class CinemachineImpulseManager
	{
		// Token: 0x06000397 RID: 919 RVA: 0x00016527 File Offset: 0x00014727
		private CinemachineImpulseManager()
		{
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0001652F File Offset: 0x0001472F
		public static CinemachineImpulseManager Instance
		{
			get
			{
				if (CinemachineImpulseManager.sInstance == null)
				{
					CinemachineImpulseManager.sInstance = new CinemachineImpulseManager();
				}
				return CinemachineImpulseManager.sInstance;
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00016547 File Offset: 0x00014747
		[RuntimeInitializeOnLoadMethod]
		private static void InitializeModule()
		{
			if (CinemachineImpulseManager.sInstance != null)
			{
				CinemachineImpulseManager.sInstance.Clear();
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001655C File Offset: 0x0001475C
		internal static float EvaluateDissipationScale(float spread, float normalizedDistance)
		{
			float num = -0.8f + 1.6f * (1f - spread);
			num = (1f - num) * 0.5f;
			float num2 = Mathf.Clamp01(normalizedDistance) / ((1f / Mathf.Clamp01(num) - 2f) * (1f - normalizedDistance) + 1f);
			return 1f - SplineHelpers.Bezier1(num2, 0f, 0f, 1f, 1f);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000165D4 File Offset: 0x000147D4
		public bool GetImpulseAt(Vector3 listenerLocation, bool distance2D, int channelMask, out Vector3 pos, out Quaternion rot)
		{
			bool flag = false;
			pos = Vector3.zero;
			rot = Quaternion.identity;
			if (this.m_ActiveEvents != null)
			{
				for (int i = this.m_ActiveEvents.Count - 1; i >= 0; i--)
				{
					CinemachineImpulseManager.ImpulseEvent impulseEvent = this.m_ActiveEvents[i];
					if (impulseEvent == null || impulseEvent.Expired)
					{
						this.m_ActiveEvents.RemoveAt(i);
						if (impulseEvent != null)
						{
							if (this.m_ExpiredEvents == null)
							{
								this.m_ExpiredEvents = new List<CinemachineImpulseManager.ImpulseEvent>();
							}
							impulseEvent.Clear();
							this.m_ExpiredEvents.Add(impulseEvent);
						}
					}
					else if ((impulseEvent.m_Channel & channelMask) != 0)
					{
						Vector3 zero = Vector3.zero;
						Quaternion identity = Quaternion.identity;
						if (impulseEvent.GetDecayedSignal(listenerLocation, distance2D, out zero, out identity))
						{
							flag = true;
							pos += zero;
							rot *= identity;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600039C RID: 924 RVA: 0x000166C3 File Offset: 0x000148C3
		public float CurrentTime
		{
			get
			{
				if (!this.IgnoreTimeScale)
				{
					return CinemachineCore.CurrentTime;
				}
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000166D8 File Offset: 0x000148D8
		public CinemachineImpulseManager.ImpulseEvent NewImpulseEvent()
		{
			if (this.m_ExpiredEvents == null || this.m_ExpiredEvents.Count == 0)
			{
				return new CinemachineImpulseManager.ImpulseEvent
				{
					m_CustomDissipation = -1f
				};
			}
			CinemachineImpulseManager.ImpulseEvent impulseEvent = this.m_ExpiredEvents[this.m_ExpiredEvents.Count - 1];
			this.m_ExpiredEvents.RemoveAt(this.m_ExpiredEvents.Count - 1);
			return impulseEvent;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001673B File Offset: 0x0001493B
		public void AddImpulseEvent(CinemachineImpulseManager.ImpulseEvent e)
		{
			if (this.m_ActiveEvents == null)
			{
				this.m_ActiveEvents = new List<CinemachineImpulseManager.ImpulseEvent>();
			}
			if (e != null)
			{
				e.m_StartTime = this.CurrentTime;
				this.m_ActiveEvents.Add(e);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001676C File Offset: 0x0001496C
		public void Clear()
		{
			if (this.m_ActiveEvents != null)
			{
				for (int i = 0; i < this.m_ActiveEvents.Count; i++)
				{
					this.m_ActiveEvents[i].Clear();
				}
				this.m_ActiveEvents.Clear();
			}
		}

		// Token: 0x04000275 RID: 629
		private static CinemachineImpulseManager sInstance;

		// Token: 0x04000276 RID: 630
		private const float Epsilon = 0.0001f;

		// Token: 0x04000277 RID: 631
		private List<CinemachineImpulseManager.ImpulseEvent> m_ExpiredEvents;

		// Token: 0x04000278 RID: 632
		private List<CinemachineImpulseManager.ImpulseEvent> m_ActiveEvents;

		// Token: 0x04000279 RID: 633
		public bool IgnoreTimeScale;

		// Token: 0x020000CA RID: 202
		[Serializable]
		public struct EnvelopeDefinition
		{
			// Token: 0x0600048E RID: 1166 RVA: 0x0001A64C File Offset: 0x0001884C
			public static CinemachineImpulseManager.EnvelopeDefinition Default()
			{
				return new CinemachineImpulseManager.EnvelopeDefinition
				{
					m_DecayTime = 0.7f,
					m_SustainTime = 0.2f,
					m_ScaleWithImpact = true
				};
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x0600048F RID: 1167 RVA: 0x0001A682 File Offset: 0x00018882
			public float Duration
			{
				get
				{
					if (this.m_HoldForever)
					{
						return -1f;
					}
					return this.m_AttackTime + this.m_SustainTime + this.m_DecayTime;
				}
			}

			// Token: 0x06000490 RID: 1168 RVA: 0x0001A6A8 File Offset: 0x000188A8
			public float GetValueAt(float offset)
			{
				if (offset >= 0f)
				{
					if (offset < this.m_AttackTime && this.m_AttackTime > 0.0001f)
					{
						if (this.m_AttackShape == null || this.m_AttackShape.length < 2)
						{
							return Damper.Damp(1f, this.m_AttackTime, offset);
						}
						return this.m_AttackShape.Evaluate(offset / this.m_AttackTime);
					}
					else
					{
						offset -= this.m_AttackTime;
						if (this.m_HoldForever || offset < this.m_SustainTime)
						{
							return 1f;
						}
						offset -= this.m_SustainTime;
						if (offset < this.m_DecayTime && this.m_DecayTime > 0.0001f)
						{
							if (this.m_DecayShape == null || this.m_DecayShape.length < 2)
							{
								return 1f - Damper.Damp(1f, this.m_DecayTime, offset);
							}
							return this.m_DecayShape.Evaluate(offset / this.m_DecayTime);
						}
					}
				}
				return 0f;
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x0001A79C File Offset: 0x0001899C
			public void ChangeStopTime(float offset, bool forceNoDecay)
			{
				if (offset < 0f)
				{
					offset = 0f;
				}
				if (offset < this.m_AttackTime)
				{
					this.m_AttackTime = 0f;
				}
				this.m_SustainTime = offset - this.m_AttackTime;
				if (forceNoDecay)
				{
					this.m_DecayTime = 0f;
				}
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x0001A7E8 File Offset: 0x000189E8
			public void Clear()
			{
				this.m_AttackShape = (this.m_DecayShape = null);
				this.m_AttackTime = (this.m_SustainTime = (this.m_DecayTime = 0f));
			}

			// Token: 0x06000493 RID: 1171 RVA: 0x0001A824 File Offset: 0x00018A24
			public void Validate()
			{
				this.m_AttackTime = Mathf.Max(0f, this.m_AttackTime);
				this.m_DecayTime = Mathf.Max(0f, this.m_DecayTime);
				this.m_SustainTime = Mathf.Max(0f, this.m_SustainTime);
			}

			// Token: 0x040003FA RID: 1018
			[Tooltip("Normalized curve defining the shape of the start of the envelope.  If blank a default curve will be used")]
			public AnimationCurve m_AttackShape;

			// Token: 0x040003FB RID: 1019
			[Tooltip("Normalized curve defining the shape of the end of the envelope.  If blank a default curve will be used")]
			public AnimationCurve m_DecayShape;

			// Token: 0x040003FC RID: 1020
			[Tooltip("Duration in seconds of the attack.  Attack curve will be scaled to fit.  Must be >= 0.")]
			public float m_AttackTime;

			// Token: 0x040003FD RID: 1021
			[Tooltip("Duration in seconds of the central fully-scaled part of the envelope.  Must be >= 0.")]
			public float m_SustainTime;

			// Token: 0x040003FE RID: 1022
			[Tooltip("Duration in seconds of the decay.  Decay curve will be scaled to fit.  Must be >= 0.")]
			public float m_DecayTime;

			// Token: 0x040003FF RID: 1023
			[Tooltip("If checked, signal amplitude scaling will also be applied to the time envelope of the signal.  Stronger signals will last longer.")]
			public bool m_ScaleWithImpact;

			// Token: 0x04000400 RID: 1024
			[Tooltip("If true, then duration is infinite.")]
			public bool m_HoldForever;
		}

		// Token: 0x020000CB RID: 203
		public class ImpulseEvent
		{
			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001A874 File Offset: 0x00018A74
			public bool Expired
			{
				get
				{
					float duration = this.m_Envelope.Duration;
					float num = this.m_Radius + this.m_DissipationDistance;
					float num2 = CinemachineImpulseManager.Instance.CurrentTime - num / Mathf.Max(1f, this.m_PropagationSpeed);
					return duration > 0f && this.m_StartTime + duration <= num2;
				}
			}

			// Token: 0x06000495 RID: 1173 RVA: 0x0001A8D1 File Offset: 0x00018AD1
			public void Cancel(float time, bool forceNoDecay)
			{
				this.m_Envelope.m_HoldForever = false;
				this.m_Envelope.ChangeStopTime(time - this.m_StartTime, forceNoDecay);
			}

			// Token: 0x06000496 RID: 1174 RVA: 0x0001A8F4 File Offset: 0x00018AF4
			public float DistanceDecay(float distance)
			{
				float num = Mathf.Max(this.m_Radius, 0f);
				if (distance < num)
				{
					return 1f;
				}
				distance -= num;
				if (distance >= this.m_DissipationDistance)
				{
					return 0f;
				}
				if (this.m_CustomDissipation >= 0f)
				{
					return CinemachineImpulseManager.EvaluateDissipationScale(this.m_CustomDissipation, distance / this.m_DissipationDistance);
				}
				switch (this.m_DissipationMode)
				{
				default:
					return Mathf.Lerp(1f, 0f, distance / this.m_DissipationDistance);
				case CinemachineImpulseManager.ImpulseEvent.DissipationMode.SoftDecay:
					return 0.5f * (1f + Mathf.Cos(3.1415927f * (distance / this.m_DissipationDistance)));
				case CinemachineImpulseManager.ImpulseEvent.DissipationMode.ExponentialDecay:
					return 1f - Damper.Damp(1f, this.m_DissipationDistance, distance);
				}
			}

			// Token: 0x06000497 RID: 1175 RVA: 0x0001A9BC File Offset: 0x00018BBC
			public bool GetDecayedSignal(Vector3 listenerPosition, bool use2D, out Vector3 pos, out Quaternion rot)
			{
				if (this.m_SignalSource != null)
				{
					float num = (use2D ? Vector2.Distance(listenerPosition, this.m_Position) : Vector3.Distance(listenerPosition, this.m_Position));
					float num2 = CinemachineImpulseManager.Instance.CurrentTime - this.m_StartTime - num / Mathf.Max(1f, this.m_PropagationSpeed);
					float num3 = this.m_Envelope.GetValueAt(num2) * this.DistanceDecay(num);
					if (num3 != 0f)
					{
						this.m_SignalSource.GetSignal(num2, out pos, out rot);
						pos *= num3;
						rot = Quaternion.SlerpUnclamped(Quaternion.identity, rot, num3);
						if (this.m_DirectionMode == CinemachineImpulseManager.ImpulseEvent.DirectionMode.RotateTowardSource && num > 0.0001f)
						{
							Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, listenerPosition - this.m_Position);
							if (this.m_Radius > 0.0001f)
							{
								float num4 = Mathf.Clamp01(num / this.m_Radius);
								quaternion = Quaternion.Slerp(quaternion, Quaternion.identity, Mathf.Cos(3.1415927f * num4 / 2f));
							}
							pos = quaternion * pos;
						}
						return true;
					}
				}
				pos = Vector3.zero;
				rot = Quaternion.identity;
				return false;
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x0001AB0C File Offset: 0x00018D0C
			public void Clear()
			{
				this.m_Envelope.Clear();
				this.m_StartTime = 0f;
				this.m_SignalSource = null;
				this.m_Position = Vector3.zero;
				this.m_Channel = 0;
				this.m_Radius = 0f;
				this.m_DissipationDistance = 100f;
				this.m_DissipationMode = CinemachineImpulseManager.ImpulseEvent.DissipationMode.ExponentialDecay;
				this.m_CustomDissipation = -1f;
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x0001AB70 File Offset: 0x00018D70
			internal ImpulseEvent()
			{
			}

			// Token: 0x04000401 RID: 1025
			public float m_StartTime;

			// Token: 0x04000402 RID: 1026
			public CinemachineImpulseManager.EnvelopeDefinition m_Envelope;

			// Token: 0x04000403 RID: 1027
			public ISignalSource6D m_SignalSource;

			// Token: 0x04000404 RID: 1028
			public Vector3 m_Position;

			// Token: 0x04000405 RID: 1029
			public float m_Radius;

			// Token: 0x04000406 RID: 1030
			public CinemachineImpulseManager.ImpulseEvent.DirectionMode m_DirectionMode;

			// Token: 0x04000407 RID: 1031
			public int m_Channel;

			// Token: 0x04000408 RID: 1032
			public CinemachineImpulseManager.ImpulseEvent.DissipationMode m_DissipationMode;

			// Token: 0x04000409 RID: 1033
			public float m_DissipationDistance;

			// Token: 0x0400040A RID: 1034
			public float m_CustomDissipation;

			// Token: 0x0400040B RID: 1035
			public float m_PropagationSpeed;

			// Token: 0x020000F4 RID: 244
			public enum DirectionMode
			{
				// Token: 0x040004BA RID: 1210
				Fixed,
				// Token: 0x040004BB RID: 1211
				RotateTowardSource
			}

			// Token: 0x020000F5 RID: 245
			public enum DissipationMode
			{
				// Token: 0x040004BD RID: 1213
				LinearDecay,
				// Token: 0x040004BE RID: 1214
				SoftDecay,
				// Token: 0x040004BF RID: 1215
				ExponentialDecay
			}
		}
	}
}
