using System.CommandLine;
using System.CommandLine.Invocation;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;

var filesOption = new Option<IEnumerable<FileInfo>>("--file", "Path to the file (required). Allow multiple files.")
{
    Arity = ArgumentArity.OneOrMore,
    AllowMultipleArgumentsPerToken = true,
    IsRequired = true
};
filesOption.AddAlias("-f");
filesOption.LegalFilePathsOnly();
filesOption.AddValidator(result =>
{
    var files = result.GetValueForOption(filesOption);
    var notExistingFiles = files!.Where(f => !f.Exists).ToArray();
    if (notExistingFiles.Length>0)
    {
        result.ErrorMessage = string.Join(Environment.NewLine,
            notExistingFiles.Select(f => $"The file '{f.Name}' not found."));
    }
});

var outputOption = new Option<FileInfo>("--output", () => new FileInfo("generated.pdf"), "Path to save generated file")
{
    Arity = ArgumentArity.ExactlyOne
};
outputOption.AddAlias("-o");
outputOption.LegalFilePathsOnly();

var watermarkOption = new Option<string>("--watermark", "Watermark text (optional)")
{
    Arity = ArgumentArity.ZeroOrOne,
    IsRequired = false
};
watermarkOption.AddAlias("-w");

var headerTextOption = new Option<string>("--header", "Page header text (optional)")
{
    Arity = ArgumentArity.ZeroOrOne,
    IsRequired = false
};

var isPreviewOption = new Option<bool>("--preview", () => false, "Show preview without saving");

var rootCommand = new RootCommand("Convert images to single PDF file");
rootCommand.AddOption(filesOption);
rootCommand.AddOption(outputOption);
rootCommand.AddOption(watermarkOption);
rootCommand.AddOption(headerTextOption);
rootCommand.AddOption(isPreviewOption);
rootCommand.SetHandler(Handle);

await rootCommand.InvokeAsync(args);

void Handle(InvocationContext context)
{
    var inputFiles = context.ParseResult.GetValueForOption(filesOption)!;
    var outputFile = context.ParseResult.GetValueForOption(outputOption)!;
    var watermark = context.ParseResult.GetValueForOption(watermarkOption);
    var headerText = context.ParseResult.GetValueForOption(headerTextOption);
    var isPreview = context.ParseResult.GetValueForOption(isPreviewOption);

    var doc = GenerateDocument(inputFiles, watermark, headerText);

    if (isPreview)
    {
        try
        {
            doc.ShowInPreviewer();
        }
        catch (TaskCanceledException)
        {
        }
    }
    else
    {
        doc.GeneratePdf(outputFile.FullName);
    }
}

static Document GenerateDocument(IEnumerable<FileInfo> files, string? watermark, string? headerText)
{
    return Document.Create(container =>
    {
        foreach (var file in files)
        {
            container.Page(page => FillPage(page, file.FullName, headerText, watermark));
        }
    });
}

static void FillPage(PageDescriptor page, string filePath, string? headerText, string? watermark)
{
    page.Size(PageSizes.A4);
    page.Margin(5);

    FillHeader(page, headerText);

    page.Content().Layers(layers =>
    {
        layers.PrimaryLayer().Image(filePath).FitArea();
        FillWatermark(layers, watermark);
    });
}

static void FillHeader(PageDescriptor page, string? headerText)
{
    if (string.IsNullOrEmpty(headerText))
    {
        return;
    }

    page.Header().AlignMiddle().Text(headerText);
}

static void FillWatermark(LayersDescriptor layers, string? watermark)
{
    if (string.IsNullOrEmpty(watermark))
    {
        return;
    }

    layers.Layer()
        .AlignCenter()
        .AlignMiddle()
        .Text(watermark)
        .FontSize(72).Bold().FontColor("#3afafafa");
}
