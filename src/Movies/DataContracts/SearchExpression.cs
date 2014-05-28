using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Movies.DataContracts
{
	[DataContract]
	public class SearchExpression
	{
		[DataMember]
		public string Field { get; set; }
		[DataMember]
		public string Expression { get; set; }
	}
}