using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FwtHelper
{
    public class FwtReader<T> : IDisposable
            where T : new()
    {
        private bool DisposeReader { set; get; }
        private TextReader TextReader;
        private string CurrentLine { get; set; }
        private T CurrentRecord;

        public void Dispose()
        {
            TextReader.Dispose();
        }

        public FwtReader(TextReader textReader, bool disposeReader = true)
        {
            TextReader = textReader;
            DisposeReader = disposeReader;
        }

        // TODO : mettre dans ColumnPrivate.Value.set() ?
        private string CleanValue(string data, char cccc)
        {
            return data.Trim(cccc);
        }

        private T Parse(string line)
        {
            T cols = new T();
            int lineLength = line.Length;

            Type myType = cols.GetType();
            IList<FieldInfo> props = new List<FieldInfo>(myType.GetRuntimeFields());

            int cnt = props.Count;
            for (int i = 0; i < cnt; i++)
            {
                FieldInfo prop = props[i];
                object propValue = prop.GetValue(cols);
                if (propValue is Column)
                {
                    Column c = propValue as Column;
                    // position is 1-based
                    // substring is 0-based

                    int itemPosition = c.Position - 1;

                    if (itemPosition > lineLength)
                    {
                        // error
                        break;
                    }
                    else if (itemPosition + c.Length > lineLength)
                    {
                        string s = line.Substring(itemPosition, lineLength - itemPosition);
                        c.Value = CleanValue(s, c.PaddingChar);
                        // warning
                    }
                    else
                    {
                        string s = line.Substring(itemPosition, c.Length);
                        c.Value = CleanValue(s, c.PaddingChar);
                    }
                }
            }

            return cols;
        }

        public bool Read()
        {
            CurrentLine = TextReader.ReadLine();

            return !string.IsNullOrEmpty(CurrentLine);
        }

        public T GetRecord()
        {
            CurrentRecord = Parse(CurrentLine);
            return CurrentRecord;
        }

        public IEnumerable<T> GetRecords()
        {
            string line = string.Empty;

            while ((line = TextReader.ReadLine()) != null)
            {
                yield return Parse(line);
            }
        }
    }
}
