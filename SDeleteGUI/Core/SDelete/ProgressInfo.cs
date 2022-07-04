using static uom.AppTools;
using System.Text;
using uom;
using System.Threading;
using System.Text.RegularExpressions;

namespace SDeleteGUI.Core.SDelete
{
	internal class ProgressInfo : EventArgs
	{

		//Pass 0 progress: 20 % (80.61 MB / s)
		private static readonly Regex rexProgressPattern
			= new(@"^Pass\s(?<PassCount>\d+)\sprogress\:\s(?<PercentProgress>\d+)\%\s\((?<SpeedValue>\d+\.\d\d)\s(?<SpeedUnits>\w+\/s)\)$");

		public readonly uint PassCount;
		public readonly uint ProgressPercent;
		public readonly float SpeedValue;
		public readonly string SpeedUnits;


		internal ProgressInfo(string raw) : base()
		{
			Debug.Write($"Building ProgressInfo from RAW '{raw}'... ");

			/*
			Pass 0 progress: 20% (80.61 MB/s)                                                   
			Pass 0 progress: 20% (80.61 MB/s)                                                   
			 */

			//Parse Progress Value
			var mx = rexProgressPattern.Match(raw);
			if (!mx.Success) throw new ArgumentOutOfRangeException($"Failed to parse progress value '{raw}'!");

			//var aGroups = mx.GetGroupNames();
			var rGroups = mx.Groups;
			PassCount = uint.Parse(rGroups["PassCount"].Value);
			ProgressPercent = uint.Parse(rGroups["PercentProgress"].Value);

			string speedString = rGroups["SpeedValue"].Value;                   //string speedString = "80.60";
			speedString = speedString.Replace(".", ",");
			SpeedValue = float.Parse(speedString, System.Globalization.NumberStyles.Float);
			SpeedUnits = rGroups["SpeedUnits"].Value ?? "";

			Debug.WriteLine(ToString());
		}

		public static bool IsMatch(string s) => rexProgressPattern.IsMatch(s);


		/// <summary>Pass 0 progress: 20% (80.61 MB/s)</summary>
		public override string ToString()
			=> $"Pass {PassCount} progress: {ProgressPercent}% ({SpeedValue} {SpeedUnits})";
	}


}
