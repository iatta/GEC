using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class Person
    {
        public int UserCode { get; set; }

        public int CardData { get; set; }
        public string PName { get; set; }
        public string PCode { get; set; }
        public string Dept { get; set; }
        public string Job { get; set; }
        public string Password { get; set; }
        public DateTime? Expiry { get; set; }
        public int TimeGroup { get; set; }
        public int OpenTimes { get; set; }
        public int Identity { get; set; }
        public int CardType { get; set; }
        public int CardStatus { get; set; }
        public int EnterStatus { get; set; }
        public DateTime? RecordTime { get; set; }
        public bool IsFaceFeatureCode { get; set; }
        public string Holiday { get; set; }
        public int FingerprintFeatureCodeCout { get; set; }
    }
}
