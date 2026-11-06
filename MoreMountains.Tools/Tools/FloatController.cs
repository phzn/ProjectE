using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000114 RID: 276
	[AddComponentMenu("More Mountains/Tools/Property Controllers/FloatController")]
	[MMRequiresConstantRepaint]
	public class FloatController : MMMonoBehaviour
	{
		// Token: 0x06000602 RID: 1538 RVA: 0x0001D834 File Offset: 0x0001BA34
		public virtual bool FindAttribute(string propertyName)
		{
			FieldInfo fieldInfo = null;
			this.TargetAttribute = null;
			PropertyInfo property = this.TargetObject.GetType().GetProperty(propertyName);
			if (property == null)
			{
				fieldInfo = this.TargetObject.GetType().GetField(propertyName);
			}
			if (property != null)
			{
				this.TargetAttribute = new MonoAttribute(this.TargetObject, MonoAttribute.MemberTypes.Property, property, null, propertyName);
			}
			if (fieldInfo != null)
			{
				this.TargetAttribute = new MonoAttribute(this.TargetObject, MonoAttribute.MemberTypes.Field, null, fieldInfo, propertyName);
			}
			if (this.PropertyName == "<Undefined Attribute>")
			{
				Debug.LogError("FloatController " + base.name + " : you need to pick a property from the Property list");
				return false;
			}
			if (property == null && fieldInfo == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"FloatController ",
					base.name,
					" couldn't find any property or field named ",
					propertyName,
					" on ",
					this.TargetObject.name
				}));
				return false;
			}
			if (this.TargetAttribute.MemberType == MonoAttribute.MemberTypes.Property)
			{
				this.TargetAttribute.MemberPropertyInfo = this.TargetObject.GetType().GetProperty(this.TargetAttribute.MemberName);
			}
			else if (this.TargetAttribute.MemberType == MonoAttribute.MemberTypes.Field)
			{
				this.TargetAttribute.MemberFieldInfo = this.TargetObject.GetType().GetField(this.TargetAttribute.MemberName);
			}
			return true;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001D9A4 File Offset: 0x0001BBA4
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001D9AC File Offset: 0x0001BBAC
		protected virtual void OnEnable()
		{
			this.InitialValue = this.GetInitialValue();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001D9BC File Offset: 0x0001BBBC
		public virtual void Initialization()
		{
			this._attributeFound = this.FindAttribute(this.PropertyName);
			if (!this._attributeFound)
			{
				return;
			}
			if (this.TargetObject == null || string.IsNullOrEmpty(this.TargetAttribute.MemberName))
			{
				return;
			}
			this._elapsedTime = 0f;
			this._randomAmplitude = Random.Range(this.Amplitude.x, this.Amplitude.y);
			this._randomFrequency = Random.Range(this.Frequency.x, this.Frequency.y);
			this._randomShift = Random.Range(this.Shift.x, this.Shift.y);
			this.InitialValue = this.GetInitialValue();
			this._shaking = false;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001DA88 File Offset: 0x0001BC88
		protected virtual float GetInitialValue()
		{
			if (this.TargetAttribute.MemberType == MonoAttribute.MemberTypes.Property)
			{
				return (float)this.TargetAttribute.MemberPropertyInfo.GetValue(this.TargetObject);
			}
			if (this.TargetAttribute.MemberType == MonoAttribute.MemberTypes.Field)
			{
				return (float)this.TargetAttribute.MemberFieldInfo.GetValue(this.TargetObject);
			}
			return 0f;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001DAED File Offset: 0x0001BCED
		public virtual void SetDrivenLevelAbsolute(float level)
		{
			this.DrivenLevel = level;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001DAF6 File Offset: 0x0001BCF6
		public virtual void SetDrivenLevelNormalized(float normalizedLevel, float remapZero, float remapOne)
		{
			this.DrivenLevel = MMMaths.Remap(normalizedLevel, 0f, 1f, remapZero, remapOne);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001DB10 File Offset: 0x0001BD10
		public virtual void OneTime()
		{
			if (this.TargetObject == null || this.TargetAttribute == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			base.enabled = true;
			this._shakeStartTimestamp = this.GetTime();
			this._shaking = true;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001DB50 File Offset: 0x0001BD50
		public virtual void ToDestination()
		{
			if (this.TargetObject == null || this.TargetAttribute == null)
			{
				return;
			}
			base.enabled = true;
			this.ControlMode = FloatController.ControlModes.ToDestination;
			this._shakeStartTimestamp = this.GetTime();
			this._shaking = true;
			this._initialValue = this.GetInitialValue();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001DBA1 File Offset: 0x0001BDA1
		protected float GetDeltaTime()
		{
			if (!this.UseUnscaledTime)
			{
				return Time.deltaTime;
			}
			return Time.unscaledDeltaTime;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001DBB6 File Offset: 0x0001BDB6
		protected float GetTime()
		{
			if (!this.UseUnscaledTime)
			{
				return Time.time;
			}
			return Time.unscaledTime;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001DBCC File Offset: 0x0001BDCC
		protected virtual void Update()
		{
			this._targetObjectLastFrame = this.TargetObject;
			this._targetAttributeLastFrame = this.TargetAttribute;
			if (this.TargetObject == null || this.TargetAttribute == null || !this._attributeFound)
			{
				return;
			}
			switch (this.ControlMode)
			{
			case FloatController.ControlModes.PingPong:
				if (this.GetTime() - this._lastPingPongPauseAt < this.PingPongPauseDuration)
				{
					return;
				}
				this.PingPong += this.GetDeltaTime() * this._pingPongDirection;
				if (this.PingPong < 0f)
				{
					this.PingPong = 0f;
					this._pingPongDirection = -this._pingPongDirection;
					this._lastPingPongPauseAt = this.GetTime();
				}
				if (this.PingPong > this.Duration)
				{
					this.PingPong = this.Duration;
					this._pingPongDirection = -this._pingPongDirection;
					this._lastPingPongPauseAt = this.GetTime();
				}
				this.CurrentValue = MMTween.Tween(this.PingPong, 0f, this.Duration, this.MinValue, this.MaxValue, this.Curve);
				this.CurrentValueNormalized = MMMaths.Remap(this.CurrentValue, this.MinValue, this.MaxValue, 0f, 1f);
				break;
			case FloatController.ControlModes.Random:
				this._elapsedTime += this.GetDeltaTime();
				this.CurrentValueNormalized = Mathf.PerlinNoise(this._randomFrequency * this._elapsedTime, this._randomShift);
				if (this.RemapNoiseValues)
				{
					this.CurrentValue = this.CurrentValueNormalized;
					this.CurrentValue = MMMaths.Remap(this.CurrentValue, 0f, 1f, this.RemapNoiseZero, this.RemapNoiseOne);
				}
				else
				{
					this.CurrentValue = (this.CurrentValueNormalized * 2f - 1f) * this._randomAmplitude;
				}
				break;
			case FloatController.ControlModes.OneTime:
				if (!this._shaking)
				{
					return;
				}
				this._remappedTimeSinceStart = MMMaths.Remap(this.GetTime() - this._shakeStartTimestamp, 0f, this.OneTimeDuration, 0f, 1f);
				this.CurrentValueNormalized = this.OneTimeCurve.Evaluate(this._remappedTimeSinceStart);
				this.CurrentValue = MMMaths.Remap(this.CurrentValueNormalized, 0f, 1f, this.OneTimeRemapMin, this.OneTimeRemapMax);
				this.CurrentValue *= this.OneTimeAmplitude;
				break;
			case FloatController.ControlModes.AudioAnalyzer:
				if (this.AudioAnalyzerMode == FloatController.AudioAnalyzerModes.Beat)
				{
					this.CurrentValue = this.AudioAnalyzer.Beats[this.BeatID].CurrentValue * this.AudioAnalyzerMultiplier;
				}
				else
				{
					this.CurrentValue = this.AudioAnalyzer.NormalizedBufferedBandLevels[this.NormalizedLevelID] * this.AudioAnalyzerMultiplier;
				}
				this.CurrentValueNormalized = Mathf.Clamp(this.CurrentValue, 0f, 1f);
				break;
			case FloatController.ControlModes.ToDestination:
			{
				if (!this._shaking)
				{
					return;
				}
				this._remappedTimeSinceStart = MMMaths.Remap(this.GetTime() - this._shakeStartTimestamp, 0f, this.ToDestinationDuration, 0f, 1f);
				float num = this.ToDestinationCurve.Evaluate(this._remappedTimeSinceStart);
				this.CurrentValue = Mathf.LerpUnclamped(this._initialValue, this.ToDestinationValue, num);
				this.CurrentValueNormalized = MMMaths.Remap(this.CurrentValue, this._initialValue, this.ToDestinationValue, 0f, 1f);
				break;
			}
			case FloatController.ControlModes.Driven:
				this.CurrentValue = this.DrivenLevel;
				this.CurrentValueNormalized = Mathf.Clamp(this.CurrentValue, 0f, 1f);
				break;
			}
			if (this.AddToInitialValue)
			{
				this.CurrentValue += this.InitialValue;
			}
			if (this.ControlMode == FloatController.ControlModes.OneTime && this._shaking && this.GetTime() - this._shakeStartTimestamp > this.OneTimeDuration)
			{
				this._shaking = false;
				if (this.RevertToInitialValueAfterEnd)
				{
					this.CurrentValue = this.InitialValue;
					this.TargetAttribute.SetValue(this.CurrentValue);
				}
				else
				{
					this.CurrentValue = this.OneTimeCurve.Evaluate(1f);
					this.CurrentValue = MMMaths.Remap(this.CurrentValue, 0f, 1f, this.OneTimeRemapMin, this.OneTimeRemapMax);
					this.CurrentValue *= this.OneTimeAmplitude;
					this.TargetAttribute.SetValue(this.CurrentValue);
				}
				if (this.DisableAfterOneTime)
				{
					base.enabled = false;
				}
				if (this.DisableGameObjectAfterOneTime)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			if (this.ControlMode == FloatController.ControlModes.ToDestination && this._shaking && this.GetTime() - this._shakeStartTimestamp > this.ToDestinationDuration)
			{
				this._shaking = false;
				if (this.RevertToInitialValueAfterEnd)
				{
					this.CurrentValue = this.InitialValue;
				}
				else
				{
					this.CurrentValue = this.ToDestinationValue;
				}
				this.TargetAttribute.SetValue(this.CurrentValue);
				if (this.DisableAfterOneTime)
				{
					base.enabled = false;
				}
				if (this.DisableGameObjectAfterOneTime)
				{
					base.gameObject.SetActive(false);
				}
				return;
			}
			this.TargetAttribute.SetValue(this.CurrentValue);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001E0FB File Offset: 0x0001C2FB
		protected virtual void OnValidate()
		{
			this.FillDropDownList();
			if (Application.isPlaying && (this._targetAttributeLastFrame != this.TargetAttribute || this._targetObjectLastFrame != this.TargetObject))
			{
				this.Initialization();
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001E131 File Offset: 0x0001C331
		protected virtual void OnDisable()
		{
			if (this.RevertToInitialValueAfterEnd)
			{
				this.CurrentValue = this.InitialValue;
				this.TargetAttribute.SetValue(this.CurrentValue);
			}
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001E158 File Offset: 0x0001C358
		public virtual void Stop()
		{
			this._shaking = false;
			base.enabled = false;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001E168 File Offset: 0x0001C368
		public virtual void FillDropDownList()
		{
			this.AttributeNames = new string[0];
			if (this.TargetObject == null)
			{
				return;
			}
			this._propertyReferences = this.TargetObject.GetType().GetProperties();
			this._attributesNamesTempList = new List<string>();
			this._attributesNamesTempList.Add("<Undefined Attribute>");
			foreach (PropertyInfo propertyInfo in this._propertyReferences)
			{
				if (propertyInfo.PropertyType.Name == "Single")
				{
					this._attributesNamesTempList.Add(propertyInfo.Name);
				}
			}
			this._fieldReferences = this.TargetObject.GetType().GetFields();
			foreach (FieldInfo fieldInfo in this._fieldReferences)
			{
				if (fieldInfo.FieldType.Name == "Single")
				{
					this._attributesNamesTempList.Add(fieldInfo.Name);
				}
			}
			this.AttributeNames = this._attributesNamesTempList.ToArray();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001E26D File Offset: 0x0001C46D
		public virtual void RestoreInitialValues()
		{
			this.TargetAttribute.SetValue(this.InitialValue);
		}

		// Token: 0x0400072B RID: 1835
		[Header("Target")]
		public MonoBehaviour TargetObject;

		// Token: 0x0400072C RID: 1836
		[Header("Global Settings")]
		public FloatController.ControlModes ControlMode;

		// Token: 0x0400072D RID: 1837
		public bool AddToInitialValue;

		// Token: 0x0400072E RID: 1838
		public bool UseUnscaledTime = true;

		// Token: 0x0400072F RID: 1839
		public bool RevertToInitialValueAfterEnd = true;

		// Token: 0x04000730 RID: 1840
		[Header("Driven")]
		public float DrivenLevel;

		// Token: 0x04000731 RID: 1841
		[Header("Ping Pong")]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04000732 RID: 1842
		public float MinValue;

		// Token: 0x04000733 RID: 1843
		public float MaxValue = 5f;

		// Token: 0x04000734 RID: 1844
		public float Duration = 1f;

		// Token: 0x04000735 RID: 1845
		public float PingPongPauseDuration;

		// Token: 0x04000736 RID: 1846
		[Header("Random")]
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Amplitude = new Vector2(0f, 5f);

		// Token: 0x04000737 RID: 1847
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Frequency = new Vector2(1f, 1f);

		// Token: 0x04000738 RID: 1848
		[MMVector(new string[] { "Min", "Max" })]
		public Vector2 Shift = new Vector2(0f, 1f);

		// Token: 0x04000739 RID: 1849
		public bool RemapNoiseValues;

		// Token: 0x0400073A RID: 1850
		[MMCondition("RemapNoiseValues", true)]
		public float RemapNoiseZero;

		// Token: 0x0400073B RID: 1851
		[MMCondition("RemapNoiseValues", true)]
		public float RemapNoiseOne = 1f;

		// Token: 0x0400073C RID: 1852
		[Header("OneTime")]
		public float OneTimeDuration = 1f;

		// Token: 0x0400073D RID: 1853
		public float OneTimeAmplitude = 1f;

		// Token: 0x0400073E RID: 1854
		public float OneTimeRemapMin;

		// Token: 0x0400073F RID: 1855
		public float OneTimeRemapMax = 1f;

		// Token: 0x04000740 RID: 1856
		public AnimationCurve OneTimeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000741 RID: 1857
		public bool DisableAfterOneTime;

		// Token: 0x04000742 RID: 1858
		public bool DisableGameObjectAfterOneTime;

		// Token: 0x04000743 RID: 1859
		[MMInspectorButton("OneTime")]
		public bool OneTimeButton;

		// Token: 0x04000744 RID: 1860
		[Header("ToDestination")]
		public float ToDestinationDuration = 1f;

		// Token: 0x04000745 RID: 1861
		public float ToDestinationValue = 1f;

		// Token: 0x04000746 RID: 1862
		public AnimationCurve ToDestinationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 0.6f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04000747 RID: 1863
		public bool DisableAfterToDestination;

		// Token: 0x04000748 RID: 1864
		[MMInspectorButton("ToDestination")]
		public bool ToDestinationButton;

		// Token: 0x04000749 RID: 1865
		[Header("AudioAnalyzer")]
		public MMAudioAnalyzer AudioAnalyzer;

		// Token: 0x0400074A RID: 1866
		public FloatController.AudioAnalyzerModes AudioAnalyzerMode;

		// Token: 0x0400074B RID: 1867
		public int BeatID;

		// Token: 0x0400074C RID: 1868
		public int NormalizedLevelID;

		// Token: 0x0400074D RID: 1869
		public float AudioAnalyzerMultiplier = 1f;

		// Token: 0x0400074E RID: 1870
		[Header("Debug")]
		[MMReadOnly]
		public float InitialValue;

		// Token: 0x0400074F RID: 1871
		[MMReadOnly]
		public float CurrentValue;

		// Token: 0x04000750 RID: 1872
		[MMReadOnly]
		public float CurrentValueNormalized;

		// Token: 0x04000751 RID: 1873
		[HideInInspector]
		public float PingPong;

		// Token: 0x04000752 RID: 1874
		[HideInInspector]
		public MonoAttribute TargetAttribute;

		// Token: 0x04000753 RID: 1875
		[HideInInspector]
		public string[] AttributeNames;

		// Token: 0x04000754 RID: 1876
		[HideInInspector]
		public string PropertyName;

		// Token: 0x04000755 RID: 1877
		[HideInInspector]
		public int ChoiceIndex;

		// Token: 0x04000756 RID: 1878
		public const string _undefinedString = "<Undefined Attribute>";

		// Token: 0x04000757 RID: 1879
		protected List<string> _attributesNamesTempList;

		// Token: 0x04000758 RID: 1880
		protected PropertyInfo[] _propertyReferences;

		// Token: 0x04000759 RID: 1881
		protected FieldInfo[] _fieldReferences;

		// Token: 0x0400075A RID: 1882
		protected bool _attributeFound;

		// Token: 0x0400075B RID: 1883
		protected float _randomAmplitude;

		// Token: 0x0400075C RID: 1884
		protected float _randomFrequency;

		// Token: 0x0400075D RID: 1885
		protected float _randomShift;

		// Token: 0x0400075E RID: 1886
		protected float _elapsedTime;

		// Token: 0x0400075F RID: 1887
		protected bool _shaking;

		// Token: 0x04000760 RID: 1888
		protected float _shakeStartTimestamp;

		// Token: 0x04000761 RID: 1889
		protected float _remappedTimeSinceStart;

		// Token: 0x04000762 RID: 1890
		protected float _pingPongDirection = 1f;

		// Token: 0x04000763 RID: 1891
		protected float _lastPingPongPauseAt;

		// Token: 0x04000764 RID: 1892
		protected float _initialValue;

		// Token: 0x04000765 RID: 1893
		protected MonoBehaviour _targetObjectLastFrame;

		// Token: 0x04000766 RID: 1894
		protected MonoAttribute _targetAttributeLastFrame;

		// Token: 0x02000435 RID: 1077
		public enum ControlModes
		{
			// Token: 0x04001F9F RID: 8095
			PingPong,
			// Token: 0x04001FA0 RID: 8096
			Random,
			// Token: 0x04001FA1 RID: 8097
			OneTime,
			// Token: 0x04001FA2 RID: 8098
			AudioAnalyzer,
			// Token: 0x04001FA3 RID: 8099
			ToDestination,
			// Token: 0x04001FA4 RID: 8100
			Driven
		}

		// Token: 0x02000436 RID: 1078
		public enum AudioAnalyzerModes
		{
			// Token: 0x04001FA6 RID: 8102
			Beat,
			// Token: 0x04001FA7 RID: 8103
			NormalizedBufferedBandLevels
		}
	}
}
