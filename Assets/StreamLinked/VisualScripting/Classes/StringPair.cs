using System;

using Unity.VisualScripting;

using UnityEngine;

namespace ScoredProductions.StreamLinked.VisualScripting {

    /// <summary>
    /// Type used to render string tuples in Visual Scripting
    /// </summary>
	[Serializable]
    [Inspectable]
	public class StringPair
    {
        [SerializeField]
		[Inspectable]
		public string Item1;
        [SerializeField]
		[Inspectable]
		public string Item2;

        public (string, string) ToTuple() {
            return (this.Item1, this.Item2);
        }
    }
}
