using System;
using System.Collections.Generic;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000023 RID: 35
	[ExecuteInEditMode]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Weather Preset")]
	public sealed class AzureWeatherPreset : MonoBehaviour
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001CA RID: 458 RVA: 0x000074DB File Offset: 0x000056DB
		// (set) Token: 0x060001CB RID: 459 RVA: 0x000074E3 File Offset: 0x000056E3
		public AzureCoreSystem azureCoreSystem
		{
			get
			{
				return this.m_azureCoreSystem;
			}
			set
			{
				this.m_azureCoreSystem = value;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000074EC File Offset: 0x000056EC
		// (set) Token: 0x060001CD RID: 461 RVA: 0x000074F4 File Offset: 0x000056F4
		public List<AzurePropertyGroupData> propertyGroupDataList
		{
			get
			{
				return this.m_propertyGroupDataList;
			}
			set
			{
				this.m_propertyGroupDataList = value;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000074FD File Offset: 0x000056FD
		private void Reset()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000074FF File Offset: 0x000056FF
		private void Awake()
		{
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007501 File Offset: 0x00005701
		private void OnDestroy()
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007504 File Offset: 0x00005704
		public void InitPropertyList()
		{
			this.m_azureCoreSystem = base.GetComponentInParent<AzureCoreSystem>();
			if (this.m_azureCoreSystem == null)
			{
				this.m_azureCoreSystem = Object.FindObjectOfType<AzureCoreSystem>();
			}
			if (this.m_azureCoreSystem == null)
			{
				return;
			}
			if (this.m_azureCoreSystem)
			{
				if (this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList.Count == this.m_propertyGroupDataList.Count)
				{
					for (int i = 0; i < this.m_propertyGroupDataList.Count; i++)
					{
						if (this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList.Count == this.m_propertyGroupDataList[i].propertyDataList.Count)
						{
							for (int j = 0; j < this.m_propertyGroupDataList[i].propertyDataList.Count; j++)
							{
								this.m_propertyGroupDataList[i].propertyDataList[j].weatherPropertyOwner = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList[j];
							}
						}
						else
						{
							for (int k = 0; k < this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList.Count; k++)
							{
								if (this.m_propertyGroupDataList[i].propertyDataList.Count < this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList.Count)
								{
									AzurePropertyData azurePropertyData = new AzurePropertyData();
									this.m_propertyGroupDataList[i].propertyDataList.Add(azurePropertyData);
									azurePropertyData.weatherPropertyOwner = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList[k];
									this.InitNewPropertyData(azurePropertyData);
								}
								else
								{
									this.m_propertyGroupDataList[i].propertyDataList.RemoveAt(k);
								}
								this.m_propertyGroupDataList[i].propertyDataList[k].weatherPropertyOwner = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].weatherPropertyList[k];
							}
						}
						this.m_propertyGroupDataList[i].name = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[i].name;
					}
					return;
				}
				for (int l = 0; l < this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList.Count; l++)
				{
					if (this.m_propertyGroupDataList.Count < this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList.Count)
					{
						AzurePropertyGroupData azurePropertyGroupData = new AzurePropertyGroupData();
						this.m_propertyGroupDataList.Add(azurePropertyGroupData);
						for (int m = 0; m < this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[l].weatherPropertyList.Count; m++)
						{
							AzurePropertyData azurePropertyData2 = new AzurePropertyData();
							azurePropertyGroupData.propertyDataList.Add(azurePropertyData2);
							azurePropertyData2.weatherPropertyOwner = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[l].weatherPropertyList[m];
							this.InitNewPropertyData(azurePropertyData2);
						}
					}
					else
					{
						this.m_propertyGroupDataList.RemoveAt(l);
					}
					this.m_propertyGroupDataList[l].name = this.m_azureCoreSystem.weatherSystem.weatherPropertyGroupList[l].name;
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007880 File Offset: 0x00005A80
		private void InitNewPropertyData(AzurePropertyData propertyData)
		{
			propertyData.floatData = propertyData.weatherPropertyOwner.defaultFloatValue;
			propertyData.colorData = propertyData.weatherPropertyOwner.defaultColorValue;
			propertyData.curveData = new AnimationCurve();
			propertyData.curveData.CopyFrom(propertyData.weatherPropertyOwner.defaultCurveValue);
			propertyData.gradientData = new Gradient();
			propertyData.gradientData.SetKeys(propertyData.weatherPropertyOwner.defaultGradientValue.colorKeys, propertyData.weatherPropertyOwner.defaultGradientValue.alphaKeys);
			propertyData.vector3Data = propertyData.weatherPropertyOwner.defaultVector3Value;
		}

		// Token: 0x04000164 RID: 356
		[SerializeField]
		private AzureCoreSystem m_azureCoreSystem;

		// Token: 0x04000165 RID: 357
		[SerializeField]
		private List<AzurePropertyGroupData> m_propertyGroupDataList = new List<AzurePropertyGroupData>();
	}
}
