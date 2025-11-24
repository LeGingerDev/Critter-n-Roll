#if UNITY_EDITOR
namespace ScoredProductions.StreamLinked.Editor {
	using System;
	using System.Collections.Generic;

	using ScoredProductions.StreamLinked.API;
	using ScoredProductions.StreamLinked.Utility;

	using UnityEditor.IMGUI.Controls;

	public class TwitchAPIClassEnumDropdownProvider : AdvancedDropdown {
		private readonly string[] _enumNames;

		public event Action<int> OnSelected;

		public TwitchAPIClassEnumDropdownProvider(Action<int> onSelected) : base(new AdvancedDropdownState()) {
			this._enumNames = Enum.GetNames(typeof(TwitchAPIClassEnum));
			this._enumNames.MakeReadable();
			OnSelected = onSelected;
		}

		protected override void ItemSelected(AdvancedDropdownItem item) {
			OnSelected.Invoke(item.id);
		}

		protected override AdvancedDropdownItem BuildRoot() {
			AdvancedDropdownItem root = new AdvancedDropdownItem("");
			Dictionary<TwitchResourceEnum, AdvancedDropdownItem> Categories = new Dictionary<TwitchResourceEnum, AdvancedDropdownItem>();

			foreach (TwitchResourceEnum resource in Enum.GetValues(typeof(TwitchResourceEnum))) {
				AdvancedDropdownItem cat = new AdvancedDropdownItem(resource.ToString().ToReadable());
				Categories.Add(resource, cat);
				root.AddChild(cat);
			}

			foreach (TwitchAPIClassEnum APIClass in Enum.GetValues(typeof(TwitchAPIClassEnum))) {
				Categories[APIClass.GetResourceType()].AddChild(new AdvancedDropdownItem(APIClass.ToString().ToReadable()) { id = (int)APIClass });
			}

			return root;
		}
	}
}
#endif