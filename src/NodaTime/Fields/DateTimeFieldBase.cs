﻿#region Copyright and license information
// Copyright 2001-2009 Stephen Colebourne
// Copyright 2009 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System;

namespace NodaTime.Fields
{
    /// <summary>
    /// DateTimeFieldBase provides the common behaviour for DateTimeField implementations.
    /// </summary>
    public abstract class DateTimeFieldBase : IDateTimeField
    {
        private readonly DateTimeFieldType fieldType;
        
        protected DateTimeFieldBase(DateTimeFieldType fieldType)
        {
            if (fieldType == null)
            {
                throw new ArgumentNullException("fieldType");
            }
            this.fieldType = fieldType;
        }

        /// <summary>
        /// Get the type of the field.
        /// </summary>
        public DateTimeFieldType FieldType { get { return fieldType; } }

        /// <summary>
        /// Gets the duration per unit value of this field, or UnsupportedDurationField if field has no duration.
        /// For example, if this
        /// field represents "hour of day", then the duration is an hour.
        /// </summary>
        public abstract DurationField DurationField { get; }

        /// <summary>
        /// Returns the range duration of this field. For example, if this field
        /// represents "hour of day", then the range duration is a day.
        /// </summary>
        public abstract DurationField RangeDurationField { get; }

        /// <summary>
        /// Defaults to fields being supported
        /// </summary>
        public virtual bool IsSupported { get { return true; } }

        /// <summary>
        /// Returns true if the set method is lenient. If so, it accepts values that
        /// are out of bounds. For example, a lenient day of month field accepts 32
        /// for January, converting it to February 1.
        /// </summary>
        public abstract bool IsLenient { get; }

        #region Values

        /// <summary>
        /// Get the value of this field from the local instant.
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <returns>The value of the field, in the units of the field</returns>
        public virtual int GetValue(LocalInstant localInstant)
        {
            return (int) GetInt64Value(localInstant);
        }

        /// <summary>
        /// Get the value of this field from the local instant.
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <returns>The value of the field, in the units of the field</returns>
        public abstract long GetInt64Value(LocalInstant localInstant);

        /// <summary>
        /// Adds a value (which may be negative) to the local instant value.
        /// <para>
        /// The value will be added to this field. If the value is too large to be
        /// added solely to this field, larger fields will increase as required.
        /// Smaller fields should be unaffected, except where the result would be
        /// an invalid value for a smaller field. In this case the smaller field is
        /// adjusted to be in range.
        /// </para>
        /// For example, in the ISO chronology:<br>
        /// 2000-08-20 add six months is 2001-02-20<br>
        /// 2000-08-20 add twenty months is 2002-04-20<br>
        /// 2000-08-20 add minus nine months is 1999-11-20<br>
        /// 2001-01-31 add one month  is 2001-02-28<br>
        /// 2001-01-31 add two months is 2001-03-31<br>
        /// </summary>
        /// <param name="localInstant">The local instant to add to</param>
        /// <param name="value">The value to add, in the units of the field</param>
        /// <returns>The updated local instant</returns>
        public virtual LocalInstant Add(LocalInstant localInstant, int value)
        {
            return DurationField.Add(localInstant, value);
        }

        /// <summary>
        /// Adds a value (which may be negative) to the local instant value.
        /// <para>
        /// The value will be added to this field. If the value is too large to be
        /// added solely to this field, larger fields will increase as required.
        /// Smaller fields should be unaffected, except where the result would be
        /// an invalid value for a smaller field. In this case the smaller field is
        /// adjusted to be in range.
        /// </para>
        /// For example, in the ISO chronology:<br>
        /// 2000-08-20 add six months is 2001-02-20<br>
        /// 2000-08-20 add twenty months is 2002-04-20<br>
        /// 2000-08-20 add minus nine months is 1999-11-20<br>
        /// 2001-01-31 add one month  is 2001-02-28<br>
        /// 2001-01-31 add two months is 2001-03-31<br>
        /// </summary>
        /// <param name="localInstant">The local instant to add to</param>
        /// <param name="value">The value to add, in the units of the field</param>
        /// <returns>The updated local instant</returns>
        public virtual LocalInstant Add(LocalInstant localInstant, long value)
        {
            return DurationField.Add(localInstant, value);
        }

        /// <summary>
        /// Computes the difference between two instants, as measured in the units
        /// of this field. Any fractional units are dropped from the result. Calling
        /// GetDifference reverses the effect of calling add. In the following code:
        /// <code>
        /// LocalInstant instant = ...
        /// int v = ...
        /// int age = GetDifference(Add(instant, v), instant);
        /// </code>
        /// The value 'age' is the same as the value 'v'.
        /// </summary>
        /// <param name="minuendInstant">The local instant to subtract from</param>
        /// <param name="subtrahendInstant">The local instant to subtract from minuendInstant</param>
        /// <returns>The difference in the units of this field</returns>
        public virtual int GetDifference(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
        {
            return DurationField.GetDifference(minuendInstant, subtrahendInstant);
        }

        /// <summary>
        /// Computes the difference between two instants, as measured in the units
        /// of this field. Any fractional units are dropped from the result. Calling
        /// GetDifference reverses the effect of calling add. In the following code:
        /// <code>
        /// LocalInstant instant = ...
        /// int v = ...
        /// int age = GetDifference(Add(instant, v), instant);
        /// </code>
        /// The value 'age' is the same as the value 'v'.
        /// </summary>
        /// <param name="minuendInstant">The local instant to subtract from</param>
        /// <param name="subtrahendInstant">The local instant to subtract from minuendInstant</param>
        /// <returns>The difference in the units of this field</returns>
        public virtual long GetInt64Difference(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
        {
            return DurationField.GetInt64Difference(minuendInstant, subtrahendInstant);
        }

        /// <summary>
        /// Sets a value in the milliseconds supplied.
        /// <para>
        /// The value of this field will be set.
        /// If the value is invalid, an exception if thrown.
        /// </para>
        /// <para>
        /// If setting this field would make other fields invalid, then those fields
        /// may be changed. For example if the current date is the 31st January, and
        /// the month is set to February, the day would be invalid. Instead, the day
        /// would be changed to the closest value - the 28th/29th February as appropriate.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to set in</param>
        /// <param name="value">The value to set, in the units of the field</param>
        /// <returns>The updated local instant</returns>
        public abstract LocalInstant SetValue(LocalInstant localInstant, long value);

        #endregion

        #region Leap

        /// <summary>
        /// Defaults to non-leap.
        /// </summary>
        public virtual bool IsLeap(LocalInstant localInstant)
        {
            return false;
        }

        /// <summary>
        /// Defaults to 0.
        /// </summary>
        public virtual int GetLeapAmount(LocalInstant localInstant)
        {
            return 0;
        }

        /// <summary>
        /// Defaults to null, i.e. no leap duration field.
        /// </summary>
        public virtual DurationField LeapDurationField { get { return null; } }

        #endregion

        #region Ranges

        /// <summary>
        /// Defaults to the absolute maximum for the field.
        /// </summary>
        public virtual long GetMaximumValue(LocalInstant localInstant)
        {
            return GetMaximumValue();
        }

        /// <summary>
        /// Defaults to the absolute maximum for the field.
        /// </summary>
        /// <param name="instant"></param>
        /// <returns></returns>
        public virtual long GetMaximumValue(IPartial instant)
        {
            return GetMaximumValue();
        }

        /// <summary>
        /// Defaults to the absolute maximum for the field.
        /// </summary>
        /// <param name="instant"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual long GetMaximumValue(IPartial instant, int[] values)
        {
            return GetMaximumValue();
        }

        /// <summary>
        /// Get the maximum allowable value for this field.
        /// </summary>
        /// <returns>The maximum valid value for this field, in the units of the field</returns>
        public abstract long GetMaximumValue();

        /// <summary>
        /// Defaults to the absolute minimum for the field.
        /// </summary>
        public virtual long GetMinimumValue(LocalInstant localInstant)
        {
            return GetMinimumValue();
        }

        /// <summary>
        /// Defaults to the absolute minimum for the field.
        /// </summary>
        /// <param name="instant"></param>
        /// <returns></returns>
        public virtual long GetMinimumValue(IPartial instant)
        {
            return GetMinimumValue();
        }

        /// <summary>
        /// Defaults to the absolute minimum for the field.
        /// </summary>
        /// <param name="instant"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public virtual long GetMinimumValue(IPartial instant, int[] values)
        {
            return GetMinimumValue();
        }

        /// <summary>
        /// Get the minimum allowable value for this field.
        /// </summary>
        /// <returns>The minimum valid value for this field, in the units of the field</returns>
        public abstract long GetMinimumValue();

        #endregion

        #region Rounding

        /// <summary>
        /// Round to the lowest whole unit of this field. After rounding, the value
        /// of this field and all fields of a higher magnitude are retained. The
        /// fractional millis that cannot be expressed in whole increments of this
        /// field are set to minimum.
        /// <para>
        /// For example, a datetime of 2002-11-02T23:34:56.789, rounded to the
        /// lowest whole hour is 2002-11-02T23:00:00.000.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to round</param>
        /// <returns>Rounded local instant</returns>
        public abstract LocalInstant RoundFloor(LocalInstant localInstant);

        /// <summary>
        /// Round to the highest whole unit of this field. The value of this field
        /// and all fields of a higher magnitude may be incremented in order to
        /// achieve this result. The fractional millis that cannot be expressed in
        /// whole increments of this field are set to minimum.
        /// <para>
        /// For example, a datetime of 2002-11-02T23:34:56.789, rounded to the
        /// highest whole hour is 2002-11-03T00:00:00.000.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to round</param>
        /// <returns>Rounded local instant</returns>
        public virtual LocalInstant RoundCeiling(LocalInstant localInstant)
        {
            LocalInstant newInstant = RoundFloor(localInstant);
            if (newInstant != localInstant)
            {
                newInstant = Add(newInstant, 1);
            }
            return newInstant;
        }

        /// <summary>
        /// Round to the nearest whole unit of this field. If the given local instant
        /// is closer to the floor or is exactly halfway, this function
        /// behaves like RoundFloor. If the local instant is closer to the
        /// ceiling, this function behaves like RoundCeiling.
        /// </summary>
        /// <param name="localInstant">The local instant to round</param>
        /// <returns>Rounded local instant</returns>
        public virtual LocalInstant RoundHalfFloor(LocalInstant localInstant)
        {
            LocalInstant floor = RoundFloor(localInstant);
            LocalInstant ceiling = RoundCeiling(localInstant);

            Duration diffFromFloor = localInstant - floor;
            Duration diffToCeiling = ceiling - localInstant;

             // Closer to the floor, or halfway - round floor
            return diffFromFloor <= diffToCeiling ? floor : ceiling;
        }

        /// <summary>
        /// Round to the nearest whole unit of this field. If the given local instant
        /// is closer to the floor, this function behaves like RoundFloor. If
        /// the local instant is closer to the ceiling or is exactly halfway,
        /// this function behaves like RoundCeiling.
        /// </summary>
        /// <param name="localInstant">The local instant to round</param>
        /// <returns>Rounded local instant</returns>
        public virtual LocalInstant RoundHalfCeiling(LocalInstant localInstant)
        {
            LocalInstant floor = RoundFloor(localInstant);
            LocalInstant ceiling = RoundCeiling(localInstant);

            long diffFromFloor = localInstant.Ticks - floor.Ticks;
            long diffToCeiling = ceiling.Ticks - localInstant.Ticks;

             // Closer to the ceiling, or halfway - round ceiling
            return diffToCeiling <= diffFromFloor ? ceiling : floor;
        }

        /// <summary>
        /// Round to the nearest whole unit of this field. If the given local instant
        /// is closer to the floor, this function behaves like RoundFloor. If
        /// the local instant is closer to the ceiling, this function behaves
        /// like RoundCeiling.
        /// </summary>
        /// <param name="localInstant">The local instant to round</param>
        /// <returns>Rounded local instant</returns>
        public virtual LocalInstant RoundHalfEven(LocalInstant localInstant)
        {
            LocalInstant floor = RoundFloor(localInstant);
            LocalInstant ceiling = RoundCeiling(localInstant);

            Duration diffFromFloor = localInstant - floor;
            Duration diffToCeiling = ceiling - localInstant;

            // Closer to the floor - round floor
            if (diffFromFloor < diffToCeiling)
            {
                return floor;
            }
            // Closer to the ceiling - round ceiling
            else if (diffToCeiling < diffFromFloor)
            {
                return ceiling;
            }
            else
            {
                // Round to the instant that makes this field even. If both values
                // make this field even (unlikely), favor the ceiling.
                return (GetInt64Value(ceiling) & 1) == 0 ? ceiling : floor;
            }
        }

        /// <summary>
        /// Returns the fractional duration of this field. In other words, 
        /// calling Remainder returns the duration that RoundFloor would subtract.
        /// <para>
        /// For example, on a datetime of 2002-11-02T23:34:56.789, the remainder by
        /// hour is 34 minutes and 56.789 seconds.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to get the remainder</param>
        /// <returns>Remainder duration</returns>
        public virtual Duration Remainder(LocalInstant localInstant)
        {
            return localInstant - RoundFloor(localInstant);
        }

        #endregion

        #region Text

        /// <summary>
        /// Get the human-readable, text value of this field from the milliseconds.
        /// <para>
        /// The default implementation calls <see cref="GetAsText(int, IFormatProvider"/>.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsText(LocalInstant localInstant, IFormatProvider provider)
        {
            return GetAsText(GetValue(localInstant), provider);
        }

        /// <summary>
        /// Get the human-readable, text value of this field from the milliseconds.
        /// <para>
        /// The default implementation calls <see cref="GetAsText(int, IFormatProvider"/>.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsText(LocalInstant localInstant)
        {
            return GetAsText(localInstant, null);
        }

        /// <summary>
        /// Get the human-readable, text value of this field from a partial instant.
        /// <para>
        /// The default implementation returns GetAsText(fieldValue, provider).
        /// </para>
        /// </summary>
        /// <param name="partial">The partial instant to query</param>
        /// <param name="fieldValue">The field value of this field, provided for performance</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsText(IPartial partial, int fieldValue, IFormatProvider provider)
        {
            return GetAsText(fieldValue, provider);
        }

        /// <summary>
        /// Get the human-readable, text value of this field from a partial instant.
        /// <para>
        /// The default implementation calls <see cref="IPartial.Get(DateTimeFieldType)"/>
        /// and <see cref="GetAsText(IPartial, int IFormatProvider"/>
        /// </para>
        /// </summary>
        /// <param name="partial">The partial instant to query</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsText(IPartial partial, IFormatProvider provider)
        {
            return GetAsText(partial, partial.Get(FieldType), provider);
        }

        /// <summary>
        /// Get the human-readable, text value of this field from the field value.
        /// <para>
        /// The default implementation returns fieldValue.ToString(provider).
        /// </para>
        /// <para>
        /// Note: subclasses that override this method should also override
        /// GetMaximumTextLength.
        /// </para>
        /// </summary>
        /// <param name="fieldValue">the numeric value to convert to text</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsText(int fieldValue, IFormatProvider provider)
        {
            return fieldValue.ToString(provider);
        }

        /// <summary>
        /// Get the human-readable, short text value of this field from the milliseconds.
        /// <para>
        /// The default implementation calls <see cref="GetAsShortText(int, IFormatProvider"/>.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsShortText(LocalInstant localInstant, IFormatProvider provider)
        {
            return GetAsShortText(GetValue(localInstant), provider);
        }

        /// <summary>
        /// Get the human-readable, short text value of this field from the milliseconds.
        /// <para>
        /// The default implementation calls <see cref="GetAsShortText(int, IFormatProvider"/>.
        /// </para>
        /// </summary>
        /// <param name="localInstant">The local instant to query</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsShortText(LocalInstant localInstant)
        {
            return GetAsShortText(localInstant, null);
        }

        /// <summary>
        /// Get the human-readable, short text value of this field from a partial instant.
        /// <para>
        /// The default implementation returns GetAsShortText(fieldValue, provider).
        /// </para>
        /// </summary>
        /// <param name="partial">The partial instant to query</param>
        /// <param name="fieldValue">The field value of this field, provided for performance</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsShortText(IPartial partial, int fieldValue, IFormatProvider provider)
        {
            return GetAsShortText(fieldValue, provider);
        }

        /// <summary>
        /// Get the human-readable, short text value of this field from a partial instant.
        /// <para>
        /// The default implementation calls <see cref="IPartial.Get(DateTimeFieldType)"/>
        /// and <see cref="GetAsShortText(IPartial, int IFormatProvider"/>
        /// </para>
        /// </summary>
        /// <param name="partial">The partial instant to query</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsShortText(IPartial partial, IFormatProvider provider)
        {
            return GetAsShortText(partial, partial.Get(FieldType), provider);
        }

        /// <summary>
        /// Get the human-readable, short text value of this field from the field value.
        /// <para>
        /// The default implementation calls <see cref="GetAsText(int, IFormatProvider"/>.
        /// </para>
        /// <para>
        /// Note: subclasses that override this method should also override
        /// GetMaximumShortTextLength.
        /// </para>
        /// </summary>
        /// <param name="fieldValue">the numeric value to convert to text</param>
        /// <param name="provider">Format provider to use</param>
        /// <returns>The text value of the field</returns>
        public virtual string GetAsShortText(int fieldValue, IFormatProvider provider)
        {
            return GetAsText(fieldValue, provider);
        }

        #endregion

        public override string ToString()
        {
            return fieldType.ToString();
        }
    }
}