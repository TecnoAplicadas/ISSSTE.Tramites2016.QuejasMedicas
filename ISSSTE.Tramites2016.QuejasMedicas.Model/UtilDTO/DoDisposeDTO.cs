using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class DoDisposeDTO : IDisposable
    {
        private bool _estaDispuesto;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (_estaDispuesto)
                _estaDispuesto = false;
        }
    }
}