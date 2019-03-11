using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FwtHelper
{
    public class FwtWriter<T> : IDisposable
            where T : new()
    {
        private readonly TextWriter TextWriter;
        private readonly bool DisposeWriter;

        private string CurrentLine { get; set; }

        public FwtWriter(TextWriter textWriter, bool disposeWriter = true)
        {
            TextWriter = textWriter;
            DisposeWriter = disposeWriter;
        }

        private SortedList<int, Column> Convert(T record)
        {
            SortedList<int, Column> keyValuePairs = new SortedList<int, Column>();
            IList<FieldInfo> props = new List<FieldInfo>(record.GetType().GetRuntimeFields());
            int cnt = props.Count;
            for (int i = 0; i < cnt; i++)
            {
                FieldInfo prop = props[i];
                object propValue = prop.GetValue(record);

                if (propValue is Column)
                {
                    Column c = propValue as Column;
                    keyValuePairs.Add(c.Position, c);
                }
            }

            return keyValuePairs;
        }

        private string FormatColumn(Column column)
        {

            string res = string.Empty;
            if (column.Value.Length > column.Length)
            {
                res = column.Value.Substring(0, column.Length);
            }
            else
            {
                res = column.Value;

                switch (column.Padding)
                {
                    case Padding.Left:
                        res = res.PadLeft(column.Length, column.PaddingChar);
                        break;
                    case Padding.Right:
                        res = res.PadRight(column.Length, column.PaddingChar);
                        break;
                }
            }

            return res;
        }

        public void WriteRecords(List<T> records)
        {
            int nbRecord = records.Count;
            for (int i = 0; i < nbRecord; i++)
            {
                WriteRecord(records[i]);
            }
        }

        public void WriteRecord(T record)
        {
            SortedList<int, Column> keyValuePairs = Convert(record);

            StringBuilder stringBuider = new StringBuilder();
            int currentposition = 0;
            foreach (var item in keyValuePairs)
            {
                // on ne traite pas les retours en arriere ou les champs qui se chevauchent
                if (currentposition > item.Value.Position)
                {
                    continue;
                }

                if (currentposition < item.Value.Position)
                {
                    int diff = item.Value.Position - currentposition + 1;
                    stringBuider.Append(new string(' ', diff));
                }

                stringBuider.Append(FormatColumn(item.Value));
                currentposition = item.Value.Position + item.Value.Length;
            }

            TextWriter.WriteLine(stringBuider.ToString());
        }

        public void Dispose()
        {
            if (DisposeWriter && TextWriter != null)
            {
                TextWriter.Close();
                TextWriter.Dispose();
            }
        }
    }
}
