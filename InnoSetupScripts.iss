; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ProjectsTM"
#define MyAppVersion GetFileVersion(SourcePath + "\ProjectsTM.exe\bin\Release\ProjectsTM.exe")
#define MyAppPublisher "Keiya Shimomura"
#define MyAppURL "https://github.com/skeiya/ProjectsTM"
#define MyAppExeName "ProjectsTM.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{4C44A710-1CB2-4662-955E-602ABEBF0088}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: ".\ProjectsTM.exe\bin\Release\FreeGridControl.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\Help\help.html"; DestDir: "{app}\Help"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.Logic.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.Service.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.UI.Common.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.UI.MainForm.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.UI.TaskList.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: ".\ProjectsTM.exe\bin\Release\ProjectsTM.ViewModel.dll"; DestDir: "{app}"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
