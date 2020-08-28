using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Tests.Interaction
{
    [TestFixture]
    public class TempoMapTests
    {
        #region Test methods

        [Test]
        public void Default()
        {
            TestSimpleTempoMap(TempoMap.Default,
                               new TicksPerQuarterNoteTimeDivision(),
                               Tempo.Default,
                               TimeSignature.Default);
        }

        [Test]
        public void Create_Tempo_TimeSignature()
        {
            var expectedTempo = Tempo.FromBeatsPerMinute(123);
            var expectedTimeSignature = new TimeSignature(3, 8);

            TestSimpleTempoMap(TempoMap.Create(expectedTempo, expectedTimeSignature),
                               new TicksPerQuarterNoteTimeDivision(),
                               expectedTempo,
                               expectedTimeSignature);
        }

        [Test]
        public void Create_Tempo()
        {
            var expectedTempo = new Tempo(123456);

            TestSimpleTempoMap(TempoMap.Create(expectedTempo),
                               new TicksPerQuarterNoteTimeDivision(),
                               expectedTempo,
                               TimeSignature.Default);
        }

        [Test]
        public void Create_TimeSignature()
        {
            var expectedTimeSignature = new TimeSignature(3, 8);

            TestSimpleTempoMap(TempoMap.Create(expectedTimeSignature),
                               new TicksPerQuarterNoteTimeDivision(),
                               Tempo.Default,
                               expectedTimeSignature);
        }

        [Test]
        public void Create_TimeDivision_Tempo_TimeSignature()
        {
            var expectedTimeDivision = new TicksPerQuarterNoteTimeDivision(10000);
            var expectedTempo = Tempo.FromBeatsPerMinute(123);
            var expectedTimeSignature = new TimeSignature(3, 8);

            TestSimpleTempoMap(TempoMap.Create(expectedTimeDivision, expectedTempo, expectedTimeSignature),
                               expectedTimeDivision,
                               expectedTempo,
                               expectedTimeSignature);
        }

        [Test]
        public void Create_TimeDivision_Tempo()
        {
            var expectedTimeDivision = new TicksPerQuarterNoteTimeDivision(10000);
            var expectedTempo = new Tempo(123456);

            TestSimpleTempoMap(TempoMap.Create(expectedTimeDivision, expectedTempo),
                               expectedTimeDivision,
                               expectedTempo,
                               TimeSignature.Default);
        }

        [Test]
        public void Create_TimeDivision_TimeSignature()
        {
            var expectedTimeDivision = new TicksPerQuarterNoteTimeDivision(10000);
            var expectedTimeSignature = new TimeSignature(3, 8);

            TestSimpleTempoMap(TempoMap.Create(expectedTimeDivision, expectedTimeSignature),
                               expectedTimeDivision,
                               Tempo.Default,
                               expectedTimeSignature);
        }

        [Test]
        public void GetTempoChanges_NoChanges()
        {
            var tempoMap = TempoMap.Default;
            CollectionAssert.IsEmpty(tempoMap.GetTempoChanges(), "There are tempo changes.");
        }

        [Test]
        public void GetTempoChanges_SingleChange_AtStart()
        {
            var microsecondsPerQuarterNote = 100000;

            var tempoMap = TempoMap.Create(new Tempo(microsecondsPerQuarterNote));
            var changes = tempoMap.GetTempoChanges();
            Assert.AreEqual(1, changes.Count(), "Count of tempo changes is invalid.");

            var change = changes.First();
            Assert.AreEqual(0, change.Time, "Time of change is invalid.");
            Assert.AreEqual(new Tempo(microsecondsPerQuarterNote), change.Value, "Tempo of change is invalid.");
        }

        [Test]
        public void GetTempoChanges_SingleChange_AtMiddle()
        {
            var microsecondsPerQuarterNote = 100000;
            var time = 1000;

            using (var tempoMapManager = new TempoMapManager())
            {
                tempoMapManager.SetTempo(time, new Tempo(microsecondsPerQuarterNote));

                var tempoMap = tempoMapManager.TempoMap;
                var changes = tempoMap.GetTempoChanges();
                Assert.AreEqual(1, changes.Count(), "Count of tempo changes is invalid.");

                var change = changes.First();
                Assert.AreEqual(time, change.Time, "Time of change is invalid.");
                Assert.AreEqual(new Tempo(microsecondsPerQuarterNote), change.Value, "Tempo of change is invalid.");
            }
        }

        [Test]
        public void GetTempoChanges_MultipleChanges()
        {
            var microsecondsPerQuarterNote1 = 100000;
            var time1 = 1000;

            var microsecondsPerQuarterNote2 = 700000;
            var time2 = 1500;

            using (var tempoMapManager = new TempoMapManager())
            {
                tempoMapManager.SetTempo(time2, new Tempo(microsecondsPerQuarterNote2));
                tempoMapManager.SetTempo(time1, new Tempo(microsecondsPerQuarterNote1));

                var tempoMap = tempoMapManager.TempoMap;
                var changes = tempoMap.GetTempoChanges();
                Assert.AreEqual(2, changes.Count(), "Count of tempo changes is invalid.");

                var change1 = changes.First();
                Assert.AreEqual(time1, change1.Time, "Time of first change is invalid.");
                Assert.AreEqual(new Tempo(microsecondsPerQuarterNote1), change1.Value, "Tempo of first change is invalid.");

                var change2 = changes.Last();
                Assert.AreEqual(time2, change2.Time, "Time of second change is invalid.");
                Assert.AreEqual(new Tempo(microsecondsPerQuarterNote2), change2.Value, "Tempo of second change is invalid.");
            }
        }

        [Test]
        public void GetTimeSignatureChanges_NoChanges()
        {
            var tempoMap = TempoMap.Default;
            CollectionAssert.IsEmpty(tempoMap.GetTimeSignatureChanges(), "There are time signature changes.");
        }

        [Test]
        public void GetTimeSignatureChanges_SingleChange_AtStart()
        {
            var numerator = 2;
            var denominator = 16;

            var tempoMap = TempoMap.Create(new TimeSignature(numerator, denominator));
            var changes = tempoMap.GetTimeSignatureChanges();
            Assert.AreEqual(1, changes.Count(), "Count of time signature changes is invalid.");

            var change = changes.First();
            Assert.AreEqual(0, change.Time, "Time of change is invalid.");
            Assert.AreEqual(new TimeSignature(numerator, denominator), change.Value, "Time signature of change is invalid.");
        }

        [Test]
        public void GetTimeSignatureChanges_SingleChange_AtMiddle()
        {
            var numerator = 2;
            var denominator = 16;
            var time = 1000;

            using (var tempoMapManager = new TempoMapManager())
            {
                tempoMapManager.SetTimeSignature(time, new TimeSignature(numerator, denominator));

                var tempoMap = tempoMapManager.TempoMap;
                var changes = tempoMap.GetTimeSignatureChanges();
                Assert.AreEqual(1, changes.Count(), "Count of time signature changes is invalid.");

                var change = changes.First();
                Assert.AreEqual(time, change.Time, "Time of change is invalid.");
                Assert.AreEqual(new TimeSignature(numerator, denominator), change.Value, "Time signature of change is invalid.");
            }
        }

        [Test]
        public void GetTimeSignatureChanges_MultipleChanges()
        {
            var numerator1 = 2;
            var denominator1 = 16;
            var time1 = 1000;

            var numerator2 = 3;
            var denominator2 = 8;
            var time2 = 1500;

            using (var tempoMapManager = new TempoMapManager())
            {
                tempoMapManager.SetTimeSignature(time2, new TimeSignature(numerator2, denominator2));
                tempoMapManager.SetTimeSignature(time1, new TimeSignature(numerator1, denominator1));

                var tempoMap = tempoMapManager.TempoMap;
                var changes = tempoMap.GetTimeSignatureChanges();
                Assert.AreEqual(2, changes.Count(), "Count of time signature changes is invalid.");

                var change1 = changes.First();
                Assert.AreEqual(time1, change1.Time, "Time of first change is invalid.");
                Assert.AreEqual(new TimeSignature(numerator1, denominator1), change1.Value, "Time signature of first change is invalid.");

                var change2 = changes.Last();
                Assert.AreEqual(time2, change2.Time, "Time of second change is invalid.");
                Assert.AreEqual(new TimeSignature(numerator2, denominator2), change2.Value, "Time signature of second change is invalid.");
            }
        }

        #endregion

        #region Private methods

        private static void TestSimpleTempoMap(TempoMap tempoMap,
                                               TimeDivision expectedTimeDivision,
                                               Tempo expectedTempo,
                                               TimeSignature expectedTimeSignature)
        {
            Assert.AreEqual(expectedTimeDivision,
                            tempoMap.TimeDivision,
                            "Unexpected time division.");

            Assert.AreEqual(expectedTempo,
                            tempoMap.GetTempoAtTime(new MidiTimeSpan(0)),
                            "Unexpected tempo at the start of tempo map.");
            Assert.AreEqual(expectedTempo,
                            tempoMap.GetTempoAtTime(new MidiTimeSpan(1000)),
                            "Unexpected tempo at the arbitrary time of tempo map.");

            Assert.AreEqual(expectedTimeSignature,
                            tempoMap.GetTimeSignatureAtTime(new MidiTimeSpan(0)),
                            "Unexpected time signature at the start of tempo map.");
            Assert.AreEqual(expectedTimeSignature,
                            tempoMap.GetTimeSignatureAtTime(new MidiTimeSpan(1000)),
                            "Unexpected time signature at the arbitrary time of tempo map.");
        }

        #endregion
    }
}
