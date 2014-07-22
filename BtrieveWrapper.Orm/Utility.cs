using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public class Utility
    {
        public static void SetMaxValue(KeyType keyType, byte[] buffer, ushort position, ushort length, object parameter) {
            switch (keyType) {
                case KeyType.Autoincrement:
                case KeyType.Currency:
                case KeyType.Money:
                case KeyType.Integer:
                    for (var i = 0; i < length; i++) {
                        if (i == length - 1) {
                            buffer[position + i] = 0x7f;
                        } else {
                            buffer[position + i] = 0xff;
                        }
                    }
                    break;
                case KeyType.Decimal:
                    for (var i = 0; i < length; i++) {
                        if (i == length - 1) {
                            buffer[position + i] = 0x9f;
                        } else {
                            buffer[position + i] = 0x99;
                        }
                    }
                    break;
                case KeyType.UnsignedBinary:
                case KeyType.String:
                    for (var i = 0; i < length; i++) {
                        buffer[position + i] = 0xff;
                    }
                    break;
                case KeyType.ZString:
                    for (var i = 0; i < length; i++) {
                        buffer[position + i] = i == length - 1 ? (byte)0x00 : (byte)0xff;
                    }
                    break;
                case KeyType.Float:
                    switch (length) {
                        case 4:
                            Array.Copy(BitConverter.GetBytes(float.MaxValue), 0, buffer, position, length);
                            break;
                        case 8:
                            Array.Copy(BitConverter.GetBytes(double.MaxValue), 0, buffer, position, length);
                            break;
                    }
                    break;
                case KeyType.Time:
                    buffer[position] = 99;
                    buffer[position + 1] = 59;
                    buffer[position + 2] = 59;
                    buffer[position + 3] = 23;
                    break;
                case KeyType.Timestamp:
                    Array.Copy(BitConverter.GetBytes(DateTime.MaxValue.Ticks), 0, buffer, position, length);
                    break;
                case KeyType.Date:
                    buffer[position] = (byte)31;
                    buffer[position + 1] = (byte)12;
                    Array.Copy(BitConverter.GetBytes((short)9999), 0, buffer, position + 2, 2);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public static void SetMinValue(KeyType keyType, byte[] buffer, ushort position, ushort length, object parameter) {
            switch (keyType) {
                case KeyType.Autoincrement:
                case KeyType.Currency:
                case KeyType.Integer:
                    for (var i = 0; i < length; i++) {
                        if (i == length - 1) {
                            buffer[position + i] = 0x80;
                        } else {
                            buffer[position + i] = 0x00;
                        }
                    }
                    break;
                case KeyType.Money:
                case KeyType.Decimal:
                    for (var i = 0; i < length; i++) {
                        if (i == length - 1) {
                            buffer[position + i] = 0x9d;
                        } else {
                            buffer[position + i] = 0x99;
                        }
                    }
                    break;
                case KeyType.UnsignedBinary:
                case KeyType.String:
                case KeyType.ZString:
                case KeyType.Time:
                case KeyType.Timestamp:
                    for (var i = 0; i < length; i++) {
                        buffer[position + i] = 0x00;
                    }
                    break;
                case KeyType.Float:
                    switch (length) {
                        case 4:
                            Array.Copy(BitConverter.GetBytes(float.MinValue), 0, buffer, position, length);
                            break;
                        case 8:
                            Array.Copy(BitConverter.GetBytes(double.MinValue), 0, buffer, position, length);
                            break;
                    }
                    break;
                case KeyType.Date:
                    buffer[position + 0] = (byte)1;
                    buffer[position + 1] = (byte)1;
                    Array.Copy(BitConverter.GetBytes((short)1), 0, buffer, position + 2, 2);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        internal static LockBias GetLockBias(LockMode lockMode) {
            LockBias lockBias;
            switch (lockMode) {
                case LockMode.None:
                    lockBias = LockBias.None;
                    break;
                case LockMode.WaitLock:
                    lockBias = LockBias.MultiRecordWaitLock;
                    break;
                case LockMode.NoWaitLock:
                    lockBias = LockBias.MultiRecordNoWaitLock;
                    break;
                default:
                    throw new ArgumentException();
            }
            return lockBias;
        }
    }
}
