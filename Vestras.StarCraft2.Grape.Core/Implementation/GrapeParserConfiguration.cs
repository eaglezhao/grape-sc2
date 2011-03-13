using System;
using System.Diagnostics;

using bsn.GoldParser.Parser;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	internal struct GrapeParserConfiguration {
		private readonly bool continueOnError;
		private readonly GrapeErrorSink errorSink;
		private readonly bool outputErrors;

		public GrapeParserConfiguration(GrapeErrorSink errorSink, bool outputErrors, bool continueOnError): this() {
			Debug.Assert((errorSink != null) || (!outputErrors));
			this.errorSink = errorSink;
			this.outputErrors = outputErrors;
			this.continueOnError = continueOnError;
		}

		public bool ContinueOnError {
			get {
				return continueOnError;
			}
		}

		public string FileName {
			get;
			internal set;
		}

		public bool OutputErrors {
			get {
				return outputErrors;
			}
		}

		internal void AddError(GrapeEntity entity) {
			if (outputErrors) {
				string message;
				GrapeErrorEntity errorEntity = entity as GrapeErrorEntity;
				if (errorEntity != null) {
					message = string.Format("Syntax error: '{0}'", errorEntity.Error);
				} else {
					message = string.Format("Unexpected token: '{0}'", ((IToken)entity).Symbol.Name);
				}
				errorSink.AddError(new GrapeErrorSink.Error {
				                                            		Description = message,
				                                            		FileName = entity.FileName,
				                                            		Offset = entity.Offset,
				                                            		Length = entity.Length,
				                                            		StartLine = entity.StartLine,
				                                            		EndLine = entity.EndLine,
				                                            		StartColumn = entity.StartColumn,
				                                            		EndColumn = entity.EndColumn
				                                            });
			}
		}
	}
}
