using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Net;
using Microsoft.Hadoop.WebHDFS;

namespace Net.WhatsUpDuck.WebHdfsShell {
	internal class WebHdfsDriveInfo : PSDriveInfo {
		public WebHDFSClient Client { get; set; }

		public WebHdfsDriveInfo(PSDriveInfo driveInfo) : base(driveInfo) { }
	}
}
