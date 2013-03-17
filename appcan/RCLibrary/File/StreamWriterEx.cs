/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace RCLibrary.File
{
    class StreamWriterEx : StreamWriter
    {
        StreamWriterEx(Stream stream) : base(stream)
        {
            

        }

        #region Overrides of StreamWriter

        public override void Close()
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            base.Flush();
        }

        public override void Write(char value)
        {
            base.Write(value);
        }

        public override void Write(char[] buffer)
        {
            base.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
        }

        public override void Write(bool value)
        {
            base.Write(value);
        }

        public override void Write(int value)
        {
            base.Write(value);
        }

        public override void Write(uint value)
        {
            base.Write(value);
        }

        public override void Write(long value)
        {
            base.Write(value);
        }

        public override void Write(ulong value)
        {
            base.Write(value);
        }

        public override void Write(float value)
        {
            base.Write(value);
        }

        public override void Write(double value)
        {
            base.Write(value);
        }

        public override void Write(decimal value)
        {
            base.Write(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void Write(object value)
        {
            base.Write(value);
        }

        public override void Write(string format, object arg0)
        {
            base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            base.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            base.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            base.Write(format, arg);
        }

        public override void WriteLine()
        {
            base.WriteLine();
        }

        public override void WriteLine(char value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            base.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            base.WriteLine(buffer, index, count);
        }

        public override void WriteLine(bool value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(decimal value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            base.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            base.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            base.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            base.WriteLine(format, arg);
        }

        public override IFormatProvider FormatProvider
        {
            get { return base.FormatProvider; }
        }

        public override bool AutoFlush { get; set; }

        public override Stream BaseStream
        {
            get { return base.BaseStream; }
        }

        public override Encoding Encoding
        {
            get { return base.Encoding; }
        }

        public override string NewLine { get; set; }

        #endregion

        #region Overrides of MarshalByRefObject

        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }

        #endregion

        #region Overrides of Object

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
