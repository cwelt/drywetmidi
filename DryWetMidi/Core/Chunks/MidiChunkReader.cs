using System;
using Melanchall.DryWetMidi.Common;

namespace Melanchall.DryWetMidi.Core
{
    public static class MidiChunkReader
    {
        #region Methods

        public static MidiChunk ReadChunk(MidiReader reader, ReadingSettings settings)
        {
            ThrowIfArgument.IsNull(nameof(reader), reader);
            ThrowIfArgument.IsNull(nameof(settings), settings);

            var chunkId = ReadChunkId(reader, settings);
            if (string.IsNullOrEmpty(chunkId))
                return null;

            var chunk = CreateChunk(chunkId, reader, settings);
            if (chunk == null)
                return null;

            chunk.Read(reader, settings);
            return chunk;
        }

        public static TrackChunk ReadTrackChunk(MidiReader reader, ReadingSettings settings, bool readChunkId)
        {
            ThrowIfArgument.IsNull(nameof(reader), reader);
            ThrowIfArgument.IsNull(nameof(settings), settings);

            if (readChunkId)
            {
                var chunkId = ReadChunkId(reader, settings);
                if (chunkId != TrackChunk.Id)
                    throw new InvalidOperationException($"Chunk ID isn't {TrackChunk.Id}.");
            }

            var trackChunk = new TrackChunk();
            trackChunk.Read(reader, settings);
            return trackChunk;
        }

        internal static MidiChunk CreateChunk(string chunkId, MidiReader reader, ReadingSettings settings)
        {
            MidiChunk chunk = null;

            switch (chunkId)
            {
                case HeaderChunk.Id:
                    chunk = new HeaderChunk();
                    break;
                case TrackChunk.Id:
                    chunk = new TrackChunk();
                    break;
                default:
                    chunk = TryCreateChunk(chunkId, settings.CustomChunkTypes);
                    break;
            }

            if (chunk == null)
            {
                switch (settings.UnknownChunkIdPolicy)
                {
                    case UnknownChunkIdPolicy.ReadAsUnknownChunk:
                        chunk = new UnknownChunk(chunkId);
                        break;

                    case UnknownChunkIdPolicy.Skip:
                        var size = reader.ReadDword();
                        reader.Position += size;
                        return null;

                    case UnknownChunkIdPolicy.Abort:
                        throw new UnknownChunkException(chunkId);
                }
            }

            return chunk;
        }

        internal static string ReadChunkId(MidiReader reader, ReadingSettings settings)
        {
            var chunkId = reader.ReadString(MidiChunk.IdLength);
            if (chunkId.Length < MidiChunk.IdLength)
            {
                switch (settings.NotEnoughBytesPolicy)
                {
                    case NotEnoughBytesPolicy.Abort:
                        throw new NotEnoughBytesException(
                            "Chunk ID cannot be read since the reader's underlying stream doesn't have enough bytes.",
                            MidiChunk.IdLength,
                            chunkId.Length);
                    case NotEnoughBytesPolicy.Ignore:
                        return null;
                }
            }

            return chunkId;
        }

        private static MidiChunk TryCreateChunk(string chunkId, ChunkTypesCollection chunksTypes)
        {
            Type type = null;
            return chunksTypes?.TryGetType(chunkId, out type) == true && IsChunkType(type)
                ? (MidiChunk)Activator.CreateInstance(type)
                : null;
        }

        private static bool IsChunkType(Type type)
        {
            return type != null &&
                   type.IsSubclassOf(typeof(MidiChunk)) &&
                   type.GetConstructor(Type.EmptyTypes) != null;
        }

        #endregion
    }
}
