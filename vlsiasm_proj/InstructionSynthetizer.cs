using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vlsiasm {
    enum InstructionType {
        DSI,
        DSSR,
        DSR,
        DRI,
        RSI,
        DRIR,
        ISSI,
        DR,
        RSR,
        NONE

    }
    struct InstructionDescriptor {
        public InstructionType type;
        public int rd, rs1, rs2, immediate;
        public string opcode;
    }
    class InstructionSynthetizer {

        
        public static  string Synthetize(InstructionDescriptor desc) {
            StringBuilder sb=new StringBuilder();
            switch (desc.type) {
                case InstructionType.NONE:
                    sb.Append(desc.opcode).Append('0', 26);
                    break;
                case InstructionType.DR:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd, 5)).Append('0', 21);
                    break;
                case InstructionType.RSR:
                    sb.Append(desc.opcode).Append('0', 5).Append(intToBinary(desc.rs1, 5)).Append('0', 16);
                    break;
                case InstructionType.RSI:
                    sb.Append(desc.opcode).Append('0', 5).Append(intToBinary(desc.rs1, 5)).Append(intToBinary(desc.immediate, 16));
                    break;
                case InstructionType.DSI:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd, 5)).Append(intToBinary(desc.rs1, 5)).Append(intToBinary(desc.immediate, 16));
                    break;
                case InstructionType.DRI:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd, 5)).Append('0', 5).Append(intToBinary(desc.immediate, 16));
                    break;
                case InstructionType.DSR:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd, 5)).Append(intToBinary(desc.rs1, 5)).Append('0', 16);
                    break;
                case InstructionType.ISSI:
                    sb.Append(desc.opcode).Append(intToBinary(desc.immediate, 16).Substring(0,5)).Append(intToBinary(desc.rs1, 5)).Append(intToBinary(desc.rs2, 5)).Append(intToBinary(desc.immediate, 16).Substring(5, 11));
                    break;
                case InstructionType.DSSR:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd,5)).Append(intToBinary(desc.rs1,5)).Append(intToBinary(desc.rs2,5)).Append('0', 11);
                    break;
                case InstructionType.DRIR:
                    sb.Append(desc.opcode).Append(intToBinary(desc.rd, 5)).Append(intToBinary(0, 5)).Append(intToBinary(desc.immediate, 5)).Append('0', 11);
                    break;
            }
            return sb.ToString();
        }

        private static string intToBinary(int input) {
            return Convert.ToString(input, 2);
        }
        private static string intToBinary(int input, int charcnt) {
            string tmp=intToBinary(input);
            StringBuilder sb=new StringBuilder();
            if (tmp.Length>charcnt) {
                for(int i=0;tmp.Length-i+1>charcnt;i++)
                    if (tmp[i]!='1') {
                        throw new Exception("Immediate constant too big");
                    }
                sb.Append(tmp.Substring(tmp.Length-charcnt));
            } else {
                sb.Append('0', charcnt-tmp.Length).Append(tmp);
            }
            return sb.ToString();

        }
    }
}
