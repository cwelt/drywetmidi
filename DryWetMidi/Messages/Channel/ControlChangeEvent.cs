﻿namespace Melanchall.DryWetMidi
{
    public sealed class ControlChangeEvent : ChannelEvent
    {
        #region Constructor

        public ControlChangeEvent()
            : base(2)
        {
        }

        public ControlChangeEvent(SevenBitNumber controlNumber, SevenBitNumber controlValue)
            : this()
        {
            ControlNumber = controlNumber;
            ControlValue = controlValue;
        }

        public ControlChangeEvent(ControlType control, SevenBitNumber controlValue)
            : this()
        {
            Control = control;
            ControlValue = controlValue;
        }

        #endregion

        #region Properties

        public SevenBitNumber ControlNumber
        {
            get { return _parameters[0]; }
            set { _parameters[0] = value; }
        }

        public SevenBitNumber ControlValue
        {
            get { return _parameters[1]; }
            set { _parameters[1] = value; }
        }

        public ControlType Control
        {
            get { return (ControlType)(byte)ControlNumber; }
            set { ControlNumber = (SevenBitNumber)(byte)value; }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return $"Control Change (channel = {Channel}, control number = {ControlNumber}, control value = {ControlValue})";
        }

        #endregion
    }
}
