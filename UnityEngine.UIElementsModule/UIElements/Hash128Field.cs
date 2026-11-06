using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.UIElements
{
	// Token: 0x020000BC RID: 188
	[MovedFrom(true, "UnityEditor.UIElements", "UnityEditor.UIElementsModule", null)]
	public class Hash128Field : TextInputBaseField<Hash128>
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x00018C3C File Offset: 0x00016E3C
		private Hash128Field.Hash128Input integerInput
		{
			get
			{
				return (Hash128Field.Hash128Input)base.textInputBase;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00018C49 File Offset: 0x00016E49
		public Hash128Field()
			: this(null, -1)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00018C55 File Offset: 0x00016E55
		public Hash128Field(int maxLength)
			: this(null, maxLength)
		{
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00018C64 File Offset: 0x00016E64
		public Hash128Field(string label, int maxLength = -1)
			: base(label, maxLength, '\0', new Hash128Field.Hash128Input())
		{
			this.SetValueWithoutNotify(default(Hash128));
			base.AddToClassList(Hash128Field.ussClassName);
			base.labelElement.AddToClassList(Hash128Field.labelUssClassName);
			base.visualInput.AddToClassList(Hash128Field.inputUssClassName);
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00018CC0 File Offset: 0x00016EC0
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x00018CD8 File Offset: 0x00016ED8
		public override Hash128 value
		{
			get
			{
				return base.value;
			}
			set
			{
				base.value = value;
				bool updateTextFromValue = this.m_UpdateTextFromValue;
				if (updateTextFromValue)
				{
					base.text = base.rawValue.ToString();
				}
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00018D14 File Offset: 0x00016F14
		internal override void UpdateValueFromText()
		{
			this.m_UpdateTextFromValue = false;
			try
			{
				this.value = this.StringToValue(base.text);
			}
			finally
			{
				this.m_UpdateTextFromValue = true;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00018D5C File Offset: 0x00016F5C
		internal override void UpdateTextFromValue()
		{
			base.text = this.ValueToString(base.rawValue);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00018D74 File Offset: 0x00016F74
		public override void SetValueWithoutNotify(Hash128 newValue)
		{
			base.SetValueWithoutNotify(newValue);
			bool updateTextFromValue = this.m_UpdateTextFromValue;
			if (updateTextFromValue)
			{
				base.text = base.rawValue.ToString();
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00018DB4 File Offset: 0x00016FB4
		protected override string ValueToString(Hash128 value)
		{
			return value.ToString();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00018DD4 File Offset: 0x00016FD4
		protected override Hash128 StringToValue(string str)
		{
			return Hash128Field.Hash128Input.Parse(str);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00018DEC File Offset: 0x00016FEC
		[EventInterest(new Type[] { typeof(BlurEvent) })]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			base.ExecuteDefaultAction(evt);
			bool flag = evt == null || base.textEdition.isReadOnly;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
				if (flag2)
				{
					bool flag3 = string.IsNullOrEmpty(base.text);
					if (flag3)
					{
						this.value = default(Hash128);
					}
					else
					{
						base.textInputBase.UpdateValueFromText();
						base.textInputBase.UpdateTextFromValue();
					}
				}
			}
		}

		// Token: 0x040002D1 RID: 721
		public new static readonly string ussClassName = "unity-hash128-field";

		// Token: 0x040002D2 RID: 722
		public new static readonly string labelUssClassName = Hash128Field.ussClassName + "__label";

		// Token: 0x040002D3 RID: 723
		public new static readonly string inputUssClassName = Hash128Field.ussClassName + "__input";

		// Token: 0x020000BD RID: 189
		public new class UxmlFactory : UxmlFactory<Hash128Field, Hash128Field.UxmlTraits>
		{
		}

		// Token: 0x020000BE RID: 190
		public new class UxmlTraits : TextValueFieldTraits<Hash128, UxmlHash128AttributeDescription>
		{
		}

		// Token: 0x020000BF RID: 191
		private class Hash128Input : TextInputBaseField<Hash128>.TextInputBase
		{
			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06000687 RID: 1671 RVA: 0x00018EAF File Offset: 0x000170AF
			private Hash128Field hash128Field
			{
				get
				{
					return (Hash128Field)base.parent;
				}
			}

			// Token: 0x06000688 RID: 1672 RVA: 0x00018EBC File Offset: 0x000170BC
			internal Hash128Input()
			{
				base.textEdition.AcceptCharacter = new Func<char, bool>(this.AcceptCharacter);
			}

			// Token: 0x17000114 RID: 276
			// (get) Token: 0x06000689 RID: 1673 RVA: 0x00018EDF File Offset: 0x000170DF
			protected string allowedCharacters
			{
				get
				{
					return "0123456789abcdefABCDEF";
				}
			}

			// Token: 0x0600068A RID: 1674 RVA: 0x00018EE8 File Offset: 0x000170E8
			internal override bool AcceptCharacter(char c)
			{
				return base.AcceptCharacter(c) && c != '\0' && this.allowedCharacters.IndexOf(c) != -1;
			}

			// Token: 0x17000115 RID: 277
			// (get) Token: 0x0600068B RID: 1675 RVA: 0x00018F1B File Offset: 0x0001711B
			public string formatString
			{
				get
				{
					return UINumericFieldsUtils.k_IntFieldFormatString;
				}
			}

			// Token: 0x0600068C RID: 1676 RVA: 0x00018F24 File Offset: 0x00017124
			protected string ValueToString(Hash128 value)
			{
				return value.ToString();
			}

			// Token: 0x0600068D RID: 1677 RVA: 0x00018F44 File Offset: 0x00017144
			protected override Hash128 StringToValue(string str)
			{
				return Hash128Field.Hash128Input.Parse(str);
			}

			// Token: 0x0600068E RID: 1678 RVA: 0x00018F5C File Offset: 0x0001715C
			internal static Hash128 Parse(string str)
			{
				ulong num;
				bool flag = str.Length == 1 && ulong.TryParse(str, out num);
				Hash128 hash;
				if (flag)
				{
					hash = new Hash128(num, 0UL);
				}
				else
				{
					hash = Hash128.Parse(str);
				}
				return hash;
			}
		}
	}
}
