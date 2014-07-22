using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BtrieveWrapper;

namespace BtrieveWrapper
{
    public class OperationResult
    {
        internal OperationResult(
            Operation operationType,
            PositionBlock positionBlock,
            byte[] dataBuffer,
            int dataBufferLength,
            byte[] keyBuffer,
            sbyte keyNumber,
            ClientId clientId) {

            this.OperationType = operationType;
            this.PositionBlock = positionBlock;
            this.DataBuffer = dataBuffer;
            this.DataBufferLength = dataBufferLength;
            this.KeyBuffer = keyBuffer;
            this.KeyNumber = keyNumber;
            this.ClientId = clientId;
        }

        public PositionBlock PositionBlock { get; private set; }
        public Operation OperationType { get; private set; }
        public byte[] DataBuffer { get; private set; }
        public int DataBufferLength { get; private set; }
        public byte[] KeyBuffer { get; private set; }
        public sbyte KeyNumber { get; private set; }
        public ClientId ClientId { get; private set; }

    }
}
