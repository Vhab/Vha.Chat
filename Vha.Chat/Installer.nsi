; A little bit of useful information
Name "Vha.Chat ${ASSEMBLY_VERSION}"
VIAddVersionKey "ProductName" "Vha.Chat ${ASSEMBLY_VERSION} Installer"
VIAddVersionKey "CompanyName" "${ASSEMBLY_COMPANY}"
VIAddVersionKey "LegalCopyright" "${ASSEMBLY_COPYRIGHT}"
VIAddVersionKey "FileDescription" "${ASSEMBLY_DESCRIPTION}"
VIAddVersionKey "FileVersion" "${ASSEMBLY_VERSION}"
VIProductVersion ${ASSEMBLY_VERSION}

; The file to write
OutFile "bin\Release\Setup.exe"

; The default installation directory
InstallDir "$PROGRAMFILES\Vha.Chat"

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
  File "bin\Release\Vha.Chat.exe"
  File "bin\Release\*.dll"
  File "bin\Release\*.txt"
  File "bin\Release\Configuration.xml"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\VhaChat" "InstallDir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "DisplayName" "Vha.Chat ${ASSEMBLY_VERSION}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\VhaChat" "UninstallString" '"$INSTDIR\uninstall.exe"'
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