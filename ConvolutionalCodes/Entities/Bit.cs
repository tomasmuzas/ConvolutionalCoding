﻿namespace ConvolutionalCodes.Entities
{
    public struct Bit
    {
        private bool _value;

        public Bit(byte value)
        {
            _value = (value & 0x1) == 1;
        }

        public Bit(int value)
        {
            _value = (value & 0x1) == 1;
        }

        public Bit(bool value)
        {
            _value = value;
        }

        public static implicit operator Bit(bool value)
        {
            return new Bit(value);
        }

        public static explicit operator bool(Bit bit)
        {
            return bit._value;
        }

        public static explicit operator int(Bit bit)
        {
            return bit._value ? 1 : 0;
        }

        public static bool operator ==(Bit bit1, Bit bit2)
        {
            return bit1._value == bit2._value;
        }

        public static bool operator !=(Bit bit1, Bit bit2)
        {
            return bit1._value != bit2._value;
        }

        public static Bit operator & (Bit bit1, Bit bit2)
        {
            return bit1._value & bit2._value;
        }

        public static Bit operator | (Bit bit1, Bit bit2)
        {
            return bit1._value | bit2._value;
        }

        public static Bit operator ^ (Bit bit1, Bit bit2)
        {
            return bit1._value ^ bit2._value;
        }

        public static Bit operator ! (Bit bit)
        {
            return !bit._value;
        }

        public override string ToString()
        {
            return _value ? "1" : "0";
        }
    }
}
