using Cyotek.Data.Ini;

string fileName;
IniDocument document;

fileName = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "settings.ini");

document = new IniDocument(fileName);

// use GetValue to read a value from a section
Console.WriteLine(document.GetValue("Settings", "stringTest"));

// you can also specify a default for when the requested value doesn't exist
Console.WriteLine(document.GetValue("Settings", "newSettings", "fallback"));

// or if you're reading multiple values, it is more efficient to get the section first
IniSectionToken section;

section = (IniSectionToken)document.GetSection("Settings"); // this will also create it if it doesn't exist
Console.WriteLine(section.GetValue("longTest"));
Console.WriteLine(section.GetValue("shortTest"));
Console.WriteLine(section.GetValue("stringTest"));

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
