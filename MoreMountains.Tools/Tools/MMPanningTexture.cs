using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000098 RID: 152
	[AddComponentMenu("More Mountains/Tools/VFX/PanningTexture")]
	public class MMPanningTexture : MonoBehaviour
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00010744 File Offset: 0x0000E944
		protected virtual void Start()
		{
			this._renderer = base.GetComponent<Renderer>();
			if (this._renderer != null && !string.IsNullOrEmpty(this.SortingLayerName))
			{
				this._renderer.sortingLayerName = this.SortingLayerName;
				this._propertyBlock = new MaterialPropertyBlock();
				this._renderer.GetPropertyBlock(this._propertyBlock);
			}
			this._position.x = this._renderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).x;
			this._position.y = this._renderer.sharedMaterials[this.MaterialIndex].GetVector(this.MaterialPropertyName).y;
			this._rawImage = base.GetComponent<RawImage>();
			this._speed = new Vector4(0f, 0f, this.Speed.x, this.Speed.y);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00010838 File Offset: 0x0000EA38
		protected virtual void Update()
		{
			if (!this.TextureShouldPan)
			{
				return;
			}
			bool flag = this._rawImage == null;
			if (flag && this._renderer == null)
			{
				return;
			}
			this._speed.z = this.Speed.x;
			this._speed.w = this.Speed.y;
			this._position += this._speed / 300f * Time.deltaTime;
			if (this._position.z > 1f)
			{
				this._position.z = this._position.z - 1f;
			}
			if (this._position.w > 1f)
			{
				this._position.w = this._position.w - 1f;
			}
			if (this._renderer != null)
			{
				this._renderer.GetPropertyBlock(this._propertyBlock);
				this._propertyBlock.SetVector(this.MaterialPropertyName, this._position);
				this._renderer.SetPropertyBlock(this._propertyBlock, this.MaterialIndex);
			}
			if (!flag)
			{
				this._rawImage.material.mainTextureOffset = this._position;
			}
		}

		// Token: 0x04000479 RID: 1145
		[MMInformation("This script will let you pan a texture on an attached Renderer.", MMInformationAttribute.InformationType.Info, false)]
		public bool TextureShouldPan = true;

		// Token: 0x0400047A RID: 1146
		public Vector2 Speed = new Vector2(10f, 10f);

		// Token: 0x0400047B RID: 1147
		public string SortingLayerName = "Above";

		// Token: 0x0400047C RID: 1148
		[Tooltip("the property name, for example _MainTex")]
		public string MaterialPropertyName = "_MainTex_ST";

		// Token: 0x0400047D RID: 1149
		[Tooltip("the index of the material")]
		public int MaterialIndex;

		// Token: 0x0400047E RID: 1150
		protected RawImage _rawImage;

		// Token: 0x0400047F RID: 1151
		protected Renderer _renderer;

		// Token: 0x04000480 RID: 1152
		protected Vector4 _position = Vector4.one;

		// Token: 0x04000481 RID: 1153
		protected Vector4 _speed;

		// Token: 0x04000482 RID: 1154
		protected MaterialPropertyBlock _propertyBlock;
	}
}
