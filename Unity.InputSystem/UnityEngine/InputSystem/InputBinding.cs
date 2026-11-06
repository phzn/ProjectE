using System;
using System.Linq;
using System.Text;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public struct InputBinding : IEquatable<InputBinding>
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000C758 File Offset: 0x0000A958
		// (set) Token: 0x060002AC RID: 684 RVA: 0x0000C760 File Offset: 0x0000A960
		public string name
		{
			get
			{
				return this.m_Name;
			}
			set
			{
				this.m_Name = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000C76C File Offset: 0x0000A96C
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000C79B File Offset: 0x0000A99B
		public Guid id
		{
			get
			{
				if (string.IsNullOrEmpty(this.m_Id))
				{
					return default(Guid);
				}
				return new Guid(this.m_Id);
			}
			set
			{
				this.m_Id = value.ToString();
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000C7B0 File Offset: 0x0000A9B0
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
		public string path
		{
			get
			{
				return this.m_Path;
			}
			set
			{
				this.m_Path = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000C7C1 File Offset: 0x0000A9C1
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000C7C9 File Offset: 0x0000A9C9
		public string overridePath
		{
			get
			{
				return this.m_OverridePath;
			}
			set
			{
				this.m_OverridePath = value;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000C7D2 File Offset: 0x0000A9D2
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000C7DA File Offset: 0x0000A9DA
		public string interactions
		{
			get
			{
				return this.m_Interactions;
			}
			set
			{
				this.m_Interactions = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000C7E3 File Offset: 0x0000A9E3
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x0000C7EB File Offset: 0x0000A9EB
		public string overrideInteractions
		{
			get
			{
				return this.m_OverrideInteractions;
			}
			set
			{
				this.m_OverrideInteractions = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		// (set) Token: 0x060002B8 RID: 696 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		public string processors
		{
			get
			{
				return this.m_Processors;
			}
			set
			{
				this.m_Processors = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000C805 File Offset: 0x0000AA05
		// (set) Token: 0x060002BA RID: 698 RVA: 0x0000C80D File Offset: 0x0000AA0D
		public string overrideProcessors
		{
			get
			{
				return this.m_OverrideProcessors;
			}
			set
			{
				this.m_OverrideProcessors = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002BB RID: 699 RVA: 0x0000C816 File Offset: 0x0000AA16
		// (set) Token: 0x060002BC RID: 700 RVA: 0x0000C81E File Offset: 0x0000AA1E
		public string groups
		{
			get
			{
				return this.m_Groups;
			}
			set
			{
				this.m_Groups = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002BD RID: 701 RVA: 0x0000C827 File Offset: 0x0000AA27
		// (set) Token: 0x060002BE RID: 702 RVA: 0x0000C82F File Offset: 0x0000AA2F
		public string action
		{
			get
			{
				return this.m_Action;
			}
			set
			{
				this.m_Action = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000C838 File Offset: 0x0000AA38
		// (set) Token: 0x060002C0 RID: 704 RVA: 0x0000C845 File Offset: 0x0000AA45
		public bool isComposite
		{
			get
			{
				return (this.m_Flags & InputBinding.Flags.Composite) == InputBinding.Flags.Composite;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputBinding.Flags.Composite;
					return;
				}
				this.m_Flags &= ~InputBinding.Flags.Composite;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000C868 File Offset: 0x0000AA68
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000C875 File Offset: 0x0000AA75
		public bool isPartOfComposite
		{
			get
			{
				return (this.m_Flags & InputBinding.Flags.PartOfComposite) == InputBinding.Flags.PartOfComposite;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= InputBinding.Flags.PartOfComposite;
					return;
				}
				this.m_Flags &= ~InputBinding.Flags.PartOfComposite;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000C898 File Offset: 0x0000AA98
		public bool hasOverrides
		{
			get
			{
				return this.overridePath != null || this.overrideProcessors != null || this.overrideInteractions != null;
			}
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000C8B8 File Offset: 0x0000AAB8
		public InputBinding(string path, string action = null, string groups = null, string processors = null, string interactions = null, string name = null)
		{
			this.m_Path = path;
			this.m_Action = action;
			this.m_Groups = groups;
			this.m_Processors = processors;
			this.m_Interactions = interactions;
			this.m_Name = name;
			this.m_Id = null;
			this.m_Flags = InputBinding.Flags.None;
			this.m_OverridePath = null;
			this.m_OverrideInteractions = null;
			this.m_OverrideProcessors = null;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C918 File Offset: 0x0000AB18
		public string GetNameOfComposite()
		{
			if (!this.isComposite)
			{
				return null;
			}
			return NameAndParameters.Parse(this.effectivePath).name;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000C944 File Offset: 0x0000AB44
		internal void GenerateId()
		{
			this.m_Id = Guid.NewGuid().ToString();
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000C96A File Offset: 0x0000AB6A
		internal void RemoveOverrides()
		{
			this.m_OverridePath = null;
			this.m_OverrideInteractions = null;
			this.m_OverrideProcessors = null;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000C984 File Offset: 0x0000AB84
		public static InputBinding MaskByGroup(string group)
		{
			return new InputBinding
			{
				groups = group
			};
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000C9A4 File Offset: 0x0000ABA4
		public static InputBinding MaskByGroups(params string[] groups)
		{
			InputBinding inputBinding = default(InputBinding);
			inputBinding.groups = string.Join(";", groups.Where<string>((string x) => !string.IsNullOrEmpty(x)));
			return inputBinding;
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000C9F0 File Offset: 0x0000ABF0
		public string effectivePath
		{
			get
			{
				return this.overridePath ?? this.path;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000CA02 File Offset: 0x0000AC02
		public string effectiveInteractions
		{
			get
			{
				return this.overrideInteractions ?? this.interactions;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000CA14 File Offset: 0x0000AC14
		public string effectiveProcessors
		{
			get
			{
				return this.overrideProcessors ?? this.processors;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000CA26 File Offset: 0x0000AC26
		internal bool isEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.effectivePath) && string.IsNullOrEmpty(this.action) && string.IsNullOrEmpty(this.groups);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000CA50 File Offset: 0x0000AC50
		public bool Equals(InputBinding other)
		{
			return string.Equals(this.effectivePath, other.effectivePath, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this.effectiveInteractions, other.effectiveInteractions, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this.effectiveProcessors, other.effectiveProcessors, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this.groups, other.groups, StringComparison.InvariantCultureIgnoreCase) && string.Equals(this.action, other.action, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is InputBinding)
			{
				InputBinding inputBinding = (InputBinding)obj;
				return this.Equals(inputBinding);
			}
			return false;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000CAF2 File Offset: 0x0000ACF2
		public static bool operator ==(InputBinding left, InputBinding right)
		{
			return left.Equals(right);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000CAFC File Offset: 0x0000ACFC
		public static bool operator !=(InputBinding left, InputBinding right)
		{
			return !(left == right);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000CB08 File Offset: 0x0000AD08
		public override int GetHashCode()
		{
			return (((((((((this.effectivePath != null) ? this.effectivePath.GetHashCode() : 0) * 397) ^ ((this.effectiveInteractions != null) ? this.effectiveInteractions.GetHashCode() : 0)) * 397) ^ ((this.effectiveProcessors != null) ? this.effectiveProcessors.GetHashCode() : 0)) * 397) ^ ((this.groups != null) ? this.groups.GetHashCode() : 0)) * 397) ^ ((this.action != null) ? this.action.GetHashCode() : 0);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000CBA0 File Offset: 0x0000ADA0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this.action))
			{
				stringBuilder.Append(this.action);
				stringBuilder.Append(':');
			}
			string effectivePath = this.effectivePath;
			if (!string.IsNullOrEmpty(effectivePath))
			{
				stringBuilder.Append(effectivePath);
			}
			if (!string.IsNullOrEmpty(this.groups))
			{
				stringBuilder.Append('[');
				stringBuilder.Append(this.groups);
				stringBuilder.Append(']');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000CC20 File Offset: 0x0000AE20
		public string ToDisplayString(InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0, InputControl control = null)
		{
			string text;
			string text2;
			return this.ToDisplayString(out text, out text2, options, control);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		public string ToDisplayString(out string deviceLayoutName, out string controlPath, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0, InputControl control = null)
		{
			if (this.isComposite)
			{
				deviceLayoutName = null;
				controlPath = null;
				return string.Empty;
			}
			InputControlPath.HumanReadableStringOptions humanReadableStringOptions = InputControlPath.HumanReadableStringOptions.None;
			if ((options & InputBinding.DisplayStringOptions.DontOmitDevice) == (InputBinding.DisplayStringOptions)0)
			{
				humanReadableStringOptions |= InputControlPath.HumanReadableStringOptions.OmitDevice;
			}
			if ((options & InputBinding.DisplayStringOptions.DontUseShortDisplayNames) == (InputBinding.DisplayStringOptions)0)
			{
				humanReadableStringOptions |= InputControlPath.HumanReadableStringOptions.UseShortNames;
			}
			string text = InputControlPath.ToHumanReadableString(((options & InputBinding.DisplayStringOptions.IgnoreBindingOverrides) != (InputBinding.DisplayStringOptions)0) ? this.path : this.effectivePath, out deviceLayoutName, out controlPath, humanReadableStringOptions, control);
			if (!string.IsNullOrEmpty(this.effectiveInteractions) && (options & InputBinding.DisplayStringOptions.DontIncludeInteractions) == (InputBinding.DisplayStringOptions)0)
			{
				string text2 = string.Empty;
				foreach (NameAndParameters nameAndParameters in NameAndParameters.ParseMultiple(this.effectiveInteractions))
				{
					string displayName = InputInteraction.GetDisplayName(nameAndParameters.name);
					if (!string.IsNullOrEmpty(displayName))
					{
						if (!string.IsNullOrEmpty(text2))
						{
							text2 = text2 + " or " + displayName;
						}
						else
						{
							text2 = displayName;
						}
					}
				}
				if (!string.IsNullOrEmpty(text2))
				{
					text = text2 + " " + text;
				}
			}
			return text;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000CD34 File Offset: 0x0000AF34
		internal bool TriggersAction(InputAction action)
		{
			return string.Compare(action.name, this.action, StringComparison.InvariantCultureIgnoreCase) == 0 || this.action == action.m_Id;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000CD5D File Offset: 0x0000AF5D
		public bool Matches(InputBinding binding)
		{
			return this.Matches(ref binding, (InputBinding.MatchOptions)0);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000CD68 File Offset: 0x0000AF68
		internal bool Matches(ref InputBinding binding, InputBinding.MatchOptions options = (InputBinding.MatchOptions)0)
		{
			if (this.name != null && (binding.name == null || !StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(this.name, binding.name, ';')))
			{
				return false;
			}
			if (this.path != null && (binding.path == null || !StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(this.path, binding.path, ';')))
			{
				return false;
			}
			if (this.action != null && (binding.action == null || !StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(this.action, binding.action, ';')))
			{
				return false;
			}
			if (this.groups != null)
			{
				bool flag = !string.IsNullOrEmpty(binding.groups);
				if (!flag && (options & InputBinding.MatchOptions.EmptyGroupMatchesAny) == (InputBinding.MatchOptions)0)
				{
					return false;
				}
				if (flag && !StringHelpers.CharacterSeparatedListsHaveAtLeastOneCommonElement(this.groups, binding.groups, ';'))
				{
					return false;
				}
			}
			return string.IsNullOrEmpty(this.m_Id) || !(binding.id != this.id);
		}

		// Token: 0x040000E2 RID: 226
		public const char Separator = ';';

		// Token: 0x040000E3 RID: 227
		internal const string kSeparatorString = ";";

		// Token: 0x040000E4 RID: 228
		[SerializeField]
		private string m_Name;

		// Token: 0x040000E5 RID: 229
		[SerializeField]
		internal string m_Id;

		// Token: 0x040000E6 RID: 230
		[Tooltip("Path of the control to bind to. Matched at runtime to controls from InputDevices present at the time.\n\nCan either be graphically from the control picker dropdown UI or edited manually in text mode by clicking the 'T' button. Internally, both methods result in control path strings that look like, for example, \"<Gamepad>/buttonSouth\".")]
		[SerializeField]
		private string m_Path;

		// Token: 0x040000E7 RID: 231
		[SerializeField]
		private string m_Interactions;

		// Token: 0x040000E8 RID: 232
		[SerializeField]
		private string m_Processors;

		// Token: 0x040000E9 RID: 233
		[SerializeField]
		internal string m_Groups;

		// Token: 0x040000EA RID: 234
		[SerializeField]
		private string m_Action;

		// Token: 0x040000EB RID: 235
		[SerializeField]
		internal InputBinding.Flags m_Flags;

		// Token: 0x040000EC RID: 236
		[NonSerialized]
		private string m_OverridePath;

		// Token: 0x040000ED RID: 237
		[NonSerialized]
		private string m_OverrideInteractions;

		// Token: 0x040000EE RID: 238
		[NonSerialized]
		private string m_OverrideProcessors;

		// Token: 0x0200017E RID: 382
		[Flags]
		public enum DisplayStringOptions
		{
			// Token: 0x04000871 RID: 2161
			DontUseShortDisplayNames = 1,
			// Token: 0x04000872 RID: 2162
			DontOmitDevice = 2,
			// Token: 0x04000873 RID: 2163
			DontIncludeInteractions = 4,
			// Token: 0x04000874 RID: 2164
			IgnoreBindingOverrides = 8
		}

		// Token: 0x0200017F RID: 383
		[Flags]
		internal enum MatchOptions
		{
			// Token: 0x04000876 RID: 2166
			EmptyGroupMatchesAny = 1
		}

		// Token: 0x02000180 RID: 384
		[Flags]
		internal enum Flags
		{
			// Token: 0x04000878 RID: 2168
			None = 0,
			// Token: 0x04000879 RID: 2169
			Composite = 4,
			// Token: 0x0400087A RID: 2170
			PartOfComposite = 8
		}
	}
}
