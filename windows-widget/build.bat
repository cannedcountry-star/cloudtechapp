@echo off
set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set WPF_DIR=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\WPF

echo Compiling CloudTech Desktop Widget...
"%CSC%" /nologo /target:winexe /optimize+ /out:CloudTechWidget.exe /lib:"%WPF_DIR%" /r:PresentationCore.dll /r:PresentationFramework.dll /r:WindowsBase.dll /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:System.Xaml.dll CloudTechWidget.cs

if %ERRORLEVEL% equ 0 (
    echo Build Successful: CloudTechWidget.exe
) else (
    echo Build Failed!
)
