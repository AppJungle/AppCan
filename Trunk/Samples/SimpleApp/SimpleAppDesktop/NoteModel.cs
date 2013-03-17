using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppCan.Core.Component;
using AppCan.Core.Application;
using System.Runtime.Serialization;

namespace HelloWorld
{
    [Serializable]
    class NoteModel : BaseNotifyPropertyChanged, IModel, ISerializable
    {
        string _notes = null;

        public NoteModel()
        {

        }


        public NoteModel(SerializationInfo info, StreamingContext ctxt)
        {
            this._notes = (string)info.GetValue("Notes", typeof(string));

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Notes", this._notes);

        }

        public string Notes
        {
            get
            {
                return _notes;

            }

            set
            {
                _notes = value;
                InvokePropertyChanged("Notes");

            }

        }
    }
}
