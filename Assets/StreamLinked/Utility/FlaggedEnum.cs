using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UnityEngine;

namespace ScoredProductions.StreamLinked.Utility {

	// File hosts both FlaggedEnum and FlaggedEnum<T>

	/// <summary>
	/// Large container for Enums to be used as flags when you need more than the maximum possible flags (32, 64, 128 [.Net9]). Enum values must be unique values. Enums can be checked using <c>CheckEnumState()</c>
	/// </summary>
	public class FlaggedEnum : ISerializationCallbackReceiver, IEnumerable<int>, IEquatable<FlaggedEnum> {
		public static void CheckEnumState(params Type[] EnumsToCheck) {
			DebugManager.LogMessage("Performing Enum check for duplicate values");
			Dictionary<long, List<string>> values = new Dictionary<long, List<string>>();
			foreach (Type t in EnumsToCheck) {
				values.Clear();
				if (t.IsEnum) {
					string[] container = Enum.GetNames(t);
					for (int x = 0; x < container.Length; x++) {
						string name = container[x];
						if (Enum.TryParse(t, name, out object result) && !values.TryAdd(Convert.ToInt64(result), new List<string>() { name })) {
							List<string> existingValues = values[Convert.ToInt64(result)];
							existingValues.Add(name);
						}
					}
					if (values.Count > 0) {
						foreach (KeyValuePair<long, List<string>> dic in values) {
							if (dic.Value.Count > 1) {
								DebugManager.LogMessage($"Enum '{t.Name}', Duplicate value assigned on value: '{dic.Key}', values with the same assigned value are: '{string.Join(", ", dic.Value)}'", DebugManager.ErrorLevel.Error);
							}
						}
					}
				}
			}
			DebugManager.LogMessage("Enum check Complete");
		}

		public static bool operator ==(FlaggedEnum left, FlaggedEnum right) {
			return EqualityComparer<FlaggedEnum>.Default.Equals(left, right);
		}

		public static bool operator !=(FlaggedEnum left, FlaggedEnum right) {
			return !(left == right);
		}

		public static FlaggedEnum operator +(FlaggedEnum self, int enumValue) {
			self.Add(enumValue);
			return self;
		}

		public static FlaggedEnum operator +(FlaggedEnum self, FlaggedEnum enumValue) {
			self.Or(enumValue);
			return self;
		}

		public static FlaggedEnum operator -(FlaggedEnum self, int enumValue) {
			self.Subtract(enumValue);
			return self;
		}

		public static FlaggedEnum operator -(FlaggedEnum self, FlaggedEnum enumValue) {
			self.Subtract(enumValue);
			return self;
		}

		public static FlaggedEnum operator |(FlaggedEnum self, int enumValue) {
			return self += enumValue;
		}

		public static FlaggedEnum operator |(FlaggedEnum self, FlaggedEnum enumValue) {
			return self += enumValue;
		}

		public static FlaggedEnum operator &(FlaggedEnum self, int enumValue) {
			self.And(enumValue);
			return self;
		}

		public static FlaggedEnum operator &(FlaggedEnum self, FlaggedEnum enumValue) {
			self.And(enumValue);
			return self;
		}

		public static FlaggedEnum operator ^(FlaggedEnum self, int enumValue) {
			self.XOr(enumValue);
			return self;
		}

		public static FlaggedEnum operator ^(FlaggedEnum self, FlaggedEnum enumValue) {
			self.XOr(enumValue);
			return self;
		}

		private void _buildDictionary() {
			if (this._storedValues == null || this._storedValues.Count < this.GetEnumLength) {
				this._storedValues.EnsureCapacity(this.GetEnumLength);

				foreach (int enumValue in this.getEnumValues) {
					this._storedValues.TryAdd(enumValue, false);
				}
			}
		}

		public readonly string[] getEnumNames;
		public string[] GetEnumNames => (string[])getEnumNames.Clone();

		public readonly int GetEnumLength;

		public readonly int[] getEnumValues;
		public int[] GetEnumValues => (int[])getEnumValues.Clone();

		public readonly Type GetUnderlyingType;

		private readonly Dictionary<int, bool> _storedValues = new Dictionary<int, bool>(0);
		/// <summary>
		/// Returns a copy of the dictionary used, not the actual values
		/// </summary>
		public ReadOnlyDictionary<int, bool> StoredValues
		{
			get
			{
				this._buildDictionary();
				return new ReadOnlyDictionary<int, bool>(this._storedValues);
			}
		}

		[SerializeField]
		private List<int> _serializedAnswers;

		private FlaggedEnum() { }

		/// <summary>
		/// Constructor for a custom enum where there is no backing enum, just a store of values to compare against in a flaggable format
		/// </summary>
		/// <param name="data"></param>
		public FlaggedEnum(int[] data) {
			for (int x = 0; x < this.GetEnumLength; x++) {
				bool found = false;
				int a = x;
				for (int y = x + 1; y < this.GetEnumLength; y++) {
					if (a == data[y]) {
						if (found) {
							throw new ArgumentException("Duplicate value found in provided array. Please ensure values are unique.");
						} else {
							found = true;
						}
					}
				}
			}
			this.GetEnumLength = data.Length;
			this.getEnumValues = data;
			this.GetUnderlyingType = typeof(int);
		}

		public FlaggedEnum(Type enumType) {
			if (enumType.IsEnum) {
				this.getEnumNames = Enum.GetNames(enumType);
				this.GetEnumLength = this.getEnumNames.Length;

				Array values = Enum.GetValues(enumType);
				this.getEnumValues = new int[this.GetEnumLength];
				for (int x = 0; x < this.GetEnumLength; x++) {
					this.getEnumValues[x] = (int)values.GetValue(x);
				}

				this.GetUnderlyingType = enumType;
			} else {
				throw new Exception("Provided type is not an enum");
			}
		}

		public FlaggedEnum(Enum @enum) : this(Enum.GetUnderlyingType(@enum.GetType())) { }

		public FlaggedEnum(Type enumType, int[] values) : this(enumType) {
			this.Add(values);
		}
		
		public FlaggedEnum(Enum @enum, int[] values) : this(@enum) {
			this.Add(values);
		}

		public bool this[int index]
		{
			get
			{
				return this.StoredValues[getEnumValues[index]];
			}
			set
			{
				this.PerformOperation(getEnumValues[index], value ? Operation.add : Operation.subtract);
			}
		}

		public override bool Equals(object obj) => obj is FlaggedEnum @enum && EqualityComparer<Dictionary<int, bool>>.Default.Equals(this._storedValues, @enum._storedValues);
		
		public bool Equals(FlaggedEnum other) => other is not null && EqualityComparer<Dictionary<int, bool>>.Default.Equals(this._storedValues, other._storedValues);
		public override int GetHashCode() => HashCode.Combine(this._storedValues);

		public List<int> GetAllFlagged() {
			List<int> result = new List<int>();
			for (int x = 0; x < GetEnumLength; x++) {
				int next = getEnumValues[x];
				if (this.StoredValues[next]) {
					result.Add(next);
				}
			}
			return result;
		}

		public E GetAllFlagged<E>() where E : ICollection<int>, new() {
			E container = new E();
			for (int x = 0; x < GetEnumLength; x++) {
				int next = getEnumValues[x];
				if (this.StoredValues[next]) {
					container.Add(next);
				}
			}
			return container;
		}

		public int[] GetAllFlaggedAsArray() {
			int count = 0;
			for (int x = 0; x < GetEnumLength; x++) {
				int next = getEnumValues[x];
				if (this.StoredValues[next]) {
					count++;
				}
			}
			int[] result = new int[count];
			count = 0;
			for (int x = 0; x < GetEnumLength; x++) {
				int next = getEnumValues[x];
				if (this.StoredValues[next]) {
					result[count++] = next;
				}
			}
			return result;
		}

		/// <summary>
		/// Number of flags set to on
		/// </summary>
		public int Count
		{
			get
			{
				int count = 0;
				for (int x = 0; x < GetEnumLength; x++) {
					if (this.StoredValues[getEnumValues[x]]) {
						count++;
					}
				}
				return count;
			}
		}

		public bool HasFlag(params int[] values) {
			for (int x = 0; x < values.Length; x++) {
				if (!this.StoredValues[values[x]]) {
					return false;
				}
			}
			return true;
		}

		public bool HasntFlag(params int[] values) {
			for (int x = 0; x < values.Length; x++) {
				if (this.StoredValues[values[x]]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Check if all values are false
		/// </summary>
		/// <returns></returns>
		public bool HasNoFlag() {
			for (int x = 0; x < GetEnumLength; x++) {
				if (this.StoredValues[getEnumValues[x]]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Check if all values are true
		/// </summary>
		/// <returns></returns>
		public bool HasAllFlag() {
			for (int x = 0; x < GetEnumLength; x++) {
				if (!this.StoredValues[getEnumValues[x]]) {
					return false;
				}
			}
			return true;
		}

		public void Clear() {
			this._buildDictionary();
			for (int x = 0; x < GetEnumLength; x++) {
				this._storedValues[getEnumValues[x]] = false;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(int value) {
			return this.PerformOperation(value, Operation.add);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(params int[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Add(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(IList<int> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Add(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(FlaggedEnum value) {
			bool changed = false;
			changed |= this.Add(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(int value) {
			return this.PerformOperation(value, Operation.subtract);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(params int[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Subtract(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(IList<int> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Subtract(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(FlaggedEnum value) {
			bool changed = false;
			changed |= this.Subtract(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(int value) {
			return this.PerformOperation(value, Operation.or);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(params int[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Or(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(IList<int> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Or(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(FlaggedEnum value) {
			bool changed = false;
			changed |= this.Or(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(int value) {
			return this.PerformOperation(value, Operation.xor);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(params int[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.XOr(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(IList<int> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.XOr(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(FlaggedEnum value) {
			bool changed = false;
			changed |= this.XOr(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(int value) {
			return this.PerformOperation(value, Operation.add);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(params int[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.And(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(IList<int> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.And(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(FlaggedEnum value) {
			bool changed = false;
			changed |= this.And(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(params int[] values) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(values);
			this.OnAfterDeserialize();
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(IList<int> values) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(values);
			this.OnAfterDeserialize();
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(FlaggedEnum value) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(value.GetAllFlaggedAsArray());
			this.OnAfterDeserialize();
		}

		private enum Operation {
			add,
			subtract,
			or,
			xor,
			and
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="op"></param>
		/// <returns>the value changed</returns>
		private bool PerformOperation(int value, Operation op) {
			this._buildDictionary();
			if (!this._storedValues.ContainsKey(value)) {
				throw new ArgumentOutOfRangeException("Value not found in built enum.");
			}
			bool before = this._storedValues[value];
			bool after = false;
			switch (op) {
				case Operation.add:
					after = (this._storedValues[value] = true);
					break;
				case Operation.subtract:
					after = (this._storedValues[value] = false);
					break;
				case Operation.or:
					after = (this._storedValues[value] |= true);
					break;
				case Operation.xor:
					after = (this._storedValues[value] ^= true);
					break;
				case Operation.and:
					after = (this._storedValues[value] &= true);
					break;
			}
			return before != after;
		}

		public void OnBeforeSerialize() {
			this._serializedAnswers = this.GetAllFlagged();
		}

		public void OnAfterDeserialize() {
			this.Clear();
			if (this._serializedAnswers != null && this._serializedAnswers.Count > 0) {
				this.Add(this._serializedAnswers);
			}
		}

		public IEnumerator<int> GetEnumerator() {
			this.OnBeforeSerialize(); // Force update the backing array to detect collection change
			foreach (int Enum in this._serializedAnswers) {
				yield return Enum;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}


	}

	/// <summary>
	/// Large container for Enums to be used as flags when you need more than the maximum possible flags (32, 64, 128 [.Net9]). Enum values must be unique values. Enums can be checked using <c>CheckEnumState()</c>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class FlaggedEnum<T> : ISerializationCallbackReceiver, IEnumerable<T>, IEquatable<FlaggedEnum<T>> where T : Enum, new() {

		private static string[] _enumNames;
		private static Type _underlyingType;
		private static T[] _enumValues;
		private static int _enumLength;

		public static string[] EnumNames
		{
			get
			{
				if (_enumNames.IsNullOrEmpty()) {
					_enumNames = Enum.GetNames(typeof(T));
				}
				return (string[])_enumNames.Clone();
			}
		}

		public static Type UnderlyingType
		{
			get
			{
				if (_underlyingType == null) {
					_underlyingType = Enum.GetUnderlyingType(typeof(T));
				}
				return _underlyingType;
			}
		}

		public static T[] EnumValues
		{
			get
			{
				if (_enumValues.IsNullOrEmpty()) {
					Array values = Enum.GetValues(typeof(T));
					_enumValues = new T[EnumLength];
					for (int x = 0; x < EnumLength; x++) {
						_enumValues[x] = (T)values.GetValue(x);
					}
				}
				return (T[])_enumValues.Clone();
			}
		}

		private static T[] GetEnumValuesInternal
		{
			get
			{
				if (_enumValues.IsNullOrEmpty()) {
					Array values = Enum.GetValues(typeof(T));
					_enumValues = new T[EnumLength];
					for (int x = 0; x < EnumLength; x++) {
						_enumValues[x] = (T)values.GetValue(x);
					}
				}
				return _enumValues;
			}
		}

		public static int EnumLength
		{
			get
			{
				if (_enumLength <= 0) {
					_enumLength = Enum.GetValues(typeof(T)).Length;
				}
				return _enumLength;
			}
		}

		static FlaggedEnum() {
			if (typeof(T).GUID == typeof(Enum).GUID) {
				return;
			}
			if (Application.isEditor) { // Check in editor only if the enum has issues
				CheckEnumState();
			}
		}

		public static void CheckEnumState() => FlaggedEnum.CheckEnumState(typeof(T));

		public static bool operator ==(FlaggedEnum<T> left, FlaggedEnum<T> right) {
			return EqualityComparer<FlaggedEnum<T>>.Default.Equals(left, right);
		}

		public static bool operator !=(FlaggedEnum<T> left, FlaggedEnum<T> right) {
			return !(left == right);
		}

		public static FlaggedEnum<T> operator +(FlaggedEnum<T> self, T enumValue) {
			self.Add(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator +(FlaggedEnum<T> self, FlaggedEnum<T> enumValue) {
			self.Or(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator -(FlaggedEnum<T> self, T enumValue) {
			self.Subtract(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator -(FlaggedEnum<T> self, FlaggedEnum<T> enumValue) {
			self.Subtract(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator |(FlaggedEnum<T> self, T enumValue) {
			return self += enumValue;
		}

		public static FlaggedEnum<T> operator |(FlaggedEnum<T> self, FlaggedEnum<T> enumValue) {
			return self += enumValue;
		}

		public static FlaggedEnum<T> operator &(FlaggedEnum<T> self, T enumValue) {
			self.And(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator &(FlaggedEnum<T> self, FlaggedEnum<T> enumValue) {
			self.And(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator ^(FlaggedEnum<T> self, T enumValue) {
			self.XOr(enumValue);
			return self;
		}

		public static FlaggedEnum<T> operator ^(FlaggedEnum<T> self, FlaggedEnum<T> enumValue) {
			self.XOr(enumValue);
			return self;
		}

		/// <summary>
		/// Adds missing values to the dictionary while maintaining the current values
		/// </summary>
		private void _buildDictionary() {
			if (this._storedValues == null || this._storedValues.Count < EnumLength) {
				this._storedValues.EnsureCapacity(EnumLength);

				foreach (T enumValue in GetEnumValuesInternal) {
					this._storedValues.TryAdd(enumValue, false);
				}
			}
		}

		public string[] GetEnumNames => EnumNames;
		public T[] GetEnumValues => EnumValues;
		public int GetEnumLength => EnumLength;
		public Type GetUnderlyingType => UnderlyingType;

		private readonly Dictionary<T, bool> _storedValues = new Dictionary<T, bool>(EnumLength);
		/// <summary>
		/// Returns a copy of the dictionary used, not the actual values
		/// </summary>
		public ReadOnlyDictionary<T, bool> StoredValues
		{
			get
			{
				this._buildDictionary();
				return new ReadOnlyDictionary<T, bool>(this._storedValues);
			}
		}

		public bool this[T index]
		{
			get
			{
				return this.StoredValues[index];
			}
			set
			{
				this.PerformOperation(index, value ? Operation.add : Operation.subtract);
			}
		}

		public bool this[int index]
		{
			get
			{
				return this.StoredValues[GetEnumValuesInternal[index]];
			}
			set
			{
				this.PerformOperation(GetEnumValuesInternal[index], value ? Operation.add : Operation.subtract);
			}
		}

		[SerializeField]
		private List<T> _serializedAnswers;

		public FlaggedEnum() {
			this._buildDictionary();
			for (int x = 0; x < EnumLength; x++) {
				this._storedValues[GetEnumValuesInternal[x]] = false;
			}
		}

		public FlaggedEnum(params T[] values) : this() {
			this.Add(values);
		}

		public FlaggedEnum(FlaggedEnum<T> value) : this() {
			this.Add(value);
		}

		public List<T> GetAllFlagged() {
			List<T> result = new List<T>();
			for (int x = 0; x < EnumLength; x++) {
				T next = GetEnumValuesInternal[x];
				if (this.StoredValues[next]) {
					result.Add(next);
				}
			}
			return result;
		}

		public E GetAllFlagged<E>() where E : ICollection<T>, new() {
			E container = new E();
			for (int x = 0; x < EnumLength; x++) {
				T next = GetEnumValuesInternal[x];
				if (this.StoredValues[next]) {
					container.Add(next);
				}
			}
			return container;
		}

		public T[] GetAllFlaggedAsArray() {
			int count = 0;
			for (int x = 0; x < EnumLength; x++) {
				T next = GetEnumValuesInternal[x];
				if (this.StoredValues[next]) {
					count++;
				}
			}
			T[] result = new T[count];
			count = 0;
			for (int x = 0; x < EnumLength; x++) {
				T next = GetEnumValuesInternal[x];
				if (this.StoredValues[next]) {
					result[count++] = next;
				}
			}
			return result;
		}

		/// <summary>
		/// Number of flags set to on
		/// </summary>
		public int Count
		{
			get
			{
				int count = 0;
				for (int x = 0; x < EnumLength; x++) {
					if (this.StoredValues[EnumValues[x]]) {
						count++;
					}
				}
				return count;
			}
		}

		public bool HasFlag(params T[] values) {
			for (int x = 0; x < values.Length; x++) {
				if (!this.StoredValues[values[x]]) {
					return false;
				}
			}
			return true;
		}

		public bool HasntFlag(params T[] values) {
			for (int x = 0; x < values.Length; x++) {
				if (this.StoredValues[values[x]]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Check if all values are false
		/// </summary>
		/// <returns></returns>
		public bool HasNoFlag() {
			for (int x = 0; x < EnumLength; x++) {
				if (this.StoredValues[GetEnumValuesInternal[x]]) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Check if all values are true
		/// </summary>
		/// <returns></returns>
		public bool HasAllFlag() {
			for (int x = 0; x < EnumLength; x++) {
				if (!this.StoredValues[GetEnumValuesInternal[x]]) {
					return false;
				}
			}
			return true;
		}

		public void Clear() {
			this._buildDictionary();
			for (int x = 0; x < EnumLength; x++) {
				this._storedValues[GetEnumValuesInternal[x]] = false;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(T value) {
			return this.PerformOperation(value, Operation.add);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(params T[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Add(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(IList<T> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Add(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Add(FlaggedEnum<T> value) {
			bool changed = false;
			changed |= this.Add(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(T value) {
			return this.PerformOperation(value, Operation.subtract);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(params T[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Subtract(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(IList<T> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Subtract(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Subtract(FlaggedEnum<T> value) {
			bool changed = false;
			changed |= this.Subtract(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(T value) {
			return this.PerformOperation(value, Operation.or);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(params T[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.Or(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(IList<T> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.Or(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool Or(FlaggedEnum<T> value) {
			bool changed = false;
			changed |= this.Or(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(T value) {
			return this.PerformOperation(value, Operation.xor);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(params T[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.XOr(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(IList<T> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.XOr(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool XOr(FlaggedEnum<T> value) {
			bool changed = false;
			changed |= this.XOr(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(T value) {
			return this.PerformOperation(value, Operation.add);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(params T[] values) {
			bool changed = false;
			for (int x = 0; x < values.Length; x++) {
				changed |= this.And(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(IList<T> values) {
			bool changed = false;
			for (int x = 0; x < values.Count; x++) {
				changed |= this.And(values[x]);
			}
			return changed;
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns>If the value changed</returns>
		public bool And(FlaggedEnum<T> value) {
			bool changed = false;
			changed |= this.And(value.GetAllFlaggedAsArray());
			return changed;
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(params T[] values) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(values);
			this.OnAfterDeserialize();
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(IList<T> values) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(values);
			this.OnAfterDeserialize();
		}

		/// <summary>
		/// Deletes the current entries and replaces them with a new set
		/// </summary>
		/// <param name="value"></param>
		public void Replace(FlaggedEnum<T> value) {
			this._serializedAnswers.Clear();
			this._serializedAnswers.AddRange(value.GetAllFlaggedAsArray());
			this.OnAfterDeserialize();
		}

		private enum Operation {
			add,
			subtract,
			or,
			xor,
			and
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="op"></param>
		/// <returns>the value changed</returns>
		private bool PerformOperation(T value, Operation op) {
			this._buildDictionary();
			bool before = this._storedValues[value];
			bool after = false;
			switch (op) {
				case Operation.add:
					after = (this._storedValues[value] = true);
					break;
				case Operation.subtract:
					after = (this._storedValues[value] = false);
					break;
				case Operation.or:
					after = (this._storedValues[value] |= true);
					break;
				case Operation.xor:
					after = (this._storedValues[value] ^= true);
					break;
				case Operation.and:
					after = (this._storedValues[value] &= true);
					break;
			}
			return before != after;
		}

		public override bool Equals(object obj) => obj is FlaggedEnum<T> @enum && EqualityComparer<Dictionary<T, bool>>.Default.Equals(this._storedValues, @enum._storedValues);
		public bool Equals(FlaggedEnum<T> other) => other is not null && EqualityComparer<Dictionary<T, bool>>.Default.Equals(this._storedValues, other._storedValues);
		public override int GetHashCode() => HashCode.Combine(this._storedValues);

		public void OnBeforeSerialize() {
			this._serializedAnswers = this.GetAllFlagged();
		}

		public void OnAfterDeserialize() {
			this.Clear();
			if (this._serializedAnswers != null && this._serializedAnswers.Count > 0) {
				this.Add(this._serializedAnswers);
			}
		}

		public IEnumerator<T> GetEnumerator() {
			this.OnBeforeSerialize(); // Force update the backing array to detect collection change
			foreach (T Enum in this._serializedAnswers) {
				yield return Enum;
			}
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

	}
}
