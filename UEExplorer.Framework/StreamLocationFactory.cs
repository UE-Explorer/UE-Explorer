using UELib;
using UELib.Core;

namespace UEExplorer.Framework
{
    public static class StreamLocationFactory
    {
        public static StreamLocation Create(object obj)
        {
            switch (obj)
            {
                case IBinaryData binaryObj:
                    return Create(binaryObj);

                case UStruct.UByteCodeDecompiler.Token token:
                    return Create(token);

                case UStruct.UByteCodeDecompiler script:
                    return Create(script);
            }

            return new StreamLocation(obj, -1);
        }

        public static StreamLocation Create(IBinaryData obj) =>
            new StreamLocation(obj, obj.GetBufferPosition(), obj.GetBufferSize());

        public static StreamLocation Create(UStruct.UByteCodeDecompiler.Token token) =>
            new StreamLocation(token, token.StoragePosition, token.StoragePosition);

        public static StreamLocation Create(UStruct.UByteCodeDecompiler script) =>
            new StreamLocation(script, script.Container.ScriptOffset, script.Container.ScriptSize);
    }
}
