# ImageToPdf
.Net global tool for convert images to PDF document

This tool uses an excellent library for generating pdf files called [QuestPDF](https://www.questpdf.com/).

## Installation

To install this tool, please execute the following command on your PC:

```
dotnet tool install Algel.ImageToPdf --global
```

To update the tool, please use:

```
dotnet tool update Algel.ImageToPdf --global
```

And to remove:

```
dotnet tool uninstall Algel.ImageToPdf --global
```

## Usage

You car run:
```
algel.imageToPdf -h
```

to show help:

```
Description:
  Convert images to single PDF file

Usage:
  Algel.ImageToPdf [options]

Options:
  -f, --file <file> (REQUIRED)  Path to the file (required). Allow multiple files.
  -o, --output <output>         Path to save generated file [default: generated.pdf]
  -w, --watermark <watermark>   Watermark text (optional)
  --header <header>             Page header text (optional)
  --preview                     Show preview without saving [default: False]
  --version                     Show version information
  -?, -h, --help                Show help and usage information
```

For example, we have a catalog with three images: 1.png, 2.jpg, 3.png
Executing the command:
```
algel.imageToPdf -f 1.png -f 2.jpg -f 3.png
```
will create a generated.pdf file with three pages that contain the listed images

It can also be simplified:
```
algel.imageToPdf -f 1.png 2.jpg 3.png
```
Optionally a watermark can be set:
```
algel.imageToPdf -f 1.png 2.jpg 3.png -w "my watermark"
```
It is also possible to preview the created document without saving it to a drive:
```
algel.imageToPdf -f 1.png 2.jpg 3.png -w "my watermark" --preview
```
But for this to work, you must first install the [QuestPDF Previewer](https://www.questpdf.com/document-previewer.html) tool
```
dotnet tool install QuestPDF.Previewer --global
```