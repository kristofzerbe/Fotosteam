using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ExifLib
{
    /// <summary>
    /// Credits to http://www.codeproject.com/script/Membership/View.aspx?mid=1636261
    /// </summary>
    public class IPTCReader
    {
        private string JPEGContentBuffer;
        private string PS3SectionContentBuffer;
        private Hashtable PS3Tags = new Hashtable();
        public Hashtable PS3TagContents = new Hashtable();
        public string Ps3Sectionheadlinetag;
        public const string Ps3Sectionheader = "PS3SectionHeader";
        public const string Ps3Sectionidtag = "PS3SectionIDTag";
        public const string Ps3Sectionobjnametag = "PS3SectionObjNameTag";
        public const string Ps3Sectioncaptiontag = "PS3SectionCaptionTag";

        public IPTCReader(string FileName)
        {
            JPEGContentBuffer = LoadJPEG(FileName);
            Initialize();
        }

        public IPTCReader(Stream stream, bool keepStreamOpen = false)
        {
            JPEGContentBuffer = LoadJPEG(stream, keepStreamOpen);
            Initialize();
        }

        
        private void Initialize()
        { 
            PS3Tags.Add(Ps3Sectionheader, "\u00FF\u00ED");
            PS3Tags.Add(Ps3Sectionidtag, "Photoshop 3.0\u0000");
            PS3Tags.Add(Ps3Sectionobjnametag, "\u001C\u0002\u0005");
            Ps3Sectionheadlinetag = "PS3SectionHeadlineTag";
            PS3Tags.Add(Ps3Sectionheadlinetag, "\u001C\u0002\u0069");
            PS3Tags.Add(Ps3Sectioncaptiontag, "\u001C\u0002\u0078");

            PS3SectionContentBuffer = ExtractPS3ContentSection(PS3Tags[Ps3Sectionheader],PS3Tags[Ps3Sectionidtag]);

            PS3TagContents.Add(Ps3Sectionobjnametag, ExtractTag(PS3Tags[Ps3Sectionobjnametag].ToString()));
            PS3TagContents.Add(Ps3Sectionheadlinetag, ExtractTag(PS3Tags[Ps3Sectionheadlinetag].ToString()));
            PS3TagContents.Add(Ps3Sectioncaptiontag, ExtractTag(PS3Tags[Ps3Sectioncaptiontag].ToString()));
        }


        public string GetTag(string tag)
        {
            return PS3TagContents.ContainsKey(tag) ? PS3TagContents[tag].ToString() : string.Empty;
        }

        public void DisplayAllTags()
        {
            foreach (DictionaryEntry de in PS3TagContents)
            {
                Console.WriteLine("Key = {0}, Value = {1}", de.Key, de.Value);
            }
        }


        private string ExtractTag(string currTagSought)
        {

            int pos = PS3SectionContentBuffer.IndexOf(currTagSought);
            if (pos > 0)
            {
                pos += 3;
                int BlockSize = (int)(PS3SectionContentBuffer[pos] * 256) + (int)(PS3SectionContentBuffer[pos + 1]);

                pos += 2;
                byte[] tagHeaderContent = new byte[BlockSize];
                System.Buffer.BlockCopy(Encoding.Default.GetBytes(PS3SectionContentBuffer), pos, tagHeaderContent, 0, BlockSize);
                return Encoding.Default.GetString(tagHeaderContent);
            }
            else
                return currTagSought + " is not available!";
        }


        private string LoadJPEG(string FileName)
        {
            FileStream fs = new FileStream(FileName,
                FileMode.Open,
                FileAccess.Read);

            byte[] RAWdata = new byte[fs.Length];
            fs.Read(RAWdata, 0, RAWdata.Length);
            fs.Close();

            return Encoding.Default.GetString(RAWdata, 0, RAWdata.Length);
        }
        private string LoadJPEG(Stream fileStream,bool keepStreamOpen = false)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            byte[] RAWdata = new byte[fileStream.Length];
            fileStream.Read(RAWdata, 0, RAWdata.Length);

            if(!keepStreamOpen)
                fileStream.Close();

            return Encoding.Default.GetString(RAWdata, 0, RAWdata.Length);
        }


        private string ExtractPS3ContentSection(object headerStr, object tagStr)
        {
            Regex rex = new Regex("(?:" + headerStr + ")(?<blocklen>.*)(?:" + tagStr + ")", RegexOptions.None);
            Match m = rex.Match(JPEGContentBuffer);

            if (m.Success)
            {
                byte[] BlockSizeCode = Encoding.Default.GetBytes(m.Groups["blocklen"].Value);
                int BlockSize = (int)(BlockSizeCode[0]) * 256 + (int)(BlockSizeCode[1]);

                byte[] CurrBlock = new byte[BlockSize];
                System.Buffer.BlockCopy(Encoding.Default.GetBytes(JPEGContentBuffer),
                    m.Groups["blocklen"].Index + m.Groups["blocklen"].Length,
                    CurrBlock,
                    0,
                    BlockSize);

                return Encoding.Default.GetString(CurrBlock, 0, CurrBlock.Length);
            }
            else
                return "Block Length can't be found!";
        }
    }
}
