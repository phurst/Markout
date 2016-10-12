# Markout

Is a simple text markup language that is designed to meet the following goals:

* Simple, consistent tag syntax.
* Support tag synonyms.
* Be extensible.
* Separate input parsing from output rendering.
* Support rendering to WPF TextBlock Inlines.
* Support macros.

# Tag Syntax

All Markout tags are of the form `{tag}`. This makes it easy to identify tags in the input text, so the parser can be rather simple. For example the bold tag is: `{b}`. 

Some tags require information in addition to a basic tag name. These are of the form `{tagname:qualifier1:qualifier2:…}`. For example the color tag: {color:red}.

Repeating a tag turns it off. For example `plain{b}bold{b}plain`. Or: `BLACK{color:red}RED{color}BLACK`.

In this first version I provide a limited set of tags, but expect to add more as the need becomes apparent.

# Tag Synonyms

I generally prefer very short tags, such as `{b}` for bold, or `{i}` for italic. Markdown supports synonyms for tags, so for instance `{c:red}`, `{color:red}`, and `{colour:red}` can be used interchangeably.

# Extensibility

Because tags are consistent and the parser simple, its pretty easy to add new tags.

# Flexible Output Mechanism

Markout converts the input text into a set of attributed `TextRun` instances. 
This intermediate form makes it quite simple to add new renderers. 
I provide a renderer to turn a set of `TextRun`s into a set of Inlines that can be applied to a WPF TextBlock. 
An HTML renderer would be an obvious potential addition.

# Support for Macros

Markout supports the notion of macros. A macro is a named tag that is expanded into other tags when rendered. 
For example you might define a macro `{code}` to be equal to `{font:Courier:10}{color:Grey}`. 
You could then use the macro thusly: `{code}int i = 0;{code}` to show the text in Courier / Grey.

Macros are  supplied as a dictionary to the MarkoutParser, which expands them into the input text.

I also supply a `MacroDefinitionParser` that can read macro definitions from a file or string.

# Support for "External" Tags (added in release 1.3)

An "external" tag is one that is resolved to text by code you provide to the parser.

By using "external" tags you can insert text (such as the current date) that can only be evaluated at display time. 
Your tag resolver code is responsible for evaluating the string value that will replace the tag in the text.

"External" tags are resolved and substitiuted into the text before any other tags are evaluated.

An "external" tag is identified by `{x:...}` or `{external:...}` and has a Name and an optional Parameter:
like this: `{x:name}` or `{x:name:parameter}`.

You provide tag resolvers to the parser by populating its `ExternalTagResolvers` dictionary. The dictionary key is the tag name
and the value is an instance of the `IExternalTagResolver` interface.

Should you misname an external tag or fail to provide a resolver for a tag name, the
tag is replaced with an empty string. Should your tag resolver code throw an exception, an ugly message
will be inserted in the text in place of the tag, so you should avoid throwing by
trapping exceptions (and log the problem via whatevere logging mechanism your program uses).

See the `ExternalTagsMarkoutParserTests` class for examples of "texternal" tags at work.

# Code Structure

The Markout code contains the following main elements:

## MarkoutParser

The `MarkoutParser` class (in the Markout.Input namespace) uses the `TagParser` to locate the tags in the input text and 
then chops the text up into a set of `TextRun` instances, combining attributes as necessary. 
The output of the `MarkoutParser` is a set of `TextRun` instances.

For example, the input text `plain{b}{i}bolditalic` would be parsed and output as two TextRun instances: 
* The first TextRun would have text “plain” and no attributes.
* The second TextRun would have text “bolditalic” and bold an italic attributes.

The TextRun instances emitted by the `MarkoutParser` class are sent to another class to render them into the desired output form. 

If you provide the `MarkoutParser` with external tag resolvers (via the `ExternalTagResolvers` dictionary), they will be called to resolve any
external tags in your input text, before any other parsing is done.

If you provide a set of macros to the `MarkoutParser` it will locate and expand them in the input text before parsing it.

## MarkoutRenderer

The `MarkoutRenderer` class (in the Markout.Output.Inlines namespace) takes as input a set of TextRun instances 
produced by the `MarkoutParser` and renders them into a set of `Inline`s which may then be assigned to the `Inlines` 
property of a WPF `TextBlock`.

The anchor tag is rendered as a `Hypertext` element in the Inlines. 
You can provide the `MarkoutRenderer` with a set of named `Action`s to be associated, with anchor tags and
the appropriate `Action` will be called when the `Hypertext` element is clicked.

## Controls

The `InlinesTextBlock` control is a subclass of the WPF `TextBlock` that exposes a `TextInlines` property.
You can bind the `TextInlines` property to an observable collection of Inline elements in a View Model class. 

See the Output.Inlines.Sample application for an example of this in action.

## Tests

Unit tests are supplied for most input components. 

A WPF application (Output.Inlines.TestApp) is provided to visualize the effect of various markups when sent to a WPF TextBlock. 

## Sample Code

The Markout.Output.Inlines.Sample project contains a minimal WPF application that illustrates how Markout can be used.

The easiest way of getting started is:
* Create a VS solution containg a WPF Application project called "Markout.Output.Inlines.Sample". 
* Delete the following files in that project:
  * MainWindow.xaml
  * MainWindow.xaml.cs 
* In Manage NuGet Packages for the project add the Markout.Output.Inlines.Sample NuGet package.
* You should now be able to compile the project and run the app.

The app shows two text areas:
* The top one contains some input text with Markout tags.
* The TextBlock below shows the rendered result.

The Output.Inlines.TestApp provides a slightly more sophisticated example.

# NuGet

Markout is available as a set of NuGet packages. Just search for Markout on https://www.nuget.org/

# Markout Tag Syntax in Detail

| Tag | Qualifiers | Descriptions | Example |
| --- | ---------- | ------------ | ------- |
| b || Bold | {b} |
| i || Italic | {i} |
| u || Underline | {u} |
| 0 || Zap (remove all attributes) | {u}{b}underlinebold{0}plain |
| f, font | Font&nbsp;Name<br>Font&nbsp;Size<br>Decorations&nbsp;(b, I, u) | Font | {f:Courier:12:bu}<br> {f:Courier:12}<br> {font:Courier} |
| c, color, colour | Color&nbsp;Name | Color | {c:DarkGreen} |
| a, anchor, hyperlink | Uri<br> Action Name | Hyperlink | {a:http://google.com:LaunchUrl}Google{a}<br> Renders as: Google |
| {{ || Escape |A tag looks like {{b}<br> Renders as: A tag looks like {b} |


# Notes

## f, font

The font name should be one recognized by the renderer used. 
So the `Output.Inlines.MarkoutRenderer` class expects WPF FontFamily names. 

## c, color
The color name should be one recognized by the renderer used. 
So the `Output.Inlines.MarkoutRenderer` class expects WPF `KnownColor` names, or a hexadecimal number 
that can be passed to the `Color.FromArgb` method.

## a, anchor
This is currently the only closed tag supported, so it will render as a hyperlink tag containing 
the text between the first `{a}` tag and the following `{a}` tag.

You can add additional tags inside the anchor content to control the text color, font, etc. 
For example: `{a::SomeAction}{c:Green}{u}Do Some Action{a}` would render the anchor content in Green and underlined. 
The default Inlines rendering of an anchor is Blue underlined.
