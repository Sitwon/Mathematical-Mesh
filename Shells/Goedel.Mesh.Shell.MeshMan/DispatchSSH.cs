﻿using Goedel.Utilities;
using Goedel.IO;
using Goedel.Mesh.Platform;
using Goedel.Cryptography.KeyFile;

namespace Goedel.Mesh.MeshMan {

    public partial class Shell {
        /// Create a new web application profile.
        /// </summary>
        /// <param name="Options">Command line parameters</param>
        public override void SSHCreate (SSHCreate Options) {
            SetReporting(Options);
            var SessionPersonal = GetPersonal(Options);
            var SSHProfile = new SSHProfile(SessionPersonal.PersonalProfile);

            foreach (var Device in PersonalProfile.Devices) {
                SSHProfile.AddDevice(Device.DeviceProfile, true);
                }
            var RegistrationApplication = SessionPersonal.Add(SSHProfile);
            }

        /// Create a new web application profile.
        /// </summary>
        /// <param name="Options">Command line parameters</param>
        public override void SSHKnown (SSHKnown Options) {
            SetReporting(Options);
            var RegistrationApplication = GetApplication(Options, "SSHProfile");
            var SSHProfile = RegistrationApplication?.ApplicationProfile as SSHProfile;
            Assert.NotNull(SSHProfile, ProfileNotFound.Throw);

            var Private = SSHProfile.Private;

            if (Private == null) {
                Report("No hosts");

                }

            var DecryptedDevicePrivate = SSHProfile.DecryptedDevicePrivate;

            Assert.NotNull(Private, ProfileNotReadable.Throw);
            foreach (var Host in Private.HostEntries) {
                Report("Host {0}", Host.Identifier);
                }

            }


        /// Create a list of SSH authorized keys from the Mesh profile
        /// </summary>
        /// <param name="Options">Command line parameters</param>
        public override void SSHAuth (SSHAuth Options) {
            SetReporting(Options);
            var RegistrationApplication = GetApplication(Options, "SSHProfile");
            var SSHProfile = RegistrationApplication?.ApplicationProfile as SSHProfile;
            Assert.NotNull(SSHProfile, ProfileNotFound.Throw);

            Debug.WriteLine("udf {0} mmm:{1}", PersonalProfile.DeviceProfile.UDF, PortalID);
            foreach (var Device in SSHProfile.Devices) {

                switch (Device) {
                    case SSHDevicePublic SSHDevicePublic: {
                        Debug.WriteLine(SSHDevicePublic.ToOpenSSH(PortalID));
                        break;
                        }
                    }
                }
            }

        /// Write out the SSH public key for this device
        /// </summary>
        /// <param name="Options">Command line parameters</param>
        public override void SSHPublic (SSHPublic Options) {
            SetReporting(Options);
            var RegistrationApplication = GetApplication(Options, "SSHProfile");
            var SSHProfile = RegistrationApplication?.ApplicationProfile as SSHProfile;
            Assert.NotNull(SSHProfile, ProfileNotFound.Throw);
            var SSHDevicePublic = SSHProfile.SSHDevicePublic;
            Assert.NotNull(SSHDevicePublic, ProfileNotFound.Throw);
            Debug.WriteLine(SSHDevicePublic.ToOpenSSH(PortalID));
            }

        


        /// Create a new web application profile.
        /// </summary>
        /// <param name="Options">Command line parameters</param>
        public override void SSHPrivate (SSHPrivate Options) {
            SetReporting(Options);
            var RegistrationApplication = GetApplication(Options, "SSHProfile");
            var SSHProfile = RegistrationApplication?.ApplicationProfile as SSHProfile;
            Assert.NotNull(SSHProfile, ProfileNotFound.Throw);
            var SSHDevicePublic = SSHProfile.SSHDevicePublic;
            Assert.NotNull(SSHDevicePublic, ProfileNotFound.Throw);

            var KeyPair = SSHDevicePublic.PublicKey.PrivateKey;
            var PEMPrivate = KeyPair.ToPEMPrivate();
            Debug.WriteLine(PEMPrivate);

            }

        }
    }
