# ImageToPdf
.Net global tool for convert images to PDF document

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
Usage:  [options]

Options:
  -?|-h|--help                Show help information.
  -f|--file <File>            Path to the file
  -o|--output <FilePath>      Path to save generated file
                              Default value is: generated.pdf.
  -w|--watermark <Watermark>  Watermark text
  -h|--header <Header>        Page header text
  --preview                   Show preview without saving
```

For example, we have a catalog with three images: 1.png, 2.jpg, 3.png
Executing the command:
```
algel.imageToPdf -f 1.png -f 2.jpg -f 3.png
```
will create a generated.pdf file with three pages that contain the listed images