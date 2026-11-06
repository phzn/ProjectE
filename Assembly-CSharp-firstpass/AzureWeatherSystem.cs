using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public sealed class AzureWeatherSystem
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000232 RID: 562 RVA: 0x000080DE File Offset: 0x000062DE
		// (set) Token: 0x06000233 RID: 563 RVA: 0x000080E6 File Offset: 0x000062E6
		public List<AzureWeatherPropertyGroup> weatherPropertyGroupList
		{
			get
			{
				return this.m_weatherPropertyGroupList;
			}
			set
			{
				this.m_weatherPropertyGroupList = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000234 RID: 564 RVA: 0x000080EF File Offset: 0x000062EF
		// (set) Token: 0x06000235 RID: 565 RVA: 0x000080F7 File Offset: 0x000062F7
		public List<AzureGlobalWeather> globalWeatherList
		{
			get
			{
				return this.m_globalWeatherList;
			}
			set
			{
				this.m_globalWeatherList = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00008100 File Offset: 0x00006300
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00008108 File Offset: 0x00006308
		public List<AzureWeatherZone> weatherZoneList
		{
			get
			{
				return this.m_weatherZoneList;
			}
			set
			{
				this.m_weatherZoneList = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00008111 File Offset: 0x00006311
		// (set) Token: 0x06000239 RID: 569 RVA: 0x00008119 File Offset: 0x00006319
		public List<AzureThunderSettings> thunderSettingsList
		{
			get
			{
				return this.m_thunderSettingsList;
			}
			set
			{
				this.m_thunderSettingsList = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00008122 File Offset: 0x00006322
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000812A File Offset: 0x0000632A
		public AzureWeatherPreset currentWeatherPreset
		{
			get
			{
				return this.m_currentWeatherPreset;
			}
			set
			{
				this.m_currentWeatherPreset = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00008133 File Offset: 0x00006333
		public AzureWeatherPreset targetWeatherPreset
		{
			get
			{
				return this.m_targetWeatherPreset;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000813B File Offset: 0x0000633B
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00008143 File Offset: 0x00006343
		public Transform weatherZoneTrigger
		{
			get
			{
				return this.m_weatherZoneTrigger;
			}
			set
			{
				this.m_weatherZoneTrigger = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000814C File Offset: 0x0000634C
		// (set) Token: 0x06000240 RID: 576 RVA: 0x00008154 File Offset: 0x00006354
		public float evaluationTime
		{
			get
			{
				return this.m_evaluationTime;
			}
			set
			{
				this.m_evaluationTime = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000815D File Offset: 0x0000635D
		public bool isWeatherChanging
		{
			get
			{
				return this.m_isWeatherChanging;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00008165 File Offset: 0x00006365
		public int globalWeatherIndex
		{
			get
			{
				return this.m_globalWeatherIndex;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000816D File Offset: 0x0000636D
		public float weatherTransitionProgress
		{
			get
			{
				return this.m_weatherTransitionProgress;
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008175 File Offset: 0x00006375
		public void Start()
		{
			this.InitializeCurrentWeatherPreset();
			this.InitializePropertyTargets();
			this.UpdateWeatherSystem();
			this.OverrideTargetProperties();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00008190 File Offset: 0x00006390
		public void UpdateWeatherSystem()
		{
			AzureNotificationCenter.Invoke.OnBeforeWeatherSystemUpdateCallback(this);
			this.m_evaluationTimeGradient = this.m_evaluationTime / 24f;
			if (!this.m_isWeatherChanging)
			{
				this.EvaluateCurrentWeather();
			}
			else
			{
				this.m_weatherTransitionProgress = Mathf.Clamp01((Time.time - this.m_weatherTransitionStart) / this.m_weatherTransitionLength);
				this.EvaluateGlobalWeatherTransition(this.m_currentWeatherPreset, this.m_targetWeatherPreset, this.m_weatherTransitionProgress);
				if (Mathf.Abs(this.m_weatherTransitionProgress - 1f) <= 0f)
				{
					this.m_isWeatherChanging = false;
					this.m_weatherTransitionProgress = 0f;
					this.m_weatherTransitionStart = 0f;
					this.m_currentWeatherPreset = this.m_targetWeatherPreset;
					AzureNotificationCenter.Invoke.OnWeatherTransitionEndCallback(this);
				}
			}
			if (!this.m_weatherZoneTrigger)
			{
				AzureNotificationCenter.Invoke.OnAfterWeatherSystemUpdateCallback(this);
				return;
			}
			this.m_weatherZoneTriggerPosition = this.m_weatherZoneTrigger.position;
			foreach (AzureWeatherZone azureWeatherZone in this.m_weatherZoneList)
			{
				if (!(azureWeatherZone == null))
				{
					this.m_weatherZoneCollider = azureWeatherZone.colliderVolume;
					if (this.m_weatherZoneCollider && this.m_weatherZoneCollider.enabled && this.m_weatherZoneCollider.gameObject.activeInHierarchy)
					{
						this.m_weatherZoneClosestDistanceSqr = float.PositiveInfinity;
						this.m_weatherZoneClosestPoint = this.m_weatherZoneCollider.ClosestPoint(this.m_weatherZoneTriggerPosition);
						this.m_weatherZoneDistance = ((this.m_weatherZoneClosestPoint - this.m_weatherZoneTriggerPosition) / 2f).sqrMagnitude;
						if (this.m_weatherZoneDistance < this.m_weatherZoneClosestDistanceSqr)
						{
							this.m_weatherZoneClosestDistanceSqr = this.m_weatherZoneDistance;
						}
						this.m_weatherZoneCollider = null;
						this.m_weatherZoneBlendDistanceSqr = azureWeatherZone.blendDistance * azureWeatherZone.blendDistance;
						if (this.m_weatherZoneClosestDistanceSqr <= this.m_weatherZoneBlendDistanceSqr)
						{
							this.m_weatherZoneInterpolationFactor = 1f;
							if (this.m_weatherZoneBlendDistanceSqr > 0f)
							{
								this.m_weatherZoneInterpolationFactor = 1f - this.m_weatherZoneClosestDistanceSqr / this.m_weatherZoneBlendDistanceSqr;
							}
							this.EvaluateWeatherZonesInfluence(azureWeatherZone.weatherPreset, this.m_weatherZoneInterpolationFactor);
						}
					}
				}
			}
			AzureNotificationCenter.Invoke.OnAfterWeatherSystemUpdateCallback(this);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000083DC File Offset: 0x000065DC
		public void OverrideTargetProperties()
		{
			AzureNotificationCenter.Invoke.OnBeforeOverrideUpdateCallback(this);
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				if (this.m_weatherPropertyGroupList[i].isEnabled)
				{
					for (int j = 0; j < this.m_weatherPropertyGroupList[i].weatherPropertyList.Count; j++)
					{
						if (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].overrideMode != AzureWeatherPropertyOverrideMode.Off)
						{
							switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyType)
							{
							case AzureWeatherPropertyType.Float:
							case AzureWeatherPropertyType.Curve:
								switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetType)
								{
								case AzureWeatherPropertyTargetType.Property:
								{
									PropertyInfo propertyInfo = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo != null)
									{
										propertyInfo.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.Field:
								{
									FieldInfo fieldInfo = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo != null)
									{
										fieldInfo.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.MaterialProperty:
								{
									Material targetMaterial = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetMaterial;
									if (targetMaterial != null)
									{
										targetMaterial.SetFloat(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalShaderUniform:
									Shader.SetGlobalFloat(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									break;
								case AzureWeatherPropertyTargetType.GlobalProperty:
								{
									PropertyInfo propertyInfo2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo2 != null)
									{
										propertyInfo2.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalField:
								{
									FieldInfo fieldInfo2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo2 != null)
									{
										fieldInfo2.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput);
									}
									break;
								}
								}
								break;
							case AzureWeatherPropertyType.Color:
							case AzureWeatherPropertyType.Gradient:
								switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetType)
								{
								case AzureWeatherPropertyTargetType.Property:
								{
									PropertyInfo propertyInfo3 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo3 != null)
									{
										propertyInfo3.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.Field:
								{
									FieldInfo fieldInfo3 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo3 != null)
									{
										fieldInfo3.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.MaterialProperty:
								{
									Material targetMaterial2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetMaterial;
									if (targetMaterial2 != null)
									{
										targetMaterial2.SetColor(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalShaderUniform:
									Shader.SetGlobalColor(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									break;
								case AzureWeatherPropertyTargetType.GlobalProperty:
								{
									PropertyInfo propertyInfo4 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo4 != null)
									{
										propertyInfo4.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalField:
								{
									FieldInfo fieldInfo4 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo4 != null)
									{
										fieldInfo4.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput);
									}
									break;
								}
								}
								break;
							case AzureWeatherPropertyType.Direction:
							case AzureWeatherPropertyType.Position:
								switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetType)
								{
								case AzureWeatherPropertyTargetType.Property:
								{
									PropertyInfo propertyInfo5 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo5 != null)
									{
										propertyInfo5.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.Field:
								{
									FieldInfo fieldInfo5 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo5 != null)
									{
										fieldInfo5.SetValueOptimized(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.MaterialProperty:
								{
									Material targetMaterial3 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetMaterial;
									if (targetMaterial3 != null)
									{
										targetMaterial3.SetVector(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalShaderUniform:
									Shader.SetGlobalVector(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									break;
								case AzureWeatherPropertyTargetType.GlobalProperty:
								{
									PropertyInfo propertyInfo6 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo;
									if (propertyInfo6 != null)
									{
										propertyInfo6.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									}
									break;
								}
								case AzureWeatherPropertyTargetType.GlobalField:
								{
									FieldInfo fieldInfo6 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo;
									if (fieldInfo6 != null)
									{
										fieldInfo6.SetValue(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType, this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output);
									}
									break;
								}
								}
								break;
							}
						}
					}
				}
			}
			AzureNotificationCenter.Invoke.OnAfterOverrideUpdateCallback(this);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00008C7C File Offset: 0x00006E7C
		public void InstantiateThunderPrefab(int index)
		{
			if (this.m_thunderSettingsList.Count <= 0)
			{
				return;
			}
			if (this.m_thunderSettingsList[index].transform == null)
			{
				return;
			}
			Object.Instantiate<Transform>(this.m_thunderSettingsList[index].transform, this.m_thunderSettingsList[index].position, Quaternion.identity);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008CE0 File Offset: 0x00006EE0
		public void InstantiateThunderPrefab(int index, Vector3 position)
		{
			if (this.m_thunderSettingsList.Count <= 0)
			{
				return;
			}
			if (this.m_thunderSettingsList[index].transform == null)
			{
				return;
			}
			Object.Instantiate<Transform>(this.m_thunderSettingsList[index].transform, position, Quaternion.identity);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00008D34 File Offset: 0x00006F34
		public void SetGlobalWeather(int index)
		{
			if (this.m_globalWeatherList == null || this.m_globalWeatherList.Count == 0)
			{
				return;
			}
			index = Mathf.Clamp(index, 0, this.m_globalWeatherList.Count - 1);
			AzureGlobalWeather azureGlobalWeather = this.m_globalWeatherList[index];
			if (azureGlobalWeather == null || azureGlobalWeather.weatherPreset == null)
			{
				return;
			}
			if (this.m_globalWeatherIndex == index)
			{
				return;
			}
			if (this.m_isWeatherChanging)
			{
				return;
			}
			this.m_targetWeatherPreset = azureGlobalWeather.weatherPreset;
			this.m_weatherTransitionLength = azureGlobalWeather.transitionTime;
			this.m_weatherTransitionProgress = 0f;
			this.m_weatherTransitionStart = Time.time;
			this.m_globalWeatherIndex = index;
			this.m_isWeatherChanging = true;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008DDC File Offset: 0x00006FDC
		public void SetGlobalWeatherInstantly(int index)
		{
			if (this.m_globalWeatherList == null || this.m_globalWeatherList.Count == 0)
			{
				return;
			}
			index = Mathf.Clamp(index, 0, this.m_globalWeatherList.Count - 1);
			AzureGlobalWeather azureGlobalWeather = this.m_globalWeatherList[index];
			AzureWeatherPreset azureWeatherPreset = ((azureGlobalWeather != null) ? azureGlobalWeather.weatherPreset : null);
			if (azureWeatherPreset == null)
			{
				return;
			}
			this.SetGlobalWeatherInstantly(azureWeatherPreset, index);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00008E40 File Offset: 0x00007040
		public void SetGlobalWeatherInstantly(AzureWeatherPreset preset, int index)
		{
			if (preset == null)
			{
				return;
			}
			index = ((this.m_globalWeatherList != null && this.m_globalWeatherList.Count > 0) ? Mathf.Clamp(index, 0, this.m_globalWeatherList.Count - 1) : 0);
			this.m_currentWeatherPreset = preset;
			this.m_targetWeatherPreset = preset;
			this.m_globalWeatherIndex = index;
			this.m_isWeatherChanging = false;
			this.m_weatherTransitionProgress = 0f;
			this.m_weatherTransitionStart = 0f;
			this.m_weatherTransitionLength = 0f;
			this.m_evaluationTimeGradient = this.m_evaluationTime / 24f;
			this.EvaluateCurrentWeather();
			this.OverrideTargetProperties();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008EE1 File Offset: 0x000070E1
		public void DelayActiveWeatherTransition(float scaledSeconds)
		{
			if (!this.m_isWeatherChanging || scaledSeconds <= 0f)
			{
				return;
			}
			this.m_weatherTransitionStart += scaledSeconds;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00008F02 File Offset: 0x00007102
		public void SetGlobalWeather(AzureWeatherPreset preset, float transitionTime)
		{
			this.m_targetWeatherPreset = preset;
			this.m_weatherTransitionLength = transitionTime;
			this.m_weatherTransitionProgress = 0f;
			this.m_weatherTransitionStart = Time.time;
			this.m_isWeatherChanging = true;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008F2F File Offset: 0x0000712F
		public void SetGlobalWeather(AzureWeatherPreset preset)
		{
			this.m_currentWeatherPreset = preset;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008F38 File Offset: 0x00007138
		public void WeatherGroupsEnableAll()
		{
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				this.m_weatherPropertyGroupList[i].isEnabled = true;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00008F70 File Offset: 0x00007170
		public void WeatherGroupsDisableAll()
		{
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				this.m_weatherPropertyGroupList[i].isEnabled = false;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008FA5 File Offset: 0x000071A5
		public void WeatherGroupsEnableAt(int index)
		{
			this.m_weatherPropertyGroupList[index].isEnabled = true;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008FB9 File Offset: 0x000071B9
		public void WeatherGroupsDisableAt(int index)
		{
			this.m_weatherPropertyGroupList[index].isEnabled = false;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008FCD File Offset: 0x000071CD
		public void WeatherGroupsSetStateAt(int index, bool state)
		{
			this.m_weatherPropertyGroupList[index].isEnabled = state;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008FE1 File Offset: 0x000071E1
		public float GetFloatOutput(int groupIndex, int propertyIndex)
		{
			return this.m_weatherPropertyGroupList[groupIndex].weatherPropertyList[propertyIndex].floatOutput;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008FFF File Offset: 0x000071FF
		public Color GetColorOutput(int groupIndex, int propertyIndex)
		{
			return this.m_weatherPropertyGroupList[groupIndex].weatherPropertyList[propertyIndex].colorOutput;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000901D File Offset: 0x0000721D
		public Vector3 GetVector3Output(int groupIndex, int propertyIndex)
		{
			return this.m_weatherPropertyGroupList[groupIndex].weatherPropertyList[propertyIndex].vector3Output;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000903C File Offset: 0x0000723C
		public void InitializePropertyTargets()
		{
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				for (int j = 0; j < this.m_weatherPropertyGroupList[i].weatherPropertyList.Count; j++)
				{
					this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyInfo = null;
					this.m_weatherPropertyGroupList[i].weatherPropertyList[j].fieldInfo = null;
					switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetType)
					{
					case AzureWeatherPropertyTargetType.Property:
						if (!(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetObject == null))
						{
							GameObject gameObject = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetObject;
							string text = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponentName;
							string text2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName;
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent = gameObject.GetComponent(text);
							AzureWeatherProperty azureWeatherProperty = this.m_weatherPropertyGroupList[i].weatherPropertyList[j];
							Component targetComponent = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent;
							azureWeatherProperty.propertyInfo = ((targetComponent != null) ? targetComponent.GetType().GetProperty(text2) : null);
						}
						break;
					case AzureWeatherPropertyTargetType.Field:
						if (!(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetObject == null))
						{
							GameObject gameObject = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetObject;
							string text = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponentName;
							string text2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName;
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent = gameObject.GetComponent(text);
							AzureWeatherProperty azureWeatherProperty2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j];
							Component targetComponent2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponent;
							azureWeatherProperty2.fieldInfo = ((targetComponent2 != null) ? targetComponent2.GetType().GetField(text2) : null);
						}
						break;
					case AzureWeatherPropertyTargetType.GlobalProperty:
					{
						string text = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponentName;
						string text2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName;
						this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType = Type.GetType(text);
						AzureWeatherProperty azureWeatherProperty3 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j];
						Type targetGlobalType = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType;
						azureWeatherProperty3.propertyInfo = ((targetGlobalType != null) ? targetGlobalType.GetProperty(text2) : null);
						break;
					}
					case AzureWeatherPropertyTargetType.GlobalField:
					{
						string text = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetComponentName;
						string text2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetPropertyName;
						this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType = Type.GetType(text);
						AzureWeatherProperty azureWeatherProperty4 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j];
						Type targetGlobalType2 = this.m_weatherPropertyGroupList[i].weatherPropertyList[j].targetGlobalType;
						azureWeatherProperty4.fieldInfo = ((targetGlobalType2 != null) ? targetGlobalType2.GetField(text2) : null);
						break;
					}
					}
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009437 File Offset: 0x00007637
		private void InitializeCurrentWeatherPreset()
		{
			if (this.m_globalWeatherList.Count <= 0)
			{
				return;
			}
			if (this.m_globalWeatherList[0] == null)
			{
				return;
			}
			this.m_currentWeatherPreset = this.m_globalWeatherList[0].weatherPreset;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009470 File Offset: 0x00007670
		private void EvaluateCurrentWeather()
		{
			if (this.m_currentWeatherPreset == null)
			{
				return;
			}
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				if (this.m_weatherPropertyGroupList[i].isEnabled)
				{
					for (int j = 0; j < this.m_weatherPropertyGroupList[i].weatherPropertyList.Count; j++)
					{
						switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyType)
						{
						case AzureWeatherPropertyType.Float:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.m_currentWeatherPreset.propertyGroupDataList[i].propertyDataList[j].floatData;
							break;
						case AzureWeatherPropertyType.Color:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.m_currentWeatherPreset.propertyGroupDataList[i].propertyDataList[j].colorData;
							break;
						case AzureWeatherPropertyType.Curve:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.m_currentWeatherPreset.propertyGroupDataList[i].propertyDataList[j].curveData.Evaluate(this.m_evaluationTime);
							break;
						case AzureWeatherPropertyType.Gradient:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.m_currentWeatherPreset.propertyGroupDataList[i].propertyDataList[j].gradientData.Evaluate(this.m_evaluationTimeGradient);
							break;
						case AzureWeatherPropertyType.Direction:
						case AzureWeatherPropertyType.Position:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output = this.m_currentWeatherPreset.propertyGroupDataList[i].propertyDataList[j].vector3Data;
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009680 File Offset: 0x00007880
		private void EvaluateGlobalWeatherTransition(AzureWeatherPreset from, AzureWeatherPreset to, float t)
		{
			if (from == null || to == null)
			{
				return;
			}
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				if (this.m_weatherPropertyGroupList[i].isEnabled)
				{
					this.m_weatherGroupRampTime = to.propertyGroupDataList[i].rampCurve.Evaluate(t);
					for (int j = 0; j < this.m_weatherPropertyGroupList[i].weatherPropertyList.Count; j++)
					{
						switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyType)
						{
						case AzureWeatherPropertyType.Float:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.FloatInterpolation(from.propertyGroupDataList[i].propertyDataList[j].floatData, to.propertyGroupDataList[i].propertyDataList[j].floatData, this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Color:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.ColorInterpolation(from.propertyGroupDataList[i].propertyDataList[j].colorData, to.propertyGroupDataList[i].propertyDataList[j].colorData, this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Curve:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.FloatInterpolation(from.propertyGroupDataList[i].propertyDataList[j].curveData.Evaluate(this.m_evaluationTime), to.propertyGroupDataList[i].propertyDataList[j].curveData.Evaluate(this.m_evaluationTime), this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Gradient:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.ColorInterpolation(from.propertyGroupDataList[i].propertyDataList[j].gradientData.Evaluate(this.m_evaluationTimeGradient), to.propertyGroupDataList[i].propertyDataList[j].gradientData.Evaluate(this.m_evaluationTimeGradient), this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Direction:
							this.m_vector3Angle.x = Mathf.LerpAngle(from.propertyGroupDataList[i].propertyDataList[j].vector3Data.x, to.propertyGroupDataList[i].propertyDataList[j].vector3Data.x, this.m_weatherGroupRampTime);
							this.m_vector3Angle.y = Mathf.LerpAngle(from.propertyGroupDataList[i].propertyDataList[j].vector3Data.y, to.propertyGroupDataList[i].propertyDataList[j].vector3Data.y, this.m_weatherGroupRampTime);
							this.m_vector3Angle.z = Mathf.LerpAngle(from.propertyGroupDataList[i].propertyDataList[j].vector3Data.z, to.propertyGroupDataList[i].propertyDataList[j].vector3Data.z, this.m_weatherGroupRampTime);
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output = this.m_vector3Angle;
							break;
						case AzureWeatherPropertyType.Position:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output = Vector3.Lerp(from.propertyGroupDataList[i].propertyDataList[j].vector3Data, to.propertyGroupDataList[i].propertyDataList[j].vector3Data, this.m_weatherGroupRampTime);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009AA4 File Offset: 0x00007CA4
		private void EvaluateWeatherZonesInfluence(AzureWeatherPreset target, float blendFactor)
		{
			if (target == null)
			{
				return;
			}
			for (int i = 0; i < this.m_weatherPropertyGroupList.Count; i++)
			{
				if (this.m_weatherPropertyGroupList[i].isEnabled)
				{
					this.m_weatherGroupRampTime = target.propertyGroupDataList[i].rampCurve.Evaluate(blendFactor);
					for (int j = 0; j < this.m_weatherPropertyGroupList[i].weatherPropertyList.Count; j++)
					{
						switch (this.m_weatherPropertyGroupList[i].weatherPropertyList[j].propertyType)
						{
						case AzureWeatherPropertyType.Float:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.FloatInterpolation(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput, target.propertyGroupDataList[i].propertyDataList[j].floatData, this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Color:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.ColorInterpolation(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput, target.propertyGroupDataList[i].propertyDataList[j].colorData, this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Curve:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput = this.FloatInterpolation(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].floatOutput, target.propertyGroupDataList[i].propertyDataList[j].curveData.Evaluate(this.m_evaluationTime), this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Gradient:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput = this.ColorInterpolation(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].colorOutput, target.propertyGroupDataList[i].propertyDataList[j].gradientData.Evaluate(this.m_evaluationTimeGradient), this.m_weatherGroupRampTime);
							break;
						case AzureWeatherPropertyType.Direction:
							this.m_vector3Angle.x = Mathf.LerpAngle(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output.x, target.propertyGroupDataList[i].propertyDataList[j].vector3Data.x, this.m_weatherGroupRampTime);
							this.m_vector3Angle.y = Mathf.LerpAngle(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output.y, target.propertyGroupDataList[i].propertyDataList[j].vector3Data.y, this.m_weatherGroupRampTime);
							this.m_vector3Angle.z = Mathf.LerpAngle(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output.z, target.propertyGroupDataList[i].propertyDataList[j].vector3Data.z, this.m_weatherGroupRampTime);
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output = this.m_vector3Angle;
							break;
						case AzureWeatherPropertyType.Position:
							this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output = Vector3.Lerp(this.m_weatherPropertyGroupList[i].weatherPropertyList[j].vector3Output, target.propertyGroupDataList[i].propertyDataList[j].vector3Data, this.m_weatherGroupRampTime);
							break;
						}
					}
				}
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009EA6 File Offset: 0x000080A6
		private float FloatInterpolation(float from, float to, float t)
		{
			return from + (to - from) * t;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009EB0 File Offset: 0x000080B0
		private Color ColorInterpolation(Color from, Color to, float t)
		{
			Color color;
			color.r = from.r + (to.r - from.r) * t;
			color.g = from.g + (to.g - from.g) * t;
			color.b = from.b + (to.b - from.b) * t;
			color.a = from.a + (to.a - from.a) * t;
			return color;
		}

		// Token: 0x04000191 RID: 401
		[SerializeField]
		private List<AzureWeatherPropertyGroup> m_weatherPropertyGroupList = new List<AzureWeatherPropertyGroup>();

		// Token: 0x04000192 RID: 402
		[SerializeField]
		private List<AzureGlobalWeather> m_globalWeatherList = new List<AzureGlobalWeather>();

		// Token: 0x04000193 RID: 403
		[SerializeField]
		private List<AzureWeatherZone> m_weatherZoneList = new List<AzureWeatherZone>();

		// Token: 0x04000194 RID: 404
		[SerializeField]
		private List<AzureThunderSettings> m_thunderSettingsList = new List<AzureThunderSettings>();

		// Token: 0x04000195 RID: 405
		[SerializeField]
		private AzureWeatherPreset m_currentWeatherPreset;

		// Token: 0x04000196 RID: 406
		private AzureWeatherPreset m_targetWeatherPreset;

		// Token: 0x04000197 RID: 407
		[SerializeField]
		private Transform m_weatherZoneTrigger;

		// Token: 0x04000198 RID: 408
		[SerializeField]
		private float m_evaluationTime = 6.5f;

		// Token: 0x04000199 RID: 409
		[SerializeField]
		private float m_evaluationTimeGradient = 0.27f;

		// Token: 0x0400019A RID: 410
		private bool m_isWeatherChanging;

		// Token: 0x0400019B RID: 411
		private int m_globalWeatherIndex;

		// Token: 0x0400019C RID: 412
		[SerializeField]
		private float m_weatherTransitionProgress;

		// Token: 0x0400019D RID: 413
		private float m_weatherTransitionStart;

		// Token: 0x0400019E RID: 414
		private float m_weatherTransitionLength;

		// Token: 0x0400019F RID: 415
		private Vector3 m_weatherZoneTriggerPosition = Vector3.zero;

		// Token: 0x040001A0 RID: 416
		private Collider m_weatherZoneCollider;

		// Token: 0x040001A1 RID: 417
		private float m_weatherZoneClosestDistanceSqr;

		// Token: 0x040001A2 RID: 418
		private Vector3 m_weatherZoneClosestPoint = Vector3.zero;

		// Token: 0x040001A3 RID: 419
		private float m_weatherZoneDistance;

		// Token: 0x040001A4 RID: 420
		private float m_weatherZoneBlendDistanceSqr;

		// Token: 0x040001A5 RID: 421
		private float m_weatherZoneInterpolationFactor;

		// Token: 0x040001A6 RID: 422
		private float m_weatherGroupRampTime;

		// Token: 0x040001A7 RID: 423
		private Vector3 m_vector3Angle;
	}
}
