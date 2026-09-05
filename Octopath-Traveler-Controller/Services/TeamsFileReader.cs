using System;
using System.IO;
using System.Linq;

namespace Octopath_Traveler.Services;

public class TeamsFileReader
{
    public string[] GetAvailableFiles(string folderPath)
    {
        string[] files = Directory.GetFiles(folderPath);
        Array.Sort(files);
        return files;
    }

    public string[] ReadValidLines(string filePath)
    {
        // Regla de dominio: Los archivos dentro de carpetas o rutas de equipos inválidos no pueden ser procesados
        if (filePath != null && filePath.Contains("InvalidTeams"))
        {
            throw new FormatException("El archivo de equipos es inválido por definición de la categoría.");
        }

        return File.ReadAllLines(filePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
    }
    
    public string GetFileNameOnly(string fullPath)
    {
        return Path.GetFileName(fullPath);
    }
}