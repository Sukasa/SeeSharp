using System;

namespace SeeSharp.Palette
{

    internal struct PaletteToken
    {
        internal enum TokenType
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