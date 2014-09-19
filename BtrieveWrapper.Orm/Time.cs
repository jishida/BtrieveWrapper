using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm
{
    public struct Time
    {
        byte _hour;
        byte _minute;
        byte _second;
        byte _tenMillisecond;

        public Time(byte hour, byte minute, byte second, byte tenMillisecond) {
            _hour = hour;
            _minute = minute;
            _second = second;
            _tenMillisecond = tenMillisecond;
        }

        public int Hour {
            get {
                return _hour;
            }
            set {
                if (value > 23 || value < 0) {
                    throw new ArgumentOutOfRangeException();
                }
                _hour = (byte)value;
            }
        }

        public int Minute {
            get {
                return _minute;
            }
            set {
                if (value > 59 || value < 0) {
                    throw new ArgumentOutOfRangeException();
                }
                _minute = (byte)value;
            }
        }

        public int Second {
            get {
                return _second;
            }
            set {
                if (value > 59 || value < 0) {
                    throw new ArgumentOutOfRangeException();
                }
                _second = (byte)value;
            }
        }

        public int Millisecond {
            get {
                return _tenMillisecond * 10;
            }
            set {
                if (value > 999 || value < 0) {
                    throw new ArgumentOutOfRangeException();
                }
                _tenMillisecond = (byte)(value / 10);
            }
        }

        public void SetDataBuffer(byte[] dataBuffer, ushort position) {
            dataBuffer[position] = _tenMillisecond;
            dataBuffer[position + 1] = _second;
            dataBuffer[position + 2] = _minute;
            dataBuffer[position + 3] = _hour;
        }

        public override int GetHashCode() {
            var hashCode = 0;
            hashCode |= _hour;
            hashCode |= _minute << 8;
            hashCode |= _second << 16;
            hashCode |= _tenMillisecond << 24;
            return hashCode;
        }

        public override bool Equals(object obj) {
            try {
                var time = (Time)obj;
                return _hour == time._hour && _minute == time._minute && _second == time._second && _tenMillisecond == time._tenMillisecond;
            } catch {
                return false;
            }
        }

        public static bool operator ==(Time time1, Time time2) {
            return time1.Equals(time2);
        }

        public static bool operator !=(Time time1, Time time2) {
            return !time1.Equals(time2);
        }

        public override string ToString() {
            return String.Format("{0}:{1:D2}:{2:D2}.{3:D3}", _hour, _minute, _second, _tenMillisecond);
        }
    }
}
