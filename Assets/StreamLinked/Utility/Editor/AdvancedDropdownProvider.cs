#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;

	using UnityEditor.IMGUI.Controls;

	// Although not used, it was the base before making TwitchAPIClassEnumDropdownProvider so might as well keep it for later

	public class AdvancedDropdownProvider : AdvancedDropdown {
		private readonly string[] _enumNames;

		public event Action<int> OnSelected;

		public AdvancedDropdownProvider(string[] stringOptions, Action<int> onSelected) : base(new AdvancedDropdownState()) {
			this._enumNames = stringOptions;
			OnSelected = onSelected;
		}

		protected override void ItemSelected(AdvancedDropdownItem item) {
			OnSelected.Invoke(item.id);
		}

		protected override AdvancedDropdownItem BuildRoot() {
			AdvancedDropdownItem root = new AdvancedDropdownItem("");

			for (int x = 0; x < this._enumNames.Length; x++) {
				AdvancedDropdownItem item = new AdvancedDropdownItem(this._enumNames[x]) {
					id = x
				};

				root.AddChild(item);
			}

			return root;
		}
	}
}
#endif