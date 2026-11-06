using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000010 RID: 16
	public static class TMPro_EventManager
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x000169D5 File Offset: 0x00014BD5
		public static void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Call(isChanged, mat);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000169E3 File Offset: 0x00014BE3
		public static void ON_FONT_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TMPro_EventManager.FONT_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000169F1 File Offset: 0x00014BF1
		public static void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000169FF File Offset: 0x00014BFF
		public static void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00016A0D File Offset: 0x00014C0D
		public static void ON_DRAG_AND_DROP_MATERIAL_CHANGED(GameObject sender, Material currentMaterial, Material newMaterial)
		{
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Call(sender, currentMaterial, newMaterial);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00016A1C File Offset: 0x00014C1C
		public static void ON_TEXT_STYLE_PROPERTY_CHANGED(bool isChanged)
		{
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Call(isChanged);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00016A29 File Offset: 0x00014C29
		public static void ON_COLOR_GRADIENT_PROPERTY_CHANGED(Object obj)
		{
			TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Call(obj);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00016A36 File Offset: 0x00014C36
		public static void ON_TEXT_CHANGED(Object obj)
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Call(obj);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00016A43 File Offset: 0x00014C43
		public static void ON_TMP_SETTINGS_CHANGED()
		{
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Call();
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00016A4F File Offset: 0x00014C4F
		public static void ON_RESOURCES_LOADED()
		{
			TMPro_EventManager.RESOURCE_LOAD_EVENT.Call();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00016A5B File Offset: 0x00014C5B
		public static void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00016A69 File Offset: 0x00014C69
		public static void ON_COMPUTE_DT_EVENT(object Sender, Compute_DT_EventArgs e)
		{
			TMPro_EventManager.COMPUTE_DT_EVENT.Call(Sender, e);
		}

		// Token: 0x04000081 RID: 129
		public static readonly FastAction<object, Compute_DT_EventArgs> COMPUTE_DT_EVENT = new FastAction<object, Compute_DT_EventArgs>();

		// Token: 0x04000082 RID: 130
		public static readonly FastAction<bool, Material> MATERIAL_PROPERTY_EVENT = new FastAction<bool, Material>();

		// Token: 0x04000083 RID: 131
		public static readonly FastAction<bool, Object> FONT_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000084 RID: 132
		public static readonly FastAction<bool, Object> SPRITE_ASSET_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000085 RID: 133
		public static readonly FastAction<bool, Object> TEXTMESHPRO_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000086 RID: 134
		public static readonly FastAction<GameObject, Material, Material> DRAG_AND_DROP_MATERIAL_EVENT = new FastAction<GameObject, Material, Material>();

		// Token: 0x04000087 RID: 135
		public static readonly FastAction<bool> TEXT_STYLE_PROPERTY_EVENT = new FastAction<bool>();

		// Token: 0x04000088 RID: 136
		public static readonly FastAction<Object> COLOR_GRADIENT_PROPERTY_EVENT = new FastAction<Object>();

		// Token: 0x04000089 RID: 137
		public static readonly FastAction TMP_SETTINGS_PROPERTY_EVENT = new FastAction();

		// Token: 0x0400008A RID: 138
		public static readonly FastAction RESOURCE_LOAD_EVENT = new FastAction();

		// Token: 0x0400008B RID: 139
		public static readonly FastAction<bool, Object> TEXTMESHPRO_UGUI_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x0400008C RID: 140
		public static readonly FastAction<Object> TEXT_CHANGED_EVENT = new FastAction<Object>();
	}
}
