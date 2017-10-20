﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Security.Principal;
using System.Security.AccessControl;
using System.IO;
using System.Diagnostics;


namespace TPA.CoreFramework
{
    [RunInstaller(true)]
    public partial class TPAInstaller : System.Configuration.Install.Installer
    {
        public TPAInstaller()
        {
            InitializeComponent();
        }
        public override void Install(IDictionary stateSaver)
        {
            Debugger.Break();
            // This gets the named parameters passed in from your custom action
            string folder = Context.Parameters["folder"];
            string logFolder = Context.Parameters["logs"];

            // This gets the "Authenticated Users" group, no matter what it's called
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            // Create the rules
            FileSystemAccessRule writerule = new FileSystemAccessRule(sid, FileSystemRights.Write, AccessControlType.Allow);
            
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                /*// Get your file's ACL
                DirectorySecurity fsecurity = Directory.GetAccessControl(folder);

                // Add the new rule to the ACL
                fsecurity.AddAccessRule(writerule);

                // Set the ACL back to the file
                Directory.SetAccessControl(folder, fsecurity);*/


                // Create security idenifier for all users (WorldSid)
                FileReader.ProvideWriteAccessToFolder(folder,true);

            }

            if (!string.IsNullOrEmpty(logFolder) && Directory.Exists(logFolder))
            {
                FileReader.ProvideWriteAccessToFolder(logFolder, false);
            }

            // Explicitly call the overriden method to properly return control to the installer
            base.Install(stateSaver);
        }

    }
}

