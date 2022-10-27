using UELib;

namespace UEExplorer.UI
{
    public static class StreamLocationFactory
    {
        public static StreamLocation Create(object obj)
        {
            switch (obj)
            {
                case IBinaryData binaryObj:
                    return Create(binaryObj);
            }

            return new StreamLocation(obj, -1);
        }
        
        public static StreamLocation Create(IBinaryData obj)
        {
            return new StreamLocation(obj, obj.GetBufferPosition());
        }
    }
}
