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
OutputBaseFilename={#AppName}-{#AppVersion}
PrivilegesRequired=lowest

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppName}.exe"
Name: "{group}\Uninstall {#AppName}"; Filename: "{uninstallexe}"
Name: "{userdesktop}\{#AppName}"; Filename: "{app}\{#AppName}.exe"; Tasks: desktopicon

[Run]
Filename: {app}\{#AppName}.exe; Description: Start after installation; Flags: nowait postinstall skipifsilent
