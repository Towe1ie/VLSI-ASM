using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vlsiasm
{
    class Program
    {

        private static string orgToStrDec(int org)
        {
            string tmp = Convert.ToString(org, 16);
            StringBuilder sb = new StringBuilder();
            sb.Append('0', 8 - tmp.Length).Append(tmp);
            return sb.ToString();
        }

        static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    Console.WriteLine("Must have exactly one argument as input file name");
            //    return;
            //}
            Tokenizer t = null;
            InstructionRecorder ir = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                t = new Tokenizer(args[0]);
                ir = new InstructionRecorder(t);
                int org = 0;
                while (!ir.eof())
                {
                    string token = ir.nextToken();
                    InstructionDescriptor desc = new InstructionDescriptor();
                    switch (token)
                    {
                        case "org":
                            org = Convert.ToInt32(ir.nextToken(), 16);
                            break;
                        case "load":
                            desc.opcode = "000000";
                            break;
                        case "store":
                            desc.opcode = "000001";
                            break;
                        case "mov":
                            desc.opcode = "000100";
                            break;
                        case "movi":
                            desc.opcode = "000101";
                            break;
                        case "add":
                            desc.opcode = "001000";
                            break;
                        case "sub":
                            desc.opcode = "001001";
                            break;
                        case "addi":
                            desc.opcode = "001100";
                            break;
                        case "subi":
                            desc.opcode = "001101";
                            break;
                        case "and":
                            desc.opcode = "010000";
                            break;
                        case "or":
                            desc.opcode = "010001";
                            break;
                        case "xor":
                            desc.opcode = "010010";
                            break;
                        case "not":
                            desc.opcode = "010011";
                            break;
                        case "shl":
                            desc.opcode = "011000";
                            break;
                        case "shr":
                            desc.opcode = "011001";
                            break;
                        case "sar":
                            desc.opcode = "011010";
                            break;
                        case "rol":
                            desc.opcode = "011011";
                            break;
                        case "ror":
                            desc.opcode = "011100";
                            break;
                        case "jmp":
                            desc.opcode = "100000";
                            break;
                        case "jsr":
                            desc.opcode = "100001";
                            break;
                        case "rts":
                            desc.opcode = "100010";
                            break;
                        case "push":
                            desc.opcode = "100100";
                            break;
                        case "pop":
                            desc.opcode = "100101";
                            break;
                        case "beq":
                            desc.opcode = "101000";
                            break;
                        case "bnq":
                            desc.opcode = "101001";
                            break;
                        case "bgt":
                            desc.opcode = "101010";
                            break;
                        case "blt":
                            desc.opcode = "101011";
                            break;
                        case "bge":
                            desc.opcode = "101100";
                            break;
                        case "ble":
                            desc.opcode = "101101";
                            break;
                        case "halt":
                            desc.opcode = "111111";
                            break;
                    }
                    switch (token)
                    {
                        case "load":
                        case "addi":
                        case "subi":
                            desc.type = InstructionType.DSI;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "store":
                            desc.type = InstructionType.ISSI;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs2))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "beq":
                        case "bnq":
                        case "blt":
                        case "bgt":
                        case "ble":
                        case "bge":
                            desc.type = InstructionType.ISSI;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs2))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "and":
                        case "xor":
                        case "not":
                        case "or":
                        case "sub":
                        case "add":
                            desc.type = InstructionType.DSSR;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs2))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "shl":
                        case "shr":
                        case "sar":
                        case "rol":
                        case "ror":
                            desc.type = InstructionType.DRIR;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "mov":
                            desc.type = InstructionType.DSR;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "movi":
                            desc.type = InstructionType.DRI;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "jmp":
                        case "jsr":
                            desc.type = InstructionType.RSI;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            if (!Parser.TryParseHex(token = ir.nextToken(), ref desc.immediate))
                            {
                                Console.WriteLine("Can't parse as immediate hex value " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "rts":
                        case "halt":
                            desc.type = InstructionType.NONE;
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "pop":
                            desc.type = InstructionType.DR;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rd))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "push":
                            desc.type = InstructionType.RSR;
                            if (!Parser.TryParseRegister(token = ir.nextToken(), ref desc.rs1))
                            {
                                Console.WriteLine("Can't parse as register " + token + ". At line: " + ir.lastTokenLine());
                                break;
                            }
                            sb.Append(orgToStrDec(org)).Append(' ').Append(InstructionSynthetizer.Synthetize(desc));
                            break;
                        case "org":
                            sb.Append(orgToStrDec(org));
                            org--;
                            break;
                        default:
                            org--;
                            Console.WriteLine("Unexpected token " + token + ". At line: " + ir.lastTokenLine());
                            break;
                    }
                    org++;
                    sb.Append(" ; ").Append(ir.getRecorded());
                    if (!ir.eof())
                    {
                        sb.AppendLine();
                    }
                    ir.resetRecording();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("!Exception: " + ex.ToString());
                if (t != null)
                {
                    try
                    {
                        Console.WriteLine("Last line of tokenizer: " + ir.lastTokenLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("!!!Exception while trying to read last line of tokenizer: " + ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Tokenizer is null, can't recover last line.");
                }
            }

            String[] fileNameParts = args[0].Split('.', '\\');
            String outFileName = null;
            if (args.Length > 1)
            {
                outFileName = args[1];
            }
            else
            {
                outFileName = fileNameParts[fileNameParts.Length - 2] + "_out." + fileNameParts[fileNameParts.Length - 1];
            }
            Console.WriteLine("Writing output to: " + outFileName);
            File.WriteAllText(outFileName, sb.ToString());
            Console.WriteLine("Assembling complete");
        }
    }
}
