using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DirectoryUtils : MonoBehaviour {
    public static DirectoryInfo SafeCreateDirectory( string path )
    {
        if ( Directory.Exists( path ) )
        {
            return null;
        }
        return Directory.CreateDirectory( path );
    }
}
