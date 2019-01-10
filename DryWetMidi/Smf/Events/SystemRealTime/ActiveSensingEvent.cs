﻿namespace Melanchall.DryWetMidi.Smf
{
    /// <summary>
    /// Represents Active Sensing event.
    /// </summary>
    /// <remarks>
    /// A MIDI event that carries the MIDI active sense message tells a MIDI device
    /// that the MIDI connection is still active.
    /// </remarks>
    public sealed class ActiveSensingEvent : SystemRealTimeEvent
    {
        #region Overrides

        /// <summary>
        /// Clones event by creating a copy of it.
        /// </summary>
        /// <returns>Copy of the event.</returns>
        protected override MidiEvent CloneEvent()
        {
            return new ActiveSensingEvent();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "Active Sensing";
        }

        #endregion
    }
}
