# Markout

Is a simple text markup language that is designed to meet the following goals:

* Simple, consistent tag syntax.
* Support tag synonyms.
* Be extensible.
* Separate input parsing from output rendering.
* Support rendering to WPF TextBlock Inlines.
* Support macros.

# Tag Syntax

All Markout tags are of the form {tag}. This makes it very easy to identify tags in the input text, which makes the parser very simple. An example is the bold tag: {b}. 

Some tags require information in addition to a basic tag name. These are of the form {tagname:qualifier1:qualifier2:…}. An example is the color tag: {color:red}.

Repeating a tag turns it off. For example plain{b}bold{b}plain. Or: BLACK{color:red}RED{color}BLACK.

In this first version I provide a very limited set of tags, but expect to add more and the need becomes apparent.

# Tag Synonyms

I generally prefer very short tags, such a {b} for bold, or {i} for italic. Markdown supports synonyms for tags, so for instance {c:…},{color:…} and {colour:…} can be used interchangeably.

# Extensibility

The Markout tag syntax is regular and very simple. That makes the parser very simple. A glance at the TagParser class should give an idea of how easy this makes it to extend Markout to incorporate new tags.

# Flexible Output Mechanism

Markout converts the input text into a set of attributed TextRun instances. This intermediate form makes it quite simple to add additional renderers. I provide a renderer to create sets of Inlines that can be applied to a WPF TextBlock. An HTML renderer would be an obvious addition.

# Support for Macros

Markout supports the notion of macros. A macro is a named tag that is expanded into other tags when rendered. For example you might define a tag {code} to be represented as {font:Courier:10}{color:DarkGrey}. 

Macros are  supplied as a dictionary to the MarkoutParser, which expands them into the input text.

# Code Structure

The Markout code contains the following main elements:

## TagParser

The TagParser class identifies Markout tags in the input text and creates a set of Tag objects. The Tag object keeps track of its location in the text and has an attribute that specifies the type of tag it represents and any collateral information (such as the color name in a color tag).

## MarkoutParser

The MarkoutParser class uses the TagParser to locate the tags in the input text and then chops the text up into a set of TextRun instances, combining attributes as necessary. 

For instance, the input text plain{b}{i}bolditalic would be output as two TextRun instances: the first would have text “plain” and no attributes; the second would have text “bolditalic” and bold an italic attributes.

The TextRun instances emitted by the MarkoutParser class are sent to another class to render them into the desired output form. 

You can supply a dictionary of macros to the MarkoutParser if desired.

## MarkoutRenderer

The MarkoutRenderer class (in the Markout.Output.Inlines namespace) takes a set of TextRun instances produced by the MarkoutParser and renders them into a set of Inlines which may then be assigned to the Inlines property of a WPF TextBlock.

The anchor tag is rendered as a hypertext element in the Inlines. You can provide a set of named actions to be associated, with anchor tags, and called when the anchor tag is clicked.

## Controls

The Inlines.TextBlock control is a subclass of the WPF TextBlock that exposes a TextInlines property that can be bound to an observable collection of Inline elements in a View Model class. See the Output.Inlines.TestApp application for an example of this in action.

## Tests

Unit tests are supplied for most input components. 

A WPF application (Output.Inlines.TestApp) is provided to visualize the effect of various markups when sent to a WPF TextBlock. 

## Sample Code

The Markout.Output.Inlines.Sample project contains a minimal WPF application that illustrates how Markout can be used.

# NuGet

Markout is available as a set of NuGet packages. Just search for Markout on https://www.nuget.org/

# Markout Tag Syntax in Detail

Tag | Qualifiers | Descriptions | Example |
--- | ---------- | ------------ | ------- |
b || Bold | {b} |
i || Italic | {i} |
u || Underline | {u} |
f, font | Font Name<br> Font Size<br> Decorations (b, I, u) | Font | {f:Courier:12:bu}<br> {f:Courier:12}<br> {font:Courier} |
c, color, colour | Color Name | Color | {c:DarkGreen} |
a, anchor | Uri<br> Action Name | Hyperlink | {a:http://google.com:LaunchUrl}Google{a}<br> Renders as: Google |
{{ || Escape |A tag looks like {{b}<br> Renders as: A tag looks like {b} |


# Notes

## f, font

The font name should be one recognized by the renderer used. So the Output.Inlines.MarkoutRenderer class expects WPF FontFamily names. 

## c, color
The color name should be one recognized by the renderer used. So the Output.Inlines.MarkoutRenderer class expects WPF KnownColor names, or a hexadecimal number that can be passed to the Color.FromArgb method.

## a, anchor
This is currently the only closed tag supported, so it will render as a hyperlink tag containing the text between the first {a} tag and the following tag.
