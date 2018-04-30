using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model
{
    [Serializable]
    public class QuejasMedicasException : Exception
    {
        public List<MensajeException> mensajes;

        public QuejasMedicasException()
        {
            mensajes = new List<MensajeException>();
        }

        public QuejasMedicasException(string message) : base(message)
        {
        }

        public QuejasMedicasException(int CodigoError, string message) : base(message)
        {
            mensajes.Add(new MensajeException {CodigoError = CodigoError, Descripcion = message});
        }

        public QuejasMedicasException(int CodigoError, string message, Exception innerException)
            : base(message, innerException)
        {
            mensajes.Add(new MensajeException {CodigoError = CodigoError, Descripcion = message});
        }

        public QuejasMedicasException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected QuejasMedicasException(SerializationInfo info, StreamingContext context)
        {
            mensajes = (List<MensajeException>) info.GetValue("mensajes", typeof(List<MensajeException>));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("mensajes", mensajes, typeof(List<MensajeException>));
        }
    }

    //[DataContract]
    public class MensajeException
    {
        //[DataMember]
        public int CodigoError { set; get; }

        //[DataMember]
        public string Descripcion { set; get; }
    }
}