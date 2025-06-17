using System.Text;

public class AxmlParser
{
    private BinaryReader reader;
    private List<string> stringPool = new List<string>();
    private StringBuilder xml = new StringBuilder();

    public AxmlParser(Stream input)
    {
        reader = new BinaryReader(input);
    }

    public string Parse()
    {
        // Read the header
        uint magicNumber = reader.ReadUInt32();
        if (magicNumber != 0x00080003)
            throw new Exception("Invalid AXML file");

        reader.ReadUInt32(); // file size

        // Parse chunks
        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            int chunkType = reader.ReadInt16();
            int headerSize = reader.ReadInt16();
            int chunkSize = reader.ReadInt32();

            long chunkEnd = reader.BaseStream.Position - 8 + chunkSize;

            switch (chunkType)
            {
                case 0x0001: // STRING_POOL
                    ParseStringPool(chunkEnd);
                    break;
                case 0x0102: // XML_START_NAMESPACE
                case 0x0103: // XML_END_NAMESPACE
                    reader.BaseStream.Position = chunkEnd;
                    break;
                case 0x0100: // XML_START_ELEMENT
                    ParseStartElement();
                    break;
                case 0x0101: // XML_END_ELEMENT
                    ParseEndElement();
                    break;
                case 0x0104: // XML_CDATA
                    reader.BaseStream.Position = chunkEnd;
                    break;
                default:
                    reader.BaseStream.Position = chunkEnd;
                    break;
            }
        }

        return xml.ToString();
    }

    private void ParseStringPool(long chunkEnd)
    {
        int stringCount = reader.ReadInt32();
        int styleCount = reader.ReadInt32();
        int flags = reader.ReadInt32();
        int stringsStart = reader.ReadInt32();
        int stylesStart = reader.ReadInt32();

        int[] stringOffsets = new int[stringCount];
        for (int i = 0; i < stringCount; i++)
            stringOffsets[i] = reader.ReadInt32();

        long pos = reader.BaseStream.Position;
        for (int i = 0; i < stringCount; i++)
        {
            reader.BaseStream.Position = pos + stringsStart + stringOffsets[i];
            int len = reader.ReadUInt16();
            byte[] chars = reader.ReadBytes(len * 2);
            string str = Encoding.Unicode.GetString(chars);
            stringPool.Add(str);
        }

        reader.BaseStream.Position = chunkEnd;
    }

    private void ParseStartElement()
    {
        reader.ReadUInt32(); // lineNumber
        reader.ReadUInt32(); // comment
        int nsIndex = reader.ReadInt32();
        int nameIndex = reader.ReadInt32();
        reader.ReadUInt16(); // flags
        int attributeCount = reader.ReadUInt16();
        reader.ReadUInt16(); // classAttribute
        reader.ReadUInt16(); // padding

        string tagName = stringPool[nameIndex];
        xml.Append($"<{tagName}");

        for (int i = 0; i < attributeCount; i++)
        {
            int attrNs = reader.ReadInt32();
            int attrName = reader.ReadInt32();
            int attrRawValue = reader.ReadInt32();
            int attrType = reader.ReadInt32() >> 24;
            int attrData = reader.ReadInt32();

            string attrNameStr = stringPool[attrName];
            string attrValue = attrRawValue != -1
                ? stringPool[attrRawValue]
                : attrType == 3 ? stringPool[attrData] : attrData.ToString();

            xml.Append($" {attrNameStr}=\"{attrValue}\"");
        }

        xml.Append(">");
    }

    private void ParseEndElement()
    {
        reader.ReadUInt32(); // lineNumber
        reader.ReadUInt32(); // comment
        int nsIndex = reader.ReadInt32();
        int nameIndex = reader.ReadInt32();

        string tagName = stringPool[nameIndex];
        xml.Append($"</{tagName}>");
    }
}
