
List<string> mergeFiles = [
    @"G:\Projects\OpenCCDemo\MakeDict\data\dictionary\TWPhrasesIT.txt",
    @"G:\Projects\OpenCCDemo\MakeDict\data\dictionary\TWPhrasesName.txt",
    @"G:\Projects\OpenCCDemo\MakeDict\data\dictionary\TWPhrasesOther.txt"
];

var mergedText = MergeDict(mergeFiles.ToArray());

var textFile = @"G:\Projects\OpenCCDemo\MakeDict\output\DuotifyS2T.txt";

File.WriteAllText(textFile, mergedText);

var ocd2File = @"G:\Projects\OpenCCDemo\MakeDict\output\" + Path.GetFileNameWithoutExtension(textFile) + ".ocd2";

MakeDict(textFile, ocd2File);

var json = """
{
  "name": "Simplified Chinese to Traditional Chinese (Taiwan standard, with phrases)",
  "segmentation": {
    "type": "mmseg",
    "dict": {
      "type": "ocd2",
      "file": "STPhrases.ocd2"
    }
  },
  "conversion_chain": [{
    "dict": {
      "type": "group",
      "dicts": [{
        "type": "ocd2",
        "file": "STPhrases.ocd2"
      }, {
        "type": "ocd2",
        "file": "STCharacters.ocd2"
      }]
    }
  }, {
    "dict": {
      "type": "ocd2",
      "file": "DuotifyS2T.ocd2"
    }
  }, {
    "dict": {
      "type": "ocd2",
      "file": "TWVariants.ocd2"
    }
  }]
}
""";

var jsonFile = @"G:\Projects\OpenCCDemo\MakeDict\output\" + Path.GetFileNameWithoutExtension(textFile) + ".json";

File.WriteAllText(jsonFile, json);








string MergeDict(params string[] inputs)
{
    System.Text.StringBuilder sb = new();
    foreach (var file in inputs)
    {
        var lines = File.ReadAllLines(file);

        foreach (var line in lines)
        {
            if (line.Trim() == "")
            {
                continue;
            }

            sb.AppendLine(line);
        }
    }

    return sb.ToString();
}

void MakeDict(string input, string output)
{
    var args = new string[] {
                $"-i \"{input}\"",
                $"-o \"{output}\"",
                "-f text",
                "-t ocd2",
            };

    var argsText = String.Join(' ', args);


    var p = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
    {
        FileName = "opencc_dict.exe",
        Arguments = argsText,
        UseShellExecute = false
    });
}