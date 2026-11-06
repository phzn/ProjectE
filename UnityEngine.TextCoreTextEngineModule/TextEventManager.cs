using System;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x02000024 RID: 36
	public static class TextEventManager
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00008C6C File Offset: 0x00006E6C
		public static void ON_PRE_RENDER_OBJECT_CHANGED()
		{
			TextEventManager.OnPreRenderObject_Event.Call();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00008C7A File Offset: 0x00006E7A
		public static void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			TextEventManager.MATERIAL_PROPERTY_EVENT.Call(isChanged, mat);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00008C8A File Offset: 0x00006E8A
		public static void ON_FONT_PROPERTY_CHANGED(bool isChanged, Object font)
		{
			TextEventManager.FONT_PROPERTY_EVENT.Call(isChanged, font);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00008C9A File Offset: 0x00006E9A
		public static void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TextEventManager.SPRITE_ASSET_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00008CAA File Offset: 0x00006EAA
		public static void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TextEventManager.TEXTMESHPRO_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00008CBA File Offset: 0x00006EBA
		public static void ON_DRAG_AND_DROP_MATERIAL_CHANGED(GameObject sender, Material currentMaterial, Material newMaterial)
		{
			TextEventManager.DRAG_AND_DROP_MATERIAL_EVENT.Call(sender, currentMaterial, newMaterial);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00008CCB File Offset: 0x00006ECB
		public static void ON_TEXT_STYLE_PROPERTY_CHANGED(bool isChanged)
		{
			TextEventManager.TEXT_STYLE_PROPERTY_EVENT.Call(isChanged);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00008CDA File Offset: 0x00006EDA
		public static void ON_COLOR_GRADIENT_PROPERTY_CHANGED(Object gradient)
		{
			TextEventManager.COLOR_GRADIENT_PROPERTY_EVENT.Call(gradient);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00008CE9 File Offset: 0x00006EE9
		public static void ON_TEXT_CHANGED(Object obj)
		{
			TextEventManager.TEXT_CHANGED_EVENT.Call(obj);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00008CF8 File Offset: 0x00006EF8
		public static void ON_TMP_SETTINGS_CHANGED()
		{
			TextEventManager.TMP_SETTINGS_PROPERTY_EVENT.Call();
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00008D06 File Offset: 0x00006F06
		public static void ON_RESOURCES_LOADED()
		{
			TextEventManager.RESOURCE_LOAD_EVENT.Call();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008D14 File Offset: 0x00006F14
		public static void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TextEventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x04000111 RID: 273
		public static readonly FastAction<bool, Material> MATERIAL_PROPERTY_EVENT = new FastAction<bool, Material>();

		// Token: 0x04000112 RID: 274
		public static readonly FastAction<bool, Object> FONT_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000113 RID: 275
		public static readonly FastAction<bool, Object> SPRITE_ASSET_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000114 RID: 276
		public static readonly FastAction<bool, Object> TEXTMESHPRO_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04000115 RID: 277
		public static readonly FastAction<GameObject, Material, Material> DRAG_AND_DROP_MATERIAL_EVENT = new FastAction<GameObject, Material, Material>();

		// Token: 0x04000116 RID: 278
		public static readonly FastAction<bool> TEXT_STYLE_PROPERTY_EVENT = new FastAction<bool>();

		// Token: 0x04000117 RID: 279
		public static readonly FastAction<Object> COLOR_GRADIENT_PROPERTY_EVENT = new FastAction<Object>();

		// Token: 0x04000118 RID: 280
		public static readonly FastAction TMP_SETTINGS_PROPERTY_EVENT = new FastAction();

		// Token: 0x04000119 RID: 281
		public static readonly FastAction RESOURCE_LOAD_EVENT = new FastAction();

		// Token: 0x0400011A RID: 282
		public static readonly FastAction<bool, Object> TEXTMESHPRO_UGUI_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x0400011B RID: 283
		public static readonly FastAction OnPreRenderObject_Event = new FastAction();

		// Token: 0x0400011C RID: 284
		public static readonly FastAction<Object> TEXT_CHANGED_EVENT = new FastAction<Object>();
	}
}
