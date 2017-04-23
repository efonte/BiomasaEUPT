using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Clases
{
    public class CursorEspera : IDisposable
    {
        private Cursor _previousCursor;

        public CursorEspera()
        {
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }
    }
}
