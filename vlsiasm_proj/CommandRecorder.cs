using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vlsiasm
{
    class InstructionRecorder
    {
        Tokenizer t;
        public InstructionRecorder(Tokenizer t)
        {
            this.t = t;
        }
        public void resetRecording()
        {
            sb = new StringBuilder();
        }
        public string getRecorded()
        {
            string tmp = sb.ToString();
            return tmp;
        }
        StringBuilder sb = new StringBuilder();
        public bool eof()
        {
            return t.eof();
        }
        public string nextToken()
        {
            string tmp = t.nextToken();
            sb.Append(tmp).Append(' ');
            return tmp;
        }
        public int lastTokenLine()
        {
            return t.lastTokenLine();
        }
    }
}
