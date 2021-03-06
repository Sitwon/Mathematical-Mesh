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

    public partial class _Data_SelectNormal :Goedel.Trojan.Data  {
		public Dialog_SelectNormal Dialog;

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
		string _Input_MeshGateway;
		public string Input_MeshGateway {
            get { return _Input_MeshGateway; }
            set { _Input_MeshGateway = value;  Refresh (); }
            }
		string _Input_AccountName;
		public string Input_AccountName {
            get { return _Input_AccountName; }
            set { _Input_AccountName = value;  Refresh (); }
            }

		// Output backing variables



		/// <summary>
		/// Here goes the action to be overriden
		/// </summary>

		public virtual bool Next () {
			return true;
			}


		}

    public partial class SelectNormal : _Data_SelectNormal {

		
		public SelectNormal (AddProfile  Data) {
			_Data = Data;

			// NB call to the initializer before we creaate the dialog so the
			// dialog can display the initialized data.
			Initialize ();
			this.Dialog = new Dialog_SelectNormal (this);
			}
		}


    /// <summary>
	/// This is the code behind for the XAML generated class.
    /// </summary>
    public partial class Dialog_SelectNormal : Page {

		public SelectNormal  Data;


		public Dialog_SelectNormal (SelectNormal Data) {
			InitializeComponent();
			this.Data = Data;
			Refresh ();

			}

		public void Refresh () {
			Input_MeshGateway.Text  = Data.Input_MeshGateway;
			Input_AccountName.Text  = Data.Input_AccountName;
			}

        private void Action_Next(object sender, RoutedEventArgs e) {
			var Result = Data.Next ();
			if (Result) {
				Data.Data.Navigate (Data.Data.Data_AskPassword);
				}
            }


		private void Changed_Input_MeshGateway (object sender, TextChangedEventArgs e) {
			Data.Input_MeshGateway = Input_MeshGateway.Text;
			}
		private void Changed_Input_AccountName (object sender, TextChangedEventArgs e) {
			Data.Input_AccountName = Input_AccountName.Text;
			}

		}
	}

