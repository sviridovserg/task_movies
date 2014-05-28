using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Movies.DataContracts
{
	public enum SortDirection {
		Asc,
		Desc
	}

	[DataContract]
	public class SortDescription
	{
		[DataMember]
		public string SortField { get; set; }
		[DataMember]
		public SortDirection SortDirection { get; set; }
	}
}