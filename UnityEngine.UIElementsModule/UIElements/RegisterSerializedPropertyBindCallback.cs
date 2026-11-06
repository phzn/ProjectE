using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000055 RID: 85
	// (Invoke) Token: 0x0600039C RID: 924
	internal delegate void RegisterSerializedPropertyBindCallback<TValueType, TField, TFieldValue>(BaseCompositeField<TValueType, TField, TFieldValue> compositeField, TField field) where TField : TextValueField<TFieldValue>, new();
}
