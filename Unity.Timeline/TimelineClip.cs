using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace UnityEngine.Timeline
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class TimelineClip : ICurvesOwner, ISerializationCallbackReceiver
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000036A4 File Offset: 0x000018A4
		private void UpgradeToLatestVersion()
		{
			if (this.m_Version < 1)
			{
				TimelineClip.TimelineClipUpgrade.UpgradeClipInFromGlobalToLocal(this);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000036B5 File Offset: 0x000018B5
		internal TimelineClip(TrackAsset parent)
		{
			this.SetParentTrack_Internal(parent);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000036F1 File Offset: 0x000018F1
		public bool hasPreExtrapolation
		{
			get
			{
				return this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None && this.m_PreExtrapolationTime > 0.0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000370E File Offset: 0x0000190E
		public bool hasPostExtrapolation
		{
			get
			{
				return this.m_PostExtrapolationMode != TimelineClip.ClipExtrapolation.None && this.m_PostExtrapolationTime > 0.0;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000372B File Offset: 0x0000192B
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00003760 File Offset: 0x00001960
		public double timeScale
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.SpeedMultiplier))
				{
					return 1.0;
				}
				return Math.Max(TimelineClip.kTimeScaleMin, Math.Min(this.m_TimeScale, TimelineClip.kTimeScaleMax));
			}
			set
			{
				this.UpdateDirty(this.m_TimeScale, value);
				this.m_TimeScale = (this.clipCaps.HasAny(ClipCaps.SpeedMultiplier) ? Math.Max(TimelineClip.kTimeScaleMin, Math.Min(value, TimelineClip.kTimeScaleMax)) : 1.0);
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000037AE File Offset: 0x000019AE
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x000037B8 File Offset: 0x000019B8
		public double start
		{
			get
			{
				return this.m_Start;
			}
			set
			{
				this.UpdateDirty(value, this.m_Start);
				double num = Math.Max(TimelineClip.SanitizeTimeValue(value, this.m_Start), 0.0);
				if (this.m_ParentTrack != null && this.m_Start != num)
				{
					this.m_ParentTrack.OnClipMove();
				}
				this.m_Start = num;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00003816 File Offset: 0x00001A16
		// (set) Token: 0x060000A5 RID: 165 RVA: 0x0000381E File Offset: 0x00001A1E
		public double duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.UpdateDirty(this.m_Duration, value);
				this.m_Duration = Math.Max(TimelineClip.SanitizeTimeValue(value, this.m_Duration), double.Epsilon);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000384D File Offset: 0x00001A4D
		public double end
		{
			get
			{
				return this.m_Start + this.m_Duration;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000385C File Offset: 0x00001A5C
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000387C File Offset: 0x00001A7C
		public double clipIn
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.ClipIn))
				{
					return 0.0;
				}
				return this.m_ClipIn;
			}
			set
			{
				this.UpdateDirty(this.m_ClipIn, value);
				this.m_ClipIn = (this.clipCaps.HasAny(ClipCaps.ClipIn) ? Math.Max(Math.Min(TimelineClip.SanitizeTimeValue(value, this.m_ClipIn), TimelineClip.kMaxTimeValue), 0.0) : 0.0);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000038D9 File Offset: 0x00001AD9
		// (set) Token: 0x060000AA RID: 170 RVA: 0x000038E1 File Offset: 0x00001AE1
		public string displayName
		{
			get
			{
				return this.m_DisplayName;
			}
			set
			{
				this.m_DisplayName = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000038EC File Offset: 0x00001AEC
		public double clipAssetDuration
		{
			get
			{
				IPlayableAsset playableAsset = this.m_Asset as IPlayableAsset;
				if (playableAsset == null)
				{
					return double.MaxValue;
				}
				return playableAsset.duration;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003918 File Offset: 0x00001B18
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00003920 File Offset: 0x00001B20
		public AnimationClip curves
		{
			get
			{
				return this.m_AnimationCurves;
			}
			internal set
			{
				this.m_AnimationCurves = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00003929 File Offset: 0x00001B29
		string ICurvesOwner.defaultCurvesName
		{
			get
			{
				return TimelineClip.kDefaultCurvesName;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003930 File Offset: 0x00001B30
		public bool hasCurves
		{
			get
			{
				return this.m_AnimationCurves != null && !this.m_AnimationCurves.empty;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00003950 File Offset: 0x00001B50
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00003958 File Offset: 0x00001B58
		public Object asset
		{
			get
			{
				return this.m_Asset;
			}
			set
			{
				this.m_Asset = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00003961 File Offset: 0x00001B61
		Object ICurvesOwner.assetOwner
		{
			get
			{
				return this.GetParentTrack();
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003969 File Offset: 0x00001B69
		TrackAsset ICurvesOwner.targetTrack
		{
			get
			{
				return this.GetParentTrack();
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00003971 File Offset: 0x00001B71
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00003974 File Offset: 0x00001B74
		[Obsolete("underlyingAsset property is obsolete. Use asset property instead", true)]
		public Object underlyingAsset
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00003976 File Offset: 0x00001B76
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x0000397E File Offset: 0x00001B7E
		[Obsolete("parentTrack is deprecated and will be removed in a future release. Use GetParentTrack() and TimelineClipExtensions::MoveToTrack() or TimelineClipExtensions::TryMoveToTrack() instead.", false)]
		public TrackAsset parentTrack
		{
			get
			{
				return this.m_ParentTrack;
			}
			set
			{
				this.SetParentTrack_Internal(value);
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003987 File Offset: 0x00001B87
		public TrackAsset GetParentTrack()
		{
			return this.m_ParentTrack;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003990 File Offset: 0x00001B90
		internal void SetParentTrack_Internal(TrackAsset newParentTrack)
		{
			if (this.m_ParentTrack == newParentTrack)
			{
				return;
			}
			if (this.m_ParentTrack != null)
			{
				this.m_ParentTrack.RemoveClip(this);
			}
			this.m_ParentTrack = newParentTrack;
			if (this.m_ParentTrack != null)
			{
				this.m_ParentTrack.AddClip(this);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000039E8 File Offset: 0x00001BE8
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00003A48 File Offset: 0x00001C48
		public double easeInDuration
		{
			get
			{
				double num = (this.hasBlendOut ? (this.duration - this.m_BlendOutDuration) : this.duration);
				if (!this.clipCaps.HasAny(ClipCaps.Blending))
				{
					return 0.0;
				}
				return Math.Min(Math.Max(this.m_EaseInDuration, 0.0), num);
			}
			set
			{
				double num = (this.hasBlendOut ? (this.duration - this.m_BlendOutDuration) : this.duration);
				this.m_EaseInDuration = (this.clipCaps.HasAny(ClipCaps.Blending) ? Math.Max(0.0, Math.Min(TimelineClip.SanitizeTimeValue(value, this.m_EaseInDuration), num)) : 0.0);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00003AB4 File Offset: 0x00001CB4
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003B14 File Offset: 0x00001D14
		public double easeOutDuration
		{
			get
			{
				double num = (this.hasBlendIn ? (this.duration - this.m_BlendInDuration) : this.duration);
				if (!this.clipCaps.HasAny(ClipCaps.Blending))
				{
					return 0.0;
				}
				return Math.Min(Math.Max(this.m_EaseOutDuration, 0.0), num);
			}
			set
			{
				double num = (this.hasBlendIn ? (this.duration - this.m_BlendInDuration) : this.duration);
				this.m_EaseOutDuration = (this.clipCaps.HasAny(ClipCaps.Blending) ? Math.Max(0.0, Math.Min(TimelineClip.SanitizeTimeValue(value, this.m_EaseOutDuration), num)) : 0.0);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00003B7F File Offset: 0x00001D7F
		[Obsolete("Use easeOutTime instead (UnityUpgradable) -> easeOutTime", true)]
		public double eastOutTime
		{
			get
			{
				return this.duration - this.easeOutDuration + this.m_Start;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003B95 File Offset: 0x00001D95
		public double easeOutTime
		{
			get
			{
				return this.duration - this.easeOutDuration + this.m_Start;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00003BAB File Offset: 0x00001DAB
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00003BCC File Offset: 0x00001DCC
		public double blendInDuration
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.Blending))
				{
					return 0.0;
				}
				return this.m_BlendInDuration;
			}
			set
			{
				this.m_BlendInDuration = (this.clipCaps.HasAny(ClipCaps.Blending) ? TimelineClip.SanitizeTimeValue(value, this.m_BlendInDuration) : 0.0);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00003BFA File Offset: 0x00001DFA
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00003C1B File Offset: 0x00001E1B
		public double blendOutDuration
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.Blending))
				{
					return 0.0;
				}
				return this.m_BlendOutDuration;
			}
			set
			{
				this.m_BlendOutDuration = (this.clipCaps.HasAny(ClipCaps.Blending) ? TimelineClip.SanitizeTimeValue(value, this.m_BlendOutDuration) : 0.0);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00003C49 File Offset: 0x00001E49
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00003C51 File Offset: 0x00001E51
		public TimelineClip.BlendCurveMode blendInCurveMode
		{
			get
			{
				return this.m_BlendInCurveMode;
			}
			set
			{
				this.m_BlendInCurveMode = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00003C5A File Offset: 0x00001E5A
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00003C62 File Offset: 0x00001E62
		public TimelineClip.BlendCurveMode blendOutCurveMode
		{
			get
			{
				return this.m_BlendOutCurveMode;
			}
			set
			{
				this.m_BlendOutCurveMode = value;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003C6B File Offset: 0x00001E6B
		public bool hasBlendIn
		{
			get
			{
				return this.clipCaps.HasAny(ClipCaps.Blending) && this.m_BlendInDuration > 0.0;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003C8F File Offset: 0x00001E8F
		public bool hasBlendOut
		{
			get
			{
				return this.clipCaps.HasAny(ClipCaps.Blending) && this.m_BlendOutDuration > 0.0;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003CB3 File Offset: 0x00001EB3
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003CDC File Offset: 0x00001EDC
		public AnimationCurve mixInCurve
		{
			get
			{
				if (this.m_MixInCurve == null || this.m_MixInCurve.length < 2)
				{
					this.m_MixInCurve = TimelineClip.GetDefaultMixInCurve();
				}
				return this.m_MixInCurve;
			}
			set
			{
				this.m_MixInCurve = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00003CE5 File Offset: 0x00001EE5
		public float mixInPercentage
		{
			get
			{
				return (float)(this.mixInDuration / this.duration);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00003CF5 File Offset: 0x00001EF5
		public double mixInDuration
		{
			get
			{
				if (!this.hasBlendIn)
				{
					return this.easeInDuration;
				}
				return this.blendInDuration;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00003D0C File Offset: 0x00001F0C
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00003D35 File Offset: 0x00001F35
		public AnimationCurve mixOutCurve
		{
			get
			{
				if (this.m_MixOutCurve == null || this.m_MixOutCurve.length < 2)
				{
					this.m_MixOutCurve = TimelineClip.GetDefaultMixOutCurve();
				}
				return this.m_MixOutCurve;
			}
			set
			{
				this.m_MixOutCurve = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003D3E File Offset: 0x00001F3E
		public double mixOutTime
		{
			get
			{
				return this.duration - this.mixOutDuration + this.m_Start;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00003D54 File Offset: 0x00001F54
		public double mixOutDuration
		{
			get
			{
				if (!this.hasBlendOut)
				{
					return this.easeOutDuration;
				}
				return this.blendOutDuration;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00003D6B File Offset: 0x00001F6B
		public float mixOutPercentage
		{
			get
			{
				return (float)(this.mixOutDuration / this.duration);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00003D7B File Offset: 0x00001F7B
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00003D83 File Offset: 0x00001F83
		public bool recordable
		{
			get
			{
				return this.m_Recordable;
			}
			internal set
			{
				this.m_Recordable = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00003D8C File Offset: 0x00001F8C
		[Obsolete("exposedParameter is deprecated and will be removed in a future release", true)]
		public List<string> exposedParameters
		{
			get
			{
				List<string> list;
				if ((list = this.m_ExposedParameterNames) == null)
				{
					list = (this.m_ExposedParameterNames = new List<string>());
				}
				return list;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public ClipCaps clipCaps
		{
			get
			{
				ITimelineClipAsset timelineClipAsset = this.asset as ITimelineClipAsset;
				if (timelineClipAsset == null)
				{
					return TimelineClip.kDefaultClipCaps;
				}
				return timelineClipAsset.clipCaps;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003DDC File Offset: 0x00001FDC
		internal int Hash()
		{
			int hashCode = this.m_Start.GetHashCode();
			int hashCode2 = this.m_Duration.GetHashCode();
			int hashCode3 = this.m_TimeScale.GetHashCode();
			int hashCode4 = this.m_ClipIn.GetHashCode();
			int num = (int)this.m_PreExtrapolationMode;
			int hashCode5 = num.GetHashCode();
			num = (int)this.m_PostExtrapolationMode;
			return HashUtility.CombineHash(hashCode, hashCode2, hashCode3, hashCode4, hashCode5, num.GetHashCode());
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003E38 File Offset: 0x00002038
		public float EvaluateMixOut(double time)
		{
			if (!this.clipCaps.HasAny(ClipCaps.Blending))
			{
				return 1f;
			}
			if (this.mixOutDuration > (double)Mathf.Epsilon)
			{
				float num = (float)(time - this.mixOutTime) / (float)this.mixOutDuration;
				return Mathf.Clamp01(this.mixOutCurve.Evaluate(num));
			}
			return 1f;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003E94 File Offset: 0x00002094
		public float EvaluateMixIn(double time)
		{
			if (!this.clipCaps.HasAny(ClipCaps.Blending))
			{
				return 1f;
			}
			if (this.mixInDuration > (double)Mathf.Epsilon)
			{
				float num = (float)(time - this.m_Start) / (float)this.mixInDuration;
				return Mathf.Clamp01(this.mixInCurve.Evaluate(num));
			}
			return 1f;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003EEF File Offset: 0x000020EF
		private static AnimationCurve GetDefaultMixInCurve()
		{
			return AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003F0A File Offset: 0x0000210A
		private static AnimationCurve GetDefaultMixOutCurve()
		{
			return AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00003F28 File Offset: 0x00002128
		public double ToLocalTime(double time)
		{
			if (time < 0.0)
			{
				return time;
			}
			if (this.IsPreExtrapolatedTime(time))
			{
				time = TimelineClip.GetExtrapolatedTime(time - this.m_Start, this.m_PreExtrapolationMode, this.m_Duration);
			}
			else if (this.IsPostExtrapolatedTime(time))
			{
				time = TimelineClip.GetExtrapolatedTime(time - this.m_Start, this.m_PostExtrapolationMode, this.m_Duration);
			}
			else
			{
				time -= this.m_Start;
			}
			time *= this.timeScale;
			time += this.clipIn;
			return time;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00003FAE File Offset: 0x000021AE
		public double ToLocalTimeUnbound(double time)
		{
			return (time - this.m_Start) * this.timeScale + this.clipIn;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003FC6 File Offset: 0x000021C6
		internal double FromLocalTimeUnbound(double time)
		{
			return (time - this.clipIn) / this.timeScale + this.m_Start;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00003FE0 File Offset: 0x000021E0
		public AnimationClip animationClip
		{
			get
			{
				if (this.m_Asset == null)
				{
					return null;
				}
				AnimationPlayableAsset animationPlayableAsset = this.m_Asset as AnimationPlayableAsset;
				if (!(animationPlayableAsset != null))
				{
					return null;
				}
				return animationPlayableAsset.clip;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000401A File Offset: 0x0000221A
		private static double SanitizeTimeValue(double value, double defaultValue)
		{
			if (double.IsInfinity(value) || double.IsNaN(value))
			{
				Debug.LogError("Invalid time value assigned");
				return defaultValue;
			}
			return Math.Max(-TimelineClip.kMaxTimeValue, Math.Min(TimelineClip.kMaxTimeValue, value));
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000404E File Offset: 0x0000224E
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00004066 File Offset: 0x00002266
		public TimelineClip.ClipExtrapolation postExtrapolationMode
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.Extrapolation))
				{
					return TimelineClip.ClipExtrapolation.None;
				}
				return this.m_PostExtrapolationMode;
			}
			internal set
			{
				this.m_PostExtrapolationMode = (this.clipCaps.HasAny(ClipCaps.Extrapolation) ? value : TimelineClip.ClipExtrapolation.None);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004080 File Offset: 0x00002280
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004098 File Offset: 0x00002298
		public TimelineClip.ClipExtrapolation preExtrapolationMode
		{
			get
			{
				if (!this.clipCaps.HasAny(ClipCaps.Extrapolation))
				{
					return TimelineClip.ClipExtrapolation.None;
				}
				return this.m_PreExtrapolationMode;
			}
			internal set
			{
				this.m_PreExtrapolationMode = (this.clipCaps.HasAny(ClipCaps.Extrapolation) ? value : TimelineClip.ClipExtrapolation.None);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000040B2 File Offset: 0x000022B2
		internal void SetPostExtrapolationTime(double time)
		{
			this.m_PostExtrapolationTime = time;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000040BB File Offset: 0x000022BB
		internal void SetPreExtrapolationTime(double time)
		{
			this.m_PreExtrapolationTime = time;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000040C4 File Offset: 0x000022C4
		public bool IsExtrapolatedTime(double sequenceTime)
		{
			return this.IsPreExtrapolatedTime(sequenceTime) || this.IsPostExtrapolatedTime(sequenceTime);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000040D8 File Offset: 0x000022D8
		public bool IsPreExtrapolatedTime(double sequenceTime)
		{
			return this.preExtrapolationMode != TimelineClip.ClipExtrapolation.None && sequenceTime < this.m_Start && sequenceTime >= this.m_Start - this.m_PreExtrapolationTime;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004100 File Offset: 0x00002300
		public bool IsPostExtrapolatedTime(double sequenceTime)
		{
			return this.postExtrapolationMode != TimelineClip.ClipExtrapolation.None && sequenceTime > this.end && sequenceTime - this.end < this.m_PostExtrapolationTime;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004125 File Offset: 0x00002325
		public double extrapolatedStart
		{
			get
			{
				if (this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					return this.m_Start - this.m_PreExtrapolationTime;
				}
				return this.m_Start;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004144 File Offset: 0x00002344
		public double extrapolatedDuration
		{
			get
			{
				double num = this.m_Duration;
				if (this.m_PostExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					num += Math.Min(this.m_PostExtrapolationTime, TimelineClip.kMaxTimeValue);
				}
				if (this.m_PreExtrapolationMode != TimelineClip.ClipExtrapolation.None)
				{
					num += this.m_PreExtrapolationTime;
				}
				return num;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004188 File Offset: 0x00002388
		private static double GetExtrapolatedTime(double time, TimelineClip.ClipExtrapolation mode, double duration)
		{
			if (duration == 0.0)
			{
				return 0.0;
			}
			switch (mode)
			{
			case TimelineClip.ClipExtrapolation.Hold:
				if (time < 0.0)
				{
					return 0.0;
				}
				if (time > duration)
				{
					return duration;
				}
				break;
			case TimelineClip.ClipExtrapolation.Loop:
				if (time < 0.0)
				{
					time = duration - -time % duration;
				}
				else if (time > duration)
				{
					time %= duration;
				}
				break;
			case TimelineClip.ClipExtrapolation.PingPong:
				if (time < 0.0)
				{
					time = duration * 2.0 - -time % (duration * 2.0);
					time = duration - Math.Abs(time - duration);
				}
				else
				{
					time %= duration * 2.0;
					time = duration - Math.Abs(time - duration);
				}
				break;
			}
			return time;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004259 File Offset: 0x00002459
		public void CreateCurves(string curvesClipName)
		{
			if (this.m_AnimationCurves != null)
			{
				return;
			}
			this.m_AnimationCurves = TimelineCreateUtilities.CreateAnimationClipForTrack(string.IsNullOrEmpty(curvesClipName) ? TimelineClip.kDefaultCurvesName : curvesClipName, this.GetParentTrack(), true);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000428C File Offset: 0x0000248C
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			this.m_Version = 1;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004295 File Offset: 0x00002495
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (this.m_Version < 1)
			{
				this.UpgradeToLatestVersion();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000042A8 File Offset: 0x000024A8
		public override string ToString()
		{
			return UnityString.Format("{0} ({1:F2}, {2:F2}):{3:F2} | {4}", new object[]
			{
				this.displayName,
				this.start,
				this.end,
				this.clipIn,
				this.GetParentTrack()
			});
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004304 File Offset: 0x00002504
		public void ConformEaseValues()
		{
			if (this.m_EaseInDuration + this.m_EaseOutDuration > this.duration)
			{
				double num = TimelineClip.CalculateEasingRatio(this.m_EaseInDuration, this.m_EaseOutDuration);
				this.m_EaseInDuration = this.duration * num;
				this.m_EaseOutDuration = this.duration * (1.0 - num);
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004360 File Offset: 0x00002560
		private static double CalculateEasingRatio(double easeIn, double easeOut)
		{
			if (Math.Abs(easeIn - easeOut) < TimeUtility.kTimeEpsilon)
			{
				return 0.5;
			}
			if (easeIn == 0.0)
			{
				return 0.0;
			}
			if (easeOut == 0.0)
			{
				return 1.0;
			}
			return easeIn / (easeIn + easeOut);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x000043B7 File Offset: 0x000025B7
		private void UpdateDirty(double oldValue, double newValue)
		{
		}

		// Token: 0x04000044 RID: 68
		private const int k_LatestVersion = 1;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		[HideInInspector]
		private int m_Version;

		// Token: 0x04000046 RID: 70
		public static readonly ClipCaps kDefaultClipCaps = ClipCaps.Blending;

		// Token: 0x04000047 RID: 71
		public static readonly float kDefaultClipDurationInSeconds = 5f;

		// Token: 0x04000048 RID: 72
		public static readonly double kTimeScaleMin = 0.001;

		// Token: 0x04000049 RID: 73
		public static readonly double kTimeScaleMax = 1000.0;

		// Token: 0x0400004A RID: 74
		internal static readonly string kDefaultCurvesName = "Clip Parameters";

		// Token: 0x0400004B RID: 75
		internal static readonly double kMinDuration = 0.016666666666666666;

		// Token: 0x0400004C RID: 76
		internal static readonly double kMaxTimeValue = 1000000.0;

		// Token: 0x0400004D RID: 77
		[SerializeField]
		private double m_Start;

		// Token: 0x0400004E RID: 78
		[SerializeField]
		private double m_ClipIn;

		// Token: 0x0400004F RID: 79
		[SerializeField]
		private Object m_Asset;

		// Token: 0x04000050 RID: 80
		[SerializeField]
		[FormerlySerializedAs("m_HackDuration")]
		private double m_Duration;

		// Token: 0x04000051 RID: 81
		[SerializeField]
		private double m_TimeScale = 1.0;

		// Token: 0x04000052 RID: 82
		[SerializeField]
		private TrackAsset m_ParentTrack;

		// Token: 0x04000053 RID: 83
		[SerializeField]
		private double m_EaseInDuration;

		// Token: 0x04000054 RID: 84
		[SerializeField]
		private double m_EaseOutDuration;

		// Token: 0x04000055 RID: 85
		[SerializeField]
		private double m_BlendInDuration = -1.0;

		// Token: 0x04000056 RID: 86
		[SerializeField]
		private double m_BlendOutDuration = -1.0;

		// Token: 0x04000057 RID: 87
		[SerializeField]
		private AnimationCurve m_MixInCurve;

		// Token: 0x04000058 RID: 88
		[SerializeField]
		private AnimationCurve m_MixOutCurve;

		// Token: 0x04000059 RID: 89
		[SerializeField]
		private TimelineClip.BlendCurveMode m_BlendInCurveMode;

		// Token: 0x0400005A RID: 90
		[SerializeField]
		private TimelineClip.BlendCurveMode m_BlendOutCurveMode;

		// Token: 0x0400005B RID: 91
		[SerializeField]
		private List<string> m_ExposedParameterNames;

		// Token: 0x0400005C RID: 92
		[SerializeField]
		private AnimationClip m_AnimationCurves;

		// Token: 0x0400005D RID: 93
		[SerializeField]
		private bool m_Recordable;

		// Token: 0x0400005E RID: 94
		[SerializeField]
		private TimelineClip.ClipExtrapolation m_PostExtrapolationMode;

		// Token: 0x0400005F RID: 95
		[SerializeField]
		private TimelineClip.ClipExtrapolation m_PreExtrapolationMode;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private double m_PostExtrapolationTime;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private double m_PreExtrapolationTime;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		private string m_DisplayName;

		// Token: 0x02000060 RID: 96
		private enum Versions
		{
			// Token: 0x0400012A RID: 298
			Initial,
			// Token: 0x0400012B RID: 299
			ClipInFromGlobalToLocal
		}

		// Token: 0x02000061 RID: 97
		private static class TimelineClipUpgrade
		{
			// Token: 0x06000324 RID: 804 RVA: 0x0000B3F3 File Offset: 0x000095F3
			public static void UpgradeClipInFromGlobalToLocal(TimelineClip clip)
			{
				if (clip.m_ClipIn > 0.0 && clip.m_TimeScale > 1.401298464324817E-45)
				{
					clip.m_ClipIn *= clip.m_TimeScale;
				}
			}
		}

		// Token: 0x02000062 RID: 98
		public enum ClipExtrapolation
		{
			// Token: 0x0400012D RID: 301
			None,
			// Token: 0x0400012E RID: 302
			Hold,
			// Token: 0x0400012F RID: 303
			Loop,
			// Token: 0x04000130 RID: 304
			PingPong,
			// Token: 0x04000131 RID: 305
			Continue
		}

		// Token: 0x02000063 RID: 99
		public enum BlendCurveMode
		{
			// Token: 0x04000133 RID: 307
			Auto,
			// Token: 0x04000134 RID: 308
			Manual
		}
	}
}
