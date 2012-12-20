namespace BuildMonitor.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TFSBuildMonitor = new System.ServiceProcess.ServiceProcessInstaller();
            this.BuildMonitor = new System.ServiceProcess.ServiceInstaller();
            // 
            // TFSBuildMonitor
            // 
            this.TFSBuildMonitor.Password = null;
            this.TFSBuildMonitor.Username = null;
            // 
            // BuildMonitor
            // 
            this.BuildMonitor.DisplayName = "TFS Build Monitor";
            this.BuildMonitor.ServiceName = "TFSBuildMonitor";
            this.BuildMonitor.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.TFSBuildMonitor,
            this.BuildMonitor});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller TFSBuildMonitor;
        private System.ServiceProcess.ServiceInstaller BuildMonitor;
    }
}