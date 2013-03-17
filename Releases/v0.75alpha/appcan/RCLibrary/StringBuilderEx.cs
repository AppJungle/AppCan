/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCLibrary
{

    /// <summary>
    /// StringBuilderEx class is not derived from StringBuilder, it contains a stringbuilder and has
    /// many of the same equivalent functions.  The idea is to use this instead of StringBuilder when possible
    /// and it will provide additional features in the future to prevent allocating or releasing memory.
    /// </summary>
    class StringBuilderEx
    {
        private StringBuilder _stringBuilder;

        public StringBuilderEx()
        {
            _stringBuilder = new StringBuilder();
        }

        public StringBuilderEx(int capacity)
        {
            _stringBuilder = new StringBuilder(capacity);
        }

        public StringBuilderEx(string value)
        {
            _stringBuilder = new StringBuilder(value);
        }

        public StringBuilderEx(int capacity, int maxCapacity)
        {
            _stringBuilder = new StringBuilder(capacity, maxCapacity);
        }

        public StringBuilderEx(string value1, int capacity)
        {
            _stringBuilder = new StringBuilder(value1, capacity);
        }

        public StringBuilderEx(string value1, int startIndex,int length, int capacity)
        {
            _stringBuilder = new StringBuilder(value1, startIndex,length,capacity);
        }

        public int Capacity { get { return _stringBuilder.Capacity; } set { _stringBuilder.Capacity = value;  } }


        public char this[int index]
        {
            get
            {

                return _stringBuilder[index];
            }

            set
            {
                _stringBuilder[index] = value;

            }

        }

        public int Length { get { return _stringBuilder.Length; } set { _stringBuilder.Length = value; } }

        public int MaxCapacity { get { return _stringBuilder.MaxCapacity; }  }

        public override bool Equals(object value)
        {

            return _stringBuilder.Equals(value);
        }

        public bool Equals(StringBuilder value)
        {

            return _stringBuilder.Equals(value);
        }


        public override int GetHashCode()
        {

            return _stringBuilder.GetHashCode();
        }

        public StringBuilderEx Insert(int index, bool value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, byte value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, char value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, char[] value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, decimal value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, double value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, short value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, int value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, long value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, object value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, sbyte value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, float value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, string value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, ushort value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, uint value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }

        public StringBuilderEx Insert(int index, ulong value)
        {
            _stringBuilder.Insert(index, value);
            return this;
        }


        public StringBuilderEx Insert(int index, string value,int count)
        {
            _stringBuilder.Insert(index, value,count);
            return this;
        }


        public StringBuilderEx Insert(int index, char[] value,int startIndex, int charCount)
        {
            _stringBuilder.Insert(index, value, startIndex,charCount);
            return this;
        }

        public StringBuilderEx Remove(int startIndex, int length)
        {
            _stringBuilder.Remove(startIndex, length);
            return this;
        }

        public StringBuilderEx Replace(char oldChar, char newChar)
        {
            _stringBuilder.Replace(oldChar, newChar);
            return this;
        }

        public StringBuilderEx Replace(string oldString, string newString)
        {
            _stringBuilder.Replace(oldString, newString);
            return this;
        }

        public StringBuilderEx Replace(char oldChar, char newChar,int startIndex,int count)
        {
            _stringBuilder.Replace(oldChar, newChar,startIndex,count);
            return this;
        }

        public StringBuilderEx Replace(string oldValue, string newValue, int startIndex, int count)
        {
            _stringBuilder.Replace(oldValue, newValue, startIndex, count);
            return this;
        }


        public override string ToString()
        {
            return _stringBuilder.ToString();
        }




    }
}
