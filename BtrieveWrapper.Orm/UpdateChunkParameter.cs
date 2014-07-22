using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class UpdateChunkParameter
    {
        public UpdateChunkParameter(VariableRange range, byte[] data, int dataPosition = 0) {
            if (data == null) {
                throw new ArgumentNullException();
            }
            if (dataPosition < 0 ||data.Length < range.Length) {
                throw new ArgumentOutOfRangeException();
            }
            this.Range = range;
            this.Data = data;
            this.DataPosition = dataPosition;
        }

        public VariableRange Range { get; private set; }
        public byte[] Data { get; private set; }
        public int DataPosition { get; private set; }
    }
}
