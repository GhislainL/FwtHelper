# FwtParser
FwtParser is a .NET Core library for reading and writing FWT files. 

## WTF is FWT ?
FWT stands for Fixed Width Text. It is a kind of flat-file database
See [flat-file database (wikipedia EN)](https://en.wikipedia.org/wiki/Flat-file_database)
The data arrangement consists of a series of columns and rows organized into a tabular format.

## Implementation

Target : .Net Standard 2.0

Content:
* FwtParser : library
* FwtParser.Demo : demo code

Choices: 

## Getting started

### Define a record

A record is a class that implements some Column properties.

A column is defined by 4 properties :
* position (mandatory)
* length (mandatory)
* padding character (optional, default is string.Empty)
* padding (optional, default is Padding.Left)

![Fwt file example](https://github.com/GhislainL/FwtHelper/blob/master/FwtFile.PNG)

In the following example, we define a Record that has 2 columns:
* columnA begins at position 5 on 12 characters with a left padding of string.empty
* columnB begins at position 20 on 10 characters with a right padding of zeros (0)

```csharp
public class Record
{
    public Column columnA = new Column(5, 12);
    public Column columnB = new Column(20, 10, '0', Padding.Right);

    public Record() { }
}
```

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
