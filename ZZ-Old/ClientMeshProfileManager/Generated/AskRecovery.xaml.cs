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

    public partial class _Data_AskRecovery :Goedel.Trojan.Data  {
		public Dialog_AskRecovery Dialog;

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



		/// <summary>
		/// Here goes the action to be overriden
		/// </summary>

		public virtual bool CreateRecovery () {
			return true;
			}

		/// <summary>
		/// Here goes the action to be overriden
		/// </summary>

		public virtual bool SkipRecovery () {
			return true;
			}


		}

    public partial class AskRecovery : _Data_AskRecovery {

		
		public AskRecovery (AddProfile  Data) {
			_Data = Data;

			// NB call to the initializer before we creaate the dialog so the
			// dialog can display the initialized data.
			Initialize ();
			this.Dialog = new Dialog_AskRecovery (this);
			}
		}


    /// <summary>
	/// This is the code behind for the XAML generated class.
    /// </summary>
    public partial class Dialog_AskRecovery : Page {

		public AskRecovery  Data;


		public Dialog_AskRecovery (AskRecovery Data) {
			InitializeComponent();
			this.Data = Data;
			Refresh ();

			}

		public void Refresh () {
			}

        private void Action_CreateRecovery(object sender, RoutedEventArgs e) {
			var Result = Data.CreateRecovery ();
			if (Result) {
				Data.Data.Navigate (Data.Data.Data_GenerateRecoveryKeys);
				}
            }
        private void Action_SkipRecovery(object sender, RoutedEventArgs e) {
			var Result = Data.SkipRecovery ();
			if (Result) {
				Data.Data.Navigate (Data.Data.Data_GenerateKeys);
				}
            }



		}
	}

