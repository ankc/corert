// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using System.Threading;
using Internal.Runtime.Augments;
using Internal.Reflection.Execution;

namespace Internal.Runtime.TypeLoader
{
    public sealed class EcmaModuleInfo : ModuleInfo
    {
        /// <summary>
        /// Ecma PE data for this module.
        /// </summary>
        public PEInfo EcmaPEInfo { get; private set; }

        /// <summary>
        /// Initialize module info and construct per-module metadata reader.
        /// </summary>
        /// <param name="moduleHandle">Handle (address) of module to initialize</param>
        internal EcmaModuleInfo(IntPtr moduleHandle, PEInfo peinfo)
            : base(moduleHandle, ModuleType.Ecma)
        {
            EcmaPEInfo = peinfo;
        }
    }

    public static class EcmaModuleList
    {
        /// <summary>
        /// Locate the containing module for a given metadata reader. Assert when not found.
        /// </summary>
        /// <param name="reader">Metadata reader to look up</param>
        /// <returns>Module handle of the module containing the given reader</returns>
        public static ModuleInfo GetModuleInfoForMetadataReader(this ModuleList moduleList, MetadataReader reader)
        {
            foreach (ModuleInfo moduleInfo in moduleList.GetLoadedModuleMapInternal().Modules)
            {
                EcmaModuleInfo ecmaModuleInfo = moduleInfo as EcmaModuleInfo;
                if (ecmaModuleInfo == null)
                    continue;
                
                if (ecmaModuleInfo.EcmaPEInfo.Reader == reader)
                {
                    return moduleInfo;
                }
            }

            // We should never have a reader that is not associated with a module (where does it come from?!)
            Debug.Assert(false);
            return null;
        }
    }
}
