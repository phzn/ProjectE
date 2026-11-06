using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200084D RID: 2125
	public class ColorPickerControl : MonoBehaviour
	{
		// Token: 0x06003842 RID: 14402 RVA: 0x00148BB4 File Offset: 0x00146DB4
		private void Start()
		{
			this.CreateHueImage();
			this.CreateSVImage();
			this.CreateOutputImage();
			this.UpdateOutputImage();
			if (this.colorObject == null)
			{
				base.gameObject.SetActive(false);
			}
			foreach (object obj in Enum.GetValues(typeof(OutfitType)))
			{
				OutfitType outfitType = (OutfitType)obj;
				this.outfitTypes.Add(outfitType.ToString());
			}
			this.CopyCatagoryText.text = this.outfitTypes[0];
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x00148C6C File Offset: 0x00146E6C
		private void CreateHueImage()
		{
			this.hueTexture = new Texture2D(1, 16);
			this.hueTexture.wrapMode = TextureWrapMode.Clamp;
			this.hueTexture.name = "HueTexture";
			for (int i = 0; i < this.hueTexture.height; i++)
			{
				this.hueTexture.SetPixel(0, i, Color.HSVToRGB((float)i / (float)this.hueTexture.height, 1f, 1f));
			}
			this.hueTexture.Apply();
			this.currentHue = 0f;
			this.hueImage.texture = this.hueTexture;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00148D0C File Offset: 0x00146F0C
		private void CreateSVImage()
		{
			this.svTexture = new Texture2D(16, 16);
			this.svTexture.wrapMode = TextureWrapMode.Clamp;
			this.svTexture.name = "SVTexture";
			for (int i = 0; i < this.svTexture.height; i++)
			{
				for (int j = 0; j < this.svTexture.width; j++)
				{
					this.svTexture.SetPixel(j, i, Color.HSVToRGB(this.currentHue, (float)j / (float)this.svTexture.width, (float)i / (float)this.svTexture.height));
				}
			}
			this.svTexture.Apply();
			this.currentSat = 0f;
			this.currentVal = 0f;
			this.satValImage.texture = this.svTexture;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x00148DD8 File Offset: 0x00146FD8
		private void CreateOutputImage()
		{
			this.outputTexture = new Texture2D(1, 16);
			this.outputTexture.wrapMode = TextureWrapMode.Clamp;
			this.outputTexture.name = "OutputTexture";
			Color color = Color.HSVToRGB(this.currentHue, this.currentSat, this.currentVal);
			for (int i = 0; i < this.hueTexture.height; i++)
			{
				this.outputTexture.SetPixel(0, 1, color);
			}
			this.outputTexture.Apply();
			this.currentHue = 0f;
			this.outputImage.texture = this.outputTexture;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00148E74 File Offset: 0x00147074
		private void UpdateOutputImage()
		{
			Color color = Color.HSVToRGB(this.currentHue, this.currentSat, this.currentVal);
			for (int i = 0; i < this.outputTexture.height; i++)
			{
				this.outputTexture.SetPixel(0, i, color);
			}
			this.outputTexture.Apply();
			if (!this.colorObject)
			{
				return;
			}
			this.Swatch.color = color;
			this.SetColor(color, this.currentChannel);
			this.channelText.color = new Color(1f - this.currentVal, 1f - this.currentVal, 1f - this.currentVal, 1f);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00148F28 File Offset: 0x00147128
		public void SetSV(float S, float V)
		{
			this.currentSat = S;
			this.currentVal = V;
			this.UpdateOutputImage();
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00148F3E File Offset: 0x0014713E
		public void SetHSV(float H, float S, float V)
		{
			this.currentHue = H;
			this.currentSat = S;
			this.currentVal = V;
			this.UpdateOutputImage();
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00148F5C File Offset: 0x0014715C
		public void UpdateSVImage()
		{
			this.currentHue = this.hueSlider.value;
			for (int i = 0; i < this.svTexture.height; i++)
			{
				for (int j = 0; j < this.svTexture.width; j++)
				{
					this.svTexture.SetPixel(j, i, Color.HSVToRGB(this.currentHue, (float)j / (float)this.svTexture.width, (float)i / (float)this.svTexture.height));
				}
			}
			this.svTexture.Apply();
			this.UpdateOutputImage();
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00148FF0 File Offset: 0x001471F0
		private void SetColor(Color color, int channel)
		{
			switch (this.mode)
			{
			case TextureType.Base:
				this.colorObject.SetColor(color, channel, false);
				return;
			case TextureType.Decal:
				this.maxChannel = 3;
				this.colorObject.SetDecalColor(color, channel);
				return;
			case TextureType.Pattern:
				this.maxChannel = 3;
				this.colorObject.SetPatternColor(color, channel);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x00149050 File Offset: 0x00147250
		public void ChangeSwatch(int value)
		{
			this.currentChannel += value;
			if (this.currentChannel > this.maxChannel)
			{
				this.currentChannel = 1;
			}
			if (this.currentChannel < 1)
			{
				this.currentChannel = this.maxChannel;
			}
			Color color = Color.black;
			switch (this.mode)
			{
			case TextureType.Base:
				color = this.colorObject.GetColor(this.currentChannel);
				break;
			case TextureType.Decal:
				color = this.colorObject.GetDecalColor(this.currentChannel);
				break;
			case TextureType.Pattern:
				color = this.colorObject.GetPatternColor(this.currentChannel);
				break;
			}
			this.Swatch.color = color;
			this.channelText.text = this.currentChannel.ToString() + "/" + this.maxChannel.ToString();
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(color, out num, out num2, out num3);
			this.hueSlider.value = num;
			this.SetHSV(num, num2, num3);
			this.UpdateSVImage();
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x00149150 File Offset: 0x00147350
		public void ChangeObject(OutfitBase ob)
		{
			if (!ob)
			{
				this.RemoveObject();
				return;
			}
			Outfit component = ob.GetComponent<Outfit>();
			if (this.maintainColors && !component.customShader && this.colorObject != null)
			{
				this.HeldColors = this.colorObject.GetColors();
				if (component.Type == this.outfitType)
				{
					for (int i = 0; i < this.HeldColors.Count; i++)
					{
						component.SetColor(this.HeldColors[i], i + 1, false);
					}
				}
			}
			this.channelNames = new string[] { "Base" };
			this.outfitType = component.Type;
			this.mode = TextureType.Base;
			this.colorObject = ob;
			if (!this.outfitPickerSettings.ContainsKey(component.Type))
			{
				ColorPickerControl.OutfitPickerSettings outfitPickerSettings = new ColorPickerControl.OutfitPickerSettings();
				this.outfitPickerSettings.Add(component.Type, outfitPickerSettings);
			}
			ColorPickerControl.OutfitPickerSettings outfitPickerSettings2 = this.outfitPickerSettings[component.Type];
			this.SetMaintainColors(outfitPickerSettings2.maintainColors);
			this.SetCopyIndex(outfitPickerSettings2.copyIndex);
			if (this.colorObject.customShader)
			{
				this.decalContainer.SetActive(false);
				this.patternContainer.SetActive(false);
				this.swatchParentContainer.SetActive(true);
				this.maxChannel = 1;
				foreach (SwatchSelector swatchSelector in this.swatchSelectors)
				{
					Object.Destroy(swatchSelector.gameObject);
				}
				this.swatchSelectors.Clear();
				for (int j = 0; j < component.outfitSwatches.Count; j++)
				{
					SwatchSelector swatchSelector2 = Object.Instantiate<SwatchSelector>(this.swatchSelectorObject, this.swatchContainer);
					swatchSelector2.Init(this, component.outfitSwatches[j], j);
					this.swatchSelectors.Add(swatchSelector2);
				}
			}
			else
			{
				Color color = this.colorObject.GetColor(1);
				color.a = 1f;
				this.currentChannel = 1;
				this.Swatch.color = color;
				this.decalContainer.SetActive(true);
				this.patternContainer.SetActive(true);
				this.channelNames = component.ColorChannels;
				if (component)
				{
					this.maxChannel = component.ColorChannels.Length;
					if (!component.supportDecals)
					{
						this.decalContainer.SetActive(false);
					}
					if (!component.supportPatterns)
					{
						this.patternContainer.SetActive(false);
					}
					this.swatchParentContainer.SetActive(false);
				}
				else
				{
					this.maxChannel = 9;
				}
				Vector4 decalSize = this.colorObject.GetDecalSize();
				this.DecalXSlider.value = decalSize.x;
				this.DecalYSlider.value = decalSize.y;
				Vector4 patternSize = this.colorObject.GetPatternSize();
				this.patternXSlider.value = patternSize.x;
				this.patternYSlider.value = patternSize.y;
			}
			this.SetChannelName();
			this.ChangeSwatch(0);
			this.objectName.text = this.colorObject.name.Replace("(Clone)", "");
			if (this.colorObject == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x001494AC File Offset: 0x001476AC
		public void SetDecalSize()
		{
			Vector2 vector = new Vector2(this.DecalXSlider.value, this.DecalYSlider.value);
			this.colorObject.SetDecalSize(vector);
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x001494E8 File Offset: 0x001476E8
		public void SetPatternSize()
		{
			Vector2 vector = new Vector2(this.patternXSlider.value, this.patternYSlider.value);
			this.colorObject.SetPatternSize(vector);
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x0014951E File Offset: 0x0014771E
		public void RemoveObject()
		{
			this.colorObject = null;
			this.SetMaintainColors(false);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0014953A File Offset: 0x0014773A
		public void SetBaseTexture(int textureIndex)
		{
			this.colorObject.GetComponent<Outfit>().SetSwatch(textureIndex, false);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00149550 File Offset: 0x00147750
		public void SwitchMode(string mode)
		{
			TextureType textureType = (TextureType)Enum.Parse(typeof(TextureType), mode);
			this.mode = textureType;
			switch (this.mode)
			{
			case TextureType.Base:
				this.ChangeObject(this.colorObject);
				return;
			case TextureType.Decal:
			{
				this.maxChannel = 3;
				this.channelText.text = "1/" + this.maxChannel.ToString();
				Color decalColor = this.colorObject.GetDecalColor(1);
				decalColor.a = 1f;
				this.currentChannel = 1;
				this.Swatch.color = decalColor;
				return;
			}
			case TextureType.Pattern:
			{
				this.maxChannel = 3;
				this.channelText.text = "1/" + this.maxChannel.ToString();
				Color patternColor = this.colorObject.GetPatternColor(1);
				patternColor.a = 1f;
				this.currentChannel = 1;
				this.Swatch.color = patternColor;
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x00149648 File Offset: 0x00147848
		public void ChangeCopyIndex(int value)
		{
			this.copyIndex += value;
			if (this.copyIndex > this.outfitTypes.Count - 1)
			{
				this.copyIndex = 0;
			}
			else if (this.copyIndex < 0)
			{
				this.copyIndex = this.outfitTypes.Count - 1;
			}
			this.CopyCatagoryText.text = this.outfitTypes[this.copyIndex];
		}

		// Token: 0x06003853 RID: 14419 RVA: 0x001496BC File Offset: 0x001478BC
		public void CopyColor(OutfitBase copyOutfit)
		{
			OutfitBase outfitBase = this.colorObject;
			for (int i = 1; i < 9; i++)
			{
				copyOutfit.SetColor(outfitBase.GetColor(i), i, false);
			}
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x001496F0 File Offset: 0x001478F0
		public void CopyColor()
		{
			OutfitBase outfitBase = this.colorObject;
			Outfit outfit = this.outfitSystem.GetOutfit(this.CopyCatagoryText.text);
			if (outfit == null)
			{
				return;
			}
			for (int i = 1; i < 9; i++)
			{
				outfit.SetColor(outfitBase.GetColor(i), i, false);
			}
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x00149744 File Offset: 0x00147944
		public void SetCopyIndex(int value)
		{
			this.copyIndex = value;
			this.CopyCatagoryText.text = this.outfitTypes[this.copyIndex];
			this.outfitPickerSettings[this.outfitType].copyIndex = this.copyIndex;
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x00149790 File Offset: 0x00147990
		public void SetChannelName()
		{
			string text = "";
			switch (this.mode)
			{
			case TextureType.Base:
				if (!this.colorObject.customShader && this.currentChannel - 1 < this.channelNames.Length)
				{
					text = this.channelNames[this.currentChannel - 1];
				}
				break;
			case TextureType.Decal:
				text = "Decal";
				break;
			case TextureType.Pattern:
				text = "Pattern";
				break;
			}
			this.channelText.text = string.Concat(new string[]
			{
				text,
				" ",
				this.currentChannel.ToString(),
				"/",
				this.maxChannel.ToString()
			});
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x00149844 File Offset: 0x00147A44
		public void SetMaintainColors(bool value)
		{
			if (value && this.colorObject != null)
			{
				this.HeldColors = this.colorObject.GetColors();
			}
			this.maintainColors = value;
			this.maintainColorsToggle.isOn = this.maintainColors;
			if (this.outfitType != null)
			{
				this.outfitPickerSettings[this.outfitType].maintainColors = this.maintainColors;
			}
		}

		// Token: 0x04003927 RID: 14631
		[Header("Picker")]
		public float currentHue;

		// Token: 0x04003928 RID: 14632
		public float currentSat;

		// Token: 0x04003929 RID: 14633
		public float currentVal;

		// Token: 0x0400392A RID: 14634
		public float currentColor;

		// Token: 0x0400392B RID: 14635
		[SerializeField]
		private RawImage hueImage;

		// Token: 0x0400392C RID: 14636
		[SerializeField]
		private RawImage satValImage;

		// Token: 0x0400392D RID: 14637
		[SerializeField]
		private RawImage outputImage;

		// Token: 0x0400392E RID: 14638
		[SerializeField]
		private Slider hueSlider;

		// Token: 0x0400392F RID: 14639
		private Texture2D hueTexture;

		// Token: 0x04003930 RID: 14640
		private Texture2D svTexture;

		// Token: 0x04003931 RID: 14641
		private Texture2D outputTexture;

		// Token: 0x04003932 RID: 14642
		public OutfitBase colorObject;

		// Token: 0x04003933 RID: 14643
		public OutfitType outfitType;

		// Token: 0x04003934 RID: 14644
		public Material colorMaterial;

		// Token: 0x04003935 RID: 14645
		public int MaterialSlot;

		// Token: 0x04003936 RID: 14646
		[Header("Editor")]
		[SerializeField]
		private OutfitSystem outfitSystem;

		// Token: 0x04003937 RID: 14647
		[SerializeField]
		private TMP_Text objectName;

		// Token: 0x04003938 RID: 14648
		[SerializeField]
		private TMP_Text channelText;

		// Token: 0x04003939 RID: 14649
		[SerializeField]
		private Image Swatch;

		// Token: 0x0400393A RID: 14650
		[SerializeField]
		private TMP_Text CopyCatagoryText;

		// Token: 0x0400393B RID: 14651
		private List<string> outfitTypes = new List<string>();

		// Token: 0x0400393C RID: 14652
		private int copyIndex;

		// Token: 0x0400393D RID: 14653
		[SerializeField]
		private TextureType mode;

		// Token: 0x0400393E RID: 14654
		[SerializeField]
		private int currentChannel;

		// Token: 0x0400393F RID: 14655
		[SerializeField]
		private int maxChannel;

		// Token: 0x04003940 RID: 14656
		[SerializeField]
		private string[] channelNames;

		// Token: 0x04003941 RID: 14657
		[Header("Decal Editor")]
		[SerializeField]
		private GameObject decalContainer;

		// Token: 0x04003942 RID: 14658
		[SerializeField]
		private Slider DecalXSlider;

		// Token: 0x04003943 RID: 14659
		[SerializeField]
		private Slider DecalYSlider;

		// Token: 0x04003944 RID: 14660
		private Vector2 outfitDefaultDecalSize;

		// Token: 0x04003945 RID: 14661
		[Header("Pattern Editor")]
		[SerializeField]
		private GameObject patternContainer;

		// Token: 0x04003946 RID: 14662
		[SerializeField]
		private Slider patternXSlider;

		// Token: 0x04003947 RID: 14663
		[SerializeField]
		private Slider patternYSlider;

		// Token: 0x04003948 RID: 14664
		private Vector2 outfitDefaultPatternSize;

		// Token: 0x04003949 RID: 14665
		[Header("Swatch Editor")]
		[SerializeField]
		private GameObject swatchParentContainer;

		// Token: 0x0400394A RID: 14666
		[SerializeField]
		private Transform swatchContainer;

		// Token: 0x0400394B RID: 14667
		[SerializeField]
		private SwatchSelector swatchSelectorObject;

		// Token: 0x0400394C RID: 14668
		private List<SwatchSelector> swatchSelectors = new List<SwatchSelector>();

		// Token: 0x0400394D RID: 14669
		[Header("AdvancedEditor")]
		[SerializeField]
		private List<Color> HeldColors = new List<Color>();

		// Token: 0x0400394E RID: 14670
		[SerializeField]
		private bool maintainColors;

		// Token: 0x0400394F RID: 14671
		[SerializeField]
		private Toggle maintainColorsToggle;

		// Token: 0x04003950 RID: 14672
		private Dictionary<OutfitType, ColorPickerControl.OutfitPickerSettings> outfitPickerSettings = new Dictionary<OutfitType, ColorPickerControl.OutfitPickerSettings>();

		// Token: 0x0200084E RID: 2126
		private class OutfitPickerSettings
		{
			// Token: 0x04003951 RID: 14673
			public bool maintainColors;

			// Token: 0x04003952 RID: 14674
			public int copyIndex;
		}
	}
}
