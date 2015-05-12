namespace Markout.Input.Parser {

    public abstract class ParserBase {

        protected const string RegexHead = @"(?<=^|[^\{])\{\s*(?<tag>";
        protected const string RegexTail = @")\s*(?(:):\s*(?<qualifier>[^\}]*)|)\}";
       
    }
}