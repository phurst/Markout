using System.Text.RegularExpressions;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Tags;

namespace Markout.Input.Interfaces {

    public interface ITagFactory {

        TextAttributeTypeEnum TextAttributeType { get; set; }
        Tag CreateTagFromMatch(Match match);
    }
}