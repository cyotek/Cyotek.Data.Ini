# Cyotek Ini Reading / Writing Library

[![NuGet][nugetbadge]][nuget]

The **Cyotek.Data.Ini** library contains a number of classes to
work with ini files.

## Did you say ini files

Yes, I said ini files. Traditionally, I would use the registry,
but that isn't such a great idea if you're making portable
software. So I could use XML, JSON, YAML, TOML etc. I have
nothing against any of these formats - with the exception of
TOML (which I have yet to use), I use the other 3 extensively.
But for my configuration files I still prefer something that
people can more easily edit, and which is more tolerant of
errors.

The bulk of these classes were wrote in 2014 and were part of my
main code library. As this isn't open source (yet), and I didn't
like the 3rd party ini helper library I was using with another
of my open source projects, I decided to extract this one and
make it available. I filled in some test coverage and fixed a
bug or two. With that said, despite its age it is still somewhat
underused, and so likely still has bugs lurking around and the
feature set and object model could definitely be improved.

## Getting the library

The easiest way of obtaining the library is via [NuGet][nuget].

> `Install-Package Cyotek.Data.Ini`

If you don't use NuGet, pre-compiled binaries can be obtained
from the [GitHub Releases page][ghrel].

Of course, you can always grab [the source][ghsrc] and build it
yourself!

## Quick Start

No documentation, so a few pointers to get started.

```csharp
using Cyotek.Data.Ini;

string fileName;
IniDocument document;

fileName = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "settings.ini");

// you can set the filename as part of the constructor
// the document is not loaded until you try to access a token
document = new IniDocument(fileName);

// or, you can load text directly with the LoadIni method
document.LoadIni("[Settings]\r\nalpha=beta");

// or, you can load text from a file (this is direct, unlike the constructor)
// overloads exist for loading from a Stream or TextWriter
document.Load(fileName);

// use GetValue to read a value from a section
Console.WriteLine(document.GetValue("Settings", "stringTest"));

// you can also specify a default for when the requested value doesn't exist
Console.WriteLine(document.GetValue("Settings", "newSettings", "fallback"));

// or if you're reading multiple values, it is more efficient to get the section first
IniSectionToken section;

section = (IniSectionToken)document.CreateSection("Settings"); // this will return the existing section if found, or create a new one if not
Console.WriteLine(section.GetValue("longTest"));
Console.WriteLine(section.GetValue("shortTest"));
Console.WriteLine(section.GetValue("stringTest"));

section = (IniSectionToken)document.GetSection("Settings"); // this version returns null if the section doesn't exist

// to get a list of defined names, you can call GetNames
foreach (string name in section.GetNames())
{
  Console.WriteLine(name);
}

// or enumerate tokens directly
foreach (IniToken token in document.ChildTokens)
{
  Console.WriteLine(token.Type.ToString());
}

// you can get the representation of a token with InnerText (or ToString(), they are equivalent)
foreach (IniToken token in document.ChildTokens)
{
  Console.WriteLine(token.InnerText);
}

// the document is a token too, so you can get the full ini configuration the same way
Console.WriteLine(document.InnerText);

// use SetValue on either IniDocument or IniSectionToken to set a value
section.SetValue("alpha", "beta");
document.SetValue("Settings", "gamma", "delta");

// save the file
// overloads exist for saving to a Stream or TextWriter
document.Save(fileName);

// for quick operations there are a couple of static methods, but these
// will cause the entire document to be loaded and saved with each call
// so should be used sparingly
Console.WriteLine(IniDocument.GetValue(fileName,"Settings","colorTest","Black"));
IniDocument.SetValue(fileName, "Settings", "newField", "Epsilon");
```

## Requirements

.NET Framework 3.5 or later.

Pre-built binaries are available via a signed [NuGet
package][nuget] containing the following targets.

* .NET 3.5
* .NET 4.0
* .NET 4.5.2
* .NET 4.6.2
* .NET 4.7.2
* .NET 4.8
* .NET 6.0
* .NET Core 3.1
* .NET Standard 2.1

## Contributing to this code

Contributions accepted!

* Found a problem? [Raise an issue][ghissue]
* Want to improve the code? [Make a pull request][ghpull]

Alternatively, if you make use of this software and it saves you
some time, [donations are welcome][donate].

[![PayPal Donation][paypalimg]][paypal]

[![By Me a Coffee Donation][bmacimg]][bmac]

## Known Issues

* No documentation
* API is a bit rough and ready

## Acknowledgements

* The wrench icon used in the package logo is the [Options,
  preferences, settings icon][IconRef] by iconify

[IconRef]: https://www.iconfinder.com/icons/510859/options_preferences_settings_tools_icon

[nuget]: https://www.nuget.org/packages/Cyotek.Data.Ini/
[nugetbadge]: https://img.shields.io/nuget/vpre/Cyotek.Data.Ini

[ghissue]: https://github.com/cyotek/Cyotek.Data.Ini/issues
[ghpull]: https://github.com/cyotek/Cyotek.Data.Ini/pulls
[ghrel]: https://github.com/cyotek/Cyotek.Data.Ini/releases
[ghsrc]: https://github.com/cyotek/Cyotek.Data.Ini

[donate]: https://www.cyotek.com/contribute
[paypal]: https://www.paypal.me/cyotek
[paypalimg]: https://static.cyotek.com/assets/images/donate.gif
[bmac]: https://www.buymeacoffee.com/cyotek
[bmacimg]: https://static.cyotek.com/assets/images/bmac.png
