using System;
using UnityEngine;

namespace Broccoli.Controller
{
	// Token: 0x02000175 RID: 373
	[ExecuteInEditMode]
	public class BroccoTreeController_1_2_5 : MonoBehaviour
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0005160D File Offset: 0x0004F80D
		private bool hasSpeedTreeWind
		{
			get
			{
				return this.shaderType == BroccoTreeController_1_2_5.ShaderType.SpeedTree7OrCompatible || this.shaderType == BroccoTreeController_1_2_5.ShaderType.SpeedTree8OrCompatible;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00051623 File Offset: 0x0004F823
		private bool hasTreeCreatorWind
		{
			get
			{
				return this.shaderType == BroccoTreeController_1_2_5.ShaderType.TreeCreatorOrCompatible;
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00004F31 File Offset: 0x00003131
		public void Awake()
		{
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x000517BC File Offset: 0x0004F9BC
		public void Start()
		{
			this._renderer = base.GetComponent<Renderer>();
			if (this._renderer != null && this.shaderType != BroccoTreeController_1_2_5.ShaderType.Standard)
			{
				this._propBlock = new MaterialPropertyBlock();
				this._renderer.GetPropertyBlock(this._propBlock);
				if (this.hasSpeedTreeWind)
				{
					this.SetupSpeedTreeWind(true);
					return;
				}
				if (this.hasTreeCreatorWind)
				{
					this.SetupTreeCreatorWind(true);
				}
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00051826 File Offset: 0x0004FA26
		private void Update()
		{
			if (this._renderer != null && this._renderer.isVisible && this.hasSpeedTreeWind)
			{
				this.UpdateSpeedTreeWind();
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00004F31 File Offset: 0x00003131
		private void EditorUpdate()
		{
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00051854 File Offset: 0x0004FA54
		public void UpdateWind()
		{
			bool flag = this.windQuality > BroccoTreeController_1_2_5.WindQuality.None;
			if (this.hasSpeedTreeWind)
			{
				this.SetupSpeedTreeWind(flag);
				return;
			}
			this.SetupTreeCreatorWind(flag);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00051884 File Offset: 0x0004FA84
		public void SetupTreeCreatorWind(bool enable = true)
		{
			this.wind = Vector4.zero;
			Vector4 zero = Vector4.zero;
			if (enable)
			{
				Vector4 zero2 = Vector4.zero;
				WindZone[] array = Object.FindObjectsOfType<WindZone>();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].gameObject.activeSelf && array[i].mode == WindZoneMode.Directional)
					{
						zero = new Vector4(array[i].transform.forward.x, array[i].transform.forward.y, array[i].transform.forward.z, 1f);
						float num = array[i].windMain * (array[i].windPulseMagnitude + 1f);
						float num2 = Mathf.Cos(array[i].windPulseFrequency * 3.1415927f) * Mathf.Cos(array[i].windPulseFrequency * 3f * 3.1415927f) * Mathf.Cos(array[i].windPulseFrequency * 5f * 3.1415927f) + Mathf.Sin(array[i].windPulseFrequency * 25f * 3.1415927f) * 0.1f;
						num *= num2;
						zero2 = new Vector4(zero.x * num, zero.y * num, zero.z * num, array[i].windTurbulence * (array[i].windPulseMagnitude + 1f));
						this.wind += zero2;
					}
				}
			}
			if (this._propBlock != null)
			{
				this._propBlock.SetVector("_Wind", this.wind);
				this._renderer.SetPropertyBlock(this._propBlock);
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00051A54 File Offset: 0x0004FC54
		public void SetupSpeedTreeWind(bool enable = true)
		{
			if (this._propBlock == null)
			{
				return;
			}
			if (!enable)
			{
				this._propBlock.SetFloat(BroccoTreeController_1_2_5.propWindEnabled, 0f);
				this._renderer.SetPropertyBlock(this._propBlock);
				return;
			}
			this.GetWindZoneValues();
			this._propBlock.SetFloat(BroccoTreeController_1_2_5.propWindEnabled, enable ? 1f : 0f);
			this._propBlock.SetFloat(BroccoTreeController_1_2_5.propWindQuality, (float)this.windQuality);
			this.valueSTWindVector = this.valueWindDirection;
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindVector, this.valueSTWindVector);
			this.valueSTWindGlobal = new Vector4(Time.time * 0.36f, this.baseWindAmplitude * this.localWindAmplitude * BroccoTreeController_1_2_5.globalWindAmplitude * this.windMain, 0.0655f, 1.728f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindGlobal, this.valueSTWindGlobal);
			this.valueSTWindBranch = new Vector4(Time.time * 0.65f, 0.4102f, Time.time * 1.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranch, this.valueSTWindBranch);
			this.valueSTWindBranchTwitch = new Vector4(0.603f, 0.147f, 0.75f, 0.3f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranchTwitch, this.valueSTWindBranchTwitch);
			this.valueSTWindBranchWhip = new Vector4(0f, 0f, 0f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranchWhip, this.valueSTWindBranchWhip);
			this.valueSTWindBranchAnchor = new Vector4(0.034f, 0.4773f, 0.878f, 11.081f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranchAnchor, this.valueSTWindBranchAnchor);
			this.valueSTWindBranchAdherences = new Vector4(0.09295f, 0.1f, 0f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranchAdherences, this.valueSTWindBranchAdherences);
			this.valueSTWindTurbulences = new Vector4(0.7f, 0.3f, 0f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindTurbulences, this.valueSTWindTurbulences);
			this.valueSTWindLeaf1Ripple = new Vector4(Time.time * 3.18f, 0.044f, 0.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Ripple, this.valueSTWindLeaf1Ripple);
			this.valueSTWindLeaf2Ripple = new Vector4(Time.time * 4.7f, 0f, 0.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Ripple, this.valueSTWindLeaf2Ripple);
			this.valueSTWindLeaf1Tumble = new Vector4(Time.time * 0.84f, 0.1298f, 0.11403f, 0.11f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Tumble, this.valueSTWindLeaf1Tumble);
			this.valueSTWindLeaf2Tumble = new Vector4(Time.time, 0.035f, 0.035f, 0.5f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Tumble, this.valueSTWindLeaf2Tumble);
			this.valueSTWindLeaf1Twitch = new Vector4(0.3315f, 0.3246f, Time.time * 1.56f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Twitch, this.valueSTWindLeaf1Twitch);
			this.valueSTWindLeaf2Twitch = new Vector4(0.01745f, 33.3333f, Time.time * 0.31f, 12.896f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Twitch, this.valueSTWindLeaf2Twitch);
			this.valueSTWindFrondRipple = new Vector4(Time.time * -40.5f, 1.2192f, 10.34f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindFrondRipple, this.valueSTWindFrondRipple);
			this._renderer.SetPropertyBlock(this._propBlock);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00051E34 File Offset: 0x00050034
		private void SetWindQuality(bool enable = true)
		{
			if (this.shaderType == BroccoTreeController_1_2_5.ShaderType.SpeedTree8OrCompatible)
			{
				foreach (Material material in this._renderer.sharedMaterials)
				{
					material.DisableKeyword("_WINDQUALITY_NONE");
					material.DisableKeyword("_WINDQUALITY_FASTEST");
					material.DisableKeyword("_WINDQUALITY_FAST");
					material.DisableKeyword("_WINDQUALITY_BETTER");
					material.DisableKeyword("_WINDQUALITY_BEST");
					material.DisableKeyword("_WINDQUALITY_PALM");
					if (enable)
					{
						switch (this.windQuality)
						{
						case BroccoTreeController_1_2_5.WindQuality.None:
							material.EnableKeyword("_WINDQUALITY_NONE");
							break;
						case BroccoTreeController_1_2_5.WindQuality.Fastest:
							material.EnableKeyword("_WINDQUALITY_FASTEST");
							break;
						case BroccoTreeController_1_2_5.WindQuality.Fast:
							material.EnableKeyword("_WINDQUALITY_FAST");
							break;
						case BroccoTreeController_1_2_5.WindQuality.Better:
							material.EnableKeyword("_WINDQUALITY_BETTER");
							break;
						case BroccoTreeController_1_2_5.WindQuality.Best:
							material.EnableKeyword("_WINDQUALITY_BEST");
							break;
						case BroccoTreeController_1_2_5.WindQuality.Palm:
							material.EnableKeyword("_WINDQUALITY_PALM");
							break;
						}
					}
				}
			}
			else if (this.shaderType == BroccoTreeController_1_2_5.ShaderType.SpeedTree7OrCompatible)
			{
				foreach (Material material2 in this._renderer.sharedMaterials)
				{
					if (enable)
					{
						material2.EnableKeyword("ENABLE_WIND");
					}
					else
					{
						material2.DisableKeyword("ENABLE_WIND");
					}
				}
			}
			this._renderer.GetPropertyBlock(this._propBlock);
			this._propBlock.SetFloat(BroccoTreeController_1_2_5.propWindEnabled, enable ? 1f : 0f);
			this._propBlock.SetFloat(BroccoTreeController_1_2_5.propWindQuality, (float)this.windQuality);
			this._renderer.SetPropertyBlock(this._propBlock);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00051FC8 File Offset: 0x000501C8
		public void UpdateSpeedTreeWind()
		{
			if (this._propBlock == null)
			{
				return;
			}
			this.valueTime = Time.time;
			this.valueSTWindGlobal.x = this.valueTime * 0.36f;
			this.valueSTWindGlobal.y = this.baseWindAmplitude * this.localWindAmplitude * BroccoTreeController_1_2_5.globalWindAmplitude * this.windMain;
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindGlobal, this.valueSTWindGlobal);
			this._renderer.SetPropertyBlock(this._propBlock);
			this.valueSTWindBranch = new Vector4(this.valueTime * 0.65f, 0.4102f, this.valueTime * 1.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindBranch, this.valueSTWindBranch);
			this.valueSTWindLeaf1Ripple = new Vector4(this.valueTime * 3.18f, 0.044f, 0.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Ripple, this.valueSTWindLeaf1Ripple);
			this.valueSTWindLeaf2Ripple = new Vector4(this.valueTime * 4.7f, 0f, 0.5f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Ripple, this.valueSTWindLeaf2Ripple);
			this.valueSTWindLeaf1Tumble = new Vector4(this.valueTime * 0.84f, 0.1298f, 0.11403f, 0.11f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Tumble, this.valueSTWindLeaf1Tumble);
			this.valueSTWindLeaf2Tumble = new Vector4(this.valueTime, 0.035f, 0.035f, 0.5f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Tumble, this.valueSTWindLeaf2Tumble);
			this.valueSTWindLeaf1Twitch = new Vector4(0.3315f, 0.3246f, this.valueTime * 1.56f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf1Twitch, this.valueSTWindLeaf1Twitch);
			this.valueSTWindLeaf2Twitch = new Vector4(0.01745f, 33.3333f, this.valueTime * 0.31f, 12.896f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindLeaf2Twitch, this.valueSTWindLeaf2Twitch);
			this.valueSTWindFrondRipple = new Vector4(this.valueTime * -40.5f, 1.2192f, 10.34f, 0f);
			this._propBlock.SetVector(BroccoTreeController_1_2_5.propSTWindFrondRipple, this.valueSTWindFrondRipple);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00052230 File Offset: 0x00050430
		public void GetWindZoneValues()
		{
			this.valueWindDirection = new Vector4(1f, 0f, 0f, 0f);
			WindZone[] array = Object.FindObjectsOfType<WindZone>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.activeSelf && array[i].mode == WindZoneMode.Directional)
				{
					this.windMain = array[i].windMain;
					this.valueWindDirection = new Vector4(array[i].transform.forward.x, array[i].transform.forward.y, array[i].transform.forward.z, 1f);
				}
			}
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00004F31 File Offset: 0x00003131
		public void EditorWindAnimate(bool enabled)
		{
		}

		// Token: 0x04000B99 RID: 2969
		public string version = "";

		// Token: 0x04000B9A RID: 2970
		public BroccoTreeController_1_2_5.ShaderType shaderType;

		// Token: 0x04000B9B RID: 2971
		[HideInInspector]
		public bool editorWindAlways;

		// Token: 0x04000B9C RID: 2972
		private Renderer _renderer;

		// Token: 0x04000B9D RID: 2973
		private MaterialPropertyBlock _propBlock;

		// Token: 0x04000B9E RID: 2974
		private Vector4 wind = Vector4.zero;

		// Token: 0x04000B9F RID: 2975
		[HideInInspector]
		public bool editorWindEnabled;

		// Token: 0x04000BA0 RID: 2976
		public BroccoTreeController_1_2_5.WindType windType;

		// Token: 0x04000BA1 RID: 2977
		public BroccoTreeController_1_2_5.WindQuality windQuality = BroccoTreeController_1_2_5.WindQuality.Better;

		// Token: 0x04000BA2 RID: 2978
		private float baseWindAmplitude = 0.2752f;

		// Token: 0x04000BA3 RID: 2979
		public float localWindAmplitude = 1f;

		// Token: 0x04000BA4 RID: 2980
		public static float globalWindAmplitude = 1f;

		// Token: 0x04000BA5 RID: 2981
		public float windMain;

		// Token: 0x04000BA6 RID: 2982
		private float valueTime;

		// Token: 0x04000BA7 RID: 2983
		private Vector4 valueWindDirection = Vector4.zero;

		// Token: 0x04000BA8 RID: 2984
		private Vector4 valueSTWindVector = Vector4.zero;

		// Token: 0x04000BA9 RID: 2985
		private Vector4 valueSTWindGlobal = Vector4.zero;

		// Token: 0x04000BAA RID: 2986
		private Vector4 valueSTWindBranch = Vector4.zero;

		// Token: 0x04000BAB RID: 2987
		private Vector4 valueSTWindBranchTwitch = Vector4.zero;

		// Token: 0x04000BAC RID: 2988
		private Vector4 valueSTWindBranchWhip = Vector4.zero;

		// Token: 0x04000BAD RID: 2989
		private Vector4 valueSTWindBranchAnchor = Vector4.zero;

		// Token: 0x04000BAE RID: 2990
		private Vector4 valueSTWindBranchAdherences = Vector4.zero;

		// Token: 0x04000BAF RID: 2991
		private Vector4 valueSTWindTurbulences = Vector4.zero;

		// Token: 0x04000BB0 RID: 2992
		private Vector4 valueSTWindLeaf1Ripple = Vector4.zero;

		// Token: 0x04000BB1 RID: 2993
		private Vector4 valueSTWindLeaf1Tumble = Vector4.zero;

		// Token: 0x04000BB2 RID: 2994
		private Vector4 valueSTWindLeaf1Twitch = Vector4.zero;

		// Token: 0x04000BB3 RID: 2995
		private Vector4 valueSTWindLeaf2Ripple = Vector4.zero;

		// Token: 0x04000BB4 RID: 2996
		private Vector4 valueSTWindLeaf2Tumble = Vector4.zero;

		// Token: 0x04000BB5 RID: 2997
		private Vector4 valueSTWindLeaf2Twitch = Vector4.zero;

		// Token: 0x04000BB6 RID: 2998
		private Vector4 valueSTWindFrondRipple = Vector4.zero;

		// Token: 0x04000BB7 RID: 2999
		private static int propWindEnabled = Shader.PropertyToID("_WindEnabled");

		// Token: 0x04000BB8 RID: 3000
		private static int propWindQuality = Shader.PropertyToID("_WindQuality");

		// Token: 0x04000BB9 RID: 3001
		private static int propSTWindVector = Shader.PropertyToID("_ST_WindVector");

		// Token: 0x04000BBA RID: 3002
		private static int propSTWindGlobal = Shader.PropertyToID("_ST_WindGlobal");

		// Token: 0x04000BBB RID: 3003
		private static int propSTWindBranch = Shader.PropertyToID("_ST_WindBranch");

		// Token: 0x04000BBC RID: 3004
		private static int propSTWindBranchTwitch = Shader.PropertyToID("_ST_WindBranchTwitch");

		// Token: 0x04000BBD RID: 3005
		private static int propSTWindBranchWhip = Shader.PropertyToID("_ST_WindBranchWhip");

		// Token: 0x04000BBE RID: 3006
		private static int propSTWindBranchAnchor = Shader.PropertyToID("_ST_WindBranchAnchor");

		// Token: 0x04000BBF RID: 3007
		private static int propSTWindBranchAdherences = Shader.PropertyToID("_ST_WindBranchAdherences");

		// Token: 0x04000BC0 RID: 3008
		private static int propSTWindTurbulences = Shader.PropertyToID("_ST_WindTurbulences");

		// Token: 0x04000BC1 RID: 3009
		private static int propSTWindLeaf1Ripple = Shader.PropertyToID("_ST_WindLeaf1Ripple");

		// Token: 0x04000BC2 RID: 3010
		private static int propSTWindLeaf1Tumble = Shader.PropertyToID("_ST_WindLeaf1Tumble");

		// Token: 0x04000BC3 RID: 3011
		private static int propSTWindLeaf1Twitch = Shader.PropertyToID("_ST_WindLeaf1Twitch");

		// Token: 0x04000BC4 RID: 3012
		private static int propSTWindLeaf2Ripple = Shader.PropertyToID("_ST_WindLeaf2Ripple");

		// Token: 0x04000BC5 RID: 3013
		private static int propSTWindLeaf2Tumble = Shader.PropertyToID("_ST_WindLeaf2Tumble");

		// Token: 0x04000BC6 RID: 3014
		private static int propSTWindLeaf2Twitch = Shader.PropertyToID("_ST_WindLeaf2Twitch");

		// Token: 0x04000BC7 RID: 3015
		private static int propSTWindFrondRipple = Shader.PropertyToID("_ST_WindFrondRipple");

		// Token: 0x02000176 RID: 374
		public enum ShaderType
		{
			// Token: 0x04000BC9 RID: 3017
			Standard,
			// Token: 0x04000BCA RID: 3018
			TreeCreatorOrCompatible,
			// Token: 0x04000BCB RID: 3019
			SpeedTree7OrCompatible,
			// Token: 0x04000BCC RID: 3020
			SpeedTree8OrCompatible,
			// Token: 0x04000BCD RID: 3021
			Billboard
		}

		// Token: 0x02000177 RID: 375
		public enum WindType
		{
			// Token: 0x04000BCF RID: 3023
			None,
			// Token: 0x04000BD0 RID: 3024
			TreeCreator,
			// Token: 0x04000BD1 RID: 3025
			ST7,
			// Token: 0x04000BD2 RID: 3026
			ST8
		}

		// Token: 0x02000178 RID: 376
		public enum WindQuality
		{
			// Token: 0x04000BD4 RID: 3028
			None,
			// Token: 0x04000BD5 RID: 3029
			Fastest,
			// Token: 0x04000BD6 RID: 3030
			Fast,
			// Token: 0x04000BD7 RID: 3031
			Better,
			// Token: 0x04000BD8 RID: 3032
			Best,
			// Token: 0x04000BD9 RID: 3033
			Palm
		}
	}
}
