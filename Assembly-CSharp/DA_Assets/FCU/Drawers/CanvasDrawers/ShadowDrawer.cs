using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C3 RID: 963
	[Serializable]
	public class ShadowDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060018B1 RID: 6321 RVA: 0x0009D584 File Offset: 0x0009B784
		public void Draw(FObject fobject)
		{
			if (this.monoBeh.Settings.ShadowSettings.ShadowComponent == ShadowComponent.TrueShadow)
			{
				this.DrawTrueShadow(fobject);
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00004F31 File Offset: 0x00003131
		private void DrawTrueShadow(FObject fobject)
		{
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0009D5A8 File Offset: 0x0009B7A8
		internal ShadowData GetShadowData(Effect effect)
		{
			ShadowData shadowData = default(ShadowData);
			shadowData.Offset = effect.Offset;
			shadowData.EffectType = effect.Type;
			float x = effect.Offset.x;
			float y = effect.Offset.y;
			float num = Mathf.Atan2(y, x) * 57.295776f;
			float num2 = Mathf.Sqrt(x * x + y * y);
			shadowData.Angle = num;
			shadowData.Distance = num2;
			shadowData.Spread = effect.Spread.ToFloat();
			shadowData.Color = effect.Color;
			shadowData.Radius = effect.Radius;
			return shadowData;
		}
	}
}
