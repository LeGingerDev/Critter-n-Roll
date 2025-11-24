using Unity.VisualScripting;

namespace ScoredProductions.StreamLinked.VisualScripting {

	[UnitTitle("String Pair")]
	[UnitCategory("StreamLinked")]
	[TypeIcon(typeof(string))]
	public class StringPairUnit : Unit {

		[DoNotSerialize]
		[PortLabelHidden]
		public ValueInput LeftValue;
		
		[DoNotSerialize]
		[PortLabelHidden]
		public ValueInput RightValue;
		
		[DoNotSerialize]
		[PortLabelHidden]
		public ValueOutput Value;

		protected override void Definition() {
			LeftValue = this.ValueInput(nameof(LeftValue), "");
			RightValue = this.ValueInput(nameof(RightValue), "");
			Value = this.ValueOutput(nameof(Value), this.BuildPair).Predictable();
		}

		public StringPair BuildPair(Flow flow) {
			return new StringPair() {
				Item1 = flow.GetValue<string>(LeftValue),
				Item2 = flow.GetValue<string>(RightValue),
			};
		}
	}
}
