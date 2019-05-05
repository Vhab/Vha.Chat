; A little bit of useful information
Name "Vha.Chat ${Version}"
VIAddVersionKey "ProductName" "Vha.Chat ${Version}"
VIAddVersionKey "LegalCopyright" "© Remco van Oosterhout"
VIAddVersionKey "FileVersion" "${Version}"
VIAddVersionKey "ProductVersion" "${Version}"
VIProductVersion ${Version}

; The file to write
OutFile "${DistDir}\Vha.Chat-${Version}-Setup.exe"

; The default installation directory
InstallDir "$PROGRAMFILES64\Vha.Chat"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------
; Pages

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------
; The stuff to install
Section "Vha.Chat (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Add files
  File "${BinDir}\Vha.Chat.exe"
  File "${BinDir}\*.*"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\VhaChat" "InstallDir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "DisplayName" "Vha.Chat ${Version}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "DisplayVersion" "${Version}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "InstallLocation" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "URLInfoAbout" "https://github.com/Vhab/Vha.Chat"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Vha.Chat"
  CreateShortCut "$SMPROGRAMS\Vha.Chat\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Vha.Chat\Vha.Chat.lnk" "$INSTDIR\Vha.Chat.exe" "" "$INSTDIR\Vha.Chat.exe" 0
  
SectionEnd

Section "Desktop Shortcut"

  CreateShortCut "$DESKTOP\Vha.Chat.lnk" "$INSTDIR\Vha.Chat.exe" "" "$INSTDIR\Vha.Chat.exe" 0
  
SectionEnd

;--------------------------------
; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat"
  DeleteRegKey HKLM "SOFTWARE\VhaChat"

  ; Remove desktop shortcut
  Delete "$DESKTOP\Vha.Chat.lnk"
  
  ; Remove directories used
  RMDir /r "$SMPROGRAMS\VhaChat"
  RMDir /r "$INSTDIR"

SectionEnd