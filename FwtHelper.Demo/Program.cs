using System;
using System.IO;

namespace FwtHelper.Demo
{
    class Program
    {
        private const string outFile = @"outFwt.txt";
        private const string inFile = @"sample.txt";

        public class InRecord
        {
            public Column Column1 = new Column(13, 8);
            public Column Column4 = new Column(261, 2);
            public Column Column6 = new Column(394, 18, '0');
            public Column Column3 = new Column(121, 3);
            public Column Column5 = new Column(374, 18, '0');
            public Column Column2 = new Column(94, 8);

            public InRecord() { }
        }

        public class OutRecord
        {
            public Column column1 = new Column(13, 12, '0');
            public Column column2 = new Column(94, 10, '-', Padding.Right);

            public OutRecord() { }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("begin");

                using (StreamWriter streamWriter = new StreamWriter(outFile))
                using (StreamReader streamReader = new StreamReader(inFile))
                {
                    // init reader
                    FwtReader<InRecord> myRecordParser = new FwtReader<InRecord>(streamReader);


                    // Reader - FIRST USE CASE
                    // read a record  
                    myRecordParser.Read();
                    // get the record
                    InRecord res = myRecordParser.GetRecord();


                    // init writer
                    FwtWriter<OutRecord> fwtWriter = new FwtWriter<OutRecord>(streamWriter);

                    // Reader - SECOND USE CASE (looping)
                    while (myRecordParser.Read())
                    {
                        InRecord rec = myRecordParser.GetRecord();
                        Console.WriteLine($"{rec.Column1.Value} - {rec.Column2.Value} - {rec.Column6.Value}");

                        // writer - create record and set the Value property
                        OutRecord extractRecord = new OutRecord();
                        extractRecord.column1.Value = rec.Column1.Value;
                        extractRecord.column2.Value = rec.Column2.Value;

                        // write the record 
                        fwtWriter.WriteRecord(extractRecord);
                    }

                    Console.WriteLine("end");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
