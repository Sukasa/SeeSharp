using System;

namespace SeeSharp
{

    internal struct PaletteToken
    {
        public enum TokenType
        {
            Command,
            Variable,
            Constant,
            Newline
        }
        public PaletteToken(int NewLine, TokenType NewType)
        {
            Type = NewType;
            TokenData = "";
            Line = NewLine;
        }
        public PaletteToken(int NewLine, TokenType NewType, String NewData)
        {
            Type = NewType;
            TokenData = NewData;
            Line = NewLine;
        }

        public TokenType Type;
        public String TokenData;
        public int Line;
    }

}