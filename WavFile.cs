using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using ZedGraph;

namespace Nazarova
{
    public class WavFile
    {
        public string path;
        //-----WaveHeader-----
        public char[] sGroupID; // "RIFF"

        // 36 + subchunk2Size, или более точно:
        // 4 + (8 + subchunk1Size) + (8 + subchunk2Size)
        // Это оставшийся размер цепочки, начиная с этой позиции
        // Иначе говоря, это размер файла - 8, то есть,
        // исключены поля chunkId и chunkSize
        public uint dwFileLength;
        public char[] sRiffType;  // "WAVE"

        //-----WaveFormatChunk-----
        public char[] sFChunkID;         // "fmt "

        // 16 для формата PCM.
        // Это оставшийся размер подцепочки, начиная с этой позиции
        public uint dwFChunkSize;
        public ushort wFormatTag;       // Для PCM = 1 
        public ushort wChannels;        // Количество каналов. Моно = 1, Стерео = 2 и т.д.
        public uint dwSamplesPerSec;    // Частота дискретизации
        public uint dwAvgBytesPerSec;   // sampleRate * numChannels * bitsPerSample/8

        // numChannels * bitsPerSample/8
        // Количество байт для одного сэмпла, включая все каналы
        public ushort wBlockAlign;      
        public ushort wBitsPerSample;    // Так называемая "глубиная" или точность звучания. 8 бит, 16 бит и т.д.

        //-----WaveDataChunk-----
        public char[] sDChunkID;     // "data"

        // numSamples * numChannels * bitsPerSample/8
        // Количество байт в области данных
        public uint dwDChunkSize;
        public byte dataStartPos;  // audio data start position
        public WavFile()
        {
            path = Environment.CurrentDirectory;
            //-----WaveHeader-----
            dwFileLength = 0;
            sGroupID = "RIFF".ToCharArray();
            sRiffType = "WAVE".ToCharArray();

            //-----WaveFormatChunk-----
            sFChunkID = "fmt ".ToCharArray();
            dwFChunkSize = 16;
            wFormatTag = 1;
            wChannels = 2;
            dwSamplesPerSec = 44100;
            wBitsPerSample = 16;
            wBlockAlign = (ushort)(wChannels * (wBitsPerSample / 8));
            dwAvgBytesPerSec = dwSamplesPerSec * wBlockAlign;

            //-----WaveDataChunk-----
            dataStartPos = 44;
            dwDChunkSize = 0;
            sDChunkID = "data".ToCharArray();
        }
        public WavFile (String path)
        {
            this.path = path;
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            this.sGroupID = r.ReadChars(4);
            this.dwFileLength = r.ReadUInt32();
            this.sRiffType = r.ReadChars(4);
            this.sFChunkID = r.ReadChars(4);
            this.dwFChunkSize = r.ReadUInt32();
            this.wFormatTag = r.ReadUInt16();
            this.wChannels = r.ReadUInt16();
            this.dwSamplesPerSec = r.ReadUInt32();
            this.dwAvgBytesPerSec = r.ReadUInt32();
            this.wBlockAlign = r.ReadUInt16();
            this.wBitsPerSample = r.ReadUInt16();
            this.sDChunkID = r.ReadChars(4);
            this.dwDChunkSize = r.ReadUInt32();
            this.dataStartPos = (byte)r.BaseStream.Position;
            r.Close();
            fsr.Close();
        }

        public void writeMultiplyWav(short A)
        {
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            string path = this.path;
            path = path.Insert(path.Length - 4, "(+)");
            FileStream fsw = null;
            try
            {
                fsw = new FileStream(path, FileMode.CreateNew);
            }
            catch (IOException)
            {
                fsw = new FileStream(path, FileMode.Truncate);
            }
            BinaryWriter w = new BinaryWriter(fsw);
            int pos = 0, len = (int)r.BaseStream.Length; 
            short temp;
            while (pos < len)
            {
                temp = (short)r.ReadInt16();
                if (pos>44) temp *= A;
                w.Write(temp);
                pos += 2;
            }
            r.Close();
            w.Close();
            fsr.Close();
            fsw.Close();
        }
        public void writeCropWav(int start, int end)
        {
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            string path = this.path;
            path = path.Insert(path.Length - 4, "(Crop)");
            FileStream fsw = null;
            try
            {
                fsw = new FileStream(path, FileMode.CreateNew);
            }
            catch (IOException)
            {
                fsw = new FileStream(path, FileMode.Truncate);
            }
            BinaryWriter w = new BinaryWriter(fsw);
            int pos = 0, len = (int)r.BaseStream.Length;
            short temp;
            while (pos < len)
            {
                temp = (short)r.ReadInt16();
                if (pos > 44 && pos>start && pos<end) w.Write(temp);
                pos += 2;
            }
            r.Close();
            w.Close();
            fsr.Close();
            fsw.Close();
        }
        public void playWav()
        {
            SoundPlayer sp = new SoundPlayer();
            sp.SoundLocation = this.path;
            sp.Load();
            sp.Play();
        }

        public PointPairList toList()
        {
            PointPairList list = new PointPairList();
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            
            int pos = 0, len = (int)r.BaseStream.Length;
            short temp;
            int i = 0;
            while (pos < len)
            {
                temp = (short)r.ReadInt16();
                if (pos >= 88) list.Add(i, temp);
                i++;
                pos += 2;
            }
            r.Close();
            fsr.Close();
            return list;
        }

        public PointPairList toList(int start, int end)
        {
            PointPairList list = new PointPairList();
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            
            int pos = 0, len = (int)r.BaseStream.Length;
            short temp;
            int i = 0;
            while (pos < len)
            {
                temp = (short)r.ReadInt16();
                if (i >= start && pos >= 88 && i <= end) list.Add(i, temp);
                else list.Add(i, 0);
                i++;
                pos += 2;
            }
            r.Close();
            fsr.Close();
            return list;
        }
        /*
        public static WavFile listToWav(PointPairList list, String path)
        {
            WavFile wav = new WavFile();
            wav.path = path;
            FileStream fsw = null;
            try
            {
                fsw = new FileStream(path, FileMode.CreateNew);
            }
            catch (IOException)
            {
                fsw = new FileStream(path, FileMode.Truncate);
            }
            BinaryWriter w = new BinaryWriter(fsw);
            
            wav.dwDChunkSize = (uint)list.Count() * wav.wChannels / 8 * wav.wBitsPerSample;
            wav.dwFileLength = wav.dwDChunkSize + 36;

            w.Write(wav.sGroupID);
            w.Write(wav.dwFileLength);
            w.Write(wav.sRiffType);
            w.Write(wav.sFChunkID);
            w.Write(wav.dwFChunkSize);
            w.Write(wav.wFormatTag);
            w.Write(wav.wChannels);
            w.Write(wav.dwSamplesPerSec);
            w.Write(wav.dwAvgBytesPerSec);
            w.Write(wav.wBlockAlign);
            w.Write(wav.wBitsPerSample);
            w.Write(wav.sDChunkID);
            w.Write(wav.dwDChunkSize);

            //int n = (int)((list.Count/2) / wav.wChannels * 8 / wav.wBitsPerSample);
            int n = list.Count();
            for (int i = 0; i < n; i++)
            {
                short tmp = (short)list.ElementAt(i).Y;
                w.Write(tmp);
            }

            w.Close();
            fsw.Close();
            return wav;
        }*/

        public void listToWav(PointPairList list, String name)
        {
            FileStream fsr = new FileStream(this.path, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fsr);
            string path = this.path;
            path = path.Insert(path.Length - 4, name);
            FileStream fsw = null;
            try
            {
                fsw = new FileStream(path, FileMode.CreateNew);
            }
            catch (IOException)
            {
                fsw = new FileStream(path, FileMode.Truncate);
            }
            BinaryWriter w = new BinaryWriter(fsw);
            int pos = 0, len = 88;
            short temp;
            while (pos<len)
            {
                temp = (short)r.ReadInt16();
                w.Write(temp);
                pos += 2;
            }
            for (int i=0; i<list.Count; i++)
            {
                temp = (short)list.ElementAt(i).Y;
                w.Write(temp);
            }
            w.Close();
            fsw.Close();
        }

    }
}
