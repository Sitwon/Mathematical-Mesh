﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using Goedel.Trojan;

namespace Goedel.MeshProfileManager {

    public partial class _Data_ConnectPending :Goedel.Trojan.Data  {
		public Dialog_ConnectPending Dialog;

		public override Page Page {
		    get { return Dialog; }
			}

		protected AddProfile _Data;
		public AddProfile Data {
			get {return _Data;}
			}


		public void Refresh () {
			if (Dialog != null) {
				Dialog.Refresh ();
				}
			}

//		protected Wizard_AddProfile  _Wizard;	
//		public Wizard_AddProfile  Wizard {
//				get {return _Wizard;}
//				}
//		public Data_AddProfile  Data {
//				get {return Wizard.Data;}
//				}

		// Input backing variables

		// Output backing variables
		string _Output_ProfileUDF;
		public string Output_ProfileUDF {
            get { return _Output_ProfileUDF; }
            set { _Output_ProfileUDF = value;   Refresh (); 				
				//if (Dialog != null) { Dialog.UpdateProgress(); } 
}
            }

		public int Completion_WaitConnectTask = 0;

		public virtual void Do_WaitConnectTask () {
            Completion_WaitConnectTask = -1;
			Dialog.UpdateProgress ();
			WaitConnectTask ();
            Completion_WaitConnectTask = 100;
			Dialog.UpdateProgress ();
			}

		public virtual void WaitConnectTask () {
            Thread.Sleep(2000);
            Completion_WaitConnectTask = 100;
			}



		}

    public partial class ConnectPending : _Data_ConnectPending {

		
		public ConnectPending (AddProfile  Data) {
			_Data = Data;

			// NB call to the initializer before we creaate the dialog so the
			// dialog can display the initialized data.
			Initialize ();
			this.Dialog = new Dialog_ConnectPending (this);
			}
		}


    /// <summary>
	/// This is the code behind for the XAML generated class.
    /// </summary>
    public partial class Dialog_ConnectPending : Page {

		public ConnectPending  Data;

		public BackgroundWorker BackgroundWorker;

		public Dialog_ConnectPending (ConnectPending Data) {
			InitializeComponent();
			this.Data = Data;
			Refresh ();

            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.DoWork += new DoWorkEventHandler(DoWork);
            BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RunWorkerCompleted);
            BackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
            BackgroundWorker.RunWorkerAsync();
			}

        // Should probably move this to the Data class so that it can be inherited
        public void DoWork(object sender, DoWorkEventArgs e) {
			Data.Do_WaitConnectTask ();
            }

        public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			Data.Data.Navigate (Data.Data.Data_CheckFingerPrint);
            }

        public void ProgressChanged(object sender, ProgressChangedEventArgs e) {
			if (Data.Completion_WaitConnectTask >= 0) {
				Task_WaitConnectTask.Value = Data.Completion_WaitConnectTask;
				Task_WaitConnectTask.IsIndeterminate = false;
				}
			else {
				Task_WaitConnectTask.IsIndeterminate = true;
				}
			Refresh ();
            }

		public void UpdateProgress () {
			BackgroundWorker.ReportProgress(100);
			} 
		public void Refresh () {
			Output_ProfileUDF.Text  = Data.Output_ProfileUDF;
			}




		}
	}

