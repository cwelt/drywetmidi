using System;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Tests.Common;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Tests.Core
{
    [TestFixture]
    public sealed class MidiChunkReaderTests
    {
        #region Properties

        public TestContext TestContext { get; set; }

        #endregion

        #region Set up

        [SetUp]
        public void SetupTest()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        #endregion

        #region Test methods

        [Test]
        public void ReadChunk()
        {
            ReadChunkData(14, (reader, settings) =>
            {
                var chunk = MidiChunkReader.ReadChunk(reader, settings);
                Assert.IsNotNull(chunk, "Chunk is null.");
                Assert.IsInstanceOf(typeof(TrackChunk), chunk, "Chunk is not of TrackChunk type.");
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ReadTrackChunk(bool readChunkId)
        {
            long position = 14 + (readChunkId ? 0 : 4);
            
            ReadChunkData(position, (reader, settings) =>
            {
                var chunk = MidiChunkReader.ReadTrackChunk(reader, settings, readChunkId);
                Assert.IsNotNull(chunk, "First track chunk is null.");
                position = reader.Position;
            });

            ReadChunkData(position + (readChunkId ? 0 : 4), (reader, settings) =>
            {
                var chunk = MidiChunkReader.ReadTrackChunk(reader, settings, readChunkId);
                Assert.IsNotNull(chunk, "Second track chunk is null.");
            });
        }

        #endregion

        #region Private methods

        private void ReadChunkData(long readerInitialPosition, Action<MidiReader, ReadingSettings> read)
        {
            var readingSettings = new ReadingSettings();

            using (var fileStream = File.OpenRead(TestFilesProvider.GetMiscFile_14000events()))
            using (var midiReader = new MidiReader(fileStream, new ReaderSettings()))
            {
                midiReader.Position = readerInitialPosition;
                read(midiReader, readingSettings);
            }
        }

        #endregion
    }
}
