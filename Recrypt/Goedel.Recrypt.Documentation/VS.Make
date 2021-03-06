
# Supplemental Makefile for Visual Studios Projects
#
# Visual Studio is responsible for managing rules for production of
# code but does not provide support for pre and post build actions.
# 
# Prebuild items supported:
#   [None currently]
# 
# PostBuild items supported:
#   * Copy library to locations on disk
#   * Build ilMerge executables
#   * Postprocess VSIX projects
#   * Copy executables

LinkFiles = \
    Goedel.ASN.dll\
    Goedel.Cryptography.dll\
    Goedel.Cryptography.Jose.dll\
    Goedel.Cryptography.Windows.dll\
    Goedel.FSR.dll\
    Goedel.IO.dll\
    Goedel.Platform.dll\
    Goedel.Platform.Framework.dll\
    Goedel.Protocol.dll\
    Goedel.Protocol.Debug.dll\
    Goedel.Protocol.Framework.dll\
    Goedel.Registry.dll\
    Goedel.Utilities.dll\
    Goedel.Recrypt.Client.dll\
    Goedel.Recrypt.Server.dll\
    Goedel.Recrypt.dll

prebuild : 
    echo "Do Prebuild"

postbuild : 
	powershell publishtarget Goedel.Recrypt.Documentation.exe $(LinkFiles) 


