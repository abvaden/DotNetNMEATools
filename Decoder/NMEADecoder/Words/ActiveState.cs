using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEA_Tools.Decoder.Words
{
    public class ActiveState : Word
    {

        public enum ActiveStateEnum { DataActive, Void }

		public ActiveStateEnum CurrentState { get{ return _CurrentState; } }
		public string Value { get{return _Value;} }

		private ActiveStateEnum _CurrentState;
		private string _Value;

        public ActiveState (string value) : base("Active State")
        {
            if(value == "A")
            {
                _CurrentState = ActiveStateEnum.DataActive;
            }
            else if(value == "V")
            {
                _CurrentState = ActiveStateEnum.Void;
            }
            else
            {
                throw new Exceptions.WordFormatException("Invalid value for ActiveState only A and V are allowed " + value + " was given", null);
            }
            _Value = value;
        }
    }
}
