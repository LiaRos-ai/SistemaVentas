; Script generado con Inno Setup
; Instalador para Sistema de Ventas

#define MyAppName "Sistema de Ventas"
#define MyAppVersion "1.1.0"
#define MyAppPublisher "Tu Empresa"
#define MyAppURL "https://www.tuempresa.com"
#define MyAppExeName "SistemaVentas.exe"

[Setup]
; Información básica
AppId={{YOUR-GUID-HERE}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
;LicenseFile=LICENSE.txt
;InfoBeforeFile=README.txt
OutputDir=Installers
OutputBaseFilename=SistemaVentas_Setup_v{#MyAppVersion}
SetupIconFile=icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

; Requisitos
MinVersion=10.0
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
; IMPORTANTE: Copiar TODOS los archivos de la carpeta publish
; Para framework-dependent, necesitas TODOS estos archivos:
;Source: "bin\Release\net10.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net10.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; Si usas una carpeta diferente, ajusta la ruta:
; Source: "bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

; Documentación (opcional)
;Source: "Manual_Usuario.pdf"; DestDir: "{app}\Docs"; Flags: ignoreversion
;Source: "Manual_Tecnico.pdf"; DestDir: "{app}\Docs"; Flags: ignoreversion

; Scripts de base de datos (opcional)
;Source: "Database\CreateDatabase.sql"; DestDir: "{app}\Database"; Flags: ignoreversion
;Source: "Database\SeedData.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Manual de Usuario"; Filename: "{app}\Docs\Manual_Usuario.pdf"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Ejecutar después de instalación
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
// Verificar si .NET Desktop Runtime está instalado
function IsDotNetDesktopRuntimeInstalled: Boolean;
var
  ResultCode: Integer;
  Output: AnsiString;
  TempFile: string;
begin
  Result := False;
  
  // Crear archivo temporal para capturar output
  TempFile := ExpandConstant('{tmp}\dotnet-check.txt');
  
  // Ejecutar dotnet --list-runtimes y guardar en archivo
  if Exec('cmd.exe', '/c dotnet --list-runtimes > "' + TempFile + '" 2>&1', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
  begin
    if LoadStringFromFile(TempFile, Output) then
    begin
      // Buscar "Microsoft.WindowsDesktop.App" en el output
      Result := Pos('Microsoft.WindowsDesktop.App', Output) > 0;
    end;
  end;
  
  // Limpiar archivo temporal
  DeleteFile(TempFile);
end;

function InitializeSetup: Boolean;
var
  ErrorMsg: string;
begin
  if not IsDotNetDesktopRuntimeInstalled then
  begin
    ErrorMsg := 'Este programa requiere .NET 8.0 Desktop Runtime (o .NET 10.0 según tu versión).' + #13#10 + #13#10 +
                'Por favor instale el runtime antes de continuar:' + #13#10 + #13#10 +
                'Para .NET 8.0:' + #13#10 +
                'https://dotnet.microsoft.com/download/dotnet/8.0' + #13#10 + #13#10 +
                'Para .NET 10.0:' + #13#10 +
                'https://dotnet.microsoft.com/download/dotnet/10.0' + #13#10 + #13#10 +
                'IMPORTANTE: Instale "Desktop Runtime" (no SDK)';
    
    MsgBox(ErrorMsg, mbError, MB_OK);
    Result := False;
  end
  else
    Result := True;
end;

// Configuración de cadena de conexión con Windows Authentication
procedure ConfigurarConexion;
var
  ConfigFile: string;
  Servidor: string;
  BaseDatos: string;
  CadenaConexion: string;
  ConfigContent: string;
begin
  ConfigFile := ExpandConstant('{app}\appsettings.json');

  // Configuración para SQL Server con autenticación de Windows
  Servidor := '(localdb)\MSSQLLocalDB';
  BaseDatos := 'SistemaVentas';

  // Cadena de conexión con Windows Authentication (Integrated Security)
  CadenaConexion := 'Server=' + Servidor + ';Database=' + BaseDatos + 
                    ';Integrated Security=True;TrustServerCertificate=True;';

  // Crear contenido JSON completo
  ConfigContent := '{' + #13#10 +
                   '  "ConnectionStrings": {' + #13#10 +
                   '    "DefaultConnection": "' + CadenaConexion + '"' + #13#10 +
                   '  },' + #13#10 +
                   '  "Logging": {' + #13#10 +
                   '    "LogLevel": {' + #13#10 +
                   '      "Default": "Information"' + #13#10 +
                   '    }' + #13#10 +
                   '  }' + #13#10 +
                   '}';

  // Guardar en archivo de configuración
  SaveStringToFile(ConfigFile, ConfigContent, False);
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    ConfigurarConexion;
  end;
end;

[UninstallDelete]
; Eliminar archivos de log al desinstalar
Type: filesandordirs; Name: "{app}\Logs"
Type: files; Name: "{app}\*.log"
