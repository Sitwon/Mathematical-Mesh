
//  Copyright (c) 2016 by .
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  
//  
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Goedel.Protocol;





namespace Goedel.Persistence {


	/// <summary>
	///
	/// Record persistence entries in a log format.
	/// </summary>
	public abstract partial class LogEntry : global::Goedel.Protocol.JSONObject {

        /// <summary>
        /// Schema tag.
        /// </summary>
        /// <returns>The tag value</returns>
		public override string Tag () {
			return "LogEntry";
			}

		/// <summary>
        /// Construct an instance from the specified tagged JSONReader stream.
        /// </summary>
        /// <param name="JSONReader">Input stream</param>
        /// <param name="Out">The created object</param>
        public static void Deserialize(JSONReader JSONReader, out JSONObject Out) {
	
			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                Out = null;
                return;
                }

			string token = JSONReader.ReadToken ();
			Out = null;

			switch (token) {

				case "DataItem" : {
					Out = new DataItem ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "Header" : {
					Out = new Header ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "Delta" : {
					Out = new Delta ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "IndexTerm" : {
					Out = new IndexTerm ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "Final" : {
					Out = new Final ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "Terminal" : {
					Out = new Terminal ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "IndexIndex" : {
					Out = new IndexIndex ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "Index" : {
					Out = new Index ();
					Out.Deserialize (JSONReader);
					break;
					}


				case "IndexEntry" : {
					Out = new IndexEntry ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
					throw new Exception ("Not supported");
					}
				}	
			JSONReader.EndObject ();
            }
		}



		// Service Dispatch Classes



		// Transaction Classes
	/// <summary>
	///
	/// A log entry
	/// </summary>
	public partial class DataItem : LogEntry {
        /// <summary>
        ///Unique Transaction ID. Text format ensures that the ID is easily
        ///visible in binary formats etc.
        /// </summary>

		public virtual string						TransactionID  {get; set;}
        /// <summary>
        ///Unique primary key.
        /// </summary>

		public virtual string						PrimaryKey  {get; set;}
        /// <summary>
        ///Specifies the immediately prior transaction that affected
        ///this identifier
        /// </summary>

		public virtual string						PriorTransactionID  {get; set;}
        /// <summary>
        ///Type of transaction, valid values include:
        ///Delete (the key is removed)
        ///New (the key is created)
        ///Modify (the data associated with the key is modified)
        ///Drop (the key is no longer active and will not be modified again)
        /// </summary>

		public virtual string						Action  {get; set;}
		bool								__Added = false;
		private DateTime						_Added;
        /// <summary>
        ///Time at which the item was added to the log
        /// </summary>

		public virtual DateTime						Added {
			get {return _Added;}
			set {_Added = value; __Added = true; }
			}
        /// <summary>
        ///Index terms for data item
        /// </summary>

		public virtual List<IndexTerm>				Keys  {get; set;}
        /// <summary>
        ///Binary data.
        /// </summary>

		public virtual byte[]						Data  {get; set;}
        /// <summary>
        ///Text data.
        /// </summary>

		public virtual string						Text  {get; set;}
		bool								__Pending = false;
		private bool						_Pending;
        /// <summary>
        ///If true, transaction is pending and will not be final until
        ///a commit transaction is posted.
        /// </summary>

		public virtual bool						Pending {
			get {return _Pending;}
			set {_Pending = value; __Pending = true; }
			}
		bool								__Commit = false;
		private bool						_Commit;
        /// <summary>
        ///If true, all pending transactions with the specified TransactionID
        ///are committed and cannot roll back.
        /// </summary>

		public virtual bool						Commit {
			get {return _Commit;}
			set {_Commit = value; __Commit = true; }
			}
		bool								__Rollback = false;
		private bool						_Rollback;
        /// <summary>
        ///If true, all pending transactions with the specified TransactionID
        ///are aborted and ignored.
        /// </summary>

		public virtual bool						Rollback {
			get {return _Rollback;}
			set {_Rollback = value; __Rollback = true; }
			}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "DataItem";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (TransactionID != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("TransactionID", 1);
					_Writer.WriteString (TransactionID);
				}
			if (PrimaryKey != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("PrimaryKey", 1);
					_Writer.WriteString (PrimaryKey);
				}
			if (PriorTransactionID != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("PriorTransactionID", 1);
					_Writer.WriteString (PriorTransactionID);
				}
			if (Action != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Action", 1);
					_Writer.WriteString (Action);
				}
			if (__Added){
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Added", 1);
					_Writer.WriteDateTime (Added);
				}
			if (Keys != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Keys", 1);
				_Writer.WriteArrayStart ();
				bool _firstarray = true;
				foreach (var _index in Keys) {
					_Writer.WriteArraySeparator (ref _firstarray);
                    _Writer.WriteObjectStart();
                    _Writer.WriteToken(_index.Tag(), 1);
					bool firstinner = true;
					_index.Serialize (_Writer, true, ref firstinner);
                    _Writer.WriteObjectEnd();
					}
				_Writer.WriteArrayEnd ();
				}

			if (Data != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Data", 1);
					_Writer.WriteBinary (Data);
				}
			if (Text != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Text", 1);
					_Writer.WriteString (Text);
				}
			if (__Pending){
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Pending", 1);
					_Writer.WriteBoolean (Pending);
				}
			if (__Commit){
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Commit", 1);
					_Writer.WriteBoolean (Commit);
				}
			if (__Rollback){
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Rollback", 1);
					_Writer.WriteBoolean (Rollback);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new DataItem From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new DataItem From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new DataItem ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new DataItem (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "DataItem" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new DataItem FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "DataItem" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new DataItem FromTagged (string _Input) {
			//DataItem _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new DataItem  FromTagged (JSONReader JSONReader) {
			DataItem Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "DataItem" : {
					Out = new DataItem ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "TransactionID" : {
					TransactionID = JSONReader.ReadString ();
					break;
					}
				case "PrimaryKey" : {
					PrimaryKey = JSONReader.ReadString ();
					break;
					}
				case "PriorTransactionID" : {
					PriorTransactionID = JSONReader.ReadString ();
					break;
					}
				case "Action" : {
					Action = JSONReader.ReadString ();
					break;
					}
				case "Added" : {
					Added = JSONReader.ReadDateTime ();
					break;
					}
				case "Keys" : {
					// Have a sequence of values
					bool _Going = JSONReader.StartArray ();
					Keys = new List <IndexTerm> ();
					while (_Going) {
						var _Item = IndexTerm.FromTagged (JSONReader); // a tagged structure
						Keys.Add (_Item);
						_Going = JSONReader.NextArray ();
						}
					break;
					}
				case "Data" : {
					Data = JSONReader.ReadBinary ();
					break;
					}
				case "Text" : {
					Text = JSONReader.ReadString ();
					break;
					}
				case "Pending" : {
					Pending = JSONReader.ReadBoolean ();
					break;
					}
				case "Commit" : {
					Commit = JSONReader.ReadBoolean ();
					break;
					}
				case "Rollback" : {
					Rollback = JSONReader.ReadBoolean ();
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Initial entry in a log file, specify the creation date, type of log,
	/// etc.
	/// </summary>
	public partial class Header : LogEntry {
        /// <summary>
        ///Log Type, usually 'Consolidated' 'Data' 'Index'
        /// </summary>

		public virtual string						Type  {get; set;}
        /// <summary>
        ///Content type identifier
        /// </summary>

		public virtual string						Content  {get; set;}
        /// <summary>
        ///Optional description of the log type
        /// </summary>

		public virtual string						Comment  {get; set;}
        /// <summary>
        ///Digest Algorithm
        /// </summary>

		public virtual string						Digest  {get; set;}
        /// <summary>
        ///Final value of last log
        /// </summary>

		public virtual byte[]						LastDigest  {get; set;}
        /// <summary>
        ///If populated, this is a delta log.
        /// </summary>

		public virtual Delta						Delta  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "Header";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Type != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Type", 1);
					_Writer.WriteString (Type);
				}
			if (Content != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Content", 1);
					_Writer.WriteString (Content);
				}
			if (Comment != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Comment", 1);
					_Writer.WriteString (Comment);
				}
			if (Digest != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Digest", 1);
					_Writer.WriteString (Digest);
				}
			if (LastDigest != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("LastDigest", 1);
					_Writer.WriteBinary (LastDigest);
				}
			if (Delta != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Delta", 1);
					Delta.Serialize (_Writer, false);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Header From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Header From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new Header ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new Header (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "Header" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Header FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "Header" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Header FromTagged (string _Input) {
			//Header _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new Header  FromTagged (JSONReader JSONReader) {
			Header Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "Header" : {
					Out = new Header ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Type" : {
					Type = JSONReader.ReadString ();
					break;
					}
				case "Content" : {
					Content = JSONReader.ReadString ();
					break;
					}
				case "Comment" : {
					Comment = JSONReader.ReadString ();
					break;
					}
				case "Digest" : {
					Digest = JSONReader.ReadString ();
					break;
					}
				case "LastDigest" : {
					LastDigest = JSONReader.ReadBinary ();
					break;
					}
				case "Delta" : {
					// An untagged structure
					Delta = new Delta ();
					Delta.Deserialize (JSONReader);
 
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Describe a log that records the changes made since a specific 
	/// checkpoint 
	/// </summary>
	public partial class Delta : LogEntry {
        /// <summary>
        ///Filename of master log this is a delta to
        /// </summary>

		public virtual string						Parent  {get; set;}
        /// <summary>
        ///Filename of previous delta log				 
        /// </summary>

		public virtual string						Previous  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "Delta";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Parent != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Parent", 1);
					_Writer.WriteString (Parent);
				}
			if (Previous != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Previous", 1);
					_Writer.WriteString (Previous);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Delta From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Delta From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new Delta ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new Delta (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "Delta" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Delta FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "Delta" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Delta FromTagged (string _Input) {
			//Delta _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new Delta  FromTagged (JSONReader JSONReader) {
			Delta Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "Delta" : {
					Out = new Delta ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Parent" : {
					Parent = JSONReader.ReadString ();
					break;
					}
				case "Previous" : {
					Previous = JSONReader.ReadString ();
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Record a collected index.
	/// </summary>
	public partial class IndexTerm : LogEntry {
        /// <summary>
        ///Key under which index term is listed
        /// </summary>

		public virtual string						Type  {get; set;}
        /// <summary>
        ///Data associated with key
        /// </summary>

		public virtual string						Term  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "IndexTerm";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Type != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Type", 1);
					_Writer.WriteString (Type);
				}
			if (Term != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Term", 1);
					_Writer.WriteString (Term);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexTerm From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexTerm From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new IndexTerm ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new IndexTerm (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "IndexTerm" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexTerm FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "IndexTerm" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexTerm FromTagged (string _Input) {
			//IndexTerm _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new IndexTerm  FromTagged (JSONReader JSONReader) {
			IndexTerm Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "IndexTerm" : {
					Out = new IndexTerm ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Type" : {
					Type = JSONReader.ReadString ();
					break;
					}
				case "Term" : {
					Term = JSONReader.ReadString ();
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Specify the digest value of a log up to but not including the 
	/// record with the fingerprint.
	/// </summary>
	public partial class Final : LogEntry {
        /// <summary>
        ///Digest of this log up to but not including this record
        /// </summary>

		public virtual byte[]						Digest  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "Final";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Digest != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Digest", 1);
					_Writer.WriteBinary (Digest);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Final From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Final From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new Final ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new Final (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "Final" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Final FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "Final" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Final FromTagged (string _Input) {
			//Final _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new Final  FromTagged (JSONReader JSONReader) {
			Final Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "Final" : {
					Out = new Final ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Digest" : {
					Digest = JSONReader.ReadBinary ();
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Final entry in a log, contains indexes that specify the location
	/// of the index records.
	/// </summary>
	public partial class Terminal : LogEntry {
        /// <summary>
        ///Location of index records for specified key(s)
        /// </summary>

		public virtual List<IndexIndex>				Indexes  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "Terminal";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Indexes != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Indexes", 1);
				_Writer.WriteArrayStart ();
				bool _firstarray = true;
				foreach (var _index in Indexes) {
					_Writer.WriteArraySeparator (ref _firstarray);
					// This is an untagged structure. Cannot inherit.
                    //_Writer.WriteObjectStart();
                    //_Writer.WriteToken(_index.Tag(), 1);
					bool firstinner = true;
					_index.Serialize (_Writer, true, ref firstinner);
                    //_Writer.WriteObjectEnd();
					}
				_Writer.WriteArrayEnd ();
				}

			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Terminal From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Terminal From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new Terminal ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new Terminal (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "Terminal" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Terminal FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "Terminal" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Terminal FromTagged (string _Input) {
			//Terminal _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new Terminal  FromTagged (JSONReader JSONReader) {
			Terminal Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "Terminal" : {
					Out = new Terminal ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Indexes" : {
					// Have a sequence of values
					bool _Going = JSONReader.StartArray ();
					Indexes = new List <IndexIndex> ();
					while (_Going) {
						// an untagged structure.
						var _Item = new  IndexIndex ();
						_Item.Deserialize (JSONReader);
						// var _Item = new IndexIndex (JSONReader);
						Indexes.Add (_Item);
						_Going = JSONReader.NextArray ();
						}
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// Record an index location in the current log file.
	/// </summary>
	public partial class IndexIndex : LogEntry {
        /// <summary>
        ///Key under which index term is listed
        /// </summary>

		public virtual string						Key  {get; set;}
		bool								__Offset = false;
		private int						_Offset;
        /// <summary>
        ///Location in file at which offset occurs
        /// </summary>

		public virtual int						Offset {
			get {return _Offset;}
			set {_Offset = value; __Offset = true; }
			}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "IndexIndex";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Key != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Key", 1);
					_Writer.WriteString (Key);
				}
			if (__Offset){
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Offset", 1);
					_Writer.WriteInteger32 (Offset);
				}
			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexIndex From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexIndex From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new IndexIndex ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new IndexIndex (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "IndexIndex" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexIndex FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "IndexIndex" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexIndex FromTagged (string _Input) {
			//IndexIndex _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new IndexIndex  FromTagged (JSONReader JSONReader) {
			IndexIndex Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "IndexIndex" : {
					Out = new IndexIndex ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Key" : {
					Key = JSONReader.ReadString ();
					break;
					}
				case "Offset" : {
					Offset = JSONReader.ReadInteger32 ();
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// An index record. These are written out to a log file after it is closed 
	/// to permit rapid access.
	/// </summary>
	public partial class Index : LogEntry {
        /// <summary>
        ///Key that is indexed
        /// </summary>

		public virtual string						Key  {get; set;}
        /// <summary>
        ///List of occurrences of the specified index term.	
        /// </summary>

		public virtual List<IndexEntry>				Entries  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "Index";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Key != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Key", 1);
					_Writer.WriteString (Key);
				}
			if (Entries != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Entries", 1);
				_Writer.WriteArrayStart ();
				bool _firstarray = true;
				foreach (var _index in Entries) {
					_Writer.WriteArraySeparator (ref _firstarray);
					// This is an untagged structure. Cannot inherit.
                    //_Writer.WriteObjectStart();
                    //_Writer.WriteToken(_index.Tag(), 1);
					bool firstinner = true;
					_index.Serialize (_Writer, true, ref firstinner);
                    //_Writer.WriteObjectEnd();
					}
				_Writer.WriteArrayEnd ();
				}

			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Index From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Index From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new Index ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new Index (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "Index" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new Index FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "Index" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new Index FromTagged (string _Input) {
			//Index _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new Index  FromTagged (JSONReader JSONReader) {
			Index Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "Index" : {
					Out = new Index ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Key" : {
					Key = JSONReader.ReadString ();
					break;
					}
				case "Entries" : {
					// Have a sequence of values
					bool _Going = JSONReader.StartArray ();
					Entries = new List <IndexEntry> ();
					while (_Going) {
						// an untagged structure.
						var _Item = new  IndexEntry ();
						_Item.Deserialize (JSONReader);
						// var _Item = new IndexEntry (JSONReader);
						Entries.Add (_Item);
						_Going = JSONReader.NextArray ();
						}
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	/// <summary>
	///
	/// An index entry.
	/// </summary>
	public partial class IndexEntry : LogEntry {
        /// <summary>
        ///Data associated with key
        /// </summary>

		public virtual string						Data  {get; set;}
        /// <summary>
        ///Location in file at which offset occurs
        /// </summary>

		public virtual List<int>				Offset  {get; set;}

        /// <summary>
        /// Tag identifying this class.
        /// </summary>
        /// <returns>The tag</returns>
		public override string Tag () {
			return "IndexEntry";
			}



        /// <summary>
        /// Serialize this object to the specified output stream.
        /// </summary>
        /// <param name="Writer">Output stream</param>
        /// <param name="wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="first">If true, item is the first entry in a list.</param>
		public override void Serialize (Writer Writer, bool wrap, ref bool first) {
			SerializeX (Writer, wrap, ref first);
			}

        /// <summary>
        /// Serialize this object to the specified output stream.
        /// Unlike the Serlialize() method, this method is not inherited from the
        /// parent class allowing a specific version of the method to be called.
        /// </summary>
        /// <param name="_Writer">Output stream</param>
        /// <param name="_wrap">If true, output is wrapped with object
        /// start and end sequences '{ ... }'.</param>
        /// <param name="_first">If true, item is the first entry in a list.</param>
		public new void SerializeX (Writer _Writer, bool _wrap, ref bool _first) {
			if (_wrap) {
				_Writer.WriteObjectStart ();
				}
			if (Data != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Data", 1);
					_Writer.WriteString (Data);
				}
			if (Offset != null) {
				_Writer.WriteObjectSeparator (ref _first);
				_Writer.WriteToken ("Offset", 1);
				_Writer.WriteArrayStart ();
				bool _firstarray = true;
				foreach (var _index in Offset) {
					_Writer.WriteArraySeparator (ref _firstarray);
					_Writer.WriteInteger32 (_index);
					}
				_Writer.WriteArrayEnd ();
				}

			if (_wrap) {
				_Writer.WriteObjectEnd ();
				}
			}



        /// <summary>
		/// Create a new instance from untagged byte input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexEntry From (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return From (_Input);
			}

        /// <summary>
		/// Create a new instance from untagged string input.
		/// i.e. {... data ... }
        /// </summary>	
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexEntry From (string _Input) {
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			var Result = new IndexEntry ();
			Result.Deserialize (JSONReader);
			return Result;
			// return new IndexEntry (JSONReader);
			}

        /// <summary>
		/// Create a new instance from tagged byte input.
		/// i.e. { "IndexEntry" : {... data ... } }
        /// </summary>	
        /// <param name="_Data">The input data.</param>
        /// <returns>The created object.</returns>				
		public static new IndexEntry FromTagged (byte[] _Data) {
			var _Input = System.Text.Encoding.UTF8.GetString(_Data);
			return FromTagged (_Input);
			}

        /// <summary>
        /// Create a new instance from tagged string input.
		/// i.e. { "IndexEntry" : {... data ... } }
        /// </summary>
        /// <param name="_Input">The input data.</param>
        /// <returns>The created object.</returns>		
		public static new IndexEntry FromTagged (string _Input) {
			//IndexEntry _Result;
			//Deserialize (_Input, out _Result);
			StringReader _Reader = new StringReader (_Input);
            JSONReader JSONReader = new JSONReader (_Reader);
			return FromTagged (JSONReader) ;
			}


        /// <summary>
        /// Deserialize a tagged stream
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <returns>The created object.</returns>		
        public static new IndexEntry  FromTagged (JSONReader JSONReader) {
			IndexEntry Out = null;

			JSONReader.StartObject ();
            if (JSONReader.EOR) {
                return null;
                }

			string token = JSONReader.ReadToken ();

			switch (token) {

				case "IndexEntry" : {
					Out = new IndexEntry ();
					Out.Deserialize (JSONReader);
					break;
					}

				default : {
                    break;
					}
				}
			JSONReader.EndObject ();

			return Out;
			}


        /// <summary>
        /// Having read a tag, process the corresponding value data.
        /// </summary>
        /// <param name="JSONReader">The input stream</param>
        /// <param name="Tag">The tag</param>
		public override void DeserializeToken (JSONReader JSONReader, string Tag) {
			
			switch (Tag) {
				case "Data" : {
					Data = JSONReader.ReadString ();
					break;
					}
				case "Offset" : {
					// Have a sequence of values
					bool _Going = JSONReader.StartArray ();
					Offset = new List <int> ();
					while (_Going) {
						int _Item = JSONReader.ReadInteger32 ();
						Offset.Add (_Item);
						_Going = JSONReader.NextArray ();
						}
					break;
					}
				default : {
					break;
					}
				}
			// check up that all the required elements are present
			}


		}

	}

