[Setup]
AppName               = JaqaacGUI
AppVerName            = JaqaacGUI 1.1
AppPublisher          = Giovanni Capuano
AppPublisherURL       = https://github.com/RoxasShadow
AppVersion            = 1.1
DefaultDirName        = {pf}\JaqaacGUI
DefaultGroupName      = JaqaacGUI
UninstallDisplayIcon  = {app}\JaqaacGUI.exe
Compression           = lzma2
SolidCompression      = yes
OutputDir             = Installer

[Tasks]
Name: "desktopicon";  Description: "{cm:CreateDesktopIcon}"
Name: "qaac";         Description: "Install qaac v1.40"       
Name: "eac3to";       Description: "Install eac3to v3.24 (Required for m2ts input)"

[Icons]
Name: "{group}\JaqaacGUI";        Filename: "{app}\JaqaacGUI.exe"
Name: "{userdesktop}\JaqaacGUI";  Filename: "{app}\JaqaacGUI.exe";  Tasks: desktopicon

[Files]
Source: "JaqaacGUI\bin\Release\JaqaacGUI.exe";        DestDir: "{app}"

Source: "Requirements\libFLAC++.dll";                 DestDir: "{win}";         Tasks: qaac
Source: "Requirements\libFLAC.dll";                   DestDir: "{win}";         Tasks: qaac
Source: "Requirements\qaac.exe";                      DestDir: "{win}";         Tasks: qaac
Source: "Requirements\refalac.exe";                   DestDir: "{win}";         Tasks: qaac
Source: "Requirements\msvcr100.dll";                  DestDir: "{syswow64}";    Tasks: qaac
Source: "Requirements\msvcp100.dll";                  DestDir: "{syswow64}";    Tasks: qaac
Source: "Requirements\libsoxrate.dll";                DestDir: "{syswow64}";    Tasks: qaac

Source: "Requirements\avcodec.dll";                   DestDir: "{win}";         Tasks: eac3to
Source: "Requirements\avutil-50.dll";                 DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\hdcd.dll";                      DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\HookSurcode.dll";               DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\libAften.dll";                  DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\libFLAC.dll";                   DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\libMatrix.dll";                 DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\libSsrc.dll";                   DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\r8b.dll";                       DestDir: "{win}";         Tasks: eac3to 
Source: "Requirements\eac3to.exe";                    DestDir: "{win}";         Tasks: eac3to

[Run]
Filename: "{app}\JaqaacGUI.exe";                      Description: "{cm:LaunchProgram,JaqaacGUI}"; Flags: nowait postinstall skipifsilent unchecked