﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Goedel.Cryptography;
using Goedel.Mesh.MeshMan;

/// <summary>
/// 
/// </summary>
namespace Meshman_Windows {

    /// <summary>
    /// 
    /// </summary>
    public class _Main {

        static _Main() {
            Goedel.Platform.PlatformFramework.Initialize();
            CryptographyWindows.Initialize();
            Goedel.Mesh.MeshWindows.Initialize();
            }

        static void Main(string[] args) {
            var CommandLineInterpreter = new CommandLineInterpreter();


            CommandLineInterpreter.MainMethod(args);
            }
        }

    }