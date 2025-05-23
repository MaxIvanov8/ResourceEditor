# ResourceEditor
This repo contains .NET project of simple resource editor on Windows Presentation Foundation(WPF) technology with using MVVM pattern. 

If client have several directories with resourse files on different languages, "Resource editor" can help to add missing resource files and change entry values.

![alt text](Screenshot.jpg)

User can:
1. Choose folder with resource files.
2. Change entries
3. Save recource files

If you need to save empty entries, you can click on checkbox.

In this project I use a DataTable because the simple DataGrid binding does not allow me to specify the number of columns in advance. So I first get the data from the files, then enter it into the DataTable. After the changes, I output the data from the DataTable and save it to the corresponding files.

## Stack:
- NET9
- WPF
- CommunityToolkit.Mvvm 8.4.0
- Microsoft.NETFramework.ReferenceAssemblies
- ResXResourceReader.NetStandard

## Clone:

Clone this repo to your local machine using: https://github.com/MaxIvanov8/ResourceEditor

## Visual Studio Build

Build LarixWpfTest.sln using Visual Studio 2022.

## Prerequisites
Have Visual Studio 2022.

## Verify Installation
You can verify the project builds correctly from Visual Studio using Build -> Build Solution.

## Translations
This project can be translated into Russian.

## Support
If you are having issues, please let me know.

## Author

MaxIvanov: https://github.com/MaxIvanov8
