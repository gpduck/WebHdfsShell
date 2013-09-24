using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace Net.WhatsUpDuck.WebHdfsShell {
	[CmdletProvider("WebHdfs", ProviderCapabilities.Credentials)]
	public class WebHdfsDriveCmdletProvider : NavigationCmdletProvider {
		protected override PSDriveInfo NewDrive(PSDriveInfo drive) {

			//Check the drive parameter
			if(drive == null) {
				ErrorRecord er = new ErrorRecord(
						new ArgumentNullException("drive"),
						"NullDrive",
						ErrorCategory.InvalidArgument,
						null);
				WriteError(er);
				return null;
			}

			//Check the Root isn't null
			if (String.IsNullOrEmpty(drive.Root)) {
				ErrorRecord er = new ErrorRecord(
						new ArgumentException("drive.Root"),
						"NoRoot",
						ErrorCategory.InvalidArgument,
						drive);
				WriteError(er);
				return null;
			}


			//Check the Root is valid
			Uri hdfsRootUri = new Uri(drive.Root);
			if (hdfsRootUri.Scheme != "webhdfs") {
				ErrorRecord er = new ErrorRecord(
						new ArgumentException("drive.Root"),
						"InvalidRoot",
						ErrorCategory.InvalidArgument,
						drive);
				WriteError(er);
				return null;
			}

			//Save the drive ino
			WebHdfsDriveInfo hdfsDriveInfo = new WebHdfsDriveInfo(drive);
			hdfsDriveInfo.Client = new Microsoft.Hadoop.WebHDFS.WebHDFSClient(hdfsRootUri, drive.Credential.UserName);

			return hdfsDriveInfo;
		}

		protected override PSDriveInfo RemoveDrive(PSDriveInfo drive) {
			return base.RemoveDrive(drive);
		}

		protected override void GetItem(string path) {
			throw new NotImplementedException();
		}

		protected override bool ItemExists(string path) {
			WebHdfsDriveInfo driveInfo = this.PSDriveInfo as WebHdfsDriveInfo;
			try {
				driveInfo.Client.GetFileStatus(path);
				return true;
			} catch {
				return false;
			}
		}

		protected override bool IsValidPath(string path) {
			WebHdfsDriveInfo driveInfo = this.PSDriveInfo as WebHdfsDriveInfo;
			return true;
		}
	}
}
