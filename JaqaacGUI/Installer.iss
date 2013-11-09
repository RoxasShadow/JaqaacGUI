[Setup]
AppName               = JaqaacGUI
AppVerName            = JaqaacGUI 1.2
AppPublisher          = Giovanni Capuano
AppPublisherURL       = https://github.com/RoxasShadow
AppVersion            = 1.2
DefaultDirName        = {pf}\JaqaacGUI
DefaultGroupName      = JaqaacGUI
UninstallDisplayIcon  = {app}\JaqaacGUI.exe
Compression           = lzma2
SolidCompression      = yes
OutputDir             = Installer

[Tasks]
Name: "desktopicon";  Description: "{cm:CreateDesktopIcon}"
Name: "qaac";         Description: "Install qaac v2.20"       
Name: "eac3to";       Description: "Install eac3to v3.27 (Required for m2ts input)"

[Icons]
Name: "{group}\JaqaacGUI";        Filename: "{app}\JaqaacGUI.exe"
Name: "{userdesktop}\JaqaacGUI";  Filename: "{app}\JaqaacGUI.exe";  Tasks: desktopicon

[Files]
Source: "JaqaacGUI\bin\Debug\JaqaacGUI.exe";        DestDir: "{app}"

Source: "Requirements\qaac.exe";                      DestDir: "{win}";         Tasks: qaac
Source: "Requirements\refalac.exe";                   DestDir: "{win}";         Tasks: qaac
Source: "Requirements\msvcr100.dll";                  DestDir: "{syswow64}";    Tasks: qaac
Source: "Requirements\msvcp100.dll";                  DestDir: "{syswow64}";    Tasks: qaac
Source: "Requirements\libsoxrate.dll";                DestDir: "{syswow64}";    Tasks: qaac 
Source: "Requirements\libFLAC.dll";                   DestDir: "{win}";         Tasks: qaac

Source: "Requirements\avcodec-54.dll";                DestDir: "{win}";         Tasks: eac3to
Source: "Requirements\avutil-52.dll";                 DestDir: "{win}";         Tasks: eac3to 
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