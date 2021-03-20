using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using UnityEngine;

public class IniFileReader {
    // 大文字小文字を区別するか？
    // Win32の実装では区別しないので、それに合わせておく
    public bool             IgnoreCase { get; set; } = true;

    private List<string>    mLines = new List<string>();
    private int             mSectionTop = -1;

    public IniFileReader(
        string path,
        Encoding encoding = null)
    {
        // UTF-8ではなくデフォルトANSI（日本ならShiftJIS）にしとく
        // ※テキストの先頭に UTF8/16LE/16BE のBOMが付いてる場合は、
        // この指定は無視して Unicode で読み込まれる。
        if ( encoding == null )
            { encoding = Encoding.Default; }

        using ( var reader = new System.IO.StreamReader( path, encoding ) )
        {
            var line = reader.ReadLine();
            while ( line != null )
            {
                // 行頭行末のスペース、空行とコメント行は最初から省いておく
                line = line.Trim();
                if ( !String.IsNullOrEmpty( line ) && (line[0] != ';') )
                    { mLines.Add( line ); }
                line = reader.ReadLine();
            }
        }
    }

    // セクションを設定
    public bool
    SetSection(
        string section)
    {
        var minLength = section.Length + 2;
        for ( int i = 0; i < mLines.Count; i++ )
        {
            var line = mLines[i];
            if ( line[0] != '[' )
                { continue; }

            if ( (line.Length >= minLength) && (line[1+section.Length] == ']') &&
                 (String.Compare(section,0, line,1, section.Length, IgnoreCase) == 0) )
            {
                mSectionTop = i + 1;
                return true;
            }
        }

        mSectionTop = -1;
        return false;
    }

    // 値を文字列で取得
    public string
    GetValue(
        string key,
        string defaultValue = null)
    {
        if ( mSectionTop < 0 )
            { return defaultValue; }

        for ( int i = mSectionTop; i < mLines.Count; i++ )
        {
            var        line = mLines[i];
            // セクション終了
            if ( line[0] == '[' )
                { return defaultValue; }

            if ( String.Compare( key, 0, line, 0, key.Length, IgnoreCase ) != 0 )
                { continue; }

            // キーとイコールの間のスペースを容認
            var        valueTop = key.Length;
            while ( valueTop < line.Length )
            {
                char    c = line[valueTop];
                if ( c == '=' )
                    { return line.Substring( valueTop + 1 ).Trim(); }
                else if ( Char.IsWhiteSpace( c ) )
                    { valueTop++; }
                else
                    { break; }
            }
        }

        return defaultValue;
    }
}