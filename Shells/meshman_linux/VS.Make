
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
    Goedel.Command.dll\
    Goedel.Cryptography.Framework.dll\
    Goedel.Cryptography.Jose.dll\
    Goedel.Cryptography.KeyFile.dll\
    PHB Framework Library.dll\
    Goedel.Cryptography.dll\
    Goedel.FSR.dll\
    Goedel.IO.dll\
    Goedel.Platform.dll\
    Goedel.Registry.dll\
    Goedel.Utilities.dll\
    Goedel.Mesh.Client.dll\
    Goedel.Mesh.Platform.Windows.dll\
    Goedel.Mesh.Platform.dll\
    Goedel.Mesh.Server.dll\
    Goedel.Mesh.Shell.MeshMan.dll\
    Goedel.Mesh.dll\
    Goedel.Persistence.dll

prebuild : 
    echo "Do Prebuild"

postbuild : 
	powershell publishtarget meshman_linux.exe $(LinkFiles) 


