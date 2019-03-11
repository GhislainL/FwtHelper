# FwtParser
FwtParser is a .NET Core library for reading and writing FWT files. 

## WTF is FWT ?
FWT stands for Fixed Width Text. It is a kind of flat-file database
See [flat-file database (wikipedia EN)](https://en.wikipedia.org/wiki/Flat-file_database)
The data arrangement consists of a series of columns and rows organized into a tabular format.

## Implementation

Target : .Net Standard 2.0

* FwtParser : library
* FwtParser.Demo : demo code

Choices 

## Getting started

### Read Data

```csharp
using (StreamReader streamReader = new StreamReader(inFile))
{
    // init reader
    FwtReader<MyRecord> myRecordParser = new FwtReader<MyRecord>(streamReader);
    
    // read a record  
    myRecordParser.Read();
    // get the record
    MyRecord res = myRecordParser.GetRecord();

    // Reader - SECOND USE CASE (looping)
    while (myRecordParser.Read())
    {
        MyRecord rec = myRecordParser.GetRecord();
        Console.WriteLine($"{rec.Column1.Value} - {rec.Column2.Value} - {rec.Column6.Value}");
    }
}
```
### Write Data

```csharp
using (StreamWriter streamWriter = new StreamWriter(outFile))
{
    // init writer
    FwtWriter<ExtractRecord> fwtWriter = new FwtWriter<ExtractRecord>(streamWriter);
    
    // writer - create record and set the Value property
    ExtractRecord extractRecord = new ExtractRecord();
    extractRecord.column1.Value = "some value";
    extractRecord.column2.Value = "another value";

    // write the record 
    fwtWriter.WriteRecord(extractRecord);
}
```

## TODO :
