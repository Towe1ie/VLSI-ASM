using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vlsiasm {
    class Tokenizer {
        string[] lines;
        int pos=0;

        string[] line;
        int pos2;
        public Tokenizer(string path) {
            lines=File.ReadAllLines(path);
            line=lines[pos].Split(' ', ',', '\t');
        }
        public bool eof() {
            return ((pos2==line.Length)&&(pos==lines.Length))||pos>lines.Length;
        }
        public string nextToken() {
            if (pos2<=line.Length-1) {
                lasttokline=pos+1;
                if(!line[pos2].Equals(""))
                    return line[pos2++];
                pos2++;
                return nextToken();
            }
            pos2=0;
            pos++;
            if (pos>=lines.Length) {
                line=new string[0];
                return "";
            }
            line=lines[pos].Split(' ', ',', '\t');
            return nextToken();
        }

        int lasttokline=0;
        public int lastTokenLine() {
            return lasttokline;
        }
    }
}
