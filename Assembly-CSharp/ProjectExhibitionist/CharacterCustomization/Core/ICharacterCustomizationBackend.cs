using System;
using Bozo.AnimeCharacters;

namespace ProjectExhibitionist.CharacterCustomization.Core
{
	// Token: 0x02000AD6 RID: 2774
	public interface ICharacterCustomizationBackend
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06004EA3 RID: 20131
		string BackendId { get; }

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004EA4 RID: 20132
		bool IsInitialized { get; }

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004EA5 RID: 20133
		CustomizationFeature SupportedFeatures { get; }

		// Token: 0x06004EA6 RID: 20134
		bool CanInitialize(CustomizationSessionContext context);

		// Token: 0x06004EA7 RID: 20135
		bool Initialize(CustomizationSessionContext context);

		// Token: 0x06004EA8 RID: 20136
		void Shutdown();

		// Token: 0x06004EA9 RID: 20137
		bool TrySetBlendShape(string blendShapeName, float value);

		// Token: 0x06004EAA RID: 20138
		bool TryGetBlendShape(string blendShapeName, out float value);

		// Token: 0x06004EAB RID: 20139
		CharacterStateData CaptureState();

		// Token: 0x06004EAC RID: 20140
		bool ApplyState(CharacterStateData stateData);
	}
}
