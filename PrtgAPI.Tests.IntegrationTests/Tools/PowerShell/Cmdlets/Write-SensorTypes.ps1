﻿function Write-SensorTypes
{
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
        [MissingSensorType[]]$Types
    )
    
    $sortedTypes  = $Types|sort Name

    $builder = New-Object System.Text.StringBuilder

    $nl = [Environment]::NewLine

    [void]$builder.AppendLine("// This code was generated by a tool. Any changes made manually will be lost");
    [void]$builder.AppendLine("// the next time this code is regenerated.");
    [void]$builder.AppendLine("using System.ComponentModel;");
    [void]$builder.AppendLine("using System.Xml.Serialization;$nl");

    [void]$builder.AppendLine("namespace PrtgAPI");
    [void]$builder.AppendLine("{");

    [void]$builder.AppendLine("    /// <summary>");
    [void]$builder.AppendLine("    /// <para type=`"description`">Specifies types of sensors that can be created in PRTG.</para>");
    [void]$builder.AppendLine("    /// </summary>");
    [void]$builder.AppendLine("    enum SensorTypeInternal");
    [void]$builder.AppendLine("    {");

    for($i = 0; $i -lt $sortedTypes.Count; $i++)
    {
        [void]$builder.AppendLine("        /// <summary>");
        [void]$builder.AppendLine("        /// $($sortedTypes[$i].Description)");
        [void]$builder.AppendLine("        /// </summary>");
        [void]$builder.AppendLine("        [XmlEnum(`"$($sortedTypes[$i].Id)`")]");
        [void]$builder.AppendLine("        [Description(`"$($sortedTypes[$i].Name)`")]");

        $enumName = $sortedTypes[$i].Name -replace "[^a-zA-Z0-9_]",""

        $enumName = $enumName -replace "BETA",""

        [void]$builder.Append("        $enumName");

        if ($i -lt $sortedTypes.Count - 1)
        {
            [void]$builder.AppendLine(",$nl");
        }
        else
        {
            [void]$builder.AppendLine();
        }
    }

    [void]$builder.AppendLine("    }");

    [void]$builder.Append("}");

    Write-Output $builder.ToString()
}