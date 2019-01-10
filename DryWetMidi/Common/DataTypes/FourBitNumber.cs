﻿using System;
using System.Linq;

namespace Melanchall.DryWetMidi.Common
{
    /// <summary>
    /// Type that is used to represent a four-bit number (0-15).
    /// </summary>
    /// <remarks>
    /// Four-bit numbers widely used by MIDI protocol as parameters of MIDI events.
    /// So instead of manipulating built-in C# numeric types (like byte or int) and checking for
    /// out-of-range errors all validation of numbers in the [0; 15] range happens on data type
    /// level via casting C# integer values to the <see cref="FourBitNumber"/>.
    /// </remarks>
    public struct FourBitNumber : IComparable<FourBitNumber>
    {
        #region Constants

        /// <summary>
        /// The smallest possible value of a <see cref="FourBitNumber"/>.
        /// </summary>
        public static readonly FourBitNumber MinValue = new FourBitNumber(Min);

        /// <summary>
        /// The largest possible value of a <see cref="FourBitNumber"/>.
        /// </summary>
        public static readonly FourBitNumber MaxValue = new FourBitNumber(Max);

        /// <summary>
        /// All possible values of <see cref="FourBitNumber"/>.
        /// </summary>
        public static readonly FourBitNumber[] Values = Enumerable.Range(MinValue, MaxValue - MinValue + 1)
                                                                  .Select(value => (FourBitNumber)value)
                                                                  .ToArray();

        private const byte Min = 0;
        private const byte Max = 15; // 00001111

        #endregion

        #region Fields

        private readonly byte _value;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FourBitNumber"/> with the specified value.
        /// </summary>
        /// <param name="value">Value representing four-bit number.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is out of
        /// [<see cref="MinValue"/>; <see cref="MaxValue"/>] range.</exception>
        public FourBitNumber(byte value)
        {
            ThrowIfArgument.IsOutOfRange(nameof(value), value, Min, Max, "Value is out of range valid for four-bit number.");

            _value = value;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts the string representation of a four-bit number to its <see cref="FourBitNumber"/> equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="input">A string containing a number to convert.</param>
        /// <param name="fourBitNumber">When this method returns, contains the <see cref="FourBitNumber"/>
        /// equivalent of the four-bit number contained in <paramref name="input"/>, if the conversion succeeded,
        /// or zero if the conversion failed. The conversion fails if the <paramref name="input"/> is null or
        /// <see cref="string.Empty"/>, or is not of the correct format. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.</param>
        /// <returns>true if <paramref name="input"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string input, out FourBitNumber fourBitNumber)
        {
            fourBitNumber = default(FourBitNumber);

            byte byteValue;
            var parsed = ShortByteParser.TryParse(input, Min, Max, out byteValue).Status == ParsingStatus.Parsed;
            if (parsed)
                fourBitNumber = (FourBitNumber)byteValue;

            return parsed;
        }

        /// <summary>
        /// Converts the string representation of a four-bit number to its <see cref="FourBitNumber"/> equivalent.
        /// </summary>
        /// <param name="input">A string containing a number to convert.</param>
        /// <returns>A <see cref="FourBitNumber"/> equivalent to the four-bit number contained in
        /// <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="input"/> is null or contains white-spaces only.</exception>
        /// <exception cref="FormatException"><paramref name="input"/> has invalid format.</exception>
        public static FourBitNumber Parse(string input)
        {
            byte byteValue;
            var parsingResult = ShortByteParser.TryParse(input, Min, Max, out byteValue);
            if (parsingResult.Status == ParsingStatus.Parsed)
                return (FourBitNumber)byteValue;

            throw parsingResult.Exception;
        }

        #endregion

        #region Casting

        /// <summary>
        /// Converts the value of a <see cref="FourBitNumber"/> to a <see cref="byte"/>.
        /// </summary>
        /// <param name="number"><see cref="FourBitNumber"/> object to convert to a byte value.</param>
        public static implicit operator byte(FourBitNumber number)
        {
            return number._value;
        }

        /// <summary>
        /// Converts the value of a <see cref="byte"/> to a <see cref="FourBitNumber"/>.
        /// </summary>
        /// <param name="number">Byte value to convert to a <see cref="FourBitNumber"/> object.</param>
        public static explicit operator FourBitNumber(byte number)
        {
            return new FourBitNumber(number);
        }

        #endregion

        #region IComparable<FourBitNumber>

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows, or
        /// occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The
        /// return value has these meanings:
        /// - Less than zero: This instance precedes other in the sort order.
        /// - Zero: This instance occurs in the same position in the sort order as other.
        /// - Greater than zero: This instance follows other in the sort order.</returns>
        public int CompareTo(FourBitNumber other)
        {
            return _value.CompareTo(other._value);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is FourBitNumber))
                return false;

            var fourBitNumber = (FourBitNumber)obj;
            return fourBitNumber._value == _value;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        #endregion
    }
}
