using System;
using System.Text.RegularExpressions;
using Markout.Common.DataModel.Enumerations;
using Markout.Input.Interfaces;

namespace Markout.Input.Tags.TagFactories {

    public abstract class BaseTagFactory : ITagFactory {

        public TextAttributeTypeEnum TextAttributeType { get; set; }

        public abstract Tag CreateTagFromMatch(Match match);
    }
}