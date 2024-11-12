#!/usr/bin/env dotnet-script

#r "nuget: Microsoft.Security.Extensions, 1.3.0"

using Microsoft.Security.Extensions;
using System.IO;

var publishDir = Args[0];
var outputFile = Args[1];

Console.WriteLine(publishDir);
Console.WriteLine(outputFile);

var unsignedBinaries = Directory.EnumerateFiles(publishDir,  "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".dll") || s.EndsWith(".exe"))
            .Select(Path.GetFullPath)
            .Where(f => 
            {
                using FileStream fs = File.OpenRead(f);
                var sigInfo = FileSignatureInfo.GetFromFileStream(fs);
                return sigInfo.State != SignatureState.Unsigned;
            });

File.WriteAllLines(outputFile, unsignedBinaries);

// foreach(var v in unsignedBinaries)
// {
//     Console.WriteLine(v);
// }
