using System;
using System.Collections.Generic;
using System.Text;

namespace FwtHelper
{
    public class Column
    {
        public int Position { get; private set; }
        public int Length { get; private set; }
        public char PaddingChar { get; private set; }
        public Padding Padding { get; private set; }
        public string Value { get; set; }

        public Column(int position, int length
            , char paddingChar = ' '
            , Padding padding = Padding.Left)
        {
            Position = position;
            Length = length;
            PaddingChar = paddingChar;
            Padding = padding;
        }
    }

    public enum Padding
    {
        Left,
        Right
    }
}
