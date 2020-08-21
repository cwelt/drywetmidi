using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Tests.Interaction
{
    [TestFixture]
    public sealed class ChannelCoarseTuningParameterTests
    {
        #region Test methods

        [Test]
        public void CheckDefaultData()
        {
            var parameter = new ChannelCoarseTuningParameter();
            Assert.AreEqual(0, parameter.HalfSteps, "Default half-steps number is invaid.");
            CheckTimedEvents(parameter,
                (101, 0x00), (100, 0x02),
                (6, 0x40),
                (101, 0x7F), (100, 0x7F));
        }

        [TestCase(-64, 0x00)]
        [TestCase(63, 0x7F)]
        public void CheckData(sbyte halfSteps, byte expectedDataByte)
        {
            var parameter = new ChannelCoarseTuningParameter(halfSteps);
            CheckTimedEvents(parameter,
                (101, 0x00), (100, 0x02),
                (6, expectedDataByte),
                (101, 0x7F), (100, 0x7F));
        }

        #endregion

        #region Private methods

        private static void CheckTimedEvents(RegisteredParameter registeredParameter, params (byte ControlNumber, byte ControlValue)[] expectedEvents)
        {
            var timedEvents = registeredParameter.GetTimedEvents();
            Assert.AreEqual(1, timedEvents.Select(e => e.Time).Distinct().Count(), "Time is different for some timed events.");
            Assert.IsTrue(timedEvents.All(e => e.Time == registeredParameter.Time), "Time is invalid.");

            var midiEvents = timedEvents.Select(e => e.Event).ToArray();
            Assert.IsTrue(midiEvents.All(e => e.EventType == MidiEventType.ControlChange), "Some events have not Control Change type.");
            Assert.IsTrue(midiEvents.All(e => e is ControlChangeEvent), "Some events are not Control Change ones.");

            Assert.That(
                midiEvents,
                Is.EqualTo(expectedEvents.Select(e => new ControlChangeEvent((SevenBitNumber)e.ControlNumber, (SevenBitNumber)e.ControlValue) { Channel = registeredParameter.Channel })).Using(new MidiEventEqualityComparer()),
                "Events are invalid.");
        }

        #endregion
    }
}
