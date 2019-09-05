#define AppName "MHWAppearanceEditor"

#ifndef AppVersion
#define AppVersion GetFileVersion("..\output\MHWAppearanceEditor.exe")
#endif

[Setup]
AppName={#AppName}
AppVerName={#AppName}
AppVersion={#AppVersion}
VersionInfoVersion={#AppVersion}
WizardStyle=modern
DefaultDirName={userappdata}\{#AppName}
DefaultGroupName={#AppName}
UninstallDisplayIcon={app}\{#AppName}.exe
Compression=lzma2
SolidCompression=yes
OutputDir=../dist
OutputBaseFilename=setup
PrivilegesRequired=lowest

[Files]
Source: "..\output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppName}.exe"
