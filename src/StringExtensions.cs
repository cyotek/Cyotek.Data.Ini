using System.Globalization;
using System.Text;

namespace Cyotek.Data.Ini
{
  internal static class StringExtensions
  {
    #region Public Methods

    public static bool StartsWithAny(this string text, char[] anyOf)
    {
      bool result;

      result = false;

      if (!string.IsNullOrEmpty(text))
      {
        char firstChar;

        firstChar = text[0];

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (int i = 0; i < anyOf.Length; i++)
        {
          if (firstChar == anyOf[i])
          {
            result = true;
            break;
          }
        }
      }

      return result;
    }

    public static string ToEscapedLiteral(this string input)
    {
      string result;

      if (string.IsNullOrEmpty(input))
      {
        result = input;
      }
      else
      {
        StringBuilder sb;
        int length;

        length = input.Length;
        sb = StringBuilderCache.Acquire(length);

        for (int i = 0; i < length; i++)
        {
          char currentChar;

          currentChar = input[i];

          switch (currentChar)
          {
            case '\'':
              sb.Append(@"\'");
              break;

            case '\"':
              sb.Append(@"""");
              break;

            case '\\':
              sb.Append(@"\\");
              break;

            case '\0':
              sb.Append(@"\0");
              break;

            case '\a':
              sb.Append(@"\a");
              break;

            case '\b':
              sb.Append(@"\b");
              break;

            case '\f':
              sb.Append(@"\f");
              break;

            case '\n':
              sb.Append(@"\n");
              break;

            case '\r':
              sb.Append(@"\r");
              break;

            case '\t':
              sb.Append(@"\t");
              break;

            case '\v':
              sb.Append(@"\v");
              break;

            default:
              if (char.GetUnicodeCategory(currentChar) != UnicodeCategory.Control)
              {
                sb.Append(currentChar);
              }
              else
              {
                sb.Append(@"\u");
                sb.Append(((ushort)currentChar).ToString("x4"));
              }

              break;
          }
        }

        result = sb.ToStringAndRelease();
      }

      return result;
    }

    public static string ToLiteral(this string input)
    {
      if (!string.IsNullOrEmpty(input) && input.IndexOf('\\') != -1)
      {
        StringBuilder sb;
        int length;

        length = input.Length;
        sb = StringBuilderCache.Acquire(length);

        for (int i = 0; i < length; i++)
        {
          char currentChar;

          currentChar = input[i];

          if (currentChar == '\\')
          {
            char controlChar;

            controlChar = input[++i];

            if (controlChar != 'u')
            {
              switch (controlChar)
              {
                case '\'':
                  sb.Append('\'');
                  break;

                case '\\':
                  sb.Append('\\');
                  break;

                case '0':
                  sb.Append('\0');
                  break;

                case 'a':
                  sb.Append('\a');
                  break;

                case 'b':
                  sb.Append('\b');
                  break;

                case 'f':
                  sb.Append('\f');
                  break;

                case 'n':
                  sb.Append('\n');
                  break;

                case 'r':
                  sb.Append('\r');
                  break;

                case 't':
                  sb.Append('\t');
                  break;

                case 'v':
                  sb.Append('\v');
                  break;
              }
            }
            else
            {
              char[] digits;

              digits = new char[4];
              input.CopyTo(i + 1, digits, 0, 4);

              sb.Append((char)int.Parse(new string(digits), NumberStyles.AllowHexSpecifier));

              i += 4;
            }
          }
          else
          {
            sb.Append(currentChar);
          }
        }

        input = sb.ToStringAndRelease();
      }

      return input;
    }

    public static string TrimWhitespace(this string text)
    {
      if (!string.IsNullOrEmpty(text) && (char.IsWhiteSpace(text[0]) || char.IsWhiteSpace(text[text.Length - 1])))
      {
        text = text.Trim();
      }

      return text;
    }

    #endregion Public Methods
  }
}